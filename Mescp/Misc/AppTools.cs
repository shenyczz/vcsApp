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
    }
}
