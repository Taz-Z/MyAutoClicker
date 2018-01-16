using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyAutoClicker.Commands
{
    internal class DoClicksCommand : GeneralCommand
    {
        public DoClicksCommand(MyAutoClicker.ViewModels.ClickLocationViewModel viewModel) : base(viewModel) { }
        public override void Execute(object parameter)
        {
            viewModel.ClickAllPoints();
        }
    }
}
