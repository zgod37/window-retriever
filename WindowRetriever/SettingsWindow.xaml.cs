using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace WindowRetriever {

    /// <summary>
    /// Interaction logic for SettingsWindow.xaml
    /// </summary>
    public partial class SettingsWindow : Window {

        MainWindow mainWindow;

        public SettingsWindow(MainWindow mainWindow) {
            InitializeComponent();
            this.mainWindow = mainWindow;
            fillTextBoxes();
        }

        /// <summary>
        /// initialize textboxes with saved default position
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void fillTextBoxes() {
            int defaultX = Properties.Settings.Default.DefaultXPosition;
            int defaultY = Properties.Settings.Default.DefaultYPosition;
            xTextBox.Text = defaultX.ToString();
            yTextBox.Text = defaultY.ToString();
        }

        /// <summary>
        /// updates the current cursor position label
        /// </summary>
        /// <param name="currentPosition">current position of cursor</param>
        private void UpdateMousePosition(Point currentPosition) {
            currentPositionLabel.Content = $"Cursor Position X:{currentPosition.X} Y:{currentPosition.Y}";
        }

        private void saveButton_Click(object sender, RoutedEventArgs e) {
            try {
                int newX = Int32.Parse(xTextBox.Text);
                int newY = Int32.Parse(yTextBox.Text);

                Properties.Settings.Default.DefaultXPosition = newX;
                Properties.Settings.Default.DefaultYPosition = newY;
                Properties.Settings.Default.Save();

                this.Hide();
            } catch (Exception ex) {
                //System.Diagnostics.Debug.WriteLine("exception caught");
                MessageBox.Show($"Received error {ex.Message}");
            }
        }

        private void Window_KeyDown(object sender, KeyEventArgs e) {

            if (e.Key == Key.C) {
                UpdateMousePosition(mainWindow.GetMousePosition());
            }
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e) {
            settingsGrid.Focus();
        }
    }
}
