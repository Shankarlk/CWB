﻿using CWB.App.Models.Contacts;
using CWB.App.Models.ItemMaster;
using CWB.App.Services.Masters;
using CWB.CommonUtils.Common;
using CWB.Logging;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using static System.Net.Mime.MediaTypeNames;

namespace CWB.App.Controllers
{
    public class MastersController : Controller
    {
        private readonly ILoggerManager _logger;
        private readonly IMastersServices _mastersService;
        private readonly IMachineService _machineService;
        IHostingEnvironment _hostingEnvironment = null;
        public MastersController(ILoggerManager logger, IMachineService machineService, IMastersServices mastersService,
            IHostingEnvironment hostingEnvironment)
        {
            _logger = logger;
            _mastersService = mastersService;
            _machineService = machineService;
            _hostingEnvironment = hostingEnvironment;
        }
        public IActionResult Index()
        {
            indexViewBag();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> UploadFileToFtp(IFormFile uploadedFile)//, [FromServices] IHostingEnvironment hostingEnvironment
        {
            string ftpUrl = "/filestorage/Active/";
            string ftpFullPath = ftpUrl + uploadedFile.FileName;

            // Write the byte array to the file
            using (var stream = new FileStream(ftpFullPath, FileMode.Create))
            {
                await uploadedFile.CopyToAsync(stream);
                //string fileName = $"{hostingEnvironment.WebRootPath}\\files\\{uploadedFile.FileName}";
                //using (FileStream fileStream = System.IO.File.Create(fileName))
                //{
                //    uploadedFile.CopyTo(fileStream);
                //    fileStream.Flush();
                //}
            }
            return Ok("Uploaded Sucess");
        }

        [HttpGet]
        public async Task<IActionResult> ViewFile(string fileName)
        {
            // Check if the file name is null or empty
            if (string.IsNullOrEmpty(fileName))
            {
                return BadRequest("File name is required");
            }

            // Get the file path
            var filePath = Path.Combine("/filestorage/Active", fileName);

            // Check if the file exists
            if (!System.IO.File.Exists(filePath))
            {
                return NotFound("File not found");
            }

            // Read the file contents
            var fileBytes = await System.IO.File.ReadAllBytesAsync(filePath);
            var stream = new FileStream(filePath, FileMode.Open);
            // Return the file contents as a response
            return File(fileBytes, GetContentType(filePath), fileName);
            //return File(stream, "application/octet-stream", fileName);
        }

        // Helper method to get the content type of a file
        private string GetContentType(string filePath)
        {
            var fileExtension = Path.GetExtension(filePath);
            var contentType = "application/octet-stream";

            switch (fileExtension.ToLower())
            {
                case ".txt":
                    contentType = "text/plain";
                    break;
                case ".pdf":
                    contentType = "application/pdf";
                    break;
                case ".jpg":
                case ".jpeg":
                    contentType = "image/jpeg";
                    break;
                case ".png":
                    contentType = "image/png";
                    break;
                case ".gif":
                    contentType = "image/gif";
                    break;
                case ".bmp":
                    contentType = "image/bmp";
                    break;
                case ".doc":
                case ".docx":
                    contentType = "application/msword";
                    break;
                case ".xls":
                case ".xlsx":
                    contentType = "application/vnd.ms-excel";
                    break;
                case ".ppt":
                case ".pptx":
                    contentType = "application/vnd.ms-powerpoint";
                    break;
            }

            return contentType;
        }

        [HttpGet]
        public async Task<JsonResult> CheckPartNoInDocList(long partId)
        {
            var result = await _mastersService.CheckPartNoInDocList(partId);
            return Json(result);
        }

        public async Task<IActionResult> MasterDetails()
        {
            await StatusViewBagForManuf();
            return View();
        }

        public IActionResult EditPart(long partId, string partType)
        {
            string encodepartid = CWBAppUtils.EncodeLong(partId);

            if (partType.Equals("BOF"))
            {
                return RedirectToAction("EditBOF", new { partId = encodepartid });
            }
            else if (partType.Equals("RawMaterial"))
            {
                return RedirectToAction("EditRawMaterial", new { partId = encodepartid });
            }
            return RedirectToAction("EditManufPart", new { partId = encodepartid });
        }

        //editmanufpart
        public async Task<IActionResult> EditManufPart(string partId)
        {
            int decodedManufPartId = (int)CWBAppUtils.DecodeString(partId.ToString());
            ManufacturedPartNoDetailVM manuf = await _mastersService.GetManufPart(decodedManufPartId);
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

        public async Task<IActionResult> EditBOF(string partId)
        {
            int decodedPartId = (int)CWBAppUtils.DecodeString(partId);
            BoughtOutFinishDetailVM manuf = await _mastersService.GetBOFPart(decodedPartId);
            await CompaniesViewBagForBOF();
            await SupplierViewBag();
            await StatusViewBagForBOF();
            return View(manuf);
        }

        [HttpPost]
        public long DecodePartId(string partId)
        {
            long decodepartID = 0;
            if (partId != null && partId != "0")
            {
             decodepartID=  CWBAppUtils.DecodeString(partId);
            }
            return decodepartID;
        }

        public async Task<IActionResult> EditRawMaterial(string partId)
        {
            int decodedPartId = (int)CWBAppUtils.DecodeString(partId);
            RawMaterialDetailVM manuf = await _mastersService.GetRMPart(decodedPartId);
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
            await SupplierViewBag();
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
        public async Task<IActionResult> GetUOMs()
        {
            var mfpdList = await _mastersService.GetUOMs();
            return Json(mfpdList);
        }

        [HttpPost]
        public async Task<IActionResult> AddUOM(UOMVM model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _mastersService.AddUOM(model);
            return Ok(result);
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
        public async Task<IActionResult> BOMS(string partId)//pass Id from ManufNoPartDetails
        {
            var mpmakefromlist = await _mastersService.BOMS(partId);
            var partsuoms = await _mastersService.GetPartsUOMs();

            foreach (MPBomVM mpvm in mpmakefromlist)
            {
                foreach (PartUOMVM partu in partsuoms)
                {
                    if (mpvm.BOMPartId == partu.PartId)
                    {
                        mpvm.UOM = partu.UOMName;
                    }
                }
            }
            return Ok(mpmakefromlist);

        }
        [HttpGet]
        public async Task<IActionResult> MPMakeFromList(string partId)
        {
            var mpmakefromlist = await _mastersService.GetMPMakeFromListByPartId(partId);
            var partsuoms = await _mastersService.GetPartsUOMs();
            
            foreach(MPMakeFromVM mpvm in mpmakefromlist)
            {
                foreach(PartUOMVM partu in partsuoms)
                {
                    if(mpvm.MPPartId == partu.PartId)
                    {
                        mpvm.UOM = partu.UOMName;
                    }
                }
            }
            return Ok(mpmakefromlist);
        }

        [HttpGet]
        public async Task<IActionResult> SortedMPMakeFromList(string partId)
        {
            try
            {
                //getmaterparts
                var mf = await _mastersService.GetMPMakeFromListByPartId(partId);
                var mpart =await _mastersService.ItemMasterParts();
                List<MPMakeFromVM> MFList = new List<MPMakeFromVM>();
                var query = from mfl in mf
                            join mp in mpart on mfl.MPPartId equals mp.PartId
                            select new MPMakeFromVM
                            {
                                MPPartId = mfl.MPPartId,
                                MPPartMadeFrom=mfl.MPPartMadeFrom,
                                InputWeight=mfl.InputWeight,
                                ScrapGenerated=mfl.ScrapGenerated,
                                QuantityPerInput=mfl.QuantityPerInput,
                                YieldNotes=mfl.YieldNotes,
                                PreferedRawMaterial=mfl.PreferedRawMaterial,
                                ManufPartId=mfl.ManufPartId,
                                MPMakeFromId=mfl.MPMakeFromId,
                                MFDescription=mfl.MFDescription,
                                InputPartNo=mp.PartNo,
                                UOM=mfl.UOM,
                                TenantId=mfl.TenantId
                            };
                MFList = query
                     .OrderBy(x => x.PreferedRawMaterial)
                    .ToList();
                return Ok(MFList);
            }
            catch(Exception ex)
            {
                return BadRequest();
            }
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

        public async Task<JsonResult> CheckPartNo(string partNo,string partNumber="")
        {
            if(partNumber.Length>0)
            {
                partNo = partNumber;
            }
            var result = await _mastersService.CheckPartNo(partNo);
            return Json(!result);
        }

        [HttpGet]
        public async Task<JsonResult> CheckPartNo(string partNo)
        {
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
        private async Task CustomerViewBag()
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

        private async Task CompaniesViewBagForManuF(ManufacturedPartNoDetailVM manuf=null)
        {
            var uoms = await _mastersService.GetUOMs();
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
            var uoms = await _mastersService.GetUOMs();
            var companies = await _mastersService.GetCompanies();
            companies = companies.Where(m => m.CompanyType.Equals("Both") || m.CompanyType.Equals("Supplier"));
            if (manuf != null)
            {
                ViewBag.Companies = companies.Select(c => new SelectListItem { Text = c.CompanyName, Value = c.CompanyId.ToString() }).ToList();
                ViewBag.UOMs = uoms.Select(c => new SelectListItem { Text = c.Name, Value = c.UOMId.ToString(), Selected = (c.UOMId == manuf.UOMId) }).ToList();
            }
            else
            {
                ViewBag.Companies = companies.Select(c => new SelectListItem { Text = c.CompanyName, Value = c.CompanyId.ToString() }).ToList();
                ViewBag.UOMs = uoms.Select(c => new SelectListItem { Text = c.Name, Value = c.UOMId.ToString() }).ToList();
            }
        }


        private async Task CompaniesViewBagForRawMaterial(RawMaterialDetailVM manuf = null)
        {
            var uoms = await _mastersService.GetUOMs();
            var companies = await _mastersService.GetCompanies();
            companies = companies.Where(m => m.CompanyType.Equals("Both") || m.CompanyType.Equals("Supplier"));
            if (manuf != null)
            {
                ViewBag.Suppliers = companies.Select(c => new SelectListItem { Text = c.CompanyName, Value = c.CompanyId.ToString(), Selected = (c.CompanyId == manuf.SupplierId) }).ToList();
                ViewBag.UOMs = uoms.Select(c => new SelectListItem { Text = c.Name, Value = c.UOMId.ToString(), Selected = (c.UOMId == manuf.UOMId) }).ToList();
            }
            else
            {
                ViewBag.Suppliers = companies.Select(c => new SelectListItem { Text = c.CompanyName, Value = c.CompanyId.ToString() }).ToList();
                ViewBag.UOMs = uoms.Select(c => new SelectListItem { Text = c.Name, Value = c.UOMId.ToString() }).ToList();
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
