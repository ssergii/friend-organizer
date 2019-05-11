using Autofac;
using FriendOrganizer.UI.Startup;
using System;
using System.Windows;
using System.Windows.Threading;

namespace FriendOrganizer.UI
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            var bootstrapper = new Bootstrapper();
            var container = bootstrapper.Bootstrap();

            var mainWindow = container.Resolve<MainWindow>();

            mainWindow.Show();
        }

        private void Application_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            MessageBox.Show("Unexepted error occured. Please information the admin." +
                Environment.NewLine + e.Exception.Message, "Unexepted error");

            e.Handled = true;
        }
    }
}
