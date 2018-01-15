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

namespace MyAutoClicker.ViewModels
{
    class ClickLocationViewModel
    {
        private ClickLocations allClicks;
        private IKeyboardMouseEvents m_GlobalHook;

        public ClickLocationViewModel()
        {
            UpdateCommand = new ClickLocationUpdateCommand(this);
            allClicks = new ClickLocations();
        }

        public ClickLocations AllClicks
        {
            get
            {
                return allClicks;
            }
        }

        public void UpdateList()
        {
            Window view  = ViewFactory.CreateNewView(ViewType.ClickAnywhere);
            view.Show();
            Subscribe();
            
        }

        public ICommand UpdateCommand
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
            Console.WriteLine("Y axis: {0}, X axis: {1}", e.Y, e.X);
            allClicks.CurrentPoint = new Point(e.X, e.Y);
            Console.Write(sender.GetType());
            Unsubscribe();



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
