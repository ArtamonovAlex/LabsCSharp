using System;
using System.Collections.Generic;

namespace LabN1
{
    public class Task
    {
        public string Title { get; private set; }
        public string Description { get; private set; }
        public DateTime Deadline { get; private set; }
        public List<string> Tags { get; private set; }
        public Task(string title, string description, DateTime deadline, List<string> tags)
        {
            Title = title;
            Description = description;
            Deadline = deadline;
            Tags = tags;
        }

    }
}
