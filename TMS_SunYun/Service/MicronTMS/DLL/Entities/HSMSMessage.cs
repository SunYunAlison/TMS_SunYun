using System;

namespace MicronTMS.DLL.Entities
{
    public class HSMSMessage
    {
        public virtual string EventId { get; set; }
        public virtual DateTime? ReportTime { get; set; }
        public virtual string MessageCode { get; set; }
        public virtual string MessageBody { get; set; }
        public virtual string PhoneNo { get; set; }
        public virtual string LmUser { get; set; }
        public virtual DateTime? LmTime { get; set; }
    }
}