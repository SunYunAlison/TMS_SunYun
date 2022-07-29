using System;

namespace MicronTMS.DLL.Entities
{
    public class HEqpStatus
    {
        public virtual string EqpId { get; set; }
        public virtual string Status { get; set; }
        public virtual string SubStatus { get; set; }
        public virtual Decimal Temp { get; set; }
        public virtual string IsOpen { get; set; }
        public virtual DateTime? DoorOpenTime { get; set; }
        public virtual string UpdateBy { get; set; }
        public virtual DateTime UpdateTime { get; set; }
    }
}