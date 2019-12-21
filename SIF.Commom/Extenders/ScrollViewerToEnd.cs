using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace SIF.Commom.Extenders
{
    public class ScrollViewerToEnd
    {
        public static bool GetScrollToEnd(DependencyObject obj)
        {
            return (bool)obj.GetValue(ScrollToEndProperty);
        }

        public static void SetScrollToEnd(DependencyObject obj, bool value)
        {
            obj.SetValue(ScrollToEndProperty, value);
        }

        // Using a DependencyProperty as the backing store for ScrollToEnd.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ScrollToEndProperty =
            DependencyProperty.RegisterAttached("ScrollToEnd", typeof(bool), typeof(ScrollViewerToEnd), new PropertyMetadata(false, new PropertyChangedCallback(OnPropertyChangedCallback)));

        private static void OnPropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ScrollViewer scr = d as ScrollViewer;
            if (scr == null)
                return;
            scr.ScrollChanged += new ScrollChangedEventHandler(ScrollChanged);
        }

        private static void ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            if (e.ExtentHeightChange > 0)
                ((ScrollViewer)sender).ScrollToEnd();
        }
    }
}
