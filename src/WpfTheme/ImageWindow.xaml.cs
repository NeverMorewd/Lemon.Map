using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace WpfTheme
{
    /// <summary>
    /// Interaction logic for ImageWindow.xaml
    /// </summary>
    public partial class ImageWindow : Window
    {
        private double zoomFactor = 1.0;
        private const double zoomStep = 0.1;

        public ImageWindow()
        {
            InitializeComponent();
            // 设置初始图片
            image.Source = new BitmapImage(new Uri("test.png", UriKind.Relative));
            scrollViewer.HorizontalScrollBarVisibility = ScrollBarVisibility.Auto;
            scrollViewer.VerticalScrollBarVisibility = ScrollBarVisibility.Auto;
        }

        private void ZoomSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            ApplyZoom(e.NewValue);
        }

        private void ApplyZoom(double zoomFactor)
        {
            if (image == null) return;
            // 更新 ScaleTransform 的缩放因子
            scaleTransform.ScaleX = zoomFactor;
            scaleTransform.ScaleY = zoomFactor;

            // 计算图片的实际大小
            double imageWidth = image.Source.Width * zoomFactor;
            double imageHeight = image.Source.Height * zoomFactor;

            // 更新 ScrollViewer 的偏移量以确保图片居中
            double offsetX = Math.Max(0, (imageWidth - scrollViewer.ViewportWidth) / 2);
            double offsetY = Math.Max(0, (imageHeight - scrollViewer.ViewportHeight) / 2);

            scrollViewer.ScrollToHorizontalOffset(offsetX);
            scrollViewer.ScrollToVerticalOffset(offsetY);
        }
    }
}
