using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIF.Aplicacao.ManagerWindow
{
    //public static class WindowFactory<C> where C : Window
    //{
    //    public static C CreateInstance(Window owner, params object[] args)
    //    {
    //        Window _window = null;
    //        if (WindowViewStateManager.Instance.WindowViewStates.Count > 0)
    //        {
    //            foreach (WindowViewStateInstance o in WindowViewStateManager.Instance.WindowViewStates)
    //            {
    //                if (o.Window.GetType().Name == typeof(C).Name)
    //                {
    //                    _window = o.Window;
    //                    break;
    //                }
    //            }

    //            if (_window != null)
    //            {
    //                _window.Activate();
    //                return (C) _window;
    //            }
    //            else
    //            {
    //                _window = (Window)Activator.CreateInstance(typeof (C), args);
    //                _window.Owner = owner;
    //                return (C) _window; 
    //            }
    //        }
    //        else
    //        {
    //            _window = (Window)Activator.CreateInstance(typeof(C), args);
    //            _window.Owner = owner;
    //            return (C)_window; 

    //        }
    //    }
    //}
}
