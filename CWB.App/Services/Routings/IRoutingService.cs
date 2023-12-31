using CWB.App.Models.Contacts;
using CWB.App.Models.ItemMaster;
using CWB.App.Models.Routing;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CWB.App.Services.Routings
{
    public interface IRoutingService
    {
        Task<List<RoutingListItemVM>> GetRoutingListItems();
        Task<RoutingVM> Routing(RoutingVM routingVM);
        Task<RoutingStepVM> RoutingStep(RoutingStepVM routingStepVM);
        Task<RoutingStepPartVM> RoutingStepPart(RoutingStepPartVM routingStepPartVM);
        Task<RoutingStepSupplierVM> RoutingStepSupplier(RoutingStepSupplierVM routingStepSupplierVM);
        Task<RoutingStepMachineVM> RoutingStepMachine(RoutingStepMachineVM routingStepMachineVM);

        Task<IEnumerable<RoutingStepVM>> RoutingSteps(int routingId);
        Task<IEnumerable<RoutingVM>> Routings(int manufPartId);
        
        Task<IEnumerable<RoutingStepPartVM>> StepParts(int stepId);
        Task<IEnumerable<RoutingStepPartVM>> StepPartsByManufId(int manufId);
        Task<IEnumerable<RoutingStepSupplierVM>> StepSuppliers(int stepId);
        Task<IEnumerable<RoutingStepMachineVM>> StepMachines(int stepId);

    }
}
