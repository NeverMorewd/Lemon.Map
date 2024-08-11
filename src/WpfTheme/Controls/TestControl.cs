using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace WpfTheme.Controls
{
    public class TestControl : FrameworkElement
    {
        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);
            Pen pen = new(Brushes.Red, 2);
            RectangleGeometry rectangleGeometry = new()
            {
                RadiusX = 5,
                RadiusY = 5,
                Rect = new Rect(0, 0, 50, 50)
            };
            drawingContext.DrawGeometry(Brushes.Blue,
                pen,
                rectangleGeometry);
        }



        public int MyProperty
        {
            get { return (int)GetValue(MyPropertyProperty); }
            set { SetValue(MyPropertyProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MyPropertyProperty =
            DependencyProperty.Register("MyProperty", typeof(int), typeof(TestControl), new PropertyMetadata(0,new PropertyChangedCallback(MyPropertyChanged)));

        private static void MyPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is TestControl testControl)
            {
                var mouse = Mouse.GetPosition(testControl);
            }
        }
    }
}
