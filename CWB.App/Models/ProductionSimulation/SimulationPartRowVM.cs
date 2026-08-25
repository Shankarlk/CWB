using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CWB.App.Models.ProductionSimulation
{
    public class SimulationPartRowVM
    {
        public long WoId { get; set; }

        public string PartNo { get; set; }

        public string Description { get; set; }

        public bool IsUrgent { get; set; }

        public List<SimulationOperationVM> Operations { get; set; } = new List<SimulationOperationVM>();

        public string PartNoColor { get; set; }
        public double SoCompletionMarkerLeft { get; set; }
        public bool HasSoCompletionMarker { get; set; }
        public string PartNoTitle { get; set; }
        public string SoCustomer { get; set; }
        public string SoNo { get; set; }
        public string SoQnty { get; set; }
        public string SoCompletionDateDisplay { get; set; }
        public double MatlReceiptMarkerLeft { get; set; }
        public bool HasMatlReceiptMarker { get; set; }
        public string MatlReceiptPartNo { get; set; }
        public string MatlReceiptPartType { get; set; }
        public string MatlReceiptDateDisplay { get; set; }

        public string MatlReceiptPartNoDesc { get; set; }
        public string MatlReceiptQnty { get; set; }
        public string MatlReceiptSupplier { get; set; }
        public string MatlReceiptPoNo { get; set; }
    }
}
