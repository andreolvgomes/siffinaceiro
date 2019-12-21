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
    public struct EstruturaPesquisa
    {
        /// <summary>
        /// Resultado
        /// </summary>
        public ResultadoPesquisa Resultado { get; set; }

        /// <summary>
        /// Lista de Clientes
        /// </summary>
        public List<Clientes> Clientes { get; set; }        

        /// <summary>
        /// Lista de Fpagamentos
        /// </summary>
        public List<Fpagamentos> Fpagamentos { get; set; }

        /// <summary>
        /// Lista de PlanoContas
        /// </summary>
        public List<Planocontas> PlanoContas { get; set; }

        /// <summary>
        /// Lista de Caixas
        /// </summary>
        public List<Caixas> Caixas { get; set; }

        /// <summary>
        /// Lista de Crfinanceiros
        /// </summary>
        public List<Crfinanceiro> Crfinanceiros { get; set; }

        /// <summary>
        /// Lista de Faturamentos
        /// </summary>
        public List<Faturamentos> Faturamentos { get; set; }
    }
}
