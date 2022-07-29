using System;

namespace MicronTMS.DLL.Entities
{
    public class CTmsEqpConfig
    {
        public virtual string EqpId { get; set; }
        public virtual string EqpName { get; set; }
        public virtual Decimal HIHI { get; set; }
        public virtual Decimal HI { get; set; }
        public virtual Decimal LOLO { get; set; }
        public virtual Decimal LO { get; set; }
        public virtual Decimal? Offset { get; set; }
        public virtual string UpdateBy { get; set; }
        public virtual DateTime UpdateTime { get; set; }
    }
}