using CWB.CommonUtils.Common;
using CWB.Constants.UserIdentity;
using CWB.Logging;
using CWB.Masters.Domain.ItemMaster;
using CWB.Masters.MastersUtils;
using CWB.Masters.MastersUtils.ItemMaster;
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
using System.Linq;
using System.Threading.Tasks;

namespace CWB.Masters.Controllers
{
    [ApiController]
    [Authorize(Roles = Roles.ADMIN)]
    public class MasterPartController : ControllerBase
    {
        private readonly ILoggerManager _logger;
        private readonly IRawMaterialDetailService _rawMaterialDetailService;
        private readonly IBoughtOutFinishDetailService _boughtOutFinishDetailService;
        private readonly IManufacturedPartNoDetailService _manufacturedPartNoDetailService;
        private readonly IMasterPartService _masterPartService;


        public MasterPartController(ILoggerManager logger
            , IRawMaterialDetailService rawMaterialDetailService
            , IManufacturedPartNoDetailService manufacturedPartNoDetailService
            , IBoughtOutFinishDetailService boughtOutFinishDetailService
            , IMasterPartService masterPartService)
        {
            _logger = logger;
            _rawMaterialDetailService = rawMaterialDetailService;
            _manufacturedPartNoDetailService = manufacturedPartNoDetailService;
            _boughtOutFinishDetailService = boughtOutFinishDetailService;
            _masterPartService = masterPartService;
        }

        [HttpGet]
        [Route(ApiRoutes.MasterParts.CheckPartNo)]
        [Produces(AppContentTypes.ContentType, Type = typeof(bool))]
        public async Task<IActionResult> CheckPartNo(string partNo)
        {
            bool exists = false;
            int partId = _masterPartService.CheckPartNo(partNo);
            if (partId > 0)
            {
                exists = _manufacturedPartNoDetailService.CheckPartNo(partId);
                if (!exists)
                {
                    exists = _rawMaterialDetailService.CheckPartNo(partId);
                    if (!exists)
                    {
                        exists = _boughtOutFinishDetailService.CheckPartNo(partId);
                    }
                }
            }
            return Ok(exists);
        }

        [HttpGet]
        [Route(ApiRoutes.Masters.GetStatuses)]
        [Produces(AppContentTypes.ContentType, Type = typeof(List<PartStatusVM>))]
        public IActionResult GetStatuses()
        {
            var companyTypes = _masterPartService.GetStatuses();
            return Ok(companyTypes);
        }
    }

}
