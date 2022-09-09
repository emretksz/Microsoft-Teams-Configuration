using Microsoft.AspNetCore.Mvc;

namespace MicrosoftTeams_Configuration_ASPCORE.Controllers
{
    public class HomeController : Controller
    {
        /// Action and  view model.....
        public IActionResult Index()
        {
            return View();
        }
    }
}
