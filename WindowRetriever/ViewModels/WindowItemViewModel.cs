using System;
using System.Windows.Input;
using WindowRetriever.Utils;

namespace WindowRetriever.ViewModels {
    public class WindowItemViewModel : BaseViewModel {


        #region Public Properties
        /// <summary>
        /// IntPtr for the window
        /// </summary>
        public IntPtr hWnd { get; set; }

        /// <summary>
        /// Title of the window
        /// </summary>
        public String title { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// default constructor
        /// </summary>
        /// <param name="hWnd">the IntPtr of window</param>
        /// <param name="title">the title of window</param>
        public WindowItemViewModel(IntPtr hWnd, String title) {
            this.hWnd = hWnd;
            this.title = title;
        }

        #endregion

    }
}