using CWB.App.Models.ItemMaster;
using CWB.App.Services.Masters;
using CWB.CommonUtils.Common;
using CWB.Logging;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace CWB.App.Controllers
{
    public class MastersController : Controller
    {
        private readonly ILoggerManager _logger;

        private readonly IMastersServices _mastersService;
        public MastersController(ILoggerManager logger, IMastersServices mastersService)
        {
            _logger = logger;
            _mastersService = mastersService;
        }
        public IActionResult Index()
        {
            indexViewBag();
            return View();
        }

        public IActionResult MasterDetails()
        {
            return View();
        }
        public async Task<IActionResult> ManufPart(string Id)
        {
            await CompaniesViewBag();
            return View();
        }

        public IActionResult RawMaterial(string Id)
        {
            return View();
        }

        public IActionResult BOF(string Id)
        {
            ViewBag.BOFId = CWBAppUtils.DecodeString(Id);
            return View();
        }

        #region Private Function
        private void indexViewBag()
        {
            ViewBag.IndexId = CWBAppUtils.EncodeLong(0);
        }

        [HttpGet]
        public async Task<IActionResult> Companies()
        {
            var companies = await _mastersService.GetCompanies();

            return Json(companies);

        }

        [HttpGet]
        public async Task<IActionResult> ManufacturedPartNoDetailList(long ManufPartType, string companyName)
        {
            var mfpdList = await _mastersService.GetManufacturedPartNoDetailList(ManufPartType, companyName);
            return Json(mfpdList);
        }

        [HttpGet]
        public async Task<IActionResult> HelloWorld(long Id)
        {
            var mfpdList = await _mastersService.HelloWorld();
            return Json(mfpdList);
        }

        [HttpGet]
        public async Task<IActionResult> GetUOMs(long Id)
        {
            var mfpdList = await _mastersService.GetUOMs(Id);
            return Json(mfpdList);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ManufacturedPartNoDetail(ManufacturedPartNoDetailVM model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _mastersService.ManufacturedPartNoDetail(model);
            return Ok(result);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> MPMakeFrom(ManufacturedPartNoDetailVM model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _mastersService.MPMakeFrom(model);
            return Ok(result);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> MPBOM(ManufacturedPartNoDetailVM model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _mastersService.MPBOM(model);
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> MPMakeFromList(string Id)
        {
            var mpmakefromlist = await _mastersService.GetMPMakeFromListBypartNumber(Id);
            return Ok(mpmakefromlist);

        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RawMaterialDetail(RawMaterialDetailVM model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _mastersService.RawMaterialDetail(model);
            return Ok(result);
        }

        //Not used
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> BoughtOutFinishDetail(BoughtOutFinishDetailVM model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _mastersService.BoughtOutFinishDetail(model);
            return Ok(result);
        }
        #endregion

        #region Private Functions - ViewBag
        private async Task CompaniesViewBag()
        {
            var companyTypes = await _mastersService.GetCompanyTypes();
            ViewBag.CompanyTypes = companyTypes.Select(c => new SelectListItem { Text = c.CompanyType, Value = c.CompanyTypeValue }).ToList();
             var uoms = await _mastersService.GetUOMs(1);
             ViewBag.UOMs = uoms.Select(c => new SelectListItem { Text = c.Name, Value = c.Id.ToString() }).ToList();
        }
        #endregion
    }
}
