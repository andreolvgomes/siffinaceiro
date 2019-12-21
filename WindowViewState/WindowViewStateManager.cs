using JulMar.Windows.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace WindowViewState
{
    public class WindowViewStateManager : ViewModel
    {
        private static WindowViewStateManager _instance;

        public static WindowViewStateManager Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new WindowViewStateManager();
                return _instance;
            }
        }

        private WindowViewStateManager()
        {
            CmdCloseWindow = new DelegatingCommand(CloseWindow, CanCloseWindow);
            CmdSetFocusWindow = new DelegatingCommand(FocusWindow, CanFocusWindow);
        }

        #region Commands

        public ICommand CmdSetFocusWindow { get; private set; }
        public ICommand CmdCloseWindow { get; private set; }



        private void CenterWindow(Window w)
        {
            var width = (SystemParameters.PrimaryScreenWidth / 2);
            var tamanhojanela = w.Width / 2;

            var height = SystemParameters.PrimaryScreenHeight / 2;
            var alturajanela = w.Height / 2;
            w.Left = width - tamanhojanela;
            w.Top = height - alturajanela;
        }



        private void FocusWindow()
        {
            if (SelectedWindow != null)
                SetFocusWindow(SelectedWindow.WindowId);
        }

        private bool CanFocusWindow()
        {
            return (SelectedWindow != null);
        }

        private void CloseWindow()
        {
            if (SelectedWindow != null)
                CloseWindow(SelectedWindow.WindowId);
        }

        private bool CanCloseWindow()
        {
            return (SelectedWindow != null);
        }

        #endregion

        private WindowViewStateInstance _selectedWindow;

        public WindowViewStateInstance SelectedWindow
        {
            get { return _selectedWindow; }
            set
            {
                _selectedWindow = value;
                OnPropertyChanged("SelectedWindow");
            }
        }

        private ObservableCollection<WindowViewStateInstance> _windowViewStates;

        public ObservableCollection<WindowViewStateInstance> WindowViewStates
        {
            get { return _windowViewStates ?? (_windowViewStates = new ObservableCollection<WindowViewStateInstance>()); }
        }

        public bool RemoveWindowViewState(string Title)
        {
            var view = (from v in _windowViewStates where v.Title == Title select v).First();
            if (view != null)
            {
                if (_windowViewStates.Count == 1)
                {
                    view.Window.IsEnabled = true;
                    view.Window.Owner.Activate();
                    view.Window.Owner.Focus();
                }
                _windowViewStates.Remove(view);
                view = null;

                return true;
            }
            return false;
        }

        public bool RemoveWindowViewState(Window window)
        {
            var view = (from v in _windowViewStates where (v.Title == window.Title) && (v.Number.ToString() == window.Tag.ToString()) select v).First();
            if (view != null)
            {
                if (_windowViewStates.Count == 1)
                {
                    view.Window.IsEnabled = true;
                    view.Window.Owner.Activate();
                    view.Window.Owner.Focus();
                }
                _windowViewStates.Remove(view);
                view = null;
                GC.Collect();
                return true;
            }
            return false;
        }

        public bool RemoveWindowViewState(int WindowHashCode)
        {
            var view = (from v in _windowViewStates where v.WindowId == WindowHashCode select v).First();
            if (view != null)
            {
                _windowViewStates.Remove(view);
                view = null;
                GC.Collect();
                return true;
            }
            return false;
        }

        public Window GetOpenWindow(int WindowHashCode)
        {
            var view = (from v in _windowViewStates where v.WindowId == WindowHashCode select v).First();
            return view.Window;
        }

        public Window GetOpenWindow(string Title)
        {
            var view = (from v in _windowViewStates where v.Title == Title select v).First();
            return view.Window;
        }

        public bool SetFocusWindow(int WindowHashCode)
        {
            var view = (from v in _windowViewStates where v.WindowId == WindowHashCode select v).First();
            HideAllVisible(view.WindowId);
            CenterWindow(view.Window);
            return view.Window.Activate();
        }

        public bool SetFocusWindow(string Title)
        {
            var view = (from v in _windowViewStates where v.Title == Title select v).First();
            HideAllVisible(view.WindowId);
            CenterWindow(view.Window);
            return view.Window.Activate();
        }

        public void CloseWindow(int WindowHashCode)
        {
            var view = (from v in _windowViewStates where v.WindowId == WindowHashCode select v).First();
            view.Window.Close();
        }

        public void CloseWindow(string Title)
        {
            var view = (from v in _windowViewStates where v.Title == Title select v).First();
            view.Window.Close();
        }

        public int CountInstancesForWindowTitle(string title)
        {
            var lista = (from v in _windowViewStates where v.Title == title select v).ToList();

            if (lista.Count > 0)
            {
                return lista.Max(w => w.Number);
            }

            return 0;
        }

        public void HideAllVisible(int WindowHashCode)
        {
            foreach (var view in _windowViewStates)
            {
                if (view.WindowId != WindowHashCode)
                {
                    view.Window.Left = -3000;
                }
            }
        }

        public void ShowLastWindow()
        {
            if (_windowViewStates.Count > 0)
            {
                var w = _windowViewStates[_windowViewStates.Count - 1];
                CenterWindow(w.Window);
            }
        }
    }
}
