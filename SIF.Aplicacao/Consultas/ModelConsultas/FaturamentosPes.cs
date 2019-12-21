using SIF.Dao;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIF.Aplicacao
{
    public class FaturamentosPes : IModelPesquisa<Faturamentos>
    {
        public FaturamentosPes(ConnectionDb connection)
            : base(connection)
        {
        }

        /// <summary>
        /// Consultas todos os Faturamentos
        /// </summary>
        /// <param name="owner"></param>
        /// <returns></returns>
        public override Faturamentos Pesquisa(System.Windows.Window owner)
        {
            if (this.Consulta(owner, "FATURAMENTOS", "SELECT * FROM dbo.Faturamentos") != null)
            {
                return this.conneciton.GetRegistros<Faturamentos>(string.Format("Fat_sequencial = {0}", this.dadosDataRowView.Get<int>("Fat_sequencial"))).FirstOrDefault();
            }
            return null;
        }

        /// <summary>
        /// Pega Faturamentos pelo nº sequencial
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="fat_sequencial"></param>
        /// <returns></returns>
        public static EstruturaPesquisa PegaFaturamentos(ConnectionDb connection, int fat_sequencial)
        {
            EstruturaPesquisa result = new EstruturaPesquisa();
            result.Resultado = ResultadoPesquisa.NAO_ENCONTRADO;
            result.Faturamentos = connection.GetRegistros<Faturamentos>(string.Format("Fat_sequencial = {0}", fat_sequencial));
            switch (result.Faturamentos.Count)
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
