namespace CWB.CompanySettings.CompanySettingsUtils
{
    public static class ApiRoutes
    {
        public const string Root = "api";

        public const string Version = "v1";

        public const string Base = Root + "/" + Version;

        public static class Aquisition
        {
            public const string GetPOLogs = Base + "/getpologs/{tenantId}/{customerOrderId}";
            public const string GetSalesOrders = Base + "/getsalesorders/{tenantId}/{customerOrderId}";
            public const string GetCustomerOrders = Base + "/getcustomerorders/{tenantId}";
            public const string GetSchedules = Base + "/getschedules/{tenantId}/{customerOrderId}";
            public const string AddSalesOrders = Base + "/addsalesorders/{tenantId}/{customerOrderId}";
            public const string HelloWorld = Base + "/helloworld/{tenantId}";

            public const string GetSOAggregate = Base + "/getsoaggregate/{tenantId}/{customerOrderId}";
            public const string PostSOAggregate = Base + "/soaggregate";

            public const string PostSalesOrder = Base + "/salesorder";

            public const string PostDeliverySchedule = Base + "/deliveryschedule";
            public const string PostCustomerOrder = Base + "/custorder";
            public const string PostOrderStatus = Base + "/orderstatus";
            public const string PostPOLog = Base + "/polog";
            public const string PostSOLog = Base + "/solog";
            

            public const string RemoveSalesOrder = Base + "/removesalesorder/{tenantId}/{salesOrderId}";
            public const string RemoveCustomerOder = Base + "/removecustomerorder/{tenantId}/{customerOrderId}";
            public const string RemoveDeliverSchedule = Base + "/removedeliveryschedule/{tenantId}/{scheduleId}";
            public const string RemoveOrderStatus = Base + "/removeorderstatus/{tenantId}/{orderStatusId}";

        }

        public static class DocType
        {
            public const string GetDocumentTypes = Base + "/document-types/{tenantId}";
            public const string PostDocumentType = Base + "/document-type";
            public const string GetDocumentType = Base + "/getdoctype/{docTypeId}";
            public const string DelDocumentType = Base + "/deldoctype/{docTypeId}";
        }

        public static class Plant
        {
            public const string GetPlants = Base + "/plants/{tenantId}";
            public const string GetCitys = Base + "/getcitys/{tenantId}";
            public const string GetSections = Base + "/getsections/{tenantId}";
            public const string GetNoOfDaysTimeSlots = Base + "/getnoofdaystimeslot";
            public const string GetTimeSlotDurations = Base + "/gettimeslotduration";
            public const string GetCountrys = Base + "/getcountry/{tenantId}";
            public const string CheckCountrys = Base + "/checkcountry/{country}";
            public const string CheckCity = Base + "/checkcity/{city}";
            public const string CheckSections = Base + "/checksection/{city}";
            public const string DelSections = Base + "/deletesection/{sectionId}";
            public const string PostPlant = Base + "/plant";
            public const string CheckPlant = Base + "/plant-exist";
            public const string GetPlant = Base + "/getplant/{plantId}";
            public const string DelPlant = Base + "/delplant/{plantId}";

            public const string PostPlantWD = Base + "/plantwd";
            public const string PostCity = Base + "/postcity";
            public const string PostSections = Base + "/postsection";
            public const string PostCountry = Base + "/postcountry";
            public const string GetPlantWD = Base + "/getplantwd/{tenantId}/{plantId}";
            public const string Holidays = Base + "/holidays/{tenantId}/{plantId}";
            public const string PostHoliday = Base + "/holiday";
            public const string DelHoliday = Base + "/delholiday/{tenantId}/{holidayId}";
        }
        public static class Department
        {
            public const string GetDepartments = Base + "/departments/{Id}/{tenantId}";
            public const string GetDepartmentsWithPlants = Base + "/plant-departments";
            public const string PostDepartment = Base + "/department";
            public const string DelDepartment = Base + "/deldepartment/{departmentId}";
            public const string CheckDepartment = Base + "/department-exist";
            public const string GetDept_Role_List = Base + "/getdeptrolelist/{tenantId}";
            public const string DelDept_Role_List = Base + "/deletedeptrolelist/{sectionId}";
            public const string PostDept_Role_List = Base + "/postdeptrolelist";
            public const string GetEmployee_Pwd = Base + "/getemployepwd/{tenantId}";
            public const string DelEmployee_Pwd = Base + "/deleteemployepwd/{sectionId}";
            public const string PostEmployee_Pwd = Base + "/postemployepwd";
            public const string GetDept_Employee = Base + "/getdeptemp/{tenantId}";
            public const string DelDept_Employee = Base + "/deletedeptemp/{sectionId}";
            public const string PostDept_Employee = Base + "/postdeptemp";
            
            public const string GetEmployee_UI_List = Base + "/getempuilist/{tenantId}";
            public const string DelEmployee_UI_List = Base + "/deleteempuilist/{sectionId}";
            public const string PostEmployee_UI_List = Base + "/postempuilist";
            public const string GetStoresIDs = Base + "/getallstoresid/{tenantId}";
        }

        public static class Designation
        {
            public const string GetDesignations = Base + "/designations/{tenantId}";
            public const string PostDesignation = Base + "/designation";
            public const string DelDesignation = Base + "/deldesignation/{designationId}";
        }
        public static class EmployeeMaster
        {
            public const string GetAllUiList = Base + "/getalluilist/{tenantId}";
            public const string GetAllOrgChart = Base + "/getallorgchart/{tenantId}";
            public const string GetAllRoleList = Base + "/getallrolelist/{tenantId}";
            public const string GetAllRoleUiList = Base + "/getallroleuilist/{tenantId}";
            public const string GetAllEmplRoleList = Base + "/getallemplrolelist/{tenantId}";
            public const string GetAllPermissionList = Base + "/getallpermissionlist/{tenantId}";
            public const string GetAllEmployee = Base + "/getallemployeelist/{tenantId}";
            public const string PostUilist = Base + "/postuilist";
            public const string PostOrgChart = Base + "/postorgchart";
            public const string PostRoleList = Base + "/postrolelist";
            public const string PostRoleUiList = Base + "/postroleuilist";
            public const string PostEmplRoleList = Base + "/postemplrolelist";
            public const string PostPermissionList = Base + "/postpermissionlist";
            public const string PostEmployee = Base + "/postemployee";
            public const string DelUiList = Base + "/deluilist/{designationId}";
            public const string DelOrgChart = Base + "/delorgchart/{designationId}";
            public const string DelRoleList = Base + "/delrolelist/{designationId}";
            public const string DelRoleUiList = Base + "/delroleuilist/{designationId}";
            public const string DelEmplRoleList = Base + "/delemplrolelist/{designationId}";
            public const string DelPermissionList = Base + "/delpermissionlist/{designationId}";
            public const string DelEmployee = Base + "/delemployee/{designationId}";
            
        }
    }
}
