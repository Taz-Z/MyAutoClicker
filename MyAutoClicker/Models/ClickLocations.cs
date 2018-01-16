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
    public class ClickLocation : INotifyPropertyChanged

    {
        private Point clickPoint;

        /// <summary>
        /// Initializes a ClickLocation class
        /// </summary>
        public ClickLocation() { }
        
        /// <summary>
        /// New clickpoint when user clicks get updated here, part of binding
        /// </summary>
        public Point ClickPoint
        {
            get
            {
                return clickPoint;
            }
            set
            {
                clickPoint = value;
                OnPropertyChanged("ClickPoint");
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
