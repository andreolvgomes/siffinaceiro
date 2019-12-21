using SIF.Aplicacao.LayoutConfiguracaoW;
using SIF.Aplicacao.LayoutWindows.LayoutConfiguracoesW;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SIF.Aplicacao.Layouts
{
    /// <summary>
    /// Interaction logic for LayoutConfiguracoes.xaml
    /// </summary>
    public partial class LayoutPainel : UserControl
    {
        public LayoutPainel()
        {
            InitializeComponent();
            if (SistemaGlobal.Sis.XmlSistema.Armazenamento == ConfiguracaoInstanciaSQL.TIPO_ARMAZENAMENTO.ARMAZENAMENTO_EM_ARQUIVO)
                this.stkBanco.Visibility = System.Windows.Visibility.Collapsed;
        }

        private void Usuarios_Click(object sender, MouseButtonEventArgs e)
        {
            SistemaGlobal.Sis.ControleJanelas.ShowWindow(new LayoutConfiguracaoW.CadastroUsuario(Window.GetWindow(this), SistemaGlobal.Sis.Connection));
        }

        private void AlterarSenha_Click(object sender, MouseButtonEventArgs e)
        {
            SistemaGlobal.Sis.ControleJanelas.ShowWindow(new LayoutConfiguracaoW.AlteracaoSenha(Window.GetWindow(this), SistemaGlobal.Sis.Connection, SistemaGlobal.Sis.DefinicoesUsuario.Usuario));
        }

        private void Backup_Click(object sender, MouseButtonEventArgs e)
        {
            SistemaGlobal.Sis.ControleJanelas.ShowWindow(new BancoBackup(Window.GetWindow(this), SistemaGlobal.Sis.Connection));
        }

        private void RestauraBanco_Click(object sender, MouseButtonEventArgs e)
        {
            using (ProviderBancodados provider = new ProviderBancodados())
            {
                if (provider.ConfigRestoreMaisRecente())
                {
                    if (SistemaGlobal.Sis.Msg.MostraMensagem("O Sistema está configurado para restaurar o backup mais recente!\n\nDESEJA REALMENTE CONTINUAR?", "Atenção", MessageBoxButton.YesNo, Window.GetWindow(this)) == MessageBoxResult.Yes)
                    {
                        if (provider.RestoreDatabase(Window.GetWindow(this)))
                            SistemaGlobal.Sis.Msg.MostraMensagem("Restauração concluída com sucesso", "Atenção", MessageBoxButton.OK, Window.GetWindow(this));
                    }
                }
                else
                {
                    System.Windows.Forms.OpenFileDialog op = new System.Windows.Forms.OpenFileDialog();
                    op.Filter = "Backup BAK|*.bak";
                    op.RestoreDirectory = true;
                    if (op.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        Window owner = Window.GetWindow(this);
                        SistemaGlobal.Sis.Msg.ExecutaSync(owner, "Restaurando banco de dados, aguarde ...",
                            () =>
                            {
                                if (provider.RestoreDatabase(owner, op.FileName))
                                    SistemaGlobal.Sis.Msg.MostraMensagem("Restauração concluída com sucesso", "Atenção", MessageBoxButton.OK, owner);
                            });
                    }
                }
            }
        }

        private void ImageIcon_MouseLeftButtonDown_1(object sender, MouseButtonEventArgs e)
        {
            Configuracoes conf = new Configuracoes(Window.GetWindow(this));
            conf.ShowDialog();
        }
    }
}
