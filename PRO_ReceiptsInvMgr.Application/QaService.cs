using PRO_ReceiptsInvMgr.Application.Global;
using PRO_ReceiptsInvMgr.BLL;
using PRO_ReceiptsInvMgr.Domain.DataObjects;
using PRO_ReceiptsInvMgr.Domain.Enum;
using PRO_ReceiptsInvMgr.Domain.IBLL;
using PRO_ReceiptsInvMgr.Model;
using PRO_ReceiptsInvMgr.Model.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Script.Serialization;

namespace PRO_ReceiptsInvMgr.Application
{
    public class QaService
    {
      
        public List<TQa> GetQaList(ref bool result)
        {
            List<TQa> QaList = new List<TQa>();
            string requestJson = new JavaScriptSerializer().Serialize(new { NSRSBH = GlobalInfo.NSRSBH, FJH = GlobalInfo.FJH });
            string errorMsg = string.Empty;
            string content = WSInterface.GetResponse(requestJson, InterfaceType.Lypl, ref result,out errorMsg);
            if (result)
            {
                QAInfo qaInfo = new JavaScriptSerializer().Deserialize<QAInfo>(content);
                foreach (var item in qaInfo.OWNISSUES)
                {
                    QaList.Add(new TQa { Question = item.Question, Answer = item.RESPONSES.Any() ? item.RESPONSES.FirstOrDefault().RESPONSE : string.Empty });
                }

                foreach (var item in qaInfo.HOTISSUES)
                {
                    QaList.Add(new TQa { Question = item.Question, Answer = item.RESPONSES.Any() ? item.RESPONSES.FirstOrDefault().RESPONSE : string.Empty });
                }
            }
            return QaList;
        }
    }

    public class TQa
    {
        public string Question { get; set; }
        public string Answer { get; set; }
    }
}
