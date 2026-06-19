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
        public IActionResult GroSetup()
        {
            _logger.LogTrace("Gro--GroSetup--Loading");
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
                var allgropartno = (await _groservicee.Getallgroparts()).ToList();
                var allgrodata = (await _groservicee.GetallgroData()).ToList();
                for (long rowNo = startingrowno; rowNo <= lastRow; rowNo++)
                {
                    IRow row = sheet.GetRow((int)rowNo);

                    if (row == null)
                    {
                        continue;
                    }

                    string companyName = row.GetCell(3)?.ToString()?.Trim();
                    var getcompabybyname = await _masterservice.GetCompanybyName(companyName);
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
                                CompanyName = companyName,
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

                        var postedgropartno =
                            await _groservicee.PostGroPart(partVm);
                        partId = postedgropartno.Gro_Part_ListId;
                        allgropartno.Add(postedgropartno);
                        // Add newly created part to collection
                    }
                    else
                    {
                        partId = part.Gro_Part_ListId;
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

             List <Gro_DataVM> result = new List<Gro_DataVM>();
            foreach (var item in allgrodata)
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
                var grodispheaderdata = new Gro_Disp_HeaderVM();
                grodispheaderdata.CWB_Customer = grodatabyindent.First().CWB_Customer;
                grodispheaderdata.Indent = grodatabyindent.First().Indent;
                grodispheaderdata.SentDate = grodatabyindent.First().SentDate;
                grodispheaderdata.Shipping_Address = grodatabyindent.First().Shipping_Address;
                grodispheaderdata.Shipping_City = grodatabyindent.First().Shipping_City;
                grodispheaderdata.Shipping_PINCODE = grodatabyindent.First().Shipping_PINCODE;
                grodispheaderdata.Courier_Partner = 0;
                grodispheaderdata.AWB = "AWB";
                grodispheaderdata.DC_No = "0000001";
                grodispheaderdata.Inv_No = "0000001";
                grodispheaderbyindent = await _groservicee.PostGroDispHeader(grodispheaderdata);

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
    }
}
