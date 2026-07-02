using CWB.App.Models.Contacts;
using CWB.App.Models.ItemMaster;
using CWB.App.Models.Routing;
using CWB.App.Models.Routings;
using CWB.App.Models.Gro;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CWB.App.Services.Gro
{
    public  interface IGroService
    {
        Task<UploadValidationResult> ValidateExcel(IFormFile file);

        Task<IEnumerable<Gro_Part_ListVM>> Getallgroparts();
        Task<Gro_Part_ListVM> PostGroPart(Gro_Part_ListVM companyVM);

        Task<IEnumerable<Gro_DataVM>> GetallgroData();

        Task<List<Gro_DataVM>> PostMultipleGrodata(IEnumerable<Gro_DataVM> productions);

        Task<Cust_Specific_DataVM> PostCustSpecificData(Cust_Specific_DataVM companyVM);
        Task<IEnumerable<Cust_Specific_DataVM>> GetallCustSpecificData();

        Task<Gro_Stock_ListVM> GetGropartnoStock(long gropartid);
        Task<IEnumerable<Gro_Stock_ListVM>> Getallgrostocklist();

        Task<IEnumerable<Gro_Disp_HeaderVM>> GetallgroDispHeader();
        Task<Gro_Disp_HeaderVM> PostGroDispHeader(Gro_Disp_HeaderVM grodispheadervm);
        Task<Gro_Disp_HeaderVM> UpdateGroDispHeaderAddress(Gro_Disp_HeaderVM grodispheadervm);
        Task<Gro_Disp_HeaderVM> UpdateGroDispHeaderAWB(Gro_Disp_HeaderVM grodispheadervm);
        Task<Gro_Disp_HeaderVM> UpdateGroDispHeaderDeliveryDate(Gro_Disp_HeaderVM grodispheadervm);

        Task<IEnumerable<Courier_ListVM>> GetallcourierList();
        Task<Courier_ListVM> PostCourier(Courier_ListVM grodispheadervm);

        Task<IEnumerable<TK_DC_Inv_ContrlVM>> Getalltkdcinvctrl();
        Task<TK_DC_Inv_ContrlVM> Posttkdcctrl(TK_DC_Inv_ContrlVM grodispheadervm);
        Task<Gro_Stock_ListVM> PostGroStockpart(Gro_Stock_ListVM companyVM);
        Task<TK_DC_Inv_ContrlVM> UpdateTkDcLastInvandDcNo(TK_DC_Inv_ContrlVM grodispheadervm);
        Task<Gro_Stock_DetVM> PostGroStockDet(Gro_Stock_DetVM grodispheadervm);
        Task<Gro_Stock_ListVM> UpdategrostocklastslnobyPartNo(Gro_Stock_ListVM grodispheadervm);
        Task<Gro_Stock_ListVM> UpdategrostockbyPart(Gro_Stock_ListVM grodispheadervm);
        Task<IEnumerable<Gro_Stock_DetVM>> GetallGroStockDet();
        Task<List<Gro_Stock_DetVM>> Updategrostockdetto1stscan(IEnumerable<Gro_Stock_DetVM> productions);
        Task<Gro_Stock_ListVM> Getgrostockbypoartid(long gropartlistid);
        Task<IEnumerable<Gro_Disp_DetVM>> GetallgroDispatchDetails();
        Task<Gro_Disp_DetVM> PostGroDispatchDetail(Gro_Disp_DetVM grodispheadervm);

        Task<Gro_Disp_DetVM> UpdategroDispatchdetailqty(Gro_Disp_DetVM grodispheadervm);
        Task<List<Indent_Part_Sl_NoVM>> PostMultipleIndentSlno(List<Indent_Part_Sl_NoVM> grodispheadervm);
        Task<IEnumerable<Indent_Part_Sl_NoVM>> GetallGroIndentpartSlno();
        Task<Gro_DataVM> UpdateGroDatabaltoDispatch(Gro_DataVM companyVM);
        Task<Gro_Indent_DispHeadVM> PostGroIndentDispHeader(Gro_Indent_DispHeadVM grodispheadervm);
        }
}
