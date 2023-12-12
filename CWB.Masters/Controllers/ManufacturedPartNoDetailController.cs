using CWB.CommonUtils.Common;
using CWB.Constants.UserIdentity;
using CWB.Logging;
using CWB.Masters.MastersUtils;
using CWB.Masters.Services.Company;
using CWB.Masters.Services.ItemMaster;
using CWB.Masters.ViewModels.Company;
using CWB.Masters.ViewModels.ItemMaster;
using CWB.Masters.ViewModelValidators.ItemMaster;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CWB.Masters.Controllers
{
    [ApiController]
    [Authorize(Roles = Roles.ADMIN)]
    public class ManufacturedPartNoDetailController : ControllerBase
    {
        private readonly ILoggerManager _logger;
        private readonly IManufacturedPartNoDetailService _manufacturedPartNoDetailService;

        public ManufacturedPartNoDetailController(ILoggerManager logger, IManufacturedPartNoDetailService manufacturedPartNoDetailService)
        {
            _logger = logger;
            _manufacturedPartNoDetailService = manufacturedPartNoDetailService;
        }

        /// <summary>
        /// Get All ManufacturedPartNoDetail by tenant
        /// </summary>
        /// <param name="tenantID"></param>
        /// <returns></returns>
        [HttpGet]
        [Route(ApiRoutes.ManufacturedPartNoDetail.GetManufacturedPartNoDetailList)]
        [Produces(AppContentTypes.ContentType, Type = typeof(List<ManufacturedPartNoDetailListVM>))]
        public IActionResult GetManufacturedPartNoDetailList(long ManufPartType, string companyName)
        {
            var manufacturedpartnodetails = _manufacturedPartNoDetailService.GetManufacturedPartNoDetailsByTypeTenant(ManufPartType, companyName);
            return Ok(manufacturedpartnodetails);
        }

        [HttpGet]
        [Route(ApiRoutes.ManufacturedPartNoDetail.HelloWorld)]
        [Produces(AppContentTypes.ContentType, Type = typeof(List<ManufacturedPartNoDetailListVM>))]
        public IActionResult HelloWorld()
        {
            //var manufacturedpartnodetails = _manufacturedPartNoDetailService.GetManufacturedPartNoDetailsByTypeTenant(manPartTypeId, tenantId);
            return Ok("Hello World");
        }

        [HttpGet]
        [Route(ApiRoutes.ManufacturedPartNoDetail.GetUOMs)]
        [Produces(AppContentTypes.ContentType, Type = typeof(List<UOMVM>))]
        public IActionResult GetUOMs(long Id)
        {
            var companies = _manufacturedPartNoDetailService.GetUOMsByTenantId(Id);
            Console.Write(companies.ToString());
            return Ok(companies);
        }





        //return Ok("Hello World");

        /// <summary>
        /// Add/Edit ManufacturedPartNoDetail
        /// </summary>
        /// <param name="ManufacturedPartNoDetailVM"></param>
        /// <returns></returns>
        [HttpPost]
        [Route(ApiRoutes.ManufacturedPartNoDetail.PostManufacturedPartNoDetail)]
        [Produces(AppContentTypes.ContentType, Type = typeof(ManufacturedPartNoDetailVM))]
        public async Task<IActionResult> PostManufacturedPartNoDetail([FromBody] ManufacturedPartNoDetailVM manufacturedPartNoDetailVM)
        {
            var validator = new ManufacturedPartNoDetailVMValidator();
            var validationResult = await validator.ValidateAsync(manufacturedPartNoDetailVM);
            if (!validationResult.IsValid)
                return BadRequest(validationResult.Errors);
            if (manufacturedPartNoDetailVM.ManufacturedPartType == 1) {
                manufacturedPartNoDetailVM.MakeFrom = "MF";
            }
            else if(manufacturedPartNoDetailVM.ManufacturedPartType == 2)
            {
                manufacturedPartNoDetailVM.BOM = "BOM";
            }
            var result = await _manufacturedPartNoDetailService.ManufacturedPartNoDetail(manufacturedPartNoDetailVM);
            return Ok(result);
        }
        [HttpPost]
        [Route(ApiRoutes.ManufacturedPartNoDetail.PostMPMakeFrom)]
        [Produces(AppContentTypes.ContentType, Type = typeof(ManufacturedPartNoDetailVM))]
        public async Task<IActionResult> PostMPMakeFrom([FromBody] ManufacturedPartNoDetailVM manufacturedPartNoDetailVM)
        {
            var validator = new ManufacturedPartNoDetailVMValidator();
            var validationResult = await validator.ValidateAsync(manufacturedPartNoDetailVM);
            if (!validationResult.IsValid)
                return BadRequest(validationResult.Errors);
            var result = await _manufacturedPartNoDetailService.MPMakeFrom(manufacturedPartNoDetailVM);
            return Ok(result);
        }
        /// <summary>
        /// Get All Companies by tenant
        /// </summary>
        /// <param name="tenantID"></param>
        /// <returns></returns>
        [HttpGet]
        [Route(ApiRoutes.ManufacturedPartNoDetail.GetMPMakeFromListBypartNumber)]
        [Produces(AppContentTypes.ContentType, Type = typeof(List<MPMakeFromListVM>))]
        public IActionResult GetMPMakeFromList(string partNumber, long tenantId)
        {
            var mpmakefromlist = _manufacturedPartNoDetailService.GetMPMakeFromListByPartNumberNTenant(partNumber, tenantId);
            return Ok(mpmakefromlist);
        }

        [HttpPost]
        [Route(ApiRoutes.ManufacturedPartNoDetail.PostMPBOM)]
        [Produces(AppContentTypes.ContentType, Type = typeof(ManufacturedPartNoDetailVM))]
        public async Task<IActionResult> PostMPBOM([FromBody] ManufacturedPartNoDetailVM manufacturedPartNoDetailVM)
        {
            var validator = new ManufacturedPartNoDetailVMValidator();
            var validationResult = await validator.ValidateAsync(manufacturedPartNoDetailVM);
            if (!validationResult.IsValid)
                return BadRequest(validationResult.Errors);
            var result = await _manufacturedPartNoDetailService.MPBOM(manufacturedPartNoDetailVM);
            return Ok(result);
        }

    }
}
