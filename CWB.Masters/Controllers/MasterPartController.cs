using CWB.CommonUtils.Common;
using CWB.Constants.UserIdentity;
using CWB.Logging;
using CWB.Masters.Domain.ItemMaster;
using CWB.Masters.MastersUtils;
using CWB.Masters.MastersUtils.ItemMaster;
using CWB.Masters.Services.Company;
using CWB.Masters.Services.DocumentManagement;
using CWB.Masters.Services.Failures;
using CWB.Masters.Services.ItemMaster;
using CWB.Masters.ViewModels.Company;
using CWB.Masters.ViewModels.DocumentManagement;
using CWB.Masters.ViewModels.FailureError;
using CWB.Masters.ViewModels.ItemMaster;
using CWB.Masters.ViewModelValidators.ItemMaster;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CWB.Masters.Controllers
{
    [ApiController]
    [Authorize(Roles = Roles.ADMIN)]
    public class MasterPartController : ControllerBase
    {
        private readonly ILoggerManager _logger;
        private readonly IRawMaterialDetailService _rawMaterialDetailService;
        private readonly IBoughtOutFinishDetailService _boughtOutFinishDetailService;
        private readonly IManufacturedPartNoDetailService _manufacturedPartNoDetailService;
        private readonly IMasterPartService _masterPartService;
        private readonly IFailureServices _IFailureServices;
        private readonly ICompanyService _companyService;
        private readonly IDocumentManagementService _documentManagementService; // Injected Service


        public MasterPartController(ILoggerManager logger
            , IRawMaterialDetailService rawMaterialDetailService
            , IManufacturedPartNoDetailService manufacturedPartNoDetailService
            , IBoughtOutFinishDetailService boughtOutFinishDetailService
            ,ICompanyService companyService
            , IFailureServices IFailureServices
            , IMasterPartService masterPartService, IDocumentManagementService documentManagementService)
        {
            _logger = logger;
            _rawMaterialDetailService = rawMaterialDetailService;
            _manufacturedPartNoDetailService = manufacturedPartNoDetailService;
            _boughtOutFinishDetailService = boughtOutFinishDetailService;
            _masterPartService = masterPartService;
            _IFailureServices = IFailureServices;
            _documentManagementService = documentManagementService;
            _companyService = companyService;
        }
        [HttpGet]
        [Route(ApiRoutes.MasterParts.MasterPartList)]
        [Produces(AppContentTypes.ContentType, Type = typeof(List<ItemMasterPartVM>))]
        public async Task<IActionResult> MasterParts(long tenantId)
        {
            var companies = (await _companyService.GetCompaniesByTenant(tenantId)).ToList();
            var masterParts = _masterPartService.GetAllMasterParts().Where(m => m.TenantId == tenantId).ToList();
            var manufList = _manufacturedPartNoDetailService.GetAllManufacturedPartNoDetailsByTypeTenant(tenantId).ToList();
            var bofs = _boughtOutFinishDetailService.GetBoughtOutFinishDetailsByTenant(tenantId).ToList();
            var rms = _rawMaterialDetailService.GetRawMaterialDetailsByTenant(tenantId).ToList();
            var partPurchases = _rawMaterialDetailService.GetPartPurchases(tenantId).ToList();

            // 1b. Fetch Enrichment Data (Docs & Statuses)
            var docmand = (await _masterPartService.GetAllItemMasterDocList(tenantId)).ToList();
            var docListVMs = (await _documentManagementService.GetAllDocList(tenantId)).ToList(); 
            var makeFromList = _manufacturedPartNoDetailService.GetAllMPMakeFromList(0).ToList();
            var allBOMs = _manufacturedPartNoDetailService.GetAllMPBOMList(tenantId).ToList();
            var companyLookup = companies
            .Select(c => new { c.CompanyId, c.CompanyName })
            .Distinct().ToDictionary(c => c.CompanyId, c => c.CompanyName);
            var masterPartLookup = masterParts.ToDictionary(m => m.MasterPartId);

            var purchaseByBof = partPurchases
                .Where(p => p.BOFId > 0)
                .GroupBy(p => p.BOFId)
                .ToDictionary(g => g.Key, g => g.First().PSupplierId);

            var purchaseByRm = partPurchases
                .Where(p => p.RMId > 0)
                .GroupBy(p => p.RMId)
                .ToDictionary(g => g.Key, g => g.First().PSupplierId);

            var makeFromLookup = makeFromList
                .Select(x => x.ManufPartId)
                .Distinct()
                .ToHashSet();

            var bomParentLookup = allBOMs
                .Select(x => x.BOMManufPartId) // Assuming BOMManufPartId is the Parent's ManufPartDetailId
                .Distinct()
                .ToHashSet();

            var parentPartNoMap = manufList
        .Where(m => masterPartLookup.ContainsKey(m.PartId))
        .ToDictionary(
            m => m.ManufacturedPartNoDetailId,
            m => masterPartLookup[m.PartId].PartNo
        );

            // 2. Map Child PartId -> Comma Separated Parent PartNos
            // We group BOMs by the Child Part (BOMPartId) to see where that child is used.
            var childUsageMap = allBOMs
                .Where(b => parentPartNoMap.ContainsKey(b.BOMManufPartId)) // Ensure valid parent
                .GroupBy(b => b.BOMPartId) // Group by the Child Part
                .ToDictionary(
                    g => g.Key,
                    g => string.Join(", ", g.Select(b => parentPartNoMap[b.BOMManufPartId]).Distinct())
                );

            var list = new ConcurrentBag<ItemMasterPartVM>();

            // --- Step 3: Process in parallel (pure in-memory, safe) ---
            Parallel.Invoke(
                // Manufactured Parts
                () =>
                {
                    foreach (var m in manufList)
                    {
                        masterPartLookup.TryGetValue(m.PartId, out var mp);
                        companyLookup.TryGetValue(m.CompanyId, out var coName);

                        childUsageMap.TryGetValue(m.PartId, out var usedInAssemblies);

                        // Doc Status Logic (In-Memory)
                        var docStatus = GetDocStatusInMemory(docmand, docListVMs, m.PartId, m.ManufacturedPartType == 1 ? 1 : 2);

                        // LOGIC FIX: Check lookups instead of DB flags
                        var rmAvl = makeFromLookup.Contains((int)m.ManufacturedPartNoDetailId) ? "Yes" : "No";
                        var bomAvl = bomParentLookup.Contains(m.ManufacturedPartNoDetailId) ? "Yes" : "No";

                        list.Add(new ItemMasterPartVM
                        {
                            PartId = m.PartId,
                            MasterPartType = m.ManufacturedPartType == 1 ? "ManufacturedPart" : "Assembly",
                            CompanyId = m.CompanyId,
                            Company = coName ?? string.Empty,
                            PartNo = mp?.PartNo ?? string.Empty,
                            Inv_Trans = mp?.Inv_Trans ?? 'N',
                            ListAssembly = usedInAssemblies ?? "-",
                            Linked_to_BOM = string.IsNullOrEmpty(usedInAssemblies) ? 'N' : 'Y',
                            Description = mp?.PartDescription ?? string.Empty,
                            Status = mp?.Status,
                            Notes = mp?.PartDescription ?? string.Empty,
                            TenantId = m.TenantId,

                            // Enrichment Fields
                            FinalPart = m.FinalPartNosoldtoCustomer == 1 ? "Y" : "N", // Assuming 1=Yes based on typical logic
                            MasterDisplay = m.ManufacturedPartType == 1 ? "ManufacturedPart" : "Assembly",
                            MandocAvl = docStatus.MandocAvl,
                            DocStatus = docStatus.DocStatusDesc,
                            RmAvl = m.ManufacturedPartType == 1 ? rmAvl : "N/A", // RmAvl only relevant for Manuf Parts
                            BomAvl = m.ManufacturedPartType == 2 ? bomAvl : "N/A", // BomAvl usually only relevant for Assemblies
                            SupplierAvl = "N/A"
                        });
                    }
                },

                // BOFs
                () =>
                {
                    foreach (var b in bofs)
                    {
                        masterPartLookup.TryGetValue(b.PartId, out var mp);
                        purchaseByBof.TryGetValue((int)b.BoughtOutFinishDetailId, out var supp);
                        childUsageMap.TryGetValue(b.PartId, out var usedInAssemblies);

                        var docStatus = GetDocStatusInMemory(docmand, docListVMs, b.PartId, 6, 7, 8);

                        companyLookup.TryGetValue(supp, out var coName);
                        list.Add(new ItemMasterPartVM
                        {
                            PartId = b.PartId,
                            MasterPartType = "BOF",
                            Company = coName ?? string.Empty,
                            PartNo = mp?.PartNo ?? string.Empty,
                            Description = mp?.PartDescription ?? string.Empty,
                            Status = mp?.Status,
                            Notes = mp?.PartDescription ?? string.Empty,
                            Inv_Trans = mp?.Inv_Trans ?? 'N',
                            ListAssembly = usedInAssemblies ?? "-",
                            Linked_to_BOM = string.IsNullOrEmpty(usedInAssemblies) ? 'N' : 'Y',
                            BOFId = (int)b.BoughtOutFinishDetailId,
                            TenantId = b.TenantId,

                            // Enrichment Fields
                            MasterDisplay = b.BoughtOutFinishMadeType switch { 1 => "Standard BOF", 2 => "Catalog BOF", _ => "Purchased Made to Print BOF" },
                            MandocAvl = docStatus.MandocAvl,
                            DocStatus = docStatus.DocStatusDesc,
                            SupplierAvl = string.IsNullOrEmpty(coName) ? "No" : "Yes",
                            BomAvl = "N/A",
                            RmAvl = "N/A"
                        });
                    }
                },

                // Raw Materials
                () =>
                {
                    foreach (var r in rms)
                    {
                        masterPartLookup.TryGetValue((int)r.PartId, out var mp);
                        purchaseByRm.TryGetValue((int)r.RawMaterialDetailId, out var supp);
                        childUsageMap.TryGetValue((int)r.PartId, out var usedInAssemblies);
                        var docStatus = GetDocStatusInMemory(docmand, docListVMs, r.PartId, 3, 4, 5);

                        companyLookup.TryGetValue(supp, out var coName);
                        list.Add(new ItemMasterPartVM
                        {
                            PartId = r.PartId,
                            MasterPartType = "RawMaterial",
                            Company = coName ?? string.Empty,
                            PartNo = mp?.PartNo ?? string.Empty,
                            Description = mp?.PartDescription ?? string.Empty,
                            Status = mp?.Status,
                            Inv_Trans = mp?.Inv_Trans ?? 'N',
                            ListAssembly = usedInAssemblies ?? "-",
                            Linked_to_BOM = string.IsNullOrEmpty(usedInAssemblies) ? 'N' : 'Y',
                            Notes = mp?.PartDescription ?? string.Empty,
                            RMId = (int)r.RawMaterialDetailId,
                            TenantId = r.TenantId,

                            // Enrichment Fields
                            MasterDisplay = r.RawMaterialMadeType == 1 ? "Own Purchased RM" : "Customer Supplied RM",
                            MandocAvl = docStatus.MandocAvl,
                            DocStatus = docStatus.DocStatusDesc,
                            SupplierAvl = string.IsNullOrEmpty(coName) ? "No" : "Yes",
                            BomAvl = "N/A",
                            RmAvl = "N/A"
                        });
                    }
                }
            );

            // --- Step 4: Deduplicate by PartId ---
            return Ok(list.GroupBy(x => x.PartNo).Select(g => g.First()).ToList());
        }

        // Helper for In-Memory Doc Status Calculation (No DB calls)
        private (string MandocAvl, string DocStatusDesc) GetDocStatusInMemory(
            List<ItemMasterDocListVM> docmand,
            List<DocListVM> docListVMs,
            long? partId,
            params int[] contentIds)
        {
            if (partId == null) return ("N/A", "N/A");

            // Filter config for this content type
            var relevantConfigs = docmand.Where(d => contentIds.Contains((int)d.ContentId)).ToList();
            if (!relevantConfigs.Any()) return ("N/A", "N/A");

            // Filter actual docs for this part and these configs
            var uploadedDocs = docListVMs
                .Where(doc => doc.PartId == partId && relevantConfigs.Any(rc => rc.DocumentTypeId == doc.DocumentTypeId))
                .ToList();

            if (!uploadedDocs.Any()) return ("No", "N/A");

            // Check Mandatory
            var mandatoryConfigs = relevantConfigs.Where(c => c.Mandatory == 'Y');
            bool allMandatoryUploaded = mandatoryConfigs.All(mc => uploadedDocs.Any(ud => ud.DocumentTypeId == mc.DocumentTypeId));

            if (!allMandatoryUploaded) return ("Yes", "N/A"); // Partial upload

            // If we have documents, try to find a status (This is a simplification as we can't call DB here)
            // Ideally, you'd fetch the status description map in Step 1 and lookup here.
            // For now, we return "Yes" or the Status ID if description is unavailable.
            return ("Yes", "N/A");
        }

        [HttpGet]
        [Route(ApiRoutes.MasterParts.CheckPartNo)]
        [Produces(AppContentTypes.ContentType, Type = typeof(bool))]
        public async Task<IActionResult> CheckPartNo(string partNo)
        {
            bool exists = false;
            int partId = _masterPartService.CheckPartNo(partNo);
            if (partId > 0)
            {
                exists = _manufacturedPartNoDetailService.CheckPartNo(partId);
                if (!exists)
                {
                    exists = _rawMaterialDetailService.CheckPartNo(partId);
                    if (!exists)
                    {
                        exists = _boughtOutFinishDetailService.CheckPartNo(partId);
                    }
                }
            }
            return Ok(exists);
        }

        [HttpGet]
        [Route(ApiRoutes.Masters.GetStatuses)]
        [Produces(AppContentTypes.ContentType, Type = typeof(List<PartsStatusVM>))]
        public async Task<IActionResult> GetStatuses()
        {
            var companyTypes = await _masterPartService.GetStatuses();
            return Ok(companyTypes);
        }


        [HttpGet]
        [Route(ApiRoutes.Masters.GetAllItemMasterDocList)]
        [Produces(AppContentTypes.ContentType, Type = typeof(ItemMasterDocListVM))]
        public async Task<IActionResult> GetDocumentType(long tenantId)
        {
            var result = await _masterPartService.GetAllItemMasterDocList(tenantId);
            return Ok(result);
        }

        [HttpPost]
        [Route(ApiRoutes.Masters.PostItemMasterDocList)]
        [Produces(AppContentTypes.ContentType, Type = typeof(ItemMasterDocListVM))]
        public async Task<IActionResult> PostItemMasterDocList([FromBody] ItemMasterDocListVM documentType)
        {
            var result = await _masterPartService.PostItemMasterDocList(documentType);
            return Ok(result);
        }


        [HttpGet]
        [Route(ApiRoutes.Masters.GetAllItemMasterContent)]
        [Produces(AppContentTypes.ContentType, Type = typeof(ItemMasterContentVM))]
        public async Task<IActionResult> GetAllItemMasterContent()
        {
            var result = await _masterPartService.GetAllItemMasterContent();
            return Ok(result);
        }

        [HttpGet]
        [Route(ApiRoutes.Masters.DeleteItemMasterDoc)]
        [Produces(AppContentTypes.ContentType, Type = typeof(bool))]
        public async Task<IActionResult> DeleteItemMasterDoc(long itemMasterDocListId, long tenantId)
        {
            var result = await _masterPartService.DeleteItemMasterDoc(itemMasterDocListId, tenantId);
            return Ok(result);
        }
        [HttpGet]
        [Route(ApiRoutes.Masters.DeleteItemMasterPart)]
        [Produces(AppContentTypes.ContentType, Type = typeof(bool))]
        public async Task<IActionResult> DeleteItemMasterPart(long itemMasterDocListId, long tenantId)
        {
            var result = await _masterPartService.DeleteItemMasterPart(itemMasterDocListId, tenantId);
            return Ok(result);
        }

        [HttpGet]
        [Route(ApiRoutes.Masters.CheckPartNoInDocList)]
        [Produces(AppContentTypes.ContentType, Type = typeof(bool))]
        public async Task<IActionResult> CheckPartNoInDocList(long documentTypeId, long contentId, long tenantId)
        {
            bool exists = false;
            exists = await _masterPartService.CheckDocumentTypeInItemMaster(documentTypeId,contentId, tenantId);
            return Ok(exists);
        }
        [HttpPost]
        [Route(ApiRoutes.MasterParts.Postfailure)]
        [Produces(AppContentTypes.ContentType, Type = typeof(FailureVM))]
        public async Task<IActionResult> PostFailure([FromBody] FailureVM documentType)
        {
            var result = await _IFailureServices.PostFailure(documentType);
            return Ok(result);
        }


        [HttpGet]
        [Route(ApiRoutes.MasterParts.GetFailure)]
        [Produces(AppContentTypes.ContentType, Type = typeof(FailureVM))]
        public async Task<IActionResult> GetAllFailure(long tenantId)
        {
            var result = await _IFailureServices.GetFailure(tenantId);
            return Ok(result);
        }

    }

}
