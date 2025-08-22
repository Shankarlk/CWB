using CWB.CommonUtils.Common;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
using CWB.App.Services.Routings;
using CWB.Constants.UserIdentity;
using Microsoft.AspNetCore.Authorization;

namespace CWB.App.Controllers
{
    [Authorize(Roles = Roles.ADMIN)]
    public class ViewRoutingInfoController : Controller
    {
        private readonly ILogger<ViewRoutingInfoController> _logger;
        private readonly IRoutingService _routingService;
        private readonly IEmployeeService _employeeService;
        private readonly IMastersServices _mastersServices;
        private readonly IMachineService _machineService;
        private readonly IOperationService _operationService;
        private readonly IDocMangService _docMangService;
        public ViewRoutingInfoController(ILogger<ViewRoutingInfoController> logger, IMachineService machineService, IRoutingService routingService, IEmployeeService employeeService,
            IMastersServices mastersServices, IOperationService operationService, IDocMangService docMangService)
        {
            _logger = logger;
            _routingService = routingService;
            _mastersServices = mastersServices;
            _machineService = machineService;
            _operationService = operationService;
            _docMangService = docMangService;
            _employeeService = employeeService;
        }
        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> RoutingDetails(string manufPartId, string partType)
        {
            int decodedManufPartId = (int)CWBAppUtils.DecodeString(manufPartId.ToString());
            var result = await _routingService.GetRoutingListItems();
            var query = from litem in result
                        where litem.ManufacturedPartId == decodedManufPartId
                        select litem;
            RoutingListItemVM routingListItemVM = query.FirstOrDefault();
            if (routingListItemVM == null)
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
                        item.MKPartName = master.PartNo + " / " + master.PartDescription;
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

    }
}
