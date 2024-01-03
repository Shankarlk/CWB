using AutoMapper;
using CWB.CommonUtils.Common;
using CWB.Logging;
using CWB.Masters.Domain;
using CWB.Masters.Infrastructure;
using CWB.Masters.MastersUtils;
using CWB.Masters.MastersUtils.ItemMaster;
using CWB.Masters.Repositories.Company;
using CWB.Masters.Repositories.Routings;
using CWB.Masters.ViewModels.Company;
using CWB.Masters.ViewModels.ItemMaster;
using CWB.Masters.ViewModels.Routings;
using CWB.Masters.Domain.Routings;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace CWB.Masters.Services.Routings
{
    public class RoutingService : IRoutingService
    {
        private readonly ILoggerManager _logger;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRoutingRepository _routingRepository;
        private readonly IRoutingStepRepository _routingStepRepository;
        private readonly IRoutingStepPartRepository _routingStepPartRepository;
        private readonly IRoutingStepMachineRepository _routingStepMachineRepository;
        private readonly IRoutingStepSupplierRepository _routingStepSupplierRepository;


        public RoutingService(ILoggerManager logger, IMapper mapper, IUnitOfWork unitOfWork
            ,IRoutingRepository routingRepository, IRoutingStepRepository routingStepRepository 
            ,IRoutingStepPartRepository routingStepPartRepository
            ,IRoutingStepMachineRepository routingStepMachineRepository
            , IRoutingStepSupplierRepository routingStepSupplierRepository)
        {
            _logger = logger;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _routingRepository = routingRepository;
            _routingStepRepository = routingStepRepository;
            _routingStepPartRepository = routingStepPartRepository;
            _routingStepMachineRepository = routingStepMachineRepository;
            _routingStepSupplierRepository = routingStepSupplierRepository;
        }

        public IEnumerable<RoutingVM> GetRoutingsForManufId(int manufId)
        {
            var routings = _routingRepository.GetRangeAsync(m => m.ManufacturedPartId == manufId).OrderBy(m=>m.Id);
            try
            {
                return _mapper.Map<IEnumerable<RoutingVM>>(routings);
            }
            catch (Exception ex) {
                return new List<RoutingVM>();
            }
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
        public async Task<RoutingStepVM> GetStep(int stepId)
        {
            RoutingStep step = await _routingStepRepository.SingleOrDefaultAsync(m=>m.Id == stepId);
            if(step == null)
            {
                return new RoutingStepVM { StepId = -1 };
            }
            return _mapper.Map <RoutingStepVM>(step);
        }
        public async Task<bool> DelStep(int stepId)
        {
            try
            {
                var step = await GetStep(stepId);
                RoutingStep routingStep = _mapper.Map<RoutingStep>(step);
                if (routingStep.Id > 0)
                {
                    _routingStepRepository.Remove(routingStep);
                    return true;
                }
                return false;
            }catch(Exception)
            {
                return false;
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

        public async Task<IEnumerable<CWB.Masters.Domain.Routings.Routing>> GetAllRoutings()
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
            var routing = _mapper.Map<CWB.Masters.Domain.Routings.Routing>(routingVM);
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
                var routingStep = _mapper.Map<RoutingStep>(routingStepVM);
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
                var routingStepPart = _mapper.Map<RoutingStepPart>(routingStepPartVM);
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

        public async Task<IEnumerable<RoutingStepMachineVM>> StepMachines(int stepId)
        {
            try
            {
                var stepMachines = _routingStepMachineRepository.GetRangeAsync(m => m.RoutingStepId == stepId).OrderBy(m => m.Id);
                return _mapper.Map<IEnumerable<RoutingStepMachineVM>>(stepMachines);
            }
            catch (Exception ex)
            {
                string msg = ex.InnerException.Message;
                string src = ex.InnerException.Source;
                return new List<RoutingStepMachineVM>();
            }
        }

        public async Task<IEnumerable<RoutingStepSupplierVM>> StepSuppliers(int stepId)
        {
            try
            {
                var stepSuppliers = _routingStepSupplierRepository.GetRangeAsync(m => m.RoutingStepId == stepId).OrderBy(m => m.Id);
                return _mapper.Map<IEnumerable<RoutingStepSupplierVM>>(stepSuppliers);
            }
            catch (Exception ex)
            {
                string msg = ex.InnerException.Message;
                string src = ex.InnerException.Source;
                return new List<RoutingStepSupplierVM>();
            }
        }

        public async Task<RoutingStepMachineVM> RoutingStepMachine(RoutingStepMachineVM routingStepMachineVM)
        {
            try
            {
                var routingStepMachine = _mapper.Map<RoutingStepMachine>(routingStepMachineVM);
                if (routingStepMachine.Id == 0)
                {
                    await _routingStepMachineRepository.AddAsync(routingStepMachine);
                }
                else
                {
                    routingStepMachine = await _routingStepMachineRepository.UpdateAsync(routingStepMachine.Id, routingStepMachine);
                }
                await _unitOfWork.CommitAsync();
                routingStepMachineVM.RoutingStepMachineId = (int)routingStepMachine.Id;
            }
            catch (Exception ex)
            {
                string msg = ex.InnerException.Message;
                string src = ex.InnerException.Source;
            }
            return routingStepMachineVM;
        }

        public async Task<RoutingStepSupplierVM> RoutingStepSupplier(RoutingStepSupplierVM routingStepSupplierVM)
        {
            try
            {
                var routingStepSupplier = _mapper.Map<RoutingStepSupplier>(routingStepSupplierVM);
                if (routingStepSupplier.Id == 0)
                {
                    await _routingStepSupplierRepository.AddAsync(routingStepSupplier);
                }
                else
                {
                    routingStepSupplier = await _routingStepSupplierRepository.UpdateAsync(routingStepSupplier.Id, routingStepSupplier);
                }
                await _unitOfWork.CommitAsync();
                routingStepSupplierVM.RoutingStepSupplierId = (int)routingStepSupplier.Id;
            }
            catch (Exception ex)
            {
                string msg = ex.InnerException.Message;
                string src = ex.InnerException.Source;
            }
            return routingStepSupplierVM;
        }
    }
}
