using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace PRO_ReceiptsInvMgr.Model
{
    [Table("T_Dqdm")]
    public class TDqdm : IBaseModel
    {
        public Int64 ID { get; set; }
        public string DqCode { get; set; }
        public string ProviceName { get; set; }
        public string NetLocation { get; set; }
    }
}
