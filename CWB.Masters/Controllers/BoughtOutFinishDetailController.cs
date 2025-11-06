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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CWB.Masters.Controllers
{
    [ApiController]
    [Authorize(Roles = Roles.ADMIN)]
    public class BoughtOutFinishDetailController : ControllerBase
    {
        private readonly ILoggerManager _logger;
        private readonly IBoughtOutFinishDetailService _boughtOutFinishDetailService;
        private readonly IMasterPartService _masterPartService;

        public BoughtOutFinishDetailController(ILoggerManager logger, 
            IBoughtOutFinishDetailService boughtOutFinishDetailService,
            IMasterPartService masterPartService)
        {
            _logger = logger;
            _boughtOutFinishDetailService = boughtOutFinishDetailService;
            _masterPartService = masterPartService;
        }


        [HttpGet]
        [Route(ApiRoutes.ManufacturedPartNoDetail.GetBOFPart)]
        [Produces(AppContentTypes.ContentType, Type = typeof(BoughtOutFinishDetailVM))]
        public async Task<IActionResult> GetBOFPart(int partId, long tenantId)
        {
            BoughtOutFinishDetailVM manufP = await _masterPartService.GetBOFPart(partId, tenantId);
            return Ok(manufP);
        }

        /// <summary>
        /// Get BoughtOutFinishDetail List by Tenant Id
        /// </summary>
        /// <param name="TenantId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route(ApiRoutes.BoughtOutFinishDetail.GetBoughtOutFinishDetailList)]
        [Produces(AppContentTypes.ContentType, Type = typeof(List<BoughtOutFinishDetailVM>))]
        public IActionResult GetBoughtOutFinishDetailList(long tenantId)
        {
            var boughtoutfinishdetails = _boughtOutFinishDetailService.GetBoughtOutFinishDetailsByTenant(tenantId);
            return Ok(boughtoutfinishdetails);
        }



        [HttpGet]
        [Route(ApiRoutes.BoughtOutFinishDetail.GetBOfLikeManufPart)]
        [Produces(AppContentTypes.ContentType, Type = typeof(List<ManufacturedPartNoDetailVM>))]
        public IActionResult GetBOfLikeManufPart(long tenantID)
        {
            List<MasterPartVM> masterParts = _masterPartService.GetAllMasterParts().ToList();
            List<BoughtOutFinishDetailVM> manufList = _boughtOutFinishDetailService.GetBoughtOutFinishDetailsByTenant(tenantID).ToList();

            var query = from manuf in manufList
                        join mp in masterParts on manuf.PartId equals mp.MasterPartId into mpjoin
                        from scojoin in mpjoin.DefaultIfEmpty()
                        select new ManufacturedPartNoDetailVM
                        {
                            ManufacturedPartType = manuf.BoughtOutFinishMadeType,
                            PartId = scojoin.MasterPartId,
                            UOMId = manuf.UOMId,
                            ManufacturedPartNoDetailId = (long)manuf.BoughtOutFinishDetailId,
                            PartNo = scojoin.PartNo,
                            PartDescription = scojoin.PartDescription,
                            RevNo = scojoin.RevNo,
                            RevDate = scojoin.RevDate,
                            Status = Convert.ToString(scojoin.Status),
                            StatusChangeReason = scojoin.StatusChangeReason,
                            MasterPartType = Convert.ToString(scojoin.MasterPartType)
                        };
            var querylist = query.ToList();
            return Ok(querylist);
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
            var result = await _boughtOutFinishDetailService.BoughtOutFinishDetail(boughtOutFinishDetailVM);
            return Ok(result);
        }
    }
}
