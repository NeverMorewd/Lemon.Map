using Lemon.Map.ViewModel;
using Lemon.Map.Wpf.Utils;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace Lemon.Map.Wpf.Controls
{
    [TemplatePart(Name = PART_CONTENTPRESENTER_NAME, Type = typeof(ContentPresenter))]
    public class Map : ContentControl
    {
        private const string PART_CONTENTPRESENTER_NAME = "PART_Presenter";
        static Map()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(Map), new FrameworkPropertyMetadata(typeof(Map)));
        }

        public static readonly DependencyPropertyKey RegionsPropertyKey =
            DependencyProperty.RegisterReadOnly(
                "Regions",
                typeof(IEnumerable<RegionModel>),
                typeof(Map),
                new PropertyMetadata(null));

        public static readonly DependencyProperty RegionsProperty =
            RegionsPropertyKey.DependencyProperty;
        public IEnumerable<RegionModel>? Regions
        {
            get { return (IEnumerable<RegionModel>?)GetValue(RegionsProperty); }
        }

        public void SetRegionsProperty(IEnumerable<RegionModel>? value)
        {
            SetValue(RegionsPropertyKey, value);
        }
        protected override void OnVisualChildrenChanged(DependencyObject visualAdded, DependencyObject visualRemoved)
        {
            base.OnVisualChildrenChanged(visualAdded, visualRemoved);
            var regions = VisualTreeUtil.FindVisualChildren<RegionBlock>(this);
            if (regions != null && regions.Count != 0)
            {
                SetRegionsProperty(regions.Select(rb => new RegionModel
                {
                    Name = rb.Content.ToString(),
                    Width = rb.Width,
                    Height = rb.Height,
                    Left = Canvas.GetLeft(rb),
                    Top = Canvas.GetTop(rb),
                }));
            }
        }
        protected override void OnContentTemplateChanged(DataTemplate oldContentTemplate, DataTemplate newContentTemplate)
        {
            base.OnContentTemplateChanged(oldContentTemplate, newContentTemplate);
            // selector can not fire this method;
        }

        protected override void OnContentTemplateSelectorChanged(DataTemplateSelector oldContentTemplateSelector, DataTemplateSelector newContentTemplateSelector)
        {
            base.OnContentTemplateSelectorChanged(oldContentTemplateSelector, newContentTemplateSelector);
        }

        protected override void OnTemplateChanged(ControlTemplate oldTemplate, ControlTemplate newTemplate)
        {
            base.OnTemplateChanged(oldTemplate, newTemplate);
        }
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            ContentPresenter? contentPresenter = GetTemplateChild(PART_CONTENTPRESENTER_NAME) as ContentPresenter;

        }

        protected override void OnContentChanged(object oldContent, object newContent)
        {
            base.OnContentChanged(oldContent, newContent);
            if (oldContent != newContent)
            {
                ///https://stackoverflow.com/questions/25977141/wpf-when-changing-a-contentcontrols-content-the-new-content-object-is-not-fir
                Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Render, UpdateRegions);
            }

        }

        private void UpdateRegions()
        {
            var regions = VisualTreeUtil.FindVisualChildren<RegionBlock>(this);
            if (regions != null && regions.Count != 0)
            {
                SetRegionsProperty(regions.Select(rb => new RegionModel
                {
                    Name = rb.Content.ToString(),
                    Width = rb.ActualWidth,
                    Height = rb.ActualHeight,
                    Left = rb.TransformToAncestor(this).Transform(new Point(0,0)).X,
                    Top = rb.TransformToAncestor(this).Transform(new Point(0, 0)).Y,
                }));
            }
        }
    }
}
