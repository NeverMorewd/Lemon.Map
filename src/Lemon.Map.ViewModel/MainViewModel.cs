using Lemon.Map.Model;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System.Drawing;
using System.Reactive;
using System.Reactive.Linq;

namespace Lemon.Map.ViewModel
{
    public class MainViewModel:ReactiveObject
    {
        private const int CrazyInterval = 1000;
        public MainViewModel() 
        {
            Random random = new();
            RegionCommand = ReactiveCommand.Create<object>(param => 
            {
                if (param is RegionModel region)
                {
                    ClickedRegion = region;
                }
            });

            Observable.Interval(TimeSpan.FromMilliseconds(CrazyInterval))
                .ObserveOn(RxApp.MainThreadScheduler)
                .Subscribe( _ =>
                {
                    try
                    {
                        var id = Environment.CurrentManagedThreadId;

                        if (Regions != null && Regions.Any())
                        {
                            foreach (var region in Regions)
                            {
                                region.BackgroundColor = Color.FromArgb(255, random.Next(0, 255), random.Next(0, 255), random.Next(0, 255));
                            }
                        }
                    }
                    catch 
                    {
                        
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
                if (regions != null && regions.Any())
                {
                    foreach (var region in regions)
                    {
                        Console.WriteLine(region);
                        region.Flags.CollectionChanged += (s,e)=> 
                        {

                        };
                    }
                }
            }
        }

        private void Flags_CollectionChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            //
        }

        [Reactive]
        public RegionModel? ClickedRegion
        {
            get;
            set;
        }

        public ReactiveCommand<object,Unit> RegionCommand
        {
            get;
            set;
        }
    }
}
