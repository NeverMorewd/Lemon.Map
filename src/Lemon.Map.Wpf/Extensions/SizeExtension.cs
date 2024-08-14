namespace Lemon.Map.Wpf.Extensions
{
    public static class SizeExtension
    {
        public static System.Windows.Size ToWpfSize(this System.Drawing.Size drawingSize)
        {
            return new System.Windows.Size(drawingSize.Width, drawingSize.Height);
        }

        public static System.Drawing.Size ToDrawingSize(this System.Windows.Size wpfSize)
        {
            return new System.Drawing.Size((int)wpfSize.Width, (int)wpfSize.Height);
        }
    }
}
