using CWB.App.AppUtils;
using System;
using System.ComponentModel;
using System.Text.Json.Serialization;

namespace CWB.App.Models.Routing
{
    public class RoutingStepMachineVM
    {
        public long RoutingStepMachineId { get; set; }
        public long TenantId { get; set; }
        public long MachineId { get; set; }
        public long RoutingStepId { get; set; }

        //[JsonConverter(typeof(TimeSpanToStringConverter))]
        public string SetupTime { get; set; }
        //[JsonConverter(typeof(TimeSpanToStringConverter))]
        public string FloorToFloorTime { get; set; }
        //[JsonConverter(typeof(TimeSpanToStringConverter))]
        public string FirstPieceProcessingTime { get; set; }
        public int NoOfPartsPerLoading { get; set; }
    }
}
