using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Shell;

namespace Lemon.Map.Wpf.Gallery.Controls
{
    [TemplatePart(Name = PART_LayoutRoot_Name, Type = typeof(Grid))]
    public class LemonWindow : Window
    {
        public const int WM_NCACTIVATE = 0x0086;
        private const string PART_LayoutRoot_Name = "PART_LayoutRoot";
        private readonly nint _trueValue = new(1);
        private Grid? _layoutRoot;

        static LemonWindow()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(LemonWindow), new FrameworkPropertyMetadata(typeof(LemonWindow)));
        }
        public LemonWindow()
        {
            CommandBindings.Add(new CommandBinding(SystemCommands.CloseWindowCommand, CloseWindow));
            CommandBindings.Add(new CommandBinding(SystemCommands.MaximizeWindowCommand, MaximizeWindow, CanResizeWindow));
            CommandBindings.Add(new CommandBinding(SystemCommands.MinimizeWindowCommand, MinimizeWindow, CanMinimizeWindow));
            CommandBindings.Add(new CommandBinding(SystemCommands.RestoreWindowCommand, RestoreWindow, CanResizeWindow));
            CommandBindings.Add(new CommandBinding(SystemCommands.ShowSystemMenuCommand, ShowSystemMenu));
        }

        #region ChromeBackground

        public Brush ChromeBackground
        {
            get { return (Brush)GetValue(ChromeBackgroundProperty); }
            set { SetValue(ChromeBackgroundProperty, value); }
        }

        public static readonly DependencyProperty ChromeBackgroundProperty =
            DependencyProperty.Register("ChromeBackground", typeof(Brush), typeof(LemonWindow), new PropertyMetadata(null));

        #endregion

        #region ShowChromeSeparator
        public bool ShowChromeSeparator
        {
            get { return (bool)GetValue(ShowChromeSeparatorProperty); }
            set { SetValue(ShowChromeSeparatorProperty, value); }
        }

        public static readonly DependencyProperty ShowChromeSeparatorProperty =
            DependencyProperty.Register("ShowChromeSeparator", typeof(bool), typeof(LemonWindow), new PropertyMetadata(false, OnShowChromeSeparatorChanged));

        private static void OnShowChromeSeparatorChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            var oldValue = (bool)args.OldValue;
            var newValue = (bool)args.NewValue;
            if (oldValue == newValue)
            {
                return;
            }
            var target = obj as LemonWindow;
            target?.OnShowChromeSeparatorChanged(oldValue, newValue);
        }

        protected virtual void OnShowChromeSeparatorChanged(bool oldValue, bool newValue)
        {
            if (_layoutRoot != null)
            {
                _layoutRoot.ShowGridLines = newValue;
            }
        }
        #endregion

        #region ChromeHeight
        public static readonly DependencyProperty WindowChromeHeightProperty =
            DependencyProperty.Register(nameof(WindowChromeHeight),
                typeof(double),
                typeof(LemonWindow),
                new PropertyMetadata(SystemParameters.WindowNonClientFrameThickness.Top));
        public double WindowChromeHeight
        {
            get { return (double)GetValue(WindowChromeHeightProperty); }
            set { SetValue(WindowChromeHeightProperty, value); }
        }
        #endregion

        #region Custom TitleBar
        public static readonly DependencyProperty TitleBarProperty =
            DependencyProperty.Register(nameof(TitleBar),
                typeof(LemonWindowTitleBar),
                typeof(LemonWindow),
                new PropertyMetadata(default(LemonWindowTitleBar),
                    OnTitleBarChanged));
        public LemonWindowTitleBar TitleBar
        {
            get => (LemonWindowTitleBar)GetValue(TitleBarProperty);
            set => SetValue(TitleBarProperty, value);
        }

        private static void OnTitleBarChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            var oldValue = (LemonWindowTitleBar)args.OldValue;
            var newValue = (LemonWindowTitleBar)args.NewValue;
            if (oldValue == newValue)
            {
                return;
            }
            var target = obj as LemonWindow;
            target?.OnTitleBarChanged(oldValue, newValue);
        }

        protected virtual void OnTitleBarChanged(LemonWindowTitleBar oldValue, LemonWindowTitleBar newValue)
        {

        }
        #endregion

        #region IsNonClientActive-Readonly
        private static readonly DependencyPropertyKey IsNonClientActivePropertyKey =
             DependencyProperty.RegisterReadOnly(nameof(IsNonClientActive),
                 typeof(bool), typeof(LemonWindow),
                 new FrameworkPropertyMetadata(false));

        public static readonly DependencyProperty IsNonClientActiveProperty
            = IsNonClientActivePropertyKey.DependencyProperty;
        public bool IsNonClientActive
        {
            get
            {
                return (bool)GetValue(IsNonClientActiveProperty);
            }
        }
        #endregion

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            _layoutRoot = (Grid)GetTemplateChild("PART_LayoutRoot")!;
            if (_layoutRoot == null)
            {
                throw new Exception($"Can not find {PART_LayoutRoot_Name} in template!");
            }
            _layoutRoot.ShowGridLines = ShowChromeSeparator;
            if (ChromeBackground == null)
            {
                ChromeBackground = Background;
            }
        }

        protected override void OnSourceInitialized(EventArgs e)
        {
            base.OnSourceInitialized(e);
            if (SizeToContent == SizeToContent.WidthAndHeight && WindowChrome.GetWindowChrome(this) != null)
            {
                InvalidateMeasure();
            }
            IntPtr handle = new WindowInteropHelper(this).Handle;
            HwndSource.FromHwnd(handle).AddHook(new HwndSourceHook(WndProc));
        }

        protected override void OnActivated(EventArgs e)
        {
            base.OnActivated(e);
            SetValue(IsNonClientActivePropertyKey, true);
        }

        protected override void OnDeactivated(EventArgs e)
        {
            base.OnDeactivated(e);
            SetValue(IsNonClientActivePropertyKey, false);
        }

        private IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            if (msg == WM_NCACTIVATE)
                SetValue(IsNonClientActivePropertyKey, wParam == _trueValue);

            return IntPtr.Zero;
        }

        private void CanResizeWindow(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = ResizeMode == ResizeMode.CanResize
                || ResizeMode == ResizeMode.CanResizeWithGrip;
        }

        private void CanMinimizeWindow(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = ResizeMode != ResizeMode.NoResize;
        }

        private void CloseWindow(object sender, ExecutedRoutedEventArgs e)
        {
            Close();
        }

        private void MaximizeWindow(object sender, ExecutedRoutedEventArgs e)
        {
            SystemCommands.MaximizeWindow(this);
            e.Handled = true;
        }

        private void MinimizeWindow(object sender, ExecutedRoutedEventArgs e)
        {
            SystemCommands.MinimizeWindow(this);
            e.Handled = true;
        }

        private void RestoreWindow(object sender, ExecutedRoutedEventArgs e)
        {
            SystemCommands.RestoreWindow(this);
            e.Handled = true;
        }

        private void ShowSystemMenu(object sender, ExecutedRoutedEventArgs e)
        {
            Point point = PointToScreen(new Point(0, 0));
            var dipScale = WindowParametersUtil.GetDpi() / 96d;
            if (WindowState == WindowState.Maximized)
            {
                point.X += (SystemParameters.WindowNonClientFrameThickness.Left +
                            WindowParametersUtil.PaddedBorderThickness.Left)
                            * dipScale;
                point.Y += (SystemParameters.WindowNonClientFrameThickness.Top +
                            WindowParametersUtil.PaddedBorderThickness.Top +
                            SystemParameters.WindowResizeBorderThickness.Top -
                            BorderThickness.Top)
                            * dipScale;
            }
            else
            {
                point.X += BorderThickness.Left * dipScale;
                point.Y += SystemParameters.WindowNonClientFrameThickness.Top * dipScale;
            }

            CompositionTarget compositionTarget = PresentationSource.FromVisual(this).CompositionTarget;
            SystemCommands.ShowSystemMenu(this, compositionTarget.TransformFromDevice.Transform(point));
            e.Handled = true;
        }
    }
}
