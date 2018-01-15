using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using MyAutoClicker.ViewModels;

namespace MyAutoClicker.Commands
{
    internal class ListUpdateCommand : ICommand
    {
        private ClickLocationViewModel viewModel;

        public ListUpdateCommand(ClickLocationViewModel viewModel)
        {
            this.viewModel = viewModel;
        }


        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            
        }
    }
}
