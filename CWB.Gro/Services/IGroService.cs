using CWB.Gro.Domain;
using CWB.Gro.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CWB.Gro.Services
{
    public interface IGroService
    {
        Task<IEnumerable<Gro_DataVM>> AllGroData(long tenantId);
        Task<Gro_DataVM> PostGrodata(Gro_DataVM GroDataVM);
        Task<List<Gro_DataVM>> MultipleGrodata(List<Gro_DataVM> GroDataVM);
        Task<bool> DeleteGroData(long Id);


        Task<IEnumerable<Gro_Part_ListVM>> AllGroPartList(long tenantId);
        Task<List<Gro_Part_ListVM>> MultipleGroPartList(List<Gro_Part_ListVM> GroDataVM);
        Task<Gro_Part_ListVM> PostGroPartList(Gro_Part_ListVM GroDataVM);
        Task<bool> DeleteGroPartList(long Id);


        Task<IEnumerable<Gro_Disp_HeaderVM>> AllGroDispatchHeader(long tenantId);
        Task<List<Gro_Disp_HeaderVM>> MultipleGroDispatchHeader(List<Gro_Disp_HeaderVM> GroDataVM);
        Task<Gro_Disp_HeaderVM> PostGroDispatchHeader(Gro_Disp_HeaderVM GroDataVM);
        Task<Gro_Disp_HeaderVM> UpdateGroDispatchHeader(Gro_Disp_HeaderVM GroDataVM);
        Task<Gro_Disp_HeaderVM> UpdateGroDispatchHeaderAWB(Gro_Disp_HeaderVM GroDataVM);
        Task<Gro_Disp_HeaderVM> UpdateGroDispatchHeaderDeliveryDate(Gro_Disp_HeaderVM GroDataVM);
        Task<bool> DeleteGroDispatchHeader(long Id);


        Task<IEnumerable<Cust_Specific_DataVM>> AllCustSpecificData(long tenantId);
        Task<List<Cust_Specific_DataVM>> MultipleCustSpecificData(List<Cust_Specific_DataVM> GroDataVM);
        Task<Cust_Specific_DataVM> PostCustSpecificData(Cust_Specific_DataVM GroDataVM);
        Task<bool> DeleteCustSpecificData(long Id);



        Task<IEnumerable<Gro_Disp_DetVM>> AllGroDispatchDetails(long tenantId);
        Task<List<Gro_Disp_DetVM>> MultipleGroDispatchDetails(List<Gro_Disp_DetVM> GroDataVM);
        Task<Gro_Disp_DetVM> PostGroDispatchDetail(Gro_Disp_DetVM GroDataVM);
        Task<bool> DeleteGroDispatchDetail(long Id);


        Task<IEnumerable<Upload_FormatVM>> AllUploadformats(long tenantId);
        Task<List<Upload_FormatVM>> MultipleUploadFormats(List<Upload_FormatVM> GroDataVM);
        Task<Upload_FormatVM> PostUploadFormat(Upload_FormatVM GroDataVM);
        Task<bool> DeleteUploadformat(long Id);


        Task<Field_TypeVM> GetFeildType(long Id);



        Task<IEnumerable<Courier_ListVM>> AllCourierlist(long tenantId);
        Task<List<Courier_ListVM>> MultipleCourierList(List<Courier_ListVM> GroDataVM);
        Task<Courier_ListVM> PostCourierList(Courier_ListVM GroDataVM);
        Task<bool> DeleteCourier(long Id);

        Task<IEnumerable<Printout_formatVM>> AllPrintoutformats(long tenantId);
        Task<List<Printout_formatVM>> MultiplePrintoutFormat(List<Printout_formatVM> GroDataVM);
        Task<Printout_formatVM> PostPrintoutformat(Printout_formatVM GroDataVM);
        Task<bool> DeletePrintoutformat(long Id);



        Task<IEnumerable<Gro_Stock_ListVM>> AllGroStockList(long tenantId);
        Task<List<Gro_Stock_ListVM>> MultipleGroStockList(List<Gro_Stock_ListVM> stockListVM);
        Task<Gro_Stock_ListVM> PostGroStockList(Gro_Stock_ListVM stockVM);
        Task<Gro_Stock_ListVM> Updategrostocklastslno(Gro_Stock_ListVM controlVM);
        Task<bool> DeleteGroStockList(long id);
        Task<Gro_Stock_ListVM> GetStockByGroPartListId(long groPartListId, long tenantId);



        Task<IEnumerable<TK_DC_Inv_ContrlVM>> AllTKDCInvContrl(long tenantId);
        Task<List<TK_DC_Inv_ContrlVM>> MultipleTKDCInvContrl(List<TK_DC_Inv_ContrlVM> controlVM);
        Task<TK_DC_Inv_ContrlVM> PostTKDCInvContrl(TK_DC_Inv_ContrlVM controlVM);
        Task<TK_DC_Inv_ContrlVM> UpdateTkDclastDcandInvNo(TK_DC_Inv_ContrlVM controlVM);
        Task<bool> DeleteTKDCInvContrl(long id);



        Task<IEnumerable<Gro_Stock_DetVM>> AllGroStockDet(long tenantId);
        Task<List<Gro_Stock_DetVM>> MultipleGroStockDet(List<Gro_Stock_DetVM> GroStockDetVM);
        Task<List<Gro_Stock_DetVM>> UpdateGrostockdetStatusto1stScan(List<Gro_Stock_DetVM> GroStockDetVM);
        Task<Gro_Stock_DetVM> PostGroStockDet(Gro_Stock_DetVM GroStockDetVM);
        Task<bool> DeleteGroStockDet(long Id);

        Task<Sl_No_Status_ListVM> GetSLstatustype(long Id);



        Task<IEnumerable<Indent_Part_Sl_NoVM>> AllIndentPartSlNo(long tenantId);
        Task<List<Indent_Part_Sl_NoVM>> MultipleIndentPartSlNo(List<Indent_Part_Sl_NoVM> indentPartSlNoVM);
        Task<Indent_Part_Sl_NoVM> PostIndentPartSlNo(Indent_Part_Sl_NoVM indentPartSlNoVM);
        Task<bool> DeleteIndentPartSlNo(long Id);
    }
}
