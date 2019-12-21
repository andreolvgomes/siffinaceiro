using SIF.Dao;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIF.Aplicacao
{
    public class UsuariosPes : IModelPesquisa<Usuarios>
    {
        public UsuariosPes(ConnectionDb conexao)
            : base(conexao)
        {
        }

        public override Usuarios Pesquisa(System.Windows.Window owner)
        {
            if (this.Consulta(owner, "USUÁRIOS", "SELECT * FROM Usuarios") != null)
            {
                string usu_nome = GetValuePesquisa<string>("Usu_nome");
                return SistemaGlobal.Sis.Connection.GetRegistros<Usuarios>(string.Format("Usu_nome = '{0}'", usu_nome)).FirstOrDefault();
            }
            return null;
        }
    }
}
