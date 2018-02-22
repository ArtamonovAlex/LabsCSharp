using System;
using System.Collections.Generic;

namespace LabN1
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
