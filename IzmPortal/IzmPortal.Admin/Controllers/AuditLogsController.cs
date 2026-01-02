using IzmPortal.Admin.Extensions;
using IzmPortal.Admin.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IzmPortal.Admin.Controllers;

[Authorize(Roles = "SuperAdmin")]
public class AuditLogsController : BaseAdminController
{
    public AuditLogsController(IHttpClientFactory factory)
        : base(factory)
    {
    }

    public async Task<IActionResult> Index(DateTime? from, DateTime? to)
    {
        var query = new List<string>();
        if (from.HasValue) query.Add($"from={from:yyyy-MM-dd}");
        if (to.HasValue) query.Add($"to={to:yyyy-MM-dd}");

        var url = "/api/audit-logs";
        if (query.Any())
            url += "?" + string.Join("&", query);

        var response = await Api.GetAsync(url);

        var failure = await HandleApiFailureAsync(
            response,
            "Audit kayıtları alınamadı.");

        if (failure != null)
            return failure;

        var items = await response
            .ReadContentAsync<List<AuditLogVm>>() ?? new();

        ViewBag.From = from?.ToString("yyyy-MM-dd");
        ViewBag.To = to?.ToString("yyyy-MM-dd");

        return View(items);
    }
}
