﻿using ReactiveUI;
using System;
using System.Collections;
using System.Collections.ObjectModel;
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

        private ObservableCollection<FlagModel> _flags;
        public ObservableCollection<FlagModel> Flags
        {
            get => _flags;
            set
            {
                this.RaiseAndSetIfChanged(ref _flags, value);
            }
        }

        public override string ToString()
        {
            return $"{nameof(Name)}:{Name};{Environment.NewLine}" +
                $"{nameof(Description)}:{Description};{Environment.NewLine}" +
                $"{nameof(BackgroundColor)}:{BackgroundColor};{Environment.NewLine}" +
                $"{nameof(Top)}:{Top};{Environment.NewLine}" +
                $"{nameof(Left)}:{Left};{Environment.NewLine}" +
                $"{nameof(Height)}:{Height};{Environment.NewLine}" +
                $"{nameof(Width)}:{Width};{Environment.NewLine}";
        }
    }
}
