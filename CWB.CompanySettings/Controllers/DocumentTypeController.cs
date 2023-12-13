using CWB.CommonUtils.Common;
using CWB.CompanySettings.CompanySettingsUtils;
using CWB.CompanySettings.Services.DocType;
using CWB.CompanySettings.ViewModels.DocType;
using CWB.CompanySettings.ViewModelValidators.DocType;
using CWB.Constants.UserIdentity;
using CWB.Logging;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CWB.CompanySettings.Controllers
{

    [ApiController]
    [Authorize]
    public class DocumentTypeController : ControllerBase
    {
        private readonly ILoggerManager _logger;
        private readonly IDocumentTypeService _documentTypeService;

        public DocumentTypeController(ILoggerManager logger, IDocumentTypeService documentTypeService)
        {
            _logger = logger;
            _documentTypeService = documentTypeService;
        }


        /// <summary>
        /// Get Document Types by tenant Id..
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route(ApiRoutes.DocType.GetDocumentTypes)]
        [Produces(AppContentTypes.ContentType, Type = typeof(List<DocumentTypeListVM>))]
        [Authorize(Roles = Roles.ADMIN)]
        public IActionResult GetDocumentTypes(long Id)
        {
            var companyTypes = _documentTypeService.GetDocumentTypes(Id);
            return Ok(companyTypes);
        }


        /// <summary>
        /// Add/edit document type
        /// </summary>
        /// <param name="documentTypeVM"></param>
        /// <returns></returns>
        [HttpPost]
        [Route(ApiRoutes.DocType.PostDocumentType)]
        [Produces(AppContentTypes.ContentType, Type = typeof(DocumentTypeVM))]
        [Authorize(Roles = Roles.ADMIN)]
        public async Task<IActionResult> PostDocumentType([FromBody] DocumentTypeVM documentTypeVM)
        {
            var validator = new DocumentTypeVMValidator();
            var validationResult = await validator.ValidateAsync(documentTypeVM);
            if (!validationResult.IsValid)
                return BadRequest(validationResult.Errors);
            //check if duplicate
            var docTypeExist = _documentTypeService.CheckDocumentTypeExisit(new CheckDocumentTypeVM
            {
                DocumentTypeId = documentTypeVM.DocumentTypeId,
                Name = documentTypeVM.Name,
                TenantId = documentTypeVM.TenantId
            });
            if (docTypeExist)
            {
                ModelState.AddModelError("Name", $"Document Type: {documentTypeVM.Name} Already Exist");
                return BadRequest(ModelState);
            }
            var result = await _documentTypeService.DocumentType(documentTypeVM);
            return Ok(result);
        }
    }
}
