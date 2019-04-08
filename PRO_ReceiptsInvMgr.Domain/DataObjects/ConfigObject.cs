using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PRO_ReceiptsInvMgr.Domain.DataObjects
{
    public class ConfigObject: BaseDataObject
    {
        public int ID { get; set; }
    
        public string NSRSBH { get; set; }

        public string NSRMC { get; set; }

        public string DQDM { get; set; }

        public string LoginPwd { get; set; }

        public string CertPwd { get; set; }

        public int? TaxType { get; set; }

        public bool? IsRemember { get; set; }
    }
}
