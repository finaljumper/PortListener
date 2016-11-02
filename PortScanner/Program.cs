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
        private static string startIP, endIP;
        private static IPAddress address;
        private static int[] startOctets;
        private static int[] endOctets;
        static StringBuilder builder;
        private static Socket sSocket;
        static int[] addressParts;
        public static void scan(int a, int b, int c, int d)
        {
            for (int i = 1; i < 65536; i++)
            {
                builder.Append(a);
                builder.Append('.');
                builder.Append(b);
                builder.Append('.');
                builder.Append(c);
                builder.Append('.');
                builder.Append(d);
                address = IPAddress.Parse(builder.ToString());
                builder.Clear();
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
        }

        static void Main(string[] args)
        {
            startOctets = new int[4];
            endOctets = new int[4];
            addressParts = new int[3];
            builder = new StringBuilder();
            Console.Write("Enter start IP for listening: ");
            startIP = Console.ReadLine();
            Console.Write("Enter end IP for listening: ");
            endIP = Console.ReadLine();
            Console.WriteLine("Press Ctrl+C to interrupt.");
            string[] temp = startIP.Split('.');
            for (int i = 0; i < 4; i++)
            {
                startOctets[i] = Int32.Parse(temp[i]);
            }
            temp = endIP.Split('.');
            for (int i = 0; i < 4; i++)
            {
                endOctets[i] = Int32.Parse(temp[i]);
            }
            for (int a = startOctets[0]; a <= endOctets[0]; a++)
            {
                if (a == endOctets[0])
                    addressParts[0] = endOctets[1];
                else
                    addressParts[0] = 255;
                for (int b = startOctets[1]; b <= addressParts[0]; b++)
                {
                    if (b == endOctets[1])
                        addressParts[1] = endOctets[2];
                    else
                        addressParts[1] = 255;
                    for (int c = startOctets[2]; c <= addressParts[1]; c++)
                    {
                        if (c == endOctets[2])
                            addressParts[2] = endOctets[3];
                        else
                            addressParts[2] = 255;
                        for (int d = startOctets[3]; d <= addressParts[2]; d++)
                        {
                            Console.WriteLine(a + "." + b + "." + c + "." + d);
                            scan(a, b, c, d);
                        }
                    }
                }
            }
            Console.ReadKey();
        }
    }
}
