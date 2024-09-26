using ReactiveUI;
using System.Drawing;

namespace Lemon.Map.Model
{
    public class FlagModel : ReactiveObject
    {
        private string _flagContent;
        public string FlagContent
        {
            get => _flagContent;
            set
            {
                this.RaiseAndSetIfChanged(ref _flagContent, value);
            }
        }

        private Color _flagColor;
        public Color FlagColor
        {
            get => _flagColor;
            set
            {
                this.RaiseAndSetIfChanged(ref _flagColor,value);
            }
        }
        private Color _flagBorderColor;
        public Color FlagBorderColor
        {
            get => _flagBorderColor;
            set
            {
                this.RaiseAndSetIfChanged(ref _flagBorderColor, value);
            }
        }
        private Color _poleColor;
        public Color PoleColor
        {
            get => _poleColor;
            set
            {
                this.RaiseAndSetIfChanged(ref _poleColor, value);
            }
        }
    }
}
