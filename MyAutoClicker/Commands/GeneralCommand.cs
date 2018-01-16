using System;
using System.Windows.Input;
using MyAutoClicker.ViewModels;

namespace MyAutoClicker.Commands
{
    internal abstract class GeneralCommand : ICommand
    {
        protected ClickLocationViewModel viewModel;

        /// <summary>
        /// Initializes and sets view model
        /// </summary>
        /// <param name="viewModel"></param>
        public GeneralCommand(ClickLocationViewModel viewModel)
        {
            this.viewModel = viewModel;
        }

        /// <summary>
        /// Hooks bindings back up
        /// </summary>
        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        /// <summary>
        /// Returns true for now
        /// </summary>
        public bool CanExecute(object parameter)
        {
            return true;
        }

        /// <summary>
        /// Subclasses to override
        /// </summary>
        /// <param name="parameter"></param>
        public abstract void Execute(object parameter);
    }
}
