using MicronTMS.DLL.Entities;
using FluentNHibernate.Mapping;

namespace MicronTMS.DLL.Mappings
{
    public class RTmsAlertMapping : ClassMap<RTmsAlert>
    {
        public RTmsAlertMapping()
        {
            Table("R_TMS_ALERT");
            Id(x => x.Id).Column("ID").Not.Nullable();
            Map(x => x.EqpId).Column("EQP_ID");
            Map(x => x.AlarmStatus).Column("ALARM_STATUS");
            Map(x => x.AlarmTime).Column("ALARM_TIME");
            Map(x => x.AlarmValue).Column("ALARM_VALUE");
            Map(x => x.RecoverTime).Column("RECOVER_TIME");
            Map(x => x.Comment).Column("COMMENT");
            Map(x => x.CommentBy).Column("COMMENT_BY");
            Map(x => x.CommentTime).Column("COMMENT_TIME");
            Map(x => x.IsSendAlarm).Column("IS_SEND_ALARM");
            Map(x => x.IsSendRecover).Column("IS_SEND_RECOVER");
            Map(x => x.UpdateBy).Column("UPDATE_BY");
            Map(x => x.UpdateTime).Column("UPDATE_TIME");
        }
    }
}