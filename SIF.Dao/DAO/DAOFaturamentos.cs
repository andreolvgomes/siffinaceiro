using SIF.Dao;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIF
{
    public class DAOFaturamentos : DAOAbstract
    {
        public DAOFaturamentos(ConnectionDb connection)
            : base(connection)
        {
        }

        /// <summary>
        /// Verifica se tem o faturamento no banco pelo número sequencial
        /// </summary>
        /// <param name="fat_sequencial"></param>
        /// <returns></returns>
        public bool TemFaturamentosByFat_sequencial(int fat_sequencial)
        {
            DataTable t = new DataTable();
            using (SqlCommand command = this.Connection.GetSqlCommand(string.Format("SELECT COUNT(Fat_sequencial) AS r FROM dbo.Faturamentos WHERE Fat_sequencial = {0}", fat_sequencial)))
            {
                using (SqlDataAdapter adp = new SqlDataAdapter(command))
                {
                    adp.Fill(t);
                }
            }
            return Convert.ToInt16(t.Rows[0][0]) > 0;
        }
    }
}
