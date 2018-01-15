using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MyAutoClicker.Views
{
    public static class ViewFactory
    {
        public static Window CreateNewView(ViewType type)
        {
            switch(type)
            {
                case ViewType.ClickAnywhere:
                  return new ChooseMouseLocationWindow();
                default:
                    return null;
            }
        }
    }

    public enum ViewType
    {
        ClickAnywhere
    }
}
