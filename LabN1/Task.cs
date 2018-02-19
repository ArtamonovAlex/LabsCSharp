using System;
using System.Collections.Generic;

namespace LabN1
{
    public class Task
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime Deadline { get; set; }
        public List<string> Tags { get; set; }
        public Task(string ti, string des, DateTime dl, List<string> tg)
        {
            this.Title = ti;
            this.Description = des;
            this.Deadline = dl;
            this.Tags = tg;
        }

    }
}
