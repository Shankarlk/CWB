using CWB.CommonUtils.Common;
using CWB.Constants.UserIdentity;
using CWB.Logging;
using CWB.Masters.Domain;
using CWB.Masters.MastersUtils;
using CWB.Masters.Services.ItemMaster;
using CWB.Masters.ViewModels.Company;
using CWB.Masters.ViewModels.ItemMaster;
using CWB.Masters.ViewModelValidators.ItemMaster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CWB.Masters.Controllers
{
    [ApiController]
    [Authorize(Roles = Roles.ADMIN)]
    public class BoughtOutFinishDetailController : ControllerBase
    {
        private readonly ILoggerManager _logger;
        private readonly IBoughtOutFinishDetailService _boughtOutFinishDetailService;

        public BoughtOutFinishDetailController(ILoggerManager logger, IBoughtOutFinishDetailService boughtOutFinishDetailService)
        {
            _logger = logger;
            _boughtOutFinishDetailService = boughtOutFinishDetailService;
        }

        /// <summary>
        /// Get BoughtOutFinishDetail List by Tenant Id
        /// </summary>
        /// <param name="TenantId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route(ApiRoutes.BoughtOutFinishDetail.GetBoughtOutFinishDetailList)]
        [Produces(AppContentTypes.ContentType, Type = typeof(List<BoughtOutFinishDetailListVM>))]
        public IActionResult GetBoughtOutFinishDetailList(long tenantId)
        {
            var boughtoutfinishdetails = _boughtOutFinishDetailService.GetBoughtOutFinishDetailsByTenant(tenantId);
            return Ok(boughtoutfinishdetails);
        }


        /// <summary>
        /// Add/Edit BoughtOutFinishDetail
        /// </summary>
        /// <param name="BoughtOutFinishDetailVM"></param>
        /// <returns></returns>
        [HttpPost]
        [Route(ApiRoutes.BoughtOutFinishDetail.PostBoughtOutFinishDetail)]
        [Produces(AppContentTypes.ContentType, Type = typeof(BoughtOutFinishDetailVM))]
        public async Task<IActionResult> PostBoughtOutFinishDetail([FromBody] BoughtOutFinishDetailVM boughtOutFinishDetailVM)
        {
            var validator = new BoughtOutFinishDetailVMValidator();
            var validationResult = await validator.ValidateAsync(boughtOutFinishDetailVM);
            if (!validationResult.IsValid)
                return BadRequest(validationResult.Errors);
            //if (boughtOutFinishDetailVM.ManufacturedPartType == 1)
            //{
            //    boughtOutFinishDetailVM.MakeFrom = "MF";
            //}
            //else if (boughtOutFinishDetailVM.ManufacturedPartType == 2)
            //{
            //    boughtOutFinishDetailVM.BOM = "BOM";
            //}
            var result = await _boughtOutFinishDetailService.BoughtOutFinishDetail(boughtOutFinishDetailVM);
            return Ok(result);
        }
    }
}
