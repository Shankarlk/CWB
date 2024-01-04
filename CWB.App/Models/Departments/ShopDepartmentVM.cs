namespace CWB.App.Models.Departments
{
    public class ShopDepartmentVM
    {
        public long DepartmentId { get; set; }
        public string Name { get; set; }
        public int NoOfShifts { get; set; }
        public long PlantId { get; set; }
        public string Activity { get; set; }
        public long TenantId { get; set; }
        
    }
}
