using System;
using System.Windows.Input;
using MyAutoClicker.ViewModels;

namespace MyAutoClicker.Commands
{
    internal class ClickLocationUpdateCommand : GeneralCommand
    {
        /// <summary>
        /// Initializes a ClickVewLocationUpdataCommand
        /// </summary>
        public ClickLocationUpdateCommand(ClickLocationViewModel viewModel) : base(viewModel) { }
        /// <summary>
        /// Suscribes listener to listen to mouse clicks
        /// </summary>
        public override void Execute(object parameter)
        {
            viewModel.Subscribe();
        }
    }
}