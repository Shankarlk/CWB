using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CWB.App.Controllers
{
    public class ViewWoInfoController : Controller
    {
        private readonly ILogger<ViewWoInfoController> _logger;
        public ViewWoInfoController(ILogger<ViewWoInfoController> logger)
        {
            _logger = logger;
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}
