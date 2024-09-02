using ReactiveUI;
using Splat;
using System.Reactive.Concurrency;
using System.Windows;
using System.Windows.Media;
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
            RenderOptions.ProcessRenderMode = System.Windows.Interop.RenderMode.SoftwareOnly;

            Application.Current.Dispatcher.UnhandledException += Dispatcher_UnhandledException;
            //Current.Resources.MergedDictionaries.Add(ApplicationContext.Default.LightResource);
            Locator.CurrentMutable.InitializeReactiveUI();
            RxApp.MainThreadScheduler = new SynchronizationContextScheduler(SynchronizationContext.Current!);
            ApplicationContext.Default.SwitchToLight();
            Timeline.DesiredFrameRateProperty.OverrideMetadata(
                typeof(Timeline),
                new FrameworkPropertyMetadata { DefaultValue = 60 });

        }

        private void Dispatcher_UnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            //
        }
    }

}
