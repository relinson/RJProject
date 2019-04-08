using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRO_ReceiptsInvMgr.Domain.DataObjects
{
    public class PrintRzjg
    {
        public string Skssq { get; set; }

        public string Qymc { get; set; }

        public string Nsrsbh { get; set; }

        public DateTime NowDate { get; set; }

        public List<ExportRzjg> ExportRzjgList
        {
            get;
            set;
        }
        public List<ExportSlType> ExportSlTypeList
        {
            get;
            set;
        }

        public string JgContent { get; set; }

    }


    public class ExportRzjg
    {
        public int Xh { get; set; }

        public string Fpdm { get; set; }

        public string Fphm { get; set; }

        public DateTime Kprq { get; set; }

        public string Xsfmc { get; set; }

        public double Je { get; set; }

        public double Se { get; set; }

        public string Slv { get; set; }

        public string Rzjg { get; set; }

        public string Fplx { get; set; }

    }

    public class ExportSlType
    {
        public string Slv { get; set; }

        public int count { get; set; }

        public double Je { get; set; }

        public double Se { get; set; }
    }


}
