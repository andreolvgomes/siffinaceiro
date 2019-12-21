using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Controls;

namespace SIF.Pesquisa.DataGrid
{
    public class DelayTextBox : TextBox
    {
        private Timer _timerDelay;
        private int delayTemp = 250;
        private bool _keyPressed = false;
        private bool _timerStart = false;
        private TextChangedEventArgs eventTextChangedEventArgs;
        private delegate void TextChangedEventArgsDelegate();

        public DelayTextBox()
        {
            _timerDelay = new Timer(delayTemp);
            _timerDelay.Elapsed += new ElapsedEventHandler(Timer_Elapse);
        }

        private void Timer_Elapse(object sender, ElapsedEventArgs e)
        {
            _timerDelay.Enabled = false;
            _timerStart = true;

            this.Dispatcher.BeginInvoke(new TextChangedEventArgsDelegate(DelayOver));
        }

        private void DelayOver()
        {
            if (eventTextChangedEventArgs != null)
                OnTextChanged(eventTextChangedEventArgs);
        }

        protected override void OnTextChanged(TextChangedEventArgs e)
        {
            if (_timerStart || !_keyPressed)
            {
                _keyPressed = false;
                _timerStart = false;

                base.OnTextChanged(e);
            }
            eventTextChangedEventArgs = e;
        }

        protected override void OnPreviewKeyDown(System.Windows.Input.KeyEventArgs e)
        {
            if (KeyValida(e))
            {
                if (!_timerDelay.Enabled)
                    _timerDelay.Enabled = true;
                else
                {
                    _timerDelay.Enabled = false;
                    _timerDelay.Enabled = true;
                }

                base.OnPreviewKeyDown(e);
            }
            _keyPressed = true;
        }

        private bool KeyValida(System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Tab
                || e.Key == System.Windows.Input.Key.Right
                || e.Key == System.Windows.Input.Key.Right
                || e.Key == System.Windows.Input.Key.Down
                || e.Key == System.Windows.Input.Key.Enter
                || e.Key == System.Windows.Input.Key.Return
                || e.Key == System.Windows.Input.Key.Up
                || e.Key == System.Windows.Input.Key.Home
                || e.Key == System.Windows.Input.Key.Prior
                || e.Key == System.Windows.Input.Key.Insert
                || e.Key == System.Windows.Input.Key.End
                || e.Key == System.Windows.Input.Key.Next
                || e.Key == System.Windows.Input.Key.Capital
                || e.Key == System.Windows.Input.Key.Escape

                || e.Key == System.Windows.Input.Key.F1
                || e.Key == System.Windows.Input.Key.F2
                || e.Key == System.Windows.Input.Key.F3
                || e.Key == System.Windows.Input.Key.F4
                || e.Key == System.Windows.Input.Key.F5
                || e.Key == System.Windows.Input.Key.F6
                || e.Key == System.Windows.Input.Key.F7
                || e.Key == System.Windows.Input.Key.F8
                || e.Key == System.Windows.Input.Key.F9
                || e.Key == System.Windows.Input.Key.F10
                || e.Key == System.Windows.Input.Key.F11
                || e.Key == System.Windows.Input.Key.F12

                ) return false;

            return true;
        }
    }
}
