using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

using System.ComponentModel;
using System.Drawing;
using System.Windows;

namespace MyAutoClicker.Models
{
    internal class ClickPointModel : INotifyPropertyChanged
    {
        #region Vars
        private int lowerTimeRange;
        private int upperTimeRange;
        #endregion

        public ClickPointModel()
        {
            AllPoints = new ObservableCollection<System.Drawing.Point>();

            LowerTimeRange = 0;
            UpperTimeRange = 1000;

        }

        #region Properties

        /// <summary>
        /// Bound to update ListBox
        /// </summary>
        public ObservableCollection<System.Drawing.Point> AllPoints
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
