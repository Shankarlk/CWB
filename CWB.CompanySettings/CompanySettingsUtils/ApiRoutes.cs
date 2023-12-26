namespace CWB.CompanySettings.CompanySettingsUtils
{
    public static class ApiRoutes
    {
        public const string Root = "api";

        public const string Version = "v1";

        public const string Base = Root + "/" + Version;

        public static class DocType
        {
            public const string GetDocumentTypes = Base + "/document-types/{Id}";
            public const string PostDocumentType = Base + "/document-type";
        }

        public static class Plant
        {
            public const string GetPlants = Base + "/plants/{Id}";
            public const string PostPlant = Base + "/plant";
            public const string CheckPlant = Base + "/plant-exist";
        }
        public static class Department
        {
            public const string GetDepartments = Base + "/departments/{Id}/{TenantId}";
            public const string GetDepartmentsWithPlants = Base + "/plant-departments";
            public const string PostDepartment = Base + "/department";
            public const string CheckDepartment = Base + "/department-exist";
        }

        public static class Designation
        {
            public const string GetDesignations = Base + "/designations/{Id}/{TenantId}";
            public const string PostDesignation = Base + "/designation";
        }
    }
}
