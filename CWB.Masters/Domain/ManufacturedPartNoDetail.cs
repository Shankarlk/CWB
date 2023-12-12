using CWB.CommonUtils.Common;
using CWB.Masters.MastersUtils.ItemMaster;
using System;

namespace CWB.Masters.Domain
{

    /**
     *   .ForMember(s => s.PartId, s => s.MapFrom(src => src.PartId))
     */
    public class ManufacturedPartNoDetail : BaseEntity
    {
        //DbPart --Start
        public long ManufacturedPartType { get; set; }
        public int CompanyId { get; set; }
        public long FinishedWeight { get; set; }
        public int PartId { get; set; }
        public long UOMId { get; set; }
        //DbPart --End

        //public string CompanyName { get; set; }
        //public string PartNumber { get; set; }
      /*  public string? PartDescription { get; set; }
        public string? RevNo { get; set; }
        public DateTime? RevDate { get; set; }
        public MasterPartType MasterPartType { get; set; }
        public PartStatus Status { get; set; }
        public string? StatusChangeReason { get; set; }*/

        public long TenantId { get; set; }


    }
}
