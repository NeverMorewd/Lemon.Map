using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows;

namespace Lemon.Map.Wpf.Controls
{
    /// <summary>
    /// Flag
    /// </summary>
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

        public static readonly DependencyProperty BorderThicknessProperty =
            DependencyProperty.Register("BorderThickness", typeof(double), typeof(Flag), new PropertyMetadata(1.0, OnVisualPropertyChanged));

        public double BorderThickness
        {
            get { return (double)GetValue(BorderThicknessProperty); }
            set { SetValue(BorderThicknessProperty, value); }
        }

        public static readonly DependencyProperty BorderBrushProperty =
            DependencyProperty.Register("BorderBrush", typeof(Brush), typeof(Flag), new PropertyMetadata(Brushes.Black, OnVisualPropertyChanged));

        public Brush BorderBrush
        {
            get { return (Brush)GetValue(BorderBrushProperty); }
            set { SetValue(BorderBrushProperty, value); }
        }

        public static readonly DependencyProperty FillBrushProperty =
            DependencyProperty.Register("FillBrush", typeof(Brush), typeof(Flag), new PropertyMetadata(Brushes.Red, OnVisualPropertyChanged));

        public Brush FillBrush
        {
            get { return (Brush)GetValue(FillBrushProperty); }
            set { SetValue(FillBrushProperty, value); }
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

            Pen flagPen = new(BorderBrush, BorderThickness)
            {
                DashCap = PenLineCap.Flat,
                EndLineCap = PenLineCap.Round,
                StartLineCap = PenLineCap.Round,
            };

            double poleWidth = width * 0.1;
            double poleHeight = height * 0.8;

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
            drawingContext.DrawGeometry(FillBrush, flagPen, geometry);

        }
    }
}
