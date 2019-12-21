using SIF.Dao;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIF.Aplicacao
{
    /// <summary>
    /// Pesquisa de caixa
    /// </summary>
    public class CaixasPes : IModelPesquisa<Caixas>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="conexao"></param>
        public CaixasPes(ConnectionDb conexao)
            : base(conexao)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="owner"></param>
        /// <returns></returns>
        public override Caixas Pesquisa(System.Windows.Window owner)
        {
            string commandSelect = @"SELECT Cai_codigo AS [Cai_codigo]
                                          ,Cai_descricao AS [Descrição]
                                          ,Cai_observacao AS [Observação]
                                          ,Cai_saldo AS [Saldo]
                                    FROM Caixas";
            if (this.Consulta(owner, "CAIXAS",commandSelect) != null)
            {
                string cai_caixa = GetValuePesquisa<string>("Cai_codigo");
                return SistemaGlobal.Sis.Connection.GetRegistros<Caixas>(string.Format("Cai_codigo = '{0}'", cai_caixa)).FirstOrDefault();
            }
            return null;
        }

        /// <summary>
        /// /
        /// </summary>
        /// <param name="conexao"></param>
        /// <param name="cai_codigo"></param>
        /// <returns></returns>
        public static EstruturaPesquisa PegaCaixaByCodigo(ConnectionDb conexao, string cai_codigo)
        {
            EstruturaPesquisa result = new EstruturaPesquisa();
            result.Resultado = ResultadoPesquisa.NAO_ENCONTRADO;

            result.Caixas = SistemaGlobal.Sis.Connection.GetRegistros<Caixas>(string.Format("Cai_codigo = '{0}'", cai_codigo));
            switch (result.Caixas.Count)
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
        /// 
        /// </summary>
        /// <param name="conexao"></param>
        /// <param name="cai_descricao"></param>
        /// <returns></returns>
        public static EstruturaPesquisa PegaCaixaByDescricao(ConnectionDb conexao, string cai_descricao)
        {
            EstruturaPesquisa result = new EstruturaPesquisa();
            result.Resultado = ResultadoPesquisa.NAO_ENCONTRADO;

            result.Caixas = SistemaGlobal.Sis.Connection.GetRegistros<Caixas>(string.Format("Cai_descricao = '{0}'", cai_descricao));
            switch (result.Caixas.Count)
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
    }
}
