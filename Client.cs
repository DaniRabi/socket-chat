﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using System.IO;

namespace SocketClient
{
    class Program
    {
        static void Main(string[] args)
        {
            StartClient();

            Console.ReadKey();
        }

        public static void StartClient()
        {
            byte[] bytes = new byte[1024];

            try
            {
                IPHostEntry ipHostInfo = Dns.GetHostEntry(Dns.GetHostName());
                IPAddress ipAddress = ipHostInfo.AddressList[2];
                IPEndPoint remoteEP = new IPEndPoint(ipAddress, 11000);

                Socket sender = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

                try
                {
                    sender.Connect(remoteEP);

                    Console.WriteLine("Socket connected to {0}", sender.RemoteEndPoint.ToString());

                    string s = Console.ReadLine();
                    byte[] msg = Encoding.ASCII.GetBytes(s);// "This is a test<EOF>");

                    int bytesSent = sender.Send(msg);

                    int bytesRec = sender.Receive(bytes);
                    Console.WriteLine("Echoed test = {0}", Encoding.ASCII.GetString(bytes, 0, bytesRec));

                    sender.Shutdown(SocketShutdown.Both);
                    sender.Close();
                }
                catch (ArgumentNullException ane)
                { Console.WriteLine("ArgumentNullException : {0}", ane.ToString()); }
                catch (SocketException se)
                { Console.WriteLine("SocketException: {0}", se.ToString()); }
                catch (Exception e)
                { Console.WriteLine("Unexpected exception: {0}", e.ToString()); }
            }
            catch (Exception e)
            { Console.WriteLine(e.ToString()); }
        }
    }
}
