namespace CWB.App.Models.Departments
{
    public class ShopDepartmentVM
    {
        public long DepartmentId { get; set; }
        public string Name { get; set; }
        public int NoOfShifts { get; set; }
        public long PlantId { get; set; }
        public long Level_No { get; set; }
        public long Part_Of { get; set; }
        public string Activity { get; set; }
        public int ProdDept { get;set; }
        public long TenantId { get; set; }
        public string PlantName { get; set; } = "";
        public string Section { get; set; } = "-";
        public string Level1 { get; set; } = "-";
        public string Level2 { get; set; } = "-";
        public string Level3 { get; set; } = "-";
        public string Level4 { get; set; } = "-";
        public string Level5 { get; set; } = "-";
        public string RoleName { get; set; } = " ";
        public string EmpName { get; set; } = " ";
        public char Stores_DirectMatl { get; set; }

        public char Stores_Cust_Dispatch { get; set; }
        public char Stores_Tools { get; set; }
        public char Stores_Consumables { get; set; }
        public char Prodn { get; set; }     // Y / '\0'
        public string Stores { get; set; }

    }
}
