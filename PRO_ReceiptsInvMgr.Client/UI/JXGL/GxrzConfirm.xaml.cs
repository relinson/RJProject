using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;
using PRO_ReceiptsInvMgr.Client.Resources.xskin;
using PRO_ReceiptsInvMgr.Domain.DataObjects;

namespace PRO_ReceiptsInvMgr.Client.UI.JXGL
{
    /// <summary>
    /// Interaction logic for MessageBoxEx.xaml
    /// </summary>
    public partial class GxrzConfirm : Window
    {
        public string ConfirmTip { get; set; }
        public Action DoRzAction { get; set; }

        public List<JXInvoiceInfo> SelectedList { get; set; }

        public GxrzConfirm()
        {
            InitializeComponent();
        }

        private void btnYes_Click(object sender, RoutedEventArgs e)
        {
            if (DoRzAction != null)
            {
                this.Close();
                DoRzAction();
            }
        }

        private void btnNo_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void JxRzConfirmPage_Loaded(object sender, RoutedEventArgs e)
        {
            grdList.ItemsSource = SelectedList;
            msgBlock.Text = ConfirmTip;
        }
    }

}

