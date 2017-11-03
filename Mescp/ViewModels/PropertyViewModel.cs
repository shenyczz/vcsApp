using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mescp.ViewModels
{
    public class PropertyViewModel: ToolViewModel
    {

        private Object _VisionProperties;
        /// <summary>
        /// 视觉属性
        /// </summary>
        public Object VisionProperties
        {
            get { return _VisionProperties; }
            set
            {
                _VisionProperties = value;
                RaisePropertyChanged("VisionProperties");
            }
        }
    }
}
