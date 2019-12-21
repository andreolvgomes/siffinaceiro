using SIF.WPF.Styles.Windows.Controls;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Sql;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace SIF.Aplicacao
{
    public partial class InstanciaSQL
    {
        private XmlConfiguracaoInstancia providerXml;

        public InstanciaSQL(XmlConfiguracaoInstancia xml)
        {
            InitializeComponent();

            providerXml = xml;
            this.DataContext = providerXml;

            this.Loaded += new RoutedEventHandler(W_Loaded);
            this.PreviewKeyDown += new KeyEventHandler(W_PreviewKeyDown);
        }

        private void W_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape) this.Close();
        }

        private void W_Loaded(object sender, RoutedEventArgs e)
        {
            providerXml.Instancia = providerXml.ListInstancias.FirstOrDefault();
            cmbBox.Focus();
        }

        private void Salvar_Click(object sender, RoutedEventArgs e)
        {
            if (Validacao())
            {
                bool ifConnection = false;
                SistemaGlobal.Sis.Msg.ExecutaSync(this, "Verificando conexão, aguarde...",
                    () =>
                    {
                        ifConnection = providerXml.TestaConnection();
                        if (ifConnection)
                            providerXml.SaveXml();
                        else
                            SistemaGlobal.Sis.Msg.MostraMensagem("Falha de conexão, verifique a instância informada!", "Atenção", MessageBoxButton.OK, this);
                    });
                if (ifConnection) this.Close();
            }
        }

        private bool Validacao()
        {
            if (string.IsNullOrEmpty(providerXml.Instancia))
            {
                cmbBox.Focus();
                return SistemaGlobal.Sis.Msg.MostraMensagem("Informe uma instância SQL", "Atenção", MessageBoxButton.OK, this) != MessageBoxResult.OK;
            }
            if (providerXml.Autenticacao)
            {
                if (string.IsNullOrEmpty(providerXml.Usuario))
                {
                    txtUsuario.Focus(); txtUsuario.SelectAll();
                    return SistemaGlobal.Sis.Msg.MostraMensagem("Informe o usuário", "Atenção", MessageBoxButton.OK, this) != MessageBoxResult.OK;
                }
                if (string.IsNullOrEmpty(providerXml.Senha))
                {
                    txtSenha.Focus(); txtSenha.SelectAll();
                    return SistemaGlobal.Sis.Msg.MostraMensagem("Informe a senha", "Atenção", MessageBoxButton.OK, this) != MessageBoxResult.OK;
                }
            }
            if (providerXml.InfoTimeout)
            {
                if (string.IsNullOrEmpty(providerXml.Timeout))
                {
                    txtTimeout.Focus();
                    return SistemaGlobal.Sis.Msg.MostraMensagem("Informe o timeout!", "Atenção", this) != MessageBoxResult.OK;
                }
            }
            return true;
        }

        private void Cancelar_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void PasswordBox_PasswordChanged_1(object sender, RoutedEventArgs e)
        {
            providerXml.Senha = ((PasswordBox)sender).Password;
        }

        private void chkAutenticacao_Checked_1(object sender, RoutedEventArgs e)
        {
            txtUsuario.Focus();
            txtUsuario.SelectAll();
        }

        private void chkAutenticacao_Unchecked_1(object sender, RoutedEventArgs e)
        {
            cmbBox.Focus();
        }

        private void chkTimeOut_Checked_1(object sender, RoutedEventArgs e)
        {
            txtTimeout.Focus();
        }

        private void chkTimeOut_Unchecked_1(object sender, RoutedEventArgs e)
        {
            providerXml.Timeout = "";
        }

        private void txtTimeout_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !ValidaIsDigit(e.Text);
        }

        private bool ValidaIsDigit(string text)
        {
            foreach (char c in text)
                if (!char.IsDigit(c))
                    return false;
            return true;
        }
    }
}
