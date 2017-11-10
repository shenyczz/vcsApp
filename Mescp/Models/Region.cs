using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mescp.Models
{
    /// <summary>
    /// 区域
    /// </summary>
    public class Region : CropModelBase
    {
        public String RgnID { get; set; }
        public String RgnCode { get; set; }
        public String RgnName { get; set; }
        public Double Cmin { get; set; }
        public Double Cmax { get; set; }

        public override string ToString()
        {
            return String.Format("{0} {1}", RgnID, RgnName);
        }
    }

    public class RegionCollection : List<Region>
    {
    }

}
