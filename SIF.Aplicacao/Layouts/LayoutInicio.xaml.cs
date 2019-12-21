using System;
using System.Collections.Generic;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SIF.Aplicacao.Layouts
{
    /// <summary>
    /// Interaction logic for LayoutInicio.xaml
    /// </summary>
    public partial class LayoutInicio : UserControl
    {
        public string DataHora
        {
            get { return (string)GetValue(DataHoraProperty); }
            set { SetValue(DataHoraProperty, value); }
        }

        // Using a DependencyProperty as the backing store for DataHora.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DataHoraProperty =
            DependencyProperty.Register("DataHora", typeof(string), typeof(LayoutInicio));


        public LayoutInicio()
        {
            InitializeComponent();

            this.DataContext = this;

            this.DataHora = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");

            Timer _timer = new Timer(1000);
            _timer.Elapsed += new ElapsedEventHandler(T_Elapsed);
            _timer.Enabled = true;
        }

        private void T_Elapsed(object sender, ElapsedEventArgs e)
        {
            this.Dispatcher.BeginInvoke(new Action(() =>
                {
                    this.DataHora = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
                }));
        }
    }
}
