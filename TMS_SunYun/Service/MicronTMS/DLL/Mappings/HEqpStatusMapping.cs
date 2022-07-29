using MicronTMS.DLL.Entities;
using FluentNHibernate.Mapping;

namespace MicronTMS.DLL.Mappings
{
    public class HEqpStatusMapping : ClassMap<HEqpStatus>
    {
        public HEqpStatusMapping()
        {
            Table("H_TMS_EQPSTATUS");
            Id(x => x.EqpId).Column("EQP_ID").Not.Nullable();
            Map(x => x.Status).Column("STATUS");
            Map(x => x.SubStatus).Column("SUB_STATUS");
            Map(x => x.Temp).Column("TEMPERATURE");
            Map(x => x.IsOpen).Column("IS_OPEN");
            Map(x => x.DoorOpenTime).Column("DOOR_OPEN_TIME");
            Map(x => x.UpdateBy).Column("UPDATE_BY");
            Map(x => x.UpdateTime).Column("UPDATE_TIME");
        }
    }
}