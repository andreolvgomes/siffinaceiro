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
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace SIF.Aplicacao.Providers
{
    public class ProviderBaixasContas : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public ImageSource Logo
        {
            get
            {
                //var uriSource = new Uri("/Icones/baixacontaspagar.png", UriKind.Relative);
                //ImageSource logo = new BitmapImage(uriSource); ;
                //return logo;
                if (IntefaceLayout == ContasReceberPagar.ContasPagar)
                    return new BitmapImage(new Uri("/Icones/baixacontaspagar.png", UriKind.Relative));
                return new BitmapImage(new Uri("/Icones/baixacontasreceber.png", UriKind.Relative));
            }
        }

        private Crfinanceiro _crFinanceiro;
        public Crfinanceiro CrFinanaceiro
        {
            get { return _crFinanceiro; }
            set
            {
                if (_crFinanceiro != value)
                {
                    _crFinanceiro = value;
                    NotifyPropertyChanged("CrFinanaceiro");
                }
            }
        }

        private bool _VisualizaDadosConta;

        public bool VisualizaDadosConta
        {
            get { return _VisualizaDadosConta; }
            set
            {
                if (_VisualizaDadosConta != value)
                {
                    _VisualizaDadosConta = value;
                    NotifyPropertyChanged("VisualizaDadosConta");
                }
            }
        }


        private bool _SomenteLeitura = true;

        public bool SomenteLeitura
        {
            get { return _SomenteLeitura; }
            set
            {
                if (_SomenteLeitura != value)
                {
                    _SomenteLeitura = value;
                    NotifyPropertyChanged("SomenteLeitura");
                }
            }
        }


        public string Titulo
        {
            get
            {
                if (IntefaceLayout == ContasReceberPagar.ContasPagar)
                    return "BAIXA CONTAS A PAGAR";
                return "BAIXA CONTAS A RECEBER";
            }
        }

        public ContasReceberPagar IntefaceLayout;
        public ProviderInterfacesCadastros2<Crbaixas> Provider { get; set; }

        public ProviderBaixasContas(Window janela, Buttons controlButtons, ConnectionDb conexao, ContasReceberPagar interf)
        {
            Provider = new ProviderInterfacesCadastros2<Crbaixas>(janela, controlButtons, conexao);

            Provider.Event_ConvertValoresDatabaseToInterfaceEventHandler += new ExecucaoCommandEventHandler<Crbaixas>(OrganizaValoresToInterface);
            Provider.Event_ConvertValoresInterfaceToDatabaseSaveEventHandler += new ExecucaoCommandEventHandler<Crbaixas>(OrganizaValoresToBanco);
            Provider.Event_ExecutandoDeleteEventHandler += new ExecucaoCommandEventHandler<Crbaixas>(ProcessoExclusaoBaixa);
            Provider.Event_CommandExecutadoEventHandler += new ExecutaCommandsNotificacaoEventHandler(ComandoExecutado);

            string where = string.Format("Crb_tipodoconta = '{0}'", (interf == ContasReceberPagar.ContasPagar) ? "CP" : "CR");
            Provider.LoadRegistros("Crbaixas", "Crf_sequencial", where);
            VisualizaDadosConta = this.Provider.CountRegistros > 0;

            this.IntefaceLayout = interf;
        }

        private void ComandoExecutado(ButtonType command)
        {
            if (command == ButtonType.New)
                VisualizaDadosConta = false;
            else
                VisualizaDadosConta = Provider.CountRegistros > 0;
        }

        private void ProcessoExclusaoBaixa(Crbaixas model)
        {
            using (ContextoDb<Crfinanceiro> providerFinan = new ContextoDb<Crfinanceiro>())
            {
                CrFinanaceiro.Crf_valorrecebido = 0;
                CrFinanaceiro.Crf_databaixa = null;
                providerFinan.Update(CrFinanaceiro);
            }
            using (ContextoDb<Camovimentos> providerMov = new ContextoDb<Camovimentos>())
            {
                Camovimentos mov = providerMov.Connection.GetRegistros<Camovimentos>(string.Format("Crf_sequencial = {0}", model.Crf_sequencial)).FirstOrDefault();
                if (providerMov.Delete(mov))
                {
                    using (ProviderRecord<Caixas> providerCai = new ProviderRecord<Caixas>())
                    {
                        Caixas caixa = SistemaGlobal.Sis.Connection.GetRegistros<Caixas>(string.Format("Cai_codigo = '{0}'", mov.Cai_codigo)).FirstOrDefault();
                        if (mov.Cam_tipomovimento == "D")
                            caixa.Cai_saldo += mov.Cam_valorlancado;
                        else
                            caixa.Cai_saldo -= mov.Cam_valorlancado;
                        providerCai.Update(caixa);
                    }
                }
            }
            CrFinanaceiro = new Crfinanceiro();

            //using (ProviderRecord<Crfinanceiro> providerFinan = new ProviderRecord<Crfinanceiro>())
            //{
            //    CrFinanaceiro.Crf_valorrecebido = 0;
            //    CrFinanaceiro.Crf_databaixa = null;
            //    providerFinan.Update(CrFinanaceiro);
            //}
            //using (ProviderRecord<Camovimentos> providerMov = new ProviderRecord<Camovimentos>())
            //{
            //    Camovimentos mov = Sistema.Sis.Connection.GetRegistros<Camovimentos>(string.Format("Crf_sequencial = {0}", model.Crf_sequencial)).FirstOrDefault();
            //    if (providerMov.Delete(mov))
            //    {
            //        using (ProviderRecord<Caixas> providerCai = new ProviderRecord<Caixas>())
            //        {
            //            Caixas caixa = Sistema.Sis.Connection.GetRegistros<Caixas>(string.Format("Cai_codigo = '{0}'", mov.Cai_codigo)).FirstOrDefault();
            //            if (mov.Cam_tipomovimento == "D")
            //                caixa.Cai_saldo += mov.Cam_valorlancado;
            //            else
            //                caixa.Cai_saldo -= mov.Cam_valorlancado;
            //            providerCai.Update(caixa);
            //        }
            //    }
            //}
            //CrFinanaceiro = new Crfinanceiro();
        }

        private void OrganizaValoresToBanco(Crbaixas model)
        {
            model.Crb_valorecebido = Convert.ToDecimal(model.ValorBaixa);
            model.Crb_databaixa = Convert.ToDateTime(model.DataBaixa);
            model.Crf_sequencial = CrFinanaceiro.Crf_sequencial;
            model.Crb_tipodoconta = CrFinanaceiro.Crf_tipoconta;
        }

        private void OrganizaValoresToInterface(Crbaixas model)
        {
            model.CodigoConta = model.Crf_sequencial.ToString();
            model.ValorBaixa = model.Crb_valorecebido.ConvertToString();
            model.DataBaixa = model.Crb_databaixa.ConvertToString();

            model.DescricaoFpagamento = FpagamentosPes.PegaFpagamento(SistemaGlobal.Sis.Connection, model.Fpa_codigo).Fpagamentos.FirstOrDefault().Fpa_descricao;
            model.DescricaoCaixa = CaixasPes.PegaCaixaByCodigo(SistemaGlobal.Sis.Connection, model.Cai_codigo).Caixas.FirstOrDefault().Cai_descricao;
            CrFinanaceiro = CrfinanceiroPes.PegaCrfinanceiro(SistemaGlobal.Sis.Connection, model.Crf_sequencial).Crfinanceiros.FirstOrDefault();
            SetValoreCrfinanceiro(CrFinanaceiro);
        }

        internal void SetCrfinanceiro(Crfinanceiro crfinanceiro)
        {
            if (crfinanceiro != null)
            {
                this.CrFinanaceiro = crfinanceiro;
                Provider.Entidade.CodigoConta = crfinanceiro.Crf_sequencial.ToString();
                SetValoreCrfinanceiro(crfinanceiro);

                Provider.Entidade.ValorBaixa = this.CrFinanaceiro.Crf_valorareceber.ConvertToString();
                Provider.Entidade.DataBaixa = DateTime.Now.ConvertToString();
            }
        }

        private void SetValoreCrfinanceiro(Crfinanceiro crfinanceiro)
        {
            this.CrFinanaceiro.DescricaoFpagamento = FpagamentosPes.PegaFpagamento(SistemaGlobal.Sis.Connection, crfinanceiro.Fpa_codigo).Fpagamentos.FirstOrDefault().Fpa_descricao;
            this.CrFinanaceiro.NomeCliente = ClientesPes.PegaCliente(SistemaGlobal.Sis.Connection, crfinanceiro.Cli_codigo).Clientes.FirstOrDefault().Cli_nome;
        }

        internal void GravaBaixa()
        {
            using (ProviderRecord<Camovimentos> providerMov = new ProviderRecord<Camovimentos>())
            {
                Camovimentos mov = new Camovimentos();
                mov.Crf_sequencial = CrFinanaceiro.Crf_sequencial;
                mov.Cai_codigo = Provider.Entidade.Cai_codigo;
                mov.Cli_codigo = CrFinanaceiro.Cli_codigo;
                mov.Fpa_codigo = Provider.Entidade.Fpa_codigo;
                mov.Pla_numeroconta = Provider.Entidade.Pla_numeroconta;
                mov.Cam_valorlancado = Convert.ToDecimal(Provider.Entidade.ValorBaixa);
                mov.Cam_datalancamento = Convert.ToDateTime(Provider.Entidade.DataBaixa);
                mov.Cam_observacao = Provider.Entidade.Crb_observacao;
                mov.Cam_tipomovimento = IntefaceLayout == ContasReceberPagar.ContasPagar ? "D" : "C";
                if (providerMov.Insert(mov))
                {
                    using (ProviderRecord<Caixas> providerCai = new ProviderRecord<Caixas>())
                    {
                        Caixas caixa = SistemaGlobal.Sis.Connection.GetRegistros<Caixas>(string.Format("Cai_codigo = '{0}'", mov.Cai_codigo)).FirstOrDefault();
                        if (mov.Cam_tipomovimento == "D")
                            caixa.Cai_saldo -= mov.Cam_valorlancado;
                        else
                            caixa.Cai_saldo += mov.Cam_valorlancado;
                        providerCai.Update(caixa);
                    }
                }
            }
            using (ProviderRecord<Crfinanceiro> fina = new ProviderRecord<Crfinanceiro>())
            {
                CrFinanaceiro.Crf_valorrecebido = Provider.Entidade.Crb_valorecebido;
                CrFinanaceiro.Crf_databaixa = Provider.Entidade.Crb_databaixa;
                fina.Update(CrFinanaceiro);
            }
        }

        internal bool ValidaFpagamento(Window owner)
        {
            if (string.IsNullOrEmpty(Provider.Entidade.DescricaoFpagamento))
                return SistemaGlobal.Sis.Msg.MostraMensagem("Informe uma Forma de Pagamento!", "Atenção", MessageBoxButton.OK, owner) != MessageBoxResult.OK;
            Fpagamentos pagamento = FpagamentosPes.PegaFpagamentoByDescricao(SistemaGlobal.Sis.Connection, Provider.Entidade.DescricaoFpagamento).Fpagamentos.FirstOrDefault();
            if (pagamento == null)
                return SistemaGlobal.Sis.Msg.MostraMensagem("Forma de Pagamento não cadastrada!", "Atenção", MessageBoxButton.OK, owner) != MessageBoxResult.OK;
            Provider.Entidade.Fpa_codigo = pagamento.Fpa_codigo;
            return true;
        }

        internal bool ValidaPlanoContas(Window owner)
        {
            if (string.IsNullOrEmpty(Provider.Entidade.Pla_numeroconta))
                return SistemaGlobal.Sis.Msg.MostraMensagem("Informe o Plano de Contas!", "Atenção", MessageBoxButton.OK, owner) != MessageBoxResult.OK;
            if (PlanoContasPes.PegaPlanoPagamento(SistemaGlobal.Sis.Connection, Provider.Entidade.Pla_numeroconta).PlanoContas.FirstOrDefault() == null)
                return SistemaGlobal.Sis.Msg.MostraMensagem("Plano de Contas não cadastrado!", "Atenção", MessageBoxButton.OK, owner) != MessageBoxResult.OK;
            return true;
        }

        internal bool ValidaCaixa(Window owner)
        {
            if (string.IsNullOrEmpty(Provider.Entidade.DescricaoCaixa))
                return SistemaGlobal.Sis.Msg.MostraMensagem("Informe o Caixa!", "Atenção", MessageBoxButton.OK, owner) != MessageBoxResult.OK;
            Caixas caixa = CaixasPes.PegaCaixaByDescricao(SistemaGlobal.Sis.Connection, Provider.Entidade.DescricaoCaixa).Caixas.FirstOrDefault();
            if (caixa == null)
                return SistemaGlobal.Sis.Msg.MostraMensagem("Caixa não cadastrado!", "Atenção", MessageBoxButton.OK, owner) != MessageBoxResult.OK;

            Provider.Entidade.Cai_codigo = caixa.Cai_codigo;
            return true;
        }

        internal bool ValidaDataBaixa(Window owner)
        {
            if (string.IsNullOrEmpty(Provider.Entidade.DataBaixa))
                return SistemaGlobal.Sis.Msg.MostraMensagem("Informe a data da baixa!", "Atenção", MessageBoxButton.OK, owner) != MessageBoxResult.OK;
            DateTime date;
            if (!DateTime.TryParse(Provider.Entidade.DataBaixa, out date))
                return SistemaGlobal.Sis.Msg.MostraMensagem("Data inválida!", "Atenção", MessageBoxButton.OK, owner) != MessageBoxResult.OK;
            return true;
        }

        internal bool ValidaValor(Window owner)
        {
            if (string.IsNullOrEmpty(Provider.Entidade.ValorBaixa))
                return SistemaGlobal.Sis.Msg.MostraMensagem("Informe um valor!", "Atenção", MessageBoxButton.OK, owner) != MessageBoxResult.OK;
            decimal valor = 0;
            if (!decimal.TryParse(Provider.Entidade.ValorBaixa, out valor))
                return SistemaGlobal.Sis.Msg.MostraMensagem("Valor inválido!", "Atenção", MessageBoxButton.OK, owner) != MessageBoxResult.OK;
            if (valor <= 0)
                return SistemaGlobal.Sis.Msg.MostraMensagem("Valor inválido!Deve ser maior que zero!", "Atenção", MessageBoxButton.OK, owner) != MessageBoxResult.OK;
            return true;
        }

        internal void SetFpagamento(Fpagamentos fpagamentos)
        {
            if (fpagamentos != null)
            {
                Provider.Entidade.DescricaoFpagamento = fpagamentos.Fpa_descricao;
                Provider.Entidade.Fpa_codigo = fpagamentos.Fpa_codigo;
            }
        }

        internal void SetPlanoContas(Planocontas planoContas)
        {
            if (planoContas != null)
            {
                Provider.Entidade.Pla_numeroconta = planoContas.Pla_numeroconta;
            }
        }

        internal void SetCaixa(Caixas caixas)
        {
            if (caixas != null)
            {
                Provider.Entidade.DescricaoCaixa = caixas.Cai_descricao;
                Provider.Entidade.Cai_codigo = caixas.Cai_codigo;
            }
        }
    }
}
