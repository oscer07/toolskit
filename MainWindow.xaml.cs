using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace ToolkitApp
{
    public partial class MainWindow : Window
    {
        private Views.HomeView homeView;
        private Views.ToolsView toolsView;
        private Views.AboutView aboutView;
        private Views.GithubInstallView githubInstallView;

        public MainWindow()
        {
            InitializeComponent();
            homeView = new Views.HomeView();
            toolsView = new Views.ToolsView();
            aboutView = new Views.AboutView();
            githubInstallView = new Views.GithubInstallView();

            // Default view
            Nav_Home(null, null);
        }

        private void ResetNavButtons()
        {
            btnHome.Background = Brushes.Transparent;
            btnTools.Background = Brushes.Transparent;
            btnInstalledTools.Background = Brushes.Transparent;
            btnInstalledTools.Foreground = (SolidColorBrush)Application.Current.Resources["TextLightBrush"];
            btnAbout.Background = Brushes.Transparent;
        }

        private void Nav_Home(object sender, RoutedEventArgs e)
        {
            ResetNavButtons();
            btnHome.Background = (SolidColorBrush)Application.Current.Resources["AccentBrush"];
            MainContent.Content = homeView;
        }

        private void Nav_Tools(object sender, RoutedEventArgs e)
        {
            ResetNavButtons();
            btnTools.Background = (SolidColorBrush)Application.Current.Resources["AccentBrush"];
            MainContent.Content = toolsView;
            
            // Re-load config in case new tools were added via GithubInstallView
            toolsView.LoadConfiguration();
        }

        private void Nav_About(object sender, RoutedEventArgs e)
        {
            ResetNavButtons();
            btnAbout.Background = (SolidColorBrush)Application.Current.Resources["AccentBrush"];
            MainContent.Content = aboutView;
        }

        private void Nav_InstalledTools(object sender, RoutedEventArgs e)
        {
            ResetNavButtons();
            btnInstalledTools.Background = (SolidColorBrush)Application.Current.Resources["AccentBrush"];
            btnInstalledTools.Foreground = Brushes.White;
            MainContent.Content = new Views.InstalledToolsView();
        }

        private void Nav_InstallGithub(object sender, RoutedEventArgs e)
        {
            ResetNavButtons();
            MainContent.Content = githubInstallView;
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (Keyboard.Modifiers == ModifierKeys.Control && e.Key == Key.Q)
            {
                Nav_InstallGithub(null, null);
                e.Handled = true;
            }
        }
    }
}
