using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;

namespace PRO_ReceiptsInvMgr.UpdateApp
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            if (e.Args.Length == 1)
            {
                Decompression de = new Decompression();
                de.localFileName = e.Args[0];
                de.Show();
            }
            else
            {
                Log4NetHelper.Error(typeof(App), "客户端更新异常：" + e.Args);
            }
        }
    }
}
