using System;
using System.Collections.Generic;

namespace LabN1
{
    class Menu
    {
        public List<MenuItem> MenuItems { get; private set; }
        public void Init(TodoList td)
        {
            List<MenuItem> menu = new List<MenuItem> { };
            string[] menuitems = { "Add task", "Last tasks", "Find task by tags","Load from .csv file", "Save in .csv format", "Exit" };
            Func<bool>[] foo = {td.AddTask, td.PrintAll, td.Delete, td.InitCsv, td.SaveCsv, td.Save};
            int i = 1;
            foreach (string item in menuitems)
            {
                menu.Add(new MenuItem(menuitems[i - 1], char.Parse(i.ToString()), foo[i-1]));
                i++;
            }
            MenuItems = menu;
        }
        public void Print()
        {
            Console.WriteLine("To continue press Enter");
            Console.ReadLine();
            Console.Clear();
            Console.WriteLine("Menu:");
            foreach (var item in MenuItems)
            {
                Console.WriteLine($"{item.Key}.{item.Label}");
            }
            Console.Write("> ");
        }
        public void Work(TodoList td)
        {
            bool run = true;
            while (run)
            {
                Print();
                if (!char.TryParse(Console.ReadLine(), out char m))
                {
                    Console.WriteLine("Wrong input");
                    continue;
                }
                foreach (var item in MenuItems)
                {
                    if (m == item.Key)
                    {
                        run = item.Action();
                    }
                }
            }
        }
        
    }
}