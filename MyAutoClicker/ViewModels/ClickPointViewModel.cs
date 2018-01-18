using System;
using MyAutoClicker.Commands;
using MyAutoClicker.Models;
using System.Windows.Input;
using Gma.System.MouseKeyHook;
using System.Windows.Forms;
using System.Windows;
using System.Runtime.InteropServices;
using System.ComponentModel;
using System.Diagnostics;

namespace MyAutoClicker.ViewModels
{
    class ClickPointViewModel : INotifyPropertyChanged
    {
        #region Variables

        private ClickPointModel clickPoint;
        private IKeyboardMouseEvents m_GlobalHook;
        private int clickNumber; //For testing purposed, remove later
        private WindowState windowState;

        #endregion

        /// <summary>
        /// Used to manipulate mouse 
        ///</summary>
        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern void mouse_event(long dwFlags, long dx, long dy, long cButtons, long dwExtraInfo);

        #region Constructors
        /// <summary>
        /// Initializes the view model, sets commands and defaults
        /// </summary>
        public ClickPointViewModel()
        {
            clickPoint = new ClickPointModel();
            ChooseClickCommand = new RelayCommand(canExecute => true, Execute => Subscribe());
            SaveClickCommand = new RelayCommand(canExecute => true, Execute => Unsubscribe());
            TestCommand = new RelayCommand(canExecute => true, Execute => Test());
            ClickCommand = new RelayCommand(canExecute => true, Execute => ClickAllPoints());
            RemoveAtCommand = new RelayCommand(canExecute => true, Execute => RemoveAt(ClickPoint.Position - 1));
            RemoveTopCommand = new RelayCommand(canExecute => true, Execute => RemoveAt(0));
            RemoveBottomCommand = new RelayCommand(canExecute => true, Execute => RemoveAt(ClickPoint.AllPoints.Count - 1));
            RemoveAllCommand = new RelayCommand(canExecute => true, Execute => ClickPoint.AllPoints.Clear());

        }

        #endregion

        #region Properties
        /// <summary>
        /// Model obj
        /// </summary>
        public ClickPointModel ClickPoint
        {
            get
            {
                return clickPoint;
            }
        }

        public WindowState StateofWindow
        {
            get
            {
                return windowState;
            }
            set
            {
                windowState = value;
                OnPropertyChanged("StateofWindow");
            }
        }

        #region Commands

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

        /// <summary>
        /// Command for testing, will remove to unit tests later
        /// </summary>
        public ICommand TestCommand
        {
            get;
            private set;
        }
        /// <summary>
        /// Command to start clicking where specified
        /// </summary>
        public ICommand ClickCommand
        {
            get;
            private set;
        }

        /// <summary>
        /// Empty List command
        /// </summary>
        public ICommand RemoveAllCommand
        {
            get;
            private set;
        }

        /// <summary>
        /// Remove specified index command
        /// </summary>
        public ICommand RemoveAtCommand
        {
            get;
            private set;
        }

        /// <summary>
        /// Remove element at front of list command
        /// </summary>
        public ICommand RemoveTopCommand
        {
            get;
            private set;
        }

        /// <summary>
        /// Remove element at end of list command
        /// </summary>
        public ICommand RemoveBottomCommand
        {
            get;
            private set;
        }

        #endregion

        #endregion

        #region Methods

        /// <summary>
        /// Clicks the mouse at all points specified in AllPoints
        /// </summary>
        private void ClickAllPoints()
        {
            int MOUSEEVENTF_LEFTDOWN = 0x02;
            int MOUSEEVENTF_LEFTUP = 0x04;
            int MOUSEEVENTF_RIGHTDOWN = 0x08;
            int MOUSEEVENTF_RIGHTUP = 0x10;
            StateofWindow = WindowState.Minimized;
            //Call the imported function to click the mouse
            foreach(Point point in ClickPoint.AllPoints)
            {
                int timeToWait = new Random().Next(ClickPoint.LowerTimeRange, ClickPoint.UpperTimeRange + 1); //gets a random time to wait between each click.
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();
                while (stopwatch.ElapsedMilliseconds < timeToWait) { } //Wait for a random amout of time
                mouse_event(MOUSEEVENTF_LEFTDOWN | MOUSEEVENTF_LEFTUP, (long)point.X,  (long)point.Y, 0, 0);
            } 
        }

        /// <summary>
        /// Removes the point from the specified index, checking for bounds
        /// </summary>
        /// <param name="index"></param>
        private void RemoveAt(int index)
        {
            if (index >= ClickPoint.AllPoints.Count || index < 0) return;
            ClickPoint.AllPoints.RemoveAt(index);
        }

        #region MouseHooks

        /// <summary>
        /// Suscribes to listen to global mouse clicks
        /// </summary>
        private void Subscribe()
        {
            ClickPoint.AbletoRun = false;
            // Note: for the application hook, use the Hook.AppEvents() instead
            m_GlobalHook = Hook.GlobalEvents();
            m_GlobalHook.MouseDownExt += GlobalHookMouseDownExt;
            ///m_GlobalHook.KeyPress += GlobalHookKeyPress;
            /// Uncomment above for keybinds
        }

        /// <summary>
        /// Currently does nothing when a key is pressed
        /// </summary>
        private void GlobalHookKeyPress(object sender, KeyPressEventArgs e)
        {
            //TODO for adding shortcuts

        }

        /// <summary>
        /// Captures the mouse click, sets the current point to clickLocation and updates Obseravble List
        /// </summary>
        private void GlobalHookMouseDownExt(object sender, MouseEventExtArgs e)
        {
            ClickPoint.AbletoRun = false;
            ClickPoint.SelectingPoints = true;
            ClickPoint.AllPoints.Add(new Point(e.X, e.Y));
            // uncommenting the following line will suppress the middle mouse button click
            // if (e.Buttons == MouseButtons.Middle) { e.Handled = true; }
        }

        /// <summary>
        /// Unsuscribes listener, no longer cares when a key or mouse is pressed/clicked
        /// </summary>
        private void Unsubscribe()
        {
            ClickPoint.SelectingPoints = false;
            ClickPoint.AbletoRun = true;
            ClickPoint.AllPoints.RemoveAt(ClickPoint.AllPoints.Count - 1); //Work around, removes the click point which is recorded when the button is clicked
            //Unsuscribe
            m_GlobalHook.MouseDownExt -= GlobalHookMouseDownExt;
            m_GlobalHook.KeyPress -= GlobalHookKeyPress;
            m_GlobalHook.Dispose();
        }
        #endregion

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));

            }
        }
        #endregion

        #region Test Methods
        /// <summary>
        /// Tests method, will move to unit test later
        /// </summary>
        private void Test()
        {
            Console.WriteLine("You have clicked the button {0} times", clickNumber++);
        }
        #endregion

        #endregion
    }
}
