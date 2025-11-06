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
        public bool? ProdDept { get;set; }
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


    }
}
