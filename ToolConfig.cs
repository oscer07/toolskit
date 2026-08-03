using System;
using System.Collections.Generic;

namespace ToolkitApp
{
    public class ToolItem
    {
        public string Name { get; set; }
        public string Command { get; set; }
        public string Shell { get; set; } // "cmd" or "powershell"

        public override string ToString()
        {
            return $"{Name} ({Shell})";
        }
    }

    public class ToolkitConfig
    {
        public List<ToolItem> Tools { get; set; } = new List<ToolItem>();
    }
}
