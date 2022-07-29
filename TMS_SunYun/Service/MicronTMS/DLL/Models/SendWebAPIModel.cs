using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MicronTMS.DLL.Models
{
    public class SendWebAPIModel
    {
        public virtual string corp_id { get; set; }
        public virtual string api_key { get; set; }
        public virtual string jobCat { get; set; }
        public virtual string scene { get; set; }
        public virtual string tag { get; set; }
        public virtual string desc { get; set; }
        public virtual string picUrl { get; set; }

        public virtual string userId { get; set; }
    }
}