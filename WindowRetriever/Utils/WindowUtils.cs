using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WindowRetriever.ViewModels;

namespace WindowRetriever.Utils {

    /// <summary>
    /// Collection of static helper methods
    /// </summary>
    public static class WindowUtils {

        #region External Platform/Invoke Methods

        #region Window Data

        /// <summary>
        /// Gets the list of windows currently open
        /// </summary>
        /// <param name="hDesktop"></param>
        /// <param name="lpEnumCallbackFunction"></param>
        /// <param name="lParam"></param>
        /// <returns></returns>
        [DllImport("user32.dll", EntryPoint = "EnumDesktopWindows",
        ExactSpelling = false, CharSet = CharSet.Auto, SetLastError = true)]
        private static extern bool EnumDesktopWindows(IntPtr hDesktop, EnumDelegate lpEnumCallbackFunction, IntPtr lParam);


        /// <summary>
        /// Get the title of the window
        /// </summary>
        /// <param name="hWnd"></param>
        /// <param name="lpWindowText"></param>
        /// <param name="nMaxCount"></param>
        /// <returns></returns>
        [DllImport("user32.dll", EntryPoint = "GetWindowText",
        ExactSpelling = false, CharSet = CharSet.Auto, SetLastError = true)]
        private static extern int GetWindowText(IntPtr hWnd, StringBuilder lpWindowText, int nMaxCount);

        #endregion

        #region Window Properties

        /// <summary>
        /// Returns whether a window is currently minimized
        /// </summary>
        /// <param name="hWnd">the IntPtr of the window</param>
        /// <returns>true if minimized, otherwise false</returns>
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool IsIconic(IntPtr hWnd);


        /// <summary>
        /// Returns whether a window is visible
        /// </summary>
        /// <param name="hWnd">the IntPtr of the window</param>
        /// <returns>true if visible, otherwise false</returns>
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool IsWindowVisible(IntPtr hWnd);

        #endregion

        #region Window Commands

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool OpenIcon(IntPtr hWnd);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);

        #endregion

        #region Mouse Commands

        /// <summary>
        /// external method to return the absolute cursor position
        /// </summary>
        /// <param name="pt"></param>
        /// <returns></returns>
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool GetCursorPos(ref Win32Point pt);


        #endregion

        #region Window-Specific Properties

        const uint SWP_NOSIZE = 0x0001;
        const uint SWP_NOZORDER = 0x0004;

        [StructLayout(LayoutKind.Sequential)]
        private struct Win32Point {
            public Int32 X;
            public Int32 Y;
        };

        #endregion

        #endregion

        #region Public Methods

        /// <summary>
        /// Get list of current visible windows
        /// </summary>
        /// <returns></returns>
        public static List<WindowItemViewModel> GetWindows() {

            List<WindowItemViewModel> windows = new List<WindowItemViewModel>();

            //build a new window item for each visible window
            EnumDelegate filter = delegate (IntPtr hWnd, int lParam) {

                //build string for window title
                StringBuilder sbTitle = new StringBuilder(255);
                int nLength = GetWindowText(hWnd, sbTitle, sbTitle.Capacity + 1);
                String title = sbTitle.ToString();

                //add new view model if window is visible
                if (IsWindowVisible(hWnd) && String.IsNullOrEmpty(title) == false) {
                    windows.Add(new WindowItemViewModel(hWnd, title));
                }

                return true;
            };

            //enumerate desktop windows, if no windows found set to null
            if (!EnumDesktopWindows(IntPtr.Zero, filter, IntPtr.Zero)) {
                //notify user that windows failed to enumerate
            }
            return windows;
        }

        /// <summary>
        /// Move the window to the given screen coordinates
        /// </summary>
        /// <param name="hWnd">the IntPtr of the window</param>
        /// <param name="X"></param>
        /// <param name="Y"></param>
        /// <returns>true if successful, else false</returns>
        public static void MoveWindowToPosition(IntPtr hWnd, int X, int Y) {
            
            //un-minimize window if needed
            if (IsIconic(hWnd)) {
                OpenIcon(hWnd);
            }

            //try to move window, handle error if fails
            if (!SetWindowPos(hWnd, IntPtr.Zero, X, Y, 0, 0, SWP_NOSIZE | SWP_NOZORDER)) {
                checkLastException();
            }

        }

        /// <summary>
        /// Get the position of the mouse using the p/invoke external method
        /// </summary>
        /// <returns>Cursor's position as a Point object</returns>
        public static Point GetMousePosition() {
            Win32Point w32Mouse = new Win32Point();
            GetCursorPos(ref w32Mouse);
            return new Point(w32Mouse.X, w32Mouse.Y);
        }

        #endregion

        #region Private Methods

        private delegate bool EnumDelegate(IntPtr hWnd, int lParam);

        /// <summary>
        /// check the last received error exception and alert user
        /// </summary>
        private static void checkLastException() {
            int errorCode = Marshal.GetLastWin32Error();
            String errorMessage = new Win32Exception(errorCode).Message;
            if (errorCode == 5) {
                errorMessage += "\nTry running WindowRetriever as administrator";
            }
            MessageBox.Show("Received error: " + errorMessage);
        }

        #endregion

    }
}
