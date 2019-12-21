using SIF.Dao;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIF.Aplicacao
{
    public class PlanoContasPes : IModelPesquisa<Planocontas>
    {
        public PlanoContasPes(ConnectionDb conexao)
            : base(conexao)
        {
        }

        public override Planocontas Pesquisa(System.Windows.Window owner)
        {
            if (this.Consulta(owner, "PLANOS DE CONTAS","SELECT * FROM PlanoContas") != null)
            {
                string Pla_numeroconta = GetValuePesquisa<string>("Pla_numeroconta");
                return SistemaGlobal.Sis.Connection.GetRegistros<Planocontas>(string.Format("Pla_numeroconta = '{0}'", Pla_numeroconta)).FirstOrDefault();
            }
            return null;
        }

        public static EstruturaPesquisa PegaPlanoPagamento(ConnectionDb conexao, string pla_numeroconta)
        {
            EstruturaPesquisa result = new EstruturaPesquisa();
            result.Resultado = ResultadoPesquisa.NAO_ENCONTRADO;
            result.PlanoContas = SistemaGlobal.Sis.Connection.GetRegistros<Planocontas>(string.Format("Pla_numeroconta = '{0}'", pla_numeroconta));
            switch (result.PlanoContas.Count)
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
