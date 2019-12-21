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
    public class CamovimentosPes : IModelPesquisa<Camovimentos>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="conexao"></param>
        public CamovimentosPes(ConnectionDb conexao)
            : base(conexao)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="owner"></param>
        /// <returns></returns>
        public override Camovimentos Pesquisa(System.Windows.Window owner)
        {
            string commandSelect = @"SELECT Camovimentos.Cam_sequencial AS [Cam_sequencial]
                                          ,Clientes.Cli_nome AS [Nome]
                                          ,Camovimentos.Pla_numeroconta AS [Nº plano contas]
                                          ,Camovimentos.Cai_codigo AS [Caixa]
                                          ,Camovimentos.Cam_valorlancado AS [Valor lançado]
                                          ,Camovimentos.Cam_tipomovimento AS [Tipo movimento]
                                          ,Camovimentos.Cam_datalancamento AS [Data lançamento]
                                          ,Camovimentos.Cam_observacao AS [Observação]
                                          ,Camovimentos.Crf_sequencial AS [Sequencial CR]
                                    FROM Camovimentos
                                    INNER JOIN Clientes ON Camovimentos.Cli_codigo = Clientes.Cli_codigo
                                    ORDER BY Camovimentos.Cam_sequencial DESC";
            if (this.Consulta(owner, "MOVIMENTAÇÃO FINANCEIRA",commandSelect) != null)
            {
                int cam_sequencial = GetValuePesquisa<int>("Cam_sequencial");
                return SistemaGlobal.Sis.Connection.GetRegistros<Camovimentos>(string.Format("Cam_sequencial = {0}", cam_sequencial)).FirstOrDefault();
            }
            return null;
        }
    }
}
