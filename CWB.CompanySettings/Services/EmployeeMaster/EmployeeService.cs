using AutoMapper;
using CWB.CompanySettings.Infrastructure;
using CWB.CompanySettings.Repositories.EmployeeMaster;
using CWB.CompanySettings.ViewModels.EmployeeMaster;
using CWB.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CWB.CompanySettings.Services.EmployeeMaster
{
    public class EmployeeService :IEmployeeSerivce
    {
        private readonly ILoggerManager _logger;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUi_ListRepository _ui_ListRepository;
        private readonly IOrg_ChartRepository _IOrg_ChartRepository;
        private readonly IRole_ListRepository _IRole_ListRepository;
        private readonly IRole_Ui_ListRepository _IRole_Ui_ListRepository;
        private readonly IEmpl_Role_ListRepository _IEmpl_Role_ListRepository;
        private readonly IPermission_ListRepository _IPermission_ListRepository;
        private readonly IEmployeeMasterRepository _IEmployeeMasterRepository;
        public EmployeeService(ILoggerManager logger, IMapper mapper, IUnitOfWork unitOfWork,
            IUi_ListRepository ui_ListRepository, IOrg_ChartRepository IOrg_ChartRepository, IRole_ListRepository IRole_ListRepository,
            IRole_Ui_ListRepository IRole_Ui_ListRepository, IEmpl_Role_ListRepository IEmpl_Role_ListRepository, IPermission_ListRepository IPermission_ListRepository
            , IEmployeeMasterRepository IEmployeeMasterRepository)
        {
            _logger = logger;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _ui_ListRepository = ui_ListRepository;
            _IOrg_ChartRepository = IOrg_ChartRepository;
            _IRole_ListRepository = IRole_ListRepository;
            _IRole_Ui_ListRepository = IRole_Ui_ListRepository;
            _IEmpl_Role_ListRepository = IEmpl_Role_ListRepository;
            _IPermission_ListRepository = IPermission_ListRepository;
            _IEmployeeMasterRepository = IEmployeeMasterRepository;

        }
        public async Task<UiListVM> PostUilist(UiListVM uiListVM)
        {
            var designation = _mapper.Map<Domain.UiList>(uiListVM);
            if (designation.Id == 0)
            {
                await _ui_ListRepository.AddAsync(designation);
            }
            else
            {
                designation = await _ui_ListRepository.UpdateAsync(designation.Id, designation);
            }
            await _unitOfWork.CommitAsync();
            uiListVM.UiListId = designation.Id;
            return uiListVM;
        }

        public async Task<IEnumerable<UiListVM>> GetAllUiList(long TenantId)
        {
            var designations = await _ui_ListRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<UiListVM>>(designations);
        }

        public async Task<bool> DelUiList(long designationId)
        {
            try
            {
                var designation = await _ui_ListRepository.SingleOrDefaultAsync(d => d.Id == designationId);
                if (designation != null)
                {
                    if (designation.Id > 0)
                    {
                        _ui_ListRepository.Remove(designation);
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
        public async Task<Org_ChartVM> PostOrgChart(Org_ChartVM uiListVM)
        {
            var designation = _mapper.Map<Domain.Org_Chart>(uiListVM);
            if (designation.Id == 0)
            {
                await _IOrg_ChartRepository.AddAsync(designation);
            }
            else
            {
                designation = await _IOrg_ChartRepository.UpdateAsync(designation.Id, designation);
            }
            await _unitOfWork.CommitAsync();
            uiListVM.Org_ChartId = designation.Id;
            return uiListVM;
        }

        public IEnumerable<Org_ChartVM> GetAllOrgChart(long TenantId)
        {
            var designations = _IOrg_ChartRepository.GetRangeAsync(d => d.TenantId == TenantId);
            return _mapper.Map<IEnumerable<Org_ChartVM>>(designations);
        }

        public async Task<bool> DelOrgChart(long designationId)
        {
            try
            {
                var designation = await _IOrg_ChartRepository.SingleOrDefaultAsync(d => d.Id == designationId);
                if (designation != null)
                {
                    if (designation.Id > 0)
                    {
                        _IOrg_ChartRepository.Remove(designation);
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
        public async Task<Role_ListVM> PostRoleList(Role_ListVM uiListVM)
        {
            var designation = _mapper.Map<Domain.Role_List>(uiListVM);
            if (designation.Id == 0)
            {
                await _IRole_ListRepository.AddAsync(designation);
            }
            else
            {
                designation = await _IRole_ListRepository.UpdateAsync(designation.Id, designation);
            }
            await _unitOfWork.CommitAsync();
            uiListVM.Role_ListId = designation.Id;
            return uiListVM;
        }

        public IEnumerable<Role_ListVM> GetAllRoleList(long TenantId)
        {
            var designations = _IRole_ListRepository.GetRangeAsync(d => d.TenantId == TenantId);
            return _mapper.Map<IEnumerable<Role_ListVM>>(designations);
        }

        public async Task<bool> DelRoleList(long designationId)
        {
            try
            {
                var designation = await _IRole_ListRepository.SingleOrDefaultAsync(d => d.Id == designationId);
                if (designation != null)
                {
                    if (designation.Id > 0)
                    {
                        _IRole_ListRepository.Remove(designation);
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
        public async Task<Role_UI_ListVM> PostRoleUiList(Role_UI_ListVM uiListVM)
        {
            var designation = _mapper.Map<Domain.Role_Ui_List>(uiListVM);
            if (designation.Id == 0)
            {
                await _IRole_Ui_ListRepository.AddAsync(designation);
            }
            else
            {
                designation = await _IRole_Ui_ListRepository.UpdateAsync(designation.Id, designation);
            }
            await _unitOfWork.CommitAsync();
            uiListVM.Role_Ui_ListId = designation.Id;
            return uiListVM;
        }

        public IEnumerable<Role_UI_ListVM> GetAllRoleUiList(long TenantId)
        {
            var designations = _IRole_Ui_ListRepository.GetRangeAsync(d => d.TenantId == TenantId);
            return _mapper.Map<IEnumerable<Role_UI_ListVM>>(designations);
        }

        public async Task<bool> DelRoleUiList(long designationId)
        {
            try
            {
                var designation = await _IRole_Ui_ListRepository.SingleOrDefaultAsync(d => d.Id == designationId);
                if (designation != null)
                {
                    if (designation.Id > 0)
                    {
                        _IRole_Ui_ListRepository.Remove(designation);
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
        public async Task<Empl_Role_ListVM> PostEmplRoleList(Empl_Role_ListVM uiListVM)
        {
            var designation = _mapper.Map<Domain.Empl_Role_List>(uiListVM);
            if (designation.Id == 0)
            {
                await _IEmpl_Role_ListRepository.AddAsync(designation);
            }
            else
            {
                designation = await _IEmpl_Role_ListRepository.UpdateAsync(designation.Id, designation);
            }
            await _unitOfWork.CommitAsync();
            uiListVM.Empl_Role_ListId = designation.Id;
            return uiListVM;
        }

        public IEnumerable<Empl_Role_ListVM> GetAllEmplRoleList(long TenantId)
        {
            var designations = _IEmpl_Role_ListRepository.GetRangeAsync(d => d.TenantId == TenantId);
            return _mapper.Map<IEnumerable<Empl_Role_ListVM>>(designations);
        }

        public async Task<bool> DelEmplRoleList(long designationId)
        {
            try
            {
                var designation = await _IEmpl_Role_ListRepository.SingleOrDefaultAsync(d => d.Id == designationId);
                if (designation != null)
                {
                    if (designation.Id > 0)
                    {
                        _IEmpl_Role_ListRepository.Remove(designation);
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
        public async Task<Permission_ListVM> PostPermissionList(Permission_ListVM uiListVM)
        {
            var designation = _mapper.Map<Domain.Permission_List>(uiListVM);
            if (designation.Id == 0)
            {
                await _IPermission_ListRepository.AddAsync(designation);
            }
            else
            {
                designation = await _IPermission_ListRepository.UpdateAsync(designation.Id, designation);
            }
            await _unitOfWork.CommitAsync();
            uiListVM.PermissionId = designation.Id;
            return uiListVM;
        }

        public async Task<IEnumerable<Permission_ListVM>> GetAllPermissionListAsync(long TenantId)
        {
            var designations = await _IPermission_ListRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<Permission_ListVM>>(designations);
        }

        public async Task<bool> DelPermissionList(long designationId)
        {
            try
            {
                var designation = await _IPermission_ListRepository.SingleOrDefaultAsync(d => d.Id == designationId);
                if (designation != null)
                {
                    if (designation.Id > 0)
                    {
                        _IPermission_ListRepository.Remove(designation);
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
        public async Task<EmployeeVM> PostEmployee(EmployeeVM uiListVM)
        {
            var designation = _mapper.Map<Domain.Employee>(uiListVM);
            if (designation.Id == 0)
            {
                await _IEmployeeMasterRepository.AddAsync(designation);
            }
            else
            {
                designation = await _IEmployeeMasterRepository.UpdateAsync(designation.Id, designation);
            }
            try
            {
                await _unitOfWork.CommitAsync();
            }
            catch (Exception ex)
            {

            }
            uiListVM.Employee_ID = designation.Id;
            return uiListVM;
        }

        public IEnumerable<EmployeeVM> GetAllEmployee(long TenantId)
        {
            var designations = _IEmployeeMasterRepository.GetRangeAsync(d => d.TenantId == TenantId);
            return _mapper.Map<IEnumerable<EmployeeVM>>(designations);
        }

        public async Task<bool> DelEmployee(long designationId)
        {
            try
            {
                var designation = await _IEmployeeMasterRepository.SingleOrDefaultAsync(d => d.Id == designationId);
                if (designation != null)
                {
                    if (designation.Id > 0)
                    {
                        _IEmployeeMasterRepository.Remove(designation);
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
