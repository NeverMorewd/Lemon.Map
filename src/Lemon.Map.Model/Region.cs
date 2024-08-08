using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lemon.Map.Model
{
    public class RegionModel : ReactiveObject
    {
        private string? _name;
        public string? Name
        {
            get
            {
                return _name;
            }
            set
            {
                this.RaiseAndSetIfChanged(ref _name, value);
            }
        }
        public string? Description { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public double Top { get; set; }
        public double Left { get; set; }
        public double Height { get; set; }
        public double Width { get; set; }

        public override string ToString()
        {
            return $"{nameof(Name)}:{Name}; " +
                $"{nameof(Description)}:{Description}; " +
                $"{nameof(Top)}:{Top}; " +
                $"{nameof(Left)}:{Left}" +
                $"{nameof(Height)}:{Height}" +
                $"{nameof(Width)}:{Width}";
        }
    }
}
