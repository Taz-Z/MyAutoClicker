using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MyAutoClicker.Models
{
    internal class ClickPointModel : INotifyPropertyChanged
    {
        #region Vars
        private int lowerTimeRange;
        private int upperTimeRange;
        private bool selectingPoints;
        private bool abletoRun;
        private int position;
        #endregion

        public ClickPointModel()
        {
            AllPoints = new ObservableCollection<Point>();

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
