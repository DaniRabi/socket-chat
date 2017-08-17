using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using System.IO;

namespace Server
{
    class Program
    {
        static void Main(string[] args)
        {
            StarListening();
        }
        public static string data = null;
        public static void StarListening()
        {
            byte[] bytes = new byte[1024];

            IPHostEntry ipHostInfo = Dns.GetHostEntry(Dns.GetHostName());
            IPAddress ipAddress = ipHostInfo.AddressList[2];
            IPEndPoint localEndPoint = new IPEndPoint(ipAddress, 11000);

            Socket listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            try
            {
                listener.Bind(localEndPoint);
                listener.Listen(4);

                while (true)
                {
                    Console.WriteLine("Waiting for a connection...");
                    Socket handler = listener.Accept();
                    data = null;

                    //while (true)
                    //{
                        bytes = new byte[1024];
                        int bytesRec = handler.Receive(bytes);
                        data += Encoding.ASCII.GetString(bytes, 0, bytesRec);
                    //    if (data.IndexOf("<EOF>") > -1)
                    //        break;
                    //}

                    Console.WriteLine("Text received : {0}", data);

                    byte[] msg = Encoding.ASCII.GetBytes(data);

                    handler.Send(msg);
                    handler.Shutdown(SocketShutdown.Both);
                    handler.Close();
                }
            }
            catch (Exception e)
            { Console.WriteLine(e.ToString()); }

            Console.WriteLine("\nPress ENTER to continue...");
            Console.Read();
        }
    }
}
