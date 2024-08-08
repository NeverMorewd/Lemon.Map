using System.Configuration;
using System.Data;
using System.Windows;

namespace WpfTheme
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            //Current.Resources.MergedDictionaries.Add(new ResourceDictionary {  Source = new Uri("pack://application:,,,/WpfTheme;component/Color.Dark.xaml") });
        }
    }

}
