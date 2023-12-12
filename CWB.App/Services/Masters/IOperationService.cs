using CWB.App.Models.OperationList;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CWB.App.Services.Masters
{
    public interface IOperationService
    {
        Task<IEnumerable<OperationListVM>> GetOperationsList();
        Task<OperationVM> Operation(OperationVM operationVM);
        Task<bool> CheckIfOperationExisit(long OperationId, string Operation);

        Task<OperationVM> Operation(long Id);

        Task<IEnumerable<DocumentTypeListVM>> GetOperationDocTypes(long OperationId);

        Task<IEnumerable<OperationalListDocmentsVM>> GetOperationDocTypesList(long OperationId);

        Task<OperationDocumentTypeVM> OperationDocument(OperationDocumentTypeVM operationDocumentTypeVM);
    }
}
