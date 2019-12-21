using SIF.WPF.Styles.Windows.Controls;
using SIF.Dao;
using SIF.Aplicacao.LayoutControle;
using SIF.Aplicacao.UserControls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace SIF.Aplicacao
{
    public class ProviderCrp : INotifyPropertyChanged, IDisposable, IProviders
    {
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private bool _Status;

        public bool Status
        {
            get { return _Status; }
            set
            {
                if (_Status != value)
                {
                    _Status = value;
                    NotifyPropertyChanged("Status");
                }
            }
        }

        private string _StatusRegistro;

        public string StatusRegistro
        {
            get { return _StatusRegistro; }
            set
            {
                if (_StatusRegistro != value)
                {
                    _StatusRegistro = value;
                    NotifyPropertyChanged("StatusRegistro");
                }
            }
        }

        private Brush _ForegroundStatus;

        public Brush ForegroundStatus
        {
            get
            {
                return _ForegroundStatus;
            }
            set
            {
                if (_ForegroundStatus != value)
                {
                    _ForegroundStatus = value;
                    NotifyPropertyChanged("ForegroundStatus");
                }
            }
        }

        public ConnectionDb Conexao;
        public ProviderInterfacesCadastros2<Crfinanceiro> Provider { get; set; }
        public ControleLayoutContasrp ControleLayout { get; private set; }
        private ContasReceberPagar interf;

        public event TextBoxSomenteIsReadOnlyEventhandler Event_TextBoxSomenteIsReadOnlyEventhandler;

        public ProviderCrp(Window janela, Buttons controlButtons, ConnectionDb conexao, ContasReceberPagar interf)
        {
            this.Conexao = conexao;
            this.interf = interf;

            ControleLayout = new ControleLayoutContasrp(interf);

            Provider = new ProviderInterfacesCadastros2<Crfinanceiro>(janela, controlButtons, conexao);
            Provider.Event_ConvertValoresDatabaseToInterfaceEventHandler += new ExecucaoCommandEventHandler<Crfinanceiro>(SetTrocaVAloresBaseToInterface);
            Provider.Event_ConvertValoresInterfaceToDatabaseSaveEventHandler += new ExecucaoCommandEventHandler<Crfinanceiro>(SetTrocaValoresInterfaceToBase);
        }

        /// <summary>
        /// Carrega os registros para memória
        /// </summary>
        internal void CarregaRegistros()
        {
            CarregaRegistros(0);
        }

        /// <summary>
        /// Carrega os registros para memória
        /// </summary>
        /// <param name="crf_sequencial">número da conta. se necessário filtrar uma conta específica</param>
        internal void CarregaRegistros(int crf_sequencial)
        {
            List<string> listFilters = new List<string>();
            if (interf == ContasReceberPagar.ContasPagar)
                listFilters.Add("Crf_tipoconta = 'CP'");
            else
                listFilters.Add("Crf_tipoconta = 'CR'");
            if (crf_sequencial > 0)
                listFilters.Add(string.Format("Crf_sequencial = {0}", crf_sequencial));

            Provider.LoadRegistros("Crfinanceiro", "Crf_sequencial", listFilters.OrganizaFiltrosWhere());
        }

        internal void SetTrocaVAloresBaseToInterface(Crfinanceiro model)
        {
            if (model.Crf_datavencimento != null)
                model.Vencimento = Convert.ToDateTime(model.Crf_datavencimento).ConvertToString();

            model.ValorParcela = model.Crf_valorparcela.ConvertToString();
            model.ValorAReceber = model.Crf_valorareceber.ConvertToString();

            if (model.Cli_codigo > 0)
                model.NomeCliente = ClientesPes.PegaCliente(Conexao, model.Cli_codigo).Clientes.FirstOrDefault().Cli_nome;
            if (!string.IsNullOrEmpty(model.Fpa_codigo))
                model.DescricaoFpagamento = FpagamentosPes.PegaFpagamento(Conexao, model.Fpa_codigo).Fpagamentos.FirstOrDefault().Fpa_descricao;

            this.Status = model.Fat_sequencial_dest > 0 || model.Fat_sequencial_origem > 0;
            bool isReadOnly = this.Status;
            if (!isReadOnly)
                isReadOnly = model.Crf_databaixa != null;
            this.OnEvent_TextBoxSomenteIsReadOnlyEventhandler(isReadOnly);

            if (model.Fat_sequencial_dest > 0)
            {
                this.StatusRegistro = string.Format("• FATURADA → DESTINO : {0}", model.Fat_sequencial_dest);
                this.ForegroundStatus = new SolidColorBrush(Colors.Red);
            }
            else if (model.Fat_sequencial_origem > 0)
            {
                this.StatusRegistro = string.Format("• FATURAMENTO → ORIGEM : {0}", model.Fat_sequencial_origem);
                this.ForegroundStatus = new SolidColorBrush(Colors.Blue);
            }
        }

        internal void SetTrocaValoresInterfaceToBase(Crfinanceiro model)
        {
            model.Crf_datavencimento = Convert.ToDateTime(model.Vencimento);
            model.Crf_valorparcela = Convert.ToDecimal(model.ValorParcela);
            if (Convert.ToDecimal(model.ValorAReceber) > 0)
                model.Crf_valorareceber = Convert.ToDecimal(model.ValorAReceber);
            else
                model.Crf_valorareceber = Convert.ToDecimal(model.ValorParcela);
            if (model.Crf_sequencial == 0)
                model.Crf_datalancamento = DateTime.Now;
        }

        private void OnEvent_TextBoxSomenteIsReadOnlyEventhandler(bool isReadOnly)
        {
            TextBoxSomenteIsReadOnlyEventhandler _event = Event_TextBoxSomenteIsReadOnlyEventhandler;
            if (_event != null)
                _event(isReadOnly);
        }

        internal bool ValidaNdocumento(Window owner)
        {
            if (string.IsNullOrEmpty(Provider.Entidade.Crf_ndocumento))
                return SistemaGlobal.Sis.Msg.MostraMensagem("Informe o Nº do documento!", "Atenção", MessageBoxButton.OK, owner) != MessageBoxResult.OK;
            return true;
        }

        internal bool ValidaVencimento(Window owner)
        {
            if (string.IsNullOrEmpty(Provider.Entidade.Vencimento))
                return SistemaGlobal.Sis.Msg.MostraMensagem("Informe o vencimento!", "Atenção", MessageBoxButton.OK, owner) != MessageBoxResult.OK;
            return true;
        }

        internal bool ValidaParcela(Window owner)
        {
            if (string.IsNullOrEmpty(Provider.Entidade.Crf_parcela))
                return SistemaGlobal.Sis.Msg.MostraMensagem("Informe o Nº do documento!", "Atenção", MessageBoxButton.OK, owner) != MessageBoxResult.OK;
            int numero = 0;
            if (!int.TryParse(Provider.Entidade.Crf_parcela, out numero))
                return SistemaGlobal.Sis.Msg.MostraMensagem("Nº inválido!", "Atenção", MessageBoxButton.OK, owner) != MessageBoxResult.OK;

            Provider.Entidade.Crf_parcela = Provider.Entidade.Crf_parcela.PadLeft(2, '0');
            //if (string.IsNullOrEmpty(Provider.Entidade.Crf_ndocumento))
            //    return SistemaGlobal.Local.Msg.MostraMensagem("Informe o Nº do documento!", "Atenção", MessageBoxButton.OK, owner) != MessageBoxResult.OK;
            return true;
        }

        internal bool ValidaCliente(Window owner)
        {
            if (string.IsNullOrEmpty(Provider.Entidade.NomeCliente))
                return SistemaGlobal.Sis.Msg.MostraMensagem("Informe o cliente!", "Atenção", MessageBoxButton.OK, owner) != MessageBoxResult.OK;
            Clientes cliente = ClientesPes.PegaCliente(Conexao, Provider.Entidade.NomeCliente).Clientes.FirstOrDefault();
            if (cliente == null)
                return SistemaGlobal.Sis.Msg.MostraMensagem("Cliente não cadastrado!", "Atenção", MessageBoxButton.OK, owner) != MessageBoxResult.OK;
            else
                Provider.Entidade.Cli_codigo = cliente.Cli_codigo;
            return true;
        }

        internal bool ValidaFpagamento(Window owner)
        {
            if (string.IsNullOrEmpty(Provider.Entidade.DescricaoFpagamento))
                return SistemaGlobal.Sis.Msg.MostraMensagem("Informe uma forma de pagamento!", "Atenção", MessageBoxButton.OK, owner) != MessageBoxResult.OK;
            Fpagamentos fpagamento = FpagamentosPes.PegaFpagamentoByDescricao(Conexao, Provider.Entidade.DescricaoFpagamento).Fpagamentos.FirstOrDefault();
            if (fpagamento == null)
                return SistemaGlobal.Sis.Msg.MostraMensagem("Forma de pagamento não cadastrada!", "Atenção", MessageBoxButton.OK, owner) != MessageBoxResult.OK;
            else
                Provider.Entidade.Fpa_codigo = fpagamento.Fpa_codigo;

            return true;
        }

        internal bool ValidaPlanoContas(Window owner)
        {
            if (string.IsNullOrEmpty(Provider.Entidade.Pla_numeroconta))
                return SistemaGlobal.Sis.Msg.MostraMensagem("Informe um plano de contas!", "Atenção", MessageBoxButton.OK, owner) != MessageBoxResult.OK;
            Planocontas plano = PlanoContasPes.PegaPlanoPagamento(Conexao, Provider.Entidade.Pla_numeroconta).PlanoContas.FirstOrDefault();
            if (plano == null)
                return SistemaGlobal.Sis.Msg.MostraMensagem("Plano de contas não cadastrado!", "Atenção", MessageBoxButton.OK, owner) != MessageBoxResult.OK;
            return true;
        }

        internal bool ValidaValor(Window owner)
        {
            if (string.IsNullOrEmpty(Provider.Entidade.Pla_numeroconta))
                return SistemaGlobal.Sis.Msg.MostraMensagem("Informe um valor!", "Atenção", MessageBoxButton.OK, owner) != MessageBoxResult.OK;
            decimal valor = 0;
            if (!decimal.TryParse(Provider.Entidade.ValorParcela, out valor))
                return SistemaGlobal.Sis.Msg.MostraMensagem("Valor inválido!", "Atenção", MessageBoxButton.OK, owner) != MessageBoxResult.OK;
            if (valor <= 0)
                return SistemaGlobal.Sis.Msg.MostraMensagem("Valor inválido!\nDeve ser maior que zero!", "Atenção", MessageBoxButton.OK, owner) != MessageBoxResult.OK;
            return true;
        }

        internal void SetCrfinanceiro(Crfinanceiro crfinanceiro)
        {
            if (crfinanceiro != null)
            {
                Provider.Entidade = crfinanceiro;
            }
        }

        internal void SetPlanoConta(Planocontas plano)
        {
            if (plano != null)
            {
                Provider.Entidade.Pla_numeroconta = plano.Pla_numeroconta;
            }
        }

        internal void SetFpagamento(Fpagamentos fpagamento)
        {
            if (fpagamento != null)
            {
                Provider.Entidade.Fpa_codigo = fpagamento.Fpa_codigo;
                Provider.Entidade.DescricaoFpagamento = fpagamento.Fpa_descricao;
            }
        }

        internal void SetCliente(Clientes cliente)
        {
            if (cliente != null)
            {
                Provider.Entidade.Cli_codigo = cliente.Cli_codigo;
                Provider.Entidade.NomeCliente = cliente.Cli_nome;
            }
        }
    }
}
