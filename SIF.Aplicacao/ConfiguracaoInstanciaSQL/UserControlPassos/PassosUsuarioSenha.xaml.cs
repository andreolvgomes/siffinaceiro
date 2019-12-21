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

namespace SIF.Aplicacao.ConfiguracaoInstanciaSQL.UserControlPassos
{
    /// <summary>
    /// Interaction logic for PassosUsuarioSenha.xaml
    /// </summary>
    public partial class PassosUsuarioSenha : UserControl
    {
        private ContextInicial context;

        public PassosUsuarioSenha(ContextInicial context)
        {
            InitializeComponent();

            this.context = context;
            
            this.DataContext = context;

            txtSenha.Password = context.SenhaSis;
            txtSenhaConfirmacao.Password = context.SenhaSis;
        }

        internal bool Validacao(Window owner)
        {
            if (!this.context.UsuarioSisExists)
            {
                if (!this.context.ValidaUsuarioSis(owner)) return this.context.SelecinaText(txtUsuario);
                if (!this.context.ValidaSenhaSis(owner, txtSenha.Password)) return this.context.SelecinaText(txtSenha);
                if (!this.context.ValidacaoSenhaConfSis(owner, txtSenhaConfirmacao.Password)) return this.context.SelecinaText(txtSenhaConfirmacao);
                if (!this.context.ValidacaoSenhaSisEquals(owner, txtSenha.Password, txtSenhaConfirmacao.Password)) return this.context.SelecinaText(txtSenha);
                this.context.SenhaSis = txtSenha.Password;
            }
            return true;
        }
    }
}