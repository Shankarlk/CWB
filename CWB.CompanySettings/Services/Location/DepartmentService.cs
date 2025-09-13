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

        public DepartmentService(ILoggerManager logger, IMapper mapper, IUnitOfWork unitOfWork, IDepartmentRepository departmentRepository, IDept_Role_ListRepository Dept_Role_ListRepository,IEmployee_PwdRepository Employee_PwdRepository
            , IDept_EmployeeRepository Dept_EmployeeRepository, IEmployee_UI_ListRepository Employee_UI_ListRepository)
        {
            _logger = logger;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _departmentRepository = departmentRepository;
            _Dept_EmployeeRepository = Dept_EmployeeRepository;
            _Employee_UI_ListRepository = Employee_UI_ListRepository;
            _Dept_Role_ListRepository = Dept_Role_ListRepository;
            _Employee_PwdRepository = Employee_PwdRepository;

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
            shopDepartmentVM.DepartmentId = department.Id;
            return shopDepartmentVM;
        }

        public async Task<IEnumerable<ShopDepartmentVM>> GetAllDepartments(long PlantId, long TenantId)
        {
            var departments = await _departmentRepository.GetAllDepartmentsByTenantAsync(TenantId);
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
