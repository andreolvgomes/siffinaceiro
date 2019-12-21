using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace SIF.Commom.Extenders
{
    public class TextBoxData : TextBox
    {
        public TextBoxData()
        {
            this.PreviewTextInput += new System.Windows.Input.TextCompositionEventHandler(TextBox_PreviewTextInput);
        }

        private void TextBox_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            bool isValided = this.IsDigitValided(e.Text);
            e.Handled = !isValided;

            if (isValided)
            {
                int _selectionStart = 0;
                int position = this.SelectionStart;
                string textInfo = "";

                if (this.SelectionLength > 0)
                {
                    _selectionStart = this.SelectionStart + this.SelectionLength;
                    this.Text = this.Text.Remove(this.SelectionStart, this.SelectionLength);
                    this.Text = this.Text.Insert(position, e.Text);
                    textInfo = this.Text;
                }
                else if (this.SelectionStart == this.Text.Length)
                {
                    textInfo = this.Text + e.Text;
                }
                else if (this.Text.Length > this.SelectionStart)
                {
                    _selectionStart = this.SelectionStart + 1;
                    this.Text = this.Text.Insert(position, e.Text);
                    textInfo = this.Text;
                }

                if (textInfo.Length > 10) textInfo = textInfo.Substring(0, 10);
                textInfo = textInfo.Replace("/", "");
                this.Text = string.Empty;
                for (int i = 0; i < textInfo.Length; i++)
                {
                    //00/00/0000
                    if (i == 2 || i == 4)
                        this.Text += "/";
                    this.Text += textInfo[i].ToString();
                }
                this.SelectionStart = (_selectionStart == 0) ? this.Text.Length : _selectionStart;
                if (this.SelectionStart == this.Text.Length && this.Text.Length == 5)
                {
                    this.Text += "/" + DateTime.Now.ToString("yyyy");
                    this.SelectionStart = 6;
                    this.SelectionLength = 4;
                }
                e.Handled = true;
            }
        }

        /// <summary>
        /// Valida se o digit informado é um digito de data
        /// </summary>
        /// <param name="digit"></param>
        /// <returns></returns>
        private bool IsDigitValided(string digit)
        {
            if ("/".Equals(digit)) return true;
            foreach (char c in digit)
                if (!char.IsDigit(c)) return false;
            return true;
        }
    }
}
