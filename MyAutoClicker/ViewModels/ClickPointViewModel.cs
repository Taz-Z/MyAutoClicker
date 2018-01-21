using System;
using MyAutoClicker.Commands;
using MyAutoClicker.Models;
using System.Windows.Input;
using Gma.System.MouseKeyHook;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Drawing;

namespace MyAutoClicker.ViewModels
{
    class ClickPointViewModel : INotifyPropertyChanged
    {
        #region Variables

        private ClickPointModel clickPoint;
        private IKeyboardMouseEvents m_GlobalHook;
        private System.Windows.WindowState windowState;
        private int position;
        private bool readytoSelect;
        private bool abletoRun;
        private bool abletoSave;
        private bool pause;

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
            ReadytoSelect = true;
            AbletoRun = false;
            AbletoSave = false;
            Position = 1;
            pause = false;
            m_GlobalHook = Hook.GlobalEvents();
            m_GlobalHook.KeyPress += GlobalHookKeyPress;
            ChooseClickCommand = new RelayCommand(canExecute => true, Execute => Subscribe());
            SaveClickCommand = new RelayCommand(canExecute => true, Execute => Unsubscribe());
            ClickCommand = new RelayCommand(canExecute => true, Execute => ClickAllPoints());
            RemoveTopCommand = new RelayCommand(canExecute => true, Execute => RemoveAt(0));
            RemoveBottomCommand = new RelayCommand(canExecute => true, Execute => RemoveAt(ClickPoint.AllPoints.Count - 1));
            RemoveAllCommand = new RelayCommand(canExecute => true, Execute => ClickPoint.AllPoints.Clear());
            RemoveAtCommand = new RelayCommand(canExecute => true, Execute => RemoveAt(position - 1));
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

        public System.Windows.WindowState StateofWindow
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
        /// Button Enabler/Disabler
        /// </summary>
        public bool ReadytoSelect
        {
            get
            {
                return readytoSelect;
            }
            set
            {
                readytoSelect = value;
                OnPropertyChanged("ReadytoSelect");
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

        public bool AbletoSave
        {
            get
            {
                return abletoSave;
            }
            set
            {
                abletoSave = value;
                OnPropertyChanged("AbletoSave");
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
            ReadytoSelect = false;
            AbletoSave = false;


            Task t = Task.Factory.StartNew(() =>
           {
               int MOUSEEVENTF_LEFTDOWN = 0x02;
               int MOUSEEVENTF_LEFTUP = 0x04;
               int MOUSEEVENTF_RIGHTDOWN = 0x08;
               int MOUSEEVENTF_RIGHTUP = 0x10;
               //StateofWindow = WindowState.Minimized;
                //Call the imported function to click the mouse
                for(;;)
               {
                   foreach (Point point in ClickPoint.AllPoints)
                   {
                       if (pause)
                       {
                           ReadytoSelect = true;
                           AbletoRun = true;
                           pause = false;
                           return;
                       }
                       Point randomPoint = GetRandomSurroundPoint(point);
                       System.Windows.Forms.Cursor.Position = randomPoint;
                       int timeToWait = new Random().Next(ClickPoint.LowerTimeRange, ClickPoint.UpperTimeRange + 1); //gets a random time to wait between each click.
                       Stopwatch stopwatch = new Stopwatch();
                       stopwatch.Start();
                       while (stopwatch.ElapsedMilliseconds < timeToWait) { } //Wait for a random amout of time
                       Console.WriteLine("Clicking at {0}", randomPoint);
                       mouse_event(MOUSEEVENTF_LEFTDOWN | MOUSEEVENTF_LEFTUP, (long)randomPoint.X, (long)randomPoint.Y, 0, 0);
                   }
               }
           });   
        }

        /// <summary>
        /// Gets a random point from a 3 by 3 square with respect to the main point located in the middle
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        private Point GetRandomSurroundPoint(Point point)
        {

            Point[] points =
            {
                new Point(point.X - 1, point.Y),
                new Point(point.X + 1, point.Y),
                new Point(point.X, point.Y - 1),
                new Point(point.X, point.Y + 1),
                new Point(point.X - 1, point.Y - 1),
                new Point(point.X - 1, point.Y + 1),
                new Point(point.X + 1, point.Y - 1),
                new Point(point.X + 1, point.Y +1),
                point
            };
            return points[new Random().Next(points.Length)];
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
            AbletoRun = false;
            AbletoSave = true;
            ReadytoSelect = false;
            m_GlobalHook.MouseDownExt += GlobalHookMouseDownExt;
        }

        /// <summary>
        /// Currently does nothing when a key is pressed
        /// </summary>
        private void GlobalHookKeyPress(object sender, KeyPressEventArgs e)
        {
                if(e.KeyChar.Equals(' '))
            {
                if (AbletoSave == false && ReadytoSelect == false)
                {
                    pause = true;
                }
                else if(AbletoSave)
                {
                    pause = false;
                    ClickAllPoints();
                }
            }
        }

        /// <summary>
        /// Captures the mouse click, sets the current point to clickLocation and updates Obseravble List
        /// </summary>
        private void GlobalHookMouseDownExt(object sender, MouseEventExtArgs e)
        {
            ReadytoSelect = false;
            ClickPoint.AllPoints.Add(new System.Drawing.Point(e.X, e.Y));
            // uncommenting the following line will suppress the middle mouse button click
            // if (e.Buttons == MouseButtons.Middle) { e.Handled = true; }
        }

        /// <summary>
        /// Unsuscribes listener, no longer cares when a key or mouse is pressed/clicked
        /// </summary>
        private void Unsubscribe()
        {
            AbletoSave = false;
            AbletoRun = true;
            ReadytoSelect = true;
            ClickPoint.AllPoints.RemoveAt(ClickPoint.AllPoints.Count - 1); //Work around, removes the click point which is recorded when the button is clicked
            //Unsuscribe
            m_GlobalHook.MouseDownExt -= GlobalHookMouseDownExt;
           // m_GlobalHook.KeyPress -= GlobalHookKeyPress;
          //  m_GlobalHook.Dispose();
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

        #endregion
    }
}
