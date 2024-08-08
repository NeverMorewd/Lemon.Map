using Lemon.Map.Model;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;

namespace Lemon.Map.Wpf.Controls
{
    /// <summary>
    /// Map
    /// </summary>
    [TemplatePart(Name = PART_CONTENTPRESENTER_NAME, Type = typeof(ContentPresenter))]
    [TemplatePart(Name = PART_CURSORTEXT_NAME, Type = typeof(TextBlock))]
    public class Map : Control
    {
        private const string PART_CONTENTPRESENTER_NAME = "PART_Presenter";
        private const string PART_CURSORTEXT_NAME = "PART_CursorText";
        static Map()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(Map), new FrameworkPropertyMetadata(typeof(Map)));
        }

        #region Regions (ReadOnly)
        public static readonly DependencyPropertyKey RegionsPropertyKey =
            DependencyProperty.RegisterReadOnly(
                "Regions",
                typeof(IEnumerable<RegionModel>),
                typeof(Map),
                new PropertyMetadata(null));

        public static readonly DependencyProperty RegionsProperty = RegionsPropertyKey.DependencyProperty;
        public IEnumerable<RegionModel>? Regions
        {
            get { return (IEnumerable<RegionModel>?)GetValue(RegionsProperty); }
        }

        public void SetRegionsProperty(IEnumerable<RegionModel>? value)
        {
            SetValue(RegionsPropertyKey, value);
        }
        #endregion

        public ContentPresenter? MapPresenter
        {
            get;
            set;
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            MapPresenter = GetTemplateChild(PART_CONTENTPRESENTER_NAME) as ContentPresenter;
            MapPresenter!.ContentTemplate = FindResource(MapType.ToString()) as MapDataTemplate;
            UpdateRegions();
            MouseMove += Map_MouseMove;
        }

        private void Map_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (GetTemplateChild(PART_CURSORTEXT_NAME) is TextBlock textBlock)
            {
                //textBlock.Text = e.GetPosition(this).ToString();
                textBlock.Text = Mouse.GetPosition(this).ToString();
            }
        }

        public MapType MapType
        {
            get { return (MapType)GetValue(MapTypeProperty); }
            set { SetValue(MapTypeProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MapType.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MapTypeProperty =
            DependencyProperty.Register("MapType",
                typeof(MapType),
                typeof(Map),
                new PropertyMetadata(MapType.WORLD_ALPHA, new PropertyChangedCallback(OnMapTypeChanged)));

        private static void OnMapTypeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is Map map)
            {
                if (e.NewValue != e.OldValue && map.MapPresenter != null)
                {
                    map.MapPresenter.ContentTemplate = map.FindResource(e.NewValue.ToString()) as MapDataTemplate;
                    map.UpdateRegions();
                }
            }
        }


        private void UpdateRegions()
        {
            ///https://stackoverflow.com/questions/25977141/wpf-when-changing-a-contentcontrols-content-the-new-content-object-is-not-fir
            Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Render, () =>
            {
                var regions = VisualTreeHelperExtension.FindVisualChildren<Region>(this);
                if (regions != null && regions.Count != 0)
                {
                    SetRegionsProperty(regions.Select(rb => new RegionModel
                    {
                        Name = rb.Content?.ToString(),
                        Width = rb.ActualWidth,
                        Height = rb.ActualHeight,
                        Left = rb.TransformToAncestor(this).Transform(new Point(0, 0)).X,
                        Top = rb.TransformToAncestor(this).Transform(new Point(0, 0)).Y,
                    }));
                }
            });
        }
    }
}