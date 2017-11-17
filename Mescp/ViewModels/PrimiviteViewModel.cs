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
    public class PrimiviteViewModel: VmMapPrimiviteBase
    {
        public PrimiviteViewModel()
        {
            App.Workspace.MapViewModel.Map.LayerManager.PrimiviteSelected += (s, e) =>
            {
                App.Workspace.PropertyViewModel.VisionProperties = e.CurrentVision.CustomProperties;

                ILayer primiviteLayer = App.Workspace.MapViewModel.Map.LayerManager.PrimiviteLayer;
                IVision v = primiviteLayer?.Vision?.Children.Find(p => p.IsSelected);
                if (v == null)
                {
                    Console.WriteLine("no target is selected!");
                    ILayer layer = App.Workspace.MapViewModel.Map.LayerManager.Layers.Find(p => p.Id == "t123");
                    IVision vision = layer?.Vision;
                    App.Workspace.PropertyViewModel.VisionProperties = vision?.CustomProperties;
                };
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

            ClearChecked();
            IsCheckedSelectTarget = true;
        }
        private Boolean CanSelectTarget(Object parameter)
        {
            return App.Workspace.MapViewModel.IsMapViewModel;
        }

        #endregion

        #region MoveTargetCommand

        private RelayCommand _MoveTargetCommand;
        public ICommand MoveTargetCommand
        {
            get
            {
                if (_MoveTargetCommand == null)
                {
                    _MoveTargetCommand = new RelayCommand(p => OnMoveTarget(p), p => CanMoveTarget(p));
                }

                return _MoveTargetCommand;
            }
        }

        private void OnMoveTarget(Object parameter)
        {
            App.Workspace.MapViewModel.Map.MapTool = MapTool.PrimiviteController.MoveTarget;

            ClearChecked();
            IsCheckedMoveTarget = true;
        }
        private Boolean CanMoveTarget(Object parameter)
        {
            return App.Workspace.MapViewModel.IsMapViewModel;
        }

        #endregion

        #region DeleteTargetCommand

        private RelayCommand _DeleteTargetCommand;
        public ICommand DeleteTargetCommand
        {
            get
            {
                if (_DeleteTargetCommand == null)
                {
                    _DeleteTargetCommand = new RelayCommand(p => OnDeleteTarget(p), p => CanDeleteTarget(p));
                }
                return _DeleteTargetCommand;
            }
        }

        private void OnDeleteTarget(Object parameter)
        {
            App.Workspace.MapViewModel.Map.MapTool = MapTool.PrimiviteController.DeleteTarget;

            ClearChecked();
            IsCheckedDeleteTarget = true;
        }
        private Boolean CanDeleteTarget(Object parameter)
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
                    _DrawLabelCommand = new RelayCommand(p => OnDrawLabel(p), p => CanDrawLabel(p));
                }

                return _DrawLabelCommand;
            }
        }

        private void OnDrawLabel(Object parameter)
        {
            App.Workspace.MapViewModel.Map.MapTool = MapTool.PrimiviteController.DrawLabel;

            ClearChecked();
            IsCheckedDrawLabel = true;
        }

        private Boolean CanDrawLabel(Object parameter)
        {
            return App.Workspace.MapViewModel.IsMapViewModel;
        }

        #endregion

        #region DrawLegendCommand

        private RelayCommand _DrawLegendCommand;
        public ICommand DrawLegendCommand
        {
            get
            {
                if (_DrawLegendCommand == null)
                {
                    _DrawLegendCommand = new RelayCommand(p => OnDrawLegend(p), p => CanDrawLegend(p));
                }

                return _DrawLegendCommand;
            }
        }

        private void OnDrawLegend(Object parameter)
        {
            App.Workspace.MapViewModel.Map.MapTool = MapTool.PrimiviteController.DrawLegend;

            ClearChecked();
            IsCheckedDrawLegend = true;
        }
        private Boolean CanDrawLegend(Object parameter)
        {
            return App.Workspace.MapViewModel.IsMapViewModel;
        }

        #endregion

    }
}
