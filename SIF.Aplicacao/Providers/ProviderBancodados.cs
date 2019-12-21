using SIF.Dao;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SIF.Aplicacao
{
    public class ProviderBancodados : INotifyPropertyChanged, IDisposable
    {
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private string _caminhoBackup;

        public string CaminhoBackup
        {
            get { return _caminhoBackup; }
            set
            {
                if (_caminhoBackup != value)
                {
                    _caminhoBackup = value;
                    NotifyPropertyChanged("CaminhoBackup");
                }
            }
        }

        /// <summary>
        /// Faz backup do banco de dados - BDSIF
        /// </summary>
        /// <param name="owner">Window pai</param>
        /// <returns></returns>
        public bool BackupDatabase(Window owner)
        {
            try
            {
                string path = string.Format(@"{0}\BKPBDSCP-{1}.BAK", CaminhoBackup, DateTime.Now.ToString("ddMMyyyyHHmm"));
                if (System.IO.File.Exists(path)) System.IO.File.Delete(path);
                using (SqlCommand command = SistemaGlobal.Sis.Connection.GetSqlConnection().CreateCommand())
                {
                    command.CommandType = System.Data.CommandType.Text;
                    command.CommandText = string.Format("BACKUP DATABASE BDSIF TO DISK = '{0}'", path);
                    command.CommandTimeout = 3600;
                    command.ExecuteNonQuery();

                    System.IO.FileInfo fileInfo = new System.IO.FileInfo(path);
                    Properties.Settings.Default.DataFileRestore = fileInfo.LastWriteTime.ToString("dd/MM/yyyy HH:mm:ss");
                    Properties.Settings.Default.Save();
                }
            }
            catch (Exception ex)
            {
                return SistemaGlobal.Sis.Log.LogError(ex, true, owner);
            }
            return true;
        }

        public bool RestoreDatabaseAutomatico(Window owner)
        {
            if (!ConfigRestoreAuto())
            {
                return true;
            }
            else
            {
                bool result = false;
                string path = GetPathRestore();
                if (!string.IsNullOrEmpty(path))
                {
                    SistemaGlobal.Sis.Msg.ExecutaSync(owner, "Restaurando banco de dados, aguarde ...",
                        () =>
                        {
                            Restore(path);
                        });
                }
                return result;
            }
        }

        private bool Restore(string path)
        {
            bool result = false;
            try
            {
                SistemaGlobal.Sis.Connection.GetSqlConnection().ChangeDatabase("master");
                string commandText = string.Format("RESTORE DATABASE BDSIF FROM DISK = '{0}' WITH REPLACE;", path);
                using (SqlCommand command = SistemaGlobal.Sis.Connection.GetSqlConnection().CreateCommand())
                {
                    command.CommandType = System.Data.CommandType.Text;
                    command.CommandTimeout = 3600;
                    command.CommandText = commandText;
                    command.ExecuteNonQuery();
                    result = true;
                }
            }
            catch (Exception ex)
            {
                SistemaGlobal.Sis.Log.LogError(ex);
            }
            finally
            {
                SistemaGlobal.Sis.Connection.GetSqlConnection().ChangeDatabase("BDSIF");
            }
            return result;
        }

        public bool ConfigRestoreAuto()
        {
            return (Properties.Settings.Default.RestoreAoLogar && System.IO.Directory.Exists(Properties.Settings.Default.PathRestoreAoLogar));
        }

        public bool ConfigRestoreMaisRecente()
        {
            return (Properties.Settings.Default.RestoreFileRecente && System.IO.Directory.Exists(Properties.Settings.Default.PathFileRestore));
        }

        private string GetPathRestore()
        {
            try
            {
                System.IO.DirectoryInfo directoryInfo = new System.IO.DirectoryInfo(Properties.Settings.Default.PathRestoreAoLogar);
                DateTime dateRestore = DateTime.Now;
                if (!string.IsNullOrEmpty(Properties.Settings.Default.DataFileRestore) && DateTime.TryParse(Properties.Settings.Default.DataFileRestore, out dateRestore))
                {
                    System.IO.FileInfo fileInfo = (from i in directoryInfo.GetFiles() where i.Name.StartsWith("BKPBDSCP") select i).OrderByDescending(c => c.LastWriteTime).FirstOrDefault();
                    if (fileInfo != null)
                    {
                        Int64 int64dateRestore = GetValue(dateRestore);
                        Int64 int64LastWriteTime = GetValue(fileInfo.LastWriteTime);
                        if (int64LastWriteTime > int64dateRestore)
                        {
                            Properties.Settings.Default.DataFileRestore = fileInfo.LastWriteTime.ToString("dd/MM/yyyy HH:mm:ss");
                            Properties.Settings.Default.Save();

                            return fileInfo.FullName;
                        }
                    }
                }
                else
                {
                    System.IO.FileInfo fileInfo = (from i in directoryInfo.GetFiles() where i.Name.StartsWith("BKPBDSCP") select i).OrderByDescending(c => c.LastWriteTime).FirstOrDefault();
                    if (fileInfo != null)
                    {
                        Properties.Settings.Default.DataFileRestore = fileInfo.LastWriteTime.ToString("dd/MM/yyyy HH:mm:ss");
                        Properties.Settings.Default.Save();
                    }
                }
            }
            catch
            {
            }
            return "";
        }

        private long GetValue(DateTime dateRest)
        {
            return Convert.ToInt64(dateRest.ToString().Replace("/", "").Replace(":", "").Replace(" ", ""));
        }

        public bool RestoreDatabase(Window owner)
        {
            return RestoreDatabase(owner, "");
        }

        /// <summary>
        /// Faz restauranção do banco de dados - BDSIF
        /// </summary>
        /// <param name="owner">Window pai</param>
        /// <param name="pathBkp">Caminho do backup.bak</param>
        /// <returns></returns>
        public bool RestoreDatabase(Window owner, string pathBkp)
        {
            bool result = false;
            try
            {
                if (ConfigRestoreMaisRecente())
                {
                    System.IO.DirectoryInfo directoryInfo = new System.IO.DirectoryInfo(Properties.Settings.Default.PathFileRestore);
                    System.IO.FileInfo fileInfo = (from i in directoryInfo.GetFiles() where i.Name.StartsWith("BKPBDSCP") select i).OrderByDescending(c => c.LastWriteTime).FirstOrDefault();
                    if (fileInfo != null)
                        result = Restore(fileInfo.FullName);
                }
                else
                {
                    result = Restore(pathBkp);
                }
            }
            catch (Exception ex)
            {
                return SistemaGlobal.Sis.Log.LogError(ex, true, owner);
            }
            return result;
        }
    }
}
