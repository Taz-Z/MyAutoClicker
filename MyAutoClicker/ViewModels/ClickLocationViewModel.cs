using System;
using MyAutoClicker.Commands;
using MyAutoClicker.Models;
using System.Windows.Input;
using Gma.System.MouseKeyHook;
using System.Windows.Forms;
using System.Windows;
using System.Collections.ObjectModel;
using System.Runtime.InteropServices;
using System.ComponentModel;
using System.Diagnostics;

namespace MyAutoClicker.ViewModels
{
    class ClickLocationViewModel : INotifyPropertyChanged
    {

        private MainModel clickLocation;
        private IKeyboardMouseEvents m_GlobalHook;
        private int lowerTimeRange;
        private int upperTimeRange;

        /// <summary>
        /// Used to manipulate mouse 
        ///</summary>
        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern void mouse_event(long dwFlags, long dx, long dy, long cButtons, long dwExtraInfo);

        /// <summary>
        /// Initializes the view model and sets commands
        /// </summary>
        public ClickLocationViewModel()
        {
            ChooseClickCommand = new ClickLocationUpdateCommand(this);
            SaveClickCommand = new ListSaveCommand(this);
            TestCommand = new TestCommand(this);
            ClickCommand = new DoClicksCommand(this);
            AllPoints = new ObservableCollection<Point>();
            clickLocation = new MainModel();
        }

        #region Properties

        /// <summary>
        /// Bound to update ListBox
        /// </summary>
        public ObservableCollection<Point> AllPoints { set; get; }

        /// <summary>
        /// Model
        /// </summary>
        public MainModel CurrentClickLocation
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
        /// Lower Time Range
        /// </summary>
        public int LowerTimeRange
        {
            get
            {
                return lowerTimeRange;
            }
            set
            {
                lowerTimeRange = value;
                OnPropertyChanged("LowerRangeTime");
            }
        }

        //Upper Time Range
        public int UpperTimeRange
        {
            get
            {
                return upperTimeRange;
            }
            set
            {
                upperTimeRange = value;
                OnPropertyChanged("UpperRangeTime");
            }
        }
        #endregion


        #region Methods

        /// <summary>
        /// Clicks the mouse at all points specified in AllPoints
        /// </summary>
        public void ClickAllPoints()
        {
            int MOUSEEVENTF_LEFTDOWN = 0x02;
            int MOUSEEVENTF_LEFTUP = 0x04;
            int MOUSEEVENTF_RIGHTDOWN = 0x08;
            int MOUSEEVENTF_RIGHTUP = 0x10;
          
            //Call the imported function to click the mouse
            foreach(Point p in AllPoints)
            {
                int timeToWait = new Random().Next(lowerTimeRange, UpperTimeRange + 1); //gets a random time to wait between each click.
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();
                Console.WriteLine("On point " + p);
                while (stopwatch.ElapsedMilliseconds != timeToWait) { }
                stopwatch.Stop();
                stopwatch.Restart();
                mouse_event(MOUSEEVENTF_LEFTDOWN | MOUSEEVENTF_LEFTUP, (long)p.X,  (long)p.Y, 0, 0);
            }
           
        }

        /// <summary>
        /// Tests method, will move to unit test later
        /// </summary>
        public void Test()
        {
            Console.WriteLine("DOES IT WORK LOL");
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
            AllPoints.Add(new Point(e.X, e.Y));
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

    }
}
