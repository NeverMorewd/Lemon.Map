using Lemon.Map.ViewModel;
using Lemon.Map.Wpf.Controls;
using ReactiveUI;
using System.Diagnostics;
using System.Reactive.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using System.Windows.Threading;

namespace Lemon.Map.Wpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : LemonWindow
    {
        private double _originalMapHeight;
        private double _originalMapWidth;
        private readonly double _deltaFactor = 1.25;
        private double _scaleFactor = 1;
        private DispatcherTimer _timer;
        private Stopwatch _stopwatch;
        private int _frameCount;
        private Point _start;
        private Point _origin;
        private bool _isDragging;
        public MainWindow()
        {
            InitializeComponent();

            DataContext = new MainViewModel();

            _stopwatch = new Stopwatch();
            _stopwatch.Start();

            _timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromMilliseconds(100) // 每100毫秒更新一次
            };
            _timer.Tick += Timer_Tick;
            _timer.Start();
            CompositionTarget.Rendering += OnRendering;
        }
        private void Timer_Tick(object? sender, EventArgs e)
        {
            var elapsed = _stopwatch.Elapsed.TotalSeconds;
            var fps = _frameCount / elapsed;
            FpsTextBlock.Text = $"FPS: {fps:F2}";

            _frameCount = 0;
            _stopwatch.Restart();
        }
        private void OnRendering(object? sender, EventArgs e)
        {
            _frameCount++;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ToggleSlider_Checked(object sender, RoutedEventArgs e)
        {
            ApplicationContext.Default.SwitchToDark();
        }

        private void ToggleSlider_Unchecked(object sender, RoutedEventArgs e)
        {
            ApplicationContext.Default.SwitchToLight();
        }

        private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (MapControl == null) return;

            double scaleFactor = (e.NewValue / 5) * 1.5;

            MapControl.Width = _originalMapWidth * scaleFactor;
            MapControl.Height = _originalMapHeight * scaleFactor;
            //AdjustScrollViewerToCenter();
        }

        private void MapControl_Loaded(object sender, RoutedEventArgs e)
        {
            _originalMapHeight = MapControl.Height;
            _originalMapWidth = MapControl.Width;
            //AdjustScrollViewerToCenter();
        }

        private void AdjustScrollViewerToCenter()
        {
            //double offsetX = Math.Max(0, (MapControl.Width - MapScrollViewer.ViewportWidth) / 2);
            //double offsetY = Math.Max(0, (MapControl.Height - MapScrollViewer.ViewportHeight) / 2);
            //Debug.WriteLine($"X:{offsetX};Y:{offsetY}");
            //MapScrollViewer.ScrollToHorizontalOffset(offsetX);
            //MapScrollViewer.ScrollToVerticalOffset(offsetY);
        }

        private void MapTypeSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void MapControl_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            bool isCtrlPressed = (Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control;
            if (!isCtrlPressed)
            {
                return;
            }
            var scaleTransform = (ScaleTransform)((TransformGroup)MapControl.RenderTransform).Children[0];
            var translateTransform = (TranslateTransform)((TransformGroup)MapControl.RenderTransform).Children[1];
            double zoom = e.Delta > 0 ? .2 : -.2;
            if (scaleTransform.ScaleX + zoom > 0.1 && scaleTransform.ScaleY + zoom > 0.1)
            {
                var relative = e.GetPosition(MapControl);
                var absoluteX = relative.X * scaleTransform.ScaleX + translateTransform.X;
                var absoluteY = relative.Y * scaleTransform.ScaleY + translateTransform.Y;

                scaleTransform.ScaleX += zoom;
                scaleTransform.ScaleY += zoom;

                translateTransform.X = absoluteX - relative.X * scaleTransform.ScaleX;
                translateTransform.Y = absoluteY - relative.Y * scaleTransform.ScaleY;
            }
            e.Handled = true;
        }

        private void MapControl_MouseMove(object sender, MouseEventArgs e)
        {
            if (Mouse.LeftButton == MouseButtonState.Pressed && _start != e.GetPosition(this))
            {
                _isDragging = true;
                MapControl.CaptureMouse();
                var v = _start - e.GetPosition(this);
                var translateTransform = (TranslateTransform)((TransformGroup)MapControl.RenderTransform).Children[1];
                translateTransform.X = _origin.X - v.X;
                translateTransform.Y = _origin.Y - v.Y;
            }

        }

        private void MapControl_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            _start = e.GetPosition(this);
            var translateTransform = (TranslateTransform)((TransformGroup)MapControl.RenderTransform).Children[1];
            _origin = new Point(translateTransform.X, translateTransform.Y);
        }

        private void MapControl_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (_isDragging)
            {
                _isDragging = false;
                MapControl.ReleaseMouseCapture();
                e.Handled = true;
            }
        }

        private void ToggleButton_Checked(object sender, RoutedEventArgs e)
        {
            WindowStyle = WindowStyle.None;
            WindowState = WindowState.Maximized;
        }

        private void ToggleButton_Unchecked(object sender, RoutedEventArgs e)
        {
            WindowStyle = WindowStyle.SingleBorderWindow;
        }

        protected override void OnActivated(EventArgs e)
        {
            base.OnActivated(e);
        }
    }
}