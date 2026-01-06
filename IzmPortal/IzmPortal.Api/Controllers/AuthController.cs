using IzmPortal.Application.DTOs.Auth;
using IzmPortal.Api.Security;
using IzmPortal.Infrastructure.Identity;
using IzmPortal.Infrastructure.Persistence;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace IzmPortal.Api.Controllers;

[ApiController]
[Route("api/auth")]
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

    // --------------------
    // LOGIN
    // --------------------
    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Email)
            || string.IsNullOrWhiteSpace(request.Password))
            return BadRequest("Email ve şifre zorunludur.");

        var user = await _userManager.FindByNameAsync(request.Email);

        // 🔍 Identity’de yoksa → PersonalDB’den oluştur
        if (user == null)
        {
            var person = await _personalDb.Tbl_Personal
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Username == request.Email);

            if (person == null)
                return Unauthorized("Kullanıcı bulunamadı.");

            user = new ApplicationUser
            {
                UserName = person.Username,
                Email = person.Username,
                TcNumber = person.TcNumber,
                ForcePasswordChange = true
            };

            var initialPassword = person.TcNumber[^4..];

            var createResult =
                await _userManager.CreateAsync(user, initialPassword);

            if (!createResult.Succeeded)
                return BadRequest(createResult.Errors);

            // varsayılan rol
            await _userManager.AddToRoleAsync(user, "Manager");
        }

        // 🔐 Şifre kontrolü
        if (!await _userManager.CheckPasswordAsync(user, request.Password))
            return Unauthorized("Hatalı şifre.");

        var roles = await _userManager.GetRolesAsync(user);

        var token = _jwt.GenerateToken(
            user,
            roles,
            user.ForcePasswordChange);

        return Ok(new LoginResponse
        {
            AccessToken = token,
            ForcePasswordChange = user.ForcePasswordChange
        });
    }

    // --------------------
    // CHANGE PASSWORD
    // --------------------
    [Authorize(Policy = "AdminAccess")]
    [HttpPost("change-password")]
    public async Task<IActionResult> ChangePassword(ChangePasswordRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.CurrentPassword)
            || string.IsNullOrWhiteSpace(request.NewPassword))
            return BadRequest("Şifre alanları zorunludur.");

        var userName = User.Identity!.Name!;
        var user = await _userManager.FindByNameAsync(userName);

        if (user == null)
            return Unauthorized();

        var result = await _userManager.ChangePasswordAsync(
            user,
            request.CurrentPassword,
            request.NewPassword);

        if (!result.Succeeded)
            return BadRequest(result.Errors);

        // 🔓 Force password change kaldır
        user.ForcePasswordChange = false;
        await _userManager.UpdateAsync(user);

        return Ok("Şifre başarıyla değiştirildi.");
    }
}
