using PRO_ReceiptsInvMgr.Application;
using PRO_ReceiptsInvMgr.Client.Resources.xskin;
using PRO_ReceiptsInvMgr.Application.ViewModel;
using PRO_ReceiptsInvMgr.Core.Helper;
using PRO_ReceiptsInvMgr.Domain.DataObjects;
using PRO_ReceiptsInvMgr.Domain.Enum;
using PRO_ReceiptsInvMgr.Resources;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.IO;
using PRO_ReceiptsInvMgr.Application.Global;
using System.Windows.Forms;
using PRO_ReceiptsInvMgr.Client.UI.JXGL.Print;
using System.Reflection;
using System.Data;

namespace PRO_ReceiptsInvMgr.Client.UI.JXGL
{
    /// <summary>
    /// Interaction logic for JXRzqd.xaml
    /// </summary>
    public partial class JXRzqd : Page
    {
        public RzqdViewModel RzqdViewModelInstance { get; set; }
        List<JXInvoiceInfo> invoiceList = new List<JXInvoiceInfo>();
        JXManagerService service = new JXManagerService();
        DateTime dtSkssq;

        public JXRzqd()
        {
            InitializeComponent();
        }

        private void btnQuery_Click(object sender, RoutedEventArgs e)
        {
            if (!RzqdViewModelInstance.QueryModel.Skssq.HasValue)
            {
                MessageBoxEx.Show(JXManager.JXManagerInstance, PRO_ReceiptsInvMgr.Resources.Message.SelectSkssq, PRO_ReceiptsInvMgr.Resources.Message.Tips, MessageBoxExButtons.OK, MessageBoxExIcon.Error);
                return;
            }

            dtSkssq = RzqdViewModelInstance.QueryModel.Skssq.Value;
            imgTip.Visibility = Visibility.Collapsed;
            pcPage.Visibility = Visibility.Collapsed;
            pcPage.CrrentPage = 1; //每次查询时返回第一页
            gifLoading.Visibility = Visibility.Visible;
            RzqdViewModelInstance.InvoiceList = new ObservableCollection<JXInvoiceInfo>();
            Task.Factory.StartNew(() =>
            {
                string msg = string.Empty;
                int totalCount = 0;
             
                invoiceList = service.GetRzqdData(RzqdViewModelInstance.QueryModel, out totalCount, out msg);
                 
                RzqdViewModelInstance.ListCount = totalCount;

                RzqdViewModelInstance.InvoiceList = new ObservableCollection<JXInvoiceInfo>(invoiceList);

                this.Dispatcher.BeginInvoke(new Action(() =>
                {
                    pcPage.TotalCount = totalCount;

                    if (!string.IsNullOrEmpty(msg))
                    {
                        MessageBoxEx.Show(JXManager.JXManagerInstance, msg, PRO_ReceiptsInvMgr.Resources.Message.Tips, MessageBoxExButtons.OK, MessageBoxExIcon.Error);
                    }

                    gifLoading.Visibility = Visibility.Collapsed;
                    if (invoiceList.Count > 0)
                    {
                        var totalAmount = RzqdViewModelInstance.InvoiceList.Sum(x => x.HJBHSJE).Value;
                        var totalSe = RzqdViewModelInstance.InvoiceList.Sum(x => x.SE).Value;
                        RzqdViewModelInstance.TotalAmount = totalAmount.ToString("f2");
                        RzqdViewModelInstance.TotalSE = totalSe.ToString("f2");

                        pcPage.Visibility = Visibility.Visible;
                        pcPage_DataSourcePageSize(null, null);
                    }
                    else
                    {
                        RzqdViewModelInstance.TotalAmount = string.Empty;
                        RzqdViewModelInstance.TotalSE = string.Empty;
                        imgTip.Visibility = Visibility.Visible;
                    }
                }));
            });
        }
        private void pcPage_DataSourcePageSize(object sender, EventArgs e)
        {
            var list = invoiceList.Skip((pcPage.CrrentPage - 1) * pcPage.PageSize).Take(pcPage.PageSize).ToList();
            RzqdViewModelInstance.InvoiceList = new ObservableCollection<JXInvoiceInfo>(list);
        }

        private void RZQDPage_Loaded(object sender, RoutedEventArgs e)
        {
            imgTip.Visibility = Visibility.Visible;

            RzqdViewModelInstance = new RzqdViewModel();
            RzqdViewModelInstance.QueryModel = new RzqdQueryViewModel();
            this.DataContext = RzqdViewModelInstance;
        }

        private bool Valid()
        {
            if (!RzqdViewModelInstance.QueryModel.Skssq.HasValue)
            {
                MessageBoxEx.Show(JXManager.JXManagerInstance, PRO_ReceiptsInvMgr.Resources.Message.SelectSkssq, PRO_ReceiptsInvMgr.Resources.Message.Tips, MessageBoxExButtons.OK, MessageBoxExIcon.Error);
                return false;
            }
            if (invoiceList.Count == 0)
            {
                MessageBoxEx.Show(JXManager.JXManagerInstance, PRO_ReceiptsInvMgr.Resources.Message.NotExistExportData, PRO_ReceiptsInvMgr.Resources.Message.Tips, MessageBoxExButtons.OK, MessageBoxExIcon.Error);
                return false;
            }
            return true;
        }

        private void btnExport_Click(object sender, RoutedEventArgs e)
        {
            if (!Valid())
            {
                return;
            }

            try
            {
                PrintRzjg rzjgObj = ToPrintData();
                if (Export(rzjgObj))
                {
                    MessageBoxEx.Show(JXManager.JXManagerInstance, PRO_ReceiptsInvMgr.Resources.Message.ExportSuccess, PRO_ReceiptsInvMgr.Resources.Message.Tips, MessageBoxExButtons.OK, MessageBoxExIcon.Information);
                }
            }
            catch(Exception ex)
            {
                Logging.Log4NetHelper.Error(this, PRO_ReceiptsInvMgr.Resources.Message.ExportRzqdFail, ex);
                MessageBoxEx.Show(JXManager.JXManagerInstance, PRO_ReceiptsInvMgr.Resources.Message.ExportRzqdFail, PRO_ReceiptsInvMgr.Resources.Message.Tips, MessageBoxExButtons.OK, MessageBoxExIcon.Error);
            }
        }

        private PrintRzjg ToPrintData()
        {
            PrintRzjg rzjgObj = new PrintRzjg();
            rzjgObj.Nsrsbh = GlobalInfo.NSRSBH;
            rzjgObj.Qymc = GlobalInfo.NSRMC;
            rzjgObj.Skssq = RzqdViewModelInstance.QueryModel.Skssq.Value.ToString("yyyy年MM月");
            rzjgObj.NowDate = DateTime.Now;
            List<ExportRzjg> rzjgList = new List<ExportRzjg>();
            foreach (var item in invoiceList)
            {
                ExportRzjg rzjgData = new ExportRzjg();
                rzjgData.Xh = invoiceList.IndexOf(item) + 1;
                rzjgData.Fpdm = item.InvoiceCode;
                rzjgData.Fphm = item.InvoiceNo;
                rzjgData.Kprq = item.InvoiceDate;
                rzjgData.Xsfmc = item.XSFMC;
                rzjgData.Je = item.HJBHSJE.HasValue ? item.HJBHSJE.Value : 0;
                rzjgData.Se = item.SE.HasValue ? item.SE.Value : 0;
                rzjgData.Slv = Math.Round((rzjgData.Se / rzjgData.Je * 100)).ToString() + "%";
                rzjgData.Rzjg = "认证相符";
                rzjgData.Fplx = "增值税专票";

                rzjgList.Add(rzjgData);
            }
            var ls = rzjgList.GroupBy(x => x.Slv).Select(x => new ExportSlType { Slv = x.Key, count = x.Count(), Je = x.Sum(g => g.Je), Se = x.Sum(g => g.Se) }).ToList();
            ExportSlType calcData = new ExportSlType();
            calcData.Slv = "总计";
            calcData.count = ls.Sum(x => x.count);
            calcData.Je = ls.Sum(x => x.Je);
            calcData.Se = ls.Sum(x => x.Se);
            ls.Add(calcData);

            rzjgObj.ExportRzjgList = rzjgList;
            rzjgObj.ExportSlTypeList = ls;
            rzjgObj.JgContent = string.Format(PRO_ReceiptsInvMgr.Resources.Message.ExportJXJgContent,
                RzqdViewModelInstance.QueryModel.Skssq.Value.ToString("yyyy年MM月"), invoiceList.Count, invoiceList.Count, invoiceList.Sum(x => x.SE));

            return rzjgObj;
        }


        public bool Export(PrintRzjg rzjgObj)
        {
            string filePath = string.Empty;

            SaveFileDialog saveFileDlg = new SaveFileDialog();
            saveFileDlg.Filter = "Excel files(*.xlsx)|*.xlsx|All files(*.*)|*.*";
            if (saveFileDlg.ShowDialog() == DialogResult.OK)
            {
                filePath = saveFileDlg.FileName;
            }
            else
            {
                return false;
            }


            try
            {
                Dictionary<string, DataTable> dic = new Dictionary<string, DataTable>(StringComparer.OrdinalIgnoreCase);
                var tempStream = Assembly.GetEntryAssembly().GetManifestResourceStream("PRO_ReceiptsInvMgr.Client.认证清单格式.xlsx");

                rzjgObj.Skssq = string.Format("税款所属期：{0}", rzjgObj.Skssq);
                rzjgObj.Qymc = string.Format("企业名称：{0}", rzjgObj.Qymc);
                rzjgObj.Nsrsbh = string.Format("纳税人识别号：{0}", rzjgObj.Nsrsbh);
               
                DataTable dt = new List<PrintRzjg>{ rzjgObj }.ToDataTable("T");
                DataTable dt1 = rzjgObj.ExportRzjgList.ToDataTable("T1");
                DataTable dt2 = rzjgObj.ExportSlTypeList.ToDataTable("T2");
                dic.Add(dt.TableName,dt);
                dic.Add(dt1.TableName, dt1);
                dic.Add(dt2.TableName, dt2);

                ExcelHelper.ExportExcel(tempStream, filePath, dic);
            }
            catch (Exception ex)
            {
                Logging.Log4NetHelper.Error(this, "Export  Error", ex);
                return false;
            }
            return true;
        }
         
        private void btnPrint_Click(object sender, RoutedEventArgs e)
        {
            if (!Valid())
            {
                return;
            }

            try
            {
                PrintRzjg rzjgObj = ToPrintData();
                PrintPreviewWindow previewWnd = new PrintPreviewWindow("/UI/JXGL/Print/Document.xaml", rzjgObj, new DocumentRenderer());
                previewWnd.Owner = JXManager.JXManagerInstance;
                previewWnd.ShowInTaskbar = false;
                previewWnd.ShowDialog();
            }
            catch (Exception ex)
            {
                Logging.Log4NetHelper.Error(this, PRO_ReceiptsInvMgr.Resources.Message.PrintRzqdError, ex);
                MessageBoxEx.Show(JXManager.JXManagerInstance, PRO_ReceiptsInvMgr.Resources.Message.PrintRzqdError, PRO_ReceiptsInvMgr.Resources.Message.Tips, MessageBoxExButtons.OK, MessageBoxExIcon.Error);
            }

        }

        private void DateSkssq_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            btnQuery_Click(null,null);
        }
    }

}
