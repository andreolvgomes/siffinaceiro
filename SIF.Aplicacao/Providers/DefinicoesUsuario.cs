using SIF.Commom;
using SIF.Dao;
using SIF.Aplicacao.UserControls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace SIF.Aplicacao
{
    /// <summary>
    /// 
    /// </summary>
    public class MenuUssecao
    {
        /// <summary>
        /// 
        /// </summary>
        public Uscontrolesecao Permissoes { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Ussecao Secao { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="permissoes"></param>
        /// <param name="secao"></param>
        public MenuUssecao(Uscontrolesecao permissoes, Ussecao secao)
        {
            this.Permissoes = permissoes;
            this.Secao = secao;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class MenuCompleto
    {
        /// <summary>
        /// 
        /// </summary>
        public Usmenu Menu { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public List<MenuUssecao> Sessao { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="menu"></param>
        /// <param name="sessao"></param>
        public MenuCompleto(Usmenu menu, List<MenuUssecao> sessao)
        {
            this.Menu = menu;
            this.Sessao = sessao;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class DefinicoesUsuario : INotifyPropertyChanged, IDisposable
    {
        /// <summary>
        /// 
        /// </summary>
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// 
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private List<MenuCompleto> _Menucompleto;
        /// <summary>
        /// 
        /// </summary>
        public List<MenuCompleto> Menucompleto
        {
            get { return _Menucompleto; }
            set
            {
                _Menucompleto = value;
            }
        }

        private List<Uscontrolesecao> _permissoes;
        /// <summary>
        /// 
        /// </summary>
        public List<Uscontrolesecao> Permissoes
        {
            get { return _permissoes; }
            set
            {
                _permissoes = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sessao"></param>
        /// <returns></returns>
        public Uscontrolesecao this[UssessaoEnum sessao]
        {
            get
            {
                return Permissoes.FirstOrDefault(c => c.Uss_descricao == PegaDescricaoEnum.GetDescription(sessao));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public Usuarios Usuario { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DefinicoesUsuario()
        {
            this.Menucompleto = new List<MenuCompleto>();
            this.Permissoes = new List<Uscontrolesecao>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="usuario"></param>
        /// <param name="conexao"></param>
        public void SetUsuario(Usuarios usuario, ConnectionDb conexao)
        {
            if (this.Usuario != usuario)
            {
                this.Usuario = usuario;

                List<Usmenu> _listMenus = new List<Usmenu>();
                List<Ussecao> _listSecao = new List<Ussecao>();

                List<MenuUssecao> completo = new List<MenuUssecao>();

                this.Permissoes = SistemaGlobal.Sis.Connection.GetRegistros<Uscontrolesecao>(string.Format("Usu_codigo = {0}", usuario.Usu_codigo));
                _listMenus = SistemaGlobal.Sis.Connection.GetRegistros<Usmenu>();
                _listSecao = SistemaGlobal.Sis.Connection.GetRegistros<Ussecao>();


                foreach (Ussecao ussecao in _listSecao)
                {
                    completo.Add(new MenuUssecao(Permissoes.FirstOrDefault(c => c.Uss_descricao == ussecao.Uss_descricao), ussecao));
                }
                foreach (Usmenu menu in _listMenus)
                {
                    Menucompleto.Add(new MenuCompleto(menu, completo.Where(c => c.Secao.Usm_sequencial == menu.Usm_sequencial).ToList()));
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="janela"></param>
        /// <param name="sessao"></param>
        public void SetDefinicoesWindow(Window janela, UssessaoEnum sessao)
        {
            SetDefinicoesWindow(janela, sessao, null);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="janela"></param>
        /// <param name="sessao"></param>
        /// <param name="buttons"></param>
        public void SetDefinicoesWindow(Window janela, UssessaoEnum sessao, Buttons buttons)
        {
            Uscontrolesecao controle = this[sessao];
            if (buttons != null)
            {
                if (!controle.Usc_excluir) buttons.btnExcluir.Visibility = Visibility.Collapsed;
                if (!controle.Usc_incluir) buttons.btnNovo.Visibility = Visibility.Collapsed;
            }
            if (!controle.Usc_editar)
            {
                    foreach (TextBox textbox in janela.FindChildren<TextBox>())
                        textbox.IsReadOnly = true;
                    foreach (ComboBox combobox in janela.FindChildren<ComboBox>())
                        combobox.IsEnabled = false;
                    foreach (CheckBox checkBox in janela.FindChildren<CheckBox>())
                        checkBox.IsEnabled = false;
                    foreach (RadioButton radioButton in janela.FindChildren<RadioButton>())
                        radioButton.IsEnabled = false;
            }
        }
    }
}
