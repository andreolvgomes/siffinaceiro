using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SIF.Aplicacao
{
    public class DbExecutaScripts : IDisposable
    {
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        //public void CreateView(Database db, string instancia)
        //{
        //    try
        //    {
        //        Sistema.Sis.Msg.MostraMensagem("Atenção", "Criando as View, aguarde ...", "", null,
        //                  () =>
        //                  {
        //                      try
        //                      {
        //                          string arguments = "";
        //                          string arquivo = "";

        //                          if (db == Database.BDSIC)
        //                          {
        //                              arquivo = "ViewSistema.sql";
        //                              arguments = string.Format(@" -S {0} -d {1} -i {2} -U sa -P sic742", instancia, DescricaoEnums.PegaDescricaoEnum(db), arquivo);
        //                          }
        //                          else
        //                          {
        //                              arquivo = "ViewSistemaPaf.sql";
        //                              arguments = string.Format(@" -S {0} -d {1} -i {2} -U sa -P sic742", instancia, DescricaoEnums.PegaDescricaoEnum(db), arquivo);
        //                          }

        //                          //string osql = RenomeaOSQL(true);

        //                          if (System.IO.File.Exists(arquivo))
        //                          {
        //                              /// OSSQL.EXE É DO SQL2000 SÓ ASSIM FUNCIONA NO WINDOW XP
        //                              /// 

        //                              //System.Threading.Thread.Sleep(1000);
        //                              System.Diagnostics.ProcessStartInfo inf = new System.Diagnostics.ProcessStartInfo()
        //                              {
        //                                  Arguments = arguments,
        //                                  WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden,
        //                                  CreateNoWindow = true,
        //                                  FileName = System.IO.Path.Combine(Environment.CurrentDirectory, "OSQL.exe")
        //                              };
        //                              //System.Diagnostics.Process.Start(inf);

        //                              ///aguarda 3 segundos
        //                              ///
        //                              //System.Threading.Thread.Sleep(2000);

        //                              System.Diagnostics.Process p = new System.Diagnostics.Process();
        //                              p.StartInfo = inf;
        //                              p.Start();

        //                              do
        //                              {
        //                                  /// Enquanto for false significa que o processo ainda está sendo executado.
        //                                  /// 
        //                              } while (!p.HasExited);
        //                          }

        //                          //RenomeaOSQL(false);
        //                      }
        //                      catch
        //                      {
        //                      }
        //                  });
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        public bool CreateDatabase(string instancia)
        {
            return CreateDatabase(null, instancia);
        }

        /// <summary>
        /// Cria banco de dados caso não exista
        /// </summary>
        /// <param name="instancia"></param>
        /// <returns></returns>
        public bool CreateDatabase(Window owner, string instancia)
        {
            bool result = false;
            try
            {
                SistemaGlobal.Sis.Msg.ExecutaSync(owner, "Criando banco de dados, aguarde ...",
                          () =>
                          {
                              try
                              {
                                  string arquivo = "BDSIF.SQL";
                                  if (System.IO.File.Exists(arquivo))
                                  {
                                      DataTable database = new DataTable();
                                      using (SqlConnection connection = new SqlConnection(string.Format("Server = {0}; Database = master; Integrated Security = SSPI;", instancia)))
                                      {
                                          connection.Open();
                                          using (SqlCommand command = connection.CreateCommand())
                                          {
                                              command.CommandType = CommandType.Text;
                                              command.CommandText = "SELECT * FROM sys.databases WHERE name = 'BDSIF';";
                                              using (SqlDataAdapter adp = new SqlDataAdapter(command))
                                              {
                                                  adp.Fill(database);
                                              }
                                              /// tem ou não o banco?
                                              /// 
                                              if (database.Rows.Count == 0) // não tem! que merda.
                                              {
                                                  command.CommandText = string.Format("CREATE DATABASE BDSIF");
                                                  command.ExecuteNonQuery();
                                              }
                                          }
                                      }
                                      if (database.Rows.Count == 0)
                                      {
                                          string arguments = string.Format(@" -S {0} -d BDSIF -i {1} -U sa -P 123456", instancia, arquivo);

                                          System.Diagnostics.ProcessStartInfo inf = new System.Diagnostics.ProcessStartInfo()
                                          {
                                              Arguments = arguments,
                                              WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden,
                                              CreateNoWindow = true,
                                              FileName = System.IO.Path.Combine(Environment.CurrentDirectory, "OSQL.exe")
                                          };
                                          System.Diagnostics.Process p = new System.Diagnostics.Process();
                                          p.StartInfo = inf;
                                          p.Start();

                                          while (!p.HasExited)
                                          {
                                          }
                                          result = true;
                                      }
                                  }
                              }
                              catch
                              {
                              }
                          });
            }
            catch
            {
            }
            return result;
        }
    }
}
