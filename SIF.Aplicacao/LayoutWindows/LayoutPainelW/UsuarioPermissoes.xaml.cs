using SIF.Dao;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    public partial class UsuarioPermissoes
    {
        public ObservableCollection<Node> Nodes
        {
            get { return (ObservableCollection<Node>)GetValue(NodeProperty); }
            set { SetValue(NodeProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Node.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty NodeProperty =
            DependencyProperty.Register("Nodes", typeof(ObservableCollection<Node>), typeof(UsuarioPermissoes));



        public Usuarios Usuario
        {
            get { return (Usuarios)GetValue(UsuarioProperty); }
            set { SetValue(UsuarioProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Usuario.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty UsuarioProperty =
            DependencyProperty.Register("Usuario", typeof(Usuarios), typeof(UsuarioPermissoes));


        //private DaoGenerico<Uscontrolesecao> provider;
        private ProviderRecord<Uscontrolesecao> provider;

        public UsuarioPermissoes(Window owner, Usuarios usuario, ConnectionDb conexao)
        {
            InitializeComponent();

            provider = new ProviderRecord<Uscontrolesecao>();

            using (EffectWindow e = new EffectWindow())
            {
                e.SetEffectBackgound(owner, this);
            }

            this.Usuario = usuario;

            this.DataContext = this;

            Nodes = new ObservableCollection<LayoutConfiguracaoW.Node>();
            LoadedOps();

            this.PreviewKeyDown += new KeyEventHandler(W_PreviewKeyDown);
            this.Closing += new System.ComponentModel.CancelEventHandler(W_Closing);
            this.Loaded += new RoutedEventHandler(W_Loaded);
        }

        private void W_PreviewKeyDown(object sender, KeyEventArgs e)
        {
        }

        private void W_Loaded(object sender, RoutedEventArgs e)
        {
            this.Dispatcher.BeginInvoke(new Action(() =>
                {
                    CheckedTreeChild(Nodes);
                }));
        }

        private void CheckedTreeChild(ObservableCollection<Node> Nodes)
        {
            foreach (Node n in Nodes)
            {
                if (n.IsChecked == false)
                    n.CheckedTreeChildLoaded();
                if (n.Children.Count > 0)
                    CheckedTreeChild(n.Children);
            }
        }

        private void LoadedOps()
        {
            foreach (MenuCompleto menu in SistemaGlobal.Sis.DefinicoesUsuario.Menucompleto)
            {
                Node principal = new Node() { Text = menu.Menu.Usm_descricao };
                foreach (MenuUssecao con in menu.Sessao)
                {
                    Node subPrincipal = new Node() { Text = con.Secao.Uss_descricao };
                    Uscontrolesecao permissoes = con.Permissoes;
                    if (permissoes != null)
                    {
                        foreach (string str in new string[] { "I", "A", "E", "D" })
                        {
                            Node ops = new Node();
                            ops.Permissoes = permissoes;

                            switch (str)
                            {
                                case "I":
                                    ops.Text = "Incluir";
                                    ops.IsChecked = permissoes.Usc_incluir;
                                    break;
                                case "A":
                                    ops.Text = "Alterar";
                                    ops.IsChecked = permissoes.Usc_editar;
                                    break;
                                case "E":
                                    ops.Text = "Excluir";
                                    ops.IsChecked = permissoes.Usc_excluir;
                                    break;
                                case "D":
                                    ops.Text = "Disponível no menu";
                                    ops.IsChecked = permissoes.Usc_disponivel;
                                    break;
                            }
                            ops.IsCheckedEventArgs += new EventHandler<ChangedIsCheckedEventArgs>(IsCheckedEventArgs);
                            ops.Parent.Add(subPrincipal);
                            subPrincipal.Children.Add(ops);
                        }
                    }

                    subPrincipal.Parent.Add(principal);
                    principal.Children.Add(subPrincipal);
                }
                Nodes.Add(principal);
            }
        }

        private void ExpandTree(ObservableCollection<Node> items, bool isExpanded)
        {
            foreach (Node item in items)
            {
                item.IsExpanded = isExpanded;
                if (item.Children.Count != 0) ExpandTree(item.Children, isExpanded);
            }
        }

        private void IsCheckedEventArgs(object sender, ChangedIsCheckedEventArgs e)
        {
            provider.Update(e.Permisoes as Uscontrolesecao);
        }

        private void W_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            provider.Dispose();
        }

        private void OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            CheckBox currentCheckBox = (CheckBox)sender;
            CheckBoxId.checkBoxId = currentCheckBox.Uid;
        }

        private void CheckedTree(ObservableCollection<Node> items, bool isChecked)
        {
            this.Dispatcher.BeginInvoke(new Action(() =>
                {
                    foreach (Node item in items)
                    {
                        item.IsChecked = isChecked;
                        if (item.Children.Count != 0) CheckedTree(item.Children, isChecked);
                    }
                }));
        }

        private void Agrupar_Click(object sender, RoutedEventArgs e)
        {
            ExpandTree(Nodes, false);
        }

        private void Expandir_Click(object sender, RoutedEventArgs e)
        {
            ExpandTree(Nodes, true);
        }

        private void MarcarTodos_Click(object sender, RoutedEventArgs e)
        {
            CheckedTree(Nodes, true);
        }

        private void DesmarcarTodos_Click(object sender, RoutedEventArgs e)
        {
            CheckedTree(Nodes, false);
        }
    }
}
