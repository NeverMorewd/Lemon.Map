using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using WpfTheme.Controls;

namespace WpfTheme
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Point _start;
        private Point _origin;
        private bool _isDragging = false;

        public MainWindow()
        {
            InitializeComponent();
            AddHandler(TestControl.MouseLeftButtonUpEvent, new MouseButtonEventHandler(TestControl_MouseLeftButtonUp),true);

            //// Load an image
            //EditableImage.Source = new BitmapImage(new Uri("test.png", UriKind.Relative));

            //// Set initial canvas size
            //ImageCanvas.Width = EditableImage.Source.Width;
            //ImageCanvas.Height = EditableImage.Source.Height;

            //// Mouse events for dragging
            //ImageCanvas.MouseWheel += ImageCanvas_MouseWheel;
            //ImageCanvas.MouseLeftButtonDown += ImageCanvas_MouseLeftButtonDown;
            //ImageCanvas.MouseMove += ImageCanvas_MouseMove;
            //ImageCanvas.MouseLeftButtonUp += ImageCanvas_MouseLeftButtonUp;
        }

        private void TestControl_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var p1 = Mouse.GetPosition(VisualTreeHelper.GetParent(control) as Grid);
            var p2 = Mouse.GetPosition(mainGrid);
        }

        //private void ImageCanvas_MouseWheel(object sender, MouseWheelEventArgs e)
        //{
        //    var scale = e.Delta > 0 ? 1.1 : 1 / 1.1;
        //    ImageScaleTransform.ScaleX *= scale;
        //    ImageScaleTransform.ScaleY *= scale;
        //}

        //private void ImageCanvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        //{
        //    if (e.LeftButton == MouseButtonState.Pressed)
        //    {
        //        _start = e.GetPosition(ImageCanvas);
        //        _origin = new Point(ImageTranslateTransform.X, ImageTranslateTransform.Y);
        //        _isDragging = true;
        //        ImageCanvas.CaptureMouse();
        //    }
        //}

        //private void ImageCanvas_MouseMove(object sender, MouseEventArgs e)
        //{
        //    if (_isDragging)
        //    {
        //        var position = e.GetPosition(ImageCanvas);
        //        ImageTranslateTransform.X = _origin.X + (position.X - _start.X);
        //        ImageTranslateTransform.Y = _origin.Y + (position.Y - _start.Y);
        //    }
        //}

        //private void ImageCanvas_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        //{
        //    _isDragging = false;
        //    ImageCanvas.ReleaseMouseCapture();
        //}
    }
}