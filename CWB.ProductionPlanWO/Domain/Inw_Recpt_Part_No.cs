using CWB.CommonUtils.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CWB.ProductionPlanWO.Domain
{
    public class Inw_Recpt_Part_No:BaseEntity
    {
        public long PO_Details_Id { get; set; }
        public char Release_Insp { get; set; }
        public char Insp_Complete { get; set; }
        public long TenantId { get; set; }
    }
}
