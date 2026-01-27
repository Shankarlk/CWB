using CWB.App.AppUtils;
using CWB.App.Models.Contacts;
using CWB.App.Models.DocumentManagement;
using CWB.App.Models.ItemMaster;
using CWB.App.Models.Machine;
using CWB.App.Models.OperationList;
using CWB.App.Models.Routing;
using CWB.App.Models.Routings;
using CWB.App.Services.DocumentMagement;
using CWB.App.Services.EmployeeMaster;
using CWB.App.Services.Masters;
using CWB.App.Services.ProductionPlanWo;
using CWB.App.Services.Routings;
using CWB.CommonUtils.Common;
using CWB.Constants.UserIdentity;
using CWB.Logging;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore.Query.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace CWB.App.Controllers
{
    [Authorize(Roles = Roles.ADMIN)]
    public class RoutingsController : Controller
    {
        private readonly ILoggerManager _logger;
        private readonly IRoutingService _routingService;
        private readonly IEmployeeService _employeeService;
        private readonly IMastersServices _mastersServices;
        private readonly IMachineService _machineService;
        private readonly IOperationService _operationService;
        private readonly IDocMangService _docMangService;
        private readonly IWOService _woService;

        public RoutingsController(ILoggerManager logger, IMachineService machineService, IRoutingService routingService, IEmployeeService employeeService,
            IMastersServices mastersServices, IOperationService operationService, IDocMangService docMangService
            , IWOService woService)
        {
            _logger = logger;
            _routingService = routingService;
            _mastersServices = mastersServices;
            _machineService = machineService;
            _operationService = operationService;
            _docMangService = docMangService;
            _employeeService = employeeService;
            _woService = woService;
        }

        public async Task<IActionResult> Index()
        {
            return View();
        }

        public async Task<IActionResult> RoutingListItems()
        {
            var result = (await _routingService.OptimizedRoutingListItems()).ToList();
            return Json(result);
        }
        public async Task<IActionResult> RoutingListItemss()
        {
            var result = (await _routingService.GetRoutingListItems()).ToList();
            var docListVMs = (await _docMangService.GetAllDocList()).ToList();

            if (!result.Any())
                return Json(result);

            // Step 2: Fetch all routing steps once
            var routingStepsMap = new Dictionary<long, List<RoutingStepVM>>();
            foreach (var item in result)
            {
                var steps = await _routingService.RoutingSteps(item.RoutingId);
                routingStepsMap[item.RoutingId] = steps.ToList();
            }

            // Step 3: Collect all unique StepOperation IDs
            var allStepOps = routingStepsMap.Values
                .SelectMany(x => x)
                .Select(s => Convert.ToInt64(s.StepOperation))
                .Distinct()
                .ToList();

            // Step 4: Prefetch all doc types in one go (if bulk API exists, use it!)
            var opDocsMap = new Dictionary<long, List<OperationalDocumentListVM>>();
            foreach (var opId in allStepOps)
            {
                var opDocs = await _operationService.GetOperationalDocTypesByOptId(opId);
                if (opDocs != null && opDocs.Any())
                    opDocsMap[opId] = opDocs.ToList();
            }

            // Step 5: Convert docListVMs to HashSet for O(1) lookups
            var docSet = new HashSet<(long DocumentTypeId, long RoutingId)>(
                docListVMs.Select(d => (d.DocumentTypeId, d.RoutingId))
            );

            // Step 6: CPU-optimized processing (single pass)
            foreach (var item in result)
            {
                if (!routingStepsMap.TryGetValue(item.RoutingId, out var steps) || steps.Count == 0)
                {
                    item.MandocAvl = "N/A";
                    continue;
                }

                bool anyYes = false, anyNo = false;

                foreach (var op in steps)
                {
                    var opId = Convert.ToInt64(op.StepOperation);

                    if (!opDocsMap.TryGetValue(opId, out var docmand))
                    {
                        item.MandocAvl = "N/A";
                        break; // no docs → no need to check further
                    }

                    // Check only once per doc type
                    foreach (var docMand in docmand)
                    {
                        if (docSet.Contains((docMand.DocumentTypeId, item.RoutingId)))
                        {
                            anyYes = true;
                        }
                        else
                        {
                            anyNo = true;
                        }

                        if (anyYes && anyNo)
                            break; // CPU save: stop scanning early
                    }

                    if (item.MandocAvl == "N/A" || (anyYes && anyNo))
                        break;
                }

                if (item.MandocAvl != "N/A") // only set if not already "N/A"
                {
                    item.MandocAvl = anyYes ? "Yes" : "No";
                }
            }

            return Json(result);
        }
        //GetRoutingListItmes

        [HttpPost]
        public async Task<IActionResult> AddNewRouting(RoutingVM model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _routingService.Routing(model);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> AltRouting(RoutingVM model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _routingService.AltRouting(model);
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> DeleteRouting(int routingId)
        {
            var prodns = await _woService.AllProductionPlan_Wo();
            var prodrt = prodns.FirstOrDefault(p => p.RoutingId == routingId);
            if (prodrt!= null)
            {
                string msg = "This Routing is already used in the WO: "+ prodrt.WONumber + " .";
                return Ok(msg);
            }
            var result = await _routingService.DeleteRouting(routingId);
            return Ok(result);
        }

        //preferredsubcon
        [HttpGet]
        public async Task<IActionResult> PreferredSubCon(int subConDetailsId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _routingService.PreferredSubCon(subConDetailsId);
            return Ok(result);
        }
        [HttpPost]
        public async Task<IActionResult> PreferredRouting(RoutingVM model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _routingService.PreferredRouting(model);
            return Ok(result);
        }

        [HttpPost]
        public string EncodeManufacturedPartId(long manufacturedPartId)
        {
            return CWBAppUtils.EncodeLong(manufacturedPartId);
        }

        //[Route("~/Ro#)&!@237$&")]
        public async Task<IActionResult> RoutingDetails(string manufPartId,string partType)
        {
            int decodedManufPartId =(int)CWBAppUtils.DecodeString(manufPartId.ToString());
            var result = await _routingService.GetRoutingListItems();
            var query = from litem in result
                        where litem.ManufacturedPartId == decodedManufPartId
                        select litem;
            RoutingListItemVM routingListItemVM = query.FirstOrDefault();
            if(routingListItemVM == null)
            {
                routingListItemVM = new RoutingListItemVM();
                routingListItemVM.MasterPartType = partType;
                routingListItemVM.RoutingVMs = new List<RoutingVM>();
            }
            else
            {
                var resultList = await _routingService.Routings(decodedManufPartId);
                foreach (var item in resultList)
                {
                    var master = await _mastersServices.ItemMasterPartById((int)item.MKPartId);
                    var oprnos = await _routingService.RoutingSteps(item.RoutingId);
                    if (master != null)
                    {
                        item.MKPartName = master.PartNo +" / "+master.PartDescription;
                    }
                    item.NoOprns = oprnos.Count();
                    foreach (var op in oprnos)
                    {
                        var docmand = await _operationService.GetOperationalDocTypesByOptId(Convert.ToInt64(op.StepOperation));
                        if (docmand.Any())
                        {
                            var docListVMs = await _docMangService.GetAllDocList();
                            if (docListVMs.Any(docList => docmand.Any(docMand => docList.DocumentTypeId == docMand.DocumentTypeId && docList.RoutingId == item.RoutingId)))
                            {
                                item.MandocAvl = "Yes";
                            }
                            else
                            {
                                item.MandocAvl = "No";
                            }
                        }
                        else
                        {
                            item.MandocAvl = "N/A";
                        }
                    }
                }
                routingListItemVM.MasterPartType = partType;
                routingListItemVM.RoutingVMs = resultList.ToList();
            }
            return View(routingListItemVM);
        }

        [HttpPost]
        public async Task<IActionResult> CreateAltRouting(RoutingVM model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _routingService.Routing(model);
            return Ok(result);
        }

        public IActionResult AddNextStep()
        {
            return Ok("Ok");
        }

        [HttpPost]
        public async Task<IActionResult> SaveStep(RoutingStepVM model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _routingService.RoutingStep(model);
            return Ok(result);
        }

       
        public async Task<IActionResult> DelStep(int stepId)
        {
            var stepMachines = await _routingService.StepMachines(stepId);
            if (stepMachines != null && stepMachines.Any())
            {
                return Ok("This Step is already used in the Machines. Please delete them first.");
            }

            // 2. Dependency: Check for Parts configured on this step
            var stepParts = await _routingService.StepParts(stepId);
            if (stepParts != null && stepParts.Any())
            {
                return Ok("This Step is already used in the StepParts. Please delete them first.");
            }

            // 3. Dependency: Check for SubCons (if applicable location)
            var subCons = await _routingService.SubCons(stepId);
            if (subCons != null && subCons.Any())
            {
                return Ok("This Step is already used in the SubCon. Please delete them first.");
            }
            var prodns = await _woService.AllProductionPlan_Wo();
            var prodrt = prodns.FirstOrDefault(p => p.StartingOpNo == stepId || p.EndingOpNo == stepId);
            if (prodrt != null)
            {
                string msg = "This Routing is already used in the WO: " + prodrt.WONumber + " .";
                return Ok(msg);
            }
            var result = await _routingService.DeleteStep(stepId);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> AddBomToAssembly(RoutingStepPartVM model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _routingService.RoutingStepPart(model);
            return Ok(result);
        }

        public async Task<IActionResult> RoutingSteps(int routingId)
        {
            var result = await _routingService.RoutingSteps(routingId);
            foreach (var item in result)
            {
                int mcminutes = 0;
                int subminutes = 0;
                int mcsetminutes = 0;
                int subsetminutes = 0;
                if (item.StepLocation == "1")
                {
                    item.LocationName = "Inhouse";
                }else if (item.StepLocation == "2")
                {
                    item.LocationName = "SubCon";
                }
                else
                {
                    item.LocationName = "Company";
                }
                var stepmc = await _routingService.StepMachines((int)item.StepId);

                var preferredMcs = stepmc
                    .OrderByDescending(m => m.PreferredMachine == 1)
                    .Take(item.NumberOfSimMachines)
                    .ToList();

                if (preferredMcs.Any())
                {
                    item.CycleTime = preferredMcs
                        .Sum(m => ToMinutes(m.FloorToFloorTime))
                        .ToString();

                    item.SetupTime = preferredMcs
                        .Sum(m => ToMinutes(m.SetupTime))
                        .ToString();

                    item.FirstPieceTime = preferredMcs
                        .Sum(m => ToMinutes(m.FirstPieceProcessingTime))
                        .ToString();
                }
                else
                {
                    item.CycleTime = "";
                    item.SetupTime = "";
                }

                var stepsubcon =await _routingService.SubCons((int)item.StepId);
                if(stepsubcon.Count() != 0)
                {
                    foreach (var sub in stepsubcon)
                    {
                        //if (sub.PreferredSubcon == 1)
                        //{
                            var subworkdetails = await _routingService.SubConWSS((int)item.StepId, sub.SubConDetailsId);
                            if(subworkdetails.Count() !=0)
                            {
                                foreach (var mc in subworkdetails)
                                {
                                    TimeSpan time = TimeSpan.Parse(mc.FloorToFloorTime);
                                    subminutes = (int)time.TotalMinutes;
                                    TimeSpan Settime = TimeSpan.Parse(mc.SetupTime);
                                    subsetminutes = (int)Settime.TotalMinutes;
                                }
                            }
                        //}
                    }
                    item.CycleTime = subminutes.ToString();
                    item.SetupTime = subsetminutes.ToString();
                }
                if(item.SetupTime == null)
                {
                    item.SetupTime = "";
                    item.CycleTime = "";
                }
                var stepparts = await _routingService.StepParts((int)item.StepId);
                item.NoOfPartsUsed = Convert.ToString((int)stepparts.Sum(op => op.QuantityAssembly));

                var docmand = await _operationService.GetOperationalDocTypesByOptId(Convert.ToInt64(item.StepOperation));
                if (docmand.Any())
                {
                    var docListVMs = await _docMangService.GetAllDocList();
                    if (docListVMs.Any(docList => docmand.Any(docMand => docList.DocumentTypeId == docMand.DocumentTypeId && docList.RoutingId==routingId)))
                    {
                        item.MandocAvl = "Yes";
                    }
                    else
                    {
                        item.MandocAvl = "No";
                    }
                }
                else
                {
                    item.MandocAvl = "N/A";
                }
            }
            
            return Ok(result);
        }
        private static double ToMinutes(string hhmmss)
        {
            if (string.IsNullOrWhiteSpace(hhmmss))
                return 0;

            return TimeSpan.Parse(hhmmss).TotalMinutes;
        }

        [HttpGet]
        public async Task<IActionResult> GetOpertaionDocList(long opId,long routingId,long stepId)
        {
            var result = await _operationService.GetOperationalDocTypesByOptId(opId);
            var docListVMs = await _docMangService.GetAllDocList();
            var doctype = await _docMangService.GetAllDocumentType();
            var custRetnDataVMs = await _docMangService.GetAllCustRet();
            var companies = await _mastersServices.GetCompanies();
            var extns = await _docMangService.GetAllFileExtn();

            List<DocListVM> docListVM = new List<DocListVM>();
            List<DocListVM> empdocLists = new List<DocListVM>();

            if (stepId == 0)
            {
                foreach (var master in result.Where(m => m.OperationListId == opId))
                {
                    DocListVM empDoc = new DocListVM();
                    PopulateItemFields(empDoc, master, doctype, extns, custRetnDataVMs, companies);
                    empDoc.Comments = string.Empty;
                    empDoc.DeletionDate = DateTime.Now;
                    empdocLists.Add(empDoc);
                }
                return Ok(empdocLists);
            }
            else
            {
                // When partid != 0, show docListVMs related to contentid and partid
                bool hasEntries = false;
                foreach (var item in docListVMs.Where(d => d.McId == 0 && d.McTypeId == 0 && d.RoutingId ==routingId && d.OprNo==stepId && result.Any(m => m.OperationListId == opId && m.DocumentTypeId == d.DocumentTypeId)))
                {
                    PopulateItemFields(item, result.First(m => m.OperationListId == opId && m.DocumentTypeId == item.DocumentTypeId), doctype, extns, custRetnDataVMs, companies);
                    ClaimsPrincipal userClaim = HttpContext.User; // Assuming you're in a controller or middleware
                    string fullName = AppUtil.GetFullName(userClaim);
                    item.UpdatedOnStr = item.CreationDt.ToString("MM-dd-yyyy");
                    item.UploadedBy = fullName;

                    docListVM.Add(item);
                    hasEntries = true;
                }

                // If there are document types related to contentid without entries in docListVMs, add to empdocLists
                if (!hasEntries || docListVM.Count < result.Count(m => m.OperationListId == opId))
                {
                    foreach (var master in result.Where(m => m.OperationListId == opId && !docListVM.Any(d => d.DocumentTypeId == m.DocumentTypeId)))
                    {
                        DocListVM empDoc = new DocListVM();
                        PopulateItemFields(empDoc, master, doctype, extns, custRetnDataVMs, companies);
                        empDoc.DeletionDate = DateTime.Now;
                        empDoc.Comments = string.Empty;
                        empdocLists.Add(empDoc);
                    }
                }

                // Return combination of docListVM and empdocLists when partid != 0
                return Ok(docListVM.Concat(empdocLists).ToList());
            }
        }

        void PopulateItemFields(DocListVM item, OperationalDocumentListVM master, IEnumerable<DocumentTypeVM> doctype,
                                IEnumerable<ExtnInfoVM> extns, IEnumerable<CustRetnDataVM> custRetnDataVMs,
                                IEnumerable<ContactsVM> companies)
        {
            var doc = doctype.FirstOrDefault(d => d.DocumentTypeId == master.DocumentTypeId);
            if (doc != null)
            {
                item.DocumentTypeName = doc.DocumentName;
                item.DocumentTypeId = doc.DocumentTypeId;
                item.DataReqdByCust = doc.DataReqdByCust;
                item.DocCat = doc.DocuCategory;
                item.Mandatory = (master.IsMandatory) ? 'Y' : 'N';
                var extn = extns.FirstOrDefault(e => e.ExtnId == doc.ExtnId);
                item.FileExtnName = extn?.ExtnName;

                // Calculate DeletionDate if required
                if (doc.DefaultRetPerYear != 0 && doc.DefaultRetPerMon != 0)
                {
                    DateTime currentDate = DateTime.Now;
                    DateTime futureDate = currentDate.AddYears(doc.DefaultRetPerYear).AddMonths(doc.DefaultRetPerMon);
                    item.DeletionDate = futureDate;
                }
            }

            var cust = custRetnDataVMs.FirstOrDefault(c => c.DocumentTypeId == master.DocumentTypeId);
            if (cust != null)
            {
                var comp = companies.FirstOrDefault(c => c.CompanyId == cust.ComapanyId);
                item.CompanyName = comp?.CompanyName;
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetMcTypeDocList(long mcTypeId, long routingId, long stepId)
        {
            var result = await _machineService.GetMcTypeDocList();
            var docListVMs = await _docMangService.GetAllDocList();
            var doctype = await _docMangService.GetAllDocumentType();
            var custRetnDataVMs = await _docMangService.GetAllCustRet();
            var companies = await _mastersServices.GetCompanies();
            var extns = await _docMangService.GetAllFileExtn();

            List<DocListVM> docListVM = new List<DocListVM>();
            List<DocListVM> empdocLists = new List<DocListVM>();

            if (stepId == 0)
            {
                foreach (var master in result.Where(m => m.McTypeId == mcTypeId))
                {
                    DocListVM empDoc = new DocListVM();
                    PopulateMcTypeFields(empDoc, master, doctype, extns, custRetnDataVMs, companies);
                    empDoc.Comments = string.Empty;
                    empDoc.DeletionDate = DateTime.Now;
                    empdocLists.Add(empDoc);
                }
                return Ok(empdocLists);
            }
            else
            {
                // When partid != 0, show docListVMs related to contentid and partid
                bool hasEntries = false;
                foreach (var item in docListVMs.Where(d => d.McTypeId == mcTypeId && d.RoutingId == routingId && d.OprNo == stepId && result.Any(m => m.McTypeId == mcTypeId && m.DocumentTypeId == d.DocumentTypeId)))
                {
                    PopulateMcTypeFields(item, result.First(m => m.McTypeId == mcTypeId && m.DocumentTypeId == item.DocumentTypeId), doctype, extns, custRetnDataVMs, companies);
                    ClaimsPrincipal userClaim = HttpContext.User; // Assuming you're in a controller or middleware
                    string fullName = AppUtil.GetFullName(userClaim);
                    item.UpdatedOnStr = item.CreationDt.ToString("MM-dd-yyyy");
                    item.UploadedBy = fullName;

                    docListVM.Add(item);
                    hasEntries = true;
                }

                // If there are document types related to contentid without entries in docListVMs, add to empdocLists
                if (!hasEntries || docListVM.Count < result.Count(m => m.McTypeId == mcTypeId))
                {
                    foreach (var master in result.Where(m => m.McTypeId == mcTypeId && !docListVM.Any(d => d.DocumentTypeId == m.DocumentTypeId)))
                    {
                        DocListVM empDoc = new DocListVM();
                        PopulateMcTypeFields(empDoc, master, doctype, extns, custRetnDataVMs, companies);
                        empDoc.DeletionDate = DateTime.Now;
                        empDoc.Comments = string.Empty;
                        empdocLists.Add(empDoc);
                    }
                }

                // Return combination of docListVM and empdocLists when partid != 0
                return Ok(docListVM.Concat(empdocLists).ToList());
            }
        }

        void PopulateMcTypeFields(DocListVM item, McTypeDocListVM master, IEnumerable<DocumentTypeVM> doctype,
                               IEnumerable<ExtnInfoVM> extns, IEnumerable<CustRetnDataVM> custRetnDataVMs,
                               IEnumerable<ContactsVM> companies)
        {
            var doc = doctype.FirstOrDefault(d => d.DocumentTypeId == master.DocumentTypeId);
            if (doc != null)
            {
                item.DocumentTypeName = doc.DocumentName;
                item.DocumentTypeId = doc.DocumentTypeId;
                item.DataReqdByCust = doc.DataReqdByCust;
                item.DocCat = doc.DocuCategory;
                item.Mandatory = master.Mandatory;
                var extn = extns.FirstOrDefault(e => e.ExtnId == doc.ExtnId);
                item.FileExtnName = extn?.ExtnName;

                // Calculate DeletionDate if required
                if (doc.DefaultRetPerYear != 0 && doc.DefaultRetPerMon != 0)
                {
                    DateTime currentDate = DateTime.Now;
                    DateTime futureDate = currentDate.AddYears(doc.DefaultRetPerYear).AddMonths(doc.DefaultRetPerMon);
                    item.DeletionDate = futureDate;
                }
            }

            var cust = custRetnDataVMs.FirstOrDefault(c => c.DocumentTypeId == master.DocumentTypeId);
            if (cust != null)
            {
                var comp = companies.FirstOrDefault(c => c.CompanyId == cust.ComapanyId);
                item.CompanyName = comp?.CompanyName;
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetMcIdDocList(long mcId, long routingId, long stepId)
        {
            var result = await _machineService.GetMcProcDocList();
            var docListVMs = await _docMangService.GetAllDocList();
            var doctype = await _docMangService.GetAllDocumentType();
            var custRetnDataVMs = await _docMangService.GetAllCustRet();
            var companies = await _mastersServices.GetCompanies();
            var extns = await _docMangService.GetAllFileExtn();

            List<DocListVM> docListVM = new List<DocListVM>();
            List<DocListVM> empdocLists = new List<DocListVM>();

            if (stepId == 0)
            {
                foreach (var master in result.Where(m => m.McId == mcId))
                {
                    DocListVM empDoc = new DocListVM();
                    PopulateMcFields(empDoc, master, doctype, extns, custRetnDataVMs, companies);
                    empDoc.Comments = string.Empty;
                    empDoc.DeletionDate = DateTime.Now;
                    empdocLists.Add(empDoc);
                }
                return Ok(empdocLists);
            }
            else
            {
                // When partid != 0, show docListVMs related to contentid and partid
                bool hasEntries = false;
                foreach (var item in docListVMs.Where(d => d.McId == mcId && d.McTypeId == 0 && d.RoutingId == routingId && d.OprNo == stepId && result.Any(m => m.McId == mcId && m.DocumentTypeId == d.DocumentTypeId)))
                {
                    PopulateMcFields(item, result.First(m => m.McId == mcId && m.DocumentTypeId == item.DocumentTypeId), doctype, extns, custRetnDataVMs, companies);
                    ClaimsPrincipal userClaim = HttpContext.User; // Assuming you're in a controller or middleware
                    string fullName = AppUtil.GetFullName(userClaim);
                    item.UpdatedOnStr = item.CreationDt.ToString("MM-dd-yyyy");
                    item.UploadedBy = fullName;

                    docListVM.Add(item);
                    hasEntries = true;
                }

                // If there are document types related to contentid without entries in docListVMs, add to empdocLists
                if (!hasEntries || docListVM.Count < result.Count(m => m.McId == mcId))
                {
                    foreach (var master in result.Where(m => m.McId == mcId && !docListVM.Any(d => d.DocumentTypeId == m.DocumentTypeId)))
                    {
                        DocListVM empDoc = new DocListVM();
                        PopulateMcFields(empDoc, master, doctype, extns, custRetnDataVMs, companies);
                        empDoc.DeletionDate = DateTime.Now;
                        empDoc.Comments = string.Empty;
                        empdocLists.Add(empDoc);
                    }
                }

                // Return combination of docListVM and empdocLists when partid != 0
                return Ok(docListVM.Concat(empdocLists).ToList());
            }
        }
        void PopulateMcFields(DocListVM item, McSlNoDocListVM master, IEnumerable<DocumentTypeVM> doctype,
                              IEnumerable<ExtnInfoVM> extns, IEnumerable<CustRetnDataVM> custRetnDataVMs,
                              IEnumerable<ContactsVM> companies)
        {
            var doc = doctype.FirstOrDefault(d => d.DocumentTypeId == master.DocumentTypeId);
            if (doc != null)
            {
                item.DocumentTypeName = doc.DocumentName;
                item.DocumentTypeId = doc.DocumentTypeId;
                item.DataReqdByCust = doc.DataReqdByCust;
                item.DocCat = doc.DocuCategory;
                item.Mandatory = master.Mandatory;
                var extn = extns.FirstOrDefault(e => e.ExtnId == doc.ExtnId);
                item.FileExtnName = extn?.ExtnName;

                // Calculate DeletionDate if required
                if (doc.DefaultRetPerYear != 0 && doc.DefaultRetPerMon != 0)
                {
                    DateTime currentDate = DateTime.Now;
                    DateTime futureDate = currentDate.AddYears(doc.DefaultRetPerYear).AddMonths(doc.DefaultRetPerMon);
                    item.DeletionDate = futureDate;
                }
            }

            var cust = custRetnDataVMs.FirstOrDefault(c => c.DocumentTypeId == master.DocumentTypeId);
            if (cust != null)
            {
                var comp = companies.FirstOrDefault(c => c.CompanyId == cust.ComapanyId);
                item.CompanyName = comp?.CompanyName;
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetRoutingStatusLog(long routingId)
        {
            var result = await _routingService.GetRoutingStatusLog(routingId);
            foreach (var item in result)
            {
                item.DateStr = item.UpdatedDate.ToString("MM-dd-yyyy");
                ClaimsPrincipal userClaim = HttpContext.User; // Assuming you're in a controller or middleware
                string fullName = AppUtil.GetFullName(userClaim);
                item.UpdatedByStr = fullName;
            }
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetMasterName(string ManufId)
        {
            int decodedManufPartId = (int)CWBAppUtils.DecodeString(ManufId);
            var master = await _mastersServices.ItemMasterPartById(decodedManufPartId);
            return Ok(master);
        }

        public async Task<IActionResult> StepParts(int stepId)
        {
            var result = await _routingService.StepParts(stepId);
            return Ok(result);
        }


        public async Task<IActionResult> BOMs(string manufId, int stepId)
        {
            int decodedManufPartId = (int)CWBAppUtils.DecodeString(manufId);
            var boms = await _mastersServices.BOMS(""+ decodedManufPartId);
            var manfsps = await _routingService.StepPartsByManufId(decodedManufPartId);
            var stepparts = await _routingService.StepParts(stepId);

            var query = from bom in boms
                        select new StepBOMVM
                        {
                            BOMPartNo = bom.BOMPartNo,
                            BOMManufPartId = bom.BOMManufPartId,
                            BOMPartId = bom.BOMPartId,
                            BOMPartDesc = bom.BOMPartDesc,
                            MPBOMId = bom.MPBOMId,
                            Quantity = bom.Quantity,
                            QuantityUsed = 0,
                            BalanceQuantity = bom.Quantity,
                            StepPartId = -1,
                            RoutingStepId = -1
                        };
            List<StepBOMVM> lst = query.ToList();
            //Calculate balance quantity for boms
            try
            {
                foreach (RoutingStepPartVM sp in manfsps)
                {
                    foreach (StepBOMVM spbom in lst)
                    {
                        if (spbom != null && sp != null)
                        {
                            if (spbom.MPBOMId == sp.BOMId)
                            {
                                spbom.QuantityUsed += sp.QuantityAssembly;
                                spbom.BalanceQuantity = spbom.Quantity - spbom.QuantityUsed;
                            }
                        }
                    }
                }
            }
            catch (Exception ex) {
                string msg = ex.InnerException.Message;
                string src = ex.InnerException.Source;
            }
            //reset quantity used on BOM... just for display purpoo
            foreach (StepBOMVM spbom in lst)
            {
                spbom.QuantityUsed = 0;
            }
            List<StepBOMVM> list1 = new List<StepBOMVM> ();
            foreach (RoutingStepPartVM sp in stepparts)
            {
                foreach (StepBOMVM bom in lst)
                {
                    if (bom.MPBOMId == sp.BOMId)
                    {
                        StepBOMVM obj = new StepBOMVM {
                            BOMPartNo = bom.BOMPartNo,
                            BOMManufPartId = bom.BOMManufPartId,
                            BOMPartId = bom.BOMPartId,
                            BOMPartDesc = bom.BOMPartDesc,
                            MPBOMId = bom.MPBOMId,
                            Quantity = 0,
                            QuantityUsed = sp.QuantityAssembly,
                            BalanceQuantity = 0,
                            StepPartId = (int)sp.StepPartId,
                            RoutingStepId = (int)sp.RoutingStepId
                        };
                        list1.Add(obj);
                        break;
                    }
                }
            }
            lst.AddRange(list1);
            return Ok(lst);
        }

        public async Task<IActionResult> StepBOMs(int stepId)
        {
            var result = await _routingService.StepParts(stepId);
            return Ok(result);
        }



        public IActionResult   Routings(int manufPartId)
        {
            return Ok("hello");
        }



        [HttpPost]
        public async Task<IActionResult> SaveStepSupplier(RoutingStepSupplierVM model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _routingService.RoutingStepSupplier(model);
            return Ok(result);
        }
        [HttpPost]
        public async Task<IActionResult> SaveStepMachine(RoutingStepMachineVM model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _routingService.RoutingStepMachine(model);
            return Ok(result);
        }

        public async Task<IActionResult> PreferredStepMachine(string routingStepId, string routingStepMachineId, int maxMachineCount)
        {
            var result = await _routingService.PreferredStepMachine(routingStepId, routingStepMachineId,maxMachineCount);
            return Ok(result);
        }

        public async Task<IActionResult> StepSuppliers(int stepId)
        {
            var companies = await _mastersServices.GetCompanies();
            var result = await _routingService.StepSuppliers(stepId);
            foreach (RoutingStepSupplierVM obj in result)
            {
                foreach (ContactsVM mobj in companies)
                {
                    if (obj.SupplierId == mobj.CompanyId)
                    {
                        obj.Name = mobj.CompanyName;
                    }
                }
            }
            return Ok(result);
        }
        

        public async Task<IActionResult> StepMachines(int stepId)
        {
            var machines = await _machineService.GetMachinesList();
            var result = await _routingService.StepMachines(stepId);
            foreach (RoutingStepMachineVM obj in result)
            {
                foreach (Models.Machine.MachineListVM mobj in machines)
                {
                    if(obj.MachineId == mobj.MachineId)
                    {
                        obj.Name = mobj.Name;
                    }
                }
                var docmand = await _machineService.GetMcProcDocList();
                if (docmand.Any(d => d.McId == obj.MachineId))
                {
                    var docListVMs = await _docMangService.GetAllDocList();
                    if (docListVMs.Any(docList => docmand.Any(docMand => docList.DocumentTypeId == docMand.DocumentTypeId)))
                    {
                        obj.MandocAvl = "Yes";
                    }
                    else
                    {
                        obj.MandocAvl = "No";
                    }
                }
                else
                {
                    obj.MandocAvl = "N/A";
                }
            }
            return Ok(result);
        }

        [HttpGet]
        public IActionResult Locations()
        {
            List<LocationVM> locations = new List<LocationVM>();
            locations.Add(new LocationVM { Id=1,Name= "Inhouse" });
            locations.Add(new LocationVM { Id = 2, Name = "SubCon" });
            locations.Add(new LocationVM { Id = 3, Name = "Company" });
            return Ok(locations);
        }


        public async Task<IActionResult> DeleteStep(int stepId)
        {
            var machines = await _machineService.GetMachinesList();
            var result = await _routingService.DeleteStep(stepId);
            return Ok(result);
        }

        public async Task<IActionResult> DeleteMachine(int stepId,int machineId)
        {
            var result = await _routingService.DeleteMachine(stepId,machineId);
            return Ok(result);
        }
        public async Task<IActionResult> DeleteStepSupplier(int stepId, int supplierId)
        {
            var result = await _routingService.DeleteSupplier(stepId,supplierId);
            return Ok(result);
        }





        public async Task<IActionResult> SubCons(int stepId)
        {
            var companies = await _mastersServices.GetCompanies();
            var subconwss = await _routingService.SubConWSS(stepId,-1);
            var result = await _routingService.SubCons(stepId);

            foreach (SubConDetailsVM obj in result)
            {
                obj.StrPreferredSubCon = (obj.PreferredSubcon==1)?"Yes":"";
            }

            foreach (SubConDetailsVM obj in result)
            {
                foreach (ContactsVM mobj in companies)
                {
                    if (obj.SupplierId == mobj.CompanyId)
                    {
                        obj.Company = mobj.CompanyName;
                    }
                }
            }
            foreach (SubConDetailsVM obj in result)
            {
                foreach (SubConWorkStepDetailsVM mobj in subconwss)
                {
                    if (obj.SubConDetailsId == mobj.SubConDetailsId)
                    {
                        obj.NoOfOperations++;
                    }

                    var docmand = await _machineService.GetMcTypeDocList();
                    if (docmand.Any(d => d.McTypeId == mobj.MachineType))
                    {
                        var docListVMs = await _docMangService.GetAllDocList();
                        if (docListVMs.Any(docList => docmand.Any(docMand => docList.DocumentTypeId == docMand.DocumentTypeId)))
                        {
                            obj.MandocAvl = "Yes";
                        }
                        else
                        {
                            obj.MandocAvl = "No";
                        }
                    }
                    else
                    {
                        obj.MandocAvl = "N/A";
                    }
                }
            }
            return Ok(result);
        }

        public async Task<IActionResult> AllSubConWSS(int stepId)
        {
            return await SubConWSS(stepId, -1);
        }
        public async Task<IActionResult> SubConWSS(int stepId,int subConDetailsId)
        {
            var companies = await _mastersServices.GetCompanies();
            var result = await _routingService.SubConWSS(stepId,subConDetailsId);
            var machines = await _machineService.GetMachineTypes();
            foreach (var item in result)
            {
                foreach (var mc in machines)
                {
                    if(mc.MachineTypeTypeId== item.MachineType)
                    {
                        item.MachineNameStr = mc.MachineTypeName;
                    }
                }
            }
            /*foreach (SubConDetailsVM obj in result)
            {
                foreach (ContactsVM mobj in companies)
                {
                    if (obj.SupplierId == mobj.CompanyId)
                    {
                        obj.Company = mobj.CompanyName;
                    }
                }
            }*/
            return Ok(result);
        }

        
        public async Task<IActionResult> DeleteSubConDetails(int stepId, int subConDetailsId)
        {
            var result = await _routingService.DeleteSubCon(stepId, subConDetailsId);
            return Ok(result);
        }

        //SubCons/SubConWSS/DeleteSubConDetails/DeleteSubConWS
        public async Task<IActionResult> DeleteSubConWS(int stepId, int subConWSDetailsId)
        {
            var result = await _routingService.DeleteSubConWS(stepId, subConWSDetailsId);
            return Ok(result);
        }

        
        [HttpPost]
        public async Task<IActionResult> AddSubCon(SubConDetailsVM model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _routingService.SubConDetails(model);
            return Ok(result);
        }
        //SubCons/SubConWSS/DeleteSubConDetails/DeleteSubConWS/AddSubCon//AddSubConWS
        [HttpPost]
        public async Task<IActionResult> AddSubConWS(SubConWorkStepDetailsVM model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _routingService.SubConWSDetails(model);
            return Ok(result);
        }


        [HttpGet]
        public async Task<IActionResult> DeleteWS(int subConWSId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _routingService.DeleteWS(subConWSId);
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> DeleteStepPart(int stepId,int stepPartId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _routingService.DeleteStepPart(stepId, stepPartId);
            return Ok(result);
        }
        [HttpPost]
        public long DecodePartId(string partId)
        {
            long decodepartID = 0;
            if (partId != null && partId != "0")
            {
                decodepartID = CWBAppUtils.DecodeString(partId);
            }
            return decodepartID;
        }

        [HttpGet]
        public async Task<IActionResult> RoutingPerformance(string manufPartId,int batchSize)
        {
            int decodedManufPartId = Convert.ToInt32(manufPartId.ToString());
           
            var resultList = await _routingService.Routings(decodedManufPartId);

            foreach (var item in resultList)
            {
                if (item.Deleted != 1)
                {
                    int inhousecount = 0;
                    int subconcount = 0;
                    var result = await RoutingSteps(item.RoutingId);
                    var oprnos = (List<RoutingStepVM>)((OkObjectResult)result).Value;
                    foreach (var op in oprnos)
                    {
                        if (op.StepLocation == "1")
                        {
                            inhousecount++;
                        }
                        else if (op.StepLocation == "2")
                        {
                            subconcount++;
                        }
                    }
                    var totalnoofmc = oprnos.Sum(op => op.NumberOfSimMachines);
                    var totalCycleTime = oprnos.Where(op => !string.IsNullOrEmpty(op.CycleTime)).Sum(op => int.Parse(op.CycleTime));

                    int noofoperations = 0;

                    if(oprnos.Count==0)
                    {
                        noofoperations = 1;
                    }
                    else
                    {
                        noofoperations = oprnos.Count;
                    }
                    var avgCycleTime = totalCycleTime / noofoperations;
                    var countAboveAvg = oprnos.Where(op => !string.IsNullOrEmpty(op.CycleTime)).Count(op => int.Parse(op.CycleTime) > avgCycleTime);
                    var totalSetupTime = oprnos.Where(op => !string.IsNullOrEmpty(op.SetupTime)).Sum(op => int.Parse(op.SetupTime));
                    var maxSetupTime = oprnos.Where(op => !string.IsNullOrEmpty(op.SetupTime))
        .Select(op => int.Parse(op.SetupTime))
        .DefaultIfEmpty(0)
        .Max();
                    double batchSizeManfTimeMinutes = 0;

                    foreach (var op in oprnos)
                    {
                        if (string.IsNullOrWhiteSpace(op.CycleTime) ||
                            string.IsNullOrWhiteSpace(op.SetupTime) ||
                            string.IsNullOrWhiteSpace(op.FirstPieceTime) ||
                            op.NumberOfSimMachines <= 0)
                            continue;

                        double cycleMin = double.Parse(op.CycleTime);
                        double firstPieceMin = double.Parse(op.FirstPieceTime);
                        double setupMin = double.Parse(op.SetupTime);
                        int simMc = op.NumberOfSimMachines;

                        double batchPerMachine = (double)batchSize / simMc;

                        double perMachineTime =
                            setupMin +
                            firstPieceMin +
                            (batchPerMachine - 1) * cycleMin;

                        batchSizeManfTimeMinutes += perMachineTime;
                    }

                    item.MaxSetupTime = maxSetupTime;
                    item.TotalSetupTime = totalSetupTime;
                    item.AvgCycleTime = avgCycleTime;
                    item.OprnGreaterAvgCycleTime = countAboveAvg;
                    item.InhouseNo = inhousecount;
                    item.SubconNo = subconcount;
                    item.BacthManufTime = Math.Round(batchSizeManfTimeMinutes / 60, 2);
                }
            }
            return Ok(resultList);
        }

        [HttpPost]
        public async Task<IActionResult> ChangeRoutingStepSequence([FromBody]IEnumerable<RoutingStepVM> routingStepVMs)
        {
            var result = await _routingService.ChangeRoutingStepSequence(routingStepVMs);
            return Ok();
        }
            //public const string DeleteSubConDetails = Base + "/deletesubcondetails/{stepId}/{subConDetailsId}";
            //public const string DeleteSubWSConDetails = Base + "/deletesubconworkstepdetails/{stepId}/{subConWSDetailsId}";

    }
}
