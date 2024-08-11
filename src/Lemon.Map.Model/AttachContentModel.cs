using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace Lemon.Map.Model
{
    public class AttachContentModel
    {
        public string Name { get; set; }
        public Point Location { get; set; }
        public object Content { get; set; }
    }
}
