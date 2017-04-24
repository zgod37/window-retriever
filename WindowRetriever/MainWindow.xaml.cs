using System.Threading.Tasks;
using System.Windows;
using WindowRetriever.ViewModels;

namespace WindowRetriever {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {

        public MainWindow() {
            MessageBox.Show($"{Properties.Settings.Default.CurrentVersionStr} \n {Properties.Settings.Default.ProjectHomepageStr}");
            InitializeComponent();
            this.DataContext = new WindowListViewModel();
        }

        /// <summary>
        /// Open the settings window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void showSettingsWindow(object sender, RoutedEventArgs e) {
            var window = new SettingsWindow();
            window.Show();
        }
    }
}
