using IzmPortal.Api.DTOs.Auth;
using IzmPortal.Api.Security;
using IzmPortal.Infrastructure.Identity;
using IzmPortal.Infrastructure.Persistence;
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
        // 1️⃣ Identity'de var mı?
        var user = await _userManager.FindByNameAsync(request.Username);

        if (user is null)
        {
            // 2️⃣ PersonalDB lookup
            var person = await _personalDb.Tbl_Personal
                .FirstOrDefaultAsync(x => x.Username == request.Username);

            if (person is null)
                return Unauthorized("Kullanıcı bulunamadı.");

            // 3️⃣ Identity user oluştur
            user = new ApplicationUser
            {
                UserName = person.Username,
                TcNumber = person.TcNumber,
                ForcePasswordChange = true
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

    [Authorize]
    [HttpPost("change-password")]
    public async Task<IActionResult> ChangePassword(ChangePasswordRequest request)
    {
        var username = User.FindFirstValue(ClaimTypes.NameIdentifier)
                       ?? User.Identity?.Name;

        if (string.IsNullOrWhiteSpace(username))
            return Unauthorized();

        var user = await _userManager.FindByNameAsync(username);
        if (user is null)
            return Unauthorized();

        // 🔒 PIN format kontrolü (0000–9999)
        if (!Regex.IsMatch(request.NewPassword, @"^\d{4}$"))
            return BadRequest("Yeni şifre 4 haneli ve sadece rakam olmalıdır.");

        // Eski PIN doğru mu?
        var check = await _userManager.CheckPasswordAsync(user, request.CurrentPassword);
        if (!check)
            return BadRequest("Mevcut şifre hatalı.");

        // Şifre değiştir
        var result = await _userManager.ChangePasswordAsync(
            user,
            request.CurrentPassword,
            request.NewPassword
        );

        if (!result.Succeeded)
            return BadRequest(result.Errors);

        // 🔓 Zorunluluk kalktı
        user.ForcePasswordChange = false;
        await _userManager.UpdateAsync(user);

        // 🔑 Yeni JWT
        var roles = await _userManager.GetRolesAsync(user);
        var token = _jwt.GenerateToken(user, roles);

        return Ok(new
        {
            message = "PIN başarıyla değiştirildi.",
            token,
            forcePasswordChange = false
        });
    }
}

