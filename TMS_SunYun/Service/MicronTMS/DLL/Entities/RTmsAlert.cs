using System;

namespace MicronTMS.DLL.Entities
{
    public class RTmsAlert
    {
        public virtual string Id { get; set; }
        public virtual string EqpId { get; set; }
        public virtual string AlarmStatus { get; set; }
        public virtual DateTime AlarmTime { get; set; }
        public virtual Decimal AlarmValue { get; set; }
        public virtual DateTime? RecoverTime { get; set; }
        public virtual string Comment { get; set; }
        public virtual string CommentBy { get; set; }
        public virtual DateTime CommentTime { get; set; }
        public virtual string IsSendAlarm { get; set; }
        public virtual string IsSendRecover { get; set; }
        public virtual string UpdateBy { get; set; }
        public virtual DateTime UpdateTime { get; set; }
    }
}