using IzmPortal.Application.DTOs.MenuHierarchy;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Json;

namespace IzmPortal.Admin.Controllers;

[Authorize(Roles = "SuperAdmin,Manager")]
public class MenuHierarchyController : Controller
{
    private readonly HttpClient _client;

    public MenuHierarchyController(IHttpClientFactory factory)
    {
        _client = factory.CreateClient("ApiClient");
    }

    public async Task<IActionResult> Index()
    {
        var response = await _client.GetAsync("api/menu-hierarchy");

        if (!response.IsSuccessStatusCode)
        {
            TempData["Error"] = "Menü hiyerarşisi alınamadı.";
            return View(new List<MenuHierarchyDto>());
        }

        var data = await response.Content
            .ReadFromJsonAsync<List<MenuHierarchyDto>>();

        return View(data ?? new List<MenuHierarchyDto>());
    }
}
