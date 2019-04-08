using Microsoft.Win32;
using PRO_ReceiptsInvMgr.Application;
using PRO_ReceiptsInvMgr.Application.Global;
using PRO_ReceiptsInvMgr.Client.Resources.xskin;
using PRO_ReceiptsInvMgr.Core.Helper;
using PRO_ReceiptsInvMgr.Domain.DataObjects;
using PRO_ReceiptsInvMgr.Domain.Enum;
using PRO_ReceiptsInvMgr.Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace PRO_ReceiptsInvMgr.Client.UI
{
    /// <summary>
    /// Interaction logic for Register.xaml
    /// </summary>
    public partial class QA : BaseWindow
    {
        QaService qaService = new QaService();

        public QA()
        {
            InitializeComponent();
        }
        private List<TQa> _qaList = new List<TQa>();
        public List<TQa> QaList
        {
            get
            {
                return _qaList;
            }
            set
            {
                _qaList = value;

                //触发事件
                OnPropertyChanged("QaList");
            }
        }

        
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var result = false;
            Task.Factory.StartNew(() => {
                QaList = qaService.GetQaList(ref result);
                if (!result)
                {
                    this.Dispatcher.BeginInvoke(new Action(() =>
                    {
                        scrollViewer1.Visibility = Visibility.Hidden;
                        imgTip.Visibility = Visibility.Visible;
                    }));
               
                }
            });
        }

       
        private void ScrollViewer_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            var eventArg = new MouseWheelEventArgs(e.MouseDevice, e.Timestamp, e.Delta);
            eventArg.RoutedEvent = UIElement.MouseWheelEvent;
            eventArg.Source = sender;
            scrollViewer1.RaiseEvent(eventArg);
        }
    }

    

}
