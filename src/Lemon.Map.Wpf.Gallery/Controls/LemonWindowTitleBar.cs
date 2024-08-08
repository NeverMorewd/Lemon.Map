using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;

namespace Lemon.Map.Wpf.Gallery.Controls
{
    public class LemonWindowTitleBar : HeaderedItemsControl
    {
        static LemonWindowTitleBar()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(LemonWindowTitleBar), new FrameworkPropertyMetadata(typeof(LemonWindowTitleBar)));
        }
        public ObservableCollection<object> Options => [];
    }
}
