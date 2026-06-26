using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CWB.Gro.GroUtils
{
    public class ApiRoutes
    {
        public const string Root = "api";

        public const string Version = "v1";

        public const string Base = Root + "/" + Version;
        public static class Gro
        {
            public const string GetAllGroData = Base + "/Getallgrodata/{tenantId}";
            public const string PostMultipleGroData = Base + "/postmultiplegrodata";
            public const string PostGrodata = Base + "/postgrodata";
            public const string DeleteGroData = Base + "/deletegrodata/{Id}";


            public const string AllGroPartList = Base + "/Getallgropartlist/{tenantId}";
            public const string PostMultiplePartList = Base + "/postmultiplegropartlist";
            public const string PostGroPartList = Base + "/postgropartlist";
            public const string DeleteGroPartList = Base + "/deletegropartlist/{Id}";

            public const string AllGroDispatchHeader = Base + "/Getallgrodispatchheader/{tenantId}";
            public const string PostMultipleGroDispatchHeader = Base + "/postmultiplegrodispatchheader";
            public const string PostGroDispatchHeader = Base + "/postgrodispatchheader";
            public const string UpdateGroDispHeaderAddress = Base + "/updategrodispheaderaddress";
            public const string UpdateGroDispatchHeaderAWB = Base + "/updategrodispheadercourierawbdispatchdate";
            public const string UpdateGroDispatchHeaderDeliveryDate = Base + "/updatedispheaderdeliverydate";
            public const string DeleteGroDispatchHeader = Base + "/deletegrodispatchheader/{Id}";

            public const string AllCustSpecificData = Base + "/Getallcustspecificdata/{tenantId}";
            public const string PostMultipleCustSpecificData = Base + "/postmultiplecustspecificdata";
            public const string PostCustSpecificData = Base + "/postcustspecificdata";
            public const string DeleteCustSpecificData = Base + "/deletecustspecificdata/{Id}";


            public const string AllGroDispatchDetails = Base + "/Getallgrodispatchdetails/{tenantId}";
            public const string PostMultipleGroDispatchDetails = Base + "/postmultiplegrodispatchdetails";
            public const string PostGroDispatchDetail = Base + "/postgrodispatchDetail";
            public const string DeleteGroDispatchDetail = Base + "/deletegrodispatchdetail/{Id}";


            public const string AllUploadformats = Base + "/Getalluploadformats/{tenantId}";
            public const string PostMultipleUploadFormats = Base + "/postmultipleuploadformats";
            public const string PostUploadFormat = Base + "/postuploadformat";
            public const string DeleteUploadformat = Base + "/deleteformat/{Id}";

            public const string GetFieldType = Base + "/getfeildtype/{Id}";


            public const string AllCourierlist = Base + "/Getallcourierlist/{tenantId}";
            public const string PostMultipleCourierList = Base + "/postmultiplecourier";
            public const string PostCourierList = Base + "/postcourier";
            public const string DeleteCourier = Base + "/deletecourier/{Id}";


            public const string AllPrintoutformats = Base + "/Getallprintoutformats/{tenantId}";
            public const string PostMultiplePrintoutFormat = Base + "/postmultipleprintoutformat";
            public const string PostPrintoutformat = Base + "/postprintoutformat";
            public const string DeletePrintoutformat = Base + "/deleteprintoutformat/{Id}";


            public const string AllGroStockList = Base + "/Getallgrostocklist/{tenantId}";
            public const string PostMultipleGroStockList = Base + "/postmultiplegrostocklist";
            public const string PostGroStockList = Base + "/postgrostocklist";
            public const string Updategrostocklastslno = Base + "/updategrostocklastslnobypartno";
            public const string DeleteGroStockList = Base + "/deletegrostocklist/{Id}";
            public const string GetStockByGroPartListId = Base + "/getstockbygropart/{gropartlistid}/{tenantId}";



            public const string AllTKDCInvContrl = Base + "/getalltkdcinctrl/{tenantId}";
            public const string PostMultipleTKDCInvContrl = Base + "/postmultipltkdcinvctrl";
            public const string PostTKDCInvContrl = Base + "/posttkdcinvctrl";
            public const string UpdateTkDclastDcandInvNo = Base + "/updatetkdclastdcandinvno";
            public const string DeleteTKDCInvContrl = Base + "/deletetkdcinvctrl/{Id}";


            public const string AllGroStockDet = Base + "/getallgrostockdetails/{tenantId}";
            public const string PostMultipleGroStockDet = Base + "/postmultiplegrostockdet";
            public const string UpdateGrostockdetStatusto1stScan = Base + "/updategrostockdetslnoto1stscan";
            public const string PostGroStockDet = Base + "/postgrostockdet";
            public const string DeleteGroStockDet = Base + "/deletegrostockdet/{Id}";

            public const string GetSLstatustype = Base + "/getslnostatusdesc/{Id}";

            public const string AllIndentPartSlNo = Base + "/getallindentpartslno/{tenantId}";
            public const string PostMultipleIndentPartSlNo = Base + "/postmultipleindentpartslno";
            public const string PostIndentPartSlNo = Base + "/postindentpartslno";
            public const string DeleteIndentPartSlNo = Base + "/deleteindentpartslno/{Id}";

        }
    }
}
