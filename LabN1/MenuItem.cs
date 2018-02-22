﻿using System;
using System.Collections.Generic;

namespace LabN1
{
    class MenuItem
    {
        public string Label { get; set; }
        public char Key { get; set; }
        public Func<bool> Action;
        public MenuItem(string label, Func<bool> action)
        {
            Label = label;
            Action = action;
        }
    }
}
