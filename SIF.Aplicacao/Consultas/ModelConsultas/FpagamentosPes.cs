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
    public class FpagamentosPes : IModelPesquisa<Fpagamentos>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="conexao"></param>
        public FpagamentosPes(ConnectionDb conexao)
            : base(conexao)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="owner"></param>
        /// <returns></returns>
        public override Fpagamentos Pesquisa(System.Windows.Window owner)
        {
            if (this.Consulta(owner, "FORMAS DE PAGAMENTO", "SELECT * FROM Fpagamentos") != null)
            {
                string codigo = GetValuePesquisa<string>("Fpa_codigo");
                return SistemaGlobal.Sis.Connection.GetRegistros<Fpagamentos>(string.Format("Fpa_codigo = '{0}'", codigo)).FirstOrDefault();
            }
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="conexao"></param>
        /// <param name="fpa_codigo"></param>
        /// <returns></returns>
        public static EstruturaPesquisa PegaFpagamento(ConnectionDb conexao, string fpa_codigo)
        {
            EstruturaPesquisa result = new EstruturaPesquisa();
            result.Resultado = ResultadoPesquisa.NAO_ENCONTRADO;
            result.Fpagamentos = SistemaGlobal.Sis.Connection.GetRegistros<Fpagamentos>(string.Format("Fpa_codigo = '{0}'", fpa_codigo));
            switch (result.Fpagamentos.Count)
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
        /// <param name="fpa_descricao"></param>
        /// <returns></returns>
        public static EstruturaPesquisa PegaFpagamentoByDescricao(ConnectionDb conexao, string fpa_descricao)
        {
            EstruturaPesquisa result = new EstruturaPesquisa();
            result.Resultado = ResultadoPesquisa.NAO_ENCONTRADO;
            result.Fpagamentos = SistemaGlobal.Sis.Connection.GetRegistros<Fpagamentos>(string.Format("Fpa_descricao = '{0}'", fpa_descricao));
            switch (result.Fpagamentos.Count)
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
