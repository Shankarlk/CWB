using CWB.App.AppUtils;
using CWB.App.Models.DocumentManagement;
using CWB.App.Services.CompanySettings;
using CWB.App.Services.DocumentMagement;
using CWB.App.Services.Masters;
using CWB.App.Services.Routings;
using CWB.Constants.UserIdentity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace CWB.App.Controllers
{
    [Authorize(Roles = Roles.ADMIN)]
    public class DocumentManagementController : Controller
    {
        private readonly ILogger<DocumentManagementController> _logger;
        private readonly IDocMangService _docMangService;
        private readonly IMastersServices _masterService;
        private readonly IDepartmentService _departmentService;
        private readonly IRoutingService _routingService;

        public DocumentManagementController(ILogger<DocumentManagementController> logger, IMastersServices masterServices, 
            IDepartmentService departmentService, IDocMangService docMangService, IRoutingService routingService

            )
        {
            _logger = logger;
            _docMangService = docMangService;
            _masterService = masterServices;
            _departmentService = departmentService;
            _routingService = routingService;

        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult ViewDocument()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetAllDocumentType()
        {
            var doctype = await _docMangService.GetAllDocumentType();
            var docUploads = await _docMangService.GetAllDocUpload();
            var docViews = await _docMangService.GetAllDocView();
            var fileextn = await _docMangService.GetAllFileExtn();
            var dept = await _departmentService.GetDepartments(1);
            int noOfFiles = 0;
            foreach (var item in doctype)
            {
                foreach (var filext in fileextn)
                {
                    if(filext.ExtnId == item.ExtnId)
                    {
                        item.FileExtnName = filext.ExtnName;
                    }
                }
                foreach (var upload in docUploads)
                {
                    if (upload.DocumentTypeId == item.DocumentTypeId)
                    {
                        foreach (var dt in dept)
                        {
                            if (upload.DepartmentId == dt.DepartmentId)
                            {
                                item.DeptUploadName = dt.Name;
                            }
                        }

                    }
                }
                foreach (var views in docViews)
                {
                    if (views.DocumentTypeId == item.DocumentTypeId)
                    {
                        foreach (var dt in dept)
                        {
                            if (views.DepartmentId == dt.DepartmentId)
                            {
                                item.DeptViewName = dt.Name;
                            }
                        }
                        noOfFiles++;
                    }
                }
                if (item.Approval_Reqd == 1)
                {
                    item.Approval_ReqdStr = "Y";
                }
                else
                {
                    item.Approval_ReqdStr = "N";
                }
                item.NoOfFiles = noOfFiles;
                noOfFiles = 0;
            }
            return Ok(doctype);
        }


        [HttpPost]
        public async Task<IActionResult> PostDocumentType([FromBody]DocumentTypeVM documentTypeVMs)
        {
            var result = await _docMangService.PostDocumentType(documentTypeVMs);
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllDocCategory()
        {
            var doccat = await _docMangService.GetAllDocCategory();
            return Ok(doccat);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllFileExtn()
        {
            var doccat = await _docMangService.GetAllFileExtn();
            return Ok(doccat);
        }

        [HttpPost]
        public async Task<IActionResult> PostFileExtn([FromBody] ExtnInfoVM extnInfoVM)
        {
            var result = await _docMangService.PostFileExtn(extnInfoVM);
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetDepartMent()
        {
            var doccat = await _departmentService.GetDepartments(1);
            return Ok(doccat);
        }


        [HttpGet]
        public async Task<IActionResult> GetAllDocList()
        {
            // 1. Parallel Bulk Fetch
            var docListTask = _docMangService.GetAllDocList();
            var docTypeTask = _docMangService.GetAllDocumentType();
            var custRetnTask = _docMangService.GetAllCustRet();
            var companiesTask = _masterService.GetCompanies();
            var masterPartsTask = _masterService.ItemMasterParts();
            var allRoutingsTask = _routingService.AllRoutings();
            var docStatusTask = _docMangService.GetAllDoc_Status_List();

            await Task.WhenAll(
                docListTask,
                docTypeTask,
                custRetnTask,
                companiesTask,
                masterPartsTask,
                allRoutingsTask,
                docStatusTask
            );

            var docListVMs = docListTask.Result.ToList();
            var doctype = docTypeTask.Result.ToList();
            var custRetnDataVMs = custRetnTask.Result.ToList();
            var companies = companiesTask.Result.ToList();
            var manufacturedPartNos = masterPartsTask.Result.ToList();
            var allRoutings = allRoutingsTask.Result.ToList();
            var docStatuses = docStatusTask.Result.ToList();

            // 2. Prepare Efficient Lookups (O(1) Access)

            // Dictionary: DocumentTypeId -> DocumentTypeVM
            var docTypeDict = doctype.ToDictionary(d => d.DocumentTypeId);

            // Dictionary: CompanyId -> CompanyName
            var companyDict = companies.ToDictionary(c => c.CompanyId, c => c.CompanyName);

            // Dictionary: PartId -> MasterPartVM (for PartNo/Desc)
            // GroupBy is used to handle potential duplicates safely
            var partDict = manufacturedPartNos.GroupBy(p => p.PartId).ToDictionary(g => g.Key, g => g.First());

            // Lookup: ManufacturedPartId -> List of Routings
            var routingsByPartId = allRoutings.ToLookup(r => r.ManufacturedPartId);

            // Dictionary: StatusId -> DocStatusDesc
            // Assuming the Status VM has an ID/Code property that matches 'item.AppvStatus'. 
            // Based on usage, I map the ID to the VM or Description.
            // (Adjust 'DocStatusId' to the actual property name in Doc_status_listVM, e.g., 'StatusId' or 'Code')
            var statusDict = docStatuses.GroupBy(s => s.Doc_status_listId).ToDictionary(g => g.Key, g => g.First());


            // Optimization for the Triple-Nested Company Name Loop:
            // Original logic: For a specific DocType, find the associated Company Name via CustRetentionData.
            // We build a map: DocumentTypeId -> CompanyName
            var docTypeToCompanyMap = new Dictionary<long, string>();
            foreach (var cust in custRetnDataVMs)
            {
                // This mimics the original logic: if multiple entries exist, the last one processed 'wins' (or first, depending on list order)
                // We assume one company per doc type relevant context, or simply map available ones.
                if (companyDict.TryGetValue(cust.ComapanyId, out var compName))
                {
                    docTypeToCompanyMap[cust.DocumentTypeId] = compName;
                }
            }

            // 3. Main Loop (Enrichment)
            ClaimsPrincipal userClaim = HttpContext.User;
            string fullName = AppUtil.GetFullName(userClaim);

            foreach (var item in docListVMs)
            {
                // A. Document Type Info
                if (docTypeDict.TryGetValue(item.DocumentTypeId, out var doc))
                {
                    item.DocumentTypeName = doc.DocumentName;
                    item.DataReqdByCust = doc.DataReqdByCust;
                    item.DocCat = doc.DocuCategory;
                }

                // B. Company Name (via pre-calculated map)
                if (docTypeToCompanyMap.TryGetValue(item.DocumentTypeId, out var companyName))
                {
                    item.CompanyName = companyName;
                }

                // C. Part Info (via Dictionary)
                if (partDict.TryGetValue(item.PartId, out var part))
                {
                    item.PartNo = part.PartNo;
                    item.PartDesc = part.Description; // Handle naming diffs if any
                }

                // D. Routing Name (via Lookup)
                // Use lookup to get routings for this part, then filter for Preferred & Matching ID
                var partRoutings = routingsByPartId[item.PartId];
                var route = partRoutings.FirstOrDefault(r => r.PreferredRouting == 1 && r.RoutingId == item.RoutingId);

                if (route != null)
                {
                    item.RoutingName = route.RoutingName;
                }

                // E. Archive Flag
                item.Archive = (item.StorageLocation == "/Archive") ? 'Y' : 'N';

                // F. Audit Info
                item.UpdatedOnStr = item.CreationDt.ToString("MM-dd-yyyy");
                item.UploadedBy = fullName;

                // G. Doc Status (via Dictionary)
                // Replaces await _docMangService.GetDoc_Status_List(item.AppvStatus)
                if (statusDict.TryGetValue(item.AppvStatus, out var statusObj))
                {
                    item.DocStatus = statusObj.Doc_Status_Desc;
                }

                // H. Approval Info
                if (item.DocCat == 1 && item.Approved_by > 0)
                {
                    item.ApprovedOnStr = item.Appv_Date_time.ToString("MM-dd-yyyy");
                    item.ApprovedByStr = fullName; // Note: Original code sets this to current user, is this intended?
                }
                else
                {
                    item.ApprovedOnStr = "";
                    item.ApprovedByStr = "";
                }
            }

            return Ok(docListVMs);
        }
        [HttpGet]
        public async Task<IActionResult> HasPartDrawingPDF(int partId)
        {
            // Get docs only for this part
            var docLists = await _docMangService.GetAllDocList();
            var docList = docLists.Where(d=>d.PartId == partId).ToList();

            // Get document types once
            var docTypes = await _docMangService.GetAllDocumentType();

            // Join document types to names (quick mapping instead of nested foreach)
            var docsWithType = from doc in docList
                               join type in docTypes on doc.DocumentTypeId equals type.DocumentTypeId
                               select new
                               {
                                   doc.FileName,
                                   DocumentTypeName = type.DocumentName
                               };

            // Check if any document meets the criteria
            bool hasPdf = docsWithType.Any(d =>
                d.DocumentTypeName == "Part Drawing" &&
                d.FileName != null &&
                d.FileName.EndsWith(".pdf", StringComparison.OrdinalIgnoreCase)
            );

            return Ok(new { partId, hasPartDrawingPDF = hasPdf });
        }

        [HttpPost]
        public async Task<IActionResult> PostDocList([FromBody] DocListVM docListVM)
        {
            var result = await _docMangService.PostDocList(docListVM);
            return Ok(result);
        }
        [HttpGet]
        public async Task<IActionResult> GetAllCustRet()
        {
            var custRetnDataVMs = await _docMangService.GetAllCustRet();
            var doctype = await _docMangService.GetAllDocumentType();
            var companies = await _masterService.GetCompanies();
            var docViews = await _docMangService.GetAllDocView();
            int noOfFiles = 0;
            foreach (var item in custRetnDataVMs)
            {
                foreach (var doc in doctype)
                {
                    if(item.DocumentTypeId == doc.DocumentTypeId)
                    {
                        item.DocumentTypeName = doc.DocumentName;
                    }
                    foreach (var views in docViews)
                    {
                        if (views.DocumentTypeId == doc.DocumentTypeId)
                        {
                            noOfFiles++;
                        }
                    }
                    item.NoOfFiles = noOfFiles;
                    noOfFiles = 0;
                }
                foreach (var comp in companies)
                {
                    if(item.ComapanyId == comp.CompanyId)
                    {
                        item.CompanyName = comp.CompanyName;
                    }
                }
            }
            return Ok(custRetnDataVMs);
        }


        [HttpPost]
        public async Task<IActionResult> PostCustRetndata([FromBody] CustRetnDataVM custRetnDataVM)
        {
            var result = await _docMangService.PostCustRetndata(custRetnDataVM);
            return Ok(result);
        }


        [HttpPost]
        public async Task<IActionResult> PostDocUpload([FromBody] IEnumerable<DocUploadVM> docUploadVMs)
        {
            var result = await _docMangService.PostDocUpload(docUploadVMs);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> PostDocView([FromBody] IEnumerable<DocViewVM> docViewVMs)
        {
            var result = await _docMangService.PostDocView(docViewVMs);
            return Ok(result);
        }

        public async Task<IActionResult> DeleteDocType(long doctypeId)
        {
            var result = await _docMangService.DeleteDocType(doctypeId);
            return Ok(result);
        }
        public async Task<IActionResult> DeleteCustRetData(long custRetId)
        {

            var result = await _docMangService.DeleteCustRetData(custRetId);
            return Ok(result);
        }

        [HttpGet]
        public async Task<JsonResult> CheckDocTypeName(string docTypeName)
        {
            var result = await _docMangService.CheckDocTypeName(docTypeName);
            return Json(!result);
        }
        [HttpGet]
        public async Task<JsonResult> CheckExtnName(string extnName)
        {
            var result = await _docMangService.CheckExtnName(extnName);
            return Json(!result);
        }
        [HttpGet]
        public async Task<JsonResult> NoFilesExtn(string extn)
        {
            bool result = false;
            var docListVMs = await _docMangService.GetAllDocList();
            foreach (var item in docListVMs)
            {
                if(item.FileName.ToLower().Contains(extn.ToLower()))
                {
                    result = true;
                    return Json(result);
                }
            }
            return Json(result);
        }
        public async Task<IActionResult> DeleteExtnInfo(long extnId)
        {
            var result = await _docMangService.DeleteExtnInfo(extnId);
            return Ok(result);
        }
        [HttpPost]
        public async Task<IActionResult> DeleteDocListAndFile([FromBody] IEnumerable<DocListVM> docListVMs)
        {
            var result = false;
            foreach (var item in docListVMs)
            {

                var filePath = Path.Combine("/filestorage/Active", item.FileName);
                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                    //return Ok("File deleted successfully");
                }
                else
                {
                    // return NotFound("File not found");
                }
                result = await _docMangService.DeleteDocList(item.DocListId);
            }
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllDocViewDepartMent(long docTypeId)
        {
            var docListVMs = await _docMangService.GetAllDocView();
            var filteredDocListVMs = docListVMs.Where(vm => vm.DocumentTypeId == docTypeId);
            return Ok(filteredDocListVMs);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllDocUploadDepartMent(long docTypeId)
        {
            var docListVMs = await _docMangService.GetAllDocUpload();
            var filteredDocListVMs = docListVMs.Where(vm => vm.DocumentTypeId == docTypeId);
            return Ok(filteredDocListVMs);
        }
        [HttpGet]
        public async Task<IActionResult> GetAllRefReson()
        {
            var docListVMs = await _docMangService.Getallreasonlist();
            return Ok(docListVMs);
        }
        [HttpPost]
        public async Task<IActionResult> PostDocReason([FromBody] RefDocReasonListVM custRetnDataVM)
        {
            var result = await _docMangService.PostDocReason(custRetnDataVM);
            return Ok(result);
        }


    }
}
