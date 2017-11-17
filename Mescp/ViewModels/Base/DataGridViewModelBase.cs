using CSharpKit.Windows.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Mescp.ViewModels
{
    public abstract class DataGridViewModelBase: DocumentViewModel
    {
        #region CloseCommand

        private RelayCommand _CloseCommand;
        public ICommand CloseCommand
        {
            get
            {
                if (_CloseCommand == null)
                {
                    _CloseCommand = new RelayCommand((p) => OnClose(p), (p) => CanClose(p));
                }

                return _CloseCommand;
            }
        }

        private void OnClose(Object parameter)
        {
            App.Workspace.Close(this);
        }

        private bool CanClose(Object parameter)
        {
            return true;
        }

        #endregion
    }
}
