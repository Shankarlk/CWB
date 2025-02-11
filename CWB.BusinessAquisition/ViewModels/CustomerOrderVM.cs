using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CWB.BusinessAquisition.ViewModels
{
    public class CustomerOrderVM
    {
        public long CustomerOrderId { get; set; }
        public string? CustomerName { get; set; } = string.Empty;
        public string? HoldStr { get; set; } = string.Empty;
        public int OrderType { get; set; }
        public long CustomerId { get; set; }
        public string? PONumber { get; set; }
        public DateTime? PODate { get; set; }
        public string? DirectEntryDetails { get; set; }
        public string? POAddress { get; set; }
        public string? POCity { get; set; }
        public string? POPIN { get; set; }
        public string? POCountry { get; set; }
        public string? Comment { get; set; }
        public string? LineNo { get; set; }
        public int Status { get; set; }
        public int Plan { get; set; }
        public int Matl { get; set; }
        public int WIP { get; set; }
        public bool Hold { get; set; }
        public bool Done { get; set; }
        public long TenantId { get; set; }
    }
}
