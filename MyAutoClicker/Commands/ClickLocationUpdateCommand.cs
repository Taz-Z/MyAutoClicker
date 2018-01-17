
namespace MyAutoClicker.Commands
{
    internal class ClickLocationUpdateCommand : GeneralCommand
    {
        /// <summary>
        /// Initializes a ClickVewLocationUpdataCommand
        /// </summary>
        public ClickLocationUpdateCommand(MyAutoClicker.ViewModels.ClickLocationViewModel viewModel) : base(viewModel) { }

        /// <summary>
        /// Suscribes listener to listen to mouse clicks
        /// </summary>
        public override void Execute(object parameter)
        {
            viewModel.Subscribe();
        }
    }
}