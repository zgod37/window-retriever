using System.ComponentModel;
using System.Windows;
using WindowRetriever.ViewModels;

namespace WindowRetriever {

    /// <summary>
    /// Interaction logic for SettingsWindow.xaml
    /// </summary>
    public partial class SettingsWindow : Window {

        public SettingsWindow() {
            InitializeComponent();
            this.DataContext = new SettingsViewModel();
        }

        /// <summary>
        /// event handler to close settings window after saving
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CloseWindowOnSave(object sender, RoutedEventArgs e) {
            this.Close();
        }

    }
}
