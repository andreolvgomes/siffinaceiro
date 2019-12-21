using SIF.Dao;
using SIF.Aplicacao.UserControls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace SIF.Aplicacao
{
    public class ProviderInterfacesCadastros2<T> : ProviderDbDAORecords2<T>, INotifyPropertyChanged where T : IModelProperty
    {
        private T _entidade_clone;

        private T _entidade;

        public T Entidade
        {
            get { return _entidade; }
            set
            {
                if (_entidade != value)
                {
                    if (_entidade != null)
                        ((T)_entidade).PropertyChanged -= Notify_PropertyChangedValue;

                    _entidade = value;
                    OnPropertyChanged("Entidade");

                    if (_entidade != null)
                    {
                        if (ComandoCorrente != ControleButtons.New)
                            OnEvent_TrocaValoresBaseToInterfaceBucandoEventHandler(_entidade);

                        this._entidade_clone = (T)_entidade.Clone();
                        ((T)_entidade).PropertyChanged += Notify_PropertyChangedValue;
                    }
                }
            }
        }

        private void Notify_PropertyChangedValue(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            OnEvent_PropertyChangedEntidadeEventHandler(sender, e);

            if (ComandoCorrente == ControleButtons.Navigate)
            {
                ComandoCorrente = ControleButtons.Edit;
                SetNotifyButtonManutencao();
            }
        }

        public bool ExcluirNaBase { get; set; }

        public event PropertyChangedEventHandler Event_PropertyChangedEntidadeEventHandler;
        public event ExecucaoCommandEventHandler<T> Event_ConvertValoresDatabaseToInterfaceEventHandler;
        public event ExecucaoCommandEventHandler<T> Event_ConvertValoresInterfaceToDatabaseSaveEventHandler;
        public event ExecucaoCommandsComDaoSqlBooleanEventHandler<T> Event_ValidacaoCamposCorrenteEventHandler;
        public event ExecutacaoCommandsComDaoSqlModelEventHandler<T> Event_GetNewRecordEventHandler;
        public event ExecucaoCommandEventHandler<T> Event_ModelRefreshEventHandler;
        public event ExecucaoCommandEventHandler<T> Event_ExecutandoDeleteEventHandler;
        public event ModelPesquisaEventHandler Event_PesquisaEventHandler;
        public event ExecutaCommandsBemSucedidaEventHandler<T> Event_EntidadeInseridaComSucessoEventHandler;
        public event ExecutaCommandsNotificacaoEventHandler Event_CommandExecutadoEventHandler;
        public event ExecucaoCommandsComDaoSqlBooleanEventHandler<T> Event_ConfirmacaoDeleteEventHandler;

        public event ExecucaoCommandsCadastrosEventHandler Event_UpdateBemSucedidoEventHandler;
        public event ExecucaoCommandsCadastrosEventHandler Event_InsertBemSucedidoEventHandler;

        public ControleButtons ComandoCorrente { get; set; }
        public Buttons ControlButtons;
        private Window janela = null;

        public ProviderInterfacesCadastros2(Window janela, Buttons controlButtons, ConnectionDb conexao)
            : base(conexao)
        {
            this.ExcluirNaBase = true;
            this.ControlButtons = controlButtons;

            this.janela = janela;
            this.ComandoCorrente = ControleButtons.Navigate;
            this.ControlButtons.VisivelCancelar = this.ControlButtons.VisivelSalvar = false;

            this.ControlButtons.Event_DelegateExecuteButton += new DelegateExecuteButton(Control_Buttons);

            this.janela.Closing += new CancelEventHandler(Janela_Closing);
        }

        private void Janela_Closing(object sender, CancelEventArgs e)
        {
            if (this.ComandoCorrente == ControleButtons.New)
            {
                MessageBoxResult r = SistemaGlobal.Sis.Msg.MostraMensagem("Registro não foi salvo, deseja salvar ?", "Atenção", MessageBoxButton.YesNoCancel, janela);
                switch (r)
                {
                    case MessageBoxResult.Cancel:
                        e.Cancel = true;
                        break;
                    case MessageBoxResult.Yes:
                        if (this.OnEvent_ValidacaoCamposCorrenteEventHandler(this.Entidade))
                            this.Control_Buttons(ButtonType.Save);
                        else
                            e.Cancel = true;
                        break;
                }
            }
            else if (this.ComandoCorrente == ControleButtons.Edit)
            {
                MessageBoxResult r = SistemaGlobal.Sis.Msg.MostraMensagem("Registro em edição, deseja salvar ?", "Atenção", MessageBoxButton.YesNoCancel, janela);
                switch (r)
                {
                    case MessageBoxResult.Cancel:
                        e.Cancel = true;
                        break;
                    case MessageBoxResult.Yes:
                        if (this.OnEvent_ValidacaoCamposCorrenteEventHandler(this.Entidade))
                            this.Control_Buttons(ButtonType.Save);
                        else
                            e.Cancel = true;
                        break;
                }
            }
        }

        internal void LoadRegistros(string tabela, string campo_ordernacao)
        {
            this.LoadRegistros(tabela, campo_ordernacao, "");
        }

        internal void LoadRegistros(string tabela, string campo_ordernacao, string where)
        {
            this.Entidade = base.LoadRecords(tabela, campo_ordernacao, where);
        }

        public void Control_Buttons(ButtonType button)
        {
            try
            {
                switch (button)
                {
                    case ButtonType.New:
                        ComandoCorrente = ControleButtons.New;
                        SetNotifyButtonManutencao();
                        Entidade = OnEvent_GetNewRecordEventHandler();
                        break;
                    case ButtonType.Save:
                        if (OnEvent_ValidacaoCamposCorrenteEventHandler(Entidade))
                        {
                            this.OnEvent_TrocaValoresInterfaceToBaseSalvandoEventHandler(Entidade);

                            if (ComandoCorrente == ControleButtons.New)
                            {                                
                                this.Insert(Entidade);

                                SetNotifyButtonNavigate();
                                ComandoCorrente = ControleButtons.Navigate;
                                OnEntidadeInseridaComSucessoEventHandler(Entidade, true);

                                Notification.Balloon b = new Notification.Balloon(janela, "Registro salvo com sucesso!");
                                b.Show();
                            }
                            else if (ComandoCorrente == ControleButtons.Edit)
                            {                                
                                Update(Entidade, _entidade_clone);
                                SetNotifyButtonNavigate();
                                ComandoCorrente = ControleButtons.Navigate;

                                this.OnEvent_UpdateBemSucedidoEventHandler();

                                Notification.Balloon b = new Notification.Balloon(janela, "Alterações salva com sucesso!");
                                b.Show();
                            }
                        }
                        break;
                    case ButtonType.Cancel:
                        SetNotifyButtonNavigate();
                        if (ComandoCorrente == ControleButtons.Edit)
                        {
                            this.Entidade = _entidade_clone;
                            OnEvent_ModelRefreshEventHandler(Entidade);
                        }
                        else if (ComandoCorrente == ControleButtons.New)
                        {
                            this.Entidade = FirstLastOrDefault();
                        }
                        OnEvent_TrocaValoresBaseToInterfaceBucandoEventHandler(_entidade);

                        ComandoCorrente = ControleButtons.Navigate;
                        break;
                    case ButtonType.Delete:
                        if (OnEvent_ConfirmacaoDeleteEventHandler(_entidade))
                        {
                            OnEvent_ExecutandoDeleteEventHandler(Entidade);
                            Delete(Entidade);
                            this.Entidade = FirstLastOrDefault();
                            Notification.Balloon b = new Notification.Balloon(janela, "Registro excluído com sucesso!");
                        }
                        break;
                    case ButtonType.Next:
                        this.Entidade = Next(this.Entidade);
                        break;
                    case ButtonType.Previous:
                        this.Entidade = this.Previous(this.Entidade);
                        break;
                    case ButtonType.First:
                        this.Entidade = First(this.Entidade);
                        break;
                    case ButtonType.Last:
                        this.Entidade = Last(this.Entidade);
                        break;
                    case ButtonType.Pesquisa:
                        OnEvent_PesquisaEventHandler();
                        break;
                }
                this.OnEvent_CommandExecutadoEventHandler(button);
            }
            catch (Exception ex)
            {
                SistemaGlobal.Sis.Log.LogError(ex, true, this.janela);
            }
        }

        public void SetNotifyButtonManutencao()
        {
            this.ControlButtons.VisivelNovo = this.ControlButtons.VisivelExcluir = this.ControlButtons.VisivelNavegacao = this.ControlButtons.VisivelPesquisa = false; ;
            this.ControlButtons.VisivelCancelar = this.ControlButtons.VisivelSalvar = true;
        }

        public void SetNotifyButtonNavigate()
        {
            this.ControlButtons.VisivelCancelar = this.ControlButtons.VisivelSalvar = false;
            this.ControlButtons.VisivelNovo = this.ControlButtons.VisivelExcluir = this.ControlButtons.VisivelNavegacao = this.ControlButtons.VisivelPesquisa = true;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler propertyChanged = PropertyChanged;
            if (propertyChanged != null)
            {
                propertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        internal void SetEntidade(T model)
        {
            if (model != null)
                this.Entidade = model;
        }

        private void OnEvent_PropertyChangedEntidadeEventHandler(object sender, PropertyChangedEventArgs e)
        {
            PropertyChangedEventHandler event_ = Event_PropertyChangedEntidadeEventHandler;
            if (event_ != null)
                event_(sender, e);
        }

        private void OnEvent_TrocaValoresBaseToInterfaceBucandoEventHandler(T model)
        {
            if (model != null)
            {
                ExecucaoCommandEventHandler<T> event_ = Event_ConvertValoresDatabaseToInterfaceEventHandler;
                if (event_ != null)
                    event_(model);
            }
        }

        private void OnEvent_TrocaValoresInterfaceToBaseSalvandoEventHandler(T model)
        {
            ExecucaoCommandEventHandler<T> event_ = Event_ConvertValoresInterfaceToDatabaseSaveEventHandler;
            if (event_ != null)
                event_(model);
        }

        private bool OnEvent_ValidacaoCamposCorrenteEventHandler(T model)
        {
            ExecucaoCommandsComDaoSqlBooleanEventHandler<T> event_ = Event_ValidacaoCamposCorrenteEventHandler;
            if (event_ == null)
                throw new Exception("O event Event_ValidacaoCamposCorrente não pode ser NULL");
            return event_(model);
        }

        public T OnEvent_GetNewRecordEventHandler()
        {
            ExecutacaoCommandsComDaoSqlModelEventHandler<T> event_ = Event_GetNewRecordEventHandler;
            if (event_ == null)
                throw new Exception("O event Event_GetNewRecord não pode ser NULL");
            return event_();
        }

        private void OnEvent_ModelRefreshEventHandler(T model)
        {
            ExecucaoCommandEventHandler<T> event_ = Event_ModelRefreshEventHandler;
            if (event_ != null)
                Event_ModelRefreshEventHandler(model);
        }

        private void OnEvent_ExecutandoDeleteEventHandler(T model)
        {
            ExecucaoCommandEventHandler<T> event_ = Event_ExecutandoDeleteEventHandler;
            if (event_ != null)
                event_(model);
        }

        private void OnEvent_PesquisaEventHandler()
        {
            ModelPesquisaEventHandler event_ = Event_PesquisaEventHandler;
            if (event_ != null)
                event_();
        }

        private void OnEntidadeInseridaComSucessoEventHandler(T model, bool sucesso)
        {
            ExecutaCommandsBemSucedidaEventHandler<T> event_ = Event_EntidadeInseridaComSucessoEventHandler;
            if (event_ != null)
                event_(model, sucesso);
        }

        private void OnEvent_CommandExecutadoEventHandler(ButtonType type)
        {
            ExecutaCommandsNotificacaoEventHandler event_ = Event_CommandExecutadoEventHandler;
            if (event_ != null)
                event_(type);
        }

        private bool OnEvent_ConfirmacaoDeleteEventHandler(T model)
        {
            ExecucaoCommandsComDaoSqlBooleanEventHandler<T> event_ = Event_ConfirmacaoDeleteEventHandler;
            if (event_ != null)
                return event_(model);
            return true;
        }

        private void OnEvent_UpdateBemSucedidoEventHandler()
        {
            ExecucaoCommandsCadastrosEventHandler _event = Event_UpdateBemSucedidoEventHandler;
            if (_event != null)
                _event();
        }
    }
}
