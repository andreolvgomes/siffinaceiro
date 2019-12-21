using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interop;
using System.Windows.Media;

namespace Notification
{
    public partial class Balloon : Window
    {
        private Control control = null;
        private Window windowCotrol = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="Balloon" /> class.
        /// </summary>
        /// <param name="control">The control.</param>
        /// <param name="caption">The caption.</param>
        /// <param name="balloonType">Type of the balloon.</param>
        /// <param name="maxHeight">The maximum height.</param>
        /// <param name="autoWidth">if set to <c>true</c> [automatic width].</param>
        public Balloon(Control control, string caption)
        {
            InitializeComponent();

            this.control = control;
            this.windowCotrol = this.control as Window;

            Application.Current.MainWindow.Closing += this.OwnerClosing;
            Application.Current.MainWindow.LocationChanged += this.MainWindowLocationChanged;
            control.LayoutUpdated += this.MainWindowLocationChanged;
            this.textBlockCaption.Text = caption;
            this.CalcPosition();
        }

        /// <summary>
        /// Calculates the position.
        /// </summary>
        /// <param name="control">The control.</param>
        private void CalcPosition()
        {
            if (windowCotrol != null)
            {
                this.Top = windowCotrol.Top;
                this.Left = windowCotrol.Left + windowCotrol.Width - this.Width;
                windowCotrol.Focus();
            }
            else
            {
                PresentationSource source = PresentationSource.FromVisual(this.control);

                if (source != null)
                {
                    var screen = System.Windows.Forms.Screen.FromHandle(new WindowInteropHelper(Application.Current.MainWindow).Handle);
                    var location = this.control.PointToScreen(new System.Windows.Point(0, 0));
                    double leftPosition = location.X + (this.control.ActualWidth / 2);

                    // Check if the window is on the secondary screen.
                    if (((leftPosition < 0 && screen.WorkingArea.Width + leftPosition + this.Width < screen.WorkingArea.Width)) ||
                        leftPosition >= 0 && leftPosition + this.Width < screen.WorkingArea.Width)
                    {
                        this.Left = leftPosition;
                    }
                    else
                    {
                        this.Left = location.X + (this.control.ActualWidth / 2) - this.Width;
                    }

                    this.Top = location.Y + (this.control.ActualHeight / 2);
                }
            }
        }

        /// <summary>
        /// Doubles the animation completed.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void DoubleAnimationCompleted(object sender, EventArgs e)
        {
            if (!this.IsMouseOver)
            {
                this.Close();
            }
        }

        /// <summary>
        /// Mains the window location changed.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void MainWindowLocationChanged(object sender, EventArgs e)
        {
            this.CalcPosition();
        }

        /// <summary>
        /// Owners the closing.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.ComponentModel.CancelEventArgs"/> instance containing the event data.</param>
        private void OwnerClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            string name = typeof(Balloon).Name;
            foreach (Window window in System.Windows.Application.Current.Windows)
            {
                string windowType = window.GetType().Name;
                if (windowType.Equals(name))
                {
                    window.Close();
                }
            }
        }

        private void DoubdleAnimationCompleted(object sender, EventArgs e)
        {

        }
    }
}
