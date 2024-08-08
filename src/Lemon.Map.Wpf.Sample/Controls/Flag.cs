using System.Windows;
using System.Windows.Media;

namespace Lemon.Map.Wpf.Controls
{
    public class Flag : FrameworkElement
    {
        static Flag()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(Flag), new FrameworkPropertyMetadata(typeof(Flag)));
        }
        public Flag()
        {
            UseLayoutRounding = true;
            SnapsToDevicePixels = true;
        }

        #region Dependency Properties

        public static readonly DependencyProperty LineThicknessProperty =
            DependencyProperty.Register("LineThickness", typeof(double), typeof(Flag), new PropertyMetadata(1.0, OnVisualPropertyChanged));

        public double LineThickness
        {
            get { return (double)GetValue(LineThicknessProperty); }
            set { SetValue(LineThicknessProperty, value); }
        }

        public static readonly DependencyProperty LineColorProperty =
            DependencyProperty.Register("LineColor", typeof(Brush), typeof(Flag), new PropertyMetadata(Brushes.Black, OnVisualPropertyChanged));

        public Brush LineColor
        {
            get { return (Brush)GetValue(LineColorProperty); }
            set { SetValue(LineColorProperty, value); }
        }

        public static readonly DependencyProperty FillColorProperty =
            DependencyProperty.Register("FillColor", typeof(Brush), typeof(Flag), new PropertyMetadata(Brushes.Red, OnVisualPropertyChanged));

        public Brush FillColor
        {
            get { return (Brush)GetValue(FillColorProperty); }
            set { SetValue(FillColorProperty, value); }
        }

        private static void OnVisualPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is Flag flag)
            {
                flag.InvalidateVisual();
            }
        }
        #endregion


        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);
            double width = ActualWidth;
            double height = ActualHeight;

            Pen flagPen = new(LineColor, LineThickness)
            {
                DashCap = PenLineCap.Flat,
                EndLineCap = PenLineCap.Round,
                StartLineCap = PenLineCap.Round,
            };

            double poleWidth = width * 0.1;
            double poleHeight = height * 0.8;
            //double poleX = (width - poleWidth) / 2;
            double poleX = 0 + 5;
            double poleY = 0 + 5;
            drawingContext.DrawLine(flagPen, new Point(poleX, poleY), new Point(poleX, poleHeight));


            double flagWidth = width * 0.6;
            double flagHeight = height * 0.4;
            double flagX = poleX + poleWidth;
            double flagY = poleY;
            StreamGeometry geometry = new();
            using (StreamGeometryContext context = geometry.Open())
            {
                context.BeginFigure(new Point(flagX, flagY), true, true);
                context.LineTo(new Point(flagX + flagWidth, flagY), true, true);
                context.LineTo(new Point(flagX + flagWidth, flagY + flagHeight), true, true);
                context.LineTo(new Point(flagX, flagY + flagHeight), true, true);
            }
            geometry.Freeze();
            //flagPen.DashCap = PenLineCap.Flat;
            drawingContext.DrawGeometry(FillColor, flagPen, geometry);

        }
    }
}
