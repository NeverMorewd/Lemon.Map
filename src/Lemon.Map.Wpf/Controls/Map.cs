using Lemon.Map.Model;
using System.Collections.ObjectModel;
using System.Reactive.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Threading;

namespace Lemon.Map.Wpf.Controls
{
    /// <summary>
    /// Map
    /// </summary>
    [TemplatePart(Name = PART_CONTENTPRESENTER_NAME, Type = typeof(ContentPresenter))]
    [TemplatePart(Name = PART_CURSORTEXT_NAME, Type = typeof(TextBlock))]
    [TemplatePart(Name = PART_ATTACHCONTENTGRID_NAME, Type = typeof(Grid))]
    public class Map : Control
    {
        private const string PART_CONTENTPRESENTER_NAME = "PART_Presenter";
        private const string PART_CURSORTEXT_NAME = "PART_CursorText";
        private const string PART_ATTACHCONTENTGRID_NAME = "PART_AttachContentGrid";
        static Map()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(Map), new FrameworkPropertyMetadata(typeof(Map)));
        }

        public Map() 
        {
            AttachContents = [];
            BindingOperations.EnableCollectionSynchronization(AttachContents, new object());
            AttachContents.CollectionChanged += AttachContents_CollectionChanged;
        }

        private void AttachContents_CollectionChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (GetTemplateChild(PART_ATTACHCONTENTGRID_NAME) is Grid attachGrid)
            {
                switch (e.Action)
                {
                    case System.Collections.Specialized.NotifyCollectionChangedAction.Add:
                        if (e.NewItems != null && e.NewItems.Count > 0)
                        {
                            foreach (var item in e.NewItems)
                            {
                                if (item is AttachContentModel attach)
                                {
                                    if (attach.Content is FrameworkElement element)
                                    {
                                        element.VerticalAlignment = VerticalAlignment.Top;
                                        element.HorizontalAlignment = HorizontalAlignment.Left;
                                        
                                        element.Margin = new Thickness(attach.Location.X, attach.Location.Y, 0, 0);
                                        attachGrid.Children.Add(element);


                                    }
                                    else
                                    {
                                        attachGrid.Children.Add(new TextBlock() { Text = item.ToString() });
                                    }
                                }
                            }
                        }
                        break;
                }

            }
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



        public ObservableCollection<AttachContentModel> AttachContents
        {
            get { return (ObservableCollection<AttachContentModel>)GetValue(AttachContentsProperty); }
            set { SetValue(AttachContentsProperty, value); }
        }

        // Using a DependencyProperty as the backing store for AttachContents.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty AttachContentsProperty =
            DependencyProperty.Register("AttachContents", typeof(ObservableCollection<AttachContentModel>), typeof(Map), new PropertyMetadata(null));


    }
}