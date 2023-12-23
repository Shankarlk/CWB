using AutoMapper;
using CWB.CommonUtils.Common;
using CWB.Logging;
using CWB.Masters.Infrastructure;
using CWB.Masters.MastersUtils;
using CWB.Masters.MastersUtils.ItemMaster;
using CWB.Masters.Repositories.Company;
using CWB.Masters.Repositories.Routing;
using CWB.Masters.ViewModels.Company;
using CWB.Masters.ViewModels.ItemMaster;
using CWB.Masters.ViewModels.Routing;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace CWB.Masters.Services.Routing
{
    public class RoutingService : IRoutingService
    {
        private readonly ILoggerManager _logger;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRoutingRepository _routingRepository;
        private readonly IRoutingStepRepository _routingStepRepository;
        private readonly IRoutingStepPartRepository _routingStepPartRepository;

        public RoutingService(ILoggerManager logger, IMapper mapper, IUnitOfWork unitOfWork,
            IRoutingRepository routingRepository, IRoutingStepRepository routingStepRepository, IRoutingStepPartRepository routingStepPartRepository)
        {
            _logger = logger;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _routingRepository = routingRepository;
            _routingStepRepository = routingStepRepository;
            _routingStepPartRepository = routingStepPartRepository;
        }

        public IEnumerable<RoutingVM> GetRoutingsForManufId(int manufId)
        {
            var routings = _routingRepository.GetRangeAsync(m => m.ManufacturedPartId == manufId).OrderBy(m=>m.Id);
            return _mapper.Map <IEnumerable<RoutingVM>>(routings);
        }

        public IEnumerable<RoutingStepVM> GetStepsForRoutingId(int routingId)
        {
            try
            {
                var routingsteps = _routingStepRepository.GetRangeAsync(m => m.RoutingId == routingId).OrderBy(m => m.Id);
                return _mapper.Map<IEnumerable<RoutingStepVM>>(routingsteps);
            } catch(Exception ex)
            {
                string msg = ex.InnerException.Message;
                string src = ex.InnerException.Source;
                return new List<RoutingStepVM>();
            }
        }


        public IEnumerable<RoutingStepPartVM> GetPartsForStepId(int stepId)
        {
            try
            {
                var stepParts = _routingStepPartRepository.GetRangeAsync(m => m.RoutingStepId == stepId).OrderByDescending(m => m.Id);
                return _mapper.Map<IEnumerable<RoutingStepPartVM>>(stepParts);
            }
            catch (Exception ex)
            {
                string msg = ex.InnerException.Message;
                string src = ex.InnerException.Source;
                return new List<RoutingStepPartVM>();
            }

        }

        public async Task<IEnumerable<Domain.Routing>> GetAllRoutings()
        {
            var routings = await _routingRepository.GetAllAsync();
            return routings;

        }

        public IEnumerable<RoutingStepPartVM> GetPartsForManufId(int manufID)
        {
            try
            {
                var stepParts = _routingStepPartRepository.GetRangeAsync(m => m.ManufacturedPartId == manufID).OrderBy(m => m.Id);
                return _mapper.Map<IEnumerable<RoutingStepPartVM>>(stepParts);
            }
            catch (Exception ex)
            {
                string msg = ex.InnerException.Message;
                string src = ex.InnerException.Source;
                return new List<RoutingStepPartVM>();
            }

        }



        public async Task<RoutingVM> Routing(RoutingVM routingVM)
        {
            var routing = _mapper.Map<Domain.Routing>(routingVM);
            if (routing.Id == 0)
            {
                await _routingRepository.AddAsync(routing);
            }
            else
            {
                routing = await _routingRepository.UpdateAsync(routing.Id, routing);
            }
            await _unitOfWork.CommitAsync();
            routingVM.RoutingId = (int)routing.Id;
            return routingVM;
        }

        public async Task<RoutingStepVM> RoutingStep(RoutingStepVM routingStepVM)
        {
            try
            {
                var routingStep = _mapper.Map<Domain.RoutingStep>(routingStepVM);
                if (routingStep.Id == 0)
                {
                    await _routingStepRepository.AddAsync(routingStep);
                }
                else
                {
                    routingStep = await _routingStepRepository.UpdateAsync(routingStep.Id, routingStep);
                }
                await _unitOfWork.CommitAsync();
                routingStepVM.StepId = (int)routingStep.Id;
                return routingStepVM;
            }catch(Exception ex)
            {
                string msg = ex.InnerException.Message;
                string src = ex.InnerException.Source;
            }
            return routingStepVM;
        }

        public async Task<RoutingStepPartVM> RoutingStepPart(RoutingStepPartVM routingStepPartVM)
        {
            try
            {
                var routingStepPart = _mapper.Map<Domain.RoutingStepPart>(routingStepPartVM);
                if (routingStepPart.Id == 0)
                {
                    await _routingStepPartRepository.AddAsync(routingStepPart);
                }
                else
                {
                    routingStepPart = await _routingStepPartRepository.UpdateAsync(routingStepPart.Id, routingStepPart);
                }
                await _unitOfWork.CommitAsync();
                routingStepPartVM.StepPartId = (int)routingStepPart.Id;
            }catch(Exception ex)
            {
                string msg = ex.InnerException.Message;
                string src = ex.InnerException.Source;
            }
            return routingStepPartVM;
        }
    }
}
