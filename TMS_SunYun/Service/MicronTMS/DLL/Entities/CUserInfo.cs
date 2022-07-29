using System;

namespace MicronTMS.DLL.Entities
{
    public class CUserInfo
    {
        public virtual string UserId { get; set; }
        public virtual string UserName { get; set; }
        public virtual string Email { get; set; }
        public virtual string ContactNo { get; set; }
        public virtual string LmUser { get; set; }
        public virtual DateTime? LmTime { get; set; }
    }
}