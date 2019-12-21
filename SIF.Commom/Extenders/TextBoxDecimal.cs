using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace SIF.Commom.Extenders
{
    public class TextBoxDecimal : TextBox
    {
        private static bool prossegue = true;

        public TextBoxDecimal()
        {
            this.PreviewTextInput += TextBox_PreviewTextInput;
            this.LostKeyboardFocus += new System.Windows.Input.KeyboardFocusChangedEventHandler(TextBox_LostKeyboardFocus);
            DataObject.AddPastingHandler(this, (DataObjectPastingEventHandler)TextBoxPastingEventHandler);

            PreencheTextBox(this);

            this.TextAlignment = TextAlignment.Right;

            ValidateTextBox(this);
        }

        private void TextBox_LostKeyboardFocus(object sender, System.Windows.Input.KeyboardFocusChangedEventArgs e)
        {
            TextBox box = (sender as TextBox);
            PreencheTextBox(box);
        }

        private void PreencheTextBox(TextBox box)
        {
            if (string.IsNullOrEmpty(box.Text))
            {
                box.Text = "0,00";
            }
            else
            {
                box.Text = (Convert.ToDecimal(box.Text)).ToString("N2").Replace(".", "");
            }
        }

        private void ValidateTextBox(TextBox _this)
        {
            _this.Text = ValidateValue(_this.Text);
        }

        private void TextBoxPastingEventHandler(object sender, DataObjectPastingEventArgs e)
        {
            TextBox _this = (sender as TextBox);
            string clipboard = e.DataObject.GetData(typeof(string)) as string;
            clipboard = ValidateValue(clipboard);
            if (!string.IsNullOrEmpty(clipboard))
            {
                _this.Text = clipboard;
            }
            e.CancelCommand();
            e.Handled = true;
        }        

        private void TextBox_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            TextBox _this = (sender as TextBox);
            prossegue = true;

            if (_this.MaxLength > 0)
                if (_this.Text.Length < _this.MaxLength)
                    prossegue = true;
                else
                    prossegue = false;

            if (prossegue)
            {
                bool isValid = IsSymbolValid(e.Text);
                isValid = IsValidaVirgula(_this, e.Text);

                e.Handled = !isValid;
                if (isValid)
                {
                    int caret = _this.CaretIndex;
                    string text = _this.Text;
                    bool textInserted = false;
                    int selectionLength = 0;

                    if (_this.SelectionLength > 0)
                    {
                        text = text.Substring(0, _this.SelectionStart) + text.Substring(_this.SelectionStart + _this.SelectionLength);
                        caret = _this.SelectionStart;
                    }

                    if (e.Text == NumberFormatInfo.CurrentInfo.NumberDecimalSeparator)
                    {
                        while (true)
                        {
                            int ind = text.IndexOf(NumberFormatInfo.CurrentInfo.NumberDecimalSeparator);
                            if (ind == -1)
                                break;

                            text = text.Substring(0, ind) + text.Substring(ind + 1);
                            if (caret > ind)
                                caret--;
                        }

                        if (caret == 0)
                        {
                            text = "0" + text;
                            caret++;
                        }
                        else
                        {
                            if (caret == 1 && string.Empty + text[0] == NumberFormatInfo.CurrentInfo.NegativeSign)
                            {
                                text = NumberFormatInfo.CurrentInfo.NegativeSign + "0" + text.Substring(1);
                                caret++;
                            }
                        }

                        if (caret == text.Length)
                        {
                            selectionLength = 1;
                            textInserted = true;
                            text = text + NumberFormatInfo.CurrentInfo.NumberDecimalSeparator + "0";
                            caret++;
                        }
                    }
                    else if (e.Text == NumberFormatInfo.CurrentInfo.NegativeSign)
                    {
                        textInserted = true;
                        if (_this.Text.Contains(NumberFormatInfo.CurrentInfo.NegativeSign))
                        {
                            text = text.Replace(NumberFormatInfo.CurrentInfo.NegativeSign, string.Empty);
                            if (caret != 0)
                                caret--;
                        }
                        else
                        {
                            text = NumberFormatInfo.CurrentInfo.NegativeSign + _this.Text;
                            caret++;
                        }
                    }

                    if (!textInserted)
                    {
                        text = text.Substring(0, caret) + e.Text + ((caret < _this.Text.Length) ? text.Substring(caret) : string.Empty);
                        caret++;
                    }

                    while (text.Length > 1 && text[0] == '0' && string.Empty + text[1] != NumberFormatInfo.CurrentInfo.NumberDecimalSeparator)
                    {
                        text = text.Substring(1);
                        if (caret > 0)
                            caret--;
                    }

                    while (text.Length > 2 && string.Empty + text[0] == NumberFormatInfo.CurrentInfo.NegativeSign && text[1] == '0' && string.Empty + text[2] != NumberFormatInfo.CurrentInfo.NumberDecimalSeparator)
                    {
                        text = NumberFormatInfo.CurrentInfo.NegativeSign + text.Substring(2);
                        if (caret > 1)
                            caret--;
                    }

                    if (caret > text.Length)
                        caret = text.Length;

                    _this.Text = text;
                    _this.CaretIndex = caret;
                    _this.SelectionStart = caret;
                    _this.SelectionLength = selectionLength;

                    e.Handled = true;
                }
            }
        }

        private bool ValidaNumero(string text)
        {
            foreach (char c in text)
                if (!Char.IsDigit(c))
                    return false;
            return true;
        }

        private bool IsValidaVirgula(TextBox box, string str)
        {
            if (str == NumberFormatInfo.CurrentInfo.NegativeSign) return false;

            if (str != NumberFormatInfo.CurrentInfo.NumberDecimalSeparator)
                foreach (char ch in str)
                {
                    if (!Char.IsDigit(ch)) return false;
                }

            int pVirgula = box.Text.IndexOf(NumberFormatInfo.CurrentInfo.NumberDecimalSeparator);
            if (str == NumberFormatInfo.CurrentInfo.NumberDecimalSeparator && pVirgula != -1)
            {
                return false;
            }
            else
            {
                if (pVirgula != -1)
                {
                    int tamanho = box.Text.Substring(pVirgula + 1, box.Text.Length - (pVirgula + 1)).Length;

                    if (box.CaretIndex > pVirgula && tamanho == 2)
                        return false;
                }
            }
            return true;
        }

        private string ValidateValue(string value)
        {
            if (string.IsNullOrEmpty(value))
                return string.Empty;

            value = value.Trim();
            try
            {
                Convert.ToDouble(value);
                return value;
            }
            catch
            {
            }
            return value;
        }

        private bool IsSymbolValid(string str)
        {
            if (str == NumberFormatInfo.CurrentInfo.NumberDecimalSeparator || str == NumberFormatInfo.CurrentInfo.NegativeSign)
                return true;

            foreach (char ch in str)
            {
                if (!Char.IsDigit(ch))
                    return false;
            }
            return true;
        }
    }
}
