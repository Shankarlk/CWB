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
        Task<IEnumerable<Courier_ListVM>> GetallcourierList();
        Task<Courier_ListVM> PostCourier(Courier_ListVM grodispheadervm);

        Task<IEnumerable<TK_DC_Inv_ContrlVM>> Getalltkdcinvctrl();
        Task<TK_DC_Inv_ContrlVM> Posttkdcctrl(TK_DC_Inv_ContrlVM grodispheadervm);
    }
}
