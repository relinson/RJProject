using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Documents;

namespace PRO_ReceiptsInvMgr.Client.UI.JXGL.Print
{
    public interface IDocumentRenderer
    {
        void Render(FlowDocument doc, Object data);
    }
}
