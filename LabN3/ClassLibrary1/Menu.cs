using System;
using System.Collections.Generic;

namespace ClassLibrary1
{
    public class Menu
    {
        public string Title { get; private set; }

        public List<MenuItem> MenuItems { get; private set; }

        private  TodoList Todo { get; set; }

        public Menu(string title, TodoList todo)
        {
            Title = title;
            MenuItems = new List<MenuItem>();
            Todo = todo;
        }

        private void AddItem(string title, Func<bool> function)
        {
            MenuItems.Add(new MenuItem(title, function));
        }

        public void Init(TodoList list)
        { 
            AddItem("1.Add task", list.AddTask);
            AddItem("2.Last tasks", list.PrintAll);
            AddItem("3.Find tasks by tags", list.Delete);
            AddItem("4.Load from .csv file", list.InitCsv);
            AddItem("5.Save in .csv format", list.SaveToPath);
            AddItem("6.Exit", list.Exit);
        }

        private void Print()
        {
            Console.WriteLine("To continue press Enter");
            Console.ReadLine();
            Console.Clear();
            Console.WriteLine(Title);
            foreach (MenuItem item in MenuItems)
            {
                Console.WriteLine(item.Label);
            }
            Console.Write("> ");
        }

        public void Work(Channel channel)
        {
            byte[] accepting = new byte[1] { 1 };
            byte[] disconnect = new byte[2] { 1, 1 };
            bool run = true;
            while (run)
            {
                Print();
                if (!char.TryParse(Console.ReadLine(), out char chosenOption))
                {
                    Console.WriteLine("Wrong input");
                    continue;
                }
                channel.Socket.Send(accepting);
                Todo = channel.Receive();
                MenuItems.Clear();
                Init(Todo);
                foreach (MenuItem item in MenuItems)
                {
                    if (chosenOption == item.Label[0])
                    {
                        run = item.Action();
                        channel.Send(Todo);
                    }
                }
            }
            channel.Socket.Send(disconnect);
        }
    }
}
