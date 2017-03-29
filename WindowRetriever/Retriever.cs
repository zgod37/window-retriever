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

            //add unique id in-case two windows are open with identical titles
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
        /// Move the desired window to primary monitor
        /// </summary>
        /// <param name="title">the stored title for the window</param>
        public void moveWindowToPrimaryMonitor(String title) {
            if (currentWindows.ContainsKey(title)) {

                //get default retrieval position
                int X = (int)Properties.Settings.Default["DefaultXPosition"];
                int Y = (int)Properties.Settings.Default["DefaultYPosition"];

                //un-minimize window if needed
                if (IsIconic(currentWindows[title])) {
                    OpenIcon(currentWindows[title]);
                }

                if (!SetWindowPos(currentWindows[title], IntPtr.Zero, X, Y, 0, 0, SWP_NOSIZE | SWP_NOZORDER)) {
                    //an error occurred
                    int errorCode = Marshal.GetLastWin32Error();
                    String errorMessage = new Win32Exception(errorCode).Message;
                    if (errorCode == 5) {
                        errorMessage += "\nDeveloper's note: Try running WindowRetriever as adminstrator";
                    }
                    MessageBox.Show($"Received error {errorCode} - {errorMessage}");
                }
            }
        }

        /// <summary>
        /// move all currentWindows to primary monitor
        /// </summary>
        public void moveAllWindowsToPrimaryMonitor() {
            
            //get default retrieval position
            int X = (int)Properties.Settings.Default["DefaultXPosition"];
            int Y = (int)Properties.Settings.Default["DefaultYPosition"];

            foreach (String title in currentWindows.Keys) {
                SetWindowPos(currentWindows[title], IntPtr.Zero, X, Y, 0, 0, SWP_NOSIZE | SWP_NOZORDER);
            }
        }

    }
}