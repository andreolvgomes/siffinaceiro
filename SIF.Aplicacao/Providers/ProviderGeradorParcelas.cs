using SIF.WPF.Styles.Windows.Controls;
using SIF.Dao;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SIF.Aplicacao
{
    public class ProviderGeradorParcelas : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public List<string> Financeiro
        {
            get
            {
                return new List<string>() { "CONTAS A PAGAR", "CONTAS A RECEBER" };
            }
        }

        private ObservableCollection<Crfinanceiro> _Parcelas;
        public ObservableCollection<Crfinanceiro> Parcelas
        {
            get { return _Parcelas; }
            set
            {
                if (_Parcelas != value)
                {
                    _Parcelas = value;
                    NotifyPropertyChanged("Parcelas");
                }
            }
        }

        private bool _DiaVencimentoFixo;

        public bool DiaVencimentoFixo
        {
            get { return _DiaVencimentoFixo; }
            set
            {
                if (_DiaVencimentoFixo != value)
                {
                    _DiaVencimentoFixo = value;
                    NotifyPropertyChanged("DiaVencimentoFixo");
                }
            }
        }
        

        private Clientes _cliente;
        public Clientes Cliente
        {
            get { return _cliente; }
            set
            {
                if (_cliente != value)
                {
                    _cliente = value;
                    NotifyPropertyChanged("Cliente");
                }
            }
        }

        private Fpagamentos _fpagamento;
        public Fpagamentos Fpagamento
        {
            get { return _fpagamento; }
            set
            {
                if (_fpagamento != value)
                {
                    _fpagamento = value;
                    NotifyPropertyChanged("Fpagamento");
                }
            }
        }

        private Planocontas _planoConta;
        public Planocontas PlanoConta
        {
            get { return _planoConta; }
            set
            {
                if (_planoConta != value)
                {
                    _planoConta = value;
                    NotifyPropertyChanged("PlanoConta");
                }
            }
        }

        private GeradorParcelaModel _ContextoInformacao;
        public GeradorParcelaModel ContextoInformacao
        {
            get { return _ContextoInformacao; }
            set
            {
                if (_ContextoInformacao != value)
                {
                    _ContextoInformacao = value;
                    NotifyPropertyChanged("ContextoInformacao");
                }
            }
        }

        //private DaoGenerico<Crfinanceiro> provider;
        private ProviderRecord<Crfinanceiro> provider;

        public ConnectionDb conexao;

        public ProviderGeradorParcelas(ConnectionDb conexao)
        {
            this.conexao = conexao;
            provider = new ProviderRecord<Crfinanceiro>();
            CriaNovo();

            ContextoInformacao.FinanceiroSelecionado = Financeiro.FirstOrDefault();
        }

        public void CriaNovo()
        {
            Parcelas = new ObservableCollection<Crfinanceiro>();
            ContextoInformacao = new GeradorParcelaModel();
            ContextoInformacao.Observacao = "";
            Cliente = new Clientes();
            Fpagamento = new Fpagamentos();
            PlanoConta = new Planocontas();
        }

        internal void SetCliente(Clientes cliente)
        {
            if (cliente != null)
            {
                this.Cliente = cliente;
            }
        }

        internal void SetFpagamento(Fpagamentos fpagamento)
        {
            if (fpagamento != null)
            {
                this.Fpagamento = fpagamento;
            }
        }

        internal void SetPlanoConta(Planocontas plano)
        {
            if (plano != null)
            {
                this.PlanoConta = plano;
            }
        }

        internal TextBoxPesquisa GetTextBoxPesquisa(System.Windows.Controls.TextBox textBox)
        {
            switch (textBox.Name)
            {
                case "txtCliente":
                    return TextBoxPesquisa.Clientes;
                case "txtFpagamento":
                    return TextBoxPesquisa.FormasPagamento;
                case "txtPlanoContas":
                    return TextBoxPesquisa.PlanosContas;
            }
            return TextBoxPesquisa.Nenhum;
        }

        internal void GeraVisualizacao()
        {
            Parcelas = new ObservableCollection<Crfinanceiro>(GetParcelas());
        }

        private IEnumerable<Crfinanceiro> GetParcelas()
        {
            List<Crfinanceiro> result = new List<Crfinanceiro>();
            int quantidade = Convert.ToInt16(ContextoInformacao.QuantidadeParcelas);

            for (int i = 1; i <= quantidade; i++)
            {
                Crfinanceiro parcela = new Crfinanceiro();
                if (ContextoInformacao.FinanceiroSelecionado == "CONTAS A RECEBER")
                    parcela.Crf_tipoconta = "CR";
                else
                    parcela.Crf_tipoconta = "CP";
                parcela.Cli_codigo = Cliente.Cli_codigo;
                parcela.Pla_numeroconta = PlanoConta.Pla_numeroconta;
                parcela.Fpa_codigo = Fpagamento.Fpa_codigo;

                parcela.Crf_ndocumento = ContextoInformacao.NumeroDocumento;
                parcela.Crf_parcela = i.ToString().PadLeft(2, '0');
                parcela.Crf_valorparcela = Convert.ToDecimal(ContextoInformacao.ValorTotal) / quantidade;
                parcela.Crf_valorareceber = parcela.Crf_valorparcela;
                parcela.Crf_valordocumento = Convert.ToDecimal(ContextoInformacao.ValorTotal);
                parcela.Crf_observacao = ContextoInformacao.Observacao;
                parcela.Crf_datavencimento = GetVencimento(i, Convert.ToDateTime(ContextoInformacao.DataVencimento));
                parcela.Crf_datalancamento = DateTime.Now;
                result.Add(parcela);
            }
            return result;
        }

        private DateTime GetVencimento(int i, DateTime dateTime)
        {

            if (DiaVencimentoFixo)
            {
                if (i == 1)
                    return dateTime.Date;
                else
                    return dateTime.AddMonths(i - 1).Date;
            }
            else
            {
                if (i == 1)
                    return dateTime.Date;
                else
                    return dateTime.AddDays(30 * (i - 1)).Date;
            }
        }

        internal bool GravaParcelas()
        {
            bool gerada = false;
            try
            {
                Parcelas = new ObservableCollection<Crfinanceiro>(GetParcelas());
                foreach (Crfinanceiro crfinanceiro in Parcelas)
                {
                    provider.Insert(crfinanceiro);
                    gerada = true;
                }
            }
            catch (Exception ex)
            {
                return SistemaGlobal.Sis.Log.LogError(ex, true, null);
            }
            return gerada;
        }

        internal bool ValidaCliente(Window owner)
        {
            if (string.IsNullOrEmpty(Cliente.Cli_nome))
                return SistemaGlobal.Sis.Msg.MostraMensagem("Informe o nome do cliente!", "Atenção", MessageBoxButton.OK, owner) != MessageBoxResult.OK;
            if (ClientesPes.PegaCliente(conexao, Cliente.Cli_codigo).Resultado == ResultadoPesquisa.NAO_ENCONTRADO)
                return SistemaGlobal.Sis.Msg.MostraMensagem("Cliente não cadastrado!", "Atenção", MessageBoxButton.OK, owner) != MessageBoxResult.OK;

            return true;
        }

        internal bool ValidaFpagamento(Window owner)
        {
            if (string.IsNullOrEmpty(Fpagamento.Fpa_descricao))
                return SistemaGlobal.Sis.Msg.MostraMensagem("Informe a Forma de Pagamento!", "Atenção", MessageBoxButton.OK, owner) != MessageBoxResult.OK;
            if (FpagamentosPes.PegaFpagamento(conexao, Fpagamento.Fpa_codigo).Resultado == ResultadoPesquisa.NAO_ENCONTRADO)
                return SistemaGlobal.Sis.Msg.MostraMensagem("Forma de pagamento não cadastrada!", "Atenção", MessageBoxButton.OK, owner) != MessageBoxResult.OK;
            return true;
        }

        internal bool ValidaNdocumento(Window owner)
        {
            if (string.IsNullOrEmpty(ContextoInformacao.NumeroDocumento))
                return SistemaGlobal.Sis.Msg.MostraMensagem("Informe o número do documento!", "Atenção", MessageBoxButton.OK, owner) != MessageBoxResult.OK;
            return true;
        }

        internal bool ValidaFinanceiro(Window owner)
        {
            if (string.IsNullOrEmpty(ContextoInformacao.FinanceiroSelecionado))
                return SistemaGlobal.Sis.Msg.MostraMensagem("Informe o Financeiro!", "Atenção", MessageBoxButton.OK, owner) != MessageBoxResult.OK;
            return true;
        }

        internal bool ValidaValor(Window owner)
        {
            if (string.IsNullOrEmpty(ContextoInformacao.ValorTotal))
                return SistemaGlobal.Sis.Msg.MostraMensagem("Informe um valor!", "Atenção", MessageBoxButton.OK, owner) != MessageBoxResult.OK;
            decimal valor = 0;
            if (!decimal.TryParse(ContextoInformacao.ValorTotal, out valor))
                return SistemaGlobal.Sis.Msg.MostraMensagem("Valor inválido!", "Atenção", MessageBoxButton.OK, owner) != MessageBoxResult.OK;
            return true;
        }

        internal bool ValidaQtParcela(Window owner)
        {
            int qtparcela = 0;
            if (!int.TryParse(ContextoInformacao.QuantidadeParcelas.ToString(), out qtparcela))
                return SistemaGlobal.Sis.Msg.MostraMensagem("Informe uma quantidade de parcela!", "Atenção", MessageBoxButton.OK, owner) != MessageBoxResult.OK;
            if (qtparcela <= 0)
                return SistemaGlobal.Sis.Msg.MostraMensagem("Quantidade de parcela inválida!", "Atenção", MessageBoxButton.OK, owner) != MessageBoxResult.OK;
            return true;
        }

        internal bool ValidaVencimento(Window owner)
        {
            DateTime date;
            if (string.IsNullOrEmpty(ContextoInformacao.DataVencimento))
                return SistemaGlobal.Sis.Msg.MostraMensagem("Informe um vencimento!", "Atenção", MessageBoxButton.OK, owner) != MessageBoxResult.OK;
            if (!DateTime.TryParse(ContextoInformacao.DataVencimento, out date))
                return SistemaGlobal.Sis.Msg.MostraMensagem("Vencimento inválido!", "Atenção", MessageBoxButton.OK, owner) != MessageBoxResult.OK;
            //if (date.Date < DateTime.Now.Date)
            //    return SistemaGlobal.Local.Msg.MostraMensagem("Vencimento inválido!\nData de Vencimento não pode ser manor que a data atual!", "Atenção", MessageBoxButton.OK, owner) != MessageBoxResult.OK;
            return true;
        }

        internal bool ValidaPlanoPagamento(Window owner)
        {
            if (string.IsNullOrEmpty(PlanoConta.Pla_numeroconta))
                return SistemaGlobal.Sis.Msg.MostraMensagem("Informe um plano de contas!", "Atenção", MessageBoxButton.OK, owner) != MessageBoxResult.OK;
            if (PlanoContasPes.PegaPlanoPagamento(conexao, PlanoConta.Pla_numeroconta).Resultado == ResultadoPesquisa.NAO_ENCONTRADO)
                return SistemaGlobal.Sis.Msg.MostraMensagem("Plano de Contas não cadastrado!", "Atenção", MessageBoxButton.OK, owner) != MessageBoxResult.OK;
            return true;
        }
    }
}
