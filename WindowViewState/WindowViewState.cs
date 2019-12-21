using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace WindowViewState
{
    public class WindowViewState
    {
        public static bool GetIsManaged(DependencyObject obj)
        {
            return (bool)obj.GetValue(IsManagedProperty);
        }

        public static void SetIsManaged(DependencyObject obj, bool value)
        {
            obj.SetValue(IsManagedProperty, value);
        }

        // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsManagedProperty =
            DependencyProperty.RegisterAttached("IsManaged", typeof(bool), typeof(WindowViewState),
                                                new FrameworkPropertyMetadata((bool)false,
                                                                              new PropertyChangedCallback(
                                                                                  OnIsManagedChanged)));

        private static void OnIsManagedChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Window target = (Window)d;
            if (target != null)
            {
                target.Loaded += new RoutedEventHandler(target_Loaded);
                target.Closed += new EventHandler(target_Closed);

            }
        }


        private static void target_Closed(object sender, EventArgs e)
        {
            Window target = (Window)sender;
            if (target != null)
            {
                WindowViewStateManager.Instance.RemoveWindowViewState(target);
                WindowViewStateManager.Instance.ShowLastWindow();
            }
        }

        private static void target_Loaded(object sender, RoutedEventArgs e)
        {
            Window target = (Window)sender;
            if (target != null)
            {
                WindowViewStateInstance instance = new WindowViewStateInstance();
                instance.Title = target.Title;
                instance.Icon = new VisualBrush(target);
                instance.Window = target;
                instance.Number = (WindowViewStateManager.Instance.CountInstancesForWindowTitle(target.Title) + 1);
                instance.Window.Tag = instance.Number;
                WindowViewStateManager.Instance.HideAllVisible(instance.WindowId);
                WindowViewStateManager.Instance.WindowViewStates.Add(instance);
            }
        }
    }
}
