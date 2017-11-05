using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mescp.ViewModels
{
    public class VmMapPrimiviteBase: DocumentViewModel
    {
        protected VmMapPrimiviteBase()
        {
            IsCheckedZoomPan = true;
        }

        public void ClearChecked()
        {
            App.Workspace.MapViewModel._ClearChecked();
            App.Workspace.PrimiviteViewModel._ClearChecked();
        }
        private void _ClearChecked()
        {
            IsChecked_ZoomIn = false;
            IsChecked_ZoomOut = false;
            IsCheckedZoomPan = false;

            IsCheckedSelectTarget = false;
            IsCheckedMoveTarget = false;
            IsCheckedDeleteTarget = false;
            IsCheckedDrawLabel = false;
            IsCheckedDrawLegend = false;
        }

        #region IsChecked_ZoomIn

        private Boolean _IsChecked_ZoomIn;
        public Boolean IsChecked_ZoomIn
        {
            get
            {
                return _IsChecked_ZoomIn;
            }
            set
            {
                _IsChecked_ZoomIn = value;
                RaisePropertyChanged("IsChecked_ZoomIn");
            }
        }

        #endregion

        #region IsChecked_ZoomOut

        private Boolean _IsChecked_ZoomOut;
        public Boolean IsChecked_ZoomOut
        {
            get
            {
                return _IsChecked_ZoomOut;
            }
            set
            {
                _IsChecked_ZoomOut = value;
                RaisePropertyChanged("IsChecked_ZoomOut");
            }
        }

        #endregion

        #region IsCheckedZoomPan

        private Boolean _IsCheckedZoomPan;
        public Boolean IsCheckedZoomPan
        {
            get
            {
                return _IsCheckedZoomPan;
            }
            set
            {
                _IsCheckedZoomPan = value;
                RaisePropertyChanged("IsCheckedZoomPan");
            }
        }

        #endregion



        #region IsCheckedSelectTarget

        private Boolean _IsCheckedSelectTarget;
        public Boolean IsCheckedSelectTarget
        {
            get
            {
                return _IsCheckedSelectTarget;
            }
            set
            {
                _IsCheckedSelectTarget = value;
                RaisePropertyChanged("IsCheckedSelectTarget");
            }
        }

        #endregion

        #region IsCheckedMoveTarget

        private Boolean _IsCheckedMoveTarget;
        public Boolean IsCheckedMoveTarget
        {
            get
            {
                return _IsCheckedMoveTarget;
            }
            set
            {
                _IsCheckedMoveTarget = value;
                RaisePropertyChanged("IsCheckedMoveTarget");
            }
        }

        #endregion
        
        #region IsCheckedDeleteTarget

        private Boolean _IsCheckedDeleteTarget;
        public Boolean IsCheckedDeleteTarget
        {
            get
            {
                return _IsCheckedDeleteTarget;
            }
            set
            {
                _IsCheckedDeleteTarget = value;
                RaisePropertyChanged("IsCheckedDeleteTarget");
            }
        }

        #endregion

        #region IsCheckedDrawLabel

        private Boolean _IsCheckedDrawLabel;
        public Boolean IsCheckedDrawLabel
        {
            get
            {
                return _IsCheckedDrawLabel;
            }
            set
            {
                _IsCheckedDrawLabel = value;
                RaisePropertyChanged("IsCheckedDrawLabel");
            }
        }

        #endregion

        #region IsCheckedDrawLegend

        private Boolean _IsCheckedDrawLegend;
        public Boolean IsCheckedDrawLegend
        {
            get
            {
                return _IsCheckedDrawLegend;
            }
            set
            {
                _IsCheckedDrawLegend = value;
                RaisePropertyChanged("IsCheckedDrawLegend");
            }
        }

        #endregion

        //


    }
}
