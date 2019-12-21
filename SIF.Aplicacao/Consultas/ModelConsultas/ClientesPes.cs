using SIF.Dao;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIF.Aplicacao
{
    /// <summary>
    /// /
    /// </summary>
    public class ClientesPes : IModelPesquisa<Clientes>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="conexao"></param>
        public ClientesPes(ConnectionDb conexao)
            : base(conexao)
        {
        }

        /// <summary>
        /// Consulta todos os clientes do cadastro
        /// </summary>
        /// <param name="owner"></param>
        /// <returns></returns>
        public override Clientes Pesquisa(System.Windows.Window owner)
        {
            if (Consulta(owner, "CLIENTES",  "SELECT * FROM Clientes") != null)
            {
                Int32 cli_codigo = GetValuePesquisa<Int32>("Cli_codigo");
                return SistemaGlobal.Sis.Connection.GetRegistros<Clientes>(string.Format("Cli_codigo = {0}", cli_codigo)).FirstOrDefault();
            }
            return null;
        }

        /// <summary>
        /// Busca no banco de dados o cliente pelo código
        /// </summary>
        /// <param name="conexao"></param>
        /// <param name="cli_codigo"></param>
        /// <returns></returns>
        public static EstruturaPesquisa PegaCliente(ConnectionDb conexao, int cli_codigo)
        {
            EstruturaPesquisa result = new EstruturaPesquisa();
            result.Resultado = ResultadoPesquisa.NAO_ENCONTRADO;
            result.Clientes = SistemaGlobal.Sis.Connection.GetRegistros<Clientes>(string.Format("Cli_codigo = {0}", cli_codigo));
            switch (result.Clientes.Count)
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

        /// <summary>
        /// Busca no banco de dados o Cliente pelo nome
        /// </summary>
        /// <param name="conexao"></param>
        /// <param name="cli_nome"></param>
        /// <returns></returns>
        public static EstruturaPesquisa PegaCliente(ConnectionDb conexao, string cli_nome)
        {
            EstruturaPesquisa result = new EstruturaPesquisa();
            result.Resultado = ResultadoPesquisa.NAO_ENCONTRADO;
            result.Clientes = SistemaGlobal.Sis.Connection.GetRegistros<Clientes>(string.Format("Cli_nome = '{0}'", cli_nome));
            switch (result.Clientes.Count)
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

        /// <summary>
        /// Consulta os clientes que tem contas a receber
        /// </summary>
        /// <param name="owner"></param>
        /// <param name="contasReceberPagar"></param>
        /// <returns></returns>
        internal Clientes ConsultaClientesTemFinanceiro(System.Windows.Window owner, ContasReceberPagar contasReceberPagar)
        {
            string commandSelect = string.Format(@"SELECT * FROM dbo.Clientes
WHERE dbo.Clientes.Cli_codigo IN (SELECT dbo.Crfinanceiro.Cli_codigo FROM dbo.Crfinanceiro
WHERE dbo.Crfinanceiro.Crf_databaixa IS NULL AND dbo.Crfinanceiro.Crf_tipoconta = '{0}')", (contasReceberPagar == ContasReceberPagar.ContasPagar) ? "CP" : "CR");
            if (this.Consulta(owner, commandSelect) != null)
            {
                Int32 cli_codigo = GetValuePesquisa<Int32>("Cli_codigo");
                return this.conneciton.GetRegistros<Clientes>(string.Format("Cli_codigo = {0}", cli_codigo)).FirstOrDefault();
            }
            return null;
        }
    }
}
