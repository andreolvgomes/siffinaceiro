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

namespace SIF.Aplicacao.UserControls
{
    /// <summary>
    /// Interaction logic for Buttons.xaml
    /// </summary>
    public partial class Buttons : UserControl
    {
        private ICommand _commandNovo;
        public ICommand CommandNovo
        {
            get { return _commandNovo ?? (_commandNovo = new ExecuteCommandDelegate(ExecuteEventCommandButton, ButtonType.New)); }
        }
        private ICommand _commandSalvar;
        public ICommand CommandSalvar
        {
            get { return _commandSalvar ?? (_commandSalvar = new ExecuteCommandDelegate(ExecuteEventCommandButton, ButtonType.Save)); }
        }
        private ICommand _commandCancelar;
        public ICommand CommandCancelar
        {
            get { return _commandCancelar ?? (_commandCancelar = new ExecuteCommandDelegate(ExecuteEventCommandButton, ButtonType.Cancel)); }
        }
        private ICommand _commandExcluir;
        public ICommand CommandExcluir
        {
            get { return _commandExcluir ?? (_commandExcluir = new ExecuteCommandDelegate(ExecuteEventCommandButton, ButtonType.Delete)); }
        }
        private ICommand _commandPrimeiro;
        public ICommand CommandPrimeiro
        {
            get { return _commandPrimeiro ?? (_commandPrimeiro = new ExecuteCommandDelegate(ExecuteEventCommandButton, ButtonType.First)); }
        }
        private ICommand _commandUltimo;
        public ICommand CommandUltimo
        {
            get { return _commandUltimo ?? (_commandUltimo = new ExecuteCommandDelegate(ExecuteEventCommandButton, ButtonType.Last)); }
        }
        private ICommand _commandProximo;
        public ICommand CommandProximo
        {
            get { return _commandProximo ?? (_commandProximo = new ExecuteCommandDelegate(ExecuteEventCommandButton, ButtonType.Next)); }
        }
        private ICommand _commandAnterior;
        public ICommand CommandAnterior
        {
            get { return _commandAnterior ?? (_commandAnterior = new ExecuteCommandDelegate(ExecuteEventCommandButton, ButtonType.Previous)); }
        }
        private ICommand _commandPesquisa;
        public ICommand CommandPesquisa
        {
            get { return _commandPesquisa ?? (_commandPesquisa = new ExecuteCommandDelegate(ExecuteEventCommandButton, ButtonType.Pesquisa)); }
        }

        public event DelegateExecuteButton Event_DelegateExecuteButton;

        private void ExecuteEventCommandButton(ButtonType button)
        {
            if (Event_DelegateExecuteButton != null)
                Event_DelegateExecuteButton(button);
        }

        private Window owner;

        public Buttons()
        {
           InitializeComponent();            

            this.DataContext = this;
            this.Loaded += new RoutedEventHandler(U_Loaded);
        }

        private void U_Loaded(object sender, RoutedEventArgs e)
        {
            owner = Window.GetWindow(this);
            if (owner != null)
                owner.PreviewKeyDown += new KeyEventHandler(W_PreviewKeyDown);
        }

        private void W_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (Keyboard.Modifiers == ModifierKeys.Control && e.Key == Key.I)
                ExecuteEventCommandButton(ButtonType.New);
            else if (Keyboard.Modifiers == ModifierKeys.Control && e.Key == Key.S)
                ExecuteEventCommandButton(ButtonType.Save);
            else if (Keyboard.Modifiers == ModifierKeys.Control && e.Key == Key.Z)
                ExecuteEventCommandButton(ButtonType.Cancel);
        }

        public bool VisivelNovo
        {
            get { return (bool)GetValue(VisivelNovoProperty); }
            set { SetValue(VisivelNovoProperty, value); }
        }

        // Using a DependencyProperty as the backing store for VisivelNovo.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty VisivelNovoProperty =
            DependencyProperty.Register("VisivelNovo", typeof(bool), typeof(Buttons), new PropertyMetadata(true));



        public bool VisivelSalvar
        {
            get { return (bool)GetValue(VisivelSalvarProperty); }
            set { SetValue(VisivelSalvarProperty, value); }
        }

        // Using a DependencyProperty as the backing store for VisivelSalvar.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty VisivelSalvarProperty =
            DependencyProperty.Register("VisivelSalvar", typeof(bool), typeof(Buttons), new PropertyMetadata(true));



        public bool VisivelCancelar
        {
            get { return (bool)GetValue(VisivelCancelarProperty); }
            set { SetValue(VisivelCancelarProperty, value); }
        }

        // Using a DependencyProperty as the backing store for VisivelCancelar.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty VisivelCancelarProperty =
            DependencyProperty.Register("VisivelCancelar", typeof(bool), typeof(Buttons), new PropertyMetadata(true));



        public bool VisivelExcluir
        {
            get { return (bool)GetValue(VisivelExcluirProperty); }
            set { SetValue(VisivelExcluirProperty, value); }
        }

        // Using a DependencyProperty as the backing store for VisivelExcluir.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty VisivelExcluirProperty =
            DependencyProperty.Register("VisivelExcluir", typeof(bool), typeof(Buttons), new PropertyMetadata(true));



        public bool VisivelNavegacao
        {
            get { return (bool)GetValue(VisivelNavegacaoProperty); }
            set { SetValue(VisivelNavegacaoProperty, value); }
        }

        // Using a DependencyProperty as the backing store for VisivelNavegacao.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty VisivelNavegacaoProperty =
            DependencyProperty.Register("VisivelNavegacao", typeof(bool), typeof(Buttons), new PropertyMetadata(true));



        public bool VisivelPesquisa
        {
            get { return (bool)GetValue(VisivelPesquisaProperty); }
            set { SetValue(VisivelPesquisaProperty, value); }
        }

        // Using a DependencyProperty as the backing store for VisivelPesquisa.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty VisivelPesquisaProperty =
            DependencyProperty.Register("VisivelPesquisa", typeof(bool), typeof(Buttons), new PropertyMetadata(true));

        
    }
}
