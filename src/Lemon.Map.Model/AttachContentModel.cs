﻿using System.Drawing;

namespace Lemon.Map.Model
{
    public class AttachContentModel
    {
        public string Name { get; set; }
        public Point Location { get; set; }
        public object Content { get; set; }
        public Size ContainerSize { get; set; }
    }
}
