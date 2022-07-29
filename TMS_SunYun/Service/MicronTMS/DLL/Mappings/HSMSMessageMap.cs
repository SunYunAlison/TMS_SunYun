using MicronTMS.DLL.Entities;
using FluentNHibernate.Mapping;

namespace MicronTMS.DataAccess.Mappings
{
    public class HSMSMessageMap : ClassMap<HSMSMessage>
    {
        public HSMSMessageMap()
        {
            Table("H_SMS_MESSAGE");
            Id(x => x.EventId).Column("EVENT_ID").Not.Nullable();
            Map(x => x.ReportTime).Column("REPORT_TIME").Not.Nullable();
            Map(x => x.PhoneNo).Column("PHONE_NO");
            Map(x => x.MessageBody).Column("MESSAGE_BODY");
            Map(x => x.MessageCode).Column("MESSAGE_CODE");
            Map(x => x.LmTime).Column("LM_TIME");
            Map(x => x.LmUser).Column("LM_USER");
        }
    }
}
