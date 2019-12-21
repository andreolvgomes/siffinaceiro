using SIF.Aplicacao.ManagerWindow;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Media;

namespace SIF.Aplicacao
{
    public class ManagerWindowState
    {
        public static void AddWindowManager(Window target)
        {
            if (NotExists(target))
            {
                target.Loaded += new RoutedEventHandler(Target_Loaded);
                target.Closed += new EventHandler(Target_Closed);
                target.StateChanged += new EventHandler(Target_StateChanged);

                target.Show();
            }
            else
            {
                SistemaGlobal.Sis.ManagerWindow.SetFocusWindow(target.Title);               
            }
        }

        private static void Target_StateChanged(object sender, EventArgs e)
        {
            Window target = sender as Window;
            if (target == null) return;
            if (target.WindowState == WindowState.Minimized)
            {
                target.WindowState = WindowState.Normal;
                target.Left = -3000;
            }
        }

        private static bool NotExists(Window target)
        {
            return (SistemaGlobal.Sis.ManagerWindow.WindowViewStates.FirstOrDefault(c => c.Title == target.Title) == null);
        }

        private static void Target_Closed(object sender, EventArgs e)
        {
            Window target = (Window)sender;
            if (target != null)
            {
                SistemaGlobal.Sis.ManagerWindow.RemoveWindowViewState(target);
                SistemaGlobal.Sis.ManagerWindow.ShowLastWindow();
            }
        }

        private static void Target_Loaded(object sender, RoutedEventArgs e)
        {
            Window target = (Window)sender;
            if (target != null)
            {
                WindowModelInstancia instance = new WindowModelInstancia();
                if (!string.IsNullOrEmpty(target.Title))
                {
                    instance.Title = target.Title;
                }
                else
                {
                    //Timer timer = new Timer();
                    //timer.Elapsed += new ElapsedEventHandler(T_Elapsed);
                    //timer.Enabled = true;
                    throw new Exception("Title IsNullOrEmpty");
                }
                instance.Icon = new VisualBrush(target);
                instance.Window = target;
                instance.Number = (SistemaGlobal.Sis.ManagerWindow.CountInstancesForWindowTitle(target.Title) + 1);
                instance.Window.Tag = instance.Number;
                SistemaGlobal.Sis.ManagerWindow.HideAllVisible(instance.WindowId);
                SistemaGlobal.Sis.ManagerWindow.WindowViewStates.Add(instance);
            }
        }
    }
}
