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
        [HttpPost]
        public async Task<IActionResult>UploadGroData(IFormFile uploadedFile)
        {
            var result =  await _groservicee.ValidateExcel(uploadedFile);

            if (!result.Success)
            {
                return Json(result);
            }
            IWorkbook workbook;

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
                var allgrostock = await _groservicee.Getallgrostocklist();
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
                    //--------------------------------------------------
                    // PART CHECK
                    //--------------------------------------------------

                    var part = allgropartno.FirstOrDefault(x => x.Gro_Part_No.Trim().ToUpper() == partNo.Trim().ToUpper());

                    long partId;

                    if (part == null)
                    {
                        var partVm =
                            new Gro_Part_ListVM
                            {
                                Gro_Part_No = partNo,
                                Part_Status = 1,
                                MRP = 0,
                                Part_No = 0,
                                Data_Update = DateTime.Now,
                                Update_By = 1

                            };

                        var postedgropartno =         await _groservicee.PostGroPart(partVm);
                        partId = postedgropartno.Gro_Part_ListId;
                        allgropartno.Add(postedgropartno);


                        var grostockpart = allgrostock.Where(x => x.Gro_Part_List_ID == partId).FirstOrDefault();
                        if(grostockpart!=null)
                        { }
                        else
                        {
                            var newgrostockpart = new Gro_Stock_ListVM();
                            newgrostockpart.Gro_Stock_ListId = 0;
                            newgrostockpart.Gro_Part_List_ID = partId;
                            newgrostockpart.Last_Sl_No = 0;
                            newgrostockpart.Qnty_on_Hand = 0;
                            newgrostockpart.Correction_User = 1;
                            var postgrostock = await _groservicee.PostGroStockpart(newgrostockpart);
                        }

                        //add





                        // Add newly created part to collection
                    }
                    else
                    {
                        partId = part.Gro_Part_ListId;
                        var grostockpart = allgrostock.Where(x => x.Gro_Part_List_ID == partId).FirstOrDefault();
                        if (grostockpart != null)
                        { }
                        else
                        {
                            var newgrostockpart = new Gro_Stock_ListVM();
                            newgrostockpart.Gro_Stock_ListId = 0;
                            newgrostockpart.Gro_Part_List_ID = partId;
                            newgrostockpart.Last_Sl_No = 0;
                            newgrostockpart.Qnty_on_Hand = 0;
                            newgrostockpart.Correction_User = 1;
                            var postgrostock = await _groservicee.PostGroStockpart(newgrostockpart);
                        }
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
            }

            return Json(new
            {
                Success = true,
                Message = "Upload Completed Successfully"
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

                var gropratstock = allgrostocklist.FirstOrDefault(x => x.Gro_Part_List_ID == item.Gro_Part_No);
                if(gropratstock==null)
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
            var allgropartno = await _groservicee.Getallgroparts();
            var allgrodata = await _groservicee.GetallgroData();
            var allgrostocklist = await _groservicee.Getallgrostocklist();
            List<Gro_DataVM> result = new List<Gro_DataVM>();
            var grodatabyindent = allgrodata.Where(x => x.Indent == indentNo).ToList();
            foreach(var item in grodatabyindent)
            {
                var part = allgropartno.FirstOrDefault(x => x.Gro_Part_ListId == item.Gro_Part_No);

                item.GroPartNo = part.Gro_Part_No ?? "";

                item.IndentDateStr = item.SentDate?.ToString("dd-MM-yyyy") ?? "";

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
            var allgrodispheader = await _groservicee.GetallgroDispHeader();
            var grodispheaderbyindent = allgrodispheader.Where(x => x.Indent == indentNo).FirstOrDefault();
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
                grodispheaderdata.AWB = "AWB";
                grodispheaderdata.DC_No = dcNo;
                grodispheaderdata.Inv_No = invNo;
                grodispheaderbyindent = await _groservicee.PostGroDispHeader(grodispheaderdata);

                var tkdcupdate = new TK_DC_Inv_ContrlVM();
                tkdcupdate.TK_DC_Inv_ContrlId = firstrecord.TK_DC_Inv_ContrlId;
                tkdcupdate.TK_Inv_Last_No = nextInvNo;
                tkdcupdate.TK_DC_Last_No = nextDcNo;
                var updatelastrowno = await _groservicee.UpdateTkDcLastInvandDcNo(tkdcupdate);
                

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
                result.Add(item);
            }
                      
            return Ok( result );

        }
        [HttpPost]
        public async Task<IActionResult> GetDispatchDetails(string indent, long groDispHeaderId)
        {
            var allgropartno = await _groservicee.Getallgroparts();
            var allgrodata = await _groservicee.GetallgroData();
            var grodatabyindent = allgrodata.Where(x => x.Indent == indent).ToList();
            var allgrodispheader = await _groservicee.GetallgroDispHeader();
            var header = allgrodispheader.Where(x => x.Gro_Disp_HeaderId == groDispHeaderId).FirstOrDefault();
            header.IndentDateStr = header.SentDate?.ToString("dd-MM-yyyy") ?? "";
            header.Company_Name = grodatabyindent.First().Company_Name;
            header.Excutive_Name = grodatabyindent.First().Excutive_Name;
            header.Contact_Person = grodatabyindent.First().Contact_Person;
            header.Contact_Person_No = grodatabyindent.First().Contact_Person_No;
            List<Gro_DataVM> result = new List<Gro_DataVM>();
            foreach(var item in grodatabyindent)
            {
                var part = allgropartno.FirstOrDefault(x => x.Gro_Part_ListId == item.Gro_Part_No);

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
            var allgropartno = await _groservicee.Getallgroparts();
            var allgrodispheader = await _groservicee.GetallgroDispHeader();
            var dispheaders = allgrodispheader.Where(x => x.Delivered_Date != null).ToList();
            var allgrodata = await _groservicee.GetallgroData();
            List<Gro_DataVM> grolist = new List<Gro_DataVM>();
            foreach (var item in dispheaders)
            {
                var grofilteredlist = allgrodata.Where(x => x.Indent == item.Indent).ToList();

                
                foreach(var data in grofilteredlist)
                {
                    var part = allgropartno.FirstOrDefault(x => x.Gro_Part_ListId == data.Gro_Part_No);

                    data.GroPartNo = part.Gro_Part_No ?? "";
                    data.IndentDateStr=data.SentDate?.ToString("dd-MM-yyyy") ?? "";
                    data.DispatchDateStr=item.Dispatch_Date?.ToString("dd-MM-yyyy") ?? "";
                    data.DeliveredDateStr=item.Delivered_Date?.ToString("dd-MM-yyyy") ?? "";
                    grolist.Add(data);
                }

            }

       

            return Ok(grolist);

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


        //hard coding for ORcode status change from printed to 1st scan now after scanner arrives  it needs to changed  
        /// <summary>
        /// 
        /// </summary>
        /// <param name="groPartListId"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> ScanLabels(long groPartListId)
        {
            try
            {
                var allgrostockdet = await _groservicee.GetallGroStockDet();

                var grostockdetbypartid = allgrostockdet.Where(x => x.Gro_Part_List_ID == groPartListId && x.Sl_No_Status_ID==1).ToList();
                List<Gro_Stock_DetVM> data = new List<Gro_Stock_DetVM>();
                foreach(var item in grostockdetbypartid)
                {
                    var updatestatusto1stscan = new Gro_Stock_DetVM();
                    updatestatusto1stscan.Gro_Stock_DetId = item.Gro_Stock_DetId;
                    updatestatusto1stscan.Part_Sl_No = item.Part_Sl_No;
                    updatestatusto1stscan.Sl_No_Status_ID = 2;
                    data.Add(updatestatusto1stscan);
                }

                var postedslno1stscan = await _groservicee.Updategrostockdetto1stscan(data);




                return Json(new
                {
                    success = true,
                    scannedCount = postedslno1stscan.Count
                }) ;
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
    }
}
