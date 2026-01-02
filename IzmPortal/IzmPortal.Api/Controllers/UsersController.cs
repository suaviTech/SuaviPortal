using IzmPortal.Application.Abstractions.Services;
using IzmPortal.Application.DTOs.Users;
using IzmPortal.Domain.Enums;
using IzmPortal.Infrastructure.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace IzmPortal.Api.Controllers;

[ApiController]
[Route("api/users")]
[Authorize(Policy = "SuperAdminOnly")]
public class UsersController : ControllerBase
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IAuditService _auditService;

    private static readonly string[] AllowedRoles =
        { "SuperAdmin", "Manager", "User" };

    public UsersController(
        UserManager<ApplicationUser> userManager,
        IAuditService auditService)
    {
        _userManager = userManager;
        _auditService = auditService;
    }

    // --------------------
    // HELPERS
    // --------------------
    private bool IsSelf(string userId)
        => _userManager.GetUserId(User) == userId;

    // --------------------
    // GET USERS
    // --------------------
    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var users = _userManager.Users.ToList();
        var result = new List<UserAdminDto>();

        foreach (var user in users)
        {
            var roles = await _userManager.GetRolesAsync(user);

            result.Add(new UserAdminDto
            {
                Id = user.Id,
                UserName = user.UserName ?? string.Empty,
                Email = user.Email ?? string.Empty,
                IsActive = user.LockoutEnd == null,
                Roles = roles.ToList()
            });
        }

        return Ok(result);
    }

    // --------------------
    // DEACTIVATE USER
    // --------------------
    [HttpPost("{id}/deactivate")]
    public async Task<IActionResult> Deactivate(string id)
    {
        if (IsSelf(id))
            return BadRequest("Kendi hesabınızı pasif yapamazsınız.");

        var user = await _userManager.FindByIdAsync(id);
        if (user == null)
            return NotFound();

        user.LockoutEnabled = true;
        user.LockoutEnd = DateTimeOffset.MaxValue;

        await _userManager.UpdateAsync(user);

        await _auditService.LogAsync(
            AuditAction.Deactivate,
            AuditEntity.User,
            user.Id);

        return Ok("Kullanıcı pasif yapıldı.");
    }

    // --------------------
    // ACTIVATE USER
    // --------------------
    [HttpPost("{id}/activate")]
    public async Task<IActionResult> Activate(string id)
    {
        if (IsSelf(id))
            return BadRequest("Kendi hesabınızı aktifleştiremezsiniz.");

        var user = await _userManager.FindByIdAsync(id);
        if (user == null)
            return NotFound();

        user.LockoutEnd = null;

        await _userManager.UpdateAsync(user);

        await _auditService.LogAsync(
            AuditAction.Activate,
            AuditEntity.User,
            user.Id);

        return Ok("Kullanıcı aktifleştirildi.");
    }

    // --------------------
    // CHANGE ROLE
    // --------------------
    [HttpPost("change-role")]
    public async Task<IActionResult> ChangeRole([FromBody] ChangeUserRoleDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        if (IsSelf(dto.UserId))
            return BadRequest("Kendi rolünüzü değiştiremezsiniz.");

        if (!AllowedRoles.Contains(dto.Role))
            return BadRequest("Geçersiz rol.");

        var user = await _userManager.FindByIdAsync(dto.UserId);
        if (user == null)
            return NotFound();

        var currentRoles = await _userManager.GetRolesAsync(user);

        await _userManager.RemoveFromRolesAsync(user, currentRoles);
        await _userManager.AddToRoleAsync(user, dto.Role);

        await _auditService.LogAsync(
            AuditAction.ChangeRole,
            AuditEntity.User,
            user.Id);

        return Ok("Kullanıcı rolü güncellendi.");
    }

    // --------------------
    // RESET PASSWORD
    // --------------------
    [HttpPost("reset-password")]
    public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        if (IsSelf(dto.UserId))
            return BadRequest("Kendi şifrenizi buradan resetleyemezsiniz.");

        var user = await _userManager.FindByIdAsync(dto.UserId);
        if (user == null)
            return NotFound();

        var token = await _userManager.GeneratePasswordResetTokenAsync(user);
        var result = await _userManager.ResetPasswordAsync(
            user,
            token,
            dto.NewPassword);

        if (!result.Succeeded)
            return BadRequest(result.Errors);

        // 🔐 Zorunlu şifre değişimi
        user.ForcePasswordChange = true;
        await _userManager.UpdateAsync(user);

        await _auditService.LogAsync(
            AuditAction.ResetPassword,
            AuditEntity.User,
            user.Id);

        return Ok("Şifre sıfırlandı.");
    }
}
