using CWB.Masters.ViewModels.Routings;
using CWB.Masters.Domain.Routings;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CWB.Masters.Services.Routings
{
    public interface IRoutingService
    {
        Task<RoutingVM> Routing(RoutingVM routingVM);
        Task<RoutingStepVM> RoutingStep(RoutingStepVM routingStepVM);
        Task<RoutingStepPartVM> RoutingStepPart(RoutingStepPartVM routingStepPartVM);
        IEnumerable<RoutingVM> GetRoutingsForManufId(int manufId);
        IEnumerable<RoutingStepVM> GetStepsForRoutingId(int routingId);
        IEnumerable<RoutingStepPartVM> GetPartsForStepId(int stepId);
        Task<bool> DelStep(int stepId);
        Task<RoutingStepVM> GetStep(int stepId);
        IEnumerable<RoutingStepPartVM> GetPartsForManufId(int manufID);
        Task<IEnumerable<Routing>> GetAllRoutings();

        Task<IEnumerable<RoutingStepMachineVM>> StepMachines(int stepId);
        Task<IEnumerable<RoutingStepSupplierVM>> StepSuppliers(int stepId);
        Task<RoutingStepMachineVM> RoutingStepMachine(RoutingStepMachineVM routingStepMachineVM);
        Task<RoutingStepSupplierVM> RoutingStepSupplier(RoutingStepSupplierVM routingStepSupplierVM);

    }
}
