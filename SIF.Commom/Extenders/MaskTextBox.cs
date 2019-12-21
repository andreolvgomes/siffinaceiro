using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace SIF.Commom.Extenders
{
    public class MaskTextBox
    {
        public static string GetMask(DependencyObject obj)
        {
            return (string)obj.GetValue(MaskProperty);
        }

        public static void SetMask(DependencyObject obj, string value)
        {
            obj.SetValue(MaskProperty, value);
        }

        // Using a DependencyProperty as the backing store for Mask.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MaskProperty =
            DependencyProperty.RegisterAttached("Mask", typeof(string), typeof(MaskTextBox), new PropertyMetadata(OnChanged));

        private static void OnChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            TextBox textbox = d as TextBox;
            if (textbox == null)
                return;

            textbox.PreviewTextInput += new System.Windows.Input.TextCompositionEventHandler(Textbox_PreviewTextInput);
        }

        private static void Textbox_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            TextBox textbox = sender as TextBox;
            string _mask = GetMask(textbox) + "0";

            if (textbox.Text.Length + e.Text.Length > _mask.Length - 1 && textbox.SelectionLength == 0)
            {
                e.Handled = true;
                return;
            }
            bool valid = DigitValid(e.Text);
            e.Handled = !valid;

            if (valid)
            {
                int _selectionStart = 0;
                int position = textbox.SelectionStart;
                string textInfo = "";

                if (textbox.SelectionLength > 0)
                {
                    _selectionStart = textbox.SelectionStart + textbox.SelectionLength;
                    textbox.Text = textbox.Text.Remove(textbox.SelectionStart, textbox.SelectionLength);
                    textbox.Text = textbox.Text.Insert(position, e.Text);
                    textInfo = textbox.Text;
                }
                else if (textbox.SelectionStart == textbox.Text.Length)
                {
                    textInfo = textbox.Text + e.Text;
                }
                else if (textbox.Text.Length > textbox.SelectionStart)
                {
                    _selectionStart = textbox.SelectionStart + 1;
                    textbox.Text = textbox.Text.Insert(position, e.Text);
                    textInfo = textbox.Text;
                }

                textbox.Text = string.Empty;
                int i = 0;
                foreach (char text in textInfo)
                {
                    i++;
                    if (char.IsDigit(text))
                    {
                        textbox.Text += text;
                        if (_mask[i].ToString() != "0")
                            textbox.Text += _mask[i].ToString();
                    }
                }
                textbox.SelectionStart = (_selectionStart == 0) ? textbox.Text.Length : _selectionStart;
                e.Handled = true;
            }
        }

        private static bool DigitValid(string text)
        {
            foreach (char c in text)
                if (!char.IsDigit(c)) return false;
            return true;
        }
    }
}
