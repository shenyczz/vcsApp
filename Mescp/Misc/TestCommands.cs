using CSharpKit.Windows.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Mescp.Misc
{
    public class TestCommands
    {
        #region TestCommand

        private RelayCommand _TestCommand;
        public ICommand TestCommand
        {
            get
            {
                if (_TestCommand == null)
                {
                    _TestCommand = new RelayCommand(p => OnTest(p), p => CanTest(p));
                }

                return _TestCommand;
            }
        }

        private void OnTest(Object parameter)
        {
            App.Workspace.AppHelper.Test();
        }
        private Boolean CanTest(Object parameter)
        {
            return true;
        }

        #endregion
    }
}
