using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;

namespace SIF.Dao
{
    //public class DaoClientes : IDaoProviders<Clientes>
    //{
    //    public DaoClientes(ConnectioDb conexao)
    //        : base(conexao)
    //    {
    //    }

    //    //public override bool Insert(Clientes model)
    //    //{
    //    //    return Insert(model);
    //    //    //using (SqlCommand command = conexao.GetConexao().CreateCommand())
    //    //    //{
    //    //    //    command.CommandType = System.Data.CommandType.Text;
    //    //    //    using (CreateCommandsSql parameter = new CreateCommandsSql(model))
    //    //    //    {
    //    //    //        command.CommandText = parameter.GetInsert();
    //    //    //        parameter.AddParamters(command);
    //    //    //    }
    //    //    //    //System.Data.SqlDbType
    //    //    //    command.ExecuteNonQuery();

    //    //    //    ListaRecord.Add(model);
    //    //    //    return true;
    //    //    //}
    //    //}

    //    //public override bool Delete(Clientes model)
    //    //{
    //    //    //using (SqlCommand command = conexao.GetConexao().CreateCommand())
    //    //    //{
    //    //    //    command.CommandType = System.Data.CommandType.Text;
    //    //    //    command.CommandText = string.Format("DELETE FROM Clientes WHERE Cli_codigo = @Cli_codigo");

    //    //    //    command.Parameters.AddWithValue("@Cli_codigo", model.Cli_codigo);
    //    //    //    command.ExecuteNonQuery();

    //    //    //    ListaRecord.Remove(model);
    //    //    //    return true;
    //    //    //}
    //    //    return Delete(model);
    //    //}

    //    //public override bool Update(Clientes model)
    //    //{
    //    //    return Update(model);
    //    //    //using (SqlCommand command = conexao.GetConexao().CreateCommand())
    //    //    //{
    //    //    //    command.CommandType = System.Data.CommandType.Text;

    //    //    //    using (CreateCommandsSql parameter = new CreateCommandsSql(model))
    //    //    //    {
    //    //    //        command.CommandText = parameter.GetUpdate(new Dictionary<string, object>());
    //    //    //        parameter.AddParamters(command, true);
    //    //    //    }
    //    //    //    command.ExecuteNonQuery();
    //    //    //    return true;
    //    //    //}
    //    //}

    //    public Clientes GetCliente()
    //    {
    //        Clientes clinew = new Clientes();

    //        clinew.Cli_nome = "";
    //        clinew.Cli_nomerazao = "";
    //        clinew.Cli_endereco = "";
    //        clinew.Cli_bairro = "";
    //        clinew.Cli_cidade = "";
    //        clinew.Cli_cep = "";
    //        clinew.Cli_complemento = "";
    //        clinew.Cli_cpfcnpj = "";
    //        clinew.Cli_extra1 = "";
    //        clinew.Cli_extra2 = "";
    //        clinew.Cli_apelido = "";
    //        clinew.Cli_celular = "";
    //        clinew.Cli_fone1 = "";
    //        clinew.Cli_fone2 = "";
    //        clinew.Cli_naturalidade = "";
    //        clinew.Cli_observacao = "";

    //        clinew.Cli_uf = clinew.ListaCommom.ListUFs.FirstOrDefault();
    //        clinew.Cli_estadocivil = clinew.ListaCommom.ListEstadoCivil.FirstOrDefault();
    //        clinew.Cli_sexo = clinew.ListaCommom.ListSexo.FirstOrDefault();

    //        return clinew;
    //    }

    //    //public override Clientes LoadedRecords()
    //    //{
    //    //    return LoadedRecords("SELECT * FROM Clientes");
    //    //}
    //}
}
