using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    class Program
    {
        static IPAddress serverIP = IPAddress.Parse("127.0.0.1");
        static int serverPort = 3333;
        static List<Socket> clients = new List<Socket>();

        static void Main(string[] args)
        {
            Socket serverSocket = new Socket(SocketType.Stream, ProtocolType.Tcp);
            IPEndPoint serverEndPoint = new IPEndPoint(serverIP, serverPort); ;
            serverSocket.Bind(serverEndPoint);
            serverSocket.Listen(50);
            Console.WriteLine("HOST STARTED");

            while (true)
            {
                Socket clientSocket = serverSocket.Accept();
                clients.Add(clientSocket);
                Task.Run(() =>
                {
                    if (clientSocket.Connected)
                    {
                        Console.WriteLine($"{clientSocket.RemoteEndPoint} connected");

                    }

                    byte[] buffer = new byte[1024000];
                    while (true)
                    {
                        try
                        {
                            for (int i = 0; i < clients.Count; i++)
                            {
                                int length = clientSocket.Receive(buffer);
                                string message = Encoding.UTF8.GetString(buffer, 0, length);
                                foreach (var client in clients)
                                {
                                    client.Send(Encoding.Unicode.GetBytes(message));
                                }
                            }

                        }
                        catch (SocketException)
                        {
                            Console.WriteLine($"{clientSocket.RemoteEndPoint} disconnected");
                            break;
                        }
                    }
                    clientSocket.Close();
                });
            }
        }
    }
}
