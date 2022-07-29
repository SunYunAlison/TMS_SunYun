using System;

namespace MicronTMS.DLL.Entities
{
    public class HUserInfo
    {
        public virtual string TrxName { get; set; }
        public virtual string EventId { get; set; }
        public virtual string UserId { get; set; }
        public virtual string UserName { get; set; }
        public virtual string Email { get; set; }
        public virtual string ContactNo { get; set; }
        public virtual string LmUser { get; set; }
        public virtual DateTime? LmTime { get; set; }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;
            var t = obj as HUserInfo;
            if (t == null)
                return false;
            if (EventId == t.EventId && UserId == t.UserId)
                return true;
            return false;
        }

        public override int GetHashCode()
        {
            int hashCode = 0;
            hashCode = hashCode ^ EventId.GetHashCode() ^ UserId.GetHashCode();
            return hashCode;
        }
    }
}