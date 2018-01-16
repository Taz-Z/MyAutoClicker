using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyAutoClicker.Commands;
using MyAutoClicker.Models;
using MyAutoClicker.Views;
using System.Windows.Input;
using static MyAutoClicker.Views.ViewFactory;
using Gma.System.MouseKeyHook;
using System.Windows.Forms;
using System.Windows;
using System.Collections.ObjectModel;

namespace MyAutoClicker.ViewModels
{
    class ClickLocationViewModel
    {
        private ClickLocations allClicks;
        private IKeyboardMouseEvents m_GlobalHook;

        public ClickLocationViewModel()
        {
            ClickCommand = new ClickLocationUpdateCommand(this);
            SaveClickCommand = new ListSaveCommand(this);
            AllPoints = new ObservableCollection<Point>();
            allClicks = new ClickLocations();
        }
        public ObservableCollection<Point> AllPoints { set; get; }

        public ClickLocations AllClicks
        {
            get
            {
                return allClicks;
            }
        }

        public ICommand ClickCommand
        {
            get;
            private set;
        }

        public ICommand SaveClickCommand
        {
            get;
            private set;
        }

        #region MouseHooks
        public void Subscribe()
        {
            // Note: for the application hook, use the Hook.AppEvents() instead
            m_GlobalHook = Hook.GlobalEvents();

            m_GlobalHook.MouseDownExt += GlobalHookMouseDownExt;
            m_GlobalHook.KeyPress += GlobalHookKeyPress;
        }

        private void GlobalHookKeyPress(object sender, KeyPressEventArgs e)
        {
            Console.WriteLine("KeyPress: \t{0}", e.KeyChar);
        }

        private void GlobalHookMouseDownExt(object sender, MouseEventExtArgs e)
        {
            allClicks.CurrentPoint = new Point(e.X, e.Y);
            AllPoints.Add(allClicks.CurrentPoint);



            // uncommenting the following line will suppress the middle mouse button click
            // if (e.Buttons == MouseButtons.Middle) { e.Handled = true; }
        }

        public void Unsubscribe()
        {
            m_GlobalHook.MouseDownExt -= GlobalHookMouseDownExt;
            m_GlobalHook.KeyPress -= GlobalHookKeyPress;

            //It is recommened to dispose it
            m_GlobalHook.Dispose();
        }
        #endregion

    }

}
