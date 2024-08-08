using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls.Primitives;

namespace Lemon.Map.Wpf.Gallery.Controls
{
    public class ToggleSlider : ToggleButton
    {
        public static readonly DependencyProperty OnTextProperty =
              DependencyProperty.Register("OnText", typeof(string), typeof(ToggleSlider), new PropertyMetadata("on"));

        public string OnText
        {
            get { return (string)GetValue(OnTextProperty); }
            set { SetValue(OnTextProperty, value); }
        }
        public static readonly DependencyProperty OffTextProperty =
              DependencyProperty.Register("OffText", typeof(string), typeof(ToggleSlider), new PropertyMetadata("off"));

        public string OffText
        {
            get { return (string)GetValue(OffTextProperty); }
            set { SetValue(OffTextProperty, value); }
        }
    }
}
