using System;

namespace MicronTMS.DLL.Entities
{
    public class HTeleMessage
    {
        public virtual string EventId { get; set; }
        public virtual DateTime? ReportTime { get; set; }
        public virtual string MessageCode { get; set; }
        public virtual string MessageBody { get; set; }
        public virtual string ChatId { get; set; }
        public virtual string ChatGroup { get; set; }
        public virtual string LmUser { get; set; }
        public virtual DateTime? LmTime { get; set; }
    }
}