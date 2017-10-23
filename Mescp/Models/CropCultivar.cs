using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mescp.Models
{
    /// <summary>
    /// 作物品种
    /// </summary>
    public class CropCultivar : CropModelBase
    {
        public String CropID { get; set; }

        public String CultivarID { get; set; }
        public String CultivarCode { get; set; }
        public String CultivarName { get; set; }

        public override string ToString()
        {
            return String.Format("{0} {1}", CultivarID, CultivarName);
        }
    }

    public class CropCultivarCollection : List<CropCultivar>
    {
    }

}
