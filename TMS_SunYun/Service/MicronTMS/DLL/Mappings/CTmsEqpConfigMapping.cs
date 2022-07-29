using MicronTMS.DLL.Entities;
using FluentNHibernate.Mapping;

namespace MicronTMS.DLL.Mappings
{
    public class CTmsEqpConfigMapping : ClassMap<CTmsEqpConfig>
    {
        public CTmsEqpConfigMapping()
        {
            Table("C_TMS_EQPCONFIG");
            Id(x => x.EqpId).Column("EQP_ID").Not.Nullable();
            Map(x => x.EqpName).Column("EQP_NAME");
            Map(x => x.HIHI).Column("HIHI_LIMIT");
            Map(x => x.HI).Column("HI_LIMIT");
            Map(x => x.LOLO).Column("LOLO_LIMIT");
            Map(x => x.LO).Column("LO_LIMIT");
            Map(x => x.Offset).Column("OFFSET");
            Map(x => x.UpdateBy).Column("UPDATE_BY");
            Map(x => x.UpdateTime).Column("UPDATE_TIME");
        }
    }
}