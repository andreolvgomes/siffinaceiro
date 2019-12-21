using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace RestoreDb
{
    public class Provider : INotifyPropertyChanged
    {
        private string _Progress = "";

        public string Progress
        {
            get { return _Progress; }
            set
            {
                if (_Progress != value)
                {
                    _Progress = value;
                    OnPropertyChanged("Progress");
                }
            }
        }

        private int _percent;

        public int Percent
        {
            get { return _percent; }
            set
            {
                if (_percent != value)
                {
                    _percent = value;
                    OnPropertyChanged("Percent");
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        public event RestoreCompletedSucessCloseWindow RestoreCompletedSucessCloseWindow;

        private void OnRestoreCompletedSucessCloseWindow()
        {
            RestoreCompletedSucessCloseWindow restoreCompletedSucessCloseWindow = RestoreCompletedSucessCloseWindow;
            if (restoreCompletedSucessCloseWindow != null)
                restoreCompletedSucessCloseWindow();
        }

        public XmlInformacoes Informacoes { get; private set; }
        private Timer timer;

        public Provider(XmlInformacoes inf)
        {
            this.Informacoes = inf;
        }

        public void RestauraDatabase()
        {
            System.Threading.Thread sync = new System.Threading.Thread(new System.Threading.ThreadStart(Restore));
            sync.Start();
        }

        private void Restore()
        {
            timer = new Timer(200);
            timer.Elapsed += new ElapsedEventHandler(T_Elapsed);
            timer.Enabled = true;

            //for (int i = 0; i < 100; i++)
            //{
            //    Percent = i;
            //    System.Threading.Thread.Sleep(50);
            //}
            
            try
            {
                string commandText = string.Format("RESTORE DATABASE {0} FROM DISK = '{1}' WITH REPLACE;", Informacoes.Database, Informacoes.CaminhoArquivo);
                using (SqlConnection connection = new SqlConnection(Informacoes.ConnectionString))
                {                    
                    connection.Open();
                    connection.ChangeDatabase("master");
                    using (SqlCommand command = connection.CreateCommand())
                    {
                        command.CommandType = System.Data.CommandType.Text;
                        command.CommandText = commandText;
                        command.CommandTimeout = 3600;
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            timer.Enabled = false;
            OnRestoreCompletedSucessCloseWindow();
        }

        private void T_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (Progress.Length < 5)
                Progress += ".";
            else
                Progress = "";
        }
    }
}
