using MicronTMS.DLL.Entities;
using FluentNHibernate.Mapping;

namespace MicronTMS.DLL.Mappings
{
    public class HourlyMap : ClassMap<Hourly>
    {
        public HourlyMap()
        {
            Table("Fridge");
            Id(x => x.SignalIndex).Column("Signal_Index").Not.Nullable();
            Map(x => x.TDate1).Column("Sample_TDate_1");
            Map(x => x.Value1).Column("Sample_Value_1");
            Map(x => x.TDate2).Column("Sample_TDate_2");
            Map(x => x.Value2).Column("Sample_Value_2");
            Map(x => x.TDate3).Column("Sample_TDate_3");
            Map(x => x.Value3).Column("Sample_Value_3");
            Map(x => x.TDate4).Column("Sample_TDate_4");
            Map(x => x.Value4).Column("Sample_Value_4");
            Map(x => x.TDate5).Column("Sample_TDate_5");
            Map(x => x.Value5).Column("Sample_Value_5");
            Map(x => x.TDate6).Column("Sample_TDate_6");
            Map(x => x.Value6).Column("Sample_Value_6");
            Map(x => x.TDate7).Column("Sample_TDate_7");
            Map(x => x.Value7).Column("Sample_Value_7");
            Map(x => x.TDate8).Column("Sample_TDate_8");
            Map(x => x.Value8).Column("Sample_Value_8");
            Map(x => x.TDate9).Column("Sample_TDate_9");
            Map(x => x.Value9).Column("Sample_Value_9");
            Map(x => x.TDate10).Column("Sample_TDate_10");
            Map(x => x.Value10).Column("Sample_Value_10");
            Map(x => x.TDate11).Column("Sample_TDate_11");
            Map(x => x.Value11).Column("Sample_Value_11");
            Map(x => x.TDate12).Column("Sample_TDate_12");
            Map(x => x.Value12).Column("Sample_Value_12");
            Map(x => x.TDate13).Column("Sample_TDate_13");
            Map(x => x.Value13).Column("Sample_Value_13");
            Map(x => x.TDate14).Column("Sample_TDate_14");
            Map(x => x.Value14).Column("Sample_Value_14");
            Map(x => x.TDate15).Column("Sample_TDate_15");
            Map(x => x.Value15).Column("Sample_Value_15");
            Map(x => x.TDate16).Column("Sample_TDate_16");
            Map(x => x.Value16).Column("Sample_Value_16");
            Map(x => x.TDate17).Column("Sample_TDate_17");
            Map(x => x.Value17).Column("Sample_Value_17");
            Map(x => x.TDate18).Column("Sample_TDate_18");
            Map(x => x.Value18).Column("Sample_Value_18");
            Map(x => x.TDate19).Column("Sample_TDate_19");
            Map(x => x.Value19).Column("Sample_Value_19");
            Map(x => x.TDate20).Column("Sample_TDate_20");
            Map(x => x.Value20).Column("Sample_Value_20");
            Map(x => x.TDate21).Column("Sample_TDate_21");
            Map(x => x.Value21).Column("Sample_Value_21");
            Map(x => x.TDate22).Column("Sample_TDate_22");
            Map(x => x.Value22).Column("Sample_Value_22");
            Map(x => x.TDate23).Column("Sample_TDate_23");
            Map(x => x.Value23).Column("Sample_Value_23");
            Map(x => x.TDate24).Column("Sample_TDate_24");
            Map(x => x.Value24).Column("Sample_Value_24");
            Map(x => x.TDate25).Column("Sample_TDate_25");
            Map(x => x.Value25).Column("Sample_Value_25");
            Map(x => x.TDate26).Column("Sample_TDate_26");
            Map(x => x.Value26).Column("Sample_Value_26");
            Map(x => x.TDate27).Column("Sample_TDate_27");
            Map(x => x.Value27).Column("Sample_Value_27");
            Map(x => x.TDate28).Column("Sample_TDate_28");
            Map(x => x.Value28).Column("Sample_Value_28");
            Map(x => x.TDate29).Column("Sample_TDate_29");
            Map(x => x.Value29).Column("Sample_Value_29");
            Map(x => x.TDate30).Column("Sample_TDate_30");
            Map(x => x.Value30).Column("Sample_Value_30");
            Map(x => x.TDate31).Column("Sample_TDate_31");
            Map(x => x.Value31).Column("Sample_Value_31");
            Map(x => x.TDate32).Column("Sample_TDate_32");
            Map(x => x.Value32).Column("Sample_Value_32");
            Map(x => x.TDate33).Column("Sample_TDate_33");
            Map(x => x.Value33).Column("Sample_Value_33");
            Map(x => x.TDate34).Column("Sample_TDate_34");
            Map(x => x.Value34).Column("Sample_Value_34");
            Map(x => x.TDate35).Column("Sample_TDate_35");
            Map(x => x.Value35).Column("Sample_Value_35");
            Map(x => x.TDate36).Column("Sample_TDate_36");
            Map(x => x.Value36).Column("Sample_Value_36");
        }
    }
}