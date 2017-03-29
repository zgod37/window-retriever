using System;
using System.Collections.Generic;
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

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool GetCursorPos(ref Win32Point pt);

        [StructLayout(LayoutKind.Sequential)]
        internal struct Win32Point {
            public Int32 X;
            public Int32 Y;
        };

        public SettingsWindow() {
            InitializeComponent();
            this.Loaded += new RoutedEventHandler(SettingsWindow_Loaded);

        }

        /// <summary>
        /// initialize textboxes with saved default position
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void SettingsWindow_Loaded(object sender, RoutedEventArgs e) {
            int defaultX = (int)Properties.Settings.Default["DefaultXPosition"];
            int defaultY = (int)Properties.Settings.Default["DefaultYPosition"];
            xTextBox.Text = defaultX.ToString();
            yTextBox.Text = defaultY.ToString();
        }

        /// <summary>
        /// Get the position of the mouse using the p/invoke external method
        /// </summary>
        /// <returns></returns>
        public Point GetMousePosition() {
            Win32Point w32Mouse = new Win32Point();
            GetCursorPos(ref w32Mouse);
            return new Point(w32Mouse.X, w32Mouse.Y);
        }

        /// <summary>
        /// updates the current position on the UI
        /// </summary>
        /// <param name="currentPosition"></param>
        public void UpdateMousePosition(Point currentPosition) {
            currentPositionLabel.Content = $"Cursor Position X:{currentPosition.X} Y:{currentPosition.Y}";
        }

        private void saveButton_Click(object sender, RoutedEventArgs e) {
            try {
                int newX = Int32.Parse(xTextBox.Text);
                int newY = Int32.Parse(yTextBox.Text);

                Properties.Settings.Default["DefaultXPosition"] = newX;
                Properties.Settings.Default["DefaultYPosition"] = newY;
                Properties.Settings.Default.Save();

                this.Hide();
            } catch (Exception ex) {
                //System.Diagnostics.Debug.WriteLine("exception caught");
                MessageBox.Show("Received error + " + ex.Message);
            }
        }

        private void Window_KeyDown(object sender, KeyEventArgs e) {

            switch (e.Key) {

                case Key.C:
                    this.CaptureMouse();
                    UpdateMousePosition(GetMousePosition());
                    this.ReleaseMouseCapture();
                    break;
                default:
                    break;

            }

        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e) {
            settingsGrid.Focus();
        }
    }
}
