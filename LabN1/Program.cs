using System;

namespace LabN1
{
    public class Program
    {
        static void Main(string[] args)
        {
            Console.Write("Please, introduce yourself: ");
            string name = Console.ReadLine();
            Console.WriteLine($"Hello, {name}!");
            TodoList todo = new TodoList();
            Menu main = Menu.InitMain(todo);
            main.Work();
            Console.WriteLine($"Goodbye, {name}!");
            Console.ReadLine();
        }
    }
}