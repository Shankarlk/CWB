using CWB.Masters.ViewModels.Routing;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CWB.Masters.Services.Routing
{
    public interface IRoutingService
    {
        Task<RoutingVM> Routing(RoutingVM routingVM);
        Task<RoutingStepVM> RoutingStep(RoutingStepVM routingStepVM);
        Task<RoutingStepPartVM> RoutingStepPart(RoutingStepPartVM routingStepPartVM);
        IEnumerable<RoutingVM> GetRoutingsForManufId(int manufId);
        IEnumerable<RoutingStepVM> GetStepsForRoutingId(int routingId);
        IEnumerable<RoutingStepPartVM> GetPartsForStepId(int stepId);
        IEnumerable<RoutingStepPartVM> GetPartsForManufId(int manufID);

        Task<IEnumerable<Domain.Routing>> GetAllRoutings();

    }
}
