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
    }
}
