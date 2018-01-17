
namespace MyAutoClicker.Commands
{
    internal class DoClicksCommand : GeneralCommand
    {
        /// <summary>
        /// Initializes a DoClickCommand obj
        /// </summary>
        /// <param name="viewModel"></param>
        public DoClicksCommand(MyAutoClicker.ViewModels.ClickLocationViewModel viewModel) : base(viewModel) { }

        /// <summary>
        /// Allows user to click to choose points
        /// </summary>
        /// <param name="parameter"></param>
        public override void Execute(object parameter)
        {
            viewModel.ClickAllPoints();
        }
    }
}
