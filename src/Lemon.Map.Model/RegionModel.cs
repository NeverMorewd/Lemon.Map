using ReactiveUI;
using System.Drawing;

namespace Lemon.Map.Model
{
    public class RegionModel : ReactiveObject
    {
        private string _name;
        public string Name
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
        public string Description { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public double Top { get; set; }
        public double Left { get; set; }
        public double Height { get; set; }
        public double Width { get; set; }

        private Color _backgroundColor = Color.Orange;
        public Color BackgroundColor 
        {
            get
            {
                return _backgroundColor;
            }
            set
            {
                this.RaiseAndSetIfChanged(ref _backgroundColor, value);
            }
        }

        public override string ToString()
        {
            return $"{nameof(Name)}:{Name}; " +
                $"{nameof(Description)}:{Description}; " +
                $"{nameof(BackgroundColor)}:{BackgroundColor}; " +
                $"{nameof(Top)}:{Top}; " +
                $"{nameof(Left)}:{Left}" +
                $"{nameof(Height)}:{Height}" +
                $"{nameof(Width)}:{Width}";
        }
    }
}
