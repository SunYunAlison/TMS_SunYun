using System;

namespace MicronTMS.DLL.Entities
{
    public class RCtlEtl
    {
        public virtual string LoaderName { get; set; }
        public virtual DateTime? LastReportTime { get; set; }
        public virtual string ChatGroup { get; set; }
        public virtual string SMSUser { get; set; }
        public virtual string SMSPassword { get; set; }
        public virtual string ActiveFlag { get; set; }
        public virtual string LmUser { get; set; }
        public virtual DateTime? LmTime { get; set; }
    }
}