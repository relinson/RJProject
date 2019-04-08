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

namespace PRO_ReceiptsInvMgr.Client
{
    /// <summary>
    /// Interaction logic for TabWindow.xaml
    /// </summary>
    public partial class AppManager : BaseWindow
    {
        public Action GetAppAction { get; set; }
        Button SelectButton;

        public AppManager()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            InitMenu();
          
            Button btn = sender as Button;
            SelectButton = btn;
            setMenuImage(btn);
            foreach (var item in grdContent.Children)
            {
                if (item is Frame)
                {
                    var o = item as Frame;
                    if (o.Name == btn.Tag.ToString())
                    {
                        o.Visibility = Visibility.Visible;
                    }
                    else
                    {
                        o.Visibility = Visibility.Hidden;
                    }
                }

            }
        }

        private void setMenuImage(Button btn, ImageSource imageSource = null)
        {
            Border b = btn.Template.FindName("ContentContainer", btn) as Border;

            if (imageSource == null)
            {
                if (btn.Name == btnApp.Name)
                {
                    imageSource = new BitmapImage(new Uri(PRO_ReceiptsInvMgr.Resources.Common.IcoAppDownHover));
                }
                b.BorderThickness = new Thickness(0, 0, 0, 5);
                b.BorderBrush = new SolidColorBrush(Color.FromRgb(0x06, 0xAB, 0xE8));
            }
            else
            {
                b.BorderThickness = new Thickness(0, 0, 0, 5);
                b.BorderBrush = new SolidColorBrush();
            }

            b.Background = new ImageBrush
            {
                ImageSource = imageSource
            };
        }

        private void InitMenu()
        {
            setMenuImage(btnApp, new BitmapImage(new Uri(PRO_ReceiptsInvMgr.Resources.Common.IcoAppDown)));
        }

        private void setBorderThickness(Button btn)
        {
            if (btn != SelectButton)
            {
                Border b = btn.Template.FindName("ContentContainer", btn) as Border;
                b.BorderThickness = new Thickness(0, 0, 0, 5);
                b.BorderBrush = new SolidColorBrush();
            } 
        }
         
        private void Button_MouseEnter(object sender, MouseEventArgs e)
        {
            foreach (var item in stpBtnContent.Children)
            {
                if (item is Button)
                {
                    setBorderThickness(item as Button);
                }
            }

            Button btn = sender as Button;
            Border b = btn.Template.FindName("ContentContainer", btn) as Border;
            b.BorderThickness = new Thickness(0, 0, 0, 5);
            b.BorderBrush = new SolidColorBrush(Color.FromRgb(0x06, 0xAB, 0xE8));
        }

        private void Button_MouseLeave(object sender, MouseEventArgs e)
        {
            foreach (var item in stpBtnContent.Children)
            {
                if (item is Button)
                {
                    setBorderThickness(item as Button);
                }
            }
        }

        AppDownload downPage = new AppDownload();

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            downPage.GetAppAction = GetAppAction;
            Tab1.NavigationService.Navigate(downPage);
         

            btnApp.Focus();
            Button_Click(btnApp, null);
        }

        private void BaseWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (downPage.DicMap.Any())
            {
                bool? isClose= MessageBoxEx.Show(this,Message.ExitDownTask, PRO_ReceiptsInvMgr.Resources.Message.Tips, MessageBoxExButtons.YesNo, MessageBoxExIcon.Information);
                if (isClose.HasValue && isClose.Value)
                {
                    if (downPage.DicMap.Any())
                    {
                        foreach (var appThread in downPage.DicMap)
                        {
                            appThread.Value.Abort();
                        }
                    }

                }
                else
                {
                    e.Cancel = true;
                }
            }
            
        }
    }
}
