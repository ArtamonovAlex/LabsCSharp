using System;
using System.Collections.Generic;

namespace LabN1
{
    class MenuItem
    {
        public string Label { get; set; }
        public char Key { get; set; }
        public MenuItem(string lbl, char k)
        {
            this.Label = lbl;
            this.Key = k;
        }
    }
}
