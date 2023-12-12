using CWB.CompanySettings.ViewModels.DocType;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CWB.CompanySettings.Services.DocType
{
    public interface IDocumentTypeService
    {
        IEnumerable<DocumentTypeListVM> GetDocumentTypes(long TenantId);

        Task<DocumentTypeVM> DocumentType(DocumentTypeVM documentTypeVM);

        bool CheckDocumentTypeExisit(CheckDocumentTypeVM checkDocumentTypeVM);
    }
}
