using PropertyChanged;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WindowRetriever.Utils;

namespace WindowRetriever.ViewModels {

    /// <summary>
    /// Base view model to handle propertychanged events
    /// </summary>
    [ImplementPropertyChanged]
    public abstract class BaseViewModel : INotifyPropertyChanged {
        /// <summary>
        /// event fired whenever a child changes its property
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged = (sender, e) => { };

        /// <summary>
        /// get the absolute position of the mouse pointer
        /// </summary>
        /// <returns></returns>
        public Point getCursorPosition() {
            Point cursorPosition = WindowUtils.GetMousePosition();
            return cursorPosition;
        }
    }
}
