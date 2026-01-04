using IzmPortal.Application.Abstractions.Services;
using IzmPortal.Application.DTOs.Users;
using IzmPortal.Domain.Enums;
using IzmPortal.Infrastructure.Identity;
using IzmPortal.Infrastructure.Persistence;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace IzmPortal.Api.Controllers;

[ApiController]
[Route("api/users")]
[Authorize(Policy = "SuperAdminOnly")]
public class UsersController : ControllerBase
{
    private static readonly string[] AllowedRoles =
        { "SuperAdmin", "Manager" };

    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IAuditService _auditService;
    private readonly PersonalDbContext _personalDb;

    public UsersController(
        UserManager<ApplicationUser> userManager,
        IAuditService auditService,
        PersonalDbContext personalDb)
    {
        _userManager = userManager;
        _auditService = auditService;
        _personalDb = personalDb;
    }

    // --------------------------------------------------
    // GET USERS (LIST)
    // --------------------------------------------------
    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var users = await _userManager.Users.ToListAsync();
        var result = new List<UserListDto>();

        foreach (var user in users)
        {
            var roles = await _userManager.GetRolesAsync(user);

            result.Add(new UserListDto
            {
                Id = user.Id,
                Email = user.Email ?? "",
                IsActive = user.LockoutEnd == null,
                Roles = roles.ToList()
            });
        }

        return Ok(result);
    }

    // --------------------------------------------------
    // LOOKUP USER (IDENTITY → PERSONAL)
    // --------------------------------------------------
    [HttpGet("lookup")]
    public async Task<IActionResult> Lookup([FromQuery] string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            return BadRequest("Email boş olamaz.");

        // 1️⃣ Identity kontrol
        var identityUser = await _userManager.FindByEmailAsync(email);
        if (identityUser != null)
        {
            var roles = await _userManager.GetRolesAsync(identityUser);

            var personal = await _personalDb.Tbl_Personal
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Username == email);

            return Ok(new UserLookupResultDto
            {
                ExistsInIdentity = true,
                ExistsInPersonal = personal != null,
                Identity = new IdentityUserInfoDto
                {
                    Id = identityUser.Id,
                    Email = identityUser.Email!,
                    IsActive = identityUser.LockoutEnd == null,
                    ForcePasswordChange = identityUser.ForcePasswordChange,
                    Roles = roles.ToList()
                },
                Personal = personal == null ? null : new PersonalUserInfoDto
                {
                    Card = personal.Card,
                    Departman = personal.Departman,
                    Phone = personal.Phone,
                    TcMasked = MaskTc(personal.TcNumber)
                }
            });
        }

        // 2️⃣ Identity yok → PersonalDB
        var personOnly = await _personalDb.Tbl_Personal
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Username == email);

        if (personOnly == null)
        {
            return Ok(new UserLookupResultDto
            {
                ExistsInIdentity = false,
                ExistsInPersonal = false
            });
        }

        return Ok(new UserLookupResultDto
        {
            ExistsInIdentity = false,
            ExistsInPersonal = true,
            Personal = new PersonalUserInfoDto
            {
                Card = personOnly.Card,
                Departman = personOnly.Departman,
                Phone = personOnly.Phone,
                TcMasked = MaskTc(personOnly.TcNumber)
            }
        });
    }

    // --------------------------------------------------
    // AUTHORIZE FROM PERSONAL
    // --------------------------------------------------
    [HttpPost("authorize-from-personal")]
    public async Task<IActionResult> AuthorizeFromPersonal(
        [FromBody] AuthorizeFromPersonalDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        if (!AllowedRoles.Contains(dto.Role))
            return BadRequest("Geçersiz rol.");

        var existing = await _userManager.FindByEmailAsync(dto.Email);
        if (existing != null)
            return BadRequest("Kullanıcı zaten yetkili.");

        var personal = await _personalDb.Tbl_Personal
            .FirstOrDefaultAsync(x => x.Username == dto.Email);

        if (personal == null)
            return NotFound("Personel bulunamadı.");

        var password = personal.TcNumber[^4..];

        var user = new ApplicationUser
        {
            UserName = dto.Email,
            Email = dto.Email,
            PhoneNumber = personal.Phone,
            ForcePasswordChange = true
        };

        var createResult = await _userManager.CreateAsync(user, password);
        if (!createResult.Succeeded)
            return BadRequest(createResult.Errors);

        await _userManager.AddToRoleAsync(user, dto.Role);

        await _auditService.LogAsync(
            AuditAction.Create,
            AuditEntity.User,
            user.Id);

        return Ok("Kullanıcı yetkilendirildi.");
    }

    // --------------------------------------------------
    // HELPERS
    // --------------------------------------------------
    private static string MaskTc(string tc)
    {
        if (string.IsNullOrWhiteSpace(tc) || tc.Length < 4)
            return "****";

        return new string('*', tc.Length - 4) + tc[^4..];
    }
}
