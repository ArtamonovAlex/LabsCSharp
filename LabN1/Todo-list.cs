using System;
using System.Collections.Generic;
using System.IO;

namespace LabN1
{
    class TodoList
    {
        public TodoList ()
        {
            Tasks = new List<Task>();
        }
        public List<Task> Tasks { get; private set; }
        public bool AddTask()
        {
            Console.WriteLine("New task:");
            Console.Write($"{"Title:",16} ");
            string title = Console.ReadLine();
            Console.Write($"{"Description:",22} ");
            string description = Console.ReadLine();
            DateTime date;
            while (true)
            {
                Console.Write($"{"Deadline in format dd.mm.yyyy:",40} ");
                if (!DateTime.TryParse(Console.ReadLine(),out date))
                {
                    Console.WriteLine("Wrong input");
                    continue;
                }
                if (date >= DateTime.Now) break;
                Console.WriteLine("Sorry, you lost your time. Please, think of a new deadline.");
            }
            Console.WriteLine($"{"Tags (finish on empty line):",38} ");
            var tags = new List<string> { };
            int counter = 1;
            do
            {
                Console.Write($"{counter,15}: ");
                counter++;
                tags.Add(Console.ReadLine());
            } while (tags[tags.Count - 1] != "");
            tags.Remove("");
            Tasks.Add(new Task(title, description, date, tags));
            Console.WriteLine("Task added!");
            return true;
        }
        public bool PrintAll()
        {
            if (Tasks != null && Tasks.Count!=0)
            {
                Tasks.Sort(delegate (Task x, Task y)
                {
                    return x.Deadline.CompareTo(y.Deadline);
                });
                Console.WriteLine("Last tasks:");
                foreach (var item in Tasks)
                {
                    Print(item);
                }
            }
            else Console.WriteLine("No tasks");
            return true;
        }
        public bool Exit()
        {
            return false;
        }
        private void Print(Task item)
        {
            Console.WriteLine($"Title: {item.Title}");
            Console.WriteLine($"Description: {item.Description}");
            Console.WriteLine($"Deadline: {item.Deadline:d}");
            Console.WriteLine($"Tags:");
            int counter = 1;
            foreach (var tag in item.Tags)
            {
                Console.WriteLine($"    {counter}: {tag}");
                counter++;
            }
            Console.WriteLine("-------------------------");
        }
        private List<Task> Find()
        {
            Console.Write("Input tags devided by whitespace: ");
            string input = Console.ReadLine();
            string[] tags = input.Split((string[]) null, StringSplitOptions.RemoveEmptyEntries);
            List<Task> result = Tasks.FindAll(delegate(Task c)
            {
                int k = 0;
                foreach (string tag in tags)
                {
                    if (c.Tags.Contains(tag)) k++;
                }
                if (k == tags.Length) return true;
                else return false;
            });
            return result;
        }
        public bool Delete()
        {
            List<Task> result = Find();
            Console.WriteLine("Results:");
            if (result.Count != 0)
            {
                foreach (var item in result)
                {
                    Print(item);
                }
                Console.WriteLine("Do you want to delete this tasks? Input 'y' if yes, other to continue");
                if (Console.ReadLine() == "y")
                {
                    foreach (var item in result)
                    {
                        Tasks.Remove(item);
                    }
                    Console.WriteLine("Deleted!");
                }
            }
            else Console.WriteLine("No tasks with such tags");
            return true;
        }
        public bool SaveCsv()
        {
            string path = PathReader();
            if (path == "cancel")
            {
                return true;
            }
            var file = new FileStream(path, FileMode.Create, FileAccess.Write);
            var writer = new StreamWriter(file);
            writer.WriteLine("Title;Description;Deadline;Tags");
            foreach (var item in Tasks)
            {
                writer.Write(item.Title + ";");
                writer.Write(item.Description + ";");
                writer.Write(item.Deadline.ToShortDateString());
                foreach (var tag in item.Tags)
                {
                    writer.Write(";" + tag);
                }
                writer.WriteLine();
            }
            writer.Close();
            file.Close();
            return true;
        }
        public bool InitCsv()
        {
            List<Task> tasks = new List<Task> { };
            FileStream file;
            while (true)
            {
                string path = PathReader();
                if (path == "cancel")
                {
                    return true;
                }
                try
                {
                    file = new FileStream(path, FileMode.Open, FileAccess.Read);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    continue;
                }
                break;
            }
            var reader = new StreamReader(file);
            reader.ReadLine();
            char[] separator = { ';' };
            while (!reader.EndOfStream)
            {
                string inputString = reader.ReadLine();
                string[] elements = inputString.Split(separator);
                string title = elements[0];
                string description = elements[1];
                DateTime date = DateTime.Parse(elements[2]);
                var tags = new List<string> { };
                for (int i = 3; i<elements.Length;i++)
                {
                    string tag = elements[i];
                    tags.Add(tag);
                }
                var task = new Task(title, description, date, tags);
                tasks.Add(task);
            }
            Tasks = tasks;
            file.Close();
            return true;
        }
        private string PathReader()
        {
            Console.Write("Path to the file (input 'cancel' to get back to menu): ");
            string path = Console.ReadLine();
            return path;
        }
    }
}