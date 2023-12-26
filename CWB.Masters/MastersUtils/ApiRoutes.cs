namespace CWB.Masters.MastersUtils
{
    public static class ApiRoutes
    {
        public const string Root = "api";

        public const string Version = "v1";

        public const string Base = Root + "/" + Version;

        public static class Company
        {
            public const string GetCompanyTypes = Base + "/company-types";
            public const string GetDivisionsById = Base + "/divisions/{Id}/{tenantId}";
            public const string GetCompanies = Base + "/companies/{Id}";
            public const string PostCompany = Base + "/company";
            public const string IsCompanyExist = Base + "/check-company";
            public const string IsDivisionExist = Base + "/check-division";
            

        }

        public static class OperationList
        {
            public const string GetOperationList = Base + "/operation-list/{Id}";
            public const string GetOperation = Base + "/operation-list/{Id}/{tenantId}";
            public const string GetOperationalDocumentTypes = Base + "/operation-list-doctypes/{Id}/{tenantId}";
            public const string PostOperation = Base + "/operation";
            public const string PostOperationalDocumentTypes = Base + "/operation-doctype";
            public const string IsOperationExist = Base + "/check-operation";
        }

        public static class Machine
        {
            public const string GetMachineTypes = Base + "/machine-types/{Id}";
            public const string PostMachineType = Base + "/machine-type";
            public const string PostMachine = Base + "/machine";
            public const string GetMachine = Base + "/machine/{Id}/{tenantId}";
            public const string GetMachines = Base + "/machines/{Id}";
            public const string IsMachineTypeExist = Base + "/machine-type-check";
            public const string IsMachineExist = Base + "/machine-check";
            public const string GetMachineProcDocs = Base + "/machine-procs-docs/{Id}/{tenantId}";
            public const string PostMachineProcDoc = Base + "/machine-procs-doc";
        }


        public static class Masters
        {
            public const string GetStatuses = Base + "/statuses";
        }

        public static class Routings {
            public const string RoutingListItems = Base + "/routinglistitems";
            //public const string RoutingListItem = Base + "/routinglistitem/{manufPartId}";
            public const string PostNewRouting = Base + "/newrouting";
            public const string PostRoutingStep = Base + "/routingstep";
            public const string PostRoutingStepPart = Base + "/routingsteppart";
            public const string RoutingList = Base + "/routings/{manufPartId}";
            public const string RoutingSteps = Base + "/routingsteps/{routingId}";
            public const string StepParts = Base + "/stepparts/{stepId}";
            public const string StepPartsByManufId = Base + "/steppartsbymanufid/{manufId}";
            
        }

        public static class ManufacturedPartNoDetail
        {
            public const string PostManufacturedPartNoDetail = Base + "/manufacturedpartnodetail";
            public const string PostMPMakeFrom = Base + "/mpmakefrom";
            public const string PostMPBOM = Base + "/mpbom";
            // Added for Listing ManufacturedPartNoDetails
            public const string GetManufPart = Base + "/getmanufpart/{partId}/{tenantId}";
            public const string GetRMPart = Base + "/getrmpart/{partId}/{tenantId}";
            public const string GetBOFPart = Base + "/getbofpart/{partId}/{tenantId}";

            public const string GetManufacturedPartNoDetailList = Base + "/getmanufacturedpartnodetailList/{ManufPartType}/{companyName}/{tenantId}";
            public const string GetAllManufacturedPartNoDetailList = Base + "/mfdlist/{tenantId}";
            public const string GetMPMakeFromList = Base + "/mpmakefromlist/{partId}";//pass manufPartId from MPRawMeterials
            public const string GetMPMakeFrom = Base + "/getmakefrom/{Id}";
            public const string RemMakeFrom = Base + "/remmakefrom";

            public const string GetMPBOMList = Base + "/boms/{partId}";//pass manufPartId from MPBOMs
            public const string GetMPBOM = Base + "/getbom/{Id}";
            public const string RemBOM = Base + "/rembom";
          //  public const string HelloWorld = Base + "/helloworld";
            public const string GetUOMs = Base + "/getuoms/{tenantId}";
        }

        public static class RawMaterialDetail
        {
            //rawmateriadetail
            public const string PostRawMaterialDetail = Base + "/rawmaterialdetail";
            // Added for Listing RawMaterialDetails
            public const string GetRawMaterialDetailList = Base + "/getrawmaterialdetailList/{tenantId}";
            public const string GetRMTypes = Base + "/rmtypes";
            public const string GetRMSpecs = Base + "/rmspecs";
            public const string GetRMStandards = Base + "/rmstandards";
            public const string GetBaseRMs = Base + "/baserms";
            public const string BaseRM = Base + "/baserm";
            public const string RMType = Base + "/rmtype";
            public const string RMSpec = Base + "/rmspec";
            public const string RMStandard = Base + "/rmstandard";

            public const string GetPartPurchasesForPartId = Base + "/purchasesbypartId/{partId}/{tenantId}";
            
            //Get All objects
            public const string GetPartPurchases = Base + "/partpurchases/{tenantId}";
            //Add/Edit
            public const string PostPartPurchaseDetail = Base + "/partpurchase";
            //Get a single object
            public const string GetPartPurchase = Base + "/getpartpurchase/{partPurchaseId}/{tenantId}";
            //public const string RemovePartPurchase = Base + "/rempartpurchase/{partPurchaseId}";
            public const string RemPartPurchaseDetail = Base + "/rempartpurchase";



            public const string OwnRMS = Base + "/ownrms/{tenantId}";
            public const string SupplierRMS = Base + "/supplierrms/{supplierId}";

            public const string GetMasterParts = Base + "/itemmasterparts/{tenantId}";
            public const string GetSelectParts = Base + "/selectparts/{tenantId}";
            //itemmasterparts
           
            
        }

        public static class MasterParts
        {
            public const string CheckPartNo = Base + "/check-partno/{partNo}";
        }

       
        public static class BoughtOutFinishDetail
        {
            public const string PostBoughtOutFinishDetail = Base + "/boughtoutfinishdetail";
            // Added for Listing BoughtOutFinishDetails
            public const string GetBoughtOutFinishDetailList = Base + "/bofs/{tenantId}";
        }
    }
}
