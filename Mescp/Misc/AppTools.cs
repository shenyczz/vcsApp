using Mescp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace Mescp
{
    public class AppTools
    {
        public Boolean Ping(String address)
        {
            PingOptions pingOption = new PingOptions();
            pingOption.DontFragment = true;

            string data = "sendData:goodgoodgoodgoodgoodgood";
            byte[] buffer = Encoding.ASCII.GetBytes(data);
            int timeout = 500;

            Ping ping = new Ping();
            PingReply reply = ping.Send(address, timeout, buffer);

            return (reply.Status == IPStatus.Success);
        }

        //string strTables = string.Format("[MeteDay{0}]", "2010S");
        /// <summary>
        /// 
        /// </summary>
        /// <param name="year">评估年份</param>
        /// <returns></returns>
        public String ConvertTableName(int year)
        {
            string tableName = "";
            //---------------------------------------------
            int iCentury = year / 100;              //世纪
            int iYears = ((year % 100) / 10) * 10;  //年代
            tableName = string.Format("MeteDay{0}{1:D02}S", iCentury, iYears);
            //---------------------------------------------
            return tableName;
        }

        public String GetStationIn(List<XStation> xStations)
        {
            int n = 0;
            StringBuilder sb = new StringBuilder();
            foreach (XStation sta in xStations)
            {
                string s = string.Format("\'{0}\'", sta.Id);
                sb.Append(s);

                if (++n >= xStations.Count)
                    break;

                sb.Append(",");
            }

            return sb.ToString();
        }

        /// <summary>
        /// 转换为 DateTime 类型
        /// </summary>
        /// <param name="yyyymmdd"></param>
        /// <param name="HHMM"></param>
        /// <returns></returns>
        public DateTime ToDateTime(int yyyymmdd, int HHMM)
        {
            int year = yyyymmdd / 10000;
            int mmdd = yyyymmdd % 10000;
            int month = mmdd / 100;
            int day = mmdd % 100;

            int hour = HHMM / 100;
            int minute = HHMM % 100;

            return new DateTime(year, month, day, hour, minute, 0);
        }

        /// <summary>
        /// 生成文件名
        /// </summary>
        public string GenerateFileName( int year, Region region, Crop crop, CropCultivar cropCultivar)
        {
            //区域、作物、品种、年份、
            string fileName = "";
            fileName = string.Format("{0}_{1}_{2}_{3}.txt", year, region.RgnID.ToUpper(), crop.CropID.ToUpper(), cropCultivar.CultivarID.ToUpper());
            return fileName;
        }

    }
}
