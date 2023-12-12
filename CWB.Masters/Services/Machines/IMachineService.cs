using CWB.Masters.ViewModels.Machines;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CWB.Masters.Services.Machines
{
    public interface IMachineService
    {
        IEnumerable<MachineTypeVM> GetMachineTypes(long TenantId);
        Task<MachineTypeVM> MachineType(MachineTypeVM machineTypeVM);
        IEnumerable<MachineListVM> GetMachines(long TenantId);
        Task<MachineVM> Machine(MachineVM machineTypeVM);
        MachineVM GetMachine(long MachineId, long TenantId);
        bool CheckMachineType(MachineTypeVM machineTypeVM);
        bool CheckMachineBySlNo(string SlNo, long MachineId, long TenantId);
        bool CheckMachineByName(string Name, long MachineId, long TenantId);
        bool CheckMachine(string Name, string SlNo, long MachineId, long TenantId);
        IEnumerable<MachineProcDocumentListVM> GetMachineProcDocuments(long MachineId, long TenantId);
        Task<MachineProcDocumentVM> MachineProcDoc(MachineProcDocumentVM machineProcDocumentVM);
        bool CheckMachineProcDoc(long MachineId, long MachineProcDocId, long TenantId, long DocumentTypeId);
    }
}
