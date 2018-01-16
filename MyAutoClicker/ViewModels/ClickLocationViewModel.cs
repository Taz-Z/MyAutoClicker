using System;
using MyAutoClicker.Commands;
using MyAutoClicker.Models;
using System.Windows.Input;
using Gma.System.MouseKeyHook;
using System.Windows.Forms;
using System.Windows;
using System.Collections.ObjectModel;

namespace MyAutoClicker.ViewModels
{
    class ClickLocationViewModel
    {
        private ClickLocation clickLocation;
        private IKeyboardMouseEvents m_GlobalHook;

        /// <summary>
        /// Initializes the view model and sets commands
        /// </summary>
        public ClickLocationViewModel()
        {
            ChooseClickCommand = new ClickLocationUpdateCommand(this);
            SaveClickCommand = new ListSaveCommand(this);
            AllPoints = new ObservableCollection<Point>();
            clickLocation = new ClickLocation();
        }

        #region Properties

        /// <summary>
        /// Bound to update ListBox
        /// </summary>
        public ObservableCollection<Point> AllPoints { set; get; }

        /// <summary>
        /// Model
        /// </summary>
        public ClickLocation CurrentClickLocation
        {
            get
            {
                return clickLocation;
            }
        }

        /// <summary>
        /// Command to choose location of mouse clicks
        /// </summary>
        public ICommand ChooseClickCommand
        {
            get;
            private set;
        }

        /// <summary>
        /// Saves location of all mouse click
        /// </summary>
        public ICommand SaveClickCommand
        {
            get;
            private set;
        }
        #endregion

        #region MouseHooks

        /// <summary>
        /// Suscribes to listen to global mouse clicks
        /// </summary>
        public void Subscribe()
        {
            // Note: for the application hook, use the Hook.AppEvents() instead
            m_GlobalHook = Hook.GlobalEvents();
            m_GlobalHook.MouseDownExt += GlobalHookMouseDownExt;
            m_GlobalHook.KeyPress += GlobalHookKeyPress;
        }

        /// <summary>
        /// Currently does nothing when a key is pressed
        /// </summary>
        private void GlobalHookKeyPress(object sender, KeyPressEventArgs e)
        {
            Console.WriteLine("KeyPress: \t{0}", e.KeyChar);
        }

        /// <summary>
        /// Captures the mouse click, sets the current point to clickLocation and updates Obseravble List
        /// </summary>
        private void GlobalHookMouseDownExt(object sender, MouseEventExtArgs e)
        {
            clickLocation.ClickPoint = new Point(e.X, e.Y);
            AllPoints.Add(clickLocation.ClickPoint);
            // uncommenting the following line will suppress the middle mouse button click
            // if (e.Buttons == MouseButtons.Middle) { e.Handled = true; }
        }

        /// <summary>
        /// Unsuscribes listener, no longer cares when a key or mouse is pressed/clicked
        /// </summary>
        public void Unsubscribe()
        {
            AllPoints.RemoveAt(AllPoints.Count - 1);
            m_GlobalHook.MouseDownExt -= GlobalHookMouseDownExt;
            m_GlobalHook.KeyPress -= GlobalHookKeyPress;
            m_GlobalHook.Dispose();   
        }
        #endregion

    }
}
