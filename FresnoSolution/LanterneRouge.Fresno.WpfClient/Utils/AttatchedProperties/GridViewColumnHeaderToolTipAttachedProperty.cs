using System.Windows;

namespace LanterneRouge.Fresno.WpfClient.Utils.AttatchedProperties
{
    public sealed class GridViewColumnHeaderToolTipAttachedProperty : DependencyObject
    {
        public static readonly DependencyProperty TooltipSourceProperty = DependencyProperty.RegisterAttached("Tooltip", typeof(string), typeof(GridViewColumnHeaderToolTipAttachedProperty), new PropertyMetadata("null"));

        public static void SetTooltip(DependencyObject element, string value) => element.SetValue(TooltipSourceProperty, value);

        public static string GetTooltip(DependencyObject element) => (string)element.GetValue(TooltipSourceProperty);
    }
}
