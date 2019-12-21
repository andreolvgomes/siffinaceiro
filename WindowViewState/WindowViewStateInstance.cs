using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace WindowViewState
{
    public class WindowViewStateInstance
    {
        private Window _window;

        public string Title { get; set; }
        public VisualBrush Icon { get; set; }
        public int Number { get; set; }
        public Window Window
        {
            get { return _window; }
            set
            {
                _window = value;
            }
        }
        public int WindowId
        {
            get { return (_window.GetHashCode() + Number.GetHashCode()); }
        }

    }
}
