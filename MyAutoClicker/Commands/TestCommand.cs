
namespace MyAutoClicker.Commands
{
    internal class TestCommand : GeneralCommand
    {
            public TestCommand(MyAutoClicker.ViewModels.ClickLocationViewModel viewModel) : base(viewModel) { }
            public override void Execute(object parameter)
            {
                viewModel.Test();
            }
        }
}
