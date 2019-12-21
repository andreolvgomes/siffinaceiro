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

namespace RestoreDb
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Provider provider;

        public MainWindow(XmlInformacoes inf)
        {
            InitializeComponent();

            provider = new Provider(inf);
            provider.RestoreCompletedSucessCloseWindow += new RestoreCompletedSucessCloseWindow(RestoreCompleted);

            this.DataContext = provider;

            this.Loaded += new RoutedEventHandler(W_Loaded);
        }

        private void RestoreCompleted()
        {
            this.Dispatcher.Invoke(new Action(() =>
            {
                this.Close();
            }));
        }

        private void W_Loaded(object sender, RoutedEventArgs e)
        {
            provider.RestauraDatabase();
        }

        private void Grid_MouseLeftButtonDown_1(object sender, MouseButtonEventArgs e)
        {
            if (Mouse.LeftButton == MouseButtonState.Pressed)
                this.DragMove();
        }
    }
}
