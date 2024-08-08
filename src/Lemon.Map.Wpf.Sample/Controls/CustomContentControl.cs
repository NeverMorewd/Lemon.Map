using System.Windows;
using System.Windows.Controls;

namespace Lemon.Map.Wpf.Controls
{
    public class CustomContentControl : ContentControl
    {
        static CustomContentControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(CustomContentControl), new FrameworkPropertyMetadata(typeof(CustomContentControl)));
        }
    }
}
