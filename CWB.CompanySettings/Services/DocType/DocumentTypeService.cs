using AutoMapper;
using CWB.CompanySettings.Infrastructure;
using CWB.CompanySettings.Repositories.DocType;
using CWB.CompanySettings.ViewModels.DocType;
using CWB.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CWB.CompanySettings.Services.DocType
{
    public class DocumentTypeService : IDocumentTypeService
    {
        private readonly ILoggerManager _logger;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IDocumentTypeRepository _documentTypeRepository;

        public DocumentTypeService(ILoggerManager logger, IMapper mapper, IUnitOfWork unitOfWork, IDocumentTypeRepository documentTypeRepository)
        {
            _logger = logger;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _documentTypeRepository = documentTypeRepository;

        }

        public bool CheckDocumentTypeExisit(CheckDocumentTypeVM checkDocumentTypeVM)
        {
            var docTypes = _documentTypeRepository.GetRangeAsync(d => d.Name == checkDocumentTypeVM.Name &&
            d.TenantId == checkDocumentTypeVM.TenantId);
            if (!docTypes.Any())
            {
                return false;
            }
            return (docTypes.First().Id != checkDocumentTypeVM.DocumentTypeId);
        }

        public async Task<DocumentTypeVM> DocumentType(DocumentTypeVM documentTypeVM)
        {
            var documentType = _mapper.Map<Domain.DocumentType>(documentTypeVM);
            if (documentType.Id == 0)
            {
                await _documentTypeRepository.AddAsync(documentType);
            }
            else
            {
                documentType = await _documentTypeRepository.UpdateAsync(documentType.Id, documentType);
            }
            await _unitOfWork.CommitAsync();
            documentTypeVM.DocumentTypeId = documentType.Id;
            return documentTypeVM;
        }

        public IEnumerable<DocumentTypeListVM> GetDocumentTypes(long TenantId)
        {
            var docTypes = _documentTypeRepository.GetRangeAsync(d => d.TenantId == TenantId);
            return _mapper.Map<IEnumerable<DocumentTypeListVM>>(docTypes);
        }
    }
}
