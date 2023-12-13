using CWB.Constants.UserIdentity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace CWB.App.Controllers
{
    [Authorize(Roles = Roles.ADMIN)]
    public class RoutingsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult AddNewRouting()
        {
            return Ok("Hello!");
        }

        public IActionResult RoutingDetails()
        {
            return View();
        }
    }
}
