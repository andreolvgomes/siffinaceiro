using SIF.WPF.Styles.Presentation;
using SIF.WPF.Styles.Windows.Controls;
using SIF.Aplicacao.ConfiguracaoInstanciaSQL;
using SIF.Dao;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace SIF.Aplicacao
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        /// <summary>
        /// App
        /// </summary>
        public App()
        {
            //http://www.andrealveslima.com.br/blog/index.php/2015/09/02/armazenando-bibliotecas-em-subdiretorios-no-c/
            AppDomain.CurrentDomain.AssemblyResolve += new ResolveEventHandler(ResolveAssemblyEventArgs);

            this.Startup += new StartupEventHandler(S_Startup);
        }

        private System.Reflection.Assembly ResolveAssemblyEventArgs(object sender, ResolveEventArgs args)
        {
            string folderPath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            string assemblyPath = System.IO.Path.Combine(folderPath, "Lib", new System.Reflection.AssemblyName(args.Name).Name + ".dll");
            if (!System.IO.File.Exists(assemblyPath)) return null;
            System.Reflection.Assembly assembly = System.Reflection.Assembly.LoadFrom(assemblyPath);
            return assembly;
        }

        private void S_Startup(object sender, StartupEventArgs e)
        {
            //foreach (System.Diagnostics.Process item in System.Diagnostics.Process.GetProcessesByName("SIF - Sistema Financeiro"))
            //{
            //    item.Kill();
            //}
            if (System.Diagnostics.Process.GetProcessesByName("SIF - Sistema Financeiro").Count() >= 2)
            {
                EncerraApp();
            }
            else
            {
                Layouts.SettingsAppearanceViewModel d = new Layouts.SettingsAppearanceViewModel();
                Menu ap = new Menu();
                if (!Navigate.Configura())
                {
                    this.EncerraApp();
                }
                else
                {
                    if (!this.EstabeleceComunicacaoDb())
                    {
                        this.EncerraApp();
                    }
                    else
                    {
                        this.CriateDatabase();
                        bool sucesso = Login.PegaLogin(SistemaGlobal.Sis.Connection);
                        if (!sucesso)
                            EncerraApp();
                        else
                            ap.Show();
                    }
                }
            }
        }

        private bool EstabeleceComunicacaoDb()
        {
            if (SistemaGlobal.Sis.XmlSistema.Armazenamento == TIPO_ARMAZENAMENTO.ARMAZENAMENTO_BANCO_DE_DADOS)
                return SistemaGlobal.Sis.VerificaConexao(null);

            if (!System.IO.File.Exists(SistemaGlobal.Sis.XmlSistema.CaminhoArquivoBanco))
                return SistemaGlobal.Sis.Msg.MostraMensagem("Não foi possível localizar o arquivo banco de dados em:\n\n" + System.IO.Path.GetDirectoryName(SistemaGlobal.Sis.XmlSistema.CaminhoArquivoBanco) + "\\", "Atenção", null) != MessageBoxResult.OK;
            return true;
        }

        /// <summary>
        /// Cria banco de dados caso não exista
        /// </summary>
        private void CriateDatabase()
        {
            //using (DbExecutaScripts cmd = new DbExecutaScripts())
            //{
            //    cmd.CreateDatabase(null, Sistema.Sis.XmlConnectionDb.Instancia);
            //}
        }

        /// <summary>
        /// Encerra a instância corrente do programa
        /// </summary>
        private void EncerraApp()
        {
            if (System.Windows.Application.Current != null)
                System.Windows.Application.Current.Shutdown();
        }

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Application.Startup"/> event.
        /// </summary>
        /// <param name="e">A <see cref="T:System.Windows.StartupEventArgs"/> that contains the event data.</param>
        protected override void OnStartup(StartupEventArgs e)
        {
            EventManager.RegisterClassHandler(typeof(TextBox), TextBox.GotKeyboardFocusEvent, new RoutedEventHandler(TextBox_GotKeyboardFocusEvent));
            EventManager.RegisterClassHandler(typeof(PasswordBox), PasswordBox.GotKeyboardFocusEvent, new RoutedEventHandler(PasswordBox_GotKeyboardFocusEvent));
            base.OnStartup(e);
        }

        private void PasswordBox_GotKeyboardFocusEvent(object sender, RoutedEventArgs e)
        {
            PasswordBox pass = sender as PasswordBox;
            pass.Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(() =>
            {
                pass.SelectAll();
            }));
        }

        private void TextBox_GotKeyboardFocusEvent(object sender, RoutedEventArgs e)
        {
            TextBox textbox = sender as TextBox;
            textbox.Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(() =>
                {
                    textbox.SelectAll();
                }));
        }
    }
}
