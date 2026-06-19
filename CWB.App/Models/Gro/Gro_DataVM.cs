using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CWB.App.Models.Gro
{
    public class Gro_DataVM
    {
        public long Gro_DataId { get; set; }
        public long CWB_Customer { get; set; }
        public long int_Part_No { get; set; }
        public DateTime? SentDate { get; set; }
        public string Excutive_Name { get; set; }
        public string Indent { get; set; }
        public string Company_Name { get; set; }
        //public string Company { get; set; }
        public long Gro_Part_No { get; set; }
        public string? GroPartNo { get; set; }

        public int Reqd_Quantity { get; set; }
        public string? IndentDateStr { get; set; }
        public string Remarks { get; set; }
        public string Shipping_Address { get; set; }
        public string Shipping_City { get; set; }
        public string Shipping_State { get; set; }
        public string Shipping_PINCODE { get; set; }
        public string Contact_Person { get; set; }
        public string Contact_Person_No { get; set; }
        public char Part_Not_Avl { get; set; }
        public int Bal_to_Disp { get; set; }
        public int QntyAval { get; set; }
        public long TenantId { get; set; }
    }
}
