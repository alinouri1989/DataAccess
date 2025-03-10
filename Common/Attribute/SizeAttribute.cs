﻿namespace System
{
    [AttributeUsage(AttributeTargets.All)]
    public class SizeAttribute : Attribute
    {
        public SizeAttribute(int width, int height)
        {
            Width = width;
            Height = height;
        }
        public int Width { get; set; }
        public int Height { get; set; }
    }
}
