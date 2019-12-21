using SIF.Dao;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIF
{
    public abstract class DAOAbstract : IDisposable
    {
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Conexão corrente
        /// </summary>
        public ConnectionDb Connection
        {
            get
            {
                return this._connection;
            }
        }

        private ConnectionDb _connection;

        public DAOAbstract(ConnectionDb connection)
        {
            this._connection = connection;
        }
    }
}
