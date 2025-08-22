using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CWB.Constants.UserIdentity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;

namespace CWB.App.Controllers
{
    [Authorize(Roles = Roles.ADMIN)]
    public class ViewNcWaitInfoController : Controller
    {
        private readonly ILogger<ViewNcWaitInfoController> _logger;
        public ViewNcWaitInfoController(ILogger<ViewNcWaitInfoController> logger)
        {
            _logger = logger;
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}
