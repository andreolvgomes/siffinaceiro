using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace RestoreDb
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            XmlInformacoes inf = new XmlInformacoes();
            inf.Database = "BDSIF";
            inf.InstanciaDb = ".";
            inf.CaminhoArquivo = @"D:\OneDrive\backup banco\BKPBDSCP-180320152251.BAK";

            MainWindow m = new MainWindow(inf);
            m.Show();
        }
    }
}
