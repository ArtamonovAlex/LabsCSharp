using System;
using System.Collections.Generic;

namespace LabN1
{
    class Menu
    {
        public Menu (string title)
        {
            Title = title;
            MenuItems = new List<MenuItem>();
        }
        public string Title { get; private set; }
        public List<MenuItem> MenuItems { get; private set; }
        private void AddItem(string title, Func<bool> function)
        {
            MenuItems.Add(new MenuItem(title, function));
        }
        public static Menu InitMain(TodoList todo)
        {
            Menu main = new Menu("Main:");
            main.AddItem("1.Add task", todo.AddTask);
            main.AddItem("2.Last tasks", todo.PrintAll);
            main.AddItem("3.Find tasks by tags", todo.Delete);
            main.AddItem("4.Load from .csv file", todo.InitCsv);
            main.AddItem("5.Save in .csv format", todo.SaveCsv);
            main.AddItem("6.Exit", todo.Exit);
            return main;
        }
        private void Print()
        {
            Console.WriteLine("To continue press Enter");
            Console.ReadLine();
            Console.Clear();
            Console.WriteLine(Title);
            foreach (var item in MenuItems)
            {
                Console.WriteLine(item.Label);
            }
            Console.Write("> ");
        }
        public void Work()
        {
            bool run = true;
            while (run)
            {
                Print();
                if (!char.TryParse(Console.ReadLine(), out char chosenOption))
                {
                    Console.WriteLine("Wrong input");
                    continue;
                }
                foreach (var item in MenuItems)
                {
                    if (chosenOption == item.Label[0])
                    {
                        run = item.Action();
                    }
                }
            }
        }
        
    }
}