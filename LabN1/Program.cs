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
            var todo = new TodoList();
            var main = Menu.InitMain(todo);
            main.Work();
            Console.WriteLine($"Goodbye, {name}!");
            Console.ReadLine();
            return;
        }
    }
}