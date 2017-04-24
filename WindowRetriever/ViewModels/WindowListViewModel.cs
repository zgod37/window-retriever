using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using WindowRetriever.Utils;

namespace WindowRetriever.ViewModels {
    public class WindowListViewModel : BaseViewModel {

        #region Public Properties

        /// <summary>
        /// the child currently selected in this listbox
        /// </summary>
        public WindowItemViewModel Selected { get; set; }

        /// <summary>
        /// list of all the open windows on the desktop
        /// </summary>
        public ObservableCollection<WindowItemViewModel> Items { get; set; }

        /// <summary>
        /// settings window
        /// </summary>
        public SettingsViewModel SettingsWindow { get; set; }

        #endregion

        #region Public Commands

        /// <summary>
        /// Command to move window to default position
        /// </summary>
        public ICommand MoveCommand { get; set; }

        /// <summary>
        /// Command to refresh the items in the list
        /// </summary>
        public ICommand RefreshCommand { get; set; }

        /// <summary>
        /// Command to move window to cursor position
        /// </summary>
        public ICommand CursorCommand { get; set; }

        /// <summary>
        /// Command to show the settings window
        /// </summary>
        public ICommand ShowSettingsWindow { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// default constructor
        /// </summary>
        public WindowListViewModel() {
            this.Items = new ObservableCollection<WindowItemViewModel>(WindowUtils.GetWindows());

            MoveCommand = new RelayCommand(MoveSelectedToDefault);
            CursorCommand = new RelayCommand(MoveSelectedToCursor);
            RefreshCommand = new RelayCommand(RefreshWindowList);
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// move the selected window to the default position
        /// </summary>
        private void MoveSelectedToDefault() {

            WindowItemViewModel selected = this.Selected;

            if (selected != null) {
                WindowUtils.MoveWindowToPosition(selected.hWnd,
                    Properties.Settings.Default.DefaultXPosition, Properties.Settings.Default.DefaultYPosition);
            }
        }

        /// <summary>
        /// move selected window to cursor's current position
        /// </summary>
        private void MoveSelectedToCursor() {

            WindowItemViewModel selected = this.Selected;

            //get cursor's position
            if (selected != null) {
                Point point = base.getCursorPosition();
                int X = (int)point.X;
                int Y = (int)point.Y;
                WindowUtils.MoveWindowToPosition(selected.hWnd, X, Y);
            }

        }

        /// <summary>
        /// update the current list of windows
        /// </summary>
        private void RefreshWindowList() {
            this.Items.Clear();
            this.Items = new ObservableCollection<WindowItemViewModel>(WindowUtils.GetWindows());
        }

        #endregion
    }
}
