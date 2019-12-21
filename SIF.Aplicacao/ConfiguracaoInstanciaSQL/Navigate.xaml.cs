using SIF.Aplicacao.ConfiguracaoInstanciaSQL.UserControlPassos;
using System;
using System.Collections.Generic;
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

namespace SIF.Aplicacao.ConfiguracaoInstanciaSQL
{
    /// <summary>
    /// Interaction logic for Navigate.xaml
    /// </summary>
    public partial class Navigate// : Window
    {
        private PASSOS_CONFIGURACAO _passoCurrent;
        /// <summary>
        /// Controle de passos
        /// </summary>
        public PASSOS_CONFIGURACAO PassoCurrent
        {
            get { return _passoCurrent; }
            set
            {
                _passoCurrent = value;

                this.AtualizaUserControl(_passoCurrent);
                this.AtualizaTitulo(_passoCurrent);
                this.VisivelButtons(_passoCurrent);
            }
        }

        public UserControl _UserConrent;

        public UserControl UserConrent
        {
            get
            {
                return this._UserConrent;
            }
            set
            {
                this._UserConrent = value;

                this.main.Children.Clear();
                this.main.Children.Add(value);
            }
        }

        private ContextInicial context = null;
        private bool carregouInstancias = false;
        private bool salvo = false;

        public Navigate()
        {
            InitializeComponent();

            this.context = new ContextInicial();
            this.PassoCurrent = PASSOS_CONFIGURACAO.PASSO_ESCOLHA_ARMAZENAMENTO;
        }

        private bool PassoPermitido(PASSOS_CONFIGURACAO novo_passo)
        {
            switch (novo_passo)
            {
                case PASSOS_CONFIGURACAO.PASSO_ESCOLHA_ARMAZENAMENTO:
                    return true;
                case PASSOS_CONFIGURACAO.PASSO_ARMAZENAMENTO_EM_ARQUIVO:
                    return (this.context.Armazenamento == TIPO_ARMAZENAMENTO.ARMAZENAMENTO_EM_ARQUIVO);
                case PASSOS_CONFIGURACAO.PASSO_ARMAZENAMENTO_EM_ISGBD:
                    return (this.context.Armazenamento == TIPO_ARMAZENAMENTO.ARMAZENAMENTO_BANCO_DE_DADOS);
                case PASSOS_CONFIGURACAO.PASSO_CONCLUISAO:
                    return true;
                case PASSOS_CONFIGURACAO.PASSO_CADASTRO_USUARIO:
                    return true;
            }
            return false;
        }

        private void VisivelButtons(PASSOS_CONFIGURACAO passo)
        {
            btnSalvar.Visibility = System.Windows.Visibility.Collapsed;
            btnAnterior.Visibility = System.Windows.Visibility.Collapsed;
            btnProximo.Visibility = System.Windows.Visibility.Collapsed;

            switch (passo)
            {
                case PASSOS_CONFIGURACAO.PASSO_ESCOLHA_ARMAZENAMENTO:
                    btnProximo.Visibility = System.Windows.Visibility.Visible;
                    break;
                case PASSOS_CONFIGURACAO.PASSO_ARMAZENAMENTO_EM_ARQUIVO:
                    btnAnterior.Visibility = System.Windows.Visibility.Visible;
                    btnProximo.Visibility = System.Windows.Visibility.Visible;
                    break;
                case PASSOS_CONFIGURACAO.PASSO_ARMAZENAMENTO_EM_ISGBD:
                    btnAnterior.Visibility = System.Windows.Visibility.Visible;
                    btnProximo.Visibility = System.Windows.Visibility.Visible;
                    break;
                case PASSOS_CONFIGURACAO.PASSO_CONCLUISAO:
                    btnSalvar.Visibility = System.Windows.Visibility.Visible;
                    btnAnterior.Visibility = System.Windows.Visibility.Visible;
                    break;
                case PASSOS_CONFIGURACAO.PASSO_CADASTRO_USUARIO:
                    btnAnterior.Visibility = System.Windows.Visibility.Visible;
                    btnProximo.Visibility = System.Windows.Visibility.Visible;
                    break;
            }
        }

        private void AtualizaTitulo(PASSOS_CONFIGURACAO passo)
        {
            switch (passo)
            {
                case PASSOS_CONFIGURACAO.PASSO_ESCOLHA_ARMAZENAMENTO:
                    this.Title = "ESCOLHA DO ARMAZENAMENTO";
                    break;
                case PASSOS_CONFIGURACAO.PASSO_ARMAZENAMENTO_EM_ARQUIVO:
                    this.Title = "CONFIGURAÇÃO DO ARQUIVO";
                    break;
                case PASSOS_CONFIGURACAO.PASSO_ARMAZENAMENTO_EM_ISGBD:
                    this.Title = "CONFIGURAÇÃO DE ACESSO AO BANCO";
                    break;
                case PASSOS_CONFIGURACAO.PASSO_CONCLUISAO:
                    this.Title = "CONCLUSÃO";
                    break;
                case PASSOS_CONFIGURACAO.PASSO_CADASTRO_USUARIO:
                    this.Title = "DEFINIÇÃO DE USUÁRIO";
                    break;
            }
        }

        private void AtualizaUserControl(PASSOS_CONFIGURACAO passo)
        {
            switch (passo)
            {
                case PASSOS_CONFIGURACAO.PASSO_ESCOLHA_ARMAZENAMENTO:
                    this.UserConrent = new PassoEscolhaArmazenamento(this.context);
                    break;
                case PASSOS_CONFIGURACAO.PASSO_ARMAZENAMENTO_EM_ARQUIVO:
                    this.UserConrent = new PassoConfiguracaoArquivoBanco(this.context);
                    break;
                case PASSOS_CONFIGURACAO.PASSO_ARMAZENAMENTO_EM_ISGBD:
                    if (!carregouInstancias)
                    {
                        SistemaGlobal.Sis.Msg.ExecutaSync(this, "Carregando servidores SQL Server, aguarde ...", this.context.LoadedInstancias);
                        carregouInstancias = true;
                    }
                    this.UserConrent = new PassoConfiguracaoISGBD(this.context);
                    break;
                case PASSOS_CONFIGURACAO.PASSO_CONCLUISAO:
                    this.UserConrent = new PassoFinalizacao(this.context);
                    break;
                case PASSOS_CONFIGURACAO.PASSO_CADASTRO_USUARIO:
                    this.UserConrent = new PassosUsuarioSenha(this.context);
                    break;
            }
        }

        private bool Validacao()
        {
            switch (this.PassoCurrent)
            {
                case PASSOS_CONFIGURACAO.PASSO_ARMAZENAMENTO_EM_ARQUIVO:
                    return (this.UserConrent as PassoConfiguracaoArquivoBanco).Validacao(this);
                case PASSOS_CONFIGURACAO.PASSO_ESCOLHA_ARMAZENAMENTO:
                    return true;
                case PASSOS_CONFIGURACAO.PASSO_ARMAZENAMENTO_EM_ISGBD:
                    return (this.UserConrent as PassoConfiguracaoISGBD).Validacao(this);
                case PASSOS_CONFIGURACAO.PASSO_CADASTRO_USUARIO:
                    return (this.UserConrent as PassosUsuarioSenha).Validacao(this);
                case PASSOS_CONFIGURACAO.PASSO_CONCLUISAO:
                    return true;
            }
            return false;
        }

        private void Proximo_Click(object sender, RoutedEventArgs e)
        {
            if (this.Validacao())
            {
                if ((int)this.PassoCurrent < Enum.GetValues(typeof(PASSOS_CONFIGURACAO)).Cast<int>().Max())
                {
                    PASSOS_CONFIGURACAO novo_passo = (PASSOS_CONFIGURACAO)((int)this.PassoCurrent + 1);
                    while (!this.PassoPermitido(novo_passo))
                    {
                        novo_passo = (PASSOS_CONFIGURACAO)((int)novo_passo + 1);
                    }
                    this.PassoCurrent = novo_passo;
                }
            }
        }

        private void Anterior_Click(object sender, RoutedEventArgs e)
        {
            if ((int)this.PassoCurrent > 0)
            {
                PASSOS_CONFIGURACAO novo_passo = (PASSOS_CONFIGURACAO)((int)this.PassoCurrent - 1);
                while (!this.PassoPermitido(novo_passo))
                {
                    novo_passo = (PASSOS_CONFIGURACAO)((int)novo_passo - 1);
                }
                this.PassoCurrent = novo_passo;
            }
        }

        private void Salvar_Click(object sender, RoutedEventArgs e)
        {
            if (this.context.Save(this))
            {
                this.salvo = true;
                this.Close();
            }
        }

        public static bool Configura()
        {
            Navigate n = new Navigate();
            return n.Conf();
        }

        private bool Conf()
        {
            if (!this.context.ExistsXml)
                this.ShowDialog();
            else
                this.salvo = true;
            if (!this.salvo) return false;

            this.context.LoadSIFXml();
            return this.salvo;
        }
    }
}
