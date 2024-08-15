using Lemon.Map.Wpf.Controls;
using ReactiveUI;
using Splat;
using System.Configuration;
using System.Data;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Windows;
using System.Windows.Media.Animation;

namespace Lemon.Map.Wpf.Gallery
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
            Locator.CurrentMutable.InitializeReactiveUI();
            RxApp.MainThreadScheduler = new SynchronizationContextScheduler(SynchronizationContext.Current!);
            ApplicationContext.Default.SwitchToLight();
            Timeline.DesiredFrameRateProperty.OverrideMetadata(
                typeof(Timeline),
                new FrameworkPropertyMetadata { DefaultValue = 60 });

        }
    }

}
