using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
