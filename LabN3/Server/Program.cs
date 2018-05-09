using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using ClassLibrary1;
using System.IO;

namespace Server
{
    public class Program
    {
        public static TodoList List;

        public static string LocalFile;

        public static void Start(object socket)
        {
            byte[] accepting = new byte[10];
            Socket client = (Socket)socket;
            Console.WriteLine("Client with ip {0} connected.", client.RemoteEndPoint);
            Channel channel = new Channel(client);
            while (client.Connected)
            {
                try {
                    if (client.Receive(accepting) == 1)
                    {
                        List.Tasks = TodoList.ReadCsv(LocalFile);
                        channel.Send(List);
                        List = channel.Receive();
                        List.SaveCsv(LocalFile);
                    }
                    else throw new SocketException();
                }
                catch (SocketException)
                {
                    break;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            Console.WriteLine("Client with ip {0} disconnected", client.RemoteEndPoint);
            client.Close();
        }

        public static void Main(string[] args)
        {
            if (args.Length != 2 || args == null || !int.TryParse(args[0], out int port))
            {
                Console.WriteLine("Invalid arguments");
                return;
            }
            LocalFile = args[1];
            try
            {
                List<Task> list = TodoList.ReadCsv(LocalFile);
                List = new TodoList(list);
            }
            catch
            {
                Console.WriteLine("Invalid arguments");
                return;
            }
            Socket listeningSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPEndPoint endPoint = new IPEndPoint(IPAddress.Loopback, port);
            listeningSocket.Bind(endPoint);
            int backlog = 5;
            listeningSocket.Listen(backlog);
            Console.WriteLine("Server is online. Waiting for connections...");
            bool exit = false;
            while (!exit)
            {
                Socket socket = listeningSocket.Accept();
                Thread thread = new Thread(new ParameterizedThreadStart(Start));
                thread.Start(socket);
            }
        }
    }
}
