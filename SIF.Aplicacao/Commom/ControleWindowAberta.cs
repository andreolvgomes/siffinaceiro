using SIF.WPF.Styles.Windows.Controls;
using SIF.Dao;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SIF.Aplicacao
{
    /// <summary>
    /// 
    /// </summary>
    public class ControleWindowAberta
    {
        private List<Window> listWindow;
        private ConnectionDb conexao;

        /// <summary>
        /// 
        /// </summary>
        public ControleWindowAberta()
        {
            listWindow = new List<Window>();
        }

        /// <summary>
        /// /
        /// </summary>
        /// <param name="janela"></param>
        public void ShowWindow(Window janela)
        {
            if (listWindow.Count == 0)
                conexao = new ConnectionDb(".");

            listWindow.Add(janela);

            janela.Closing += new System.ComponentModel.CancelEventHandler(W_Closing);
            janela.ShowDialog();
        }

        private void W_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            listWindow.Remove((Window)sender);
            if (listWindow.Count == 0)
                conexao.Dispose();
        }
    }
}
