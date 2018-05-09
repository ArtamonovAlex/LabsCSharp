using System;
using System.Net;
using System.Net.Sockets;
using ClassLibrary1;
using System.Collections.Generic;

namespace Client
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length != 1 || args == null || !int.TryParse(args[0], out int port))
            {
                Console.WriteLine("Invalid arguments");
                return;
            }
            Socket handler = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            try
            {
                handler.Connect(IPAddress.Loopback, port);
                Channel channel = new Channel(handler);
                TodoList list = new TodoList(new List<Task>());
                Menu menu = new Menu("Main", list);
                menu.Init(list);
                menu.Work(channel);
                handler.Shutdown(SocketShutdown.Both);
                handler.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
