using SIF.WPF.Styles.Presentation;
using SIF.WPF.Styles.Windows.Controls;
using SIF.Commom;
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
using SIF.Aplicacao.ManagerWindow;

namespace SIF.Aplicacao
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class Menu : ModernWindow
    {
        /// <summary>
        /// Window principal
        /// </summary>
        public Menu()
        {            
            InitializeComponent();

            this.Closing += new System.ComponentModel.CancelEventHandler(W_Closing);
            this.Loaded += new RoutedEventHandler(W_Loaded);
            this.DataContext = SistemaGlobal.Sis.ManagerWindow;
        }

        private void W_Loaded(object sender, RoutedEventArgs e)
        {
            //using (ProviderBancodados restore = new ProviderBancodados())
            //{
            //    if (restore.ConfigRestoreAuto())
            //    {
            //        System.Timers.Timer timer = new System.Timers.Timer(125);
            //        timer.Elapsed += new System.Timers.ElapsedEventHandler(
            //            (object senderT, System.Timers.ElapsedEventArgs eT) =>
            //            {
            //                timer.Enabled = false;
            //                restore.RestoreDatabaseAutomatico(this);
            //            });
            //        timer.Enabled = true;
            //    }
            //}
        }

        private void W_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (System.Windows.Application.Current != null)
                System.Windows.Application.Current.Shutdown();
        }
    }
}
