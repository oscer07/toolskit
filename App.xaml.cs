using System.Windows;
using System.IO;

namespace ToolkitApp
{
    public partial class App : Application
    {
        public App()
        {
            this.DispatcherUnhandledException += (s, e) => 
            {
                File.WriteAllText("crash.txt", e.Exception.ToString());
                MessageBox.Show(e.Exception.Message, "Crash", MessageBoxButton.OK, MessageBoxImage.Error);
                e.Handled = true;
            };
            System.AppDomain.CurrentDomain.UnhandledException += (s, e) =>
            {
                File.WriteAllText("crash_domain.txt", e.ExceptionObject.ToString());
            };
        }
    }
}
