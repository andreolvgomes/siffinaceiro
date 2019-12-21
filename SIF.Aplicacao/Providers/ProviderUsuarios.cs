using SIF.WPF.Styles.Windows.Controls;
using SIF.Commom;
using SIF.Dao;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SIF.Aplicacao
{
    public class ProviderUsuarios : INotifyPropertyChanged, IDisposable
    {
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public List<string> SourcePerfil
        {
            get
            {
                return new List<string>() { "ADMINISTRADOR", "RASGADO", "FRACO", "ZÉ RUELA" };
            }
        }

        private bool _EmNavegacao = true;

        public bool EmNavegacao
        {
            get { return _EmNavegacao; }
            set
            {
                if (_EmNavegacao != value)
                {
                    _EmNavegacao = value;
                    NotifyPropertyChanged("EmNavegacao");
                }
            }
        }

        private bool _informarSenha;

        public bool InformarSenha
        {
            get { return _informarSenha; }
            set
            {
                if (_informarSenha != value)
                {
                    _informarSenha = value;
                    NotifyPropertyChanged("InformarSenha");
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private Action<Usuarios> action;
        public ProviderInterfacesCadastros2<Usuarios> Provider { get; private set; }
        public bool ValidaNomeUsuario { get; set; }

        public ProviderUsuarios(Window janela, UserControls.Buttons buttons, ConnectionDb conexao, Action<Usuarios> action)
        {
            Provider = new ProviderInterfacesCadastros2<Usuarios>(janela, buttons, conexao);

            Provider.Event_CommandExecutadoEventHandler += new ExecutaCommandsNotificacaoEventHandler(ComandoExecutado);
            Provider.Event_PropertyChangedEntidadeEventHandler += new PropertyChangedEventHandler(PropertyChangedEntidade);
            Provider.Event_ConvertValoresDatabaseToInterfaceEventHandler += new ExecucaoCommandEventHandler<Usuarios>(OrganizaValoresToInterface);
            Provider.Event_EntidadeInseridaComSucessoEventHandler += new ExecutaCommandsBemSucedidaEventHandler<Usuarios>(InsertSucesso);
            Provider.Event_ExecutandoDeleteEventHandler += new ExecucaoCommandEventHandler<Usuarios>(UsuarioExcluido);

            this.action = action;

            Provider.LoadRegistros("Usuarios", "Usu_codigo");
        }

        private void UsuarioExcluido(Usuarios model)
        {
            SistemaGlobal.Sis.Connection.ExecuteNonQueryTransaction(string.Format("DELETE FROM Uscontrolesecao WHERE Usu_codigo = {0}", model.Usu_codigo));
        }

        private void InsertSucesso(Usuarios model, bool sucesso)
        {
            InformarSenha = false;
            Provider.Entidade.Usu_codigo = SistemaGlobal.Sis.Connection.GetValue<int>("SELECT TOP 1 Usu_codigo FROM Usuarios ORDER BY Usu_codigo DESC");
            using (ProviderRecord<Uscontrolesecao> provider = new ProviderRecord<Uscontrolesecao>())
            {
                foreach (UssessaoEnum item in Enum.GetValues(typeof(UssessaoEnum)))
                {
                    Uscontrolesecao permissao = new Uscontrolesecao();
                    permissao.Usu_codigo = Provider.Entidade.Usu_codigo;
                    permissao.Uss_descricao = SIF.Commom.PegaDescricaoEnum.GetDescription(item);
                    permissao.Usc_disponivel = permissao.Usc_editar = permissao.Usc_excluir = permissao.Usc_incluir = true;
                    provider.Insert(permissao);
                }
            }
            Provider.SetNotifyButtonNavigate();
        }

        private void OrganizaValoresToInterface(Usuarios model)
        {
            if (action != null)
                action(model);
        }

        private void PropertyChangedEntidade(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Usu_nome")
            {
                ValidaNomeUsuario = true;
            }
            EmNavegacao = false;
        }

        private void ComandoExecutado(ButtonType command)
        {
            if (command == ButtonType.Cancel)
            {
                ValidaNomeUsuario = InformarSenha = false;
                EmNavegacao = true;
            }
            else if (command == ButtonType.Save)
                EmNavegacao = true;
            else if (command == ButtonType.New)
                EmNavegacao = false;
        }

        private void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        internal bool ValidaUsuario(Window owner)
        {
            if (ValidaNomeUsuario)
            {
                if (string.IsNullOrEmpty(Provider.Entidade.Usu_nome))
                    return SistemaGlobal.Sis.Msg.MostraMensagem("Informe o usuário!", "Atenção", MessageBoxButton.OK, owner) != MessageBoxResult.OK;
                //if (Provider.Tabela.Select(string.Format("Usu_nome = '{0}'", Provider.Entidade.Usu_nome)).FirstOrDefault() != null)
                //    return SistemaGlobal.Sis.Msg.MostraMensagem("Já existe um usuário cadastrado com o mesmo nome!", "Atenção", MessageBoxButton.OK, owner) != MessageBoxResult.OK;

                int count = this.Provider.Connection.GetValue<int>(string.Format("SELECT COUNT(Usu_codigo) FROM dbo.Usuarios WHERE Usu_nome = '{0}'", Provider.Entidade.Usu_nome));
                if (count > 0)
                    return SistemaGlobal.Sis.Msg.MostraMensagem("Já existe um usuário cadastrado com o mesmo nome!", "Atenção", MessageBoxButton.OK, owner) != MessageBoxResult.OK;
            }
            return true;
        }

        internal bool ValidaSenha(Window owner, string senha)
        {
            if (string.IsNullOrEmpty(senha))
                return SistemaGlobal.Sis.Msg.MostraMensagem("Informe a senha!", "Atenção", MessageBoxButton.OK, owner) != MessageBoxResult.OK;
            return true;
        }

        internal bool ValidaSenhaConf(Window owner, string senhaConf)
        {
            if (string.IsNullOrEmpty(senhaConf))
                return SistemaGlobal.Sis.Msg.MostraMensagem("Informe a senha de confirmação!", "Atenção", MessageBoxButton.OK, owner) != MessageBoxResult.OK;
            return true;
        }

        internal bool ValidaSenhaOk(Window owner, string senha, string senhaConf)
        {
            if (senha != senhaConf)
                return SistemaGlobal.Sis.Msg.MostraMensagem("Senha diferente da senha de confirmação!", "Atenção", MessageBoxButton.OK, owner) != MessageBoxResult.OK;

            Provider.Entidade.Usu_senha = senha;
            ValidaNomeUsuario = InformarSenha = false;
            return true;
        }

        internal void SetUsuario(Usuarios usuarios)
        {
            if (usuarios != null)
            {
                Provider.SetEntidade(usuarios);
            }
        }
    }
}
