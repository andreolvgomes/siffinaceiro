using SIF.WPF.Styles.Windows.Controls;
using SIF.Commom;
using SIF.Dao;
using SIF.Aplicacao.UserControls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace SIF.Aplicacao.LayoutCadastroW
{
    /// <summary>
    /// Interaction logic for CadastroClientes.xaml
    /// </summary>
    public partial class CadastroClientes : ModernWindow
    {
        private ProviderInterfacesCadastros2<Clientes> provider;

        public CadastroClientes(Window owner, ConnectionDb conexao)
        {
            InitializeComponent();

            provider = new ProviderInterfacesCadastros2<Clientes>(this, buttons, conexao);

            provider.Event_ConvertValoresDatabaseToInterfaceEventHandler += new ExecucaoCommandEventHandler<Clientes>(SetTrocaVAloresBaseToInterface);
            provider.Event_ConvertValoresInterfaceToDatabaseSaveEventHandler += new ExecucaoCommandEventHandler<Clientes>(SetTrocaValoresInterfaceToBase);
            provider.Event_ValidacaoCamposCorrenteEventHandler += new ExecucaoCommandsComDaoSqlBooleanEventHandler<Clientes>(Validacao);
            provider.Event_GetNewRecordEventHandler += new ExecutacaoCommandsComDaoSqlModelEventHandler<Clientes>(GetNewRecod);
            provider.Event_PesquisaEventHandler += new ModelPesquisaEventHandler(ExecutaPesquisa);
            provider.Event_ConfirmacaoDeleteEventHandler += new ExecucaoCommandsComDaoSqlBooleanEventHandler<Clientes>(ConfirmacaoDelete);

            provider.LoadRegistros("Clientes", "Cli_codigo");
            if (provider.Entidade == null)
                provider.Control_Buttons(ButtonType.New);

            this.DataContext = provider;

            using (EffectWindow e = new EffectWindow())
            {
                e.SetEffectBackgound(owner, this);
            }
            if (System.Windows.Forms.Screen.GetWorkingArea(new System.Drawing.Point()).Bottom <= 728 && System.Windows.Forms.Screen.GetWorkingArea(new System.Drawing.Point()).Width <= 1366)
            {
                this.WindowState = System.Windows.WindowState.Maximized;
                this.SizeToContent = System.Windows.SizeToContent.Manual;
            }

            SistemaGlobal.Sis.DefinicoesUsuario.SetDefinicoesWindow(this, SIF.Commom.UssessaoEnum.CADADASTRO_CLIENTES, buttons);

            this.PreviewKeyDown += new KeyEventHandler(W_PreviewKeyDown);
        }

        private bool ConfirmacaoDelete(Clientes model)
        {
            if (SistemaGlobal.Sis.Msg.MostraMensagem("Deseja realmente excluir o registro?", "Atenção", MessageBoxButton.YesNo, this) == MessageBoxResult.Yes)
                return true;
            return false;
        }

        private void ExecutaPesquisa()
        {
            using (ClientesPes pes = new ClientesPes(SistemaGlobal.Sis.Connection))
            {
                provider.SetEntidade(pes.Pesquisa(this));
            }
        }

        private void W_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.SystemKey == Key.F10)
            {
                ExecutaPesquisa();
                e.Handled = true;
            }
        }

        private void SetTrocaValoresInterfaceToBase(Clientes model)
        {
            if (!string.IsNullOrEmpty(model.DataNascimento))
            {
                DateTime date;
                if (DateTime.TryParse(model.DataNascimento, out date))
                    model.Cli_datanascimento = date;
            }
        }

        private void SetTrocaVAloresBaseToInterface(Clientes model)
        {
            if (model.Cli_datanascimento != null)
                model.DataNascimento = Convert.ToDateTime(model.Cli_datanascimento).ToString("dd/MM/yyyy");
        }

        private Clientes GetNewRecod()
        {
            SelecionaText(txtNomeCompleto);
            return Clientes.GetNewCliente();
        }

        private bool Validacao(Clientes model)
        {
            if (string.IsNullOrEmpty(model.Cli_nome))
            {
                SistemaGlobal.Sis.Msg.MostraMensagem("O campo Nome não pode ser vazio!", "Atenção", MessageBoxButton.OK, this);
                return SelecionaText(txtNomeCompleto);
            }
            if (string.IsNullOrEmpty(model.Cli_endereco))
            {
                SistemaGlobal.Sis.Msg.MostraMensagem("O campo Endereço não pode ser vazio!", "Atenção", MessageBoxButton.OK, this);
                return SelecionaText(txtEndereco);
            }
            if (string.IsNullOrEmpty(model.Cli_bairro))
            {
                SistemaGlobal.Sis.Msg.MostraMensagem("O campo Bairro não pode ser vazio!", "Atenção", MessageBoxButton.OK, this);
                return SelecionaText(txtBairro);
            }
            if (string.IsNullOrEmpty(model.Cli_cidade))
            {
                SistemaGlobal.Sis.Msg.MostraMensagem("O campo Cidade não pode ser vazio!", "Atenção", MessageBoxButton.OK, this);
                return SelecionaText(txtCidade);
            }
            model.Cli_nomerazao = model.Cli_nome;
            return true;
        }

        private void btnAddFoto_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.OpenFileDialog op = new System.Windows.Forms.OpenFileDialog();
            op.Filter = "Imagens JPG|*.jpg|Imagens PNG|*.png";
            op.RestoreDirectory = true;

            if (op.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                using (FileStream fs = new FileStream(op.FileName, FileMode.Open, FileAccess.Read))
                {
                    byte[] data = new byte[fs.Length];
                    fs.Read(data, 0, System.Convert.ToInt32(fs.Length));
                    using (RedimensionarImage resize = new RedimensionarImage())
                    {
                        provider.Entidade.Cli_foto = resize.ImageResize(data, (int)imgFoto.Width, (int)imgFoto.Height);
                    }
                }
            }
        }

        private void imgFoto_Event_DelegateExecuteButton_1(ButtonType button)
        {
            switch (button)
            {
                case ButtonType.Next:
                    break;
                case ButtonType.Previous:
                    break;
                case ButtonType.Alter:
                    System.Windows.Forms.OpenFileDialog op = new System.Windows.Forms.OpenFileDialog();
                    op.Filter = "Imagens JPG|*.jpg|Imagens PNG|*.png";
                    op.RestoreDirectory = true;

                    if (op.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        using (FileStream fs = new FileStream(op.FileName, FileMode.Open, FileAccess.Read))
                        {
                            byte[] data = new byte[fs.Length];
                            fs.Read(data, 0, System.Convert.ToInt32(fs.Length));
                            using (RedimensionarImage resize = new RedimensionarImage())
                            {
                                provider.Entidade.Cli_foto = resize.ImageResize(data, (int)imgFoto.Width, (int)imgFoto.Height);
                            }
                        }
                    }
                    break;
            }
        }
    }
}
