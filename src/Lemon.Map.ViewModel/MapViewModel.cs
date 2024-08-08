using Lemon.Map.Model;
using ReactiveUI;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace Lemon.Map.ViewModel
{
    public class MainViewModel:ReactiveObject,IObserver<RegionModel>
    {
        public MainViewModel() 
        {
            TestRegions = [new RegionModel { Name = "test" }];

            //Observable.Interval(TimeSpan.FromSeconds(2)).Subscribe(x =>
            //{
            //    foreach (var r in TestRegions)
            //    {
            //        r.Name = new Random().Next(1111,9999).ToString();
            //    }
            //});
            //Thread thread = new Thread(AddItemsToCollection);
            //thread.Start();

            Thread thread2 = new Thread(Update);
            thread2.Start();
        }
        private void Update()
        {
            foreach (var r in TestRegions)
            {
                r.Name = new Random().Next(1111, 9999).ToString();
            }
        }
        private void AddItemsToCollection()
        {
            for (int i = 0; i < 10; i++)
            {
                Thread.Sleep(100);
                TestRegions.Add(new RegionModel { Name = "add" });
            }
        }
        private void Regions_CollectionChanged(object? sender, 
            System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {

        }
        public ObservableCollection<RegionModel> TestRegions { get; set; }
        private IEnumerable<RegionModel>? regions;
        public IEnumerable<RegionModel>? Regions
        {
            get => regions;
            set
            {
                this.RaiseAndSetIfChanged(ref regions, value);
                if (regions != null) 
                {
                    foreach (var region in regions)
                    {
                        Debug.WriteLine(region.ToString());
                    }
                }
            }
        }
        //private ObservableCollection<Region> regions = new ObservableCollection<Region>();
        //public ObservableCollection<Region> Regions
        //{
        //    get => regions;
        //    set
        //    {
        //        this.RaiseAndSetIfChanged(ref regions,value);
        //    }
        //} 
        public void OnCompleted()
        {
            Console.WriteLine("OnCompleted");
        }

        public void OnError(Exception error)
        {
            Console.WriteLine(error);
        }

        public void OnNext(RegionModel value)
        {
           Console.WriteLine(value.ToString());
        }
    }
}
