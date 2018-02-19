using System;
using System.Collections.Generic;

namespace LabN1
{
    class Menu
    {
        public List<MenuItem> M { get; set; }
        public void Init()
        {
            List<MenuItem> m = new List<MenuItem> { };
            string[] mi = { "Add task", "Last tasks", "Find task by tags","Load from .csv file", "Save in .csv format", "Exit" };
            int i = 1;
            foreach (string item in mi)
            {
                m.Add(new MenuItem(mi[i - 1], char.Parse(i.ToString())));
                i++;
            }
            M = m;
        }
        public void Print()
        {
            Console.WriteLine("To continue press Enter");
            Console.ReadLine();
            Console.Clear();
            Console.WriteLine("Menu:");
            foreach (var item in M)
            {
                Console.WriteLine($"{item.Key}.{item.Label}");
            }
            Console.Write("> ");
        }
        public void Work(Todo_list td)
        {
            while (true)
            {
                Print();
                if (!char.TryParse(Console.ReadLine(), out char m))
                {
                    Console.WriteLine("Wrong input");
                    continue;
                }
                switch (m)
                {
                    case '1':
                        {
                            td.AddTask();
                            break;
                        }
                    case '2':
                        {
                            td.PrintAll();
                            break;
                        }
                    case '3':
                        {
                            td.Delete();
                            break;
                        }
                    case '4':
                        {
                            td.InitCsv();
                            break;
                        }
                    case '5':
                        {
                            td.SaveCsv();
                            break;
                        }
                    case '6':
                        {
                            td.Save();                            
                            return;
                        }
                    default:
                        {
                            Console.WriteLine("Unknown command");
                            break;
                        }
                }
            }
        }
    }
}