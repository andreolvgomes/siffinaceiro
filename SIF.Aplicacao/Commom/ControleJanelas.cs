using SIF.Dao;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SIF.Aplicacao.Commom
{
    /// <summary>
    /// 
    /// </summary>
    public class ControleJanelas
    {

        private List<Window> listWindow;

        /// <summary>
        /// 
        /// </summary>
        public ControleJanelas()
        {
            listWindow = new List<Window>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="janela"></param>
        public void ShowWindow(Window janela)
        {
            try
            {
                listWindow.Add(janela);

                janela.Closing += new System.ComponentModel.CancelEventHandler(W_Closing);
                janela.ShowDialog();
            }
            catch (Exception ex)
            {
                janela.Close();
                SistemaGlobal.Sis.Log.LogError(ex, true, null);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void W_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            listWindow.Remove((Window)sender);
        }
    }
}
