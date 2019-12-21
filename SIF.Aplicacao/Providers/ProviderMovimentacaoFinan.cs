using SIF.WPF.Styles.Windows.Controls;
using SIF.Dao;
using SIF.Aplicacao.UserControls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SIF.Aplicacao
{
    public class ProviderMovimentacaoFinan : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private bool _VisualizaButtonExcluir;

        public bool VisualizaButtonExcluir
        {
            get { return _VisualizaButtonExcluir; }
            set
            {
                if (_VisualizaButtonExcluir != value)
                {
                    _VisualizaButtonExcluir = value;
                    NotifyPropertyChanged("VisualizaButtonExcluir");
                }
            }
        }


        private bool _somenteLeitura = true;
        public bool SomenteLeitura
        {
            get { return _somenteLeitura; }
            set
            {
                if (_somenteLeitura != value)
                {
                    _somenteLeitura = value;
                    NotifyPropertyChanged("SomenteLeitura");
                }
            }
        }

        public List<string> ListTipoMovimento
        {
            get
            {
                return new List<string> { "C - Crédito", "D - Débito" };
            }
        }

        public ProviderInterfacesCadastros2<Camovimentos> Provider { get; private set; }

        public ProviderMovimentacaoFinan(Window janela, Buttons controlButtons, ConnectionDb conexao)
        {
            Provider = new ProviderInterfacesCadastros2<Camovimentos>(janela, controlButtons, conexao);

            Provider.Event_ExecutandoDeleteEventHandler += new ExecucaoCommandEventHandler<Camovimentos>(ExecutaExclusao);
            Provider.Event_ConvertValoresDatabaseToInterfaceEventHandler += new ExecucaoCommandEventHandler<Camovimentos>(SetTrocaVAloresBaseToInterface);
            Provider.Event_ConvertValoresInterfaceToDatabaseSaveEventHandler += new ExecucaoCommandEventHandler<Camovimentos>(SetTrocaValoresInterfaceToBase);
            Provider.Event_ModelRefreshEventHandler += new ExecucaoCommandEventHandler<Camovimentos>(Refresh);
            Provider.Event_EntidadeInseridaComSucessoEventHandler += new ExecutaCommandsBemSucedidaEventHandler<Camovimentos>(InsertSucesso);

            VisualizaButtonExcluir = true;
            Provider.LoadRegistros("Camovimentos", "Cam_sequencial");
        }

        private void InsertSucesso(Camovimentos model, bool sucesso)
        {
            /// atualiza saldo caixa
            /// 
            using (ProviderRecord<Caixas> providerCai = new ProviderRecord<Caixas>())
            {
                Caixas caixa = CaixasPes.PegaCaixaByCodigo(SistemaGlobal.Sis.Connection, model.Cai_codigo).Caixas.FirstOrDefault();
                if (Provider.Entidade.Cam_tipomovimento == "D")
                    caixa.Cai_saldo -= Provider.Entidade.Cam_valorlancado;
                else
                    caixa.Cai_saldo += Provider.Entidade.Cam_valorlancado;
                providerCai.Update(caixa);
            }
        }

        private void ExecutaExclusao(Camovimentos model)
        {
            if (model.Crf_sequencial > 0)
            {
                using (ProviderRecord<Crfinanceiro> provider = new ProviderRecord<Crfinanceiro>())
                {
                    Crfinanceiro crfinanceiro = CrfinanceiroPes.PegaCrfinanceiro(SistemaGlobal.Sis.Connection, model.Crf_sequencial).Crfinanceiros.FirstOrDefault();
                    crfinanceiro.Crf_valorrecebido = 0;
                    crfinanceiro.Crf_databaixa = null;
                    provider.Update(crfinanceiro);
                }
            }
            using (ProviderRecord<Caixas> providerCai = new ProviderRecord<Caixas>())
            {
                Caixas caixa = CaixasPes.PegaCaixaByCodigo(SistemaGlobal.Sis.Connection, model.Cai_codigo).Caixas.FirstOrDefault();
                if (Provider.Entidade.Cam_tipomovimento == "D")
                    caixa.Cai_saldo += Provider.Entidade.Cam_valorlancado;
                else
                    caixa.Cai_saldo -= Provider.Entidade.Cam_valorlancado;
                providerCai.Update(caixa);
            }
        }

        private void Refresh(Camovimentos model)
        {
            SetTrocaVAloresBaseToInterface(model);
        }

        private void SetTrocaVAloresBaseToInterface(Camovimentos model)
        {
            SomenteLeitura = (model.Crf_sequencial != 0);
            Provider.ControlButtons.VisivelExcluir = VisualizaButtonExcluir = (model.Crf_sequencial == 0);

            model.Valor = model.Cam_valorlancado.ConvertToString();
            model.DataLancamento = Provider.Entidade.Cam_datalancamento.ConvertToString();
            model.TipoMovimentoEscolhido = (Provider.Entidade.Cam_tipomovimento == "C") ? "C - Crédito" : "D - Débito";

            model.NomeCliente = ClientesPes.PegaCliente(SistemaGlobal.Sis.Connection, model.Cli_codigo).Clientes.FirstOrDefault().Cli_nome;
            model.FpagamentoDescricao = FpagamentosPes.PegaFpagamento(SistemaGlobal.Sis.Connection, model.Fpa_codigo).Fpagamentos.FirstOrDefault().Fpa_descricao;
            model.Caixa = CaixasPes.PegaCaixaByCodigo(SistemaGlobal.Sis.Connection, model.Cai_codigo).Caixas.FirstOrDefault().Cai_descricao;
        }

        private void SetTrocaValoresInterfaceToBase(Camovimentos model)
        {
            Provider.Entidade.Cam_valorlancado = Convert.ToDecimal(model.Valor);
            Provider.Entidade.Cam_datalancamento = Convert.ToDateTime(model.DataLancamento);
            Provider.Entidade.Cam_tipomovimento = (model.TipoMovimentoEscolhido == "C - Crédito") ? "C" : "D";
        }

        internal void SetCliente(Clientes cliente)
        {
            if (cliente != null)
            {
                Provider.Entidade.NomeCliente = cliente.Cli_nome;
                Provider.Entidade.Cli_codigo = cliente.Cli_codigo;
            }
        }

        internal void SetFpagamento(Fpagamentos fpagamento)
        {
            if (fpagamento != null)
            {
                Provider.Entidade.FpagamentoDescricao = fpagamento.Fpa_descricao;
                Provider.Entidade.Fpa_codigo = fpagamento.Fpa_codigo;
            }
        }

        internal void SetCamovimentos(Camovimentos camoviemntos)
        {
            if (camoviemntos != null)
            {
                Provider.Entidade = camoviemntos;
            }
        }

        internal void SetPlanoContas(Planocontas planoContas)
        {
            if (planoContas != null)
            {
                Provider.Entidade.Pla_numeroconta = planoContas.Pla_numeroconta;
            }
        }

        internal void SetCaixa(Caixas caixa)
        {
            if (caixa != null)
            {
                Provider.Entidade.Caixa = caixa.Cai_descricao;
                Provider.Entidade.Cai_codigo = caixa.Cai_codigo;
            }
        }

        internal bool ValidaCliente(Window owner)
        {
            if (string.IsNullOrEmpty(Provider.Entidade.NomeCliente))
                return SistemaGlobal.Sis.Msg.MostraMensagem("Informe o nome do Cliente!", "Atenção", MessageBoxButton.OK, owner) != MessageBoxResult.OK;
            Clientes cliente = ClientesPes.PegaCliente(SistemaGlobal.Sis.Connection, Provider.Entidade.NomeCliente).Clientes.FirstOrDefault();
            if (cliente == null)
                return SistemaGlobal.Sis.Msg.MostraMensagem("Cliente não cadastrado!", "Atenção", MessageBoxButton.OK, owner) != MessageBoxResult.OK;
            else
                Provider.Entidade.Cli_codigo = cliente.Cli_codigo;
            return true;
        }

        internal bool ValidaFpagamento(Window owner)
        {
            if (string.IsNullOrEmpty(Provider.Entidade.FpagamentoDescricao))
                return SistemaGlobal.Sis.Msg.MostraMensagem("Informe uma Forma de Pagamento!", "Atenção", MessageBoxButton.OK, owner) != MessageBoxResult.OK;
            Fpagamentos fpagamento = FpagamentosPes.PegaFpagamentoByDescricao(SistemaGlobal.Sis.Connection, Provider.Entidade.FpagamentoDescricao).Fpagamentos.FirstOrDefault();
            if (fpagamento == null)
                return SistemaGlobal.Sis.Msg.MostraMensagem("Forma de pagamento não cadastrada!", "Atenção", MessageBoxButton.OK, owner) != MessageBoxResult.OK;
            else
                Provider.Entidade.Fpa_codigo = fpagamento.Fpa_codigo;
            return true;
        }

        internal bool ValidaPlanoContas(Window owner)
        {
            if (string.IsNullOrEmpty(Provider.Entidade.Pla_numeroconta))
                return SistemaGlobal.Sis.Msg.MostraMensagem("Informe um Plano de Contas!", "Atenção", MessageBoxButton.OK, owner) != MessageBoxResult.OK;
            Planocontas planoContas = PlanoContasPes.PegaPlanoPagamento(SistemaGlobal.Sis.Connection, Provider.Entidade.Pla_numeroconta).PlanoContas.FirstOrDefault();
            if (planoContas == null)
                return SistemaGlobal.Sis.Msg.MostraMensagem("Plano de Contas não cadastrado!", "Atenção", MessageBoxButton.OK, owner) != MessageBoxResult.OK;
            return true;
        }

        internal bool ValidaCaixa(Window owner)
        {
            if (string.IsNullOrEmpty(Provider.Entidade.Caixa))
                return SistemaGlobal.Sis.Msg.MostraMensagem("Informe um Caixa!", "Atenção", MessageBoxButton.OK, owner) != MessageBoxResult.OK;
            Caixas caixa = CaixasPes.PegaCaixaByDescricao(SistemaGlobal.Sis.Connection, Provider.Entidade.Caixa).Caixas.FirstOrDefault();
            if (caixa == null)
                return SistemaGlobal.Sis.Msg.MostraMensagem("Caixa não cadastrado!", "Atenção", MessageBoxButton.OK, owner) != MessageBoxResult.OK;
            else
                Provider.Entidade.Cai_codigo = caixa.Cai_codigo;
            return true;
        }

        internal bool ValidaDataLancamento(Window owner)
        {
            if (string.IsNullOrEmpty(Provider.Entidade.DataLancamento))
                return SistemaGlobal.Sis.Msg.MostraMensagem("Informe a data de lançamento!", "Atenção", MessageBoxButton.OK, owner) != MessageBoxResult.OK;
            DateTime date;
            if (!DateTime.TryParse(Provider.Entidade.DataLancamento, out date))
                return SistemaGlobal.Sis.Msg.MostraMensagem("Data inválida!", "Atenção", MessageBoxButton.OK, owner) != MessageBoxResult.OK;
            return true;
        }

        internal bool ValidaValor(Window owner)
        {
            if (string.IsNullOrEmpty(Provider.Entidade.Valor))
                return SistemaGlobal.Sis.Msg.MostraMensagem("Informe um valor!", "Atenção", MessageBoxButton.OK, owner) != MessageBoxResult.OK;
            decimal valor = 0;
            if (!decimal.TryParse(Provider.Entidade.Valor, out valor))
                return SistemaGlobal.Sis.Msg.MostraMensagem("Valor inválido!", "Atenção", MessageBoxButton.OK, owner) != MessageBoxResult.OK;
            if (valor <= 0)
                return SistemaGlobal.Sis.Msg.MostraMensagem("Valor inválido!Valor deve ser maior que zero", "Atenção", MessageBoxButton.OK, owner) != MessageBoxResult.OK;
            return true;
        }

        internal bool ValidaTipoMovimento(Window owner)
        {
            if (string.IsNullOrEmpty(Provider.Entidade.TipoMovimentoEscolhido))
                return SistemaGlobal.Sis.Msg.MostraMensagem("Escolha um tipo de movimento!", "Atenção", MessageBoxButton.OK, owner) != MessageBoxResult.OK;
            return true;
        }
    }
}
