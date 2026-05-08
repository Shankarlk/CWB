using AutoMapper;
using CWB.CompanySettings.Domain;
using CWB.CompanySettings.Infrastructure;
using CWB.CompanySettings.Repositories.Location;
using CWB.CompanySettings.ViewModels.Location;
using CWB.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CWB.CompanySettings.Services.Location
{
    public class DepartmentService : IDepartmentService
    {
        private readonly ILoggerManager _logger;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IDepartmentRepository _departmentRepository;
        private readonly IDept_Role_ListRepository _Dept_Role_ListRepository;
        private readonly IEmployee_PwdRepository _Employee_PwdRepository;
        private readonly IDept_EmployeeRepository _Dept_EmployeeRepository;
        private readonly IEmployee_UI_ListRepository _Employee_UI_ListRepository;
        private readonly IStores_Dept_IDListRepository _stores_Dept_IDListRepository;
        public DepartmentService(ILoggerManager logger, IMapper mapper, IUnitOfWork unitOfWork, IDepartmentRepository departmentRepository, IDept_Role_ListRepository Dept_Role_ListRepository,IEmployee_PwdRepository Employee_PwdRepository
            , IDept_EmployeeRepository Dept_EmployeeRepository, IEmployee_UI_ListRepository Employee_UI_ListRepository, IStores_Dept_IDListRepository stores_Dept_IDListRepository)
        {
            _logger = logger;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _departmentRepository = departmentRepository;
            _Dept_EmployeeRepository = Dept_EmployeeRepository;
            _Employee_UI_ListRepository = Employee_UI_ListRepository;
            _Dept_Role_ListRepository = Dept_Role_ListRepository;
            _Employee_PwdRepository = Employee_PwdRepository;
            _stores_Dept_IDListRepository = stores_Dept_IDListRepository;
        }

        public bool CheckDepartmentExisit(CheckDepartmentVM checkDepartmentVM)
        {
            var departments = _departmentRepository.GetRangeAsync(d => d.Name == checkDepartmentVM.Name &&
            d.TenantId == checkDepartmentVM.TenantId && d.PlantId == checkDepartmentVM.PlantId);
            if (!departments.Any())
            {
                return false;
            }
            return (departments.First().Id != checkDepartmentVM.DepartmentId);
        }

        public async Task<ShopDepartmentVM> Department(ShopDepartmentVM shopDepartmentVM)
        {
            var department = _mapper.Map<ShopDepartment>(shopDepartmentVM);
            if (department.Id == 0)
            {
                await _departmentRepository.AddAsync(department);
            }
            else
            {
                department = await _departmentRepository.UpdateAsync(department.Id, department);
            }
            await _unitOfWork.CommitAsync();
            await RebuildStoresMapping();

            await _unitOfWork.CommitAsync();
            shopDepartmentVM.DepartmentId = department.Id;
            return shopDepartmentVM;
        }
        private async Task RebuildStoresMapping()
        {
            var storeMap = await _stores_Dept_IDListRepository
                .SingleOrDefaultAsync(x => x.Id > 0);

            bool isNew = false;

            if (storeMap == null)
            {
                storeMap = new Stores_Dept_IDList
                {
                    CreationDate = DateTime.Now
                };

                isNew = true;
            }

            // 🔥 RESET
            storeMap.Stores_DirMatl_ID = 0;
            storeMap.Stores_Cust_Dispatch_ID = 0;
            storeMap.Stores_Tools_ID = 0;
            storeMap.Stores_Consumables_ID = 0;

            // 🔥 Fetch all departments
            var allDepts = await _departmentRepository
                .GetAllDepartmentsByTenantAsync(1); //  ideally pass tenantId

            var storeDepts = allDepts
                .Where(x => x.ProdDept == 2)
                .ToList();

            foreach (var dept in storeDepts)
            {
                if (dept.Stores_DirectMatl == 'Y')
                    storeMap.Stores_DirMatl_ID = dept.Id;

                if (dept.Stores_Cust_Dispatch == 'Y')
                    storeMap.Stores_Cust_Dispatch_ID = dept.Id;

                if (dept.Stores_Tools == 'Y')
                    storeMap.Stores_Tools_ID = dept.Id;

                if (dept.Stores_Consumables == 'Y')
                    storeMap.Stores_Consumables_ID = dept.Id;
            }

            storeMap.LastModifiedDate = DateTime.Now;

            if (isNew)
                await _stores_Dept_IDListRepository.AddAsync(storeMap);
            else
                await _stores_Dept_IDListRepository.UpdateAsync(storeMap.Id, storeMap);
        }
        public async Task<Stores_Dept_IDListVM> GetStoresIDs()
        {
            var Stores = await _stores_Dept_IDListRepository.SingleOrDefaultAsync(x => x.Id > 0);
            return _mapper.Map<Stores_Dept_IDListVM>(Stores);
            //if (bastatus != null)
            //{
            //    
            //}
            //return new BAStatusVM { StatusId = -1 };
        }
        public async Task<IEnumerable<ShopDepartmentVM>> GetAllDepartments(long PlantId, long TenantId)
        {
            var departments =  _departmentRepository.GetRangeAsync(p => p.TenantId == TenantId);
            return _mapper.Map<IEnumerable<ShopDepartmentVM>>(departments);
        }

        

        public IEnumerable<ShopDepartmentVM> GetDepartmentListWithPlants(List<long> DepartmentIds, long TenantId)
        {
            var departments = _departmentRepository.GetDepartmentsWithPlant(DepartmentIds, TenantId);
            return _mapper.Map<IEnumerable<ShopDepartmentVM>>(departments);
        }

        public async Task<bool> DelDepartment(long departmentId)
        {
            try
            {
                var dept = await _departmentRepository.SingleOrDefaultAsync(d => d.Id == departmentId);
                if (dept != null)
                {
                    if (dept.Id > 0)
                    {
                        _departmentRepository.Remove(dept);
                        await _unitOfWork.CommitAsync();
                    }
                }
            }
            catch (Exception ex)
            {
                return false;
            }

            return true;
        }
        public IEnumerable<Dept_Role_ListVM> GetDept_Role_List(long TenantId)
        {
            var plants = _Dept_Role_ListRepository.GetRangeAsync(p => p.TenantId == TenantId);
            return _mapper.Map<IEnumerable<Dept_Role_ListVM>>(plants);
        }
        public async Task<Dept_Role_ListVM> PostDept_Role_List(Dept_Role_ListVM plantWdVM)
        {
            var plantWd = _mapper.Map<Domain.Dept_Role_List>(plantWdVM);
            if (plantWd.Id == 0)
            {
                await _Dept_Role_ListRepository.AddAsync(plantWd);
            }
            else
            {
                plantWd = await _Dept_Role_ListRepository.UpdateAsync(plantWd.Id, plantWd);
            }
            await _unitOfWork.CommitAsync();
            plantWdVM.Dept_Role_ListId = plantWd.Id;
            return plantWdVM;
        }
        public async Task<bool> DelDept_Role_List(long designationId)
        {
            try
            {
                var designation = await _Dept_Role_ListRepository.SingleOrDefaultAsync(d => d.Id == designationId);
                if (designation != null)
                {
                    if (designation.Id > 0)
                    {
                        _Dept_Role_ListRepository.Remove(designation);
                        await _unitOfWork.CommitAsync();
                    }
                }
            }
            catch (Exception ex)
            {
                return false;
            }

            return true;
        }
        public IEnumerable<Employee_PwdVM> GetEmployee_Pwd(long TenantId)
        {
            var plants = _Employee_PwdRepository.GetRangeAsync(p => p.TenantId == TenantId);
            return _mapper.Map<IEnumerable<Employee_PwdVM>>(plants);
        }
        public async Task<Employee_PwdVM> PostEmployee_Pwd(Employee_PwdVM plantWdVM)
        {
            var plantWd = _mapper.Map<Domain.Employee_Pwd>(plantWdVM);
            if (plantWd.Id == 0)
            {
                await _Employee_PwdRepository.AddAsync(plantWd);
            }
            else
            {
                plantWd = await _Employee_PwdRepository.UpdateAsync(plantWd.Id, plantWd);
            }
            await _unitOfWork.CommitAsync();
            plantWdVM.Employee_PwdId = plantWd.Id;
            return plantWdVM;
        }
        public async Task<bool> DelEmployee_Pwd(long designationId)
        {
            try
            {
                var designation = await _Employee_PwdRepository.SingleOrDefaultAsync(d => d.Id == designationId);
                if (designation != null)
                {
                    if (designation.Id > 0)
                    {
                        _Employee_PwdRepository.Remove(designation);
                        await _unitOfWork.CommitAsync();
                    }
                }
            }
            catch (Exception ex)
            {
                return false;
            }

            return true;
        }
        public IEnumerable<Dept_EmployeeVM> GetDept_Employee(long TenantId)
        {
            var plants = _Dept_EmployeeRepository.GetRangeAsync(p => p.TenantId == TenantId);
            return _mapper.Map<IEnumerable<Dept_EmployeeVM>>(plants);
        }
        public async Task<Dept_EmployeeVM> PostDept_Employee(Dept_EmployeeVM plantWdVM)
        {
            var plantWd = _mapper.Map<Domain.Dept_Employee>(plantWdVM);
            if (plantWd.Id == 0)
            {
                await _Dept_EmployeeRepository.AddAsync(plantWd);
            }
            else
            {
                plantWd = await _Dept_EmployeeRepository.UpdateAsync(plantWd.Id, plantWd);
            }
            await _unitOfWork.CommitAsync();
            plantWdVM.Dept_EmployeeId = plantWd.Id;
            return plantWdVM;
        }
        public async Task<bool> DelDept_Employee(long designationId)
        {
            try
            {
                var designation = await _Dept_EmployeeRepository.SingleOrDefaultAsync(d => d.Id == designationId);
                if (designation != null)
                {
                    if (designation.Id > 0)
                    {
                        _Dept_EmployeeRepository.Remove(designation);
                        await _unitOfWork.CommitAsync();
                    }
                }
            }
            catch (Exception ex)
            {
                return false;
            }

            return true;
        }
        public IEnumerable<Employee_UI_ListVM> GetEmployee_UI_List(long TenantId)
        {
            var plants = _Employee_UI_ListRepository.GetRangeAsync(p => p.TenantId == TenantId);
            return _mapper.Map<IEnumerable<Employee_UI_ListVM>>(plants);
        }
        public async Task<Employee_UI_ListVM> PostEmployee_UI_List(Employee_UI_ListVM plantWdVM)
        {
            var plantWd = _mapper.Map<Domain.Employee_UI_List>(plantWdVM);
            if (plantWd.Id == 0)
            {
                await _Employee_UI_ListRepository.AddAsync(plantWd);
            }
            else
            {
                plantWd = await _Employee_UI_ListRepository.UpdateAsync(plantWd.Id, plantWd);
            }
            await _unitOfWork.CommitAsync();
            plantWdVM.Employee_UI_ListId = plantWd.Id;
            return plantWdVM;
        }
        public async Task<bool> DelEmployee_UI_List(long designationId)
        {
            try
            {
                var designation = await _Employee_UI_ListRepository.SingleOrDefaultAsync(d => d.Id == designationId);
                if (designation != null)
                {
                    if (designation.Id > 0)
                    {
                        _Employee_UI_ListRepository.Remove(designation);
                        await _unitOfWork.CommitAsync();
                    }
                }
            }
            catch (Exception ex)
            {
                return false;
            }

            return true;
        }
    }
}
