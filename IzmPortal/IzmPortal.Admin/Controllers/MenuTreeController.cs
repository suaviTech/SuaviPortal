using IzmPortal.Admin.ViewModels.MenuTree;
using IzmPortal.Application.DTOs.Menu;
using IzmPortal.Application.DTOs.MenuDocument;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IzmPortal.Admin.Controllers;

[Authorize]
public class MenuTreeController : BaseAdminController
{
    public MenuTreeController(IHttpClientFactory factory)
        : base(factory) { }

    public async Task<IActionResult> Index(CancellationToken ct)
    {
        var menus = await Api
            .GetFromJsonAsync<List<MenuDto>>("/api/menus", ct)
            ?? new();

        var result = new List<MenuTreeVm>();

        foreach (var menu in menus)
        {
            var menuVm = new MenuTreeVm
            {
                MenuId = menu.Id,
                MenuTitle = menu.Title
            };

            foreach (var sm in menu.SubMenus)
            {
                var docs = await Api
                    .GetFromJsonAsync<List<MenuDocumentDto>>(
                        $"/api/menudocuments/by-submenu/{sm.Id}", ct)
                    ?? new();

                menuVm.SubMenus.Add(new SubMenuNodeVm
                {
                    SubMenuId = sm.Id,
                    Title = sm.Title,
                    Documents = docs.Select(d => new DocumentNodeVm
                    {
                        Id = d.Id,
                        Title = d.Title,
                        FilePath = d.FilePath,
                        IsActive = d.IsActive
                    }).ToList()
                });
            }

            result.Add(menuVm);
        }

        return View(result);
    }
}

