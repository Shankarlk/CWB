using CWB.Masters.Domain.ItemMaster;
using CWB.Masters.MastersUtils.ItemMaster;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CWB.Masters.ViewModels.ItemMaster
{
    public class PartsStatusVM
    {
        public string Status { get; set; }
        public long PartsStatusId { get; set; }
        public long TenantId { get; set; }
    }
}
