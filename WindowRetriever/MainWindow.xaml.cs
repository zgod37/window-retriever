using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WindowRetriever {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {

        Retriever retriever;

        public MainWindow() {

            MessageBox.Show($"{Properties.Settings.Default.CurrentVersionStr} \n {Properties.Settings.Default.ProjectHomepageStr}");

            InitializeComponent();

            retriever = new Retriever(this);
            retriever.getCurrentWindows();
        }

        private void refreshButton_Click(object sender, RoutedEventArgs e) {
            listBox.Items.Clear();
            retriever.getCurrentWindows();
        }


        private void retrieveButton_Click(object sender, RoutedEventArgs e) {
            String title = (String)listBox.SelectedItem;
            if (String.IsNullOrEmpty(title) == false) {
                retriever.moveWindowToPrimaryMonitor(title);
            }
        }

        public void fillListBox(Dictionary<string, IntPtr>.KeyCollection keys) {
            foreach (String title in keys) {
                listBox.Items.Add(title);
            }
        }

        private void settingsButton_Click(object sender, RoutedEventArgs e) {
            new SettingsWindow().Show();
        }
    }
}
