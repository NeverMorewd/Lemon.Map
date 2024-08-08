using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Media3D;
using System.Windows.Media;
using System.Windows;

namespace Lemon.Map.Wpf.Controls
{
    public class GridRuler : Canvas
    {
        public static readonly DependencyProperty GridSizeProperty =
            DependencyProperty.Register("GridSize", typeof(double), typeof(GridRuler), new PropertyMetadata(20.0, OnGridSizeChanged));

        public double GridSize
        {
            get => (double)GetValue(GridSizeProperty);
            set => SetValue(GridSizeProperty, value);
        }



        public Brush LineBrush
        {
            get { return (Brush)GetValue(LineBrushProperty); }
            set { SetValue(LineBrushProperty, value); }
        }

        // Using a DependencyProperty as the backing store for LineBrush.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty LineBrushProperty =
            DependencyProperty.Register("LineBrush", typeof(Brush), typeof(GridRuler), new PropertyMetadata(Brushes.LightGray));



        public GridRuler()
        {

        }

        private static void OnGridSizeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var ruler = (GridRuler)d;
            ruler.InvalidateVisual();
        }

        protected override void OnRender(DrawingContext dc)
        {
            base.OnRender(dc);
            DrawGrid(dc);
        }

        private void DrawGrid(DrawingContext dc)
        {
            var gridPen = new Pen(LineBrush, 1);
            double step = GridSize;

            for (double x = 0; x < ActualWidth; x += step)
            {
                dc.DrawLine(gridPen, new Point(x, 0), new Point(x, ActualHeight - step));
            }

            for (double y = 0; y < ActualHeight; y += step)
            {
                dc.DrawLine(gridPen, new Point(0, y), new Point(ActualWidth, y));
            }
        }
    }
}
