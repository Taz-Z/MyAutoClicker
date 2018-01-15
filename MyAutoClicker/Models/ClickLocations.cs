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
    public class ClickLocations : INotifyPropertyChanged

    {
        private Point currPoint;

        public ClickLocations()
        {
        }

        public Point CurrentPoint
        {
            get
            {
                return currPoint;
            }
            set
            {
                currPoint = value;
                OnPropertyChanged("CurrentPoint");

            }

        }


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
