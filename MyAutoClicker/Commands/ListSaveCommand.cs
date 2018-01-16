using System;
using System.Windows.Input;
using MyAutoClicker.ViewModels;

namespace MyAutoClicker.Commands
{
    internal class ListSaveCommand : GeneralCommand
    {
        /// <summary>
        /// Initializes ListSaveCommand
        /// </summary>
        public ListSaveCommand(ClickLocationViewModel viewModel) : base(viewModel) { }

        /// <summary>
        /// Unsuscribes the moeus listener to stagnate listbox
        /// </summary>
        public override void Execute(object parameter)
        {
            viewModel.Unsubscribe();
        }
    }
}
