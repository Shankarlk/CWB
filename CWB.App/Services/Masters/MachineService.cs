using CWB.App.AppUtils;
using CWB.App.Models.Departments;
using CWB.App.Models.Machine;
using CWB.App.Models.OperationList;
using CWB.CommonUtils.Common;
using CWB.Logging;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CWB.App.Services.Masters
{
    public class MachineService : IMachineService
    {
        private readonly ILoggerManager _logger;
        private readonly ApiUrls _apiUrls;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly long tenantId;

        public MachineService(ILoggerManager logger, ApiUrls apiUrlsOptions, IHttpContextAccessor httpContextAccessor)
        {
            _logger = logger;
            _apiUrls = apiUrlsOptions;
            _httpContextAccessor = httpContextAccessor;
            tenantId = long.Parse(AppUtil.GetTenantId(_httpContextAccessor.HttpContext.User));
        }

        public async Task<bool> CheckMachine(CheckMachineVM checkMachineVM)
        {
            var uri = new Uri(_apiUrls.Gateway + $"/cwbms/machine-check");
            var headers = await AppUtil.GetAuthToken(_httpContextAccessor.HttpContext);
            checkMachineVM.TenantId = tenantId;
            return await RestHelper<bool>.PostAsync(uri, checkMachineVM, headers);
        }

        public async Task<bool> CheckMachineType(MachineTypeVM machineTypeVM)
        {
            var uri = new Uri(_apiUrls.Gateway + $"/cwbms/machine-type-check");
            var headers = await AppUtil.GetAuthToken(_httpContextAccessor.HttpContext);
            machineTypeVM.TenantId = tenantId;
            return await RestHelper<bool>.PostAsync(uri, machineTypeVM, headers);
        }

        public async Task<MachineVM> GetMachine(long Id)
        {
            var uri = new Uri(_apiUrls.Gateway + $"/cwbms/machine/{Id}/{tenantId}");
            var headers = await AppUtil.GetAuthToken(_httpContextAccessor.HttpContext);
            return await RestHelper<MachineVM>.GetAsync(uri, headers);
        }

        public async Task<IEnumerable<MachineListVM>> GetMachinesList()
        {
            var uri = new Uri(_apiUrls.Gateway + $"/cwbms/machines/{tenantId}");
            var departmentsUri = new Uri(_apiUrls.Gateway + $"/cwbcs/plant-departments");
            var headers = await AppUtil.GetAuthToken(_httpContextAccessor.HttpContext);
            var machines = await RestHelper<List<MachineListVM>>.GetAsync(uri, headers);
            var sectionsUri = new Uri(_apiUrls.Gateway + $"/cwbcs/getsections/{tenantId}");
            var sections = await RestHelper<List<SectionsVM>>.GetAsync(sectionsUri, headers);


            // Get distinct department IDs once
            var departmentIds = machines.Select(m => m.ShopId).Distinct().ToList();
            if (!departmentIds.Any())
                return machines; // nothing to map

            // Fetch department details in one call
            var data = new { TenantId = tenantId, DepartmentIds = departmentIds };
            var departments = await RestHelper<List<ShopDepartmentVM>>.PostAsync(departmentsUri, data, headers);

            // Convert departments to dictionary for O(1) lookup
            var deptLookup = departments.ToDictionary(d => d.DepartmentId, d => d);
            var section = sections.ToDictionary(d => d.SectionsId, d => d);

            // Map machines efficiently
            foreach (var machine in machines)
            {
                if (deptLookup.TryGetValue(machine.ShopId, out var dept))
                {
                    machine.Shop = dept.Name;
                    machine.Plant = dept.PlantName;
                }
                if (section.TryGetValue(machine.SectionId, out var sec))
                {
                    machine.SectionName = sec.Name ?? string.Empty;
                }
                else
                {
                    machine.SectionName = string.Empty;
                }

            }

            return machines;
        }

        public async Task<IEnumerable<MachineTypeVM>> GetMachineTypes()
        {
            var uri = new Uri(_apiUrls.Gateway + $"/cwbms/machine-types/{tenantId}");
            var headers = await AppUtil.GetAuthToken(_httpContextAccessor.HttpContext);
            return await RestHelper<List<MachineTypeVM>>.GetAsync(uri, headers);
        } 
        public async Task<IEnumerable<McTypeDocListVM>> GetMcTypeDocList()
        {
            var uri = new Uri(_apiUrls.Gateway + $"/cwbms/getmctypedoclist/{tenantId}");
            var headers = await AppUtil.GetAuthToken(_httpContextAccessor.HttpContext);
            return await RestHelper<List<McTypeDocListVM>>.GetAsync(uri, headers);
        }
        public async Task<IEnumerable<McSlNoDocListVM>> GetMcProcDocList()
        {
            var uri = new Uri(_apiUrls.Gateway + $"/cwbms/getmcslnodoclist/{tenantId}");
            var headers = await AppUtil.GetAuthToken(_httpContextAccessor.HttpContext);
            return await RestHelper<List<McSlNoDocListVM>>.GetAsync(uri, headers);
        }
        public async Task<McSlNoDocListVM> PostMcProcDocList(McSlNoDocListVM machineVM)
        {
            var uri = new Uri(_apiUrls.Gateway + $"/cwbms/postmcslnodoclist");
            var headers = await AppUtil.GetAuthToken(_httpContextAccessor.HttpContext);
            machineVM.TenantId = tenantId;
            return await RestHelper<McSlNoDocListVM>.PostAsync(uri, machineVM, headers);
        }
        public async Task<bool> DeleteMcProcDoc(long mcSlNoDocListId)
        {
            var uri = new Uri(_apiUrls.Gateway + $"/cwbms/deletemcslnodoclist/{mcSlNoDocListId}/{tenantId}");
            var headers = await AppUtil.GetAuthToken(_httpContextAccessor.HttpContext);
            return await RestHelper<bool>.GetAsync(uri, headers);
        }
        public async Task<McTypeDocListVM> PostMcTypeDocList(McTypeDocListVM machineVM)
        {
            var uri = new Uri(_apiUrls.Gateway + $"/cwbms/postmctypedoclist");
            var headers = await AppUtil.GetAuthToken(_httpContextAccessor.HttpContext);
            machineVM.TenantId = tenantId;
            return await RestHelper<McTypeDocListVM>.PostAsync(uri, machineVM, headers);
        }
        public async Task<bool> DeleteMcTypeDoc(long mcTypeDocListId)
        {
            var uri = new Uri(_apiUrls.Gateway + $"/cwbms/deletemctypdoclist/{mcTypeDocListId}/{tenantId}");
            var headers = await AppUtil.GetAuthToken(_httpContextAccessor.HttpContext);
            return await RestHelper<bool>.GetAsync(uri, headers);
        }

        public async Task<MachineVM> Machine(MachineVM machineVM)
        {
            var uri = new Uri(_apiUrls.Gateway + $"/cwbms/machine");
            var headers = await AppUtil.GetAuthToken(_httpContextAccessor.HttpContext);
            machineVM.TenantId = tenantId;
            return await RestHelper<MachineVM>.PostAsync(uri, machineVM, headers);
        }

        public async Task<MachineTypeVM> MachineType(MachineTypeVM machineTypeVM)
        {
            var uri = new Uri(_apiUrls.Gateway + $"/cwbms/machine-type");
            var headers = await AppUtil.GetAuthToken(_httpContextAccessor.HttpContext);
            machineTypeVM.TenantId = tenantId;
            return await RestHelper<MachineTypeVM>.PostAsync(uri, machineTypeVM, headers);
        }

        public async Task<IEnumerable<MachineProcDocumentListVM>> GetMachineProcsDocLists(long MachineId)
        {
            var uri = new Uri(_apiUrls.Gateway + $"/cwbcs/document-types/{tenantId}");
            var headers = await AppUtil.GetAuthToken(_httpContextAccessor.HttpContext);
            var docType = await RestHelper<List<DocumentTypeListVM>>.GetAsync(uri, headers);
            var machineProcsDocTypes = await machineProcsDocLists(MachineId);

            return from d in docType
                   join m in machineProcsDocTypes on d.DocumentTypeId equals m.MachineProcDocumentTypeId
                   select new MachineProcDocumentListVM
                   {

                       MachineProcDocumentId = m.MachineProcDocumentId,
                       MachineProcDocumentTypeId = m.MachineProcDocumentTypeId,
                       IsMachineProcDocumentMandatory = m.IsMachineProcDocumentMandatory,
                       MachineProcDocumentType = d.Name
                   };
        }

        public async Task<IEnumerable<DocumentTypeListVM>> GetMachineDocTypes(long MachineId)
        {
            var uri = new Uri(_apiUrls.Gateway + $"/cwbcs/document-types/{tenantId}");
            var headers = await AppUtil.GetAuthToken(_httpContextAccessor.HttpContext);
            var docType = await RestHelper<List<DocumentTypeListVM>>.GetAsync(uri, headers);
            var machineProcsDocTypes = await machineProcsDocLists(MachineId);
            return docType.Where(d => !machineProcsDocTypes.Any(m => m.MachineProcDocumentTypeId == d.DocumentTypeId));
        }

        private async Task<IEnumerable<MachineProcDocumentListVM>> machineProcsDocLists(long Id)
        {
            var uri = new Uri(_apiUrls.Gateway + $"/cwbms/machine-procs-docs/{Id}/{tenantId}");
            var headers = await AppUtil.GetAuthToken(_httpContextAccessor.HttpContext);
            return await RestHelper<List<MachineProcDocumentListVM>>.GetAsync(uri, headers);
        }

        public async Task<MachineProcDocumentVM> MachineProcDoc(MachineProcDocumentVM machineProcDocumentVM)
        {
            var uri = new Uri(_apiUrls.Gateway + $"/cwbms/machine-procs-doc");
            var headers = await AppUtil.GetAuthToken(_httpContextAccessor.HttpContext);
            machineProcDocumentVM.TenantId = tenantId;
            return await RestHelper<MachineProcDocumentVM>.PostAsync(uri, machineProcDocumentVM, headers);
        }
    }
}
