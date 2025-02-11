using CWB.CommonUtils.Common;
using System;
using System.Collections.Generic;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace CWB.App.Models.BusinessProcesses
{
    public class CustomerOrderVM 
    {
        DateTime? poDate = null;
        public long CustomerOrderId { get; set; }
        public int OrderType { get; set; }
        public long CustomerId { get; set; }
        public string? CustomerName { get; set; } = string.Empty;
        public string? PONumber { get; set; }

        public DateTime? PODate
        {
            get { return poDate; }
            set { poDate = value; }
        }

        public String PODateStr
        {
            get
            {
                if (poDate == null)
                {
                    return "";
                }
                return poDate.Value.ToString("dd-MM-yyyy");
            }
            set { }
        }
        public string? DirectEntryDetails {  get; set; }
        public string? POAddress {  get; set; }
        public string? POCity {  get; set; }
        public string? POPIN {  get; set; }
        public string? POCountry {  get; set; }
        public string? Comment { get; set; }
        public string? LineNo { get; set; }

        /*public int Status { get; set; }
        public int Plan { get; set; }
        public int Matl { get; set; }
        public int WIP { get; set; }
        public bool Hold { get; set; }
        public bool Done { get; set; }*/

        public long TenantId { get; set; }
  
    }
}
