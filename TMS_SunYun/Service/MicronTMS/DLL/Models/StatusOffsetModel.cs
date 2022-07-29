using System;

namespace MicronTMS.DLL.Models
{
    public class StatusOffsetModel
    {
        public virtual string EqpId { get; set; }
        public virtual string Status { get; set; }
        public virtual decimal Offset { get; set; }
    }
}