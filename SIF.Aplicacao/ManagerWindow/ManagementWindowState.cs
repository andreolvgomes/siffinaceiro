using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

namespace SIF.Aplicacao.ManagerWindow
{
    public class ManagementWindowState : INotifyPropertyChanged //: ViewModel
    {
        //private static WindowViewStateManager _instance;

        //public static WindowViewStateManager Instance
        //{
        //    get
        //    {
        //        if (_instance == null)
        //            _instance = new WindowViewStateManager();
        //        return _instance;
        //    }
        //}

        private Visibility _VisivelManagerWindow = Visibility.Collapsed;

        public Visibility VisivelManagerWindow
        {
            get { return _VisivelManagerWindow; }
            set
            {
                if (_VisivelManagerWindow != value)
                {
                    _VisivelManagerWindow = value;
                    this.OnPropertyChanged("VisivelManagerWindow");
                }
            }
        }

        public ManagementWindowState()
        {
            this.WindowViewStates.CollectionChanged += new System.Collections.Specialized.NotifyCollectionChangedEventHandler(View_CollectionChanged);
        }

        private void View_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (this.WindowViewStates.Count > 0)
                this.VisivelManagerWindow = Visibility.Visible;
            else
                this.VisivelManagerWindow = Visibility.Collapsed;
        }

        private void CenterWindow(Window w)
        {
            var width = (SystemParameters.PrimaryScreenWidth / 2);
            var tamanhojanela = w.Width / 2;

            var height = SystemParameters.PrimaryScreenHeight / 2;
            var alturajanela = w.Height / 2;
            w.Left = width - tamanhojanela;
            w.Top = height - alturajanela;
        }

        private WindowModelInstancia _selectedWindow;

        public WindowModelInstancia SelectedWindow
        {
            get { return _selectedWindow; }
            set
            {
                if (_selectedWindow != value)
                {
                    _selectedWindow = value;
                    this.OnPropertyChanged("SelectedWindow");

                    if (_selectedWindow != null)
                    {
                        this.SetFocusWindow(_selectedWindow.WindowId);
                    }
                }
            }
        }

        private ObservableCollection<WindowModelInstancia> _windowViewStates;

        public ObservableCollection<WindowModelInstancia> WindowViewStates
        {
            get { return _windowViewStates ?? (_windowViewStates = new ObservableCollection<WindowModelInstancia>()); }
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
            this.HideAllVisible(view.WindowId);
            this.CenterWindow(view.Window);
            this.SetFocusForce(view.Window);
            return view.Window.Activate();
        }

        private void SetFocusForce(Window window)
        {
            if (window != null)
            {
                if (window.Dispatcher.Thread != System.Threading.Thread.CurrentThread)
                {
                    window.Dispatcher.BeginInvoke(DispatcherPriority.Send, (System.Threading.SendOrPostCallback)delegate { SetFocusForce(window); }, null);
                }
                else
                {
                    window.Dispatcher.Invoke(new Action(() =>
                    {
                        window.Activate();
                        Keyboard.Focus(window);
                        window.Focus();
                        window.Topmost = true;
                        window.Topmost = false;
                    }));
                }
            }
        }

        public bool SetFocusWindow(string Title)
        {
            var view = (from v in _windowViewStates where v.Title == Title select v).First();
            this.HideAllVisible(view.WindowId);
            this.CenterWindow(view.Window);
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
                return lista.Max(w => w.Number);
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

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
