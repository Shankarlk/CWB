using AutoMapper;
using CWB.Logging;
using CWB.Masters.Domain;
using CWB.Masters.Infrastructure;
using CWB.Masters.Repositories.Machines;
using CWB.Masters.ViewModels.Machines;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CWB.Masters.Services.Machines
{
    public class MachineService : IMachineService
    {
        private readonly ILoggerManager _logger;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMachineTypeRepository _machineTypeRepository;
        private readonly IMachineRepository _machineRepository;
        private readonly IMachineProcessDocumentRepository _machineProcessDocumentRepository;

        public MachineService(ILoggerManager logger, IMapper mapper,
            IUnitOfWork unitOfWork, IMachineTypeRepository machineTypeRepository,
            IMachineRepository machineRepository, IMachineProcessDocumentRepository machineProcessDocumentRepository)
        {
            _logger = logger;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _machineTypeRepository = machineTypeRepository;
            _machineRepository = machineRepository;
            _machineProcessDocumentRepository = machineProcessDocumentRepository;
        }

        public bool CheckMachine(string Name, string SlNo, long MachineId, long TenantId)
        {
            var machines = _machineRepository.GetRangeAsync(m => (m.Name == Name || m.SlNo == SlNo) &&
             m.TenantId == TenantId);
            if (!machines.Any())
            {
                return false;
            }
            return (machines.First().Id != MachineId);
        }

        public bool CheckMachineByName(string Name, long MachineId, long TenantId)
        {
            var machines = _machineRepository.GetRangeAsync(m => m.Name == Name &&
             m.TenantId == TenantId);
            if (!machines.Any())
            {
                return false;
            }
            return (machines.First().Id != MachineId);
        }

        public bool CheckMachineBySlNo(string SlNo, long MachineId, long TenantId)
        {
            var machines = _machineRepository.GetRangeAsync(m => m.SlNo == SlNo &&
            m.TenantId == TenantId);
            if (!machines.Any())
            {
                return false;
            }
            return (machines.First().Id != MachineId);
        }

        public bool CheckMachineProcDoc(long MachineId, long MachineProcDocId, long TenantId, long DocumentTypeId)
        {
            var machineProcDocs = _machineProcessDocumentRepository.GetRangeAsync(m => m.MachineId == MachineId && m.TenantId == TenantId
            && m.DocumentTypeId == DocumentTypeId);
            if (!machineProcDocs.Any())
            {
                return false;
            }
            return (machineProcDocs.First().Id != MachineProcDocId);
        }

        public bool CheckMachineType(MachineTypeVM machineTypeVM)
        {
            var machineTypes = _machineTypeRepository.GetRangeAsync(m => m.Name == machineTypeVM.MachineTypeName &&
            m.TenantId == machineTypeVM.TenantId);
            if (!machineTypes.Any())
            {
                return false;
            }
            return (machineTypes.First().Id != machineTypeVM.MachineTypeTypeId);
        }

        public MachineVM GetMachine(long MachineId, long TenantId)
        {
            var machines = _machineRepository.GetRangeAsync(m => m.TenantId == TenantId && m.Id == MachineId);
            if (!machines.Any())
            {
                return null;
            }
            return _mapper.Map<MachineVM>(machines.First());
        }

        public IEnumerable<MachineProcDocumentListVM> GetMachineProcDocuments(long MachineId, long TenantId)
        {
            var machineProcDocuments = _machineProcessDocumentRepository.GetRangeAsync(m => m.MachineId == MachineId && m.TenantId == TenantId);
            return _mapper.Map<IEnumerable<MachineProcDocumentListVM>>(machineProcDocuments);
        }

        public IEnumerable<MachineListVM> GetMachines(long TenantId)
        {
            var machines = _machineRepository.GetRangeAsync(m => m.TenantId == TenantId);
            return _mapper.Map<IEnumerable<MachineListVM>>(machines);
        }

        public IEnumerable<MachineTypeVM> GetMachineTypes(long TenantId)
        {
            var machineTypes = _machineTypeRepository.GetRangeAsync(m => m.TenantId == TenantId);
            return _mapper.Map<IEnumerable<MachineTypeVM>>(machineTypes);
        }

        public async Task<MachineVM> Machine(MachineVM machineVM)
        {
            var machine = _mapper.Map<Machine>(machineVM);
            if (machine.Id == 0)
            {
                await _machineRepository.AddAsync(machine);
            }
            else
            {
                machine = await _machineRepository.UpdateAsync(machine.Id, machine);
            }
            await _unitOfWork.CommitAsync();
            machineVM.MachineMachineId = machine.Id;
            return machineVM;
        }

        public async Task<MachineProcDocumentVM> MachineProcDoc(MachineProcDocumentVM machineProcDocumentVM)
        {
            var machineProcessDocument = _mapper.Map<MachineProcessDocument>(machineProcDocumentVM);
            if (machineProcessDocument.Id == 0)
            {
                await _machineProcessDocumentRepository.AddAsync(machineProcessDocument);
            }
            else
            {
                machineProcessDocument = await _machineProcessDocumentRepository.UpdateAsync(machineProcessDocument.Id, machineProcessDocument);
            }
            await _unitOfWork.CommitAsync();
            machineProcDocumentVM.MachineProcDocumentId = machineProcessDocument.Id;
            return machineProcDocumentVM;
        }

        public async Task<MachineTypeVM> MachineType(MachineTypeVM machineTypeVM)
        {
            var machineType = _mapper.Map<MachineType>(machineTypeVM);
            if (machineType.Id == 0)
            {
                await _machineTypeRepository.AddAsync(machineType);
            }
            else
            {
                machineType = await _machineTypeRepository.UpdateAsync(machineType.Id, machineType);
            }
            await _unitOfWork.CommitAsync();
            machineTypeVM.MachineTypeTypeId = machineType.Id;
            return machineTypeVM;
        }


    }
}
