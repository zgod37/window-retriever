using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows;

namespace WindowRetriever {

    /// <summary>
    /// Enumerates and retrieves any open desktop windows
    /// </summary>
    public class Retriever {

        //injected view
        private MainWindow mainWindow;

        //keep current windows in dictionary
        Dictionary<String, IntPtr> currentWindows = new Dictionary<String, IntPtr>();

        //send delegate to filter out invisible windows
        private delegate bool EnumDelegate(IntPtr hWnd, int lParam);

        //################################
        //p/invoke methods/fields delcared here
        //################################

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool IsIconic(IntPtr hWnd);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool OpenIcon(IntPtr hWnd);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool IsWindowVisible(IntPtr hWnd);

        [DllImport("user32.dll", EntryPoint = "EnumDesktopWindows",
        ExactSpelling = false, CharSet = CharSet.Auto, SetLastError = true)]
        private static extern bool EnumDesktopWindows(IntPtr hDesktop, EnumDelegate lpEnumCallbackFunction, IntPtr lParam);

        [DllImport("user32.dll", EntryPoint = "GetWindowText",
        ExactSpelling = false, CharSet = CharSet.Auto, SetLastError = true)]
        private static extern int GetWindowText(IntPtr hWnd, StringBuilder lpWindowText, int nMaxCount);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);

        const uint SWP_NOSIZE = 0x0001;
        const uint SWP_NOZORDER = 0x0004;

        //################################

        public Retriever(MainWindow mainWindow) {
            this.mainWindow = mainWindow;
        }

        /// <summary>
        /// get current of windows open on desktop
        /// update currentWindows dict and fill view's listbox
        /// </summary>
        public void getCurrentWindows() {

            currentWindows.Clear();

            //add unique id for duplicate window titles
            int id = 0;
            EnumDelegate filter = delegate (IntPtr hWnd, int lParam) {

                //build the window title with unique id
                StringBuilder strbTitle = new StringBuilder(255);
                int nLength = GetWindowText(hWnd, strbTitle, strbTitle.Capacity + 1);
                String strTitle = strbTitle.ToString();

                if (IsWindowVisible(hWnd) && String.IsNullOrEmpty(strTitle) == false) {
                    currentWindows.Add($"<{id}> {strTitle}", hWnd);
                }

                id++;

                return true;
            };

            //update currentWindows and fill listbox in view
            if (EnumDesktopWindows(IntPtr.Zero, filter, IntPtr.Zero)) {

                mainWindow.fillListBox(currentWindows.Keys);
            }
        }

        /// <summary>
        /// Move a desktop window to the desired position
        /// </summary>
        /// <param name="hWnd"></param>
        /// <param name="X"></param>
        /// <param name="Y"></param>
        private void moveWindowToPosition(String title, int X, int Y) {
            if (currentWindows.ContainsKey(title)) {

                IntPtr hWnd = currentWindows[title];

                //un-minimize window if needed
                if (IsIconic(hWnd)) {
                    OpenIcon(hWnd);
                }

                //set window position and handle any errors
                if (!SetWindowPos(hWnd, IntPtr.Zero, X, Y, 0, 0, SWP_NOSIZE | SWP_NOZORDER)) {
                    int errorCode = Marshal.GetLastWin32Error();
                    String errorMessage = new Win32Exception(errorCode).Message;
                    if (errorCode == 5) {
                        errorMessage += "\nNote: Try running WindowRetriever as adminstrator";
                    }
                    MessageBox.Show($"Received error {errorCode} - {errorMessage}");
                }
            }
        }


        public void moveWindowToCursorPosition(String title) {
            Point cursorPosition = mainWindow.GetMousePosition();
            moveWindowToPosition(title, (int) cursorPosition.X, (int) cursorPosition.Y);
        }

        /// <summary>
        /// Move the desired window to position [10,10]
        /// this *should* be the top-left corner of the primary monitor
        /// </summary>
        /// <param name="title"></param>
        public void moveWindowToPrimaryMonitor(String title) {
            moveWindowToPosition(title, 10, 10);
        }

        /// <summary>
        /// Move given window to the default position
        /// </summary>
        /// <param name="title"></param>
        public void moveWindowToDefaultPosition(String title) {
            int X = Properties.Settings.Default.DefaultXPosition;
            int Y = Properties.Settings.Default.DefaultYPosition;
            moveWindowToPosition(title, X, Y);
        }

        /// <summary>
        /// move all currentWindows to primary monitor
        /// </summary>
        public void moveAllWindowsToPrimaryMonitor() {

            //get default retrieval position
            int X = Properties.Settings.Default.DefaultXPosition;
            int Y = Properties.Settings.Default.DefaultYPosition;

            foreach (String title in currentWindows.Keys) {
                SetWindowPos(currentWindows[title], IntPtr.Zero, X, Y, 0, 0, SWP_NOSIZE | SWP_NOZORDER);
            }
        }

    }
}