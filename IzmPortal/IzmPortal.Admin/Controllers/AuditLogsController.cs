using IzmPortal.Admin.Extensions;
using IzmPortal.Admin.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;

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
        var query = new Dictionary<string, string?>();


        if (from.HasValue) query["from"] = from.Value.ToString("yyyy-MM-dd");
        if (to.HasValue) query["to"] = to.Value.ToString("yyyy-MM-dd");

        var url = QueryHelpers.AddQueryString("/api/audit-logs", query);

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
