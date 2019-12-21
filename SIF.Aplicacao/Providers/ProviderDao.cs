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
    //public class ProviderDao<T> : IDaoProviders<T> where T : IModelProperty
    //{
    //    private T _entidade_clone;
    //    private T _entidade;

    //    public T Entidade
    //    {
    //        get { return _entidade; }
    //        set
    //        {
    //            if (_entidade != value)
    //            {
    //                if (_entidade != null)
    //                {
    //                    ((T)_entidade).PropertyChanged -= Notify_PropertyChangedValue;
    //                }

    //                _entidade = value;
    //                NotifyPropertyChanged("Entidade");

    //                if (_entidade != null)
    //                {
    //                    if (Event_SetTrocaValoresBaseToInterfaceBucando != null)
    //                        if (ComandoCorrente != ControleButtons.New)
    //                            Event_SetTrocaValoresBaseToInterfaceBucando(_entidade);

    //                    this._entidade_clone = (T)_entidade.Clone();
    //                    ((T)_entidade).PropertyChanged += Notify_PropertyChangedValue;                        
    //                }
    //            }
    //        }
    //    }

    //    private void Notify_PropertyChangedValue(object sender, System.ComponentModel.PropertyChangedEventArgs e)
    //    {
    //        if (Event_PropertyChangedEntidade != null)
    //            Event_PropertyChangedEntidade(sender, e);

    //        if (ComandoCorrente == ControleButtons.Navigate)
    //        {
    //            ComandoCorrente = ControleButtons.Edit;
    //            SetNotifyButtonManutencao();
    //        }
    //    }

    //    public bool ExcluirNaBase { get; set; }
    //    public IProviders Provider { get; private set; }

    //    public event PropertyChangedEventHandler Event_PropertyChangedEntidade;
    //    public event ExecutacaoCommandsComDaoSql<T> Event_SetTrocaValoresBaseToInterfaceBucando;
    //    public event ExecutacaoCommandsComDaoSql<T> Event_SetTrocaValoresInterfaceToBaseSalvando;
    //    public event ExecutacaoCommandsComDaoSqlBoolean<T> Event_ValidacaoCamposCorrente;
    //    public event ExecutacaoCommandsComDaoSqlModel<T> Event_GetNewRecord;
    //    public event ExecutacaoCommandsComDaoSql<T> Event_ModelRefresh;
    //    public event ExecutacaoCommandsComDaoSql<T> Event_ExecucaoExclusao;
    //    public event ModelPesquisaDelegate Event_Pesquisa;
    //    public event ExecutaCommandsBemSucedida<T> EntidadeInseridaComSucesso;
    //    public event ExecutaCommandsNotificacao Event_CommandExecutado;

    //    public ControleButtons ComandoCorrente { get; set; }
    //    public Buttons ControlButtons;

    //    public ProviderDao(Buttons controlButtons, ConnectioDb conexao)
    //        : this(controlButtons, conexao, null)
    //    {
    //    }
    //    public ProviderDao(Buttons controlButtons, ConnectioDb conexao, IProviders provider)
    //        : base(conexao)
    //    {
    //        ExcluirNaBase = true;
    //        this.ControlButtons = controlButtons;

    //        this.Provider = provider;

    //        this.ComandoCorrente = ControleButtons.Navigate;
    //        this.ControlButtons.VisivelCancelar = this.ControlButtons.VisivelSalvar = false;

    //        this.ControlButtons.Event_DelegateExecuteButton += new DelegateExecuteButton(Control_Buttons);
    //    }

    //    public void CarregaRegistros()
    //    {
    //        CarregaRegistros("");
    //    }

    //    public void CarregaRegistros(string where)
    //    {
    //        LoadedRecords(where);
    //        Entidade = First();
    //    }

    //    public void SetEntidade(T model)
    //    {
    //        if (model != null)
    //        {
    //            this.Entidade = model;
    //            ///posiciona index da navegação
    //            ///
    //            //new Thread(new ThreadStart(
    //            //    () =>
    //            //    {
    //            //        int i = 0;
    //            //        foreach (T item in ListaRecord)
    //            //        {
    //            //            if (item == model)
    //            //                break;
    //            //            i++;
    //            //        }
    //            //        SetIndexNavigate(i);
    //            //    })).Start();
    //        }
    //    }

    //    public void Control_Buttons(ButtonType button)
    //    {
    //        switch (button)
    //        {
    //            case ButtonType.New:
    //                ComandoCorrente = ControleButtons.New;
    //                SetNotifyButtonManutencao();
    //                Entidade = Event_GetNewRecord();
    //                break;
    //            case ButtonType.Save:
    //                if (Event_ValidacaoCamposCorrente(Entidade))
    //                {
    //                    if (Event_SetTrocaValoresInterfaceToBaseSalvando != null)
    //                        Event_SetTrocaValoresInterfaceToBaseSalvando(Entidade);

    //                    if (ComandoCorrente == ControleButtons.New)
    //                    {
    //                        SetNotifyButtonNavigate();

    //                        Insert(Entidade);
    //                        ComandoCorrente = ControleButtons.Navigate;
    //                        if (EntidadeInseridaComSucesso != null)
    //                            EntidadeInseridaComSucesso(Entidade, true);
    //                    }
    //                    else if (ComandoCorrente == ControleButtons.Edit)
    //                    {
    //                        SetNotifyButtonNavigate();
    //                        Update(Entidade, _entidade_clone);
    //                        ComandoCorrente = ControleButtons.Navigate;
    //                    }
    //                }
    //                break;
    //            case ButtonType.Cancel:
    //                SetNotifyButtonNavigate();
    //                if (ComandoCorrente == ControleButtons.Edit)
    //                {
    //                    Refresh(Entidade);
    //                    if (Event_ModelRefresh != null)
    //                        Event_ModelRefresh(Entidade);
    //                }
    //                else if (ComandoCorrente == ControleButtons.New)
    //                {
    //                    Entidade = First();
    //                }
    //                if (Event_SetTrocaValoresBaseToInterfaceBucando != null && _entidade != null)
    //                    Event_SetTrocaValoresBaseToInterfaceBucando(_entidade);

    //                ComandoCorrente = ControleButtons.Navigate;
    //                break;
    //            case ButtonType.Delete:
    //                if (ListaRecord.Count > 0)
    //                {
    //                    if (Sistema.Sis.Msg.MostraMensagem("Deseja realmente excluir?", "Atenção", MessageBoxButton.YesNo, null) == MessageBoxResult.Yes)
    //                    {
    //                        if (Event_ExecucaoExclusao != null)
    //                            Event_ExecucaoExclusao(Entidade);
    //                        Delete(Entidade);
    //                        Entidade = First();
    //                    }
    //                }
    //                break;
    //            case ButtonType.Next:
    //                Entidade = Next();
    //                break;
    //            case ButtonType.Previous:
    //                Entidade = Previous();
    //                break;
    //            case ButtonType.First:
    //                Entidade = First();
    //                break;
    //            case ButtonType.Last:
    //                Entidade = Last();
    //                break;
    //            case ButtonType.Pesquisa:
    //                if (Event_Pesquisa != null)
    //                    Event_Pesquisa();
    //                break;
    //        }
    //        if (Event_CommandExecutado != null)
    //            Event_CommandExecutado(button);
    //    }

    //    public void SetNotifyButtonManutencao()
    //    {
    //        this.ControlButtons.VisivelNovo = this.ControlButtons.VisivelExcluir = this.ControlButtons.VisivelNavegacao = this.ControlButtons.VisivelPesquisa = false; ;
    //        this.ControlButtons.VisivelCancelar = this.ControlButtons.VisivelSalvar = true;
    //    }

    //    public void SetNotifyButtonNavigate()
    //    {
    //        this.ControlButtons.VisivelCancelar = this.ControlButtons.VisivelSalvar = false;
    //        this.ControlButtons.VisivelNovo = this.ControlButtons.VisivelExcluir = this.ControlButtons.VisivelNavegacao = this.ControlButtons.VisivelPesquisa = true;
    //    }
    //}
}
