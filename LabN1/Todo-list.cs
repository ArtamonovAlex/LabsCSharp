using System;
using System.Collections.Generic;
using System.IO;

namespace LabN1
{
    class TodoList
    {
        public List<Task> Tasks { get; set; }
        public void Init()
        {
            List<Task> tasks = new List<Task> { };
            string path = "ToDo.txt";
            var file = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Read);
            var reader = new StreamReader(file);
            while (!reader.EndOfStream)
            {
                string title = reader.ReadLine();
                string desc = reader.ReadLine();
                DateTime date = DateTime.Parse(reader.ReadLine());
                var tags = new List<string> { };
                do
                {
                    tags.Add(reader.ReadLine());
                } while (tags[tags.Count - 1] != "");
                tags.Remove("");
                var task = new Task(title, desc, date, tags);
                tasks.Add(task);               
            }
            Tasks = tasks;
            file.Close();
        }
        public bool AddTask()
        {
            List<Task> tasks = new List<Task> { };
            tasks = Tasks;
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
            int i = 1;
            do
            {
                Console.Write($"{i,15}: ");
                i++;
                tags.Add(Console.ReadLine());
            } while (tags[tags.Count - 1] != "");
            tags.Remove("");
            var task = new Task(title, description, date, tags);
            tasks.Add(task);
            Tasks = tasks;
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
        public bool Save()
        {
            Console.WriteLine("Do you want to save changes in ToDo.txt? (yes/any different)");
            if (Console.ReadLine() == "yes")
            {
                string path = "ToDo.txt";
                var file = new FileStream(path, FileMode.Truncate, FileAccess.Write);
                var writer = new StreamWriter(file);
                foreach (var item in Tasks)
                {
                    writer.WriteLine(item.Title);
                    writer.WriteLine(item.Description);
                    writer.WriteLine(item.Deadline.ToShortDateString());
                    foreach (var tag in item.Tags)
                    {
                        writer.WriteLine(tag);
                    }
                    writer.WriteLine("");
                }
                writer.Close();
                file.Close();
            }
            return false;
        }
        private void Print(Task item)
        {
            Console.WriteLine($"Title: {item.Title}");
            Console.WriteLine($"Description: {item.Description}");
            Console.WriteLine($"Deadline: {item.Deadline:d}");
            Console.WriteLine($"Tags:");
            int i = 1;
            foreach (var tag in item.Tags)
            {
                Console.WriteLine($"    {i}: {tag}");
                i++;
            }
            Console.WriteLine("-------------------------");
        }
        private List<Task> Find()
        {
            List<Task> tasks = new List<Task> { };
            tasks = Tasks;
            Console.Write("Input tags devided by whitespace: ");
            string input = Console.ReadLine();
            string[] tags = input.Split((string[]) null, StringSplitOptions.RemoveEmptyEntries);
            List<Task> cur = tasks.FindAll(delegate(Task c)
            {
                int k = 0;
                foreach (string tag in tags)
                {
                    if (c.Tags.Contains(tag)) k++;
                }
                if (k == tags.Length) return true;
                else return false;
            });
            return cur;
        }
        public bool Delete()
        {
            List<Task> cur = Find();
            Console.WriteLine("Results:");
            if (cur.Count != 0)
            {
                foreach (var item in cur)
                {
                    Print(item);
                }
                Console.WriteLine("Do you want to delete this tasks? Input 'y' if yes, other to continue");
                if (Console.ReadLine() == "y")
                {
                    foreach (var item in cur)
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
                string str = reader.ReadLine();
                string[] elements = str.Split(separator, StringSplitOptions.RemoveEmptyEntries);
                string title = elements[0];
                string desc = elements[1];
                DateTime date = DateTime.Parse(elements[2]);
                var tags = new List<string> { };
                for (int i = 3; i<elements.Length;i++)
                {
                    string tag = elements[i];
                    tags.Add(tag);
                }
                var task = new Task(title, desc, date, tags);
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