using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CWB.Constants.UserIdentity;
using Microsoft.AspNetCore.Authorization;

namespace CWB.App.Controllers
{
    [Authorize(Roles = Roles.ADMIN)]
    public class ViewInwInspInfoController : Controller
    {
        private readonly ILogger<ViewInwInspInfoController> _logger;
        public ViewInwInspInfoController(ILogger<ViewInwInspInfoController> logger)
        {
            _logger = logger;
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}
