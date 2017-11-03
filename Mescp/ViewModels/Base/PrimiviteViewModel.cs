using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using CSharpKit.Vision;
using CSharpKit.Vision.Mapping;
using CSharpKit.Windows.Input;

namespace Mescp.ViewModels
{
    public class PrimiviteViewModel: ViewModelBase
    {
        public PrimiviteViewModel()
        {
            App.Workspace.MapViewModel.Map.LayerManager.PrimiviteSelected += (s, e) =>
            {
                App.Workspace.PropertyViewModel.VisionProperties = e.CurrentVision.CustomProperties;
            };
        }

        #region SelectTargetCommand

        private RelayCommand _SelectTargetCommand;
        public ICommand SelectTargetCommand
        {
            get
            {
                if (_SelectTargetCommand == null)
                {
                    _SelectTargetCommand = new RelayCommand(p => OnSelectTarget(p), p => CanSelectTarget(p));
                }

                return _SelectTargetCommand;
            }
        }

        private void OnSelectTarget(Object parameter)
        {
            App.Workspace.MapViewModel.Map.MapTool = MapTool.PrimiviteController.SelectTarget;
        }
        private Boolean CanSelectTarget(Object parameter)
        {
            return App.Workspace.MapViewModel.IsMapViewModel;
        }

        #endregion



        #region DrawRectCommand

        private RelayCommand _DrawRectCommand;
        public ICommand DrawRectCommand
        {
            get
            {
                if (_DrawRectCommand == null)
                {
                    _DrawRectCommand = new RelayCommand(p => OnDrawRect(p), p => CanDrawRect(p));
                }

                return _DrawRectCommand;
            }
        }

        private void OnDrawRect(Object parameter)
        {
            App.Workspace.MapViewModel.Map.MapTool = MapTool.PrimiviteController.DrawRect;
        }

        private Boolean CanDrawRect(Object parameter)
        {
            return App.Workspace.MapViewModel.IsMapViewModel;
        }

        #endregion

        #region DrawLabelCommand

        private RelayCommand _DrawLabelCommand;
        public ICommand DrawLabelCommand
        {
            get
            {
                if (_DrawLabelCommand == null)
                {
                    _DrawLabelCommand = new RelayCommand(p => OnDrawLabel(p), p => CanDrawRect(p));
                }

                return _DrawLabelCommand;
            }
        }

        private void OnDrawLabel(Object parameter)
        {
            App.Workspace.MapViewModel.Map.MapTool = MapTool.PrimiviteController.DrawLabel;
        }

        private Boolean CanDrawLabel(Object parameter)
        {
            return App.Workspace.MapViewModel.IsMapViewModel;
        }

        #endregion
    }
}
