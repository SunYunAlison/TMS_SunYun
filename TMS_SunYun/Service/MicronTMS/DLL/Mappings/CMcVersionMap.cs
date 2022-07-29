using MicronTMS.DLL.Entities;
using FluentNHibernate.Mapping;

namespace MicronTMS.DataAccess.Mappings
{
    public class CMcVersionMap : ClassMap<CMcVersion>
    {
        public CMcVersionMap()
        {
            Table("C_CMC_VERSION");
            Id(x => x.Id).Column("ID").Not.Nullable();
            Map(x => x.VersionNumber).Column("VERSION_NUMBER");
            Map(x => x.LmTime).Column("LM_TIME");
            Map(x => x.LmUser).Column("LM_USER");
        }
    }
}

