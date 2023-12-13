using CWB.Masters.ViewModels.OperationList;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CWB.Masters.Services.OperationList
{
    public interface IOperationListService
    {
        IEnumerable<OperationListVM> GetOperationsByTenant(long tenantID);
        Task<OperationVM> Operation(OperationVM operationVM);
        OperationVM Operation(long Id, long TenantId);
        bool CheckIfOperationExisit(CheckOperationVM checkOperationVM);
        IEnumerable<OperationalDocumentListVM> GetOperationDocumentTypes(long TenantId, long OperationId);
        Task<OperationalDocumentListVM> OperationDocumentTypes(OperationalDocumentListVM operationalDocumentListVM);
    }
}
