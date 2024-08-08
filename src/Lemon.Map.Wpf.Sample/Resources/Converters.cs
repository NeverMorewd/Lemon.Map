using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Lemon.Map.Wpf.Resources
{
    public static class Converters
    {
        public static readonly NullToVisibilityConverter NullToVisibility = new();

        public class NullToVisibilityConverter : DependencyObject, IValueConverter
        {
            public static readonly DependencyProperty EmptyValueProperty =
                DependencyProperty.Register(nameof(EmptyValue), typeof(Visibility), typeof(NullToVisibilityConverter), new PropertyMetadata(default(Visibility)));

            public static readonly DependencyProperty NotEmptyValueProperty =
                DependencyProperty.Register(nameof(NotEmptyValue), typeof(Visibility), typeof(NullToVisibilityConverter), new PropertyMetadata(default(Visibility)));

            public NullToVisibilityConverter()
            {
                EmptyValue = Visibility.Collapsed;
                NotEmptyValue = Visibility.Visible;
            }

            public Visibility EmptyValue
            {
                get => (Visibility)GetValue(EmptyValueProperty);
                set => SetValue(EmptyValueProperty, value);
            }

            public Visibility NotEmptyValue
            {
                get => (Visibility)GetValue(NotEmptyValueProperty);
                set => SetValue(NotEmptyValueProperty, value);
            }

            public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            {
                return value == null ? EmptyValue : NotEmptyValue;
            }

            public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            {
                throw new NotImplementedException();
            }
        }
    }


}
