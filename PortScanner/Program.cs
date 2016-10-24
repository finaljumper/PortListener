using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;

namespace PortScanner
{
    class Program
    {
        private static IPAddress address;
        private static Socket sSocket;
        static void Main(string[] args)
        {
            Console.Write("Enter IP for listening: ");
            address = IPAddress.Parse(Console.ReadLine());
            Console.WriteLine("Press Ctrl+C to interrupt.");
            for (int i = 1; i < 65536; i++)
            {
                try
                {
                    IPEndPoint endPoint = new IPEndPoint(address, i);
                    sSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                    sSocket.Connect(endPoint);
                    Console.WriteLine("\nPort {0} is listening.", i);
                }
                catch (SocketException ignored)
                {
                    Console.Write(".");
                    if (ignored.ErrorCode != 10061)
                        Console.WriteLine(ignored.Message);
                }
                finally
                {
                    if (sSocket.Connected)
                    {
                        sSocket.Shutdown(SocketShutdown.Both);
                        sSocket.Close();
                    }
                }

            }
            Console.ReadKey();
        }
    }
}
