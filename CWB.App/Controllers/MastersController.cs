using CWB.App.Models.Contacts;
using CWB.App.Models.ItemMaster;
using CWB.App.Services.Masters;
using CWB.CommonUtils.Common;
using CWB.Logging;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
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

        public IActionResult EditPart(int partId, string partType)
        {
            if(partType.Equals("BOF"))
            {
                return RedirectToAction("EditBOF", new { partId = partId});
            }
            else if (partType.Equals("RawMaterial"))
            {
                return RedirectToAction("EditRawMaterial", new { partId = partId });
            }
            return RedirectToAction("EditManufPart", new { partId = partId });
        }

        //editmanufpart
        public async Task<IActionResult> EditManufPart(int partId)
        {
            ManufacturedPartNoDetailVM manuf = await _mastersService.GetManufPart(partId);
            await CustomerViewBag();
            await CompaniesViewBagForManuF(manuf);
            await StatusViewBagForManuf(manuf);
            return View(manuf);
        }

        public async Task<IActionResult> ManufPart(string Id)
        {
            
            await CustomerViewBag();
            await CompaniesViewBagForManuF();
            await StatusViewBagForManuf();
            return View(new ManufacturedPartNoDetailVM {MasterPartType="0", PartId=0, ManufacturedPartNoDetailId=0, ManufacturedPartType=1});
        }

        public async Task<IActionResult> EditBOF(int partId)
        {
            BoughtOutFinishDetailVM manuf = await _mastersService.GetBOFPart(partId);
            await CompaniesViewBagForBOF();
            await StatusViewBagForBOF();
            return View(manuf);
        }

        public async Task<IActionResult> EditRawMaterial(int partId)
        {
            RawMaterialDetailVM manuf = await _mastersService.GetRMPart(partId);
            await SupplierViewBag();
            await CompaniesViewBagForRawMaterial(manuf);
            await StatusViewBagForRawMaterial(manuf);
            await RawMaterialViewBags(manuf);
            return View(manuf);
        }

       

        public async Task<IActionResult> RawMaterial(string Id)
        {
            await SupplierViewBag();
            await CompaniesViewBagForRawMaterial();
            await StatusViewBagForManuf();
            await RawMaterialViewBags();
            return View(new RawMaterialDetailVM { RawMaterialDetailId=0, PartId=0});
        }

        public async Task<IActionResult> BOF(string Id)
        {
            await CompaniesViewBagForBOF();
            await StatusViewBagForBOF();
            ViewBag.BOFId = CWBAppUtils.DecodeString(Id);
            return View(new BoughtOutFinishDetailVM { BoughtOutFinishDetailId=0, PartId = 0});
        }

        #region Private Function
        private void indexViewBag()
        {
            ViewBag.IndexId = CWBAppUtils.EncodeLong(0);
        }

        public async Task<IActionResult> Customers()
        {
            var companies = await _mastersService.GetCompanies();
            return Json(companies.Where(m=>m.CompanyType.Equals("Both") || m.CompanyType.Equals("Customer")));
        }
        public async Task<IActionResult> Suppliers()
        {
            var companies = await _mastersService.GetCompanies();
            return Json(companies.Where(m => m.CompanyType.Equals("Both") || m.CompanyType.Equals("Supplier")));
        }

        [HttpGet]
        public async Task<IActionResult> Companies()
        {
            var companies = await _mastersService.GetCompanies();

            return Json(companies);

        }

        [HttpGet]
        public async Task<IActionResult> RMTypes()
        {
            var companies = await _mastersService.GetRMTypes();

            return Json(companies);

        }

        [HttpGet]
        public async Task<IActionResult> RMSpecs()
        {
            var companies = await _mastersService.GetRMSpecs();

            return Json(companies);

        }

        [HttpGet]
        public async Task<IActionResult> RMStandards()
        {
            var companies = await _mastersService.GetRMStandards();

            return Json(companies);

        }

        [HttpGet]
        public async Task<IActionResult> BaseRMs()
        {
            var companies = await _mastersService.GetBaseRMs();

            return Json(companies);

        }

        [HttpGet]
        public async Task<IActionResult> ManufacturedPartNoDetailList(long ManufPartType, string companyName)
        {
            var mfpdList = await _mastersService.GetManufacturedPartNoDetailList(ManufPartType, companyName);
            return Json(mfpdList);
        }

        /*[HttpGet]
        public async Task<IActionResult> HelloWorld(long Id)
        {
            var mfpdList = await _mastersService.HelloWorld();
            return Json(mfpdList);
        }*/

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
        public async Task<IActionResult> MPMakeFrom(MPMakeFromVM model)
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
        public async Task<IActionResult> MPBOM(MPBomVM model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _mastersService.MPBOM(model);
            return Ok(result);
        }

       

        [HttpGet]
        public async Task<IActionResult> GetMakeFrom(string Id)
        {
            var mpmakefromlist = await _mastersService.GetMakeFrom(Id);
            return Ok(mpmakefromlist);

        }
        [HttpGet]
        public async Task<IActionResult> GetBOM(string Id)
        {
            var mpmakefromlist = await _mastersService.GetBOM(Id);
            return Ok(mpmakefromlist);

        }


        [HttpGet]
        public async Task<IActionResult> BOFS()
        {
            var mpmakefromlist = await _mastersService.BOFS();
            return Ok(mpmakefromlist);

        }

        [HttpGet]
        public async Task<IActionResult> SupplierRMS(string supplierId)
        {
            var mfpdList = await _mastersService.SupplierRMS(supplierId);
            return Ok(mfpdList);
        }

        [HttpGet]
        public async Task<IActionResult> mpdlist()
        {
            var mfpdList = await _mastersService.GetAllManufacturedPartNoDetailList();
            return Json(mfpdList);
        }
        //GetAllManufacturedPartNoDetailList


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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PartPurchase(PartPurchaseDetailsVM model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _mastersService.PartPurchase(model);
            return Ok(result);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemPartPurchase(PartPurchaseDetailsVM model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _mastersService.RemPartPurchase(model);
            return Ok(result);
        }

       

        [HttpGet]
        public async Task<IActionResult> PartPurchases()
        {
            var mfpdList = await _mastersService.PartPurchases();
            return Json(mfpdList);
        }

        [HttpGet]
        public async Task<IActionResult> PartPurchasesFor(int partId)
        {
            var mfpdList = await _mastersService.PartPurchasesFor(partId);
            return Json(mfpdList);
        }

        //partpurchasesbypartNo

        [HttpGet]
        public async Task<IActionResult> GetPartPurchase(int  partPurchaseId)
        {
            var mfpdList = await _mastersService.GetPartPurchase(partPurchaseId);
            return Json(mfpdList);
        }


        [HttpGet]
        public async Task<IActionResult> MasterParts()
        {
            var mfpdList = await _mastersService.ItemMasterParts();
            return Json(mfpdList);
        }

        [HttpGet]
        public async Task<IActionResult> SelectParts()
        {
            var mfpdList = await _mastersService.SelectParts();
            return Json(mfpdList);
        }

        [HttpGet]
        public async Task<IActionResult> OwnRMS()
        {
            var mfpdList = await _mastersService.OwnRMS();
            return Ok(mfpdList);
        }

        


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> BaseRM(BaseRawMatrialVM model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _mastersService.BaseRM(model);
            return Ok(result);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RMType(RawMateriaTypeVM model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _mastersService.RMType(model);
            return Ok(result);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RMSpec(RawMaterialSpecVM model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _mastersService.RMSpec(model);
            return Ok(result);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RMStandard(RawMaterialStandardVM model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _mastersService.RMStandard(model);
            return Ok(result);
        }


        [HttpGet]
        public async Task<IActionResult> BOMS(string partNo)
        {
            var mpmakefromlist = await _mastersService.BOMS(partNo);
            return Ok(mpmakefromlist);

        }
        [HttpGet]
        public async Task<IActionResult> MPMakeFromList(string partNo)
        {
            var mpmakefromlist = await _mastersService.GetMPMakeFromListBypartNumber(partNo);
            return Ok(mpmakefromlist);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemMakeFrom(MPMakeFromVM model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _mastersService.RemMakeFrom(model);
            return Ok(result);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemBOM(MPBomVM model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _mastersService.RemBOM(model);
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

        [HttpGet]
        public async Task<JsonResult> CheckPartNo(string partNo,string partNumber="")
        {
            if(partNumber.Length>0)
            {
                partNo = partNumber;
            }
            var result = await _mastersService.CheckPartNo(partNo);
            return Json(!result);
        }

        #region Private Functions - ViewBag
        private async Task SupplierViewBag()
        {
            var companyTypes = await _mastersService.GetCompanyTypes();
            List<CompanyTypeVM> types = new List<CompanyTypeVM>();
            foreach (var co in companyTypes)
            {
                if (co.CompanyTypeValue.Equals("Supplier"))
                    continue;
                if (co.CompanyType.Equals("Supplier"))
                    continue;
                CompanyTypeVM companyTypeVM = new CompanyTypeVM();
                companyTypeVM.CompanyType = co.CompanyType;
                companyTypeVM.CompanyTypeValue = co.CompanyTypeValue;
                types.Add(companyTypeVM);
            }
            ViewBag.CompanyTypes = types.Select(c => new SelectListItem { Text = c.CompanyType, Value = c.CompanyTypeValue }).ToList();
        }
        private async Task CustomerViewBag()
        {
            var companyTypes = await _mastersService.GetCompanyTypes();
            List<CompanyTypeVM> types = new List<CompanyTypeVM>();
            foreach (var co in companyTypes)
            {
                if (co.CompanyTypeValue.Equals("Customer"))
                    continue;
                if (co.CompanyType.Equals("Customer"))
                    continue;
                CompanyTypeVM companyTypeVM = new CompanyTypeVM();
                companyTypeVM.CompanyType = co.CompanyType;
                companyTypeVM.CompanyTypeValue = co.CompanyTypeValue;
                types.Add(companyTypeVM);
            }
            ViewBag.CompanyTypes = types.Select(c => new SelectListItem { Text = c.CompanyType, Value = c.CompanyTypeValue }).ToList();
        }

        private async Task CompaniesViewBagForManuF(ManufacturedPartNoDetailVM manuf=null)
        {
            var uoms = await _mastersService.GetUOMs(1);
            var companies = await _mastersService.GetCompanies();
            companies = companies.Where(m => m.CompanyType.Equals("Both") || m.CompanyType.Equals("Customer"));
            if (manuf != null)
            {
                ViewBag.Companies = companies.Select(c => new SelectListItem { Text = c.CompanyName, Value = c.CompanyId.ToString(), Selected = (c.CompanyId == manuf.CompanyId) }).ToList();
                ViewBag.UOMs = uoms.Select(c => new SelectListItem { Text = c.Name, Value = c.UOMId.ToString(),Selected=(c.UOMId == manuf.UOMId) }).ToList();
            }
            else
            {
                ViewBag.Companies = companies.Select(c => new SelectListItem { Text = c.CompanyName, Value = c.CompanyId.ToString() }).ToList();
                ViewBag.UOMs = uoms.Select(c => new SelectListItem { Text = c.Name, Value = c.UOMId.ToString() }).ToList();
            }
        }

        private async Task CompaniesViewBagForBOF(BoughtOutFinishDetailVM manuf = null)
        {
            var companies = await _mastersService.GetCompanies();
            companies = companies.Where(m => m.CompanyType.Equals("Both") || m.CompanyType.Equals("Supplier"));
            if (manuf != null)
            {
                ViewBag.Companies = companies.Select(c => new SelectListItem { Text = c.CompanyName, Value = c.CompanyId.ToString() }).ToList();
            }
            else
            {
                ViewBag.Companies = companies.Select(c => new SelectListItem { Text = c.CompanyName, Value = c.CompanyId.ToString() }).ToList();
            }
        }


        private async Task CompaniesViewBagForRawMaterial(RawMaterialDetailVM manuf = null)
        {
            var companies = await _mastersService.GetCompanies();
            companies = companies.Where(m => m.CompanyType.Equals("Both") || m.CompanyType.Equals("Supplier"));
            if (manuf != null)
            {
                ViewBag.Suppliers = companies.Select(c => new SelectListItem { Text = c.CompanyName, Value = c.CompanyId.ToString(), Selected = (c.CompanyId == manuf.SupplierId) }).ToList();
            }
            else
            {
                ViewBag.Suppliers = companies.Select(c => new SelectListItem { Text = c.CompanyName, Value = c.CompanyId.ToString() }).ToList();
            }
        }

        private async Task StatusViewBagForManuf(ManufacturedPartNoDetailVM manuf = null)
        {
            var statuses = await _mastersService.GetStatuses();
            List<PartStatusVM> types = new List<PartStatusVM>();
            foreach (var co in statuses)
            {
                PartStatusVM partStatusVM = new PartStatusVM();
                partStatusVM.Status = co.Status;
                partStatusVM.StatusValue = co.StatusValue;
                types.Add(partStatusVM);
            }
            if (manuf != null)
            {
                ViewBag.Statuses = types.Select(c => new SelectListItem { Text = c.Status, Value = c.StatusValue,Selected=(c.Status.Equals(manuf.Status)) }).ToList();
            }
            else
            {
                ViewBag.Statuses = types.Select(c => new SelectListItem { Text = c.Status, Value = c.StatusValue }).ToList();
            }
            
        }
        private async Task StatusViewBagForBOF(BoughtOutFinishDetailVM manuf = null)
        {
            var statuses = await _mastersService.GetStatuses();
            List<PartStatusVM> types = new List<PartStatusVM>();
            foreach (var co in statuses)
            {
                PartStatusVM partStatusVM = new PartStatusVM();
                partStatusVM.Status = co.Status;
                partStatusVM.StatusValue = co.StatusValue;
                types.Add(partStatusVM);
            }
            if (manuf != null)
            {
                ViewBag.Statuses = types.Select(c => new SelectListItem { Text = c.Status, Value = c.StatusValue, Selected = (c.Status.Equals(manuf.Status)) }).ToList();
            }
            else
            {
                ViewBag.Statuses = types.Select(c => new SelectListItem { Text = c.Status, Value = c.StatusValue }).ToList();
            }

        }
        private async Task StatusViewBagForRawMaterial(RawMaterialDetailVM manuf = null)
        {
            var statuses = await _mastersService.GetStatuses();
            List<PartStatusVM> types = new List<PartStatusVM>();
            foreach (var co in statuses)
            {
                PartStatusVM partStatusVM = new PartStatusVM();
                partStatusVM.Status = co.Status;
                partStatusVM.StatusValue = co.StatusValue;
                types.Add(partStatusVM);
            }
            if (manuf != null)
            {
                ViewBag.Statuses = types.Select(c => new SelectListItem { Text = c.Status, Value = c.StatusValue, Selected = (c.Status.Equals(manuf.Status)) }).ToList();
            }
            else
            {
                ViewBag.Statuses = types.Select(c => new SelectListItem { Text = c.Status, Value = c.StatusValue }).ToList();
            }

        }

        private async Task RawMaterialViewBags(RawMaterialDetailVM rmVm=null)
        {
            var rmTypes = await _mastersService.GetRMTypes();
            var standards = await _mastersService.GetRMStandards();
            var specs = await _mastersService.GetRMSpecs();
            var baseRms = await _mastersService.GetBaseRMs();
            if (rmVm != null)
            {
                ViewBag.RMTypes = rmTypes.Select(c => new SelectListItem { Text = c.Name, Value = c.RawMaterialTypeId.ToString(), Selected = (c.RawMaterialTypeId == rmVm.RawMaterialTypeId) }).ToList();
                ViewBag.Standards = standards.Select(c => new SelectListItem { Text = c.Name, Value = c.Standard.ToString(), Selected = (c.Standard == rmVm.Standard) }).ToList();
                ViewBag.Specs = specs.Select(c => new SelectListItem { Text = c.Name, Value = c.MaterialSpecId.ToString(), Selected = (c.MaterialSpecId == rmVm.MaterialSpecId) }).ToList();
                ViewBag.BaseRMs = baseRms.Select(c => new SelectListItem { Text = c.Name, Value = c.BaseRawMaterialId.ToString(), Selected = (c.BaseRawMaterialId == rmVm.BaseRawMaterialId) }).ToList();
            }
            else
            {
                ViewBag.RMTypes = rmTypes.Select(c => new SelectListItem { Text = c.Name, Value = c.RawMaterialTypeId.ToString() }).ToList();
                ViewBag.Standards = standards.Select(c => new SelectListItem { Text = c.Name, Value = c.Standard.ToString() }).ToList();
                ViewBag.Specs = specs.Select(c => new SelectListItem { Text = c.Name, Value = c.MaterialSpecId.ToString() }).ToList();
                ViewBag.BaseRMs = baseRms.Select(c => new SelectListItem { Text = c.Name, Value = c.BaseRawMaterialId.ToString() }).ToList();
            }
        }
        #endregion
    }


}
