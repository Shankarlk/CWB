using CWB.App.Models.Contacts;
using CWB.App.Models.ItemMaster;
using CWB.App.Services.Masters;
using CWB.CommonUtils.Common;
using CWB.Constants.UserIdentity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CWB.App.Controllers
{
    [Authorize(Roles = Roles.ADMIN)]
    public class ItemMasterInfoController : Controller
    {
        private readonly ILogger<ItemMasterInfoController> _logger;
        private readonly IMastersServices _mastersService;
        public ItemMasterInfoController(ILogger<ItemMasterInfoController> logger, IMastersServices mastersService)
        {
            _logger = logger;
            _mastersService = mastersService;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult WhereUsed()
        {
            return View();
        }
        public IActionResult ChildRawMatl()
        {
            return View();
        }
        public IActionResult ChildBom()
        {
            return View();
        }
        public async Task<IActionResult> ViewSuppForPart()
        {
            await ContactsViewBag();
            return View();
        }
        public async Task<IActionResult> ViewEditPart(string returnUrl = null)
        {
            await StatusViewBagForManuf();
            if (string.IsNullOrEmpty(returnUrl))
            {
                returnUrl = Url.Action("Index", "Home"); // or wherever you want as default
            }

            ViewBag.ReturnUrl = returnUrl;
            return View();
        }
        public IActionResult EditPart(long partId, string partType, string returnUrl)
        {
            string encodepartid = CWBAppUtils.EncodeLong(partId);

            if (partType.Equals("BOF") || partType.Contains("BOF"))
            {
                return RedirectToAction("EditBOF", new { partId = encodepartid, returnUrl });
            }
            else if (partType.Equals("RawMaterial") || partType.Contains("RM"))
            {
                return RedirectToAction("EditRawMaterial", new { partId = encodepartid, returnUrl });
            }
            return RedirectToAction("EditManufPart", new { partId = encodepartid, returnUrl });
        }

        public async Task<IActionResult> EditBOF(string partId, string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl ?? Url.Action("Index");
            int decodedPartId = (int)CWBAppUtils.DecodeString(partId);
            BoughtOutFinishDetailVM manuf = await _mastersService.GetBOFPart(decodedPartId);
            await CompaniesViewBagForBOF();
            await SupplierViewBag();
            await StatusViewBagForBOF();
            return View(manuf);
        }
        public async Task<IActionResult> EditRawMaterial(string partId, string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl ?? Url.Action("Index");
            int decodedPartId = (int)CWBAppUtils.DecodeString(partId);
            RawMaterialDetailVM manuf = await _mastersService.GetRMPart(decodedPartId);
            await SupplierViewBag();
            await CompaniesViewBagForRawMaterial(manuf);
            await StatusViewBagForRawMaterial(manuf);
            await RawMaterialViewBags(manuf);
            return View(manuf);
        }
        public async Task<IActionResult> EditManufPart(string partId, string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl ?? Url.Action("Index");
            int decodedManufPartId = (int)CWBAppUtils.DecodeString(partId.ToString());
            ManufacturedPartNoDetailVM manuf = await _mastersService.GetManufPart(decodedManufPartId);
            await CustomerViewBag();
            await CompaniesViewBagForManuF(manuf);
            await StatusViewBagForManuf(manuf);
            return View(manuf);
        }

        [HttpGet]
        public async Task<IActionResult> MasterParts()
        {
            var mfpdList = await _mastersService.ItemMasterParts();
            foreach (var item in mfpdList)
            {

                if (item.MasterPartType == "ManufacturedPart")
                {
                    item.MasterDisplay = "Child Manufactured Part";
                }
                if (item.MasterPartType == "Assembly")
                {
                    item.MasterDisplay = "Assembly";
                } 
                if (item.MasterPartType == "BOF")
                {
                    if (item.BoughtOutFinishMadeType == 1)
                    {
                        item.MasterDisplay = "Standard BOF";
                    }
                    else if (item.BoughtOutFinishMadeType == 2)
                    {
                        item.MasterDisplay = "Catalog BOF";
                    }
                    else
                    {
                        item.MasterDisplay = "Purchased Made to Print BOF";
                    }
                }
                if (item.MasterPartType == "RawMaterial")
                {
                    var rm = await _mastersService.GetRMPart((int)item.PartId);
                    if (rm.RawMaterialMadeType == 1)
                    {
                        item.MasterDisplay = "Own Purchased RM";
                    }
                    else
                    {
                        item.MasterDisplay = "Customer Supplied RM";
                    }
                }
            }
            return Ok(mfpdList);
        }
        [HttpGet]
        public async Task<IActionResult> ChildPartParents(int partid, string parttype)
        {
            if(parttype == "Own Purchased RM" || parttype == "Customer Supplied RM")
            {
                var mfpdList = await _mastersService.ItemMasterParts();
                var result = new List<ItemMasterPartVM>();
                var manufacturedParts = mfpdList
                    .Where(item => item.MasterPartType == "ManufacturedPart")
                    .ToList();

                var manufTasks = manufacturedParts.Select(async item =>
                {
                    var manuf = await _mastersService.GetManufPart((int)item.PartId);
                    var mk = await _mastersService.GetMPMakeFromListByPartId(manuf.ManufacturedPartNoDetailId.ToString());

                    var preferredMaterial = mk.FirstOrDefault(m => m.MPPartId == partid);
                    if (preferredMaterial != null)
                    {
                        item.Preferred = preferredMaterial.PreferedRawMaterial ? "Yes" : "No";
                        return item;
                    }
                    return null; // Return null if no preferred material found
                });
                result.AddRange(await Task.WhenAll(manufTasks) ?? Enumerable.Empty<ItemMasterPartVM>());

                return Ok(result.Where(item => item != null));
            }
            else if (parttype == "Purchased Made to Print BOF" || parttype == "Standard BOF" || parttype == "Catalog BOF")
            {
                var mfpdList = await _mastersService.ItemMasterParts();
                var result = new List<ItemMasterPartVM>();
                var assemblyItems = mfpdList.Where(item => item.MasterPartType == "Assembly").ToList();
                var assemblyTasks = assemblyItems.Select(async item =>
                {
                    var manuf = await _mastersService.GetManufPart((int)item.PartId);
                    var mpmakefromlist = await _mastersService.BOMS(manuf.ManufacturedPartNoDetailId.ToString());
                    var matchingBOM = mpmakefromlist.FirstOrDefault(m => m.BOMPartId == partid);
                    if (matchingBOM != null)
                    {
                        item.Preferred = mpmakefromlist.Count() == 1 ? "Yes" : "No";
                        return item; // Return the modified item
                    }
                    return null; // Return null if no match found
                });
                var processedItems = await Task.WhenAll(assemblyTasks);
                result.AddRange(processedItems.Where(item => item != null));
                return Ok(result);
            }
            else if (parttype == "Child Manufactured Part")
            {
                var mfpdList = await _mastersService.ItemMasterParts();
                var result = new List<ItemMasterPartVM>();

                // First, get all manufactured parts
                var manufacturedParts = mfpdList
                    .Where(item => item.MasterPartType == "ManufacturedPart" || item.MasterPartType == "Assembly")
                    .ToList();

                var manufTasks = manufacturedParts.Select(async item =>
                {
                    var manuf = await _mastersService.GetManufPart((int)item.PartId);

                    // Get both MPMakeFromList and BOM list
                    var mkList = await _mastersService.GetMPMakeFromListByPartId(manuf.ManufacturedPartNoDetailId.ToString());
                    var bomList = await _mastersService.BOMS(manuf.ManufacturedPartNoDetailId.ToString());

                    // Check if our given partid exists in either list
                    var matchFromMK = mkList.FirstOrDefault(m => m.MPPartId == partid);
                    var matchFromBOM = bomList.FirstOrDefault(b => b.BOMPartId == partid);

                    if (matchFromMK != null || matchFromBOM != null)
                    {
                        // Prefered logic:
                        if (matchFromMK != null)
                        {
                            item.Preferred = matchFromMK.PreferedRawMaterial ? "Yes" : "No";
                        }
                        else
                        {
                            // For BOM, preferred = "Yes" if it's the only BOM item
                            item.Preferred = bomList.Count() == 1 ? "Yes" : "No";
                        }

                        return item;
                    }

                    return null; // no match found
                });

                var processedItems = await Task.WhenAll(manufTasks);
                result.AddRange(processedItems.Where(item => item != null));

                return Ok(result);
            }
            else if (parttype == "Assembly")
            {
                var mfpdList = await _mastersService.ItemMasterParts();
                var result = new List<ItemMasterPartVM>();

                // Get only assembly type parts
                var assemblyParts = mfpdList
                    .Where(item => item.MasterPartType == "ManufacturedPart" || item.MasterPartType == "Assembly")
                    .ToList();

                var assemblyTasks = assemblyParts.Select(async item =>
                {
                    var manuf = await _mastersService.GetManufPart((int)item.PartId);

                    // Only check in BOMS
                    var bomList = await _mastersService.BOMS(manuf.ManufacturedPartNoDetailId.ToString());
                    var matchFromBOM = bomList.FirstOrDefault(b => b.BOMPartId == partid);

                    if (matchFromBOM != null)
                    {
                        // Preferred = Yes if only one BOM item exists
                        item.Preferred = bomList.Count() == 1 ? "Yes" : "No";
                        return item;
                    }
                    return null;
                });

                var processedItems = await Task.WhenAll(assemblyTasks);
                result.AddRange(processedItems.Where(item => item != null));

                return Ok(result);
            }
            return Ok("Not Found");

        }
        [HttpGet]
        public async Task<IActionResult> RawMatlOfCmp(int partId)
        {
            var partsuoms = await _mastersService.GetPartsUOMs();
            var allParts = new List<MPMakeFromVM>();

            async Task TraverseCMPChain(int currentPartId)
            {
                var manuf = await _mastersService.GetManufPart(currentPartId);
                var mpmakefromlist = await _mastersService
                    .GetMPMakeFromListByPartId(manuf.ManufacturedPartNoDetailId.ToString());

                foreach (var mpvm in mpmakefromlist)
                {
                    // Attach UOM if available
                    mpvm.UOM = partsuoms.FirstOrDefault(p => p.PartId == mpvm.MPPartId)?.UOMName;

                    // Set MadeFrom and decide if we continue
                    switch (mpvm.MPPartMadeFrom)
                    {
                        case 1:
                            mpvm.MadeFrom = "Customer Supplied Raw Material";
                            break;
                        case 2:
                            mpvm.MadeFrom = "Own Raw Material";
                            break;
                        default:
                            mpvm.MadeFrom = "Other Manufactured Part"; // CMP
                            break;
                    }

                    allParts.Add(mpvm);

                    // If this is a CMP, go deeper
                    if (mpvm.MadeFrom == "Other Manufactured Part")
                    {
                        await TraverseCMPChain(mpvm.MPPartId);
                    }
                }
            }

            await TraverseCMPChain(partId);

            // Remove duplicates (if any) and keep order
            //var result = allParts
            //    .GroupBy(p => p.MPPartId)
            //    .Select(g => g.First())
            //    .ToList();
            var mfpdList = await _mastersService.ItemMasterParts();
            foreach (var partss in allParts)
            {
                foreach (var item in mfpdList)
                {
                    if (item.PartId != partss.MPPartId)
                        continue;
                    partss.PartNo = item.PartNo;
                    partss.Description = item.Description;
                    partss.PartId = item.PartId;
                    if (item.MasterPartType == "ManufacturedPart")
                    {
                        partss.MasterPartType = "Child Manufactured Part";
                    }
                    if (item.MasterPartType == "Assembly")
                    {
                        partss.MasterPartType = "Assembly";
                    }
                    if (item.MasterPartType == "BOF")
                    {
                        if (item.BoughtOutFinishMadeType == 1)
                        {
                            partss.MasterPartType = "Standard BOF";
                        }
                        else if (item.BoughtOutFinishMadeType == 2)
                        {
                            partss.MasterPartType = "Catalog BOF";
                        }
                        else
                        {
                            partss.MasterPartType = "Purchased Made to Print BOF";
                        }
                    }
                    if (item.MasterPartType == "RawMaterial")
                    {
                        var rm = await _mastersService.GetRMPart((int)item.PartId);
                        if (rm.RawMaterialMadeType == 1)
                        {
                            partss.MasterPartType = "Own Purchased RM";
                        }
                        else
                        {
                            partss.MasterPartType = "Customer Supplied RM";
                        }
                    }
                }
            }
            return Ok(allParts);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAssemWithBom()
        {
            var mfpdList = await _mastersService.ItemMasterParts();
            var result = new List<ItemMasterPartVM>(); 

            foreach (var item in mfpdList)
            {
                if (item.MasterPartType != "Assembly")
                    continue;

                var manuf = await _mastersService.GetManufPart((int)item.PartId);
                var mpmakefromlist = await _mastersService.BOMS(manuf.ManufacturedPartNoDetailId.ToString());

                if (mpmakefromlist != null && mpmakefromlist.Any())
                {
                    item.BomAvl = "Yes";
                    result.Add(item); // only add assemblies with BOM
                }
            }

            return Ok(result);
        }
        [HttpGet]
        public async Task<IActionResult> GetAllBOMS(int partId)
        {
            var allBomParts = new List<MPBomVM>();
            var itemMasterParts = await _mastersService.ItemMasterParts();

            async Task TraverseBOM(int currentPartId, int level)
            {
                // Get manufacturing part details
                var manuf = await _mastersService.GetManufPart(currentPartId);
                if (manuf == null)
                    return;

                // Get BOM for this manufactured part
                var bomList = await _mastersService.BOMS(manuf.ManufacturedPartNoDetailId.ToString());

                foreach (var bomItem in bomList)
                {
                    // Optional: indent part numbers to visually show hierarchy
                    var itemMaster = itemMasterParts.FirstOrDefault(p => p.PartId == bomItem.BOMPartId);
                    if (itemMaster != null)
                    {
                        bomItem.PartNo = itemMaster.PartNo;
                        bomItem.Description = itemMaster.Description;
                        bomItem.PartId = itemMaster.PartId;
                        if (itemMaster.MasterPartType == "ManufacturedPart")
                        {
                            bomItem.MasterPartType = "Child Manufactured Part";
                        }
                        if (itemMaster.MasterPartType == "Assembly")
                        {
                            bomItem.MasterPartType = "Assembly";
                        }
                        if (itemMaster.MasterPartType == "BOF")
                        {
                            if (itemMaster.BoughtOutFinishMadeType == 1)
                            {
                                bomItem.MasterPartType = "Standard BOF";
                            }
                            else if (itemMaster.BoughtOutFinishMadeType == 2)
                            {
                                bomItem.MasterPartType = "Catalog BOF";
                            }
                            else
                            {
                                bomItem.MasterPartType = "Purchased Made to Print BOF";
                            }
                        }
                        if (itemMaster.MasterPartType == "RawMaterial")
                        {
                            var rm = await _mastersService.GetRMPart((int)itemMaster.PartId);
                            if (rm.RawMaterialMadeType == 1)
                            {
                                bomItem.MasterPartType = "Own Purchased RM";
                            }
                            else
                            {
                                bomItem.MasterPartType = "Customer Supplied RM";
                            }
                        }
                    }
                    allBomParts.Add(bomItem);

                    // If this BOM item is itself an assembly, go deeper
                    if (itemMaster?.MasterPartType == "Assembly")
                    {
                        await TraverseBOM(bomItem.BOMPartId, level + 1);
                    }
                }
            }

            // Start recursion from the given partId
            await TraverseBOM(partId, 0);

            return Ok(allBomParts);
        }
        [HttpGet]
        public async Task<IActionResult> PartPurchases()
        {
            var partpurchase = await _mastersService.PartPurchases();
            var companies = await _mastersService.GetCompanies();
            var itemMasterParts = await _mastersService.ItemMasterParts();
            foreach (var item in partpurchase)
            {
                var itemmaster = itemMasterParts.Where(i => i.PartId == item.PPartId).FirstOrDefault();
                if(itemmaster != null)
                {
                    item.PartNo = itemmaster.PartNo;
                    item.Description = itemmaster.Description;
                    if (itemmaster.MasterPartType == "ManufacturedPart")
                    {
                        item.MasterDisplay = "Child Manufactured Part";
                    }
                    if (itemmaster.MasterPartType == "Assembly")
                    {
                        item.MasterDisplay = "Assembly";
                    }
                    if (itemmaster.MasterPartType == "BOF")
                    {
                        if (itemmaster.BoughtOutFinishMadeType == 1)
                        {
                            item.MasterDisplay = "Standard BOF";
                        }
                        else if (itemmaster.BoughtOutFinishMadeType == 2)
                        {
                            item.MasterDisplay = "Catalog BOF";
                        }
                        else
                        {
                            item.MasterDisplay = "Purchased Made to Print BOF";
                        }
                    }
                    if (itemmaster.MasterPartType == "RawMaterial")
                    {
                        var rm = await _mastersService.GetRMPart((int)itemmaster.PartId);
                        if (rm.RawMaterialMadeType == 1)
                        {
                            item.MasterDisplay = "Own Purchased RM";
                        }
                        else
                        {
                            item.MasterDisplay = "Customer Supplied RM";
                        }
                    }
                }
                var supplier = companies.Where(c => c.CompanyId == item.PSupplierId).FirstOrDefault();
                if(supplier != null)
                {
                    item.PSupplier = supplier.CompanyName;
                    item.CompanyId = supplier.CompanyId;
                    item.DivisionId = supplier.DivisionId;
                    item.CompanyType = supplier.CompanyType;
                    item.CompanyName = supplier.CompanyName;
                    item.DivisionName = supplier.DivisionName;
                    item.Location = supplier.Location;
                    item.Notes = supplier.Notes;
                    item.PlantName = supplier.PlantName;
                    item.City = supplier.City;
                    item.Pincode = supplier.Pincode;
                    item.Country = supplier.Country;
                    item.GstNo = supplier.GstNo;
                    item.PanNo = supplier.PanNo;
                }
            }

            return Ok(partpurchase);
        }


        private async Task RawMaterialViewBags(RawMaterialDetailVM rmVm = null)
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
        private async Task CompaniesViewBagForManuF(ManufacturedPartNoDetailVM manuf = null)
        {
            var uoms = await _mastersService.GetUOMs();
            var companies = await _mastersService.GetCompanies();
            companies = companies.Where(m => m.CompanyType.Equals("Both") || m.CompanyType.Equals("Customer"));
            if (manuf != null)
            {
                ViewBag.Companies = companies.Select(c => new SelectListItem { Text = c.CompanyName, Value = c.CompanyId.ToString(), Selected = (c.CompanyId == manuf.CompanyId) }).ToList();
                ViewBag.UOMs = uoms.Select(c => new SelectListItem { Text = c.Name, Value = c.UOMId.ToString(), Selected = (c.UOMId == manuf.UOMId) }).ToList();
            }
            else
            {
                ViewBag.Companies = companies.Select(c => new SelectListItem { Text = c.CompanyName, Value = c.CompanyId.ToString() }).ToList();
                ViewBag.UOMs = uoms.Select(c => new SelectListItem { Text = c.Name, Value = c.UOMId.ToString() }).ToList();
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


        private async Task ContactsViewBag()
        {
            var companyTypes = await _mastersService.GetCompanyTypes();
            ViewBag.CompanyTypes = companyTypes.Select(c => new SelectListItem { Text = c.CompanyType, Value = c.CompanyTypeValue }).ToList();
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
                ViewBag.Statuses = types.Select(c => new SelectListItem { Text = c.Status, Value = c.StatusValue, Selected = (c.Status.Equals(manuf.Status)) }).ToList();
            }
            else
            {
                ViewBag.Statuses = types.Select(c => new SelectListItem { Text = c.Status, Value = c.StatusValue }).ToList();
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

    }
}
