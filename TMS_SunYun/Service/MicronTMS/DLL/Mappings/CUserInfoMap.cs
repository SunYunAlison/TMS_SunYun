using MicronTMS.DLL.Entities;
using FluentNHibernate.Mapping;

namespace MicronTMS.DLL.Mappings
{
    public class CUserInfoMap : ClassMap<CUserInfo>
    {
        public CUserInfoMap()
        {
            Table("C_USER_INFO");
            Id(x => x.UserId).Column("USER_ID").Not.Nullable();
            Map(x => x.UserName).Column("USER_NAME");
            Map(x => x.Email).Column("EMAIL");
            Map(x => x.ContactNo).Column("CONTACT_NO");
            Map(x => x.LmTime).Column("LM_TIME");
            Map(x => x.LmUser).Column("LM_USER");
        }
    }
}