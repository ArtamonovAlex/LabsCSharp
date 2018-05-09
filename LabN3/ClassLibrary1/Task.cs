using System;
using System.Collections.Generic;

namespace ClassLibrary1
{
    [Serializable]
    public class Task
    {
        public Task()
        {
        }

        public string Title;

        public string Description;

        public DateTime Deadline;

        public List<string> Tags;

        public Task(string title, string description, DateTime deadline, List<string> tags)
        {
            Title = title;
            Description = description;
            Deadline = deadline;
            Tags = tags;
        }
    }
}
