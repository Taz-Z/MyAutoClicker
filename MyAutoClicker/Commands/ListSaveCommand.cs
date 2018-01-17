
namespace MyAutoClicker.Commands
{
    internal class ListSaveCommand : GeneralCommand
    {
        /// <summary>
        /// Initializes ListSaveCommand
        /// </summary>
        public ListSaveCommand(MyAutoClicker.ViewModels.ClickLocationViewModel viewModel) : base(viewModel) { }

        /// <summary>
        /// Unsuscribes the moeus listener to stagnate listbox
        /// </summary>
        public override void Execute(object parameter)
        {
            viewModel.Unsubscribe();
        }
    }
}
