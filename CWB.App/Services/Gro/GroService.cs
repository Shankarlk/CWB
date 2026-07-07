using CWB.App.AppUtils;
using CWB.App.Models.Contacts;
using CWB.App.Models.ItemMaster;
using CWB.CommonUtils.Common;
using CWB.Logging;
using Microsoft.AspNetCore.Http;
using CWB.App.Models.Gro;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CWB.App.Models.Routing;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System.IO;

namespace CWB.App.Services.Gro
{
    public class GroService:IGroService
    {
        private readonly ILoggerManager _logger;
        private readonly ApiUrls _apiUrls;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly long tenantId;

        public GroService(ILoggerManager logger, ApiUrls apiUrlsOptions, IHttpContextAccessor httpContextAccessor)
        {
            _logger = logger;
            _apiUrls = apiUrlsOptions;
            _httpContextAccessor = httpContextAccessor;
            tenantId = long.Parse(AppUtil.GetTenantId(_httpContextAccessor.HttpContext.User));

        }
        public async Task<UploadValidationResult>  ValidateExcel(IFormFile file)
        {
            try
            {
                if (file == null || file.Length == 0)
                {
                    return new UploadValidationResult
                    {
                        Success = false,
                        Message = "Please Select Excel File"
                    };
                }

                IWorkbook workbook;

                using (var stream = file.OpenReadStream())
                {
                    string ext =
                        Path.GetExtension(file.FileName)
                            .ToLower();

                    if (ext == ".xlsx")
                    {
                        workbook = new XSSFWorkbook(stream);
                    }
                    else if (ext == ".xls")
                    {
                        workbook = new HSSFWorkbook(stream);
                    }
                    else
                    {
                        return new UploadValidationResult
                        {
                            Success = false,
                            Message = "Only .xls and .xlsx files allowed"
                        };
                    }

                    ISheet sheet =
                        workbook.GetSheetAt(0);

                    return ValidateSheet(sheet);
                }
            }
            catch (Exception ex)
            {
                return new UploadValidationResult
                {
                    Success = false,
                    Message = ex.Message
                };
            }
        }
        private UploadValidationResult ValidateSheet(ISheet sheet)
        {
            int lastRow = sheet.LastRowNum;

            // START FROM ROW 2
            for (int rowNo = 1; rowNo <= lastRow; rowNo++)
            {
                IRow row = sheet.GetRow(rowNo);

                if (row == null)
                {
                    continue;
                }

                // A to M
                for (int col = 0; col <= 12; col++)
                {
                    ICell cell = row.GetCell(col);

                    string value =
                        cell?.ToString()?.Trim();

                    if(col==6)
                    {
                        continue;
                    }
                    if (string.IsNullOrWhiteSpace(value))
                    {
                        return new UploadValidationResult
                        {
                            Success = false,

                            Message =
                            $"Empty Cell Found in Row {rowNo + 1}, Column {GetColumnName(col)}"
                        };
                    }
                }

                //-------------------------------------------------
                // Column A : Sent Date
                //-------------------------------------------------

                string sentDate =
                    row.GetCell(0)?.ToString()?.Trim();

                if (!DateTime.TryParse(sentDate, out _))
                {
                    return new UploadValidationResult
                    {
                        Success = false,

                        Message =
                        $"Date Format Error in Row {rowNo + 1}, Column A (Sent Date)"
                    };
                }

                //-------------------------------------------------
                // Column F : Quantity
                //-------------------------------------------------

                string qty =
                    row.GetCell(5)?.ToString()?.Trim();

                if (!decimal.TryParse(qty, out _))
                {
                    return new UploadValidationResult
                    {
                        Success = false,

                        Message =
                        $"Quantity Format Error in Row {rowNo + 1}, Column F"
                    };
                }
            }

            return new UploadValidationResult
            {
                Success = true,
                Message = "Excel Validation Successful"
            };
        }
        private string GetColumnName(int index)
        {
            return ((char)('A' + index)).ToString();
        }
        public async Task<IEnumerable<Gro_Part_ListVM>> Getallgroparts()
        {
            var uri = new Uri(_apiUrls.Gateway + $"/cwbgro/Getallgropartlist/{tenantId}");
            var headers = await AppUtil.GetAuthToken(_httpContextAccessor.HttpContext);
            return await RestHelper<List<Gro_Part_ListVM>>.GetAsync(uri, headers);
        }

        public async Task<Gro_Part_ListVM> PostGroPart(Gro_Part_ListVM companyVM)
        {
            var uri = new Uri(_apiUrls.Gateway + $"/cwbgro/postgropartlist");
            var headers = await AppUtil.GetAuthToken(_httpContextAccessor.HttpContext);
            companyVM.TenantId = tenantId;
            return await RestHelper<Gro_Part_ListVM>.PostAsync(uri, companyVM, headers);
        }

        public async Task<IEnumerable<Gro_DataVM>> GetallgroData()
        {
            var uri = new Uri(_apiUrls.Gateway + $"/cwbgro/Getallgrodata/{tenantId}");
            var headers = await AppUtil.GetAuthToken(_httpContextAccessor.HttpContext);
            return await RestHelper<List<Gro_DataVM>>.GetAsync(uri, headers);
        }
        public async Task<List<Gro_DataVM>> PostMultipleGrodata(IEnumerable<Gro_DataVM> productions)
        {
            var uri = new Uri(_apiUrls.Gateway + $"/cwbgro/postmultiplegrodata");
            var headers = await AppUtil.GetAuthToken(_httpContextAccessor.HttpContext);
            foreach (var item in productions)
            {
                item.TenantId = tenantId;
            }
            return await RestHelper<List<Gro_DataVM>>.PostAsync(uri, productions, headers);
        }

        public async Task<Cust_Specific_DataVM> PostCustSpecificData(Cust_Specific_DataVM companyVM)
        {
            var uri = new Uri(_apiUrls.Gateway + $"/cwbgro/postcustspecificdata");
            var headers = await AppUtil.GetAuthToken(_httpContextAccessor.HttpContext);
            companyVM.TenantId = tenantId;
            return await RestHelper<Cust_Specific_DataVM>.PostAsync(uri, companyVM, headers);
        }
        public async Task<IEnumerable<Cust_Specific_DataVM>> GetallCustSpecificData()
        {
            var uri = new Uri(_apiUrls.Gateway + $"/cwbgro/Getallcustspecificdata/{tenantId}");
            var headers = await AppUtil.GetAuthToken(_httpContextAccessor.HttpContext);
            return await RestHelper<List<Cust_Specific_DataVM>>.GetAsync(uri, headers);
        }
        public async Task <Gro_Stock_ListVM> GetGropartnoStock(long gropartid)
        {
            var uri = new Uri(_apiUrls.Gateway + $"/cwbgro/getstockbygropart/{gropartid}/{tenantId}");
            var headers = await AppUtil.GetAuthToken(_httpContextAccessor.HttpContext);
            return await RestHelper<Gro_Stock_ListVM>.GetAsync(uri, headers);
        }
        public async Task<IEnumerable<Gro_Stock_ListVM>> Getallgrostocklist()
        {
            var uri = new Uri(_apiUrls.Gateway + $"/cwbgro/Getallgrostocklist/{tenantId}");
            var headers = await AppUtil.GetAuthToken(_httpContextAccessor.HttpContext);
            return await RestHelper<List<Gro_Stock_ListVM>>.GetAsync(uri, headers);
        }
        public async Task<IEnumerable<Gro_Disp_HeaderVM>> GetallgroDispHeader()
        {
            var uri = new Uri(_apiUrls.Gateway + $"/cwbgro/Getallgrodispatchheader/{tenantId}");
            var headers = await AppUtil.GetAuthToken(_httpContextAccessor.HttpContext);
            return await RestHelper<List<Gro_Disp_HeaderVM>>.GetAsync(uri, headers);
        }
        public async Task<Gro_Disp_HeaderVM> PostGroDispHeader(Gro_Disp_HeaderVM grodispheadervm)
        {
            var uri = new Uri(_apiUrls.Gateway + $"/cwbgro/postgrodispatchheader");

            var headers = await AppUtil.GetAuthToken(_httpContextAccessor.HttpContext);
            grodispheadervm.TenantId = tenantId;
            return await RestHelper<Gro_Disp_HeaderVM>.PostAsync(uri, grodispheadervm, headers);
        }
        public async Task<Gro_Disp_HeaderVM> UpdateGroDispHeaderAddress(Gro_Disp_HeaderVM grodispheadervm)
        {
            var uri = new Uri(_apiUrls.Gateway + $"/cwbgro/updategrodispheaderaddress");

            var headers = await AppUtil.GetAuthToken(_httpContextAccessor.HttpContext);
            grodispheadervm.TenantId = tenantId;
            return await RestHelper<Gro_Disp_HeaderVM>.PostAsync(uri, grodispheadervm, headers);
        }
        public async Task<Gro_Disp_HeaderVM> UpdateGroDispHeaderAWB(Gro_Disp_HeaderVM grodispheadervm)
        {
            var uri = new Uri(_apiUrls.Gateway + $"/cwbgro/updategrodispheadercourierawbdispatchdate");

            var headers = await AppUtil.GetAuthToken(_httpContextAccessor.HttpContext);
            grodispheadervm.TenantId = tenantId;
            return await RestHelper<Gro_Disp_HeaderVM>.PostAsync(uri, grodispheadervm, headers);
        }
        public async Task<Gro_Disp_HeaderVM> UpdateGroDispHeaderDeliveryDate(Gro_Disp_HeaderVM grodispheadervm)
        {
            var uri = new Uri(_apiUrls.Gateway + $"/cwbgro/updatedispheaderdeliverydate");

            var headers = await AppUtil.GetAuthToken(_httpContextAccessor.HttpContext);
            grodispheadervm.TenantId = tenantId;
            return await RestHelper<Gro_Disp_HeaderVM>.PostAsync(uri, grodispheadervm, headers);
        }
        public async Task<IEnumerable<Courier_ListVM>> GetallcourierList()
        {
            var uri = new Uri(_apiUrls.Gateway + $"/cwbgro/Getallcourierlist/{tenantId}");
            var headers = await AppUtil.GetAuthToken(_httpContextAccessor.HttpContext);
            return await RestHelper<List<Courier_ListVM>>.GetAsync(uri, headers);
        }
        public async Task<Courier_ListVM> PostCourier(Courier_ListVM grodispheadervm)
        {
            var uri = new Uri(_apiUrls.Gateway + $"/cwbgro/postcourier");
            var headers = await AppUtil.GetAuthToken(_httpContextAccessor.HttpContext);
            grodispheadervm.TenantId = tenantId;
            return await RestHelper<Courier_ListVM>.PostAsync(uri, grodispheadervm, headers);
        }
        public async Task<IEnumerable<TK_DC_Inv_ContrlVM>> Getalltkdcinvctrl()
        {
            var uri = new Uri(_apiUrls.Gateway + $"/cwbgro/getalltkdcinctrl/{tenantId}");
            var headers = await AppUtil.GetAuthToken(_httpContextAccessor.HttpContext);
            return await RestHelper<List<TK_DC_Inv_ContrlVM>>.GetAsync(uri, headers);
        }

        public async Task<TK_DC_Inv_ContrlVM> Posttkdcctrl(TK_DC_Inv_ContrlVM grodispheadervm)
        {
            var uri = new Uri(_apiUrls.Gateway + $"/cwbgro/posttkdcinvctrl");
            var headers = await AppUtil.GetAuthToken(_httpContextAccessor.HttpContext);
            grodispheadervm.TenantId = tenantId;
            return await RestHelper<TK_DC_Inv_ContrlVM>.PostAsync(uri, grodispheadervm, headers);
        }
        public async Task<TK_DC_Inv_ContrlVM> UpdateTkDcLastInvandDcNo(TK_DC_Inv_ContrlVM grodispheadervm)
        {
            var uri = new Uri(_apiUrls.Gateway + $"/cwbgro/updatetkdclastdcandinvno");
            var headers = await AppUtil.GetAuthToken(_httpContextAccessor.HttpContext);
            grodispheadervm.TenantId = tenantId;
            return await RestHelper<TK_DC_Inv_ContrlVM>.PostAsync(uri, grodispheadervm, headers);
        }
        public async Task<Gro_Stock_DetVM> PostGroStockDet(Gro_Stock_DetVM grodispheadervm)
        {
            var uri = new Uri(_apiUrls.Gateway + $"/cwbgro/postgrostockdet");
            var headers = await AppUtil.GetAuthToken(_httpContextAccessor.HttpContext);
            grodispheadervm.TenantId = tenantId;
            return await RestHelper<Gro_Stock_DetVM>.PostAsync(uri, grodispheadervm, headers);
        }
        public async Task<Gro_Stock_ListVM> UpdategrostocklastslnobyPartNo(Gro_Stock_ListVM grodispheadervm)
        {
            var uri = new Uri(_apiUrls.Gateway + $"/cwbgro/updategrostocklastslnobypartno");
            var headers = await AppUtil.GetAuthToken(_httpContextAccessor.HttpContext);
            grodispheadervm.TenantId = tenantId;
            return await RestHelper<Gro_Stock_ListVM>.PostAsync(uri, grodispheadervm, headers);
        }
        public async Task<Gro_Stock_ListVM> UpdategrostockbyPart(Gro_Stock_ListVM grodispheadervm)
        {
            var uri = new Uri(_apiUrls.Gateway + $"/cwbgro/updategrostockbypart");
            var headers = await AppUtil.GetAuthToken(_httpContextAccessor.HttpContext);
            grodispheadervm.TenantId = tenantId;
            return await RestHelper<Gro_Stock_ListVM>.PostAsync(uri, grodispheadervm, headers);
        }
        public async Task<IEnumerable<Gro_Stock_DetVM>> GetallGroStockDet()
        {
            var uri = new Uri(_apiUrls.Gateway + $"/cwbgro/getallgrostockdetails/{tenantId}");
            var headers = await AppUtil.GetAuthToken(_httpContextAccessor.HttpContext);
            return await RestHelper<List<Gro_Stock_DetVM>>.GetAsync(uri, headers);
        }
        public async Task<Gro_Stock_ListVM> PostGroStockpart(Gro_Stock_ListVM companyVM)
        {
            var uri = new Uri(_apiUrls.Gateway + $"/cwbgro/postgrostocklist");
            var headers = await AppUtil.GetAuthToken(_httpContextAccessor.HttpContext);
            companyVM.TenantId = tenantId;
            return await RestHelper<Gro_Stock_ListVM>.PostAsync(uri, companyVM, headers);
        }
        public async Task<List<Gro_Stock_DetVM>> Updategrostockdetto1stscan(IEnumerable<Gro_Stock_DetVM> productions)
        {
            var uri = new Uri(_apiUrls.Gateway + $"/cwbgro/updategrostockdetslnoto1stscan");
            var headers = await AppUtil.GetAuthToken(_httpContextAccessor.HttpContext);
            foreach (var item in productions)
            {
                item.TenantId = tenantId;
            }
            return await RestHelper<List<Gro_Stock_DetVM>>.PostAsync(uri, productions, headers);
        }
        public async Task<Gro_Stock_ListVM> Getgrostockbypoartid(long gropartlistid)
        {
            var uri = new Uri(_apiUrls.Gateway + $"/cwbgro/getstockbygropart/{gropartlistid}/{tenantId}");
            var headers = await AppUtil.GetAuthToken(_httpContextAccessor.HttpContext);
            return await RestHelper<Gro_Stock_ListVM>.GetAsync(uri, headers);
        }
        public async Task<IEnumerable<Gro_Disp_DetVM>> GetallgroDispatchDetails()
        {
            var uri = new Uri(_apiUrls.Gateway + $"/cwbgro/Getallgrodispatchdetails/{tenantId}");
            var headers = await AppUtil.GetAuthToken(_httpContextAccessor.HttpContext);
            return await RestHelper<List<Gro_Disp_DetVM>>.GetAsync(uri, headers);
        }
        public async Task<Gro_Disp_DetVM> PostGroDispatchDetail(Gro_Disp_DetVM grodispheadervm)
        {
            var uri = new Uri(_apiUrls.Gateway + $"/cwbgro/postgrodispatchDetail");
            var headers = await AppUtil.GetAuthToken(_httpContextAccessor.HttpContext);
            grodispheadervm.TenantId = tenantId;
            return await RestHelper<Gro_Disp_DetVM>.PostAsync(uri, grodispheadervm, headers);
        }
        public async Task<List<Indent_Part_Sl_NoVM>> PostMultipleIndentSlno(List<Indent_Part_Sl_NoVM> grodispheadervm)
        {
            var uri = new Uri(_apiUrls.Gateway + $"/cwbgro/postmultipleindentpartslno");
            var headers = await AppUtil.GetAuthToken(_httpContextAccessor.HttpContext);
            foreach (var item in grodispheadervm)
            {
                item.TenantId = tenantId;
            }
            return await RestHelper <List<Indent_Part_Sl_NoVM>>.PostAsync(uri, grodispheadervm, headers);
        }
        public async Task<Gro_Disp_DetVM> UpdategroDispatchdetailqty(Gro_Disp_DetVM grodispheadervm)
        {
            var uri = new Uri(_apiUrls.Gateway + $"/cwbgro/updategrodispatchdetailqty");
            var headers = await AppUtil.GetAuthToken(_httpContextAccessor.HttpContext);
            grodispheadervm.TenantId = tenantId;
            return await RestHelper<Gro_Disp_DetVM>.PostAsync(uri, grodispheadervm, headers);
        }
        public async Task<IEnumerable<Indent_Part_Sl_NoVM>> GetallGroIndentpartSlno()
        {
            var uri = new Uri(_apiUrls.Gateway + $"/cwbgro/getallindentpartslno/{tenantId}");
            var headers = await AppUtil.GetAuthToken(_httpContextAccessor.HttpContext);
            return await RestHelper<List<Indent_Part_Sl_NoVM>>.GetAsync(uri, headers);
        }
        public async Task<Gro_DataVM> UpdateGroDatabaltoDispatch(Gro_DataVM companyVM)
        {
            var uri = new Uri(_apiUrls.Gateway + $"/cwbgro/updategrodatabaltodispatch");
            var headers = await AppUtil.GetAuthToken(_httpContextAccessor.HttpContext);
            companyVM.TenantId = tenantId;
            return await RestHelper<Gro_DataVM>.PostAsync(uri, companyVM, headers);
        }
        public async Task<Gro_Indent_DispHeadVM> PostGroIndentDispHeader(Gro_Indent_DispHeadVM grodispheadervm)
        {
            var uri = new Uri(_apiUrls.Gateway + $"/cwbgro/postgroindentdispheader");

            var headers = await AppUtil.GetAuthToken(_httpContextAccessor.HttpContext);
            grodispheadervm.TenantId = tenantId;
            return await RestHelper<Gro_Indent_DispHeadVM>.PostAsync(uri, grodispheadervm, headers);
        }
        public async Task<bool> DeleteInvoiceHeader(long doctypeId)
        {
            var uri = new Uri(_apiUrls.Gateway + $"/cwbgro/deletegrodispatchheader/{doctypeId}");
            var headers = await AppUtil.GetAuthToken(_httpContextAccessor.HttpContext);
            return await RestHelper<bool>.GetAsync(uri, headers);
        }
    }
}
