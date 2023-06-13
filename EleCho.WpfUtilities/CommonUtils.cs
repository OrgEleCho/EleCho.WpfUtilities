using System;
using System.Windows;
using System.Windows.Media;

namespace EleCho.WpfUtilities
{
    public static class CommonUtils
    {
        public static void RunOnFirstLoaded(this FrameworkElement element, EventHandler handler)
        {
            void Once(object? sender, RoutedEventArgs e)
            {
                element.Loaded -= Once;
                handler.Invoke(sender, e);
            }

            if (element.IsLoaded)
                handler.Invoke(element, EventArgs.Empty);
            else
                element.Loaded += Once;
        }

        public static TElement? GetElementFromVisualTree<TElement>(this FrameworkElement control) where TElement : FrameworkElement
        {
            if (control is TElement ele)
                return ele;

            int childrenCount = VisualTreeHelper.GetChildrenCount(control);
            for (int i = 0; i < childrenCount; i++)
            {
                DependencyObject child = VisualTreeHelper.GetChild(control, i);
                if (child is TElement eleChild)
                    return eleChild;
            }

            return null;
        }
    }
}