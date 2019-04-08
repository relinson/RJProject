using PRO_ReceiptsInvMgr.Domain.DataObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PRO_ReceiptsInvMgr.Domain
{
    public class AppInfoObject: BaseDataObject
    {
        public AppInfoObject() { }
        public int ID { get; set; }
        public string AppName { get; set; }
        public string AppDescription { get; set; }
        public string Ico { get; set; }
        public string ExePath { get; set; }
        public bool Status { get; set; }
        public string FileDirectory { get; set; }
        public string AppCode { get; set; }
        public string DownUrl { get; set; }
        public bool isExist { get; set; }
        public string btnImgUrl { get; set;}
        public string MD5 { get; set; }
        public bool Enable { get; set; }
        public string AppVersion { get; set; }
        public bool IsUpdate { get; set; }
    }
}
