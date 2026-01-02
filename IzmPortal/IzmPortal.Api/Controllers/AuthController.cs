using IzmPortal.Api.DTOs.Auth;
using IzmPortal.Api.Security;
using IzmPortal.Infrastructure.Identity;
using IzmPortal.Infrastructure.Persistence;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Text.RegularExpressions;

namespace IzmPortal.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly PersonalDbContext _personalDb;
    private readonly JwtTokenGenerator _jwt;

    public AuthController(
        UserManager<ApplicationUser> userManager,
        PersonalDbContext personalDb,
        JwtTokenGenerator jwt)
    {
        _userManager = userManager;
        _personalDb = personalDb;
        _jwt = jwt;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginRequest request)
    {
        // 1️⃣ Identity’de var mı?
        var user = await _userManager.FindByNameAsync(request.Email);

        if (user is null)
        {
            // 2️⃣ PersonalDB lookup
            var person = await _personalDb.Tbl_Personal
                .FirstOrDefaultAsync(x => x.Username == request.Email);

            if (person is null)
                return Unauthorized("Kullanıcı bulunamadı.");

            // 3️⃣ Identity user oluştur
            user = new ApplicationUser
            {
                UserName = person.Username,
                Email = person.Username,
                TcNumber = person.TcNumber,
                ForcePasswordChange = true,
                EmailConfirmed = true
            };

            var initialPassword = person.TcNumber[^4..];

            var createResult = await _userManager.CreateAsync(user, initialPassword);
            if (!createResult.Succeeded)
                return BadRequest(createResult.Errors);

            await _userManager.AddToRoleAsync(user, "Manager");
        }

        // 4️⃣ Şifre kontrolü
        if (!await _userManager.CheckPasswordAsync(user, request.Password))
            return Unauthorized("Hatalı şifre.");

        // 5️⃣ JWT üret
        var roles = await _userManager.GetRolesAsync(user);
        var token = _jwt.GenerateToken(user, roles);

        return Ok(new LoginResponse
        {
            Token = token,
            ForcePasswordChange = user.ForcePasswordChange
        });
    }




    [Authorize(Policy = "AdminAccess")]
    [HttpPost("change-password")]
    public async Task<IActionResult> ChangePassword(ChangePasswordRequest request)
    {
        var userName = User.Identity!.Name!;
        var user = await _userManager.FindByNameAsync(userName);

        if (user == null)
            return Unauthorized();

        var result = await _userManager.ChangePasswordAsync(
            user,
            request.CurrentPassword,
            request.NewPassword
        );

        if (!result.Succeeded)
            return BadRequest(result.Errors);

        user.ForcePasswordChange = false;
        await _userManager.UpdateAsync(user);

        return Ok("Şifre başarıyla değiştirildi.");
    }





}

