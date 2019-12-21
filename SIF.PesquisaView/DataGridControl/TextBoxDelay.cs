using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Controls;

namespace SIF.PesquisaView.DataGridControl
{
    public delegate void TimerTextChangedEventArgs();

    public class TextBoxDelay : TextBox
    {
        private bool timerStart = false;
        private bool pressedKey = false;

        private const int DELAY = 250;
        private Timer timer = null;

        private TextChangedEventArgs _e;

        public TextBoxDelay()
        {
            timer = new Timer(DELAY);
            timer.Elapsed += new ElapsedEventHandler(T_Elapsed);
        }

        private void T_Elapsed(object sender, ElapsedEventArgs e)
        {
            timer.Enabled = false;
            timerStart = true;
            this.Dispatcher.BeginInvoke(new TimerTextChangedEventArgs(OnTextChangedDelay));
        }

        private void OnTextChangedDelay()
        {
            if (_e != null)
                this.OnTextChanged(_e);
        }

        protected override void OnTextChanged(TextChangedEventArgs e)
        {
            if (timerStart || !pressedKey)
            {
                timerStart = false;
                pressedKey = false;

                base.OnTextChanged(e);
            }
            this._e = e;
        }

        protected override void OnPreviewTextInput(System.Windows.Input.TextCompositionEventArgs e)
        {
            if (!timer.Enabled)
                timer.Enabled = true;
            else
            {
                timer.Enabled = false;
                timer.Enabled = true;
            }
            base.OnPreviewTextInput(e);
            pressedKey = true;
        }
    }
}
