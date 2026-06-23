using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CWB.App.Models.Gro
{
    public class Gro_Disp_HeaderVM
    {
        public long Gro_Disp_HeaderId { get; set; }
        public long CWB_Customer { get; set; }
        public string Indent { get; set; }
        public DateTime? SentDate { get; set; }

        public string Shipping_Address { get; set; }
        public string Shipping_City { get; set; }
        public string Shipping_State { get; set; }
        public string Shipping_PINCODE { get; set; }
        public long Courier_Partner { get; set; }
        public DateTime? Dispatch_Date { get; set; }
        public string Status { get; set; }
        public string AWB { get; set; }

        public char DC_Printed { get; set; }
        public DateTime? Delivered_Date { get; set; }

        public string DC_No { get; set; }
        public string Inv_No { get; set; }
        public char Inv_Printed { get; set; }
        public char Inv_Uploaded { get; set; }
        public char Customer_Inv_Attached { get; set; }

        public string Company_Name { get; set; }
        public string? IndentDateStr { get; set; }
        public string? DispatchDateStr { get; set; }
        public string Contact_Person { get; set; }
        public string Contact_Person_No { get; set; }
        public string Excutive_Name { get; set; }
        public string Courier { get; set; }
        public long TenantId { get; set; }
    }
}
