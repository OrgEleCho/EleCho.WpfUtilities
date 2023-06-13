using System.Windows;

namespace EleCho.WpfUtilities
{
    public static class ScrollViewerUtils
    {

        [AttachedPropertyBrowsableForType(typeof(FrameworkElement))]
        public static bool GetEnableSmoothScroll(DependencyObject obj)
        {
            return (bool)obj.GetValue(EnableSmoothScrollProperty);
        }

        public static void SetEnableSmoothScroll(DependencyObject obj, bool value)
        {
            obj.SetValue(EnableSmoothScrollProperty, value);
        }

        // Using a DependencyProperty as the backing store for EnableSmoothScroll.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty EnableSmoothScrollProperty =
            DependencyProperty.RegisterAttached("EnableSmoothScroll", typeof(bool), typeof(ScrollViewerUtils), new PropertyMetadata(false, EnableSmoothScrollChanged));

        private static void EnableSmoothScrollChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            
        }
    }
}