using CWB.CommonUtils.Common;
using CWB.Constants.UserIdentity;
using CWB.Logging;
using CWB.Masters.MastersUtils;
using CWB.Masters.Services.Company;
using CWB.Masters.Services.ItemMaster;
using CWB.Masters.ViewModels.ItemMaster;
using CWB.Masters.ViewModelValidators.ItemMaster;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CWB.Masters.Controllers
{
    [ApiController]
    [Authorize(Roles = Roles.ADMIN)]
    public class RawMaterialDetailController : ControllerBase
    {
        private readonly ILoggerManager _logger;
        private readonly IRawMaterialDetailService _rawMaterialDetailService;

        public RawMaterialDetailController(ILoggerManager logger, IRawMaterialDetailService rawMaterialDetailService)
        {
            _logger = logger;
            _rawMaterialDetailService = rawMaterialDetailService;
        }

        /// <summary>
        /// Get All RawMaterialDetail by tenant
        /// </summary>
        /// <param name="tenantID"></param>
        /// <returns></returns>
        [HttpGet]
        [Route(ApiRoutes.RawMaterialDetail.GetRawMaterialDetailList)]
        [Produces(AppContentTypes.ContentType, Type = typeof(List<RawMaterialDetailListVM>))]
        public IActionResult GetRawMaterialDetailList(long tenantId)
        {
            var rawmaterialdetails = _rawMaterialDetailService.GetRawMaterialDetailsByTenant(tenantId);
            return Ok(rawmaterialdetails);
        }

        /// <summary>
        /// Add/Edit company
        /// </summary>
        /// <param name="RawMaterialDetailVM"></param>
        /// <returns></returns>
        [HttpPost]
        [Route(ApiRoutes.RawMaterialDetail.PostRawMaterialDetail)]
        [Produces(AppContentTypes.ContentType, Type = typeof(RawMaterialDetailVM))]
        public async Task<IActionResult> PostRawMaterialDetail([FromBody] RawMaterialDetailVM rawMaterialDetailVM)
        {
            var validator = new RawMaterialDetailVMValidator();
            var validationResult = await validator.ValidateAsync(rawMaterialDetailVM);
            if (!validationResult.IsValid)
                return BadRequest(validationResult.Errors);
            var result = await _rawMaterialDetailService.RawMaterialDetail(rawMaterialDetailVM);
            return Ok(result);
        }
    }
}
