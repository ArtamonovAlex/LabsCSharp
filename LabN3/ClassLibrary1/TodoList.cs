using System;
using System.Collections.Generic;
using System.IO;

namespace ClassLibrary1
{
    [Serializable]
    public class TodoList
    {
        public TodoList()
        {
        }

        public TodoList(List<Task> list)
        {
            Tasks = list;
        }

        public List<Task> Tasks { get; set; }

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
                if (!DateTime.TryParse(Console.ReadLine(), out date))
                {
                    Console.WriteLine("Wrong input");
                    continue;
                }
                if (date >= DateTime.Now) break;
                Console.WriteLine("Sorry, you lost your time. Please, think of a new deadline.");
            }
            Console.WriteLine($"{"Tags (finish on empty line):",38} ");
            List<string> tags = new List<string> { };
            int counter = 1;
            do
            {
                Console.Write($"{counter,15}: ");
                counter++;
                tags.Add(Console.ReadLine());
            } while (tags[tags.Count - 1] != "");
            tags.Remove("");
            Tasks.Add(new Task(title,description,date,tags));
            Console.WriteLine("Task added!");
            return true;
        }

        public bool PrintAll()
        {
            if (Tasks != null && Tasks.Count != 0)
            {
                Tasks.Sort((Task x, Task y) => x.Deadline.CompareTo(y.Deadline));
                Console.WriteLine("Last tasks:");
                foreach (Task item in Tasks)
                {
                    Print(item);
                }
            }
            else
            {
                Console.WriteLine("No tasks");
            }
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
            foreach (string tag in item.Tags)
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
            string[] tags = input.Split((string[])null, StringSplitOptions.RemoveEmptyEntries);
            List<Task> result = Tasks.FindAll(delegate (Task c)
            {
                int counter = 0;
                foreach (string tag in tags)
                {
                    if (c.Tags.Contains(tag)) counter++;
                }
                if (counter == tags.Length) return true;
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
                foreach (Task item in result)
                {
                    Print(item);
                }
                Console.WriteLine("Do you want to delete this tasks? Input 'y' if yes, other to continue");
                if (Console.ReadLine() == "y")
                {
                    foreach (Task item in result)
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
            FileStream file = new FileStream(path, FileMode.Create, FileAccess.Write);
            using (StreamWriter writer = new StreamWriter(file))
            {
                writer.WriteLine("Title;Description;Deadline;Tags");
                foreach (Task item in Tasks)
                {
                    writer.Write(item.Title + ";");
                    writer.Write(item.Description + ";");
                    writer.Write(item.Deadline.ToShortDateString());
                    foreach (string tag in item.Tags)
                    {
                        writer.Write(";" + tag);
                    }
                    writer.WriteLine();
                }
            }
            file.Close();
            return true;
        }

        public bool InitCsv()
        {
            List<Task> list = new List<Task>();
            while (true)
            {
                string path = PathReader();
                if (path == "cancel")
                {
                    return true;
                }
                try
                {
                    list = ReadCsv(path);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    continue;
                }
                break;
            }
            Tasks = list;
            return true;
        }

        public static List<Task> ReadCsv(string path)
        {
            List<Task> tasks = new List<Task> { };
            FileStream file = new FileStream(path, FileMode.Open, FileAccess.Read);
            using (StreamReader reader = new StreamReader(file))
            {
                reader.ReadLine();
                char[] separator = { ';' };
                while (!reader.EndOfStream)
                {
                    string inputString = reader.ReadLine();
                    string[] elements = inputString.Split(separator);
                    string title = elements[0];
                    string description = elements[1];
                    DateTime date = DateTime.Parse(elements[2]);
                    List<string> tags = new List<string> { };
                    for (int i = 3; i < elements.Length; i++)
                    {
                        string tag = elements[i];
                        tags.Add(tag);
                    }
                    Task task = new Task(title, description, date, tags );
                    tasks.Add(task);
                }
            }
            file.Close();
            return tasks;
        }

        private string PathReader()
        {
            Console.Write("Path to the file (input 'cancel' to get back to menu): ");
            return Console.ReadLine();
        }
    }
}
