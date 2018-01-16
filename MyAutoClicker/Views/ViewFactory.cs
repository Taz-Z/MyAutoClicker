using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MyAutoClicker.Views
{
    /// <summary>
    /// Skeleton structure for if/when multiple views may be needed
    /// </summary>
    public static class ViewFactory
    {
        public static Window CreateNewView(ViewType type)
        {
            switch(type)
            {
                default:
                    return null;
            }
        }
    }

    /// <summary>
    /// Skeleton enum to label multiple views
    /// </summary>
    public enum ViewType
    {
        
    }
}
