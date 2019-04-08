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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PRO_ReceiptsInvMgr.Client.UserControls
{
    /// <summary>
    /// 创建日期：2016-07-12
    /// 功能描述：分页自定义控件
    /// </summary>
    public partial class PageControl : UserControl
    {
        public event EventHandler DataSourcePageSize;
        private int _totalCount = 0;//总记录数
        private int _totalPageCount = 0;//总页数
        private int _pageSize = 0;//分页数
        private int _currentPage = 1;//当前页

        public PageControl()
        {
            InitializeComponent();

        }
        /// <summary>
        /// 当前页数
        /// </summary>
        public int CrrentPage
        {
            get
            {
                return _currentPage;
            }
            set
            {
                if (value <= 0)
                {
                    _currentPage = 1;
                }
                else
                {
                    _currentPage = value;
                }
            }
        }

        /// <summary>
        /// 总记录数
        /// </summary>
        public int TotalCount
        {
            get
            {
                return _totalCount;
            }
            set
            {
                _totalCount = value;
                if (value <= 0)
                {
                    SetPageInfo(0);
                }
                else
                {
                    SetPageInfo(CrrentPage);
                    SetNowPageNumberButton(CrrentPage);
                }
            }
        }

        /// <summary>
        /// 总页数
        /// </summary>
        public int TotalPageCount
        {
            get
            {
                return _totalPageCount;
            }
            set
            {
                _totalPageCount = value;
            }
        }

        /// <summary>
        /// 分页数
        /// </summary>
        public int PageSize
        {
            get
            {
                return _pageSize;
            }
            set
            {
                if (value <= 0)
                {
                    _pageSize = 10;
                }
                else
                {
                    _pageSize = value;
                }
            }
        }


        private int number1;
        private int number2;
        private int number3;
        private int number4;
        private int number5;

        public int Number1
        {
            get
            {
                return number1;
            }

            set
            {
                number1 = value;
                if (value > 0)
                {
                    PageNumber1.Content = value.ToString();
                    PageNumber1.Visibility = Visibility.Visible;
                }
                else
                {
                    PageNumber1.Visibility = Visibility.Collapsed;
                }
            }
        }

        public int Number2
        {
            get
            {
                return number2;
            }

            set
            {
                number2 = value;
                if (value > 0)
                {
                    PageNumber2.Content = value.ToString();
                    PageNumber2.Visibility = Visibility.Visible;
                }
                else
                {
                    PageNumber2.Visibility = Visibility.Collapsed;
                }
            }
        }

        public int Number3
        {
            get
            {
                return number3;
            }

            set
            {
                number3 = value;
                if (value > 0)
                {
                    PageNumber3.Content = value.ToString();
                    PageNumber3.Visibility = Visibility.Visible;
                }
                else
                {
                    PageNumber3.Visibility = Visibility.Collapsed;
                }
            }
        }

        public int Number4
        {
            get
            {
                return number4;
            }

            set
            {
                number4 = value;
                if (value > 0)
                {
                    PageNumber4.Content = value.ToString();
                    PageNumber4.Visibility = Visibility.Visible;
                }
                else
                {
                    PageNumber4.Visibility = Visibility.Collapsed;
                }
            }
        }

        public int Number5
        {
            get
            {
                return number5;
            }

            set
            {
                number5 = value;
                if (value > 0)
                {
                    PageNumber5.Content = value.ToString();
                    PageNumber5.Visibility = Visibility.Visible;
                }
                else
                {
                    PageNumber5.Visibility = Visibility.Collapsed;
                }
            }
        }

        /// <summary>
        /// 分页条信息设置
        /// </summary>
        /// <param name="currentPage"></param>
        private void SetPageInfo(int currentPage)
        {
            if (TotalCount > 0)
            {
                FrontPage.Visibility = Visibility.Visible;
                NextPage.Visibility = Visibility.Visible;
                pageNumberSPL.Visibility = Visibility.Visible;

                //总页数
                TotalPageCount = TotalCount % PageSize == 0 ? TotalCount / PageSize : TotalCount / PageSize + 1;
                //数据分页信息
                var pageCounts = TotalCount % PageSize == 0 ? PageSize : TotalCount % PageSize;
                label1.Content = string.Format("本页{0}张发票，共计{1}张发票", currentPage < TotalPageCount ? PageSize : pageCounts, TotalCount);

                if (TotalPageCount >= 5)
                {
                    Number1 = 1;
                    Number2 = 2;
                    Number3 = 3;
                    Number4 = 4;
                    Number5 = 5;

                    if (currentPage >= 3)
                    {
                        if (TotalPageCount - 2 >= currentPage)
                        {
                            Number1 = currentPage - 2;
                            Number2 = currentPage - 1;
                            Number3 = currentPage;
                            Number4 = currentPage + 1;
                            Number5 = currentPage + 2;
                        }
                        else
                        {
                            if (TotalPageCount == currentPage)
                            {
                                Number1 = currentPage - 4;
                                Number2 = currentPage - 3;
                                Number3 = currentPage - 2;
                                Number4 = currentPage - 1;
                                Number5 = currentPage;
                            }
                            else if (TotalPageCount - 1 == currentPage)
                            {
                                Number1 = currentPage - 3;
                                Number2 = currentPage - 2;
                                Number3 = currentPage - 1;
                                Number4 = currentPage;
                                Number5 = currentPage + 1;
                            }
                        }
                    }
                }
                else
                {
                    Number1 = 0;
                    Number2 = 0;
                    Number3 = 0;
                    Number4 = 0;
                    Number5 = 0;
                    if (TotalPageCount == 1)
                    {
                        Number1 = 1;
                    }
                    else if (TotalPageCount == 2)
                    {
                        Number1 = 1;
                        Number2 = 2;
                    }
                    else if (TotalPageCount == 3)
                    {
                        Number1 = 1;
                        Number2 = 2;
                        Number3 = 3;
                    }
                    else if (TotalPageCount == 4)
                    {
                        Number1 = 1;
                        Number2 = 2;
                        Number3 = 3;
                        Number4 = 4;
                    }
                }


                //上一页
                if (currentPage == 1)
                {
                    FrontPage.IsEnabled = false;
                    PreviewPage.IsEnabled = false;
                }
                else
                {
                    PreviewPage.IsEnabled = true;
                    FrontPage.IsEnabled = true;
                }

                if (TotalPageCount > currentPage)
                {
                    NextPage.IsEnabled = true;
                    LastPage.IsEnabled = true;
                }
                else
                {
                    NextPage.IsEnabled = false;
                    LastPage.IsEnabled = false;
                }

            }
            else
            {
                label1.Content = "暂无记录";
                FrontPage.Visibility = Visibility.Hidden;
                NextPage.Visibility = Visibility.Hidden;
                pageNumberSPL.Visibility = Visibility.Hidden;
            }
        }

        /// <summary>
        /// 上一页
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PreviewPage_Click(object sender, EventArgs e)
        {
            CrrentPage--;
            SetPageInfo(CrrentPage);
            SetNowPageNumberButton(CrrentPage);
            if (this.DataSourcePageSize != null)
            {
                this.DataSourcePageSize(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// 下一页
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NextPage_Click(object sender, RoutedEventArgs e)
        {
            CrrentPage++;
            SetPageInfo(CrrentPage);
            SetNowPageNumberButton(CrrentPage);
            if (this.DataSourcePageSize != null)
            {
                this.DataSourcePageSize(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// 首页
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FrontPage_Click(object sender, RoutedEventArgs e)
        {
            CrrentPage = 1;
            SetPageInfo(CrrentPage);
            SetNowPageNumberButton(CrrentPage);
            if (this.DataSourcePageSize != null)
            {
                this.DataSourcePageSize(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// 末页
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LastPage_Click(object sender, RoutedEventArgs e)
        {
            CrrentPage = TotalPageCount;
            SetPageInfo(CrrentPage);
            SetNowPageNumberButton(CrrentPage);
            if (this.DataSourcePageSize != null)
            {
                this.DataSourcePageSize(this, EventArgs.Empty);
            }
        }

        private void PageNumber_Click(object sender, RoutedEventArgs e)
        {
            var btn = sender as Button;
            if (btn != null)
            {
                CrrentPage = Convert.ToInt32(btn.Content.ToString());
                SetPageInfo(CrrentPage);
                SetNowPageNumberButton(CrrentPage);
                if (this.DataSourcePageSize != null)
                {

                    this.DataSourcePageSize(this, EventArgs.Empty);
                }

            }
        }

        /// <summary>
        /// 设置翻页页码背景
        /// </summary>
        /// <param name="CrrentPage"></param>
        private void SetNowPageNumberButton(int CurrentPage)
        {
            foreach (var item in pageNumberSPL.Children)
            {
                var btn = item as Button;
                if (btn.Content.ToString() == CurrentPage.ToString())
                {
                    PageNumber1.Background = new SolidColorBrush(Color.FromRgb(0xFF, 0xFF, 0xFF));
                    PageNumber2.Background = new SolidColorBrush(Color.FromRgb(0xFF, 0xFF, 0xFF));
                    PageNumber3.Background = new SolidColorBrush(Color.FromRgb(0xFF, 0xFF, 0xFF));
                    PageNumber4.Background = new SolidColorBrush(Color.FromRgb(0xFF, 0xFF, 0xFF));
                    PageNumber5.Background = new SolidColorBrush(Color.FromRgb(0xFF, 0xFF, 0xFF));

                    PageNumber1.Foreground = new SolidColorBrush(Color.FromRgb(0xaa, 0xaa, 0xaa));
                    PageNumber2.Foreground = new SolidColorBrush(Color.FromRgb(0xaa, 0xaa, 0xaa));
                    PageNumber3.Foreground = new SolidColorBrush(Color.FromRgb(0xaa, 0xaa, 0xaa));
                    PageNumber4.Foreground = new SolidColorBrush(Color.FromRgb(0xaa, 0xaa, 0xaa));
                    PageNumber5.Foreground = new SolidColorBrush(Color.FromRgb(0xaa, 0xaa, 0xaa));

                    btn.Background = new SolidColorBrush(Color.FromRgb(0x4b, 0xa6, 0xe6));
                    btn.Foreground = new SolidColorBrush(Color.FromRgb(0xff, 0xff, 0xff));
                }
            }

        }



    }
}

