using System.Drawing;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace Lemon.Map.Wpf.Resources
{
    public static class Converters
    {
        public static readonly DrawingColorToWpfBrushConverter DrawingColorToWpfBrushConverterSingleton = new();
        public class DrawingColorToWpfBrushConverter : IValueConverter
        {
            public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            {
                if (value is System.Drawing.Color drawingColor)
                {
                    System.Windows.Media.Color mediaColor = System.Windows.Media.Color.FromArgb(
                        drawingColor.A,
                        drawingColor.R,
                        drawingColor.G,
                        drawingColor.B);
                    return new SolidColorBrush(mediaColor);
                }
                return new SolidColorBrush(Colors.YellowGreen);
            }

            public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            {
                ///https://stackoverflow.com/questions/16093157/wpf-binding-onewaytosource-with-a-converter-results-in-immediate-exception
                if (value is SolidColorBrush brush)
                {
                    System.Drawing.Color color = System.Drawing.Color.FromArgb(brush.Color.A,
                        brush.Color.R,
                        brush.Color.G,
                        brush.Color.B);
                    return color;
                }
                return System.Drawing.Color.Transparent;
            }
        }
    }
}
