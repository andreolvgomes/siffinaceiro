using SIF;
using SIF.Dao;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SIF.Aplicacao.Providers
{
    public class ProviderFaturamento : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private void OnNotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        private Crfinanceiro _Conta;

        public Crfinanceiro Conta
        {
            get { return _Conta; }
            set
            {
                if (_Conta != value)
                {
                    _Conta = value;
                    OnNotifyPropertyChanged("Conta");
                }
            }
        }

        private bool _FaturamentoIniciada;
        /// <summary>
        /// Se o faturamento correte já foi iniciado
        /// </summary>
        public bool FaturamentoIniciada
        {
            get { return _FaturamentoIniciada; }
            set
            {
                if (_FaturamentoIniciada != value)
                {
                    _FaturamentoIniciada = value;
                    OnNotifyPropertyChanged("FaturamentoIniciada");
                }
            }
        }
        
        private Crfinanceiro _lancamento;

        public Crfinanceiro Lancamento
        {
            get { return _lancamento; }
            set
            {
                if (_lancamento != value)
                {
                    _lancamento = value;
                    OnNotifyPropertyChanged("Lancamento");
                }
            }
        }

        private ObservableCollection<Crfinanceiro> _ListLancamentos;

        public ObservableCollection<Crfinanceiro> ListLancamentos
        {
            get { return _ListLancamentos; }
            set
            {
                if (_ListLancamentos != value)
                {
                    _ListLancamentos = value;
                    OnNotifyPropertyChanged("ListLancamentos");
                }
            }
        }

        private ObservableCollection<Crfinanceiro> _ListContas;

        public ObservableCollection<Crfinanceiro> ListContas
        {
            get { return _ListContas; }
            set
            {
                if (_ListContas != value)
                {
                    _ListContas = value;
                    OnNotifyPropertyChanged("ListContas");
                }
            }
        }

        public List<string> Financeiro
        {
            get
            {
                return new List<string>() { "CONTAS A PAGAR", "CONTAS A RECEBER" };
            }
        }

        private string _FinanceiroSelected;

        public string FinanceiroSelected
        {
            get { return _FinanceiroSelected; }
            set
            {
                if (_FinanceiroSelected != value)
                {
                    _FinanceiroSelected = value;
                    OnNotifyPropertyChanged("FinanceiroSelected");
                }
            }
        }

        private Faturamentos _faturamentos;

        public Faturamentos Faturamentos
        {
            get { return _faturamentos; }
            set
            {
                if (_faturamentos != value)
                {
                    _faturamentos = value;
                    OnNotifyPropertyChanged("Faturamentos");
                }
            }
        }

        private ConnectionDb conexao = null;
        private DAOFaturamentos daoFaturamentos = null;

        public ProviderFaturamento(ConnectionDb conexao)
        {
            this.conexao = conexao;
            this.daoFaturamentos = new DAOFaturamentos(conexao);
            this.FinanceiroSelected = this.Financeiro.FirstOrDefault();
        }

        public void NovoFaturamento()
        {
            this.Faturamentos = new Faturamentos();

            this.ListContas = new ObservableCollection<Crfinanceiro>();
            this.ListLancamentos = new ObservableCollection<Crfinanceiro>();

            this.NovoLancamento();
            this.NovaConta();

            this.FaturamentoIniciada = false;
        }

        public void NovoLancamento()
        {
            Lancamento = new Crfinanceiro();
            Lancamento.DescricaoFpagamento = "";
            Lancamento.Pla_numeroconta = "";
            Lancamento.Crf_parcela = "";
            Lancamento.Vencimento = "";
            Lancamento.ValorAReceber = "";
            Lancamento.Crf_ndocumento = "";
        }

        public void NovaConta()
        {
            Conta = new Crfinanceiro();
            Conta.Crf_sequencialString = "";
            Conta.Crf_valorareceber = 0;
            Conta.Crf_sequencialString = "";
        }

        /// <summary>
        /// Set uma Forma de pagamento
        /// </summary>
        /// <param name="fpagamento"></param>
        internal void SetFpagamento(Fpagamentos fpagamento)
        {
            if (fpagamento != null)
            {
                this.Lancamento.DescricaoFpagamento = fpagamento.Fpa_descricao;
                this.Lancamento.Fpa_codigo = fpagamento.Fpa_codigo;
            }
        }

        /// <summary>
        /// Set um Plano de Contas
        /// </summary>
        /// <param name="plano"></param>
        internal void SetPlanoConta(Planocontas plano)
        {
            if (plano != null)
            {
                this.Lancamento.Pla_numeroconta = plano.Pla_numeroconta;
            }
        }

        /// <summary>
        /// Set um faturamento existente
        /// </summary>
        /// <param name="faturamentos"></param>
        internal void SetFaturamentos(Faturamentos faturamentos)
        {
            if (faturamentos != null)
            {
                this.Faturamentos = faturamentos;
                this.Faturamentos.Cli_nomeString = SistemaGlobal.Sis.Connection.GetDataRowFirst(string.Format("SELECT TOP 1 Cli_nome FROM dbo.Clientes WHERE Cli_codigo = {0}", faturamentos.Cli_codigo)).Get<string>();
                this.ListContas = SistemaGlobal.Sis.Connection.GetRegistros<Crfinanceiro>(string.Format("Fat_sequencial_dest = {0}", faturamentos.Fat_sequencial)).ToObservableCollection<Crfinanceiro>();
                this.ListLancamentos = SistemaGlobal.Sis.Connection.GetRegistros<Crfinanceiro>(string.Format("Fat_sequencial_origem = {0}", faturamentos.Fat_sequencial)).ToObservableCollection<Crfinanceiro>();

                this.FaturamentoIniciada = true;
            }
        }

        /// <summary>
        /// Set uma conta
        /// </summary>
        /// <param name="crfinanceiro"></param>
        internal void SetCrfinanceiro(Crfinanceiro crfinanceiro)
        {
            if (crfinanceiro != null)
            {
                this.Conta = crfinanceiro;
                this.Conta.Crf_sequencialString = crfinanceiro.Crf_sequencial.ToString();
                this.Conta.NomeCliente = ClientesPes.PegaCliente(SistemaGlobal.Sis.Connection, crfinanceiro.Cli_codigo).Clientes.FirstOrDefault().Cli_nome;
            }
        }

        internal bool ValidaCrf_sequencial(Window owner)
        {
            try
            {
                if (string.IsNullOrEmpty(this.Conta.Crf_sequencialString))
                    return SistemaGlobal.Sis.Msg.MostraMensagem("Nº Sequêncial não pode ser vazio!", "Atenção", owner) != MessageBoxResult.OK;

                Crfinanceiro crfinanceiro = this.ListContas.FirstOrDefault(c => c.Crf_sequencial == Convert.ToInt16(this.Conta.Crf_sequencialString));
                if (crfinanceiro != null)
                    return SistemaGlobal.Sis.Msg.MostraMensagem("Conta já lançada!", "Atenção", owner) != MessageBoxResult.OK;
                crfinanceiro = CrfinanceiroPes.PegaCrfinanceiro(SistemaGlobal.Sis.Connection, Convert.ToInt16(this.Conta.Crf_sequencialString)).Crfinanceiros.FirstOrDefault();
                if (crfinanceiro == null)
                    return SistemaGlobal.Sis.Msg.MostraMensagem("Conta não encontrada!", "Atenção", owner) != MessageBoxResult.OK;

                this.SetCrfinanceiro(crfinanceiro);
            }
            catch (Exception ex)
            {
                return SistemaGlobal.Sis.Log.LogError(ex, true, owner);
            }
            return true;
        }

        /// <summary>
        /// Valida forma de pagamento
        /// </summary>
        /// <param name="owner"></param>
        /// <returns></returns>
        internal bool ValidaFpagamentos(Window owner)
        {
            try
            {
                if (string.IsNullOrEmpty(this.Lancamento.DescricaoFpagamento))
                    return SistemaGlobal.Sis.Msg.MostraMensagem("F.Pagamento não pode ser vazio!", "Atenção", owner) != MessageBoxResult.OK;
                Fpagamentos fpagamentos = FpagamentosPes.PegaFpagamentoByDescricao(conexao, this.Lancamento.DescricaoFpagamento).Fpagamentos.FirstOrDefault();
                if (fpagamentos == null)
                    return SistemaGlobal.Sis.Msg.MostraMensagem("Forma de pagamento não cadastrada!", "Atenção", owner) != MessageBoxResult.OK;
                this.SetFpagamento(fpagamentos);
            }
            catch (Exception ex)
            {
                return SistemaGlobal.Sis.Log.LogError(ex, true, owner);
            }
            return true;
        }

        /// <summary>
        /// Valida plano de contas
        /// </summary>
        /// <param name="owner"></param>
        /// <returns></returns>
        internal bool ValidaPla_numeroconta(Window owner)
        {
            try
            {
                if (string.IsNullOrEmpty(this.Lancamento.Pla_numeroconta))
                    return SistemaGlobal.Sis.Msg.MostraMensagem("Plano de Contas não cadastrado!", "Atenção", owner) != MessageBoxResult.OK;
                Planocontas planocontas = PlanoContasPes.PegaPlanoPagamento(conexao, this.Lancamento.Pla_numeroconta).PlanoContas.FirstOrDefault();
                if (planocontas == null)
                    return SistemaGlobal.Sis.Msg.MostraMensagem("Plano de Contas não cadastrado!", "Atenção", owner) != MessageBoxResult.OK;
                this.SetPlanoConta(planocontas);
            }
            catch (Exception ex)
            {
                return SistemaGlobal.Sis.Log.LogError(ex, true, owner); 
            }
            return true;
        }

        /// <summary>
        /// Valida o valor da nova parcela
        /// </summary>
        /// <param name="owner"></param>
        /// <returns></returns>
        internal bool ValidaValorLancamento(Window owner)
        {
            try
            {
                decimal value = this.GetValueSaldo();
                if (value <= 0)
                    return SistemaGlobal.Sis.Msg.MostraMensagem("Não há valor para ser lançado!", "Atenção", owner) != System.Windows.MessageBoxResult.OK;
                //if (Convert.ToDecimal(this.Lancamento.ValorAReceber) > value)
                //    return Sistema.Sis.Msg.MostraMensagem(string.Format("Saldo insufuciente! Resta somente {0:C}", value), "Atenção", owner) != System.Windows.MessageBoxResult.OK;
            }
            catch (Exception ex)
            {
                return SistemaGlobal.Sis.Log.LogError(ex, true, owner);
            }
            return true;
        }

        /// <summary>
        /// Valida saldo faturamento
        /// </summary>
        /// <param name="owner"></param>
        /// <returns></returns>
        internal bool ValidaSaldoFaturamento(Window owner)
        {
            try
            {
                decimal value = this.GetValueSaldo();
                if (value > 0)
                    return SistemaGlobal.Sis.Msg.MostraMensagem(string.Format("Ainda resta {0:C} para ser lançado em Parcelas! Deve lançar todo o valor restante para salvar o Faturamento!", value), "Atenção", owner) != System.Windows.MessageBoxResult.OK;
            }
            catch (Exception ex)
            {
                SistemaGlobal.Sis.Log.LogError(ex, true, owner);
            }
            return true;
        }

        /// <summary>
        /// Valida faturamento
        /// </summary>
        /// <param name="owner"></param>
        /// <returns></returns>
        internal bool ValidaFaturamento(Window owner)
        {
            try
            {
                if (this.ListLancamentos.Count == 0)
                    return SistemaGlobal.Sis.Msg.MostraMensagem("Nenhum parcela Lançada!", "Atenção", owner) != MessageBoxResult.OK;
            }
            catch (Exception ex)
            {
                return SistemaGlobal.Sis.Log.LogError(ex, true, owner);
            }
            return true;
        }

        /// <summary>
        /// Valida cliente
        /// </summary>
        /// <param name="owner"></param>
        /// <returns></returns>
        internal bool ValidaCliente(Window owner)
        {
            try
            {
                if (string.IsNullOrEmpty(this.Faturamentos.Cli_nomeString))
                    return SistemaGlobal.Sis.Msg.MostraMensagem("Cliente não pode ser vazio!", "Atenção", owner) != MessageBoxResult.OK;
                Clientes cliente = ClientesPes.PegaCliente(conexao, this.Faturamentos.Cli_nomeString).Clientes.FirstOrDefault();
                if (cliente == null)
                    return SistemaGlobal.Sis.Msg.MostraMensagem("Cliente não cadastrado!", "Atenção", owner) != MessageBoxResult.OK;

                this.SetCliente(cliente);
            }
            catch (Exception ex)
            {
                return SistemaGlobal.Sis.Log.LogError(ex, true, owner);
            }
            return true;
        }

        /// <summary>
        /// Valida exclusão do faturamento
        /// </summary>
        /// <param name="owner"></param>
        /// <returns></returns>
        internal bool ValidaExclusaoFat_sequencial(Window owner)
        {
            try
            {
                if (!this.daoFaturamentos.TemFaturamentosByFat_sequencial(Convert.ToInt16(this.Faturamentos.Fat_sequencialString)))
                    return SistemaGlobal.Sis.Msg.MostraMensagem("Faturamento não encontrada!", "Atenção", owner) != MessageBoxResult.OK;
            }
            catch (Exception ex)
            {
                return SistemaGlobal.Sis.Log.LogError(ex, true, owner);
            }
            return true;
        }

        /// <summary>
        /// Adiciona uma conta na lista de contas a faturar
        /// </summary>
        internal void AddContas()
        {
            this.ListContas.Add(this.Conta);
            this.FaturamentoIniciada = this.ListContas.Count > 0;
            this.Faturamentos.Fat_totfatura = (from i in this.ListContas select i.Crf_valorparcela).Sum();
        }

        /// <summary>
        /// Adiciona uma nova parcela
        /// </summary>
        internal void AddLancamentos()
        {
            this.Lancamento.Crf_valordocumento = this.GetValueContas();
            this.Lancamento.Crf_valorareceber = Convert.ToDecimal(this.Lancamento.ValorAReceber);
            this.Lancamento.Crf_valorparcela = Convert.ToDecimal(this.Lancamento.ValorAReceber);

            this.Lancamento.Crf_datalancamento = DateTime.Now;
            this.Lancamento.Crf_datavencimento = Convert.ToDateTime(this.Lancamento.Vencimento);

            this.ListLancamentos.Add(this.Lancamento);
        }

        internal string GetValorParcela()
        {
            return GetValueSaldo().ConvertToString();
        }

        private decimal GetValueSaldo()
        {
            return this.GetValueContas() - this.GetValueLancamentos();
        }

        public decimal GetValueContas()
        {
            return (from i in this.ListContas select i.Crf_valorparcela).Sum();
        }

        public decimal GetValueLancamentos()
        {
            return (from s in this.ListLancamentos select s.Crf_valorparcela).Sum();
        }

        public bool TemSaldo()
        {
            return this.GetValueSaldo() > 0;
        }

        internal void GravaFaturamentos()
        {
            Faturamentos.Fat_datafatura = DateTime.Now;
            Faturamentos.Fat_quantidadecontas = this.ListContas.Count;
            Faturamentos.Fat_quantidadelancamentos = this.ListLancamentos.Count;

            using (ProviderRecord<Faturamentos> dao = new ProviderRecord<Faturamentos>())
            {
                dao.Insert(Faturamentos);
            }
            using (ProviderRecord<Crfinanceiro> daoCrf = new ProviderRecord<Crfinanceiro>())
            {
                foreach (Crfinanceiro c in this.ListContas)
                {
                    c.Fat_sequencial_dest = this.Faturamentos.Fat_sequencial;
                    daoCrf.Update(c);
                }
                foreach (Crfinanceiro l in this.ListLancamentos)
                {
                    l.Fat_sequencial_origem = this.Faturamentos.Fat_sequencial;
                    l.Cli_codigo = this.Faturamentos.Cli_codigo;
                    l.Crf_tipoconta = (this.GetFinanceiro() == ContasReceberPagar.ContasPagar) ? "CP" : "CR";
                    daoCrf.Insert(l);
                }
            }
        }

        internal void SetCliente(Clientes cliente)
        {
            if (cliente != null)
            {
                this.Faturamentos.Cli_nomeString = cliente.Cli_nome;
                this.Faturamentos.Cli_codigo = cliente.Cli_codigo;
            }
        }

        internal List<int> GetListCrf()
        {
            List<int> ls = new List<int>();
            foreach (Crfinanceiro c in this.ListContas)
                ls.Add(c.Crf_sequencial);
            return ls;
        }

        internal ContasReceberPagar GetFinanceiro()
        {
            if ("CONTAS A RECEBER".Equals(FinanceiroSelected))
                return ContasReceberPagar.ContasReceber;
            return ContasReceberPagar.ContasPagar;
        }
    }
}