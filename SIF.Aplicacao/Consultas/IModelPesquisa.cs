using SIF.Dao;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SIF.Aplicacao
{
    public abstract class IModelPesquisa<Result> : IDisposable where Result : IModelProperty
    {
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public abstract Result Pesquisa(Window owner);

        protected DataRowView dadosDataRowView;
        protected ConnectionDb conneciton;

        public IModelPesquisa(ConnectionDb conexao)
        {
            this.conneciton = conexao;
        }

        protected T GetValuePesquisa<T>(string nomecampo)
        {
            return (T)dadosDataRowView[nomecampo];
        }

        protected DataRowView Consulta(Window owner, string commandSelect)
        {
            return this.Consulta(owner, "CONSULTA", commandSelect);
        }

        /// <summary>
        /// Executa consulta
        /// </summary>
        /// <param name="owner"></param>
        /// <param name="commandSelect"></param>
        /// <returns></returns>
        protected DataRowView Consulta(Window owner, string titulo, string commandSelect)
        {
            if (SistemaGlobal.Sis.ConsultaType == CONSULTAS_SELECTED.COM_VIEW_EM_EXECUCAO)
            {
                SIF.PesquisaView.PesquisaWindow pes = new SIF.PesquisaView.PesquisaWindow(owner, titulo, commandSelect, conneciton.GetSqlConnection());
                dadosDataRowView = pes.PegaPesquisaSelected();
            }
            else if (SistemaGlobal.Sis.ConsultaType == CONSULTAS_SELECTED.COM_VIEW_E_SP)
            {
                SIF.PesquisaViewSP.PesquisaWindow pes = new SIF.PesquisaViewSP.PesquisaWindow(owner, titulo, commandSelect, conneciton.GetSqlConnection());
                dadosDataRowView = pes.PegaPesquisaSelected();
            }
            else
            {
                SIF.Pesquisa.PesquisaWindow pes = new SIF.Pesquisa.PesquisaWindow(owner, titulo, commandSelect, conneciton.GetSqlConnection());
                dadosDataRowView = pes.PegaPesquisaSelected();
            }
            return dadosDataRowView;
        }
    }
}
