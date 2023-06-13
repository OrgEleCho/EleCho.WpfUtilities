using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace EleCho.WpfUtilities
{

    public static class BorderUtils
    {
        [AttachedPropertyBrowsableForType(typeof(FrameworkElement))]
        public static CornerRadius GetCornerRadius(DependencyObject obj)
        {
            return (CornerRadius)obj.GetValue(CornerRadiusProperty);
        }

        public static void SetCornerRadius(DependencyObject obj, CornerRadius value)
        {
            obj.SetValue(CornerRadiusProperty, value);
        }

        // Using a DependencyProperty as the backing store for CornerRadius.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CornerRadiusProperty =
            DependencyProperty.RegisterAttached("CornerRadius", typeof(CornerRadius), typeof(BorderUtils), new PropertyMetadata(new CornerRadius(), CornerRadiusChanged));

        private static void CornerRadiusChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is not FrameworkElement ele)
                return;

            ele.RunOnFirstLoaded((s, _e) =>
            {
                if (CommonUtils.GetElementFromVisualTree<Border>(ele) is not Border border)
                    return;

                border.CornerRadius = (CornerRadius)e.NewValue;
            });
        }
    }
}