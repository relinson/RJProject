using PRO_ReceiptsInvMgr.Client.Resources.xskin;
using PRO_ReceiptsInvMgr.Client.UI;
using PRO_ReceiptsInvMgr.Resources;
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

namespace PRO_ReceiptsInvMgr.Client.UI.JXGL
{
    /// <summary>
    /// Interaction logic for TabWindow.xaml
    /// </summary>
    public partial class JXManager : BaseWindow
    {
        public Action GetAppAction { get; set; }

        public static JXManager JXManagerInstance { get; set; }

        Button SelectButton;
        public JXManager()
        {
            JXManagerInstance = this;
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            InitMenu();

            Button btn = sender as Button;
            SelectButton = btn;
            SetMenuImage(btn);

            if (btn == btnGXRZ)
            {
                frame.Navigate(new JXGxgz());
            }
            else if (btn == btnYQYJ)
            {
                frame.Navigate(new JXYqyj());
            }
            else if (btn == btnRZQD)
            {
                frame.Navigate(new JXRzqd());
            }
            else if (btn == btnSMRZ)
            {
                frame.Navigate(new JXSmrz());
            }
        }

        private void SetMenuImage(Button btn, ImageSource imageSource = null)
        {
            btn.Foreground = new SolidColorBrush(Color.FromRgb(0x9b, 0xa3, 0xb0));
            btn.BorderThickness = new Thickness(0, 0, 0, 3);

            if (imageSource == null)
            {
                if (btn.Name == btnGXRZ.Name)
                {
                    imageSource = new BitmapImage(new Uri(PRO_ReceiptsInvMgr.Resources.Common.IcoJxglGxrzActive));
                }
                else if (btn.Name == btnSMRZ.Name)
                {
                    imageSource = new BitmapImage(new Uri(PRO_ReceiptsInvMgr.Resources.Common.IcoJxglSmrzActive));
                }
                else if (btn.Name == btnYQYJ.Name)
                {
                    imageSource = new BitmapImage(new Uri(PRO_ReceiptsInvMgr.Resources.Common.IcoJxglYqyjActive));
                }
                else
                {
                    imageSource = new BitmapImage(new Uri(PRO_ReceiptsInvMgr.Resources.Common.IcoJxglRzqdActive));
                }
                btn.BorderBrush = new SolidColorBrush(Color.FromRgb(0x4a, 0x82, 0xf3));
                btn.Foreground = new SolidColorBrush(Color.FromRgb(0x4a, 0x82, 0xf3));
            }
            else
            {
                btn.BorderBrush = new SolidColorBrush(Color.FromArgb(1, 0, 0, 0));
            }


            btn.Background = new ImageBrush
            {
                ImageSource = imageSource
            };
        }

        private void InitMenu()
        {
            SetMenuImage(btnGXRZ, new BitmapImage(new Uri(PRO_ReceiptsInvMgr.Resources.Common.IcoJxglGxrz)));
            SetMenuImage(btnSMRZ, new BitmapImage(new Uri(PRO_ReceiptsInvMgr.Resources.Common.IcoJxglSmrz)));
            SetMenuImage(btnYQYJ, new BitmapImage(new Uri(PRO_ReceiptsInvMgr.Resources.Common.IcoJxglYqyj)));
            SetMenuImage(btnRZQD, new BitmapImage(new Uri(PRO_ReceiptsInvMgr.Resources.Common.IcoJxglRzqd)));
        }

        private void BaseWindow_Loaded(object sender, RoutedEventArgs e)
        {
            InitMenu();
            Button_Click(btnGXRZ, null);
        }

        private void BaseWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            JXManagerInstance = null;
            MainWindow.GetMainWindowInstance.NotifyMenuItemRestore_Click(null, null);
        }
    }
}
