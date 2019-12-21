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
    /// Interaction logic for CadastroImageLogo.xaml
    /// </summary>
    public partial class CadastroImageLogo : UserControl
    {
        public ImageSource Imagem
        {
            get { return (ImageSource)GetValue(ImagemProperty); }
            set { SetValue(ImagemProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Imagem.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ImagemProperty =
            DependencyProperty.Register("Imagem", typeof(ImageSource), typeof(CadastroImageLogo)); //, new PropertyMetadata(new BitmapImage(new Uri("pack://application:,,,/SIF.Aplicacao;component/Icones/default.png"))));


        private ICommand _commandNext;
        public ICommand CommandNext
        {
            get { return _commandNext ?? (_commandNext = new ExecuteCommandDelegate(ExecuteEventCommandButton, ButtonType.Next)); }
        }
        private ICommand _commandPrevious;
        public ICommand CommandPrevious
        {
            get { return _commandPrevious ?? (_commandPrevious = new ExecuteCommandDelegate(ExecuteEventCommandButton, ButtonType.Previous)); }
        }
        private ICommand _commandAlter;
        public ICommand CommandAlter
        {
            get { return _commandAlter ?? (_commandAlter = new ExecuteCommandDelegate(ExecuteEventCommandButton, ButtonType.Alter)); }
        }
        public event DelegateExecuteButton Event_DelegateExecuteButton;

        private void ExecuteEventCommandButton(ButtonType button)
        {
            if (Event_DelegateExecuteButton != null)
                Event_DelegateExecuteButton(button);
        }

        public CadastroImageLogo()
        {
            InitializeComponent();

            this.DataContext = this;
        }
    }
}
