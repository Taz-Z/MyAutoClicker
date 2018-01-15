using System;
using System.Windows.Input;
using MyAutoClicker.ViewModels;

namespace MyAutoClicker.Commands
{
    internal class ClickLocationUpdateCommand : ICommand
    {
        private ClickLocationViewModel viewModel;

        public ClickLocationUpdateCommand(ClickLocationViewModel viewModel)
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
            viewModel.UpdateList();
        }
    }
}