using SIF.Dao;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIF.Aplicacao
{
    /// <summary>
    /// 
    /// </summary>
    public class CrfinanceiroPes : IModelPesquisa<Crfinanceiro>
    {
        private TipoPesquisaFinanceiro pes;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pes"></param>
        /// <param name="conexao"></param>
        public CrfinanceiroPes(TipoPesquisaFinanceiro pes, ConnectionDb conexao)
            : base(conexao)
        {
            this.pes = pes;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="owner"></param>
        /// <returns></returns>
        public override Crfinanceiro Pesquisa(System.Windows.Window owner)
        {
            string script = @"SELECT 
                                Crfinanceiro.Crf_sequencial AS Crf_sequencial,
                                Clientes.Cli_nome AS [Nome],
                                Crfinanceiro.Crf_ndocumento AS [Nº documento],
                                Crfinanceiro.Crf_tipoconta AS [Tipo Conta],
                                Crfinanceiro.Crf_parcela AS [Parcela],
                                Crfinanceiro.Crf_valorparcela AS [Valor parcela],
                                Crfinanceiro.Crf_valordocumento AS [Valor documento],
                                Crfinanceiro.Crf_datalancamento AS [Data lançamento],
                                Crfinanceiro.Crf_datavencimento AS [Data vencimento],
                                Crfinanceiro.Crf_observacao AS [Observação]
                            FROM Crfinanceiro
                            INNER JOIN Clientes ON Crfinanceiro.Cli_codigo = Clientes.Cli_codigo ";
            switch (pes)
            {
                case TipoPesquisaFinanceiro.Contas_pagar:
                    script += "WHERE Crfinanceiro.Crf_tipoconta = 'CP'";
                    break;
                case TipoPesquisaFinanceiro.Contas_receber:
                    script += "WHERE Crfinanceiro.Crf_tipoconta = 'CR'";
                    break;
                case TipoPesquisaFinanceiro.Contas_pagar_baixada:
                    script += "WHERE Crfinanceiro.Crf_tipoconta = 'CP' AND Crfinanceiro.Crf_databaixa IS NOT NULL";
                    break;
                case TipoPesquisaFinanceiro.Contas_receber_baixada:
                    script += "WHERE Crfinanceiro.Crf_tipoconta = 'CR' AND Crfinanceiro.Crf_databaixa IS NOT NULL";
                    break;
                case TipoPesquisaFinanceiro.Contas_receber_nao_baixada:
                    script += "WHERE Crfinanceiro.Crf_tipoconta = 'CR' AND Crfinanceiro.Crf_databaixa IS NULL";
                    break;
                case TipoPesquisaFinanceiro.Contas_pagar_nao_baixada:
                    script += "WHERE Crfinanceiro.Crf_tipoconta = 'CP' AND Crfinanceiro.Crf_databaixa IS NULL";
                    break;
                default: throw new Exception("Informe uma pesquisa");
            }
            script += " AND Fat_sequencial_dest = 0";
            script += "\nORDER BY Crfinanceiro.Crf_sequencial DESC";
            if (this.Consulta(owner, "CONTAS", script) != null)
            {
                int crf_sequencial = GetValuePesquisa<int>("Crf_sequencial");
                return SistemaGlobal.Sis.Connection.GetRegistros<Crfinanceiro>(string.Format("Crf_sequencial = {0}", crf_sequencial)).FirstOrDefault();
            }
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="conexao"></param>
        /// <param name="crf_sequencial"></param>
        /// <returns></returns>
        public static EstruturaPesquisa PegaCrfinanceiro(ConnectionDb conexao, int crf_sequencial)
        {
            EstruturaPesquisa result = new EstruturaPesquisa();
            result.Resultado = ResultadoPesquisa.NAO_ENCONTRADO;

            result.Crfinanceiros = SistemaGlobal.Sis.Connection.GetRegistros<Crfinanceiro>(string.Format("Crf_sequencial = {0}", crf_sequencial));
            switch (result.Crfinanceiros.Count)
            {
                case 0:
                    result.Resultado = ResultadoPesquisa.NAO_ENCONTRADO;
                    break;
                case 1:
                    result.Resultado = ResultadoPesquisa.ENCONTRADO;
                    break;
                default:
                    result.Resultado = ResultadoPesquisa.VARIOS_ENCONTRADO;
                    break;
            }
            return result;
        }

        public Crfinanceiro PesquisaByCli_nome(System.Windows.Window owner, string cli_nome)
        {
            string commandText = string.Format(@"SELECT 
                                Crfinanceiro.Crf_sequencial AS Crf_sequencial,
                                Clientes.Cli_nome AS [Nome],
                                Crfinanceiro.Crf_ndocumento AS [Nº documento],
                                Crfinanceiro.Crf_tipoconta AS [Tipo Conta],
                                Crfinanceiro.Crf_parcela AS [Parcela],
                                Crfinanceiro.Crf_valorparcela AS [Valor parcela],
                                Crfinanceiro.Crf_valordocumento AS [Valor documento],
                                Crfinanceiro.Crf_datalancamento AS [Data lançamento],
                                Crfinanceiro.Crf_datavencimento AS [Data vencimento],
                                Crfinanceiro.Crf_observacao AS [Observação]
                            FROM Crfinanceiro
                            INNER JOIN Clientes ON Crfinanceiro.Cli_codigo = Clientes.Cli_codigo
                                WHERE 
Crfinanceiro.Crf_tipoconta = 'CP'
AND Clientes.Cli_nome = '{0}'
AND Crf_databaixa IS NULL
AND Fat_sequencial_dest = 0 
AND Fat_sequencial_origem = 0
ORDER BY Crfinanceiro.Crf_sequencial DESC", cli_nome);
            if (Consulta(owner, commandText) != null)
            {
                int crf_sequencial = GetValuePesquisa<int>("Crf_sequencial");
                return SistemaGlobal.Sis.Connection.GetRegistros<Crfinanceiro>(string.Format("Crf_sequencial = {0}", crf_sequencial)).FirstOrDefault();
            }
            return null;
        }

        public Crfinanceiro PesquisaFaturamentoByCli_nome(System.Windows.Window owner, string cli_nome, ContasReceberPagar type, List<int> listCrf_sequencial)
        {
            StringBuilder sb = new StringBuilder(string.Format(@"SELECT 
                                Crfinanceiro.Crf_sequencial AS Crf_sequencial,
                                Clientes.Cli_nome AS [Nome],
                                Crfinanceiro.Crf_ndocumento AS [Nº documento],
                                Crfinanceiro.Crf_tipoconta AS [Tipo Conta],
                                Crfinanceiro.Crf_parcela AS [Parcela],
                                Crfinanceiro.Crf_valorparcela AS [Valor parcela],
                                Crfinanceiro.Crf_valordocumento AS [Valor documento],
                                Crfinanceiro.Crf_datalancamento AS [Data lançamento],
                                Crfinanceiro.Crf_datavencimento AS [Data vencimento],
                                Crfinanceiro.Crf_observacao AS [Observação]
                            FROM Crfinanceiro
                            INNER JOIN Clientes ON Crfinanceiro.Cli_codigo = Clientes.Cli_codigo
                                WHERE 
Clientes.Cli_nome = '{0}'
AND Crf_databaixa IS NULL
AND Crfinanceiro.Fat_sequencial_dest = 0 
AND Crfinanceiro.Crf_tipoconta = '{1}'", cli_nome, (type == ContasReceberPagar.ContasPagar) ? "CP" : "CR"));
            if (listCrf_sequencial.Count > 0)
            {
                sb.Append("\nAND Crfinanceiro.Crf_sequencial NOT IN (");
                for (int i = 0; i < listCrf_sequencial.Count; i++)
                {
                    if (i > 0)
                        sb.Append(", ");
                    sb.Append(listCrf_sequencial[i]);
                }
                sb.Append(")");
            }
            sb.Append("\nORDER BY Crfinanceiro.Crf_sequencial DESC");
            if (Consulta(owner, sb.ToString()) != null)
            {
                int crf_sequencial = GetValuePesquisa<int>("Crf_sequencial");
                return SistemaGlobal.Sis.Connection.GetRegistros<Crfinanceiro>(string.Format("Crf_sequencial = {0}", crf_sequencial)).FirstOrDefault();
            }
            return null;
        }
    }
}
