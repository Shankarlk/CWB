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

        public static class ManufacturedPartNoDetail
        {
            public const string PostManufacturedPartNoDetail = Base + "/manufacturedpartnodetail";
            public const string PostMPMakeFrom = Base + "/mpmakefrom";
            public const string PostMPBOM = Base + "/mpbom";
            // Added for Listing ManufacturedPartNoDetails
            public const string GetManufacturedPartNoDetailList = Base + "/getmanufacturedpartnodetailList/{ManufPartType}/{companyName}";
            public const string GetMPMakeFromListBypartNumber = Base + "/mpmakefromlist/{partNumber}/{tenantId}";
            public const string HelloWorld = Base + "/helloworld";
            public const string GetUOMs = Base + "/getuoms/{tenantId}";
            public const string PostRawMaterialDetail = Base + "/rawmaterialdetail";
            // Added for Listing RawMaterialDetails
            public const string GetRawMaterialDetailList = Base + "/getrawmaterialdetailList/{tenantId}";
        }

        public static class RawMaterialDetail
        {
            public const string PostRawMaterialDetail = Base + "/rawmaterialdetail";
            // Added for Listing RawMaterialDetails
            public const string GetRawMaterialDetailList = Base + "/getrawmaterialdetailList/{tenantId}";
        }

        public static class BoughtOutFinishDetail
        {
            public const string PostBoughtOutFinishDetail = Base + "/boughtoutfinishdetail";
            // Added for Listing BoughtOutFinishDetails
            public const string GetBoughtOutFinishDetailList = Base + "/getboughtoutfinishdetailList/{tenantId}";
        }
    }
}
