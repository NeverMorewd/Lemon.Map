using System.Configuration;
using System.Data;
using System.Windows;
using System.Windows.Media.Animation;

namespace Lemon.Map.Wpf
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            //Current.Resources.MergedDictionaries.Add(ApplicationContext.Default.LightResource);
            ApplicationContext.Default.SwitchToLight();
            Timeline.DesiredFrameRateProperty.OverrideMetadata(
                typeof(Timeline),
                new FrameworkPropertyMetadata { DefaultValue = 60 });
        }
    }

}
