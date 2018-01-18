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
using System.Windows;

namespace MyAutoClicker.ViewModels
{
    class ClickLocationViewModel : INotifyPropertyChanged
    {
        private MainModel clickLocation;
        private IKeyboardMouseEvents m_GlobalHook;
        private int lowerTimeRange;
        private int upperTimeRange;
        private bool selectingPoints;
        private bool abletoRun;
        private int position;
        private int clickNumber; //For testing purposed, remove later
        private WindowState windowState;

        /// <summary>
        /// Used to manipulate mouse 
        ///</summary>
        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern void mouse_event(long dwFlags, long dx, long dy, long cButtons, long dwExtraInfo);

        /// <summary>
        /// Initializes the view model, sets commands and defaults
        /// </summary>
        public ClickLocationViewModel()
        {
            ChooseClickCommand = new RelayCommand(canExecute => true, Execute => Subscribe());
            SaveClickCommand = new RelayCommand(canExecute => true, Execute => Unsubscribe());
            TestCommand = new RelayCommand(canExecute => true, Execute => Test());
            ClickCommand = new RelayCommand(canExecute => true, Execute => ClickAllPoints());
            RemoveAtCommand = new RelayCommand(canExecute => true, Execute => RemoveAt(Position - 1));
            RemoveTopCommand = new RelayCommand(canExecute => true, Execute => RemoveAt(0));
            RemoveBottomCommand = new RelayCommand(canExecute => true, Execute => RemoveAt(AllPoints.Count - 1));
            RemoveAllCommand = new RelayCommand(canExecute => true, Execute => AllPoints.Clear());
            AllPoints = new ObservableCollection<Point>();
            clickLocation = new MainModel();

            LowerTimeRange = 0;
            UpperTimeRange = 1000;
            Position = 1;
            SelectingPoints = false;
            AbletoRun = false;

        }

        #region Properties

        /// <summary>
        /// Bound to update ListBox
        /// </summary>
        public ObservableCollection<Point> AllPoints
        {
            set;
            get;
        }

        /// <summary>
        /// Model
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

        /// <summary>
        /// Upper Time Range
        /// </summary>
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

        /// <summary>
        /// Button Enabler/Disabler
        /// </summary>
        public bool SelectingPoints
        {
            get
            {
                return selectingPoints;
            }
            set
            {
                selectingPoints = value;
                OnPropertyChanged("SelectingPoints");
            }
        }

        /// <summary>
        /// Button Enabler/Disabler
        /// </summary>
        public bool AbletoRun
        {
            get
            {
                return abletoRun; ;
            }
            set
            {
                abletoRun = value;
                OnPropertyChanged("AbletoRun");
            }
        }

        /// <summary>
        /// Position of element user wants to delete
        /// </summary>
        public int Position
        {
            get
            {
                return position;
            }
            set
            {
                position = value;
                OnPropertyChanged("Position");
            }
        }

        /// <summary>
        /// Model obj
        /// </summary>
        public MainModel CurrentClickLocation
        {
            get
            {
                return clickLocation;
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

        #endregion

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
            foreach(Point p in AllPoints)
            {
                int timeToWait = new Random().Next(lowerTimeRange, UpperTimeRange + 1); //gets a random time to wait between each click.
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();
                Console.WriteLine("On point " + p);
                while (stopwatch.ElapsedMilliseconds < timeToWait) { } //Wait for a random amout of time
                stopwatch.Stop();
                stopwatch.Restart();
                mouse_event(MOUSEEVENTF_LEFTDOWN | MOUSEEVENTF_LEFTUP, (long)p.X,  (long)p.Y, 0, 0);
            }
           
        }

        /// <summary>
        /// Tests method, will move to unit test later
        /// </summary>
        private void Test()
        {
            Console.WriteLine("You have clicked the button {0} times", clickNumber++);
        }

        /// <summary>
        /// Removes the point from the specified index, checking for bounds
        /// </summary>
        /// <param name="index"></param>
        private void RemoveAt(int index)
        {
            if (index >= AllPoints.Count || index < 0) return;
            AllPoints.RemoveAt(index);
        }

        #endregion

        #region MouseHooks

        /// <summary>
        /// Suscribes to listen to global mouse clicks
        /// </summary>
        private void Subscribe()
        {
            AbletoRun = false;
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
            //TODO for adding shortcuts
                    
        }

        /// <summary>
        /// Captures the mouse click, sets the current point to clickLocation and updates Obseravble List
        /// </summary>
        private void GlobalHookMouseDownExt(object sender, MouseEventExtArgs e)
        {
            AbletoRun = false;
            SelectingPoints = true;
            AllPoints.Add(new Point(e.X, e.Y));
            // uncommenting the following line will suppress the middle mouse button click
            // if (e.Buttons == MouseButtons.Middle) { e.Handled = true; }
        }

        /// <summary>
        /// Unsuscribes listener, no longer cares when a key or mouse is pressed/clicked
        /// </summary>
        private void Unsubscribe()
        {
            SelectingPoints = false;
            AbletoRun = true;
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
