using System;

namespace LabN1
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Write("Please, introduce yourself: ");
            string name = Console.ReadLine();
            Console.WriteLine($"Hello, {name}!");
            var td = new TodoList();
            td.Init();
            var menu = new Menu();
            menu.Init(td);
            menu.Work(td);
            Console.WriteLine($"Goodbye, {name}!");
            Console.ReadLine();
            return;
        }
    }
}