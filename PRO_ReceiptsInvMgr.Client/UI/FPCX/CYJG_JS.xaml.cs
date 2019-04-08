﻿using PRO_ReceiptsInvMgr.Application;
using PRO_ReceiptsInvMgr.Application.Global;
using PRO_ReceiptsInvMgr.Client.Resources.xskin;
using PRO_ReceiptsInvMgr.Core.Helper;
using PRO_ReceiptsInvMgr.Domain.DataObjects;
using PRO_ReceiptsInvMgr.Domain.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Script.Serialization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace PRO_ReceiptsInvMgr.Client.UI.FPCX
{
    /// <summary>
    /// Interaction logic for CYJG_01.xaml
    /// </summary>
    public partial class CYJG_JS : BaseWindow
    { 

        public FpcyResponse InvoiceData { get; set; }
        private string cyDate;

        public string CyDate
        {
            get
            {
                return cyDate;
            }

            set
            {
                cyDate = value;
                OnPropertyChanged("CyDate");
            }
        }

        


        public CYJG_JS()
        {
            InitializeComponent(); 
        }

        private void cyWindow_Loaded(object sender, RoutedEventArgs e)
        {

            if (InvoiceData != null)
            {
                var invoiceInfo = InvoiceData.invoiceList[0].invoiceInfo;
                if (invoiceInfo.cancellationMark == "Y")
                {
                    lblZF.Visibility = Visibility.Visible;
                }

                invoiceInfo.DXtotalAmount = CommonHelper.MoneyToUpper(invoiceInfo.totalAmount.ToString());
                this.DataContext = invoiceInfo;

                grdGoods.ItemsSource = invoiceInfo.detailList;
            }
        }
    }
 
}
