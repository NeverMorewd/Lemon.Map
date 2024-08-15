using Lemon.Map.Model;
using ReactiveUI;
using System.Diagnostics;
using System.Drawing;
using System.Reactive.Linq;

namespace Lemon.Map.ViewModel
{
    public class MainViewModel:ReactiveObject
    {
        public MainViewModel() 
        {
            Random random = new();
            Observable.Interval(TimeSpan.FromSeconds(1))
                .ObserveOn(RxApp.MainThreadScheduler)
                //.ObserveOn(new SynchronizationContextScheduler(SynchronizationContext.Current!))
                .Subscribe(_ =>
                {
                    var id = Environment.CurrentManagedThreadId;

                    if (Regions != null && Regions.Any())
                    {
                        foreach (var region in Regions)
                        {
                            region.Name = new Random().Next().ToString();
                            region.BackgroundColor = Color.FromArgb(255, random.Next(0, 255), random.Next(0, 255), random.Next(0, 255));
                        }
                      
                    }
                });

        }
        private IEnumerable<RegionModel>? regions;
        public IEnumerable<RegionModel>? Regions
        {
            get => regions;
            set
            {
                this.RaiseAndSetIfChanged(ref regions, value);
                if (regions != null) 
                {
                    ///for test
                    foreach (var region in regions)
                    {
                        region.Name = new Random().Next().ToString();
                        Debug.WriteLine(region.ToString());
                    }
                }
            }
        }
    }
}
