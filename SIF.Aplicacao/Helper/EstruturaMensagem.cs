using SIF.WPF.Styles.Windows.Controls;
using SIF.Aplicacao;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace SIF.Helper
{
    /// <summary>
    /// 
    /// </summary>
    public class EstruturaMensagem
    {
        private List<Window> listSync;

        /// <summary>
        /// 
        /// </summary>
        public EstruturaMensagem()
        {
            this.listSync = new List<Window>();
        }

        /// <summary>
        /// Executa processo(Assíncrono), desta forma o processo é executado em outra Thread. Enquanto o processo
        /// é executado o usuário fica visualizando uma tela de aguarde.
        /// </summary>
        /// <param name="owner">Window pai</param>
        /// <param name="mensagem">Mensagem que será mostrada em tela</param>
        /// <param name="syncFunction">Método que será executado</param>
        public void ExecutaSync(Window owner, string mensagem, MessageSplashDelegate syncFunction)
        {
            Window window = null;
            if (listSync.Count > 0)
                window = listSync.LastOrDefault();
            else
                window = owner;
            if (window != null && window.Dispatcher.Thread != System.Threading.Thread.CurrentThread)
            {
                window.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Send, (System.Threading.SendOrPostCallback)delegate { ExecutaSync(window, mensagem, syncFunction); }, null);
            }
            else
            {
                Splash sp = new Splash(owner, mensagem);
                listSync.Add(sp);

                Thread sync = new Thread(new ParameterizedThreadStart(ExecutaSplash));
                sync.Start(new Tuple<Splash, MessageSplashDelegate>(sp, syncFunction));
                sp.ShowDialog();

                listSync.Remove(sp);
            }
        }

        /// <summary>
        /// Executa Thread
        /// </summary>
        /// <param name="obj"></param>
        private void ExecutaSplash(object obj)
        {
            Tuple<Splash, MessageSplashDelegate> ob = obj as Tuple<Splash, MessageSplashDelegate>;
            ob.Item2();
            ((Splash)ob.Item1).CloseSync();
        }

        /// <summary>
        /// Mostra mensagem
        /// </summary>
        /// <param name="mensagem">Mensagem</param>
        /// <param name="titulo">Título de mensagem</param>
        /// <param name="owner">Window pai</param>
        /// <returns></returns>
        public MessageBoxResult MostraMensagem(string mensagem, string titulo, Window owner)
        {
            return this.MostraMensagem(mensagem, titulo, owner, MessageBoxImage.None);
        }

        /// <summary>
        /// Mostra mensagem
        /// </summary>
        /// <param name="mensagem">Mensagem</param>
        /// <param name="titulo">Título de mensagem</param>
        /// <param name="owner">Window pai</param>
        /// <returns></returns>
        public MessageBoxResult MostraMensagem(string mensagem, string titulo, Window owner, MessageBoxImage type)
        {
            return this.MostraMensagem(mensagem, titulo, MessageBoxButton.OK, owner, type);       
        }

        /// <summary>
        /// Mostra mensagem
        /// </summary>
        /// <param name="mensagem">Mensagem</param>
        /// <param name="titulo">Título da mensagem</param>
        /// <param name="button">Botões que deveram aparecer</param>
        /// <param name="owner">Window pai</param>
        /// <returns></returns>
        public MessageBoxResult MostraMensagem(string mensagem, string titulo, MessageBoxButton button, Window owner)
        {
            return this.MostraMensagem(mensagem, titulo, button, owner, MessageBoxImage.None);
        }

        /// <summary>
        /// Mostra mensagem
        /// </summary>
        /// <param name="mensagem">Mensagem</param>
        /// <param name="titulo">Título da mensagem</param>
        /// <param name="button">Botões que deveram aparecer</param>
        /// <param name="owner">Window pai</param>
        /// <param name="type">Tipo de msg(error, information)</param>
        /// <returns></returns>
        public MessageBoxResult MostraMensagem(string mensagem, string titulo, MessageBoxButton button, Window owner, MessageBoxImage type)
        {
            Window window = null;
            if (listSync.Count > 0)
                window = listSync.LastOrDefault();
            else
                window = owner;
            if (window != null && window.Dispatcher.Thread != System.Threading.Thread.CurrentThread)
            {
                return (MessageBoxResult)window.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Send, (System.Windows.Threading.DispatcherOperationCallback)delegate { return MostraMensagem(mensagem, titulo, button, window); }, null);
            }
            else
            {
                ModernDialog msg = new ModernDialog();
                return msg.ShowResult(mensagem, titulo, button, owner, type);
            }
        }
    }
}
