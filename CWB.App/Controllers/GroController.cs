using CWB.App.AppUtils;
using CWB.App.Models.BusinessProcesses;
using CWB.App.Models.ItemMaster;
using CWB.App.Models.Routing;
using CWB.App.Services.BusinessProcesses;
using CWB.App.Services.Masters;
using CWB.App.Services.ProductionPlanWo;
using CWB.App.Services.Routings;
using CWB.App.Services.CompanySettings;
using CWB.App.Services.Gro;
using CWB.App.Models.Contacts;
using CWB.App.Models.Gro;
using CWB.Constants.UserIdentity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System;
using CWB.App.Models.WorkOrder;
using Microsoft.AspNetCore.Http;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System.IO;
using QRCoder;
namespace CWB.App.Controllers
{
    [Authorize(Roles = Roles.ADMIN)]
    public class GroController : Controller
    {
        private readonly ILogger<GroController> _logger;
        private readonly IGroService _groservicee;
        private readonly IMastersServices _masterservice;
        public GroController(ILogger<GroController> logger,IGroService groservicee,IMastersServices mastersservice
            )
        {
            _logger = logger;
            _groservicee = groservicee;
            _masterservice = mastersservice;

        }
        public IActionResult Index()
        {
            _logger.LogTrace("Gro--Index--Loading");
            return View();
        }
        [Route("~/G!@S$%T()P ")]
        public IActionResult GroSetup()
        {
            _logger.LogTrace("Gro--GroSetup--Loading");
            return View();
        }
        [Route("~/G!@#S$%O*&K")]
        public IActionResult GroStock()
        {
            _logger.LogTrace("Gro--stock--Loading");
            return View();
        }
        [Route("~/G!@#D$%O*&M")]
        public IActionResult GroDocumentManagement()
        {
            _logger.LogTrace("Gro--Document--Loading");
            return View();
        }
        [Route("~/G!@#F$%P*&U")]
        public IActionResult GroFinalPackUpdate()
        {
            _logger.LogTrace("Gro--FinalPack--Loading");
            return View();
        }
        [HttpPost]
        public async Task<IActionResult>UploadGroData(IFormFile uploadedFile)
        {
            var result =  await _groservicee.ValidateExcel(uploadedFile);

            if (!result.Success)
            {
                return Json(result);
            }
            IWorkbook workbook;
            int indentCount = 0;
            int productCount = 0;
            using (var stream = uploadedFile.OpenReadStream())
            {
                string ext =
                    Path.GetExtension(uploadedFile.FileName)
                    .ToLower();

                if (ext == ".xlsx")
                {
                    workbook = new XSSFWorkbook(stream);
                }
                else
                {
                    workbook = new HSSFWorkbook(stream);
                }

                ISheet sheet = workbook.GetSheetAt(0);
                var custspecificdata1 = await _groservicee.GetallCustSpecificData();
                var custspecificdata = custspecificdata1.FirstOrDefault();
                long startingrowno;
                if(custspecificdata==null)
                {
                    startingrowno = 1;
                }
                else
                {
                    startingrowno = custspecificdata.Last_Upload_Row_No;
                }

                long lastRow = sheet.LastRowNum;
                var groDataList = new List<Gro_DataVM>();
                //var companies = (await _masterservice.GetCompaniesGro()).ToList();
                var allgrostock = (await _groservicee.Getallgrostocklist()).ToList();
                var allgropartno = (await _groservicee.Getallgroparts()).ToList();
                var allgrodata = (await _groservicee.GetallgroData()).ToList();
                for (long rowNo = startingrowno; rowNo <= lastRow; rowNo++)
                {
                    IRow row = sheet.GetRow((int)rowNo);

                    if (row == null)
                    {
                        continue;
                    }

                    //string companyName = row.GetCell(3)?.ToString()?.Trim();
                    var getcompabybyname = await _masterservice.GetCompanybyName("Gro");
                   // var company = companies.FirstOrDefault(c => c.CompanyName.Trim().ToUpper() == companyName.ToUpper());

                    //--------------------------------------------------
                    // COMPANY CHECK
                    //--------------------------------------------------
                    long companyId;

                    if (getcompabybyname == null)
                    {
                        var companyVm =
                            new ContactsVM
                            {
                                CompanyName = "Gro",
                                CompanyType = "Customer"
                            };

                        var postcompany = await _masterservice.PostGroCompany(companyVm);

                        companyId = postcompany.CompanyId;
                        //companies.Add(postcompany);
                    }
                    else
                    {
                        companyId = getcompabybyname.CompanyId;
                    }


                    string partNo =
                        row.GetCell(4)?.ToString()?.Trim();

                    // PART CHECK
                    var part = allgropartno.FirstOrDefault(x =>
                        x.Gro_Part_No.Trim().ToUpper() == partNo.Trim().ToUpper());

                    long partId;

                    if (part == null)
                    {
                        var partVm = new Gro_Part_ListVM
                        {
                            Gro_Part_No = partNo,
                            Part_Status = 1,
                            MRP = 0,
                            Part_No = 0,
                            Data_Update = DateTime.Now,
                            Update_By = 1
                        };

                        var postedgropartno = await _groservicee.PostGroPart(partVm);
                        partId = postedgropartno.Gro_Part_ListId;

                        allgropartno.Add(postedgropartno);
                    }
                    else
                    {
                        partId = part.Gro_Part_ListId;
                    }

                    // STOCK CHECK (ONLY ONCE)
                    var grostockpart = allgrostock
                        .FirstOrDefault(x => x.Gro_Part_List_ID == partId);

                    if (grostockpart == null)
                    {
                        var newgrostockpart = new Gro_Stock_ListVM
                        {
                            Gro_Stock_ListId = 0,
                            Gro_Part_List_ID = partId,
                            Last_Sl_No = 0,
                            Qnty_on_Hand = 0,
                            Correction_User = 1
                        };

                        var postgrostock = await _groservicee.PostGroStockpart(newgrostockpart);

                        allgrostock.Add(postgrostock);
                    }

                    //--------------------------------------------------
                    // DUPLICATE INDENT CHECK
                    //--------------------------------------------------
                    string indentNo =
                       row.GetCell(2)?.ToString()?.Trim();

                    DateTime sentDate = Convert.ToDateTime(row.GetCell(0)?.ToString());
                    var existingIndentRows = allgrodata.Where(x => x.CWB_Customer == companyId &&
                   x.Indent.Trim().ToUpper() == indentNo.ToUpper()).ToList();

                    if (existingIndentRows.Any())
                    {
                        var earliestDate =
                            existingIndentRows
                            .Min(x => x.SentDate);

                        //if (earliestDate != sentDate.Date)
                        //{
                            return Json(new
                            {
                                Success = false,
                                Message =
                                    $"Indent {indentNo} already appears on " +
                                    $"{earliestDate:dd-MM-yyyy}. " +
                                    $"Upload aborted."
                            });
                      //  }
                    }
                    //indentSet.Add(indentNo.ToUpper());
                    ////--------------------------------------------------
                    //// Add GRO DATA
                    ////--------------------------------------------------
                    var groVm = new Gro_DataVM();

                    groVm.CWB_Customer = companyId;
                    groVm.int_Part_No = 0;
                    groVm.SentDate = DateTime.Parse(row.GetCell(0)?.ToString());
                    groVm.Excutive_Name = row.GetCell(1)?.ToString();
                    groVm.Indent = row.GetCell(2)?.ToString();
                    groVm.Gro_Part_No = partId;

                    groVm.Reqd_Quantity = Convert.ToInt32(row.GetCell(5)?.ToString());
                    groVm.Company_Name = row.GetCell(3)?.ToString()?.Trim(); 
                    groVm.Remarks = row.GetCell(6)?.ToString();
                    groVm.Shipping_Address = row.GetCell(7)?.ToString();
                    groVm.Shipping_City = row.GetCell(8)?.ToString();
                    groVm.Shipping_State = row.GetCell(9)?.ToString();

                    groVm.Shipping_PINCODE = row.GetCell(10)?.ToString();
                    groVm.Contact_Person = row.GetCell(11)?.ToString();
                    groVm.Contact_Person_No = row.GetCell(12)?.ToString();
                    groVm.Part_Not_Avl = 'N';
                    groVm.Bal_to_Disp = Convert.ToInt32(row.GetCell(5)?.ToString());
                    groDataList.Add(groVm);
                    //allgrodata.Add(groVm);

                }

                if (custspecificdata == null)
                {
                    custspecificdata = new Cust_Specific_DataVM();
                    custspecificdata.File_Location = "Excel";
                    custspecificdata.Last_Upload_Row_No = lastRow + 1;
                    custspecificdata.Last_Upload_date = DateTime.Now;
                    custspecificdata.Cust_Specific_DataId = 0;
                    custspecificdata.CWB_Customer = 0;
                    custspecificdata.UI_ID = 0;
                    var postcustdata = await _groservicee.PostCustSpecificData(custspecificdata);

                }
                else
                {
                    custspecificdata.Last_Upload_Row_No = lastRow + 1;
                    custspecificdata.Last_Upload_date = DateTime.Now;
                    var postcustdata = await _groservicee.PostCustSpecificData(custspecificdata);
                }

                var postmultiplegrodata = await _groservicee.PostMultipleGrodata(groDataList);

                 indentCount = groDataList
                    .Select(x => x.Indent)
                    .Distinct()
                    .Count();

                 productCount = groDataList
                                    .Sum(x => x.Reqd_Quantity);
            }

            return Json(new
            {
                Success = true,
                Message = "Upload Completed Successfully",
                IndentCount = indentCount,
                ProductCount = productCount
            });
        }
        [HttpGet]
        public async Task<IActionResult> GetGroUploadSummary()
        {
            var custSpecific =
                (await _groservicee.GetallCustSpecificData())
                .FirstOrDefault();

            var groData =
                await _groservicee.GetallgroData();

            return Json(new
            {
                lastUpdatedDate = custSpecific?.Last_Upload_date?
                                        .ToString("dd-MM-yyyy")
                                        ?? "--",
                indentCount = groData
                                .Select(x => x.Indent)
                                .Distinct()
                                .Count(),
                productCount = groData
                                .Sum(x => x.Reqd_Quantity)
            });
        }
        [HttpGet]
        public async Task<IActionResult> GetDispatchSelection()
        {
            var allgropartno = await _groservicee.Getallgroparts();
            var allgrodata = await _groservicee.GetallgroData();
            var allgrostocklist = await _groservicee.Getallgrostocklist();
            var filteredgrodata = allgrodata.Where(x => x.Bal_to_Disp > 0).ToList();
             List <Gro_DataVM> result = new List<Gro_DataVM>();
            foreach (var item in filteredgrodata)
            {
                var part = allgropartno .FirstOrDefault(x => x.Gro_Part_ListId == item.Gro_Part_No);

                item.GroPartNo = part.Gro_Part_No??"";
                 
                item.IndentDateStr = item.SentDate?.ToString("dd-MM-yyyy") ?? "";
                var days = (DateTime.Today - item.SentDate.Value.Date).Days;

                if (days <= 0)
                    item.Ageing = 0;
                else if (days == 1)
                    item.Ageing = 1;
                else if (days == 2)
                    item.Ageing = 2;
                else if (days == 3)
                    item.Ageing = 3;
                else
                    item.Ageing = 4;
                var gropratstock = allgrostocklist.FirstOrDefault(x => x.Gro_Part_List_ID == item.Gro_Part_No);
                if (item.Bal_to_Disp == item.Reqd_Quantity)
                {
                    item.DispatchStatus = "No Dispatch";
                }
                else if (item.Bal_to_Disp < item.Reqd_Quantity)
                {
                    item.DispatchStatus = "Partial Dispatch";
                }
                if (gropratstock==null)
                {
                    item.QntyAval = 0;
                }
                else
                {
                    item.QntyAval = gropratstock.Qnty_on_Hand;
                }
                result.Add(item);

            }
            
            //var result = await _groservice.GetDispatchSelection();

            return Ok(result);
        }
        [HttpGet]
        public async Task<IActionResult> GetDispatchData(string indentNo)
        {
            var allgrodispdetails = await _groservicee.GetallgroDispatchDetails(); 
            var allgropartno = await _groservicee.Getallgroparts();
            var allgrodata = await _groservicee.GetallgroData();
            var allgrostocklist = await _groservicee.Getallgrostocklist();
            List<Gro_DataVM> result = new List<Gro_DataVM>();
            var grodatabyindent = allgrodata.Where(x => x.Indent == indentNo && x.Bal_to_Disp>=0).ToList();
            
            var allgrodispheader = await _groservicee.GetallgroDispHeader();
            var grodispheaderbyindent = allgrodispheader.Where(x => x.Indent == indentNo && x.Dispatched=='N').FirstOrDefault();
           // var createdheader=null ;
            if(grodispheaderbyindent==null)
            {
                var tkdc = await _groservicee.Getalltkdcinvctrl();
                var firstrecord = tkdc.FirstOrDefault();
                long nextDcNo = firstrecord.TK_DC_Last_No + 1;
                long nextInvNo = firstrecord.TK_Inv_Last_No + 1;
                string dcNo = nextDcNo.ToString("D7");     // 0000003
                string invNo = nextInvNo.ToString("D7");   // 0000003

                var grodispheaderdata = new Gro_Disp_HeaderVM();
                grodispheaderdata.CWB_Customer = grodatabyindent.First().CWB_Customer;
                grodispheaderdata.Indent = grodatabyindent.First().Indent;
                grodispheaderdata.SentDate = grodatabyindent.First().SentDate;
                grodispheaderdata.Shipping_Address = grodatabyindent.First().Shipping_Address;
                grodispheaderdata.Shipping_City = grodatabyindent.First().Shipping_City;
                grodispheaderdata.Shipping_PINCODE = grodatabyindent.First().Shipping_PINCODE;
                grodispheaderdata.Courier_Partner = 0;
                //grodispheaderdata.AWB = "AWB";
                grodispheaderdata.DC_No = dcNo;
                grodispheaderdata.Inv_No = invNo;
                grodispheaderbyindent = await _groservicee.PostGroDispHeader(grodispheaderdata);

                var tkdcupdate = new TK_DC_Inv_ContrlVM();
                tkdcupdate.TK_DC_Inv_ContrlId = firstrecord.TK_DC_Inv_ContrlId;
                tkdcupdate.TK_Inv_Last_No = nextInvNo;
                tkdcupdate.TK_DC_Last_No = nextDcNo;
                var updatelastrowno = await _groservicee.UpdateTkDcLastInvandDcNo(tkdcupdate);
                

            }

            foreach (var item in grodatabyindent)
            {
                var getdispatchdetailbyheaderandgroodataid = allgrodispdetails.Where(x => x.Gro_Disp_Header_ID == grodispheaderbyindent.Gro_Disp_HeaderId
                     && x.Gro_data_ID == item.Gro_DataId && x.Gro_Part_No == item.Gro_Part_No).FirstOrDefault();
                if(getdispatchdetailbyheaderandgroodataid!=null)
                {
                    item.Scannedqty = getdispatchdetailbyheaderandgroodataid.Qnty_Dispatched;
                }
                else
                {
                    item.Scannedqty = 0;
                }

                var part = allgropartno.FirstOrDefault(x => x.Gro_Part_ListId == item.Gro_Part_No);

                item.GroPartNo = part.Gro_Part_No ?? "";

                item.IndentDateStr = item.SentDate?.ToString("dd-MM-yyyy") ?? "";
                item.DispatchDateStr= grodispheaderbyindent.Dispatch_Date?.ToString("dd-MM-yyyy") ?? "";
                var gropratstock = allgrostocklist.FirstOrDefault(x => x.Gro_Part_List_ID == item.Gro_Part_No);
                if (gropratstock == null)
                {
                    item.QntyAval = 0;
                }
                else
                {
                    item.QntyAval = gropratstock.Qnty_on_Hand;
                }
                result.Add(item);

            }


            return Ok(new
            {
                grodispheaderbyindent,
                result
            });

        }
        [HttpGet]
        public async Task<IActionResult> GetGroPartList()
        {
            var allgropartno = await _groservicee.Getallgroparts();
            foreach( var item in allgropartno)
            {
                if(item.OurPartDescription==null)
                {
                    item.OurPartDescription = "";
                }
            }
             return Ok(allgropartno);

        }
        [HttpPost]
        public async Task<IActionResult> SaveGroPart(Gro_Part_ListVM gropratvm)
        {
            //postgropartno
            var allgropartno = await _groservicee.PostGroPart(gropratvm);
            var allgrostock = await _groservicee.Getallgrostocklist();
            var grostockpart = allgrostock.Where(x => x.Gro_Part_List_ID == allgropartno.Gro_Part_ListId).FirstOrDefault();
            if (grostockpart != null)
            { }
            else
            {
                var newgrostockpart = new Gro_Stock_ListVM();
                newgrostockpart.Gro_Stock_ListId = 0;
                newgrostockpart.Gro_Part_List_ID = allgropartno.Gro_Part_ListId;
                newgrostockpart.Last_Sl_No = 0;
                newgrostockpart.Qnty_on_Hand = 0;
                newgrostockpart.Correction_User = 1;
                var postgrostock = await _groservicee.PostGroStockpart(newgrostockpart);
            }
            return Ok();

        }
        [HttpGet]
        public async Task<IActionResult> GetCourierList()
        {
            var allgropartno = await _groservicee.GetallcourierList();
            return Ok(allgropartno);

        }
        [HttpPost]
        public async Task<IActionResult> SaveCourier(Courier_ListVM gropratvm)
        {
            //postgropartno
            var allgropartno = await _groservicee.PostCourier(gropratvm);
            return Ok();

        }

        [HttpGet]
        public async Task<IActionResult> GetInvDcControl()
        {
            var allgropartno = await _groservicee.Getalltkdcinvctrl();
            var firstrecord = allgropartno.FirstOrDefault();
            return Ok(firstrecord);

        }
        [HttpPost]
        public async Task<IActionResult> SaveInvDcControl(TK_DC_Inv_ContrlVM gropratvm)
        {
            //postgropartno
            var allgropartno = await _groservicee.Posttkdcctrl(gropratvm);
            return Ok();

        }
        [HttpGet]
        public async Task<IActionResult> GetDispatchAddress(long headerId,string indentId)
        {
            
            var allgrodata = await _groservicee.GetallgroData();
            var allgrodispheader = await _groservicee.GetallgroDispHeader();
            var header = allgrodispheader.Where(x => x.Gro_Disp_HeaderId == headerId).FirstOrDefault();
            var result = allgrodata .Where(x => x.Indent == indentId).FirstOrDefault();

            if (result != null)
            {
                result.IndentDateStr = result.SentDate?.ToString("dd-MM-yyyy") ?? "";
            }
            return Ok(new
            {
                header,
                result
            });

        }
        [HttpPost]
        public async Task<IActionResult> UpdateDispatchAddress(Gro_Disp_HeaderVM gropratvm)
        {
             
            var allgropartno = await _groservicee.UpdateGroDispHeaderAddress(gropratvm);
            return Ok();

        }
        [HttpGet]
        public async Task<IActionResult> GetDispatchDetailsDataUpdate( )
        {

            var allgrodata = await _groservicee.GetallgroData();
            var allgrodispheader = await _groservicee.GetallgroDispHeader();
            var filtereredheader = allgrodispheader.Where(x => x.Courier_Partner == 0 || x.Dispatch_Date == null).ToList();
            List<Gro_Disp_HeaderVM> result = new List<Gro_Disp_HeaderVM>();

            foreach(var item in filtereredheader)
            {
                item.IndentDateStr = item.SentDate?.ToString("dd-MM-yyyy") ?? "";
                var alldata = allgrodata.Where(x => x.Indent == item.Indent).FirstOrDefault();
                item.Company_Name = alldata.Company_Name;
                item.Excutive_Name = alldata.Excutive_Name;
                result.Add(item);
            }
                      
            return Ok( result );

        }
        [HttpPost]
        public async Task<IActionResult> GetDispatchDetails(string indent, long groDispHeaderId)
        {
            var allgrodispdetails = await _groservicee.GetallgroDispatchDetails();
            var allgropartno = await _groservicee.Getallgroparts();
            var allgrodata = await _groservicee.GetallgroData();
            var grodatabyindent = allgrodata.Where(x => x.Indent == indent  ).ToList();
            var allgrodispheader = await _groservicee.GetallgroDispHeader();
            var header = allgrodispheader.Where(x => x.Gro_Disp_HeaderId == groDispHeaderId && x.Dispatched=='N').FirstOrDefault();
            header.IndentDateStr = header.SentDate?.ToString("dd-MM-yyyy") ?? "";
            header.Company_Name = grodatabyindent.First().Company_Name;
            header.Excutive_Name = grodatabyindent.First().Excutive_Name;
            header.Contact_Person = grodatabyindent.First().Contact_Person;
            header.Contact_Person_No = grodatabyindent.First().Contact_Person_No;
            List<Gro_DataVM> result = new List<Gro_DataVM>();
            foreach(var item in grodatabyindent)
            {
                var part = allgropartno.FirstOrDefault(x => x.Gro_Part_ListId == item.Gro_Part_No);
                var currentDispatch = allgrodispdetails.FirstOrDefault(x =>
     x.Gro_Disp_Header_ID == groDispHeaderId &&
     x.Gro_data_ID == item.Gro_DataId);

                item.QntyDispatched = currentDispatch?.Qnty_Dispatched ?? 0;
                var dispatchedTillDate = allgrodispdetails
    .Where(x => x.Gro_data_ID == item.Gro_DataId
             && x.Gro_Disp_Header_ID != groDispHeaderId
            
             )
    .Sum(x => x.Qnty_Dispatched);

                item.DispatchedTillDate = dispatchedTillDate;

                item.GroPartNo = part.Gro_Part_No ?? "";
                result.Add(item);
            }
            // var data = _dispatchService.GetDispatchDetails(indent, groDispHeaderId);

            return Ok(new
            {
                header,
                result
            });
        }
        [HttpGet]
        public async Task<IActionResult>  GetCouriers()
        {

            var allgrodata = await _groservicee.GetallcourierList();
             

            return Ok(allgrodata);

        }
        [HttpPost]
        public async Task<IActionResult> CheckAwbUnique( Gro_Disp_HeaderVM model)
        {
            var getalldispatchheaders = await _groservicee.GetallgroDispHeader();

            bool exists = getalldispatchheaders.Any(x =>
        x.AWB == model.AWB &&
        x.Gro_Disp_HeaderId != model.Gro_Disp_HeaderId);

            if (exists)
            {
                return Json(new UploadValidationResult
                {
                    Success = false,
                    Message = "AWB / Reference No already exists."
                });
            }

            return Json(new UploadValidationResult
            {
                Success = true,
                Message = ""
            });


        }
        [HttpPost]
        public async Task<IActionResult> UpdateDispatchDetailByheader(Gro_Disp_HeaderVM model)
        {
            //update dispatch header courier,awb and dispatch date 
            var getalldispatchheaders = await _groservicee.UpdateGroDispHeaderAWB((model));
            var alldispheaders = await _groservicee.GetallgroDispHeader();
            var dispheader = alldispheaders.Where(x => x.Gro_Disp_HeaderId == model.Gro_Disp_HeaderId).FirstOrDefault();
            var post = new Gro_Indent_DispHeadVM();
            post.Gro_Indent_DispHead_ID = 0;
            post.Gro_Indent = dispheader.Indent;
            post.Gro_Disp_Header_ID = dispheader.Gro_Disp_HeaderId;
            var postedata = await _groservicee.PostGroIndentDispHeader(post);
            //post dispatch to gro_indent_dispheader
            var allDispatchDetails = await _groservicee.GetallgroDispatchDetails();

            var dispatchDetails = allDispatchDetails
                .Where(x => x.Gro_Disp_Header_ID == model.Gro_Disp_HeaderId)
                .ToList();
            var allIndentPartSlNo = await _groservicee.GetallGroIndentpartSlno();
            var allGroStockDet = await _groservicee.GetallGroStockDet();
            List<Gro_Stock_DetVM> updateList = new List<Gro_Stock_DetVM>();
            foreach (var dispatch in dispatchDetails)
            {
                var serialLinks = allIndentPartSlNo
    .Where(x => x.Gro_Disp_Det_ID == dispatch.Gro_Disp_DetId)
    .ToList();
                foreach (var sl in serialLinks)
                {
                    var stockDet = allGroStockDet
                        .FirstOrDefault(x => x.Gro_Stock_DetId == sl.Gro_Stock_Det_ID);

                    if (stockDet == null)
                        continue;

                    updateList.Add(new Gro_Stock_DetVM
                    {
                        Gro_Stock_DetId = stockDet.Gro_Stock_DetId,
                        Part_Sl_No = stockDet.Part_Sl_No,
                        Sl_No_Status_ID = 4      // Sent
                    });
                }
                if (updateList.Any())
                {
                    await _groservicee.Updategrostockdetto1stscan(updateList);
                }
            }
                return Ok();


        }
        [HttpGet]
        public async Task<IActionResult> GetPendingDeliveryDetails()
        {

            var allgrodata = await _groservicee.GetallgroData();
            var allgrodispheader = await _groservicee.GetallgroDispHeader();
            var allcourier = await _groservicee.GetallcourierList();
            var filtereredheader = allgrodispheader.Where(x => x.Courier_Partner!= 0 && x.Dispatch_Date != null &&x.Delivered_Date==null ).ToList();
            List<Gro_Disp_HeaderVM> result = new List<Gro_Disp_HeaderVM>();

            foreach (var item in filtereredheader)
            {
                item.DispatchDateStr= item.Dispatch_Date?.ToString("dd-MM-yyyy") ?? "";
                item.DeliveryAgeing =
            (DateTime.Today - item.Dispatch_Date.Value.Date).Days;
                var alldata = allgrodata.Where(x => x.Indent == item.Indent).FirstOrDefault();
                item.Company_Name = alldata.Company_Name;
                var courier = allcourier.Where(x => x.courier_List_ID == item.Courier_Partner).FirstOrDefault();
                item.Courier = courier.Courier_Name;
                result.Add(item);
            }

            return Ok(result);

        }
        [HttpPost]
        public async Task<IActionResult> GetDeliveryDetails(long groDispHeaderId)
        {
            var allgrodispdetails = await _groservicee.GetallgroDispatchDetails();
            var allgropartno = await _groservicee.Getallgroparts();
            var allgrodata = await _groservicee.GetallgroData();
            var allcourier = await _groservicee.GetallcourierList();
            var allgrodispheader = await _groservicee.GetallgroDispHeader();
            var header = allgrodispheader.Where(x => x.Gro_Disp_HeaderId == groDispHeaderId).FirstOrDefault();

            var comp = allcourier.Where(x => x.courier_List_ID == header.Courier_Partner).FirstOrDefault();
            var grodatabyindent = allgrodata.Where(x => x.Indent == header.Indent).ToList();
            header.IndentDateStr = header.SentDate?.ToString("dd-MM-yyyy") ?? "";
            header.Company_Name = grodatabyindent.First().Company_Name;
            header.Excutive_Name = grodatabyindent.First().Excutive_Name;
            header.Contact_Person = grodatabyindent.First().Contact_Person;
            header.Contact_Person_No = grodatabyindent.First().Contact_Person_No;
            header.DispatchDateStr=header.Dispatch_Date?.ToString("dd-MM-yyyy") ?? "";
            header.Courier = comp.Courier_Name;
            List<Gro_DataVM> result = new List<Gro_DataVM>();
            foreach (var item in grodatabyindent)
            {
                var part = allgropartno.FirstOrDefault(x => x.Gro_Part_ListId == item.Gro_Part_No);
                var currentDispatch = allgrodispdetails.FirstOrDefault(x =>
       x.Gro_Disp_Header_ID == groDispHeaderId &&
       x.Gro_data_ID == item.Gro_DataId);

                item.QntyDispatched = currentDispatch?.Qnty_Dispatched ?? 0;

                // Qty dispatched till date from OTHER dispatches
                var dispatchedTillDate = allgrodispdetails
                    .Where(x => x.Gro_data_ID == item.Gro_DataId
                             && x.Gro_Disp_Header_ID != groDispHeaderId)
                    .Sum(x => x.Qnty_Dispatched);

                item.DispatchedTillDate = dispatchedTillDate;
                item.GroPartNo = part.Gro_Part_No ?? "";
                result.Add(item);
            }
            // var data = _dispatchService.GetDispatchDetails(indent, groDispHeaderId);

            return Ok(new
            {
                header,
                result
            });

        }
        [HttpPost]
        public async Task<IActionResult> UpdateDeliveryDateDispHeader(Gro_Disp_HeaderVM model)
        {
            //update dispatch header deliverydate
            var getalldispatchheaders = await _groservicee.UpdateGroDispHeaderDeliveryDate((model));

            return Ok(new
            {
                success = true
            });
        }
        [HttpGet]
        public async Task<IActionResult> GetDeliveryCompleteData()
        {
            var allHeaders = await _groservicee.GetallgroDispHeader();
            var allDetails = await _groservicee.GetallgroDispatchDetails();
            var allGroData = await _groservicee.GetallgroData();
            var allParts = await _groservicee.Getallgroparts();

            List<Gro_DataVM> result = new List<Gro_DataVM>();

            // Only delivered headers
            var deliveredHeaders = allHeaders
                .Where(x => x.Delivered_Date != null)
                .ToList();

            foreach (var groData in allGroData)
            {
                // All delivered dispatch details for this Gro_Data
                var dispatchDetails = allDetails
                    .Where(x => x.Gro_data_ID == groData.Gro_DataId)
                    .Join(deliveredHeaders,
                          d => d.Gro_Disp_Header_ID,
                          h => h.Gro_Disp_HeaderId,
                          (d, h) => new { Detail = d, Header = h })
                    .ToList();

                if (!dispatchDetails.Any())
                    continue;

                var part = allParts.FirstOrDefault(x => x.Gro_Part_ListId == groData.Gro_Part_No);

                groData.GroPartNo = part?.Gro_Part_No ?? "";

                groData.IndentDateStr = groData.SentDate?.ToString("dd-MM-yyyy") ?? "";

                // Total dispatched across all delivered dispatches
                groData.QntyDispatched = dispatchDetails.Sum(x => x.Detail.Qnty_Dispatched);

                // Latest Dispatch Date
                groData.DispatchDateStr = dispatchDetails
                    .Max(x => x.Header.Dispatch_Date)?
                    .ToString("dd-MM-yyyy") ?? "";

                // Latest Delivered Date
                groData.DeliveredDateStr = dispatchDetails
                    .Max(x => x.Header.Delivered_Date)?
                    .ToString("dd-MM-yyyy") ?? "";

                result.Add(groData);
            }

            return Ok(result);
        }
        [HttpGet]
        public async Task<IActionResult> GetGroPartWithStock()
        {
           
            var allgroparts = await _groservicee.Getallgroparts();
            var allgrostocklist = await _groservicee.Getallgrostocklist();
            var filteredgrostocklist = allgrostocklist.Where(x => x.Qnty_on_Hand > 0).ToList();
            List<Gro_Stock_ListVM> result = new List<Gro_Stock_ListVM>();
            foreach(var item in filteredgrostocklist)
            {
                var part = allgroparts.Where(x => x.Gro_Part_ListId == item.Gro_Part_List_ID).FirstOrDefault();
                item.Gro_Part_No = part.Gro_Part_No;
                result.Add(item);
            }

            

            return Ok(result);

        }
        [HttpGet]
        public async Task<IActionResult> GetGroPartWithoutStock()
        {

            var allgroparts = await _groservicee.Getallgroparts();
            var allgrostocklist = await _groservicee.Getallgrostocklist();
            var filteredgrostocklist = allgrostocklist.Where(x => x.Qnty_on_Hand == 0).ToList();
            List<Gro_Stock_ListVM> result = new List<Gro_Stock_ListVM>();
            foreach (var item in filteredgrostocklist)
            {
                var part = allgroparts.Where(x => x.Gro_Part_ListId == item.Gro_Part_List_ID).FirstOrDefault();
                item.Gro_Part_No = part.Gro_Part_No;
                result.Add(item);
            }
             return Ok(result);

        }
        [HttpPost]
        public async Task<IActionResult> GenerateQrCode(GrostockVM model)
        {
            var allgroparts = await _groservicee.Getallgroparts();
            var allgrostocklist = await _groservicee.Getallgrostocklist();
            var allgrostockdetails = await _groservicee.GetallGroStockDet();

            var qrpartno = allgroparts.Where(x => x.Gro_Part_No == model.GroPartNo).FirstOrDefault();
            var gropartstockdetailsbyId = allgrostockdetails.Where(x => x.Gro_Part_List_ID == qrpartno.Gro_Part_ListId && x.Sl_No_Status_ID == 1).ToList();

            var partstock = allgrostocklist.Where(x => x.Gro_Part_List_ID == qrpartno.Gro_Part_ListId).FirstOrDefault();
            var lastSerialNo = partstock.Last_Sl_No;
            var qty = model.Qty;
            if (gropartstockdetailsbyId.Any())
            {
                List<QRLabelVM> labels = new List<QRLabelVM>();

                foreach (var item in gropartstockdetailsbyId)
                {
                    string qrText = item.Part_Sl_No;

                    QRCodeGenerator generator = new QRCodeGenerator();

                    QRCodeData qrData =  generator.CreateQrCode( qrText, QRCodeGenerator.ECCLevel.Q);

                    PngByteQRCode qrCode =  new PngByteQRCode(qrData);

                    byte[] bytes =    qrCode.GetGraphic(20);

                    string base64 =  Convert.ToBase64String(bytes);

                    labels.Add(new QRLabelVM
                    {
                        GroPartNo = model.GroPartNo,
                        SerialNo = qrText,
                        QRCode = base64
                    });
                }

                return Json(new
                {
                    success = true,
                    labels = labels
                });
            }
            else { 
                List<QRLabelVM> labels = new List<QRLabelVM>();
                    for (long i = 1; i <= model.Qty; i++)
                    {
                        long currentSerial = lastSerialNo + i;

                        string serialNo =
                            currentSerial.ToString("D7");

                        string qrText = $"{model.GroPartNo}-{serialNo}";

                        QRCodeGenerator generator = new QRCodeGenerator();

                        QRCodeData qrData = generator.CreateQrCode(qrText, QRCodeGenerator.ECCLevel.Q);

                        PngByteQRCode qrCode = new PngByteQRCode(qrData);

                        byte[] bytes = qrCode.GetGraphic(20);

                        string base64 = Convert.ToBase64String(bytes);

                        labels.Add(new QRLabelVM
                        {
                            GroPartNo = model.GroPartNo,
                            SerialNo = qrText,
                            QRCode = base64
                        });
                        var gro_stock_det = new Gro_Stock_DetVM();
                        gro_stock_det.Gro_Part_List_ID = qrpartno.Gro_Part_ListId;
                        gro_stock_det.Part_Sl_No = qrText;
                        gro_stock_det.Sl_No_Status_ID = 1;
                        var postgrostockdet = await _groservicee.PostGroStockDet(gro_stock_det);




                    }
                var updatelastserailno = new Gro_Stock_ListVM();
                updatelastserailno.Gro_Stock_ListId = partstock.Gro_Stock_ListId;
                updatelastserailno.Last_Sl_No = lastSerialNo + model.Qty;
                updatelastserailno.Gro_Part_List_ID = qrpartno.Gro_Part_ListId;
                var postgro = await _groservicee.UpdategrostocklastslnobyPartNo(updatelastserailno);
                //update api  for that grostockid in the gro_stock_list table by Id
                return Json(new
                {
                    success = true,
                    labels = labels
                });

            }
            
        }

        //stock page from line number 983 to
        //hard coding for ORcode status change from printed to 1st scan now after scanner arrives  it needs to changed  
        /// <summary>
        /// 
        /// </summary>
        /// <param name="groPartListId"></param>
        /// <returns></returns>
        //[HttpGet]
        //public async Task<IActionResult> ScanLabels(long groPartListId)
        //{
        //    try
        //    {
        //        var allgrostockdet = await _groservicee.GetallGroStockDet();

        //        var grostockdetbypartid = allgrostockdet.Where(x => x.Gro_Part_List_ID == groPartListId && x.Sl_No_Status_ID==1).ToList();
        //        List<Gro_Stock_DetVM> data = new List<Gro_Stock_DetVM>();
        //        foreach(var item in grostockdetbypartid)
        //        {
        //            var updatestatusto1stscan = new Gro_Stock_DetVM();
        //            updatestatusto1stscan.Gro_Stock_DetId = item.Gro_Stock_DetId;
        //            updatestatusto1stscan.Part_Sl_No = item.Part_Sl_No;
        //            updatestatusto1stscan.Sl_No_Status_ID = 2;
        //            data.Add(updatestatusto1stscan);
        //        }

        //        var postedslno1stscan = await _groservicee.Updategrostockdetto1stscan(data);




        //        return Json(new
        //        {
        //            success = true,
        //            scannedCount = postedslno1stscan.Count
        //        }) ;
        //    }
        //    catch (Exception ex)
        //    {
        //        return Json(new
        //        {
        //            success = false,
        //            message = ex.Message
        //        });
        //    }
        //}
        [HttpGet]
        public async Task<IActionResult> ScanLabels(long groPartListId, string qrCode)
        {
            try
            {
                var allgrostockdet = await _groservicee.GetallGroStockDet();

                var label = allgrostockdet.FirstOrDefault(x =>
                                x.Gro_Part_List_ID == groPartListId &&
                                x.Part_Sl_No == qrCode);

                if (label == null)
                {
                    return Json(new
                    {
                        success = false,
                        message = "QR Code not found."
                    });
                }

                if (label.Sl_No_Status_ID == 2)
                {
                    return Json(new
                    {
                        success = false,
                        message = "QR Code already scanned. Move to next."
                    });
                }

                if (label.Sl_No_Status_ID != 1)
                {
                    return Json(new
                    {
                        success = false,
                        message = "QR Code is not in Printed status."
                    });
                }

                var update = new List<Gro_Stock_DetVM>();

                update.Add(new Gro_Stock_DetVM
                {
                    Gro_Stock_DetId = label.Gro_Stock_DetId,
                    Part_Sl_No = label.Part_Sl_No,
                    Sl_No_Status_ID = 2
                });

                await _groservicee.Updategrostockdetto1stscan(update);

                var scannedCount = allgrostockdet.Count(x =>
                                    x.Gro_Part_List_ID == groPartListId &&
                                    x.Sl_No_Status_ID == 2) + 1;

                return Json(new
                {
                    success = true,
                    message = "Label scanned successfully.",
                    scannedCount = scannedCount
                });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    success = false,
                    message = ex.Message
                });
            }
        }
        [HttpGet]
        public async Task<IActionResult> UpdateStockFromScannedLabels(long groPartListId)
        {
            try
            {
                var allgrostockdet = await _groservicee.GetallGroStockDet();

                var grostockdetbypartid = allgrostockdet.Where(x => x.Gro_Part_List_ID == groPartListId && x.Sl_No_Status_ID == 2).ToList();

                List<Gro_Stock_DetVM> data = new List<Gro_Stock_DetVM>();
                foreach (var item in grostockdetbypartid)
                {
                    var updatestatusto1stscan = new Gro_Stock_DetVM();
                    updatestatusto1stscan.Gro_Stock_DetId = item.Gro_Stock_DetId;
                    updatestatusto1stscan.Part_Sl_No = item.Part_Sl_No;
                    updatestatusto1stscan.Sl_No_Status_ID = 6;
                    data.Add(updatestatusto1stscan);
                }

                var postedslno1stscan = await _groservicee.Updategrostockdetto1stscan(data);


                var getgropartbyid = await _groservicee.Getgrostockbypoartid(groPartListId);

                int existingqty = getgropartbyid.Qnty_on_Hand;

                var newgrostock = new Gro_Stock_ListVM();
                newgrostock.Gro_Stock_ListId = getgropartbyid.Gro_Stock_ListId;
                newgrostock.Qnty_on_Hand = existingqty + grostockdetbypartid.Count();


                var result = await _groservicee.PostGroStockpart(newgrostock);

                return Json(new
                {
                    success = true,
                    qtyAdded = grostockdetbypartid.Count,
                    updatedStock = result.Qnty_on_Hand
                });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    success = false,
                    message = ex.Message
                });
            }
        }
        [HttpGet]
        public async Task<IActionResult> GetPendingPrintedLabels(long groPartListId)
        {
            var stock = await _groservicee.GetallGroStockDet();

            var pending = stock
                .Where(x => x.Gro_Part_List_ID == groPartListId &&
                            x.Sl_No_Status_ID == 1)
                .OrderBy(x => x.Part_Sl_No)
                .ToList();

            return Json(new
            {
                success = true,
                qtyEntered = stock.Count(x => x.Gro_Part_List_ID == groPartListId),
                stickerCount = stock.Count(x => x.Gro_Part_List_ID == groPartListId &&
                                                x.Sl_No_Status_ID == 2),
                pendingCount = pending.Count,
                serialNos = pending.Select(x => x.Part_Sl_No).ToList()
            });
        }

        [HttpPost]
        public async Task<IActionResult> ReScanLabel(long groPartListId, string qrCode)
        {
            var stock = await _groservicee.GetallGroStockDet();

            var label = stock.FirstOrDefault(x => x.Gro_Part_List_ID == groPartListId &&
                                                x.Part_Sl_No == qrCode);

            if (label == null)
            {
                return Json(new
                {
                    success = false,
                    message = "QR Code not found."
                });
            }

            if (label.Sl_No_Status_ID == 2)
            {
                return Json(new
                {
                    success = false,
                    message = "Already scanned."
                });
            }

            if (label.Sl_No_Status_ID != 1)
            {
                return Json(new
                {
                    success = false,
                    message = "Label cannot be scanned."
                });
            }

            await _groservicee.Updategrostockdetto1stscan(new List<Gro_Stock_DetVM>
    {
        new Gro_Stock_DetVM
        {
            Gro_Stock_DetId=label.Gro_Stock_DetId,
            Part_Sl_No=label.Part_Sl_No,
            Sl_No_Status_ID=2
        }
    });

            return Json(new
            {
                success = true,
                message = "Label scanned successfully."
            });
        }
        [HttpPost]
        public async Task<IActionResult> DeletePendingLabels(long groPartListId)
        {
            var stock = await _groservicee.GetallGroStockDet();

            var pending = stock
                .Where(x => x.Gro_Part_List_ID == groPartListId &&
                            x.Sl_No_Status_ID == 1)
                .ToList();

            var update = new List<Gro_Stock_DetVM>();

            foreach (var item in pending)
            {
                update.Add(new Gro_Stock_DetVM
                {
                    Gro_Stock_DetId = item.Gro_Stock_DetId,
                    Part_Sl_No = item.Part_Sl_No,
                    Sl_No_Status_ID = 5   // Deleted
                });
            }

            await _groservicee.Updategrostockdetto1stscan(update);

            var latest = await _groservicee.GetallGroStockDet();

            var pendingCount = latest.Count(x =>
                x.Gro_Part_List_ID == groPartListId &&
                x.Sl_No_Status_ID == 1);

            return Json(new
            {
                success = true,
                pendingCount = pendingCount
            });
        }
        /// <summary>
        /// //stock page dynamic implementation
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        /// 



        //  from here dynamic implementation  of scanning at final pack 
        [HttpPost]
        public async Task<IActionResult> CreateDispatchDetail( Gro_Disp_DetVM model)
        {
            var allgrodispdet = await _groservicee.GetallgroDispatchDetails();
            var postGrodispdetail = allgrodispdet.Where(x => x.Gro_Disp_Header_ID == model.Gro_Disp_Header_ID && x.Gro_data_ID == model.Gro_data_ID
             && x.Gro_Part_No == model.Gro_Part_No).FirstOrDefault();
            if(postGrodispdetail==null)
            {
                var datadispatchdetail = new Gro_Disp_DetVM();
                datadispatchdetail.Gro_Disp_DetId = 0;
                datadispatchdetail.Gro_Disp_Header_ID = model.Gro_Disp_Header_ID;
                datadispatchdetail.Gro_data_ID = model.Gro_data_ID;
                datadispatchdetail.Gro_Part_No = model.Gro_Part_No;
                datadispatchdetail.int_Part_No = 0;
                datadispatchdetail.Qnty_Dispatched = 0;
                //postdatadispatch detail to db?
                postGrodispdetail = await _groservicee.PostGroDispatchDetail(datadispatchdetail);

            }
                       

            return Ok(postGrodispdetail);
        }
        [HttpPost]
        public async Task<IActionResult> ScanDispatchLabels( DispatchScanVM model)
        {
            var allgrodata = await _groservicee.GetallgroData();
            
            var allgrodispdet = await _groservicee.GetallgroDispatchDetails();
            var getdispatchdetbyid = allgrodispdet.Where(x => x.Gro_Disp_DetId == model.Gro_Disp_Det_Id).FirstOrDefault();
            var grodatabydispatch = allgrodata.Where(x => x.Gro_DataId == getdispatchdetbyid.Gro_data_ID).FirstOrDefault();
           // var requiredquanity = model.RequiredQty;
            var getstockbypartid = await _groservicee.GetGropartnoStock(getdispatchdetbyid.Gro_Part_No);
            //var assignedqty=0;
            //if(getstockbypartid.Qnty_on_Hand>= requiredquanity)
            //{
            //    assignedqty = requiredquanity;
            //}
            //else
            //{
            //    assignedqty = getstockbypartid.Qnty_on_Hand;
            //}
            var allgrostockdet = await _groservicee.GetallGroStockDet();
            var stock = allgrostockdet .FirstOrDefault(x => x.Part_Sl_No == model.QRCode);
            if (stock == null)
            {
                return Ok(new
                {

                    success = false,

                    message = "QR Code not found."

                });
            }
            if (stock.Gro_Part_List_ID != getdispatchdetbyid.Gro_Part_No)
            {
                return Ok(new
                {

                    success = false,

                    message = "Wrong Part."

                });
            }
            if (stock.Sl_No_Status_ID == 3)
            {
                return Ok(new
                {

                    success = false,

                    message = "Label already scanned."

                });
            }
            if (stock.Sl_No_Status_ID == 1)
            {
                return Ok(new
                {
                    success = false,

                    showPrintedPopup = true,

                    partNo = getdispatchdetbyid.Gro_Part_No,

                    serialNo = stock.Part_Sl_No,

                    groStockDetId = stock.Gro_Stock_DetId
                });
            }
            if (stock.Sl_No_Status_ID != 6)
            {
                return Ok(new
                {

                    success = false,

                    message = "Invalid Status."

                });
            }
            var update = new Gro_Stock_DetVM();

            update.Gro_Stock_DetId = stock.Gro_Stock_DetId;

            update.Part_Sl_No = stock.Part_Sl_No;

            update.Sl_No_Status_ID = 3;

            await _groservicee.Updategrostockdetto1stscan(
                new List<Gro_Stock_DetVM> { update });
            var indent = new Indent_Part_Sl_NoVM();

            indent.Gro_Disp_Det_ID = model.Gro_Disp_Det_Id;

            indent.Gro_Stock_Det_ID = stock.Gro_Stock_DetId;

            await _groservicee.PostMultipleIndentSlno(new List<Indent_Part_Sl_NoVM> { indent });
            var allindentpartslno = await _groservicee.GetallGroIndentpartSlno();
            var scanned =allindentpartslno.Count(x => x.Gro_Disp_Det_ID ==model.Gro_Disp_Det_Id);
            var balance =grodatabydispatch.Reqd_Quantity - scanned;


            //var grostockdetbypartid = allgrostockdet.Where(x => x.Gro_Part_List_ID == getdispatchdetbyid.Gro_Part_No && x.Sl_No_Status_ID == 6).ToList();
            //var selectedStock = grostockdetbypartid.Take(assignedqty).ToList();
            //List<Gro_Stock_DetVM> data = new List<Gro_Stock_DetVM>();
            //List<Indent_Part_Sl_NoVM> Indentslno = new List<Indent_Part_Sl_NoVM>();
            //foreach (var item in selectedStock)
            //{
            //    var updatestatusto1stscan = new Gro_Stock_DetVM();
            //    updatestatusto1stscan.Gro_Stock_DetId = item.Gro_Stock_DetId;
            //    updatestatusto1stscan.Part_Sl_No = item.Part_Sl_No;
            //    updatestatusto1stscan.Sl_No_Status_ID = 3;
            //    data.Add(updatestatusto1stscan);


            //    var indentdata = new Indent_Part_Sl_NoVM();
            //    indentdata.Gro_Disp_Det_ID = model.Gro_Disp_Det_Id;
            //    indentdata.Gro_Stock_Det_ID = item.Gro_Stock_DetId;
            //    Indentslno.Add(indentdata);

            //}

            //var postedslno1stscan = await _groservicee.Updategrostockdetto1stscan(data);

            //var postindentslno = await _groservicee.PostMultipleIndentSlno(Indentslno);







            return Ok(new
            {

                success = true,

                scannedQty = scanned,

                balanceQty = balance,

                message =
     balance == 0
     ? "Balance to Dispatch Quantity Scanned - Save & Exit"
     : "Continue Scanning"

            });
        }

        [HttpGet]
        public async Task<IActionResult> GetDispatchPendingSession(long groDispDetId)
        {
            var allIndent = await _groservicee.GetallGroIndentpartSlno();

            var allStock = await _groservicee.GetallGroStockDet();

            var scanned = allIndent
                .Where(x => x.Gro_Disp_Det_ID == groDispDetId)
                .ToList();

            var serials = scanned
                .Join(allStock,
                      i => i.Gro_Stock_Det_ID,
                      s => s.Gro_Stock_DetId,
                      (i, s) => s.Part_Sl_No)
                .ToList();

            return Json(new
            {
                success = true,

                scannedQty = serials.Count,

                serialNos = serials
            });
        }

        [HttpPost]
        public async Task<IActionResult> CancelDispatchScan(long groDispDetId)
        {
            var allIndent = await _groservicee.GetallGroIndentpartSlno();

            var allStock = await _groservicee.GetallGroStockDet();

            var session = allIndent
                .Where(x => x.Gro_Disp_Det_ID == groDispDetId)
                .ToList();

            var updates = new List<Gro_Stock_DetVM>();

            foreach (var item in session)
            {
                var stock = allStock
                    .First(x => x.Gro_Stock_DetId == item.Gro_Stock_Det_ID);

                updates.Add(new Gro_Stock_DetVM
                {
                    Gro_Stock_DetId = stock.Gro_Stock_DetId,

                    Part_Sl_No = stock.Part_Sl_No,

                    Sl_No_Status_ID = 6 // InStock
                });
            }

            await _groservicee.Updategrostockdetto1stscan(updates);
            //delete indentpartslno pendin api 
            await _groservicee.DeleteIndentpartSlno(groDispDetId);

            return Json(new
            {
                success = true
            });
        }
        [HttpPost]
        public async Task<IActionResult> ForceAssignPrintedLabel(ForceAssignVM model)
        {
            var allgrodata = await _groservicee.GetallgroData();

            var allgrodispdet = await _groservicee.GetallgroDispatchDetails();
            var allStock = await _groservicee.GetallGroStockDet();
            var getdispatchdetbyid = allgrodispdet.Where(x => x.Gro_Disp_DetId == model.groDispDetId).FirstOrDefault();
            var grodatabydispatch = allgrodata.Where(x => x.Gro_DataId == getdispatchdetbyid.Gro_data_ID).FirstOrDefault();
            var stock = allStock
                .First(x => x.Gro_Stock_DetId == model.groStockDetId);
            var update = new Gro_Stock_DetVM();

            update.Gro_Stock_DetId = stock.Gro_Stock_DetId;

            update.Part_Sl_No = stock.Part_Sl_No;

            update.Sl_No_Status_ID = 6;
            var stockList =
await _groservicee.GetGropartnoStock(getdispatchdetbyid.Gro_Part_No);

            stockList.Qnty_on_Hand += 1;
            int updatedStock = stockList.Qnty_on_Hand;
            await _groservicee.UpdategrostockbyPart(stockList);
            await _groservicee.Updategrostockdetto1stscan(
                new List<Gro_Stock_DetVM> { update });
            update.Sl_No_Status_ID = 3;

            await _groservicee.Updategrostockdetto1stscan(
                new List<Gro_Stock_DetVM> { update });
            var indent = new Indent_Part_Sl_NoVM();

            indent.Gro_Disp_Det_ID = model.groDispDetId;

            indent.Gro_Stock_Det_ID = stock.Gro_Stock_DetId;

            await _groservicee.PostMultipleIndentSlno(
                new List<Indent_Part_Sl_NoVM> { indent });

            var allIndent =
await _groservicee.GetallGroIndentpartSlno();

            var scanned =
            allIndent.Count(x =>
            x.Gro_Disp_Det_ID == model.groDispDetId);
            var balance = grodatabydispatch.Reqd_Quantity - scanned;
            return Ok(new
            {
                success = true,

                scannedQty = scanned,

                balanceQty = balance,
                qtyOnHand = updatedStock,
                message = balance == 0
        ? "Balance to Dispatch Quantity Scanned - Save & Exit"
        : "Continue Scanning"
            });
        }
        [HttpPost]
        public async Task<IActionResult> SaveDispatchQty(DispatchScanVM model)
        {
            
            var allgrodispdet = await _groservicee.GetallgroDispatchDetails();
            var allindentslno = await _groservicee.GetallGroIndentpartSlno();
            var allgrodata = await _groservicee.GetallgroData();
            var getdispatchdetbyid = allgrodispdet.Where(x => x.Gro_Disp_DetId == model.Gro_Disp_Det_Id).FirstOrDefault();
            var indtslnobydipatchdetid = allindentslno.Where(x => x.Gro_Disp_Det_ID == model.Gro_Disp_Det_Id).ToList();
            var gropartrequired = allgrodata.Where(X => X.Gro_DataId == getdispatchdetbyid.Gro_data_ID).FirstOrDefault();


            var currentdispatchquantity = getdispatchdetbyid.Qnty_Dispatched;
            var Newquanity =  indtslnobydipatchdetid.Count;

            var updatedispatchqnty = new Gro_Disp_DetVM();
            updatedispatchqnty.Gro_Disp_DetId = model.Gro_Disp_Det_Id;
            updatedispatchqnty.Qnty_Dispatched = Newquanity;
            var updateqty = await _groservicee.UpdategroDispatchdetailqty(updatedispatchqnty);
            //update stock for gro_Disp_det

            var getstockbypart = await _groservicee.GetGropartnoStock(getdispatchdetbyid.Gro_Part_No);
            var curretnstock = getstockbypart.Qnty_on_Hand;
            var newstock = curretnstock - Newquanity;
            var stockupdate = new Gro_Stock_ListVM();
            stockupdate.Gro_Stock_ListId = getstockbypart.Gro_Stock_ListId;
            stockupdate.Gro_Part_List_ID = getstockbypart.Gro_Part_List_ID;
            stockupdate.Qnty_on_Hand = newstock;
            var poststockupdate = await _groservicee.UpdategrostockbyPart(stockupdate);
            //update stock api

            var bal_to_dispatch = gropartrequired.Bal_to_Disp;
            var Newbaltodispatch = bal_to_dispatch - Newquanity;

            var updategrodata = new Gro_DataVM();
            updategrodata.Gro_DataId = gropartrequired.Gro_DataId;
            updategrodata.Bal_to_Disp = Newbaltodispatch;
            var postgrodata = await _groservicee.UpdateGroDatabaltoDispatch(updategrodata);
            //update gro_data bal to dispatchs




            //yet to update  stock and gro part list 
            //
            return Ok();
        }
        ///below is to me implemented in save and exit 
        //hard coding for ORcode status change from printed to 1st scan now after scanner arrives  it needs to changed  
        [HttpGet]
        public async Task<IActionResult> GetDCPrintData(long headerId)
        {
            var allGroDispHeaders = await _groservicee.GetallgroDispHeader();
            var allGroDispDetails = await _groservicee.GetallgroDispatchDetails();
            var allGroParts = await _groservicee.Getallgroparts();
            var allgrodata = await _groservicee.GetallgroData();
            // Header
          
            var header = allGroDispHeaders
                            .FirstOrDefault(x => x.Gro_Disp_HeaderId == headerId);
            var grofirtrecord = allgrodata.Where(x => x.Indent == header.Indent).FirstOrDefault();
            if (header == null)
                return NotFound();
            
            header.IndentDateStr = header.SentDate?.ToString("dd-MM-yyyy") ?? "";
            header.DispatchDateStr = header.Dispatch_Date?.ToString("dd-MM-yyyy") ?? "";
            header.Company_Name = grofirtrecord.Company_Name;
            // Items
            List<Gro_DataVM> items = new List<Gro_DataVM>();

            var dispatchDetails = allGroDispDetails
                                    .Where(x => x.Gro_Disp_Header_ID == headerId &&x.Qnty_Dispatched>0)
                                    .ToList();

            foreach (var detail in dispatchDetails)
            {
                var part = allGroParts
                            .FirstOrDefault(x => x.Gro_Part_ListId == detail.Gro_Part_No);

                var item = new Gro_DataVM();

                item.Gro_Part_No = detail.Gro_Part_No;
                item.GroPartNo = (part?.Gro_Part_No ?? "") + " " + (part?.OurPartDescription ?? "");
                item.QntyDispatched = detail.Qnty_Dispatched;

                items.Add(item);
            }

            decimal invoiceValue = 0;

            foreach (var detail in dispatchDetails)
            {
                var part = allGroParts
                            .FirstOrDefault(x => x.Gro_Part_ListId == detail.Gro_Part_No);

                var item = new Gro_DataVM();

                item.Gro_Part_No = detail.Gro_Part_No;
                item.GroPartNo = (part?.Gro_Part_No ?? "") + " " + (part?.OurPartDescription ?? "");
                item.QntyDispatched = detail.Qnty_Dispatched;

                

                // Calculate invoice value
                if (part != null)
                {
                    decimal taxable = detail.Qnty_Dispatched * part.OurPrice;
                    decimal gst = taxable * part.GSTRate / 100m;

                    invoiceValue += taxable + gst;
                }
            }

            decimal approxValue = Math.Round(invoiceValue * 0.20m, 2);

            return Ok(new
            {
                header,
                items,
                approxValue
            });
        }
        [HttpGet]
        public async Task<IActionResult> GetInvoicePrintData(long headerId)
        {
            var allgrodata = await _groservicee.GetallgroData();
            var allgrodispheader = await _groservicee.GetallgroDispHeader();
            var alldispatchdetails = await _groservicee.GetallgroDispatchDetails();
            var allgroparts = await _groservicee.Getallgroparts();
            var groheaderdetails = allgrodispheader.Where(x => x.Gro_Disp_HeaderId == headerId).FirstOrDefault();
            var grodatafirstrecord = allgrodata.Where(x => x.Indent == groheaderdetails.Indent).FirstOrDefault();
            groheaderdetails.Excutive_Name = grodatafirstrecord.Excutive_Name;
            groheaderdetails.Company_Name = grodatafirstrecord.Company_Name;
            groheaderdetails.Contact_Person = grodatafirstrecord.Contact_Person;
            groheaderdetails.Contact_Person_No = grodatafirstrecord.Contact_Person_No;
            InvoicePrintVM vm = new InvoicePrintVM();

            vm.Header = groheaderdetails;
            var dispatchbyheader = alldispatchdetails.Where(x => x.Gro_Disp_Header_ID == headerId &&x.Qnty_Dispatched>0).ToList();
            int slno = 1;

            foreach (var item in dispatchbyheader)
            {
                var part = allgroparts
                    .FirstOrDefault(x => x.Gro_Part_ListId == item.Gro_Part_No);

                if (part == null)
                    continue;

                InvoiceDetailVM line = new InvoiceDetailVM();

                line.SlNo = slno++;

                line.PartId = part.Gro_Part_ListId;

                line.PartNo = part.Gro_Part_No;

               // line.Description = part.GroPartDesc;

                line.HSNCode = part.HSNCode;

                line.Unit = "Nos";

                line.Qty = item.Qnty_Dispatched;

                line.Rate = part.OurPrice;

               line.GSTRate = part.GSTRate;

                line.TaxableAmount = line.Qty * line.Rate;

                line.GSTAmount = line.TaxableAmount * line.GSTRate / 100;

                line.TotalAmount = line.TaxableAmount + line.GSTAmount;

                vm.Details.Add(line);
            }

            vm.TaxableAmount = vm.Details.Sum(x => x.TaxableAmount);

            vm.GSTAmount = vm.Details.Sum(x => x.GSTAmount);

            vm.GrandTotal = vm.Details.Sum(x => x.TotalAmount);
            vm.AmountInWords = ConvertAmountToWords(vm.GrandTotal);
            vm.TaxAmountInWords = ConvertAmountToWords(vm.GSTAmount);


            return Ok(vm);
        }
        private string ConvertAmountToWords(decimal amount)
        {
            long rupees = (long)Math.Floor(amount);

            int paise = (int)Math.Round((amount - rupees) * 100);

            string words = "Indian Rupees " + ConvertNumber(rupees);

            if (paise > 0)
            {
                words += " and " + ConvertNumber(paise) + " Paise";
            }

            words += " Only";

            return words;
        }

        private string ConvertNumber(long number)
        {
            string[] ones =
            {
        "", "One", "Two", "Three", "Four", "Five", "Six",
        "Seven", "Eight", "Nine", "Ten", "Eleven", "Twelve",
        "Thirteen", "Fourteen", "Fifteen", "Sixteen",
        "Seventeen", "Eighteen", "Nineteen"
    };

            string[] tens =
            {
        "", "", "Twenty", "Thirty", "Forty", "Fifty",
        "Sixty", "Seventy", "Eighty", "Ninety"
    };

            if (number == 0)
                return "Zero";

            if (number < 20)
                return ones[number];

            if (number < 100)
                return tens[number / 10] +
                       ((number % 10 > 0) ? " " + ConvertNumber(number % 10) : "");

            if (number < 1000)
                return ConvertNumber(number / 100) + " Hundred" +
                       ((number % 100 > 0) ? " " + ConvertNumber(number % 100) : "");

            if (number < 100000)
                return ConvertNumber(number / 1000) + " Thousand" +
                       ((number % 1000 > 0) ? " " + ConvertNumber(number % 1000) : "");

            if (number < 10000000)
                return ConvertNumber(number / 100000) + " Lakh" +
                       ((number % 100000 > 0) ? " " + ConvertNumber(number % 100000) : "");

            return ConvertNumber(number / 10000000) + " Crore" +
                   ((number % 10000000 > 0) ? " " + ConvertNumber(number % 10000000) : "");
        }

        [HttpGet]
        public async Task<IActionResult> GetDispatchSummary(string indent)
        {
            var allHeaders = await _groservicee.GetallgroDispHeader();
            var allDetails = await _groservicee.GetallgroDispatchDetails();
            var allGroData = await _groservicee.GetallgroData();
            var allParts = await _groservicee.Getallgroparts();

            var deliveredHeaderIds = allHeaders
                .Where(x => x.Delivered_Date != null)
                .Select(x => x.Gro_Disp_HeaderId)
                .ToList();

            var groDataByIndent = allGroData
                .Where(x => x.Indent == indent)
                .ToList();

            List<Gro_DataVM> result = new List<Gro_DataVM>();

            foreach (var item in groDataByIndent)
            {
                var part = allParts.FirstOrDefault(x => x.Gro_Part_ListId == item.Gro_Part_No);

                item.GroPartNo = part?.Gro_Part_No ?? "";

                var dispatches = allDetails
                    .Where(x => x.Gro_data_ID == item.Gro_DataId &&
                                deliveredHeaderIds.Contains(x.Gro_Disp_Header_ID))
                    .ToList();
                 
                item.QntyDispatched = dispatches.Sum(x => x.Qnty_Dispatched);
                item.TotalRecordedQnty = dispatches.Sum(x => x.Qnty_Dispatched);
                item.DeliveredDateStr = allHeaders
                    .Where(x => deliveredHeaderIds.Contains(x.Gro_Disp_HeaderId) &&
                                dispatches.Select(d => d.Gro_Disp_Header_ID)
                                          .Contains(x.Gro_Disp_HeaderId))
                    .Max(x => x.Delivered_Date)?
                    .ToString("dd-MM-yyyy") ?? "";

                item.Bal_to_Disp = item.Reqd_Quantity - item.QntyDispatched;
                item.BalToReceive= item.Reqd_Quantity - item.QntyDispatched;
                result.Add(item);
            }

            return Ok(result);
        }
        [HttpGet]
        public async Task<IActionResult> GetDispatchHistory(string indent)
        {
            var allHeaders = await _groservicee.GetallgroDispHeader();
            var allDetails = await _groservicee.GetallgroDispatchDetails();
            var allGroData = await _groservicee.GetallgroData();
            var allParts = await _groservicee.Getallgroparts();
            var allCourier = await _groservicee.GetallcourierList();

            List<GroDispatchHistoryVM> result = new List<GroDispatchHistoryVM>();

            var headers = allHeaders
                .Where(x => x.Indent == indent && x.Delivered_Date != null)
                .OrderBy(x => x.Dispatch_Date)
                .ToList();
            var grofirstrecord = allGroData.Where(x => x.Indent == indent).FirstOrDefault();
            foreach (var header in headers)
            {
                var vm = new GroDispatchHistoryVM();

                var courier = allCourier
                    .FirstOrDefault(x => x.courier_List_ID == header.Courier_Partner);

                header.Courier = courier?.Courier_Name ?? "";
                header.DispatchDateStr = header.Dispatch_Date?.ToString("dd-MM-yyyy") ?? "";
                header.DeliveredDateStr = header.Delivered_Date?.ToString("dd-MM-yyyy") ?? "";
                header.Contact_Person = grofirstrecord.Contact_Person;
                header.Contact_Person_No = grofirstrecord.Contact_Person_No;
                vm.Header = header;

                var details = allDetails
                    .Where(x => x.Gro_Disp_Header_ID == header.Gro_Disp_HeaderId && x.Qnty_Dispatched>0)
                    .ToList();
                if(!details.Any())
                {
                    continue;
                }
                foreach (var det in details)
                {
                    var gro = allGroData.FirstOrDefault(x => x.Gro_DataId == det.Gro_data_ID);

                    if (gro != null)
                    {
                        var part = allParts.FirstOrDefault(x => x.Gro_Part_ListId == gro.Gro_Part_No);

                        det.PartNo = part?.Gro_Part_No ?? "";
                    }
                }

                vm.Details = details;

                result.Add(vm);
            }

            return Ok(result);
        }
        [HttpGet]
        public async Task<IActionResult> GetLineItemDispatchHistory(long groDataId)
        {
            var allHeaders = await _groservicee.GetallgroDispHeader();
            var allDetails = await _groservicee.GetallgroDispatchDetails();

            var result = (from det in allDetails
                          join head in allHeaders
                          on det.Gro_Disp_Header_ID equals head.Gro_Disp_HeaderId
                          where det.Gro_data_ID == groDataId
                                && head.Dispatch_Date != null
                          orderby head.Dispatch_Date
                          select new
                          {
                              DispatchDate = head.Dispatch_Date?.ToString("dd-MM-yyyy") ?? "",
                              DispatchQty = det.Qnty_Dispatched
                          }).ToList();

            return Ok(result);
        }


        [HttpGet]
        public async Task<IActionResult> GetInvoiceDeleteData()
        {
            var allHeaders = await _groservicee.GetallgroDispHeader();
            var allGroData = await _groservicee.GetallgroData();

            var result = allHeaders
                .Where(x => string.IsNullOrWhiteSpace(x.AWB))
                .ToList();

            foreach (var item in result)
            {
                var gro = allGroData.FirstOrDefault(x => x.Indent == item.Indent);

                if (gro != null)
                {
                    item.Company_Name = gro.Company_Name;
                }
                item.HeaderDatestr=item.HeaderCreationDate?.ToString("dd-MM-yyyy") ?? "";
                item.IndentDateStr = item.SentDate?.ToString("dd-MM-yyyy") ?? "";
            }

            return Ok(result);
        }
        [HttpGet]
        public async Task<IActionResult> GetDeleteDispatchHeaderDetails(long headerId)
        {
            var headers = await _groservicee.GetallgroDispHeader();
            var details = await _groservicee.GetallgroDispatchDetails();
            var groData = await _groservicee.GetallgroData();
            var parts = await _groservicee.Getallgroparts();

            var header = headers
                .FirstOrDefault(x => x.Gro_Disp_HeaderId == headerId);

            header.IndentDateStr = header.SentDate?.ToString("dd-MM-yyyy") ?? "";
            header.HeaderDatestr = header.HeaderCreationDate?.ToString("dd-MM-yyyy") ?? "";
            var indentData = groData
                .Where(x => x.Indent == header.Indent)
                .ToList();

            if (indentData.Any())
            {
                header.Company_Name = indentData.First().Company_Name;
                header.Excutive_Name = indentData.First().Excutive_Name;
                header.Contact_Person = indentData.First().Contact_Person;
                header.Contact_Person_No = indentData.First().Contact_Person_No;
            }

            List<DeleteInvoiceLineVM> result = new List<DeleteInvoiceLineVM>();

            var dispatchDetails = details
                .Where(x => x.Gro_Disp_Header_ID == headerId &&
                            x.Qnty_Dispatched > 0)
                .ToList();

            foreach (var det in dispatchDetails)
            {
                var item = groData
                    .FirstOrDefault(x => x.Gro_DataId == det.Gro_data_ID);

                if (item == null)
                    continue;

                var part = parts
                    .FirstOrDefault(x => x.Gro_Part_ListId == item.Gro_Part_No);

                DeleteInvoiceLineVM vm = new DeleteInvoiceLineVM();

                vm.GroPartNo = part?.Gro_Part_No ?? "";

                vm.QntyDispatched = det.Qnty_Dispatched;

                vm.OurPrice = part?.OurPrice ?? 0;

                vm.InvoiceValue = vm.OurPrice * vm.QntyDispatched;

                result.Add(vm);
            }

            return Ok(new
            {
                header,
                details = result
            });
        }
        [HttpPost]
        public async Task<IActionResult> DeleteInvoiceHeader(long headerId)
        {
            var deleteinvoiceheader = await _groservicee.DeleteInvoiceHeader(headerId);

            return Ok(deleteinvoiceheader);
        }
        [HttpGet]
        public async Task<IActionResult> GetDispatchAgeingSummary()
        {
            var groData = (await _groservicee.GetallgroData())
                            .Where(x => x.Bal_to_Disp > 0)
                            .ToList();

            int full0 = 0, full1 = 0, full2 = 0, full3 = 0, fullGt3 = 0;
            int partial0 = 0, partial1 = 0, partial2 = 0, partial3 = 0, partialGt3 = 0;

            foreach (var item in groData)
            {
                if (!item.SentDate.HasValue)
                    continue;

                int ageing = (DateTime.Today - item.SentDate.Value.Date).Days;

                bool isFullDispatch = item.Bal_to_Disp == item.Reqd_Quantity;
                bool isPartialDispatch = item.Bal_to_Disp < item.Reqd_Quantity;

                if (isFullDispatch)
                {
                    switch (ageing)
                    {
                        case 0:
                            full0++;
                            break;

                        case 1:
                            full1++;
                            break;

                        case 2:
                            full2++;
                            break;

                        case 3:
                            full3++;
                            break;

                        default:
                            fullGt3++;
                            break;
                    }
                }
                else if (isPartialDispatch)
                {
                    switch (ageing)
                    {
                        case 0:
                            partial0++;
                            break;

                        case 1:
                            partial1++;
                            break;

                        case 2:
                            partial2++;
                            break;

                        case 3:
                            partial3++;
                            break;

                        default:
                            partialGt3++;
                            break;
                    }
                }
            }

            return Json(new
            {
                full = new
                {
                    day0 = full0,
                    day1 = full1,
                    day2 = full2,
                    day3 = full3,
                    gt3 = fullGt3,
                    total = full0 + full1 + full2 + full3 + fullGt3
                },

                partial = new
                {
                    day0 = partial0,
                    day1 = partial1,
                    day2 = partial2,
                    day3 = partial3,
                    gt3 = partialGt3,
                    total = partial0 + partial1 + partial2 + partial3 + partialGt3
                }
            });
        }
        [HttpGet]
        public async Task<IActionResult> GetDeliveryAgeingSummary()
        {
            var headers = await _groservicee.GetallgroDispHeader();

            var pending = headers
                .Where(x =>
                    x.Dispatch_Date != null &&
                    x.Delivered_Date == null)
                .ToList();

            int day0to2 = 0;
            int day3to5 = 0;
            int day6to7 = 0;
            int day8to10 = 0;
            int daygt10 = 0;

            foreach (var item in pending)
            {
                int days = (DateTime.Today - item.Dispatch_Date.Value.Date).Days;

                if (days <= 2)
                    day0to2++;

                else if (days <= 5)
                    day3to5++;

                else if (days <= 7)
                    day6to7++;

                else if (days <= 10)
                    day8to10++;

                else
                    daygt10++;
            }

            return Ok(new
            {
                day0to2,
                day3to5,
                day6to7,
                day8to10,
                daygt10,
                total = pending.Count
            });
        }

        [HttpGet]
        public async Task<IActionResult> GetPendingCourierCount()
        {
            var headers = await _groservicee.GetallgroDispHeader();

            int count = headers.Count(x =>
                (x.Courier_Partner == 0) &&
                x.Delivered_Date == null);

            return Ok(count);
        }
        [HttpGet]
        public async Task<IActionResult> GetPendingInvoicesForUpload()
        {
            var allHeaders = await _groservicee.GetallgroDispHeader();

            // Select only invoices waiting for upload
            var pendingHeaders = allHeaders
                .Where(x => x.Dispatched == 'Y' &&
                            x.Inv_Uploaded == 'N')
                .ToList();

            List<InvoicePrintVM> invoices = new List<InvoicePrintVM>();

            foreach (var header in pendingHeaders)
            {
                var invoice = await BuildInvoicePrintData(header.Gro_Disp_HeaderId);

                invoices.Add(invoice);
            }

            return Ok(invoices);
        }
        private async Task<InvoicePrintVM> BuildInvoicePrintData(long headerId)
        {
            var allgrodata = await _groservicee.GetallgroData();
            var allgrodispheader = await _groservicee.GetallgroDispHeader();
            var alldispatchdetails = await _groservicee.GetallgroDispatchDetails();
            var allgroparts = await _groservicee.Getallgroparts();

            var groheaderdetails = allgrodispheader
                .FirstOrDefault(x => x.Gro_Disp_HeaderId == headerId);

            var grodatafirstrecord = allgrodata
                .FirstOrDefault(x => x.Indent == groheaderdetails.Indent);

            groheaderdetails.Excutive_Name = grodatafirstrecord.Excutive_Name;
            groheaderdetails.Company_Name = grodatafirstrecord.Company_Name;
            groheaderdetails.Contact_Person = grodatafirstrecord.Contact_Person;
            groheaderdetails.Contact_Person_No = grodatafirstrecord.Contact_Person_No;

            InvoicePrintVM vm = new InvoicePrintVM();

            vm.Header = groheaderdetails;

            var dispatchbyheader = alldispatchdetails
                .Where(x => x.Gro_Disp_Header_ID == headerId &&x.Qnty_Dispatched>0)
                .ToList();

            int slno = 1;

            foreach (var item in dispatchbyheader)
            {
                var part = allgroparts
                    .FirstOrDefault(x => x.Gro_Part_ListId == item.Gro_Part_No);

                if (part == null)
                    continue;

                InvoiceDetailVM line = new InvoiceDetailVM();

                line.SlNo = slno++;
                line.PartId = part.Gro_Part_ListId;
                line.PartNo = part.Gro_Part_No;
                // line.Description = part.GroPartDesc;
                line.HSNCode = part.HSNCode;
                line.Unit = "Nos";
                line.Qty = item.Qnty_Dispatched;
                line.Rate = part.OurPrice;
                line.GSTRate = part.GSTRate;

                line.TaxableAmount = line.Qty * line.Rate;
                line.GSTAmount = line.TaxableAmount * line.GSTRate / 100;
                line.TotalAmount = line.TaxableAmount + line.GSTAmount;

                vm.Details.Add(line);
            }

            vm.TaxableAmount = vm.Details.Sum(x => x.TaxableAmount);
            vm.GSTAmount = vm.Details.Sum(x => x.GSTAmount);
            vm.GrandTotal = vm.Details.Sum(x => x.TotalAmount);

            vm.AmountInWords = ConvertAmountToWords(vm.GrandTotal);
            vm.TaxAmountInWords = ConvertAmountToWords(vm.GSTAmount);

            return vm;
        }

        //stock Correction In Gro Document Page
        [HttpPost]
        public async Task<IActionResult> GetCorrectionLabel(string qrCode)
        {
            try
            {
                var allStock = await _groservicee.GetallGroStockDet();
                var allParts = await _groservicee.Getallgroparts();

                var stock = allStock
                    .FirstOrDefault(x => x.Part_Sl_No == qrCode);

                if (stock == null)
                {
                    return Ok(new
                    {
                        success = false,
                        message = "QR Code not found."
                    });
                }

                var part = allParts
                    .FirstOrDefault(x => x.Gro_Part_ListId == stock.Gro_Part_List_ID);

                if (part == null)
                {
                    return Ok(new
                    {
                        success = false,
                        message = "Part not found."
                    });
                }

                return Ok(new
                {
                    success = true,

                    stockDetId = stock.Gro_Stock_DetId,

                    partId = part.Gro_Part_ListId,

                    partNo = part.Gro_Part_No,

                    description = part.OurPartDescription,

                    serialNo = stock.Part_Sl_No,

                    statusId = stock.Sl_No_Status_ID
                });
            }
            catch (Exception ex)
            {
                return Ok(new
                {
                    success = false,
                    message = ex.Message
                });
            }
        }
        [HttpGet]
        public async Task<IActionResult> GetCorrectionPart(string partNo)
        {

            var stock =
                await _groservicee.GetGropartnoStock(Convert.ToInt64(partNo));
            var part = await _groservicee.Getallgroparts();
            var correctionpart = part.Where(x => x.Gro_Part_ListId == stock.Gro_Part_List_ID).FirstOrDefault();
            if (stock == null)
            {
                return Ok(new
                {
                    success = false
                });
            }

            return Ok(new
            {
                success = true,

                partId = stock.Gro_Part_List_ID,

                partNo = correctionpart.Gro_Part_No,

                description = correctionpart.OurPartDescription,

                qoh = stock.Qnty_on_Hand
            });
        }
    }
}
