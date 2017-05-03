using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication4
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                int port = 11000;
                IPHostEntry ipHost = Dns.GetHostEntry("localhost");
                IPAddress ipAddr = IPAddress.Parse("127.0.0.1");
                IPEndPoint ipEndPoint = new IPEndPoint(ipAddr, port);
                Socket sender = new Socket(ipAddr.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                sender.Connect(ipEndPoint);
                string message = "test  ";
                byte[] msg = Encoding.UTF8.GetBytes(message);
                int bytesSent = sender.Send(msg);
                sender.Shutdown(SocketShutdown.Both);
                sender.Close();
            }
            catch (Exception ex) {
                Console.WriteLine(ex);
            }
        }
    }
}
