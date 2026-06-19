using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CWB.Gro.ViewModels
{
    public class Upload_FormatVM
    {
        public long Upload_Format_ID { get; set; }
        public long CWB_Customer { get; set; }
        public string Column_Name { get; set; }
        public string Column_Position { get; set; }

        public long Field_Type { get; set; }
        public string Mapped_Field { get; set; }
        public long TenantId { get; set; }
    }
}
