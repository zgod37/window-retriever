using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
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

        // use p/invoke to get cursor positon for windows
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool GetCursorPos(ref Win32Point pt);

        [StructLayout(LayoutKind.Sequential)]
        internal struct Win32Point {
            public Int32 X;
            public Int32 Y;
        };


        public MainWindow() {
            MessageBox.Show($"{Properties.Settings.Default.CurrentVersionStr} \n {Properties.Settings.Default.ProjectHomepageStr}");
            InitializeComponent();
            retriever = new Retriever(this);
            retriever.getCurrentWindows();
        }

        /// <summary>
        /// Get the position of the mouse using the p/invoke external method
        /// </summary>
        /// <returns>Cursor's position as a Point object</returns>
        public Point GetMousePosition() {
            Win32Point w32Mouse = new Win32Point();
            this.CaptureMouse();
            GetCursorPos(ref w32Mouse);
            this.ReleaseMouseCapture();
            return new Point(w32Mouse.X, w32Mouse.Y);
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
            new SettingsWindow(this).Show();
        }

        private void Window_KeyDown(object sender, KeyEventArgs e) {
            String title = (String)listBox.SelectedItem;
            if (String.IsNullOrEmpty(title) == false) {
                retriever.moveWindowToCursorPosition(title);
            }
        }
    }
}
