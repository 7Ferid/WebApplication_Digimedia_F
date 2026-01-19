using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;


namespace WebApplication_Digimedia_F.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    
    }
}
