using IzmPortal.Admin.Extensions;
using IzmPortal.Admin.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Json;

namespace IzmPortal.Admin.Controllers;

[Authorize]
public class HomeController : BaseAdminController
{
    public HomeController(IHttpClientFactory factory)
        : base(factory)
    {
    }

    public async Task<IActionResult> Index()
    {
        // Basit özet veriler (ileride API genişletilebilir)
        var dashboard = new DashboardVm();

        // Son 5 audit log
        var auditResponse = await Api.GetAsync("/api/audit-logs?take=5");
        if (auditResponse.IsSuccessStatusCode)
        {
            dashboard.RecentAudits =
                await auditResponse.ReadContentAsync<List<AuditLogVm>>() ?? new();
        }

        return View(dashboard);
    }
}
