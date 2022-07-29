using MicronTMS.DLL.Entities;
using FluentNHibernate.Mapping;

namespace MicronTMS.DataAccess.Mappings
{
    public class HTeleMessageMap : ClassMap<HTeleMessage>
    {
        public HTeleMessageMap()
        {
            Table("H_TELEGRAM_MESSAGE");
            Id(x => x.EventId).Column("EVENT_ID").Not.Nullable();
            Map(x => x.ReportTime).Column("REPORT_TIME").Not.Nullable();
            Map(x => x.ChatGroup ).Column("CHAT_GROUP");
            Map(x => x.ChatId).Column("CHAT_ID");
            Map(x => x.MessageBody).Column("MESSAGE_BODY");
            Map(x => x.MessageCode).Column("MESSAGE_CODE");
            Map(x => x.LmTime).Column("LM_TIME");
            Map(x => x.LmUser).Column("LM_USER");
        }
    }
}
