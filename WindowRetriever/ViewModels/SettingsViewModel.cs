using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using WindowRetriever.Utils;

namespace WindowRetriever.ViewModels {
    public class SettingsViewModel : BaseViewModel {

        #region Public Properties

        /// <summary>
        /// default X-coordinate as string
        /// </summary>
        public String XString { get; set; }

        /// <summary>
        /// default Y-coordinate as string
        /// </summary>
        public String YString { get; set; }

        /// <summary>
        /// position of the cursor in X: {point.X} Y: {point.Y} format
        /// </summary>
        public String CursorPosition { get; set; }

        /// <summary>
        /// hint for getting the cursor's position
        /// </summary>
        public String CursorToolTip { get; set; }

        #endregion

        #region Public Commands

        /// <summary>
        /// Command to update the cursor position on screen
        /// </summary>
        public ICommand UpdatePositionCommand { get; set; }

        /// <summary>
        /// Command to save the default position
        /// </summary>
        public ICommand SaveCommand { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// default constructor
        /// </summary>
        public SettingsViewModel() {

            //fill textboxes with default position
            this.XString = Properties.Settings.Default.DefaultXPosition.ToString();
            this.YString = Properties.Settings.Default.DefaultYPosition.ToString();

            //set cursor position info
            this.CursorPosition = "Current cursor position:";
            this.UpdatePositionCommand = new RelayCommand(UpdateCursorPosition);
            this.CursorToolTip = "Press the 'C' key to update";

            //set save command
            this.SaveCommand = new RelayCommand(Save);
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// update the current cursor position displayed the UI
        /// </summary>
        private void UpdateCursorPosition() {
            Point cursorPosition = base.getCursorPosition();
            this.XString = cursorPosition.X.ToString();
            this.YString = cursorPosition.Y.ToString();
        }

        /// <summary>
        /// Save the new default X and Y positions
        /// </summary>
        private void Save() {
            
            //try to parse the textboxes, reset to default if invalid input
            //otherwise save as new value
            try {
                int newX = Int32.Parse(XString);
                int newY = Int32.Parse(YString);

                Properties.Settings.Default.DefaultXPosition = newX;
                Properties.Settings.Default.DefaultYPosition = newY;
                Properties.Settings.Default.Save();

            } catch (Exception ex) {
                MessageBox.Show("Error received: " + ex.Message);
                this.XString = Properties.Settings.Default.DefaultXPosition.ToString();
                this.YString = Properties.Settings.Default.DefaultYPosition.ToString();
            }
        }
        #endregion
    }
}
