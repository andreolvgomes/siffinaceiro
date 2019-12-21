using SIF.Aplicacao.Helper;
using SIF.Dao;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIF.Aplicacao.Providers
{
    public class ProviderBaixaColetiva : INotifyPropertyChanged
    {
        private DataView _contas;
        /// <summary>
        /// Resultado da consulta das contas
        /// </summary>
        public DataView Contas
        {
            get { return _contas; }
            set
            {
                if (_contas != value)
                {
                    _contas = value;
                    NotifyPropertyChanged("Contas");
                }
            }
        }

        private bool _especificarCliente;
        /// <summary>
        /// Checar se vai ou não especificar o nome do cliente para ser executado junto aos filtros das contas
        /// </summary>
        public bool EspecificarCliente
        {
            get { return _especificarCliente; }
            set
            {
                if (_especificarCliente != value)
                {
                    _especificarCliente = value;
                    NotifyPropertyChanged("EspecificarCliente");
                }
            }
        }

        private string _nomeCliente;
        /// <summary>
        /// Nome do cliente informado para filtro das contas
        /// </summary>
        public string NomeCliente
        {
            get { return _nomeCliente; }
            set
            {
                if (_nomeCliente != value)
                {
                    _nomeCliente = value;
                    NotifyPropertyChanged("NomeCliente");
                }
            }
        }

        private TipoFiltro _filtro;
        /// <summary>
        /// Tipos de filtro escolhido para execução
        /// </summary>
        public TipoFiltro Filtro
        {
            get { return _filtro; }
            set
            {
                if (_filtro != value)
                {
                    _filtro = value;
                    NotifyPropertyChanged("Filtro");

                    if (_filtro != TipoFiltro.PorData)
                    {
                        AtualizaCollection();
                    }
                    else
                    {
                        Contas = new DataView();
                        ResumoValores.Reset();
                    }
                }
            }
        }

        private PagamentoStatus _StatusPagamento;
        /// <summary>
        /// Status do andamento do pagamento(se saiu para ser pago ou ainda está pendente)
        /// </summary>
        public PagamentoStatus StatusPagamento
        {
            get { return _StatusPagamento; }
            set
            {
                if (_StatusPagamento != value)
                {
                    _StatusPagamento = value;

                    NotifyPropertyChanged("StatusPagamento");
                    AtualizaCollection();
                }
            }
        }

        private ContasReceberPagar _financeiro;
        /// <summary>
        /// Tipo de contas que será apresentado em tela(datagrid). Contas a receber ou Contas a pagar
        /// </summary>
        public ContasReceberPagar Financeiro
        {
            get { return _financeiro; }
            set
            {
                if (_financeiro != value)
                {
                    _financeiro = value;
                    NotifyPropertyChanged("Financeiro");
                    AtualizaCollection();
                }
            }
        }

        public event Action Event_AtualizaCollection;
        //public DaoGenerico<Crfinanceiro> provider { get; private set; }
        public DataIncialFinal ControleData { get; private set; }
        private SIF.Dao.ConnectionDb conexao;
        public ValoresBaixaC ResumoValores { get; private set; }

        /// <summary>
        /// Construtor
        /// </summary>
        /// <param name="conexao">Estrutor de conexão com o banco</param>
        public ProviderBaixaColetiva(SIF.Dao.ConnectionDb conexao)
        {
            this.conexao = conexao;
            this.ControleData = new DataIncialFinal();
            this.ControleData.PropertyChanged += new PropertyChangedEventHandler(ControleData_PropertyChanged);

            this._filtro = TipoFiltro.DoMesEvencidas;
            this._financeiro = ContasReceberPagar.ContasPagar;
            this.ResumoValores = new ValoresBaixaC();

            //provider = new DaoGenerico<Crfinanceiro>(conexao);
        }

        private void ControleData_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            AtualizaCollection();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Atualiza o datagrid sempre que precisar
        /// </summary>
        internal void AtualizaCollection()
        {
            if (Event_AtualizaCollection != null)
                Event_AtualizaCollection();
        }

        /// <summary>
        /// Executa busca no banco das contas de acordo com o script sql
        /// </summary>
        public void StartCollection()
        {
            try
            {
                string commandTextSelect = GetSelect();
                commandTextSelect += GetFiltro();

                DataTable datatable = new DataTable();
                using (SqlCommand command = conexao.GetSqlCommand(commandTextSelect))
                {
                    using (SqlDataAdapter adp = new SqlDataAdapter(command))
                    {
                        adp.Fill(datatable);
                    }
                }
                Contas = new DataView(datatable);
            }
            catch (Exception ex)
            {
                SistemaGlobal.Sis.Log.LogError(ex);
            }
        }

        /// <summary>
        /// Filtros where
        /// </summary>
        /// <returns></returns>
        private string GetFiltro()
        {
            List<string> listFilter = new List<string>();
            switch (Financeiro)
            {
                case ContasReceberPagar.ContasReceber:
                    listFilter.Add("Crfinanceiro.Crf_tipoconta = 'CR'");
                    break;
                case ContasReceberPagar.ContasPagar:
                    listFilter.Add("Crfinanceiro.Crf_tipoconta = 'CP'");
                    break;
            }
            if (EspecificarCliente && !string.IsNullOrEmpty(NomeCliente))
            {
                listFilter.Add(string.Format("Clientes.Cli_nome = '{0}'", NomeCliente));
            }
            switch (Filtro)
            {
                case TipoFiltro.Todas:
                    break;
                case TipoFiltro.PorData:
                    listFilter.Add(string.Format(@"CONVERT(DATETIME, Crfinanceiro.Crf_datavencimento, 103) >= CONVERT(DATETIME, '{0}', 103) AND 
                                            CONVERT(DATETIME, Crfinanceiro.Crf_datavencimento, 103) <= CONVERT(DATETIME, '{1}', 103)", ControleData.DateTimeIncial.ConvertToString(), ControleData.DateTimeFinal.ConvertToString()));
                    break;
                case TipoFiltro.DoMesEvencidas:
                    listFilter.Add(string.Format(@"(CONVERT(VARCHAR, Crfinanceiro.Crf_datavencimento, 103) LIKE '%{0}' OR 
                                                    CONVERT(DATETIME, Crfinanceiro.Crf_datavencimento, 103) <= CONVERT(DATETIME, '{1}', 103))", DateTime.Now.ToString("MM/yyyy"), DateTime.Now.ConvertToString()));
                    break;
                case TipoFiltro.SomenteVencidas:
                    listFilter.Add(string.Format(@"CONVERT(DATETIME, Crfinanceiro.Crf_datavencimento, 103) < CONVERT(DATETIME, '{0}', 103)", DateTime.Now.ConvertToString()));
                    break;
                case TipoFiltro.SomenteDoMes:
                    listFilter.Add(string.Format(@"CONVERT(VARCHAR, Crfinanceiro.Crf_datavencimento, 103) LIKE '%{0}'", DateTime.Now.ToString("MM/yyyy")));
                    break;
            }
            switch (StatusPagamento)
            {
                case PagamentoStatus.Pagamento_andamento:
                    listFilter.Add("Crf_empagamento = 1");
                    break;
                case PagamentoStatus.Pagamento_pendente:
                    listFilter.Add("Crf_empagamento = 0");
                    break;
            }
            listFilter.Add("Fat_sequencial_dest = 0");
            string commandWhere = " AND ";
            for (int i = 0; i < listFilter.Count; i++)
            {
                if (i > 0)
                    commandWhere += " AND ";
                commandWhere += listFilter[i];
            }
            return commandWhere;
        }

        /// <summary>
        /// Script sql para buscar as contas no banco
        /// </summary>
        /// <returns></returns>
        private string GetSelect()
        {
            return string.Format(@"SELECT
                                    Crfinanceiro.Crf_sequencial,
                                    Crfinanceiro.Crf_ndocumento, 
                                    Clientes.Cli_nome, 
                                    Crfinanceiro.Crf_parcela,
                                    Crfinanceiro.Crf_datalancamento,
                                    Crfinanceiro.Crf_datavencimento,
                                    Crfinanceiro.Crf_valordocumento,
                                    Crfinanceiro.Crf_valorparcela,
                                    Crfinanceiro.Crf_observacao,
                                    Crfinanceiro.Crf_empagamento
                                    FROM Crfinanceiro
                                    INNER JOIN Clientes ON Crfinanceiro.Cli_codigo = Clientes.Cli_codigo
                                    WHERE 
                                    Crfinanceiro.Crf_databaixa IS NULL");
        }

        internal void SetCliente(Clientes cliente)
        {
            this.NomeCliente = cliente.Cli_nome;
        }

        /// <summary>
        /// Organiza o remuso dos valores
        /// </summary>
        /// <param name="dataView"></param>
        internal void ProcessaResumoValores(DataView dataView)
        {
            this.ResumoValores.Reset();
            foreach (DataRowView row in dataView)
            {
                if (row.Get<DateTime>("Crf_datavencimento").Date < DateTime.Now.Date)
                    ResumoValores.TotalVencidas += row.Get<decimal>("Crf_valorparcela");
                else
                    ResumoValores.TotalEmDias += row.Get<decimal>("Crf_valorparcela");
                ResumoValores.TotalGeral += row.Get<decimal>("Crf_valorparcela");
            }
        }

        internal void SetSaveStatusCrfinanceiro(DataRowView dataRowView, bool emPagamento)
        {
            if (dataRowView != null)
            {
                int crf_sequencial = dataRowView.Get<int>("Crf_sequencial");
                conexao.ExecuteNonQueryTransaction(string.Format("UPDATE dbo.Crfinanceiro SET Crf_empagamento = {0} WHERE Crf_sequencial = {1}", ((emPagamento) ? 1 : 0), crf_sequencial));
                AtualizaCollection();
            }
        }
    }
}
