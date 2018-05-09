using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using ClassLibrary1;

namespace Server
{
    public class Program
    {
        public static TodoList List;

        public static void Start(object socket)
        {
            Socket client = (Socket)socket;
            Console.WriteLine("Client with ip {0} connected.", client.RemoteEndPoint);
            Channel channel = new Channel(client);
            //channel.Send(List);
            while (client.Connected)
            {
                try {
                    channel.Send(List);
                    List = channel.Receive();
                    List.PrintAll();
                    }
                catch (SocketException)
                {

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
            try
            {
                List<Task> list = TodoList.ReadCsv(args[1]);
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
