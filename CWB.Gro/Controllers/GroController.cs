using CWB.CommonUtils.Common;
using CWB.Constants.UserIdentity;
using CWB.Logging;
using CWB.Gro.Services;
using CWB.Gro.GroUtils;
using CWB.Gro.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CWB.Gro.Controllers
{
   
    [ApiController]
    [Authorize]
    public class GroController : ControllerBase
    {
        private readonly ILoggerManager _logger;
        private readonly IGroService _groService;
        public GroController(ILoggerManager logger, IGroService groservice)
        {
            _logger = logger;
            _groService = groservice;
        }
        [HttpGet]
        [Route(ApiRoutes.Gro.GetAllGroData)]
        [Produces(AppContentTypes.ContentType, Type = typeof(List<Gro_DataVM>))]
        [Authorize(Roles = Roles.ADMIN)]
        public async Task<IActionResult> AllGroData(long tenantId)
        {
            var grodata = await _groService.AllGroData(tenantId);
            return Ok(grodata);
        }
        [HttpPost]
        [Route(ApiRoutes.Gro.PostGrodata)]
        [Produces(AppContentTypes.ContentType, Type = typeof(Gro_DataVM))]
        public async Task<IActionResult> PostGrodata([FromBody] Gro_DataVM workOrdersVM)
        {
            var result = await _groService.PostGrodata(workOrdersVM);
            return Ok(result);
        }
        [HttpPost]
        [Route(ApiRoutes.Gro.PostMultipleGroData)]
        [Produces(AppContentTypes.ContentType, Type = typeof(Gro_DataVM))]
        public async Task<IActionResult> PostMultipleGroData([FromBody] List<Gro_DataVM> workOrdersVM)
        {
            var result = await _groService.MultipleGrodata(workOrdersVM);
            return Ok(result);
        }
        [HttpGet]
        [Route(ApiRoutes.Gro.DeleteGroData)]
        [Produces(AppContentTypes.ContentType, Type = typeof(bool))]
        public async Task<IActionResult> DeleteGroData(long Id)
        {
            var result = await _groService.DeleteGroData(Id);
            return Ok(result);
        }
        [HttpGet]
        [Route(ApiRoutes.Gro.AllGroPartList)]
        [Produces(AppContentTypes.ContentType, Type = typeof(List<Gro_Part_ListVM>))]
        [Authorize(Roles = Roles.ADMIN)]
        public async Task<IActionResult> AllGroPartList(long tenantId)
        {
            var grodata = await _groService.AllGroPartList(tenantId);
            return Ok(grodata);
        }
        [HttpPost]
        [Route(ApiRoutes.Gro.PostGroPartList)]
        [Produces(AppContentTypes.ContentType, Type = typeof(Gro_Part_ListVM))]
        public async Task<IActionResult> PostGroPartList([FromBody] Gro_Part_ListVM workOrdersVM)
        {
            var result = await _groService.PostGroPartList(workOrdersVM);
            return Ok(result);
        }
        [HttpPost]
        [Route(ApiRoutes.Gro.PostMultiplePartList)]
        [Produces(AppContentTypes.ContentType, Type = typeof(Gro_Part_ListVM))]
        public async Task<IActionResult> PostMultiplePartList([FromBody] List<Gro_Part_ListVM> workOrdersVM)
        {
            var result = await _groService.MultipleGroPartList(workOrdersVM);
            return Ok(result);
        }
        [HttpGet]
        [Route(ApiRoutes.Gro.DeleteGroPartList)]
        [Produces(AppContentTypes.ContentType, Type = typeof(bool))]
        public async Task<IActionResult> DeleteGroPartList(long Id)
        {
            var result = await _groService.DeleteGroPartList(Id);
            return Ok(result);
        }

        [HttpGet]
        [Route(ApiRoutes.Gro.AllGroDispatchHeader)]
        [Produces(AppContentTypes.ContentType, Type = typeof(List<Gro_Disp_HeaderVM>))]
        [Authorize(Roles = Roles.ADMIN)]
        public async Task<IActionResult> AllGroDispatchHeader(long tenantId)
        {
            var grodata = await _groService.AllGroDispatchHeader(tenantId);
            return Ok(grodata);
        }
        [HttpPost]
        [Route(ApiRoutes.Gro.PostGroDispatchHeader)]
        [Produces(AppContentTypes.ContentType, Type = typeof(Gro_DataVM))]
        public async Task<IActionResult> PostGroDispatchHeader([FromBody] Gro_Disp_HeaderVM workOrdersVM)
        {
            var result = await _groService.PostGroDispatchHeader(workOrdersVM);
            return Ok(result);
        }
        [HttpPost]
        [Route(ApiRoutes.Gro.UpdateGroDispHeaderAddress)]
        [Produces(AppContentTypes.ContentType, Type = typeof(Gro_DataVM))]
        public async Task<IActionResult> UpdateGroDispHeaderAddress([FromBody] Gro_Disp_HeaderVM workOrdersVM)
        {
            var result = await _groService.UpdateGroDispatchHeader(workOrdersVM);
            return Ok(result);
        }
        [HttpPost]
        [Route(ApiRoutes.Gro.UpdateGroDispatchHeaderAWB)]
        [Produces(AppContentTypes.ContentType, Type = typeof(Gro_DataVM))]
        public async Task<IActionResult> UpdateGroDispatchHeaderAWB([FromBody] Gro_Disp_HeaderVM workOrdersVM)
        {
            var result = await _groService.UpdateGroDispatchHeaderAWB(workOrdersVM);
            return Ok(result);
        }
        [HttpPost]
        [Route(ApiRoutes.Gro.UpdateGroDispatchHeaderDeliveryDate)]
        [Produces(AppContentTypes.ContentType, Type = typeof(Gro_DataVM))]
        public async Task<IActionResult> UpdateGroDispatchHeaderDeliveryDate([FromBody] Gro_Disp_HeaderVM workOrdersVM)
        {
            var result = await _groService.UpdateGroDispatchHeaderDeliveryDate(workOrdersVM);
            return Ok(result);
        }

        [HttpPost]
        [Route(ApiRoutes.Gro.PostMultipleGroDispatchHeader)]
        [Produces(AppContentTypes.ContentType, Type = typeof(Gro_DataVM))]
        public async Task<IActionResult> PostMultipleGroDispatchHeader([FromBody] List<Gro_Disp_HeaderVM> workOrdersVM)
        {
            var result = await _groService.MultipleGroDispatchHeader(workOrdersVM);
            return Ok(result);
        }
        [HttpGet]
        [Route(ApiRoutes.Gro.DeleteGroDispatchHeader)]
        [Produces(AppContentTypes.ContentType, Type = typeof(bool))]
        public async Task<IActionResult> DeleteGroDispatchHeader(long Id)
        {
            var result = await _groService.DeleteGroDispatchHeader(Id);
            return Ok(result);
        }

        [HttpGet]
        [Route(ApiRoutes.Gro.AllCustSpecificData)]
        [Produces(AppContentTypes.ContentType, Type = typeof(List<Cust_Specific_DataVM>))]
        [Authorize(Roles = Roles.ADMIN)]
        public async Task<IActionResult> AllCustSpecificData(long tenantId)
        {
            var grodata = await _groService.AllCustSpecificData(tenantId);
            return Ok(grodata);
        }
        [HttpPost]
        [Route(ApiRoutes.Gro.PostCustSpecificData)]
        [Produces(AppContentTypes.ContentType, Type = typeof(Cust_Specific_DataVM))]
        public async Task<IActionResult> PostCustSpecificData([FromBody] Cust_Specific_DataVM workOrdersVM)
        {
            var result = await _groService.PostCustSpecificData(workOrdersVM);
            return Ok(result);
        }
        [HttpPost]
        [Route(ApiRoutes.Gro.PostMultipleCustSpecificData)]
        [Produces(AppContentTypes.ContentType, Type = typeof(Cust_Specific_DataVM))]
        public async Task<IActionResult> PostMultipleCustSpecificData([FromBody] List<Cust_Specific_DataVM> workOrdersVM)
        {
            var result = await _groService.MultipleCustSpecificData(workOrdersVM);
            return Ok(result);
        }
        [HttpGet]
        [Route(ApiRoutes.Gro.DeleteCustSpecificData)]
        [Produces(AppContentTypes.ContentType, Type = typeof(bool))]
        public async Task<IActionResult> DeleteCustSpecificData(long Id)
        {
            var result = await _groService.DeleteCustSpecificData(Id);
            return Ok(result);
        }


        [HttpGet]
        [Route(ApiRoutes.Gro.AllGroDispatchDetails)]
        [Produces(AppContentTypes.ContentType, Type = typeof(List<Gro_Disp_DetVM>))]
        [Authorize(Roles = Roles.ADMIN)]
        public async Task<IActionResult> AllGroDispatchDetails(long tenantId)
        {
            var grodata = await _groService.AllGroDispatchDetails(tenantId);
            return Ok(grodata);
        }
        [HttpPost]
        [Route(ApiRoutes.Gro.PostGroDispatchDetail)]
        [Produces(AppContentTypes.ContentType, Type = typeof(Gro_Disp_DetVM))]
        public async Task<IActionResult> PostGroDispatchDetail([FromBody] Gro_Disp_DetVM workOrdersVM)
        {
            var result = await _groService.PostGroDispatchDetail(workOrdersVM);
            return Ok(result);
        }
        [HttpPost]
        [Route(ApiRoutes.Gro.PostMultipleGroDispatchDetails)]
        [Produces(AppContentTypes.ContentType, Type = typeof(Gro_Disp_DetVM))]
        public async Task<IActionResult> PostMultipleGroDispatchDetails([FromBody] List<Gro_Disp_DetVM> workOrdersVM)
        {
            var result = await _groService.MultipleGroDispatchDetails(workOrdersVM);
            return Ok(result);
        }
        [HttpGet]
        [Route(ApiRoutes.Gro.DeleteGroDispatchDetail)]
        [Produces(AppContentTypes.ContentType, Type = typeof(bool))]
        public async Task<IActionResult> DeleteGroDispatchDetail(long Id)
        {
            var result = await _groService.DeleteGroDispatchDetail(Id);
            return Ok(result);
        }



        [HttpGet]
        [Route(ApiRoutes.Gro.AllUploadformats)]
        [Produces(AppContentTypes.ContentType, Type = typeof(List<Upload_FormatVM>))]
        [Authorize(Roles = Roles.ADMIN)]
        public async Task<IActionResult> AllUploadformats(long tenantId)
        {
            var grodata = await _groService.AllUploadformats(tenantId);
            return Ok(grodata);
        }
        [HttpPost]
        [Route(ApiRoutes.Gro.PostUploadFormat)]
        [Produces(AppContentTypes.ContentType, Type = typeof(Upload_FormatVM))]
        public async Task<IActionResult> PostUploadFormat([FromBody] Upload_FormatVM workOrdersVM)
        {
            var result = await _groService.PostUploadFormat(workOrdersVM);
            return Ok(result);
        }
        [HttpPost]
        [Route(ApiRoutes.Gro.PostMultipleUploadFormats)]
        [Produces(AppContentTypes.ContentType, Type = typeof(Upload_FormatVM))]
        public async Task<IActionResult> PostMultipleUploadFormats([FromBody] List<Upload_FormatVM> workOrdersVM)
        {
            var result = await _groService.MultipleUploadFormats(workOrdersVM);
            return Ok(result);
        }
        [HttpGet]
        [Route(ApiRoutes.Gro.DeleteUploadformat)]
        [Produces(AppContentTypes.ContentType, Type = typeof(bool))]
        public async Task<IActionResult> DeleteUploadformat(long Id)
        {
            var result = await _groService.DeleteUploadformat(Id);
            return Ok(result);
        }
        [HttpGet]
        [Route(ApiRoutes.Gro.GetFieldType)]
        [Produces(AppContentTypes.ContentType, Type = typeof(Field_TypeVM))]
        [Authorize(Roles = Roles.ADMIN)]
        public async Task<IActionResult> GetFeildType(long Id)
        {
            var allprocplan = await _groService.GetFeildType(Id);
            return Ok(allprocplan);
        }

        [HttpGet]
        [Route(ApiRoutes.Gro.AllCourierlist)]
        [Produces(AppContentTypes.ContentType, Type = typeof(List<Courier_ListVM>))]
        [Authorize(Roles = Roles.ADMIN)]
        public async Task<IActionResult> AllCourierlist(long tenantId)
        {
            var grodata = await _groService.AllCourierlist(tenantId);
            return Ok(grodata);
        }
        [HttpPost]
        [Route(ApiRoutes.Gro.PostCourierList)]
        [Produces(AppContentTypes.ContentType, Type = typeof(Courier_ListVM))]
        public async Task<IActionResult> PostCourierList([FromBody] Courier_ListVM workOrdersVM)
        {
            var result = await _groService.PostCourierList(workOrdersVM);
            return Ok(result);
        }
        [HttpPost]
        [Route(ApiRoutes.Gro.PostMultipleCourierList)]
        [Produces(AppContentTypes.ContentType, Type = typeof(Courier_ListVM))]
        public async Task<IActionResult> PostMultipleCourierList([FromBody] List<Courier_ListVM> workOrdersVM)
        {
            var result = await _groService.MultipleCourierList(workOrdersVM);
            return Ok(result);
        }
        [HttpGet]
        [Route(ApiRoutes.Gro.DeleteCourier)]
        [Produces(AppContentTypes.ContentType, Type = typeof(bool))]
        public async Task<IActionResult> DeleteCourier(long Id)
        {
            var result = await _groService.DeleteCourier(Id);
            return Ok(result);
        }


        [HttpGet]
        [Route(ApiRoutes.Gro.AllPrintoutformats)]
        [Produces(AppContentTypes.ContentType, Type = typeof(List<Printout_formatVM>))]
        [Authorize(Roles = Roles.ADMIN)]
        public async Task<IActionResult> AllPrintoutformats(long tenantId)
        {
            var grodata = await _groService.AllPrintoutformats(tenantId);
            return Ok(grodata);
        }
        [HttpPost]
        [Route(ApiRoutes.Gro.PostPrintoutformat)]
        [Produces(AppContentTypes.ContentType, Type = typeof(Printout_formatVM))]
        public async Task<IActionResult> PostPrintoutformat([FromBody] Printout_formatVM workOrdersVM)
        {
            var result = await _groService.PostPrintoutformat(workOrdersVM);
            return Ok(result);
        }
        [HttpPost]
        [Route(ApiRoutes.Gro.PostMultiplePrintoutFormat)]
        [Produces(AppContentTypes.ContentType, Type = typeof(Printout_formatVM))]
        public async Task<IActionResult> PostMultiplePrintoutFormat([FromBody] List<Printout_formatVM> workOrdersVM)
        {
            var result = await _groService.MultiplePrintoutFormat(workOrdersVM);
            return Ok(result);
        }
        [HttpGet]
        [Route(ApiRoutes.Gro.DeletePrintoutformat)]
        [Produces(AppContentTypes.ContentType, Type = typeof(bool))]
        public async Task<IActionResult> DeletePrintoutformat(long Id)
        {
            var result = await _groService.DeletePrintoutformat(Id);
            return Ok(result);
        }

        [HttpGet]
        [Route(ApiRoutes.Gro.AllGroStockList)]
        [Produces(AppContentTypes.ContentType, Type = typeof(List<Gro_Stock_ListVM>))]
        [Authorize(Roles = Roles.ADMIN)]
        public async Task<IActionResult> AllGroStockList(long tenantId)
        {
            var grodata = await _groService.AllGroStockList(tenantId);
            return Ok(grodata);
        }
        [HttpPost]
        [Route(ApiRoutes.Gro.PostGroStockList)]
        [Produces(AppContentTypes.ContentType, Type = typeof(Gro_Stock_ListVM))]
        public async Task<IActionResult> PostGroStockList([FromBody] Gro_Stock_ListVM workOrdersVM)
        {
            var result = await _groService.PostGroStockList(workOrdersVM);
            return Ok(result);
        }
        [HttpPost]
        [Route(ApiRoutes.Gro.PostMultipleGroStockList)]
        [Produces(AppContentTypes.ContentType, Type = typeof(Gro_Stock_ListVM))]
        public async Task<IActionResult> PostMultipleGroStockList([FromBody] List<Gro_Stock_ListVM> workOrdersVM)
        {
            var result = await _groService.MultipleGroStockList(workOrdersVM);
            return Ok(result);
        }
        [HttpGet]
        [Route(ApiRoutes.Gro.DeleteGroStockList)]
        [Produces(AppContentTypes.ContentType, Type = typeof(bool))]
        public async Task<IActionResult> DeleteGroStockList(long Id)
        {
            var result = await _groService.DeleteGroStockList(Id);
            return Ok(result);
        }
        [HttpGet]
        [Route(ApiRoutes.Gro.GetStockByGroPartListId)]
        [Produces(AppContentTypes.ContentType, Type = typeof( Gro_Stock_ListVM))]
        [Authorize(Roles = Roles.ADMIN)]
        public async Task<IActionResult> GetStockByGroPartListId(long gropartlistid,long tenantId)
        {
            var grodata = await _groService.GetStockByGroPartListId(gropartlistid,tenantId);
            return Ok(grodata);
        }


        [HttpGet]
        [Route(ApiRoutes.Gro.AllTKDCInvContrl)]
        [Produces(AppContentTypes.ContentType, Type = typeof(List<TK_DC_Inv_ContrlVM>))]
        [Authorize(Roles = Roles.ADMIN)]
        public async Task<IActionResult> AllTKDCInvContrl(long tenantId)
        {
            var grodata = await _groService.AllTKDCInvContrl(tenantId);
            return Ok(grodata);
        }
        [HttpPost]
        [Route(ApiRoutes.Gro.PostTKDCInvContrl)]
        [Produces(AppContentTypes.ContentType, Type = typeof(TK_DC_Inv_ContrlVM))]
        public async Task<IActionResult> PostTKDCInvContrl([FromBody] TK_DC_Inv_ContrlVM workOrdersVM)
        {
            var result = await _groService.PostTKDCInvContrl(workOrdersVM);
            return Ok(result);
        }
        [HttpPost]
        [Route(ApiRoutes.Gro.UpdateTkDclastDcandInvNo)]
        [Produces(AppContentTypes.ContentType, Type = typeof(TK_DC_Inv_ContrlVM))]
        public async Task<IActionResult> UpdateTkDclastDcandInvNo([FromBody] TK_DC_Inv_ContrlVM workOrdersVM)
        {
            var result = await _groService.UpdateTkDclastDcandInvNo(workOrdersVM);
            return Ok(result);
        }
        [HttpPost]
        [Route(ApiRoutes.Gro.PostMultipleTKDCInvContrl)]
        [Produces(AppContentTypes.ContentType, Type = typeof(TK_DC_Inv_ContrlVM))]
        public async Task<IActionResult> PostMultipleTKDCInvContrl([FromBody] List<TK_DC_Inv_ContrlVM> workOrdersVM)
        {
            var result = await _groService.MultipleTKDCInvContrl(workOrdersVM);
            return Ok(result);
        }
        [HttpGet]
        [Route(ApiRoutes.Gro.DeleteTKDCInvContrl)]
        [Produces(AppContentTypes.ContentType, Type = typeof(bool))]
        public async Task<IActionResult> DeleteTKDCInvContrl(long Id)
        {
            var result = await _groService.DeleteTKDCInvContrl(Id);
            return Ok(result);
        }

        [HttpGet]
        [Route(ApiRoutes.Gro.AllGroStockDet)]
        [Produces(AppContentTypes.ContentType, Type = typeof(List<Gro_Stock_DetVM>))]
        [Authorize(Roles = Roles.ADMIN)]
        public async Task<IActionResult> AllGroStockDet(long tenantId)
        {
            var grodata = await _groService.AllGroStockDet(tenantId);
            return Ok(grodata);
        }
        [HttpPost]
        [Route(ApiRoutes.Gro.PostGroStockDet)]
        [Produces(AppContentTypes.ContentType, Type = typeof(Gro_Stock_DetVM))]
        public async Task<IActionResult> PostGroStockDet([FromBody] Gro_Stock_DetVM workOrdersVM)
        {
            var result = await _groService.PostGroStockDet(workOrdersVM);
            return Ok(result);
        }
        [HttpPost]
        [Route(ApiRoutes.Gro.PostMultipleGroStockDet)]
        [Produces(AppContentTypes.ContentType, Type = typeof(Gro_Stock_DetVM))]
        public async Task<IActionResult> PostMultipleGroStockDet([FromBody] List<Gro_Stock_DetVM> workOrdersVM)
        {
            var result = await _groService.MultipleGroStockDet(workOrdersVM);
            return Ok(result);
        }
        [HttpGet]
        [Route(ApiRoutes.Gro.DeleteGroStockDet)]
        [Produces(AppContentTypes.ContentType, Type = typeof(bool))]
        public async Task<IActionResult> DeleteGroStockDet(long Id)
        {
            var result = await _groService.DeleteGroStockDet(Id);
            return Ok(result);
        }

        [HttpGet]
        [Route(ApiRoutes.Gro.GetSLstatustype)]
        [Produces(AppContentTypes.ContentType, Type = typeof(Sl_No_Status_ListVM))]
        [Authorize(Roles = Roles.ADMIN)]
        public async Task<IActionResult> GetSLstatustype(long Id)
        {
            var allprocplan = await _groService.GetSLstatustype(Id);
            return Ok(allprocplan);
        }


        [HttpGet]
        [Route(ApiRoutes.Gro.AllIndentPartSlNo)]
        [Produces(AppContentTypes.ContentType, Type = typeof(List<Indent_Part_Sl_NoVM>))]
        [Authorize(Roles = Roles.ADMIN)]
        public async Task<IActionResult> AllIndentPartSlNo(long tenantId)
        {
            var grodata = await _groService.AllIndentPartSlNo(tenantId);
            return Ok(grodata);
        }
        [HttpPost]
        [Route(ApiRoutes.Gro.PostIndentPartSlNo)]
        [Produces(AppContentTypes.ContentType, Type = typeof(Indent_Part_Sl_NoVM))]
        public async Task<IActionResult> PostIndentPartSlNo([FromBody] Indent_Part_Sl_NoVM workOrdersVM)
        {
            var result = await _groService.PostIndentPartSlNo(workOrdersVM);
            return Ok(result);
        }
        [HttpPost]
        [Route(ApiRoutes.Gro.PostMultipleIndentPartSlNo)]
        [Produces(AppContentTypes.ContentType, Type = typeof(Indent_Part_Sl_NoVM))]
        public async Task<IActionResult> PostMultipleIndentPartSlNo([FromBody] List<Indent_Part_Sl_NoVM> workOrdersVM)
        {
            var result = await _groService.MultipleIndentPartSlNo(workOrdersVM);
            return Ok(result);
        }
        [HttpGet]
        [Route(ApiRoutes.Gro.DeleteIndentPartSlNo)]
        [Produces(AppContentTypes.ContentType, Type = typeof(bool))]
        public async Task<IActionResult> DeleteIndentPartSlNo(long Id)
        {
            var result = await _groService.DeleteIndentPartSlNo(Id);
            return Ok(result);
        }


    }
}
