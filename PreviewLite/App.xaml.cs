using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace PreviewLite
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        void App_Startup(object sender, StartupEventArgs e)
        {
            string image_name = "";
            for (int i = 0; i != e.Args.Length; ++i)
            {
                // Check if command line argument is a valid uri
                Uri result;
                if (Uri.TryCreate(e.Args[i], UriKind.Absolute, out result)
                    && result.Scheme == Uri.UriSchemeFile)
                {
                    image_name = e.Args[i];
                }
            }

            object mainWindow;
            if (image_name != "")
            {
                mainWindow = new MainWindow(image_name);
            } else
            {
                mainWindow = new MainWindow();
            }
            
            ((MainWindow)mainWindow).Show();
        }
    }
}
