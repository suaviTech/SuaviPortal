using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IzmPortal.Admin.Controllers;

[Authorize] // 👈 login olmayan giremez
public class HomeController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}
