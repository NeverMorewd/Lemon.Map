using Lemon.Map.Model;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Lemon.Map.Wpf.Controls
{
    [TemplatePart(Name = PART_CONTENTPRESENTER_NAME, Type = typeof(ContentPresenter))]
    [TemplatePart(Name = PART_CONTENTPOPUP_NAME, Type = typeof(Popup))]
    public class Region : ButtonBase
    {
        private const string PART_CONTENTPRESENTER_NAME = "PART_ContentPresenter";
        private const string PART_CONTENTPOPUP_NAME = "PART_ContentPopup";

        private ContentPresenter? _contentPresenter;
        private Popup? _contentPopup;
        private bool _signing;
        private Point _lastContextMenuPosition;
        static Region()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(Region), new FrameworkPropertyMetadata(typeof(Region)));
        }

        public Region()
        {
            AddHandler(PreviewMouseLeftButtonUpEvent, new RoutedEventHandler(MouseLeftButtonUpEventHandler), true);
        }

        private void MouseLeftButtonUpEventHandler(object sender, RoutedEventArgs e)
        {
            Debug.WriteLine("MouseLeftButtonUpEventHandler");
            IsPressed = true;
        }


        public Geometry RegionBoundary
        {
            get { return (Geometry)GetValue(RegionBoundaryProperty); }
            set { SetValue(RegionBoundaryProperty, value); }
        }

        // Using a DependencyProperty as the backing store for RegionBoundary.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty RegionBoundaryProperty =
            DependencyProperty.Register("RegionBoundary", typeof(Geometry), typeof(Region), new PropertyMetadata(null, RegionBoundaryChangedCallBack));

        private static void RegionBoundaryChangedCallBack(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is Region region)
            {
                region.UpdateClip();
            }
        }

        public static readonly DependencyPropertyKey ActualBackgroundPropertyKey =
            DependencyProperty.RegisterReadOnly(
                "ActualBackground",
                typeof(Brush),
                typeof(Region),
                new PropertyMetadata(null));

        public static readonly DependencyProperty ActualBackgroundProperty =
            ActualBackgroundPropertyKey.DependencyProperty;
        public Brush ActualBackground
        {
            get { return (Brush)GetValue(ActualBackgroundProperty); }
        }

        private void SetActualBackgroundProperty(Brush? value)
        {
            SetValue(ActualBackgroundPropertyKey, value);
            InvalidateVisual();
        }


        public Brush MouseOverBackground
        {
            get { return (Brush)GetValue(MouseOverBackgroundProperty); }
            set { SetValue(MouseOverBackgroundProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MouseOverBackground.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MouseOverBackgroundProperty =
            DependencyProperty.Register("MouseOverBackground", typeof(Brush), typeof(Region), new PropertyMetadata(null));



        public Brush MousePressBackground
        {
            get { return (Brush)GetValue(MousePressBackgroundProperty); }
            set { SetValue(MousePressBackgroundProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MousePressBackground.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MousePressBackgroundProperty =
            DependencyProperty.Register("MousePressBackground", typeof(Brush), typeof(Region), new PropertyMetadata(null));




        public double ContentHorizontalOffset
        {
            get { return (double)GetValue(ContentHorizontalOffsetProperty); }
            set { SetValue(ContentHorizontalOffsetProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ContentHorizontalOffset.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ContentHorizontalOffsetProperty =
            DependencyProperty.Register("ContentHorizontalOffset", typeof(double), typeof(Region), new PropertyMetadata(0.0));

        public double ContentVerticalOffset
        {
            get { return (double)GetValue(ContentVerticalOffsetProperty); }
            set { SetValue(ContentVerticalOffsetProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ContentVerticalOffset.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ContentVerticalOffsetProperty =
            DependencyProperty.Register("ContentVerticalOffset", typeof(double), typeof(Region), new PropertyMetadata(0.0));




        public bool IsEmpty
        {
            get { return (bool)GetValue(IsEmptyProperty); }
            set { SetValue(IsEmptyProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsEmpty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsEmptyProperty =
            DependencyProperty.Register("IsEmpty", typeof(bool), typeof(Region), new PropertyMetadata(false));




        private void OnFlagColorChanged()
        {
            Flag targetFlag = new()
            {
                Width = 30,
                Height = 30,
                FillBrush = Brushes.Green,
                BorderBrush = Brushes.Red,
                BorderThickness = 1,
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Center
            };
            var map = VisualTreeHelperExtension.FindVisualParent<Map>(this);
            map!.AttachContents.Add(new AttachContentModel { Name="flag",Content = targetFlag,Location = new System.Drawing.Point((int)_lastContextMenuPosition.X,(int)_lastContextMenuPosition.Y) });
        }

        protected override void OnRender(DrawingContext drawingContext)
        {

            base.OnRender(drawingContext);
            if (RegionBoundary == null) return;

            var pen = new Pen(BorderBrush, BorderThickness.Left)
            {
                DashCap = PenLineCap.Flat
            };
            drawingContext.DrawGeometry(ActualBackground,
                pen,
                RegionBoundary);
        }


        protected override Size ArrangeOverride(Size finalSize)
        {
            var size = base.ArrangeOverride(finalSize);
            return size;
        }
        protected override Size MeasureOverride(Size constraint)
        {
            var size = base.MeasureOverride(constraint);
            return size;
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            _contentPresenter = GetTemplateChild(PART_CONTENTPRESENTER_NAME) as ContentPresenter;
            _contentPopup = GetTemplateChild(PART_CONTENTPOPUP_NAME) as Popup;
            _contentPresenter!.SizeChanged += ContentPresenter_SizeChanged;
            BuildDefaultContextMenu();
        }

        private void ContentPresenter_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (RegionBoundary.Bounds.Width < e.NewSize.Width)
            {
                _contentPresenter!.ClearValue(ContentPresenter.ContentProperty);
                _contentPresenter.Visibility = Visibility.Collapsed;

            }
            else
            {
                Rect boundingRect = CalculateBoundingRect();

                var arrangeRect = new Rect(
                        boundingRect.X + (boundingRect.Width - e.NewSize.Width) / 2,
                        boundingRect.Y + (boundingRect.Height - e.NewSize.Height) / 2,
                        e.NewSize.Width,
                        e.NewSize.Height);

                arrangeRect.Offset(ContentHorizontalOffset, ContentVerticalOffset);

                _contentPresenter?.Arrange(arrangeRect);
            }
        }

        private Rect CalculateBoundingRect()
        {
            if (RegionBoundary == null)
            {
                return Rect.Empty;
            }
            Rect boundingRect = RegionBoundary.Bounds;
            return boundingRect;
        }

        protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            base.OnPropertyChanged(e);

            switch (e.Property.Name)
            {
                case nameof(IsMouseOver):
                    SetContentPopupOpen(IsMouseOver);
                    if (_signing)
                    {
                        return;
                    }
                    if (!InvalidateMouseOverBackground())
                    {
                        if (!InvalidateMousePressBackground())
                        {
                            RestoreOriginalBackground();
                        }
                    }
                    InvalidateVisual();
                    break;

                case nameof(IsEmpty):
                    if (e.NewValue is true)
                    {
                        SetActualBackgroundProperty(null);
                        IsHitTestVisible = false;
                    }
                    else
                    {
                        RestoreOriginalBackground();
                        IsHitTestVisible = true;
                    }
                    InvalidateVisual();
                    break;
                case nameof(IsPressed):
                    if (_signing)
                    {
                        return;
                    }
                    Debug.WriteLine($"IsPressed:{IsPressed}");
                    if (!InvalidateMousePressBackground())
                    {
                        RestoreOriginalBackground();
                        InvalidateMouseOverBackground();
                    }
                    InvalidateVisual();
                    break;
                case nameof(Background):
                    if (e.NewValue is not null)
                    {
                        SetActualBackgroundProperty(Background);
                    }
                    break;
                case nameof(BorderBrush):
                    InvalidateVisual();
                    break;
                case nameof(ContextMenu):
                    if (e.NewValue is null)
                    {
                        BuildDefaultContextMenu();
                    }

                    break;
                default:
                    break;
            }
        }
        private void BuildDefaultContextMenu()
        {
            var contextMenu = new ContextMenu();
            var fillColorMenuItem = new MenuItem { Header = "FillColor" };

            var revertMenuItem = new MenuItem
            {
                Header = new TextBlock { Text = "Revert" },
            };
            revertMenuItem.Click += OnFillColorRevertClicked;

            var noneMenuItem = new MenuItem
            {
                Header = new TextBlock { Text = "None" },
                Icon = new Rectangle
                {
                    Fill = Brushes.Transparent,
                    Stroke = Brushes.Black,
                    StrokeThickness = 1,
                    Width = 10,
                    Height = 10
                }
            };
            noneMenuItem.Click += OnFillColorNoneClicked;

            var greenMenuItem = new MenuItem
            {
                Header = new TextBlock { Text = "Green" },
                Icon = new Rectangle
                {
                    Fill = Brushes.Green,
                    Width = 10,
                    Height = 10
                }
            };
            greenMenuItem.Click += OnFillColorGreenClicked;

            var redMenuItem = new MenuItem
            {
                Header = new TextBlock { Text = "Red" },
                Icon = new Rectangle
                {
                    Fill = Brushes.Red,
                    Width = 10,
                    Height = 10
                }
            };
            redMenuItem.Click += OnFillColorRedClicked;

            fillColorMenuItem.Items.Add(noneMenuItem);
            fillColorMenuItem.Items.Add(greenMenuItem);
            fillColorMenuItem.Items.Add(redMenuItem);
            fillColorMenuItem.Items.Add(revertMenuItem);

            var setFlagMenuItem = new MenuItem { Header = "SetFlag" };
            var greenFlagMenuItem = new MenuItem
            {
                Header = new TextBlock { Text = "Green" },
                Icon = new Rectangle
                {
                    Fill = Brushes.Green,
                    Width = 10,
                    Height = 10
                }
            };
            greenFlagMenuItem.Click += OnFlagColorGreenClicked;
            setFlagMenuItem.Items.Add(greenFlagMenuItem);

            contextMenu.Items.Add(fillColorMenuItem);
            contextMenu.Items.Add(setFlagMenuItem);
            ContextMenu = contextMenu;
            ContextMenu.Opened += ContextMenu_Opened;
        }

        private void ContextMenu_Opened(object sender, RoutedEventArgs e)
        {
            var canvas = VisualTreeHelperExtension.FindVisualParent<Map>(this);
            _lastContextMenuPosition = Mouse.GetPosition(canvas);
        }

        private void OnFlagColorGreenClicked(object sender, RoutedEventArgs e)
        {
            OnFlagColorChanged();
        }

        private void OnFillColorNoneClicked(object sender, RoutedEventArgs e)
        {
            SetActualBackgroundProperty(Brushes.Transparent);
            _signing = true;
        }

        private void OnFillColorRedClicked(object sender, RoutedEventArgs e)
        {
            SetActualBackgroundProperty(Brushes.Red);
            _signing = true;
        }

        private void OnFillColorGreenClicked(object sender, RoutedEventArgs e)
        {
            SetActualBackgroundProperty(Brushes.Green);
            _signing = true;
        }

        private void OnFillColorRevertClicked(object sender, RoutedEventArgs e)
        {
            SetActualBackgroundProperty(Background);
            _signing = false;
        }

        private bool InvalidateMouseOverBackground()
        {
            if (IsMouseOver)
            {
                SetActualBackgroundProperty(MouseOverBackground);
                return true;
            }
            return false;
        }

        private bool InvalidateMousePressBackground()
        {
            if (IsPressed)
            {
                SetActualBackgroundProperty(MousePressBackground);
                return true;
            }
            return false;
        }


        private void RestoreOriginalBackground()
        {
            SetActualBackgroundProperty(Background);
        }

        private void UpdateClip()
        {
            Clip = RegionBoundary;
            InvalidateVisual();
        }

        public void SetContentPopupOpen(bool isOpen)
        {
            if (_contentPopup == null)
            {
                return;
            }
            if (Content == null)
            {
                return;
            }
            _contentPopup!.IsOpen = isOpen;
        }
    }
}
