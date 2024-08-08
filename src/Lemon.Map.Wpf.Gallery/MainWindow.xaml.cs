using Lemon.Map.ViewModel;
using Lemon.Map.Wpf.Gallery.Controls;
using System.Diagnostics;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Lemon.Map.Wpf.Gallery
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

        private void ToggleSlider_Checked(object sender, RoutedEventArgs e)
        {
            ApplicationContext.Default.SwitchToDark();
        }

        private void ToggleSlider_Unchecked(object sender, RoutedEventArgs e)
        {
            ApplicationContext.Default.SwitchToLight();
        }

        private void MapControl_Loaded(object sender, RoutedEventArgs e)
        {
            _originalMapHeight = MapControl.Height;
            _originalMapWidth = MapControl.Width;
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
            OperationPanel.Visibility = Visibility.Collapsed;
        }

        private void ToggleButton_Unchecked(object sender, RoutedEventArgs e)
        {
            WindowStyle = WindowStyle.SingleBorderWindow;
            OperationPanel.Visibility = Visibility.Visible;
        }

        protected override void OnActivated(EventArgs e)
        {
            base.OnActivated(e);
        }
    }
}