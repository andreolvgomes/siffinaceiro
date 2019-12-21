using SIF.Dao;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace SIF.Aplicacao.LayoutConfiguracaoW
{
    /// <summary>
    /// Interaction logic for BancoBackup.xaml
    /// </summary>
    public partial class BancoBackup
    {
        private ProviderBancodados provider;

        public BancoBackup(Window owner, ConnectionDb conexao)
        {
            InitializeComponent();

            using (EffectWindow e = new EffectWindow())
            {
                e.SetEffectBackgound(owner, this);
            }
            provider = new ProviderBancodados();

            provider.CaminhoBackup = Properties.Settings.Default.CaminhoBackup;

            this.DataContext = provider;
        }

        private void FazerBackup_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(provider.CaminhoBackup))
            {
                SistemaGlobal.Sis.Msg.MostraMensagem("Escolha um local!", "Atenção", MessageBoxButton.OK, this);
            }
            else
            {
                SistemaGlobal.Sis.Msg.ExecutaSync(this, "Fazendo backup, aguarde ...",
                    () =>
                    {
                        if (provider.BackupDatabase(this))
                            SistemaGlobal.Sis.Msg.MostraMensagem("Backup realizado com sucesso", "Aviso", MessageBoxButton.OK, this);
                        else
                            SistemaGlobal.Sis.Msg.MostraMensagem("Erro", "Erro", MessageBoxButton.OK, this);
                    });
            }
        }

        private void BuscaCaminho_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.FolderBrowserDialog fold = new System.Windows.Forms.FolderBrowserDialog();
            fold.SelectedPath = Environment.GetFolderPath(Environment.SpecialFolder.MyComputer);
            if (fold.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                provider.CaminhoBackup = fold.SelectedPath;
            }
        }

        private void Salvar_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(provider.CaminhoBackup))
            {
                SistemaGlobal.Sis.Msg.MostraMensagem("Escolha um local!", "Atenção", MessageBoxButton.OK, this);
            }
            else
            {
                Properties.Settings.Default.CaminhoBackup = provider.CaminhoBackup;
                Properties.Settings.Default.Save();
            }
        }
    }
}
