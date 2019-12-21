using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace SIF.Commom.Extenders
{
    /// <summary>
    /// Extesão de TextBox programado para aceitar somente números
    /// </summary>
    public class TextBoxInteger : TextBox
    {
        public TextBoxInteger()
        {
            this.PreviewTextInput += new System.Windows.Input.TextCompositionEventHandler(TextBox_PreviewTextInput);
        }

        private void TextBox_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            if (this.MaxLength > 0 && (this.Text + e.Text).Length > this.MaxLength)
                e.Handled = true;

            if (!e.Handled)
            {
                foreach (char c in e.Text)
                {
                    if (!char.IsDigit(c))
                        e.Handled = true;
                    else
                        e.Handled = false;
                }
            }
        }
    }
}
