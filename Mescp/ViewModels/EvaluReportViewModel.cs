using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CSharpKit;

namespace Mescp.ViewModels
{
    public class EvaluReportViewModel : DocumentViewModel
    {
        public EvaluReportViewModel()
        {
        }

        List<StationInfo> _StationInfos;
        public List<StationInfo> StationInfos
        {
            get
            {
                return _StationInfos;
            }
            set
            {
                _StationInfos = value;
                RaisePropertyChanged("StationInfos");
            }
        }
    }
}
