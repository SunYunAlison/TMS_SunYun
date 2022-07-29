using MicronTMS.DLL.Entities;
using FluentNHibernate.Mapping;

namespace MicronTMS.DLL.Mappings
{
    public class HUserInfoMap : ClassMap<HUserInfo>
    {
        public HUserInfoMap()
        {
            Table("H_USER_INFO");
            CompositeId()
              .KeyProperty(x => x.EventId, "EVENT_ID")
              .KeyProperty(x => x.UserId, "USER_INFO");
            Map(x => x.TrxName).Column("TRX_NAME");
            Map(x => x.UserName).Column("USER_NAME");
            Map(x => x.Email).Column("EMAIL");
            Map(x => x.ContactNo).Column("CONTACT_NO");
            Map(x => x.LmTime).Column("LM_TIME");
            Map(x => x.LmUser).Column("LM_USER");
        }
    }
}