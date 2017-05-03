using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace ConsoleApplication3
{
    class Program
    {
        static void Main(string[] args)
        {
            IPHostEntry ipHost = Dns.GetHostEntry("localhost");
            IPAddress ipAddr = IPAddress.Parse("127.0.0.1");
            IPEndPoint ipEndPoint = new IPEndPoint(ipAddr, 11000);

            Socket sListener = new Socket(ipAddr.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            List<String> gotStrings = new List<String>();
            bool er;
            try
            {
                sListener.Bind(ipEndPoint);
                sListener.Listen(50);
                while (true)
                {
                    Console.WriteLine("Ожидаем соединение через порт {0}", ipEndPoint);
                    Socket handler = sListener.Accept();
                    string data = null;
                    byte[] bytes = new byte[1024];
                    int bytesRec = handler.Receive(bytes);
                    data += Encoding.UTF8.GetString(bytes, 0, bytesRec);
                    //Console.Write("Полученный текст: " + data + "\n\n");
                    data = data.Replace(" ", string.Empty);
                    er = false;
                    foreach (string s in gotStrings)
                        if (String.Compare(data, s) == 0)
                        {
                            Console.WriteLine("Ошибка, совпадение найдено.");
                            er = true;
                        }
                    if (!er)
                    {
                        Console.WriteLine(@"Строка '" + data + "' добавлена, совпадений нет.");
                        gotStrings.Add(data);
                    }
                    handler.Shutdown(SocketShutdown.Both);
                    handler.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
    }
}
