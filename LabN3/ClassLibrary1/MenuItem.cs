using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary1
{
    public class MenuItem
    {
        public string Label { get; private set; }

        public Func<bool> Action { get; private set; }

        public MenuItem(string label, Func<bool> action)
        {
            Label = label;
            Action = action;
        }
    }
}
