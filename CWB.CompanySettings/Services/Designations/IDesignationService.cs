using CWB.CompanySettings.ViewModels.Designations;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CWB.CompanySettings.Services.Designations
{
    public interface IDesignationService
    {
        Task<IEnumerable<DesignationListVM>> GetDesignationsAsync(long TenantId);
        Task<DesignationVM> Designation(DesignationVM designationVM);
        bool CheckDesignationExisit(CheckDesignationVM checkDesignationVM);
        Task<bool> DelDesignation(long designationId);
    }
}
