using System.Windows.Controls;
using System.Windows;
using Lemon.Map.ViewModel;
using Lemon.Map.Wpf.Utils;

namespace Lemon.Map.Wpf.Controls
{
    public class MapContentTemplateSelector : DataTemplateSelector
    {
        public DataTemplate? TemplateChinaAlpha { get; set; }
        public DataTemplate? TemplateChinaBeta { get; set; }
        public DataTemplate? TemplateWorldAlpha { get; set; }

        public override DataTemplate? SelectTemplate(object item, DependencyObject container)
        {
            try
            {
                if (item is MapType mapType)
                {
                    return mapType switch
                    {
                        MapType.CHINA_ALPHA => TemplateChinaAlpha,
                        MapType.CHINA_BETA => TemplateChinaBeta,
                        MapType.WORLD_ALPHA => TemplateWorldAlpha,
                        _ => base.SelectTemplate(item, container),
                    };
                }
                return TemplateWorldAlpha;
            }
            finally
            {

            }
        }
    }

}
