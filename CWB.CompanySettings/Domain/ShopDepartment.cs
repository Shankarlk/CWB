using CWB.CommonUtils.Common;
using System.Collections.Generic;

namespace CWB.CompanySettings.Domain
{
    public class ShopDepartment : BaseEntity
    {
        public string Name { get; set; }
        public int NoOfShifts { get; set; }
        public long TenantId { get; set; }
        public long PlantId { get; set; }
        public long Level_No { get; set; }
        public long Part_Of { get; set; }
        public string Activity { get; set; }
        public int ProdDept { get; set; }
        public string Section { get; set; }
        public Plant Plant { get; set; }

        public char Stores_DirectMatl { get; set; }

        public char Stores_Cust_Dispatch {get;set;}
        public char Stores_Tools { get; set; }
        public char Stores_Consumables { get; set; }
        public ICollection<Section> Sections { get; set; }
        //public ICollection<DocumentType> DocumentTypes { get; set; }
    }
}