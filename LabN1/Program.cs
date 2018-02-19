using System;
using System.Collections.Generic;

namespace LabN1
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Write("Please, introduce yourself: ");
            string name = Console.ReadLine();
            Console.WriteLine($"Hello, {name}!");
            var td = new Todo_list();
            td.Init();
            var menu = new Menu();
            menu.Init();
            menu.Work(td);
            Console.WriteLine($"Goodbye, {name}!");
            Console.ReadLine();
            return;
        }
    }
}