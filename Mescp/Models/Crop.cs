using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mescp.Models
{
    /// <summary>
    /// 作物
    /// </summary>
    public class Crop : CropModelBase
    {
        public String CropID { get; set; }
        public String CropCode { get; set; }
        public String CropName { get; set; }

        public override string ToString()
        {
            return String.Format("{0} {1}", CropID, CropName);
        }
    }

    public class CropCollection : List<Crop>
    {
    }

}
