using Avalonia.Data;
using Lemon.Map.Model;
using Lemon.Map.ViewModel;
using System;
using System.Collections.ObjectModel;
using System.Reactive.Linq;
using System.Threading.Tasks;

namespace Lemon.Map.Avaloniaui.Sample.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
#pragma warning disable CA1822 // Mark members as static
        public string Greeting => "Welcome to Avalonia!";
#pragma warning restore CA1822 // Mark members as static

        public MainWindowViewModel() 
        {
            Regions = [new RegionModel { Name = "test" }];

            Observable.Interval(TimeSpan.FromSeconds(2)).Subscribe(x => 
            {
                foreach (var r in Regions)
                {
                    r.Name = "new";
                }
            });
        }

        public ObservableCollection<RegionModel> Regions { get; set; }
    }
}
