using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CWB.App.Controllers
{
    public class ViewSoInfoController : Controller
    {
        private readonly ILogger<ViewSoInfoController> _logger;
        public ViewSoInfoController(ILogger<ViewSoInfoController> logger)
        {
            _logger = logger;
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}
