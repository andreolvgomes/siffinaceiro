using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Effects;

namespace SIF.Aplicacao
{
    /// <summary>
    /// 
    /// </summary>
    public class EffectWindow : IDisposable
    {
        /// <summary>
        /// 
        /// </summary>
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        private Effect EffectOriginal { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="wPai"></param>
        /// <param name="wFilha"></param>
        public void SetEffectBackgound(Window wPai, Window wFilha)
        {
            //if (wPai == null)
            //    throw new Exception("Window pai não pode ser null");
            //if (wFilha == null)
            //    throw new Exception("Window filha não pode ser null");

            wFilha.Owner = wPai;
            //this.EffectOriginal = wPai.Effect;
            //wFilha.Owner.Effect = new BlurEffect() { Radius = 5, KernelType = KernelType.Gaussian };
            wFilha.ShowInTaskbar = false;
            //wFilha.Closing += new System.ComponentModel.CancelEventHandler(Window_Closing);
            wFilha.PreviewKeyDown += new System.Windows.Input.KeyEventHandler(Window_PreviewKeyDown);
        }

        private void Window_PreviewKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Escape)
            {
                Window w = sender as Window;
                w.Close();
                e.Handled = true;
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Window w = sender as Window;
            if (w.Owner != null)
                w.Owner.Effect = this.EffectOriginal;
        }        
    }
}
