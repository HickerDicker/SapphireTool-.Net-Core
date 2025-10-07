using System.Windows;
using Wpf.Ui.Appearance;
using SapphireTool.Pages;

namespace SapphireTool
{
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();
            Loaded += (_, _) =>
            {
                RootNavigation.Navigate(typeof(Page1));
                SystemThemeWatcher.Watch(this, Wpf.Ui.Controls.WindowBackdropType.Mica, true);
            };
        }

        private void componentsBtn_Click(object sender, RoutedEventArgs e)
        {
            DashboardPage.Visibility = Visibility.Collapsed;
            RootNavigation.Navigate(typeof(TweaksPage));
        }

        private void browsersBtn_Click(object sender, RoutedEventArgs e)
        {
            DashboardPage.Visibility = Visibility.Collapsed;
            RootNavigation.Navigate(typeof(BrowsersPage));
        }

        private void updatesBtn_Click(object sender, RoutedEventArgs e)
        {
            DashboardPage.Visibility = Visibility.Collapsed;
            RootNavigation.Navigate(typeof(UpdatesPage));
        }

        private void dashboardNavBtn_Click(object sender, RoutedEventArgs e) => DashboardPage.Visibility = Visibility.Visible;

        private void componentsNavBtn_Click(object sender, RoutedEventArgs e) => DashboardPage.Visibility = Visibility.Collapsed;

        private void browsersNavBtn_Click(object sender, RoutedEventArgs e) => DashboardPage.Visibility = Visibility.Collapsed;

        private void driversNavBtn_Click(object sender, RoutedEventArgs e) => DashboardPage.Visibility = Visibility.Collapsed;

        private void updatesNavBtn_Click(object sender, RoutedEventArgs e) => DashboardPage.Visibility = Visibility.Collapsed;
        private void ctxMenu_Click(object sender, RoutedEventArgs e) => DashboardPage.Visibility = Visibility.Collapsed;

        private void aboutNavBtn_Click(object sender, RoutedEventArgs e) => DashboardPage.Visibility = Visibility.Collapsed;

        private void ctxMenuBtn_Click(object sender, RoutedEventArgs e)
        {
            DashboardPage.Visibility = Visibility.Collapsed;
            RootNavigation.Navigate(typeof(ContextMenuPage));
        }

        private void PremiumBtn_Click(object sender, RoutedEventArgs e)
        {
            Upgrade dialog = new Upgrade();
            dialog.Show();
            dialog.Activate();
        }
    }
}