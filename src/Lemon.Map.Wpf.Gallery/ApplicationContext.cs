using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Lemon.Map.Wpf.Gallery
{
    public class ApplicationContext
    {
        static ApplicationContext()
        {
            Default = new ApplicationContext();
        }
        private ApplicationContext() 
        {
            DarkResource = new()
            {
                Source = new Uri("pack://application:,,,/Lemon.Map.Wpf.Gallery;component/Resources/Brush.Dark.xaml")
            };

            LightResource = new()
            {
                Source = new Uri("pack://application:,,,/Lemon.Map.Wpf.Gallery;component/Resources/Brush.Light.xaml")
            };
        }

        public static ApplicationContext Default
        {
            get;
            private set;
        }

        public ResourceDictionary DarkResource
        {
            get;
            private set;
        }
        public ResourceDictionary LightResource
        {
            get;
            private set;
        }

        public void SwitchToDark()
        {
            Application.Current.Resources.MergedDictionaries[0].Source = new Uri("pack://application:,,,/Lemon.Map.Wpf.Gallery;component/Resources/Brush.Dark.xaml");
            Application.Current.MainWindow?.InvalidateVisual();
        }
        public void SwitchToLight() 
        {
            Application.Current.Resources.MergedDictionaries[0].Source = new Uri("pack://application:,,,/Lemon.Map.Wpf.Gallery;component/Resources/Brush.Light.xaml");
            Application.Current.MainWindow?.InvalidateVisual();
        }
    }
}
