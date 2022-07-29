using System;

namespace MicronTMS.DLL.Entities
{ 
    public class CMcVersion
    {
        public virtual decimal Id { get; set; }
        public virtual string VersionNumber { get; set; }
        public virtual string LmUser { get; set; }
        public virtual DateTime? LmTime { get; set; }
    }
}