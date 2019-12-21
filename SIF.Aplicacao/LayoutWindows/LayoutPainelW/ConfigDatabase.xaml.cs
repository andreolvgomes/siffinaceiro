using SIF.WPF.Styles.Presentation;
using SIF.Aplicacao.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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

namespace SIF.Aplicacao.LayoutWindows.LayoutPainelW
{
    /// <summary>
    /// Interaction logic for ConfigDatabase.xaml
    /// </summary>
    public partial class ConfigDatabase : UserControl, INotifyPropertyChanged
    {
        public bool RestoreAoLogar
        {
            get
            {
                return Settings.Default.RestoreAoLogar;
            }
            set
            {
                if (Settings.Default.RestoreAoLogar != value)
                {
                    Settings.Default.RestoreAoLogar = value;
                    Settings.Default.Save();

                    OnPropertyChanged("RestoreAoLogar");
                }
            }
        }

        public bool RestoreMaisRecente
        {
            get
            {
                return Settings.Default.RestoreFileRecente;
            }
            set
            {
                if (Settings.Default.RestoreFileRecente != value)
                {
                    Settings.Default.RestoreFileRecente = value;
                    Settings.Default.Save();

                    OnPropertyChanged("RestoreMaisRecente");
                }
            }
        }

        public string PathRestoreAoLogar
        {
            get
            {
                return Settings.Default.PathRestoreAoLogar;
            }
            set
            {
                if (Settings.Default.PathRestoreAoLogar != value)
                {
                    Settings.Default.PathRestoreAoLogar = value;
                    Settings.Default.Save();

                    OnPropertyChanged("PathRestoreAoLogar");
                }
            }
        }

        public string PathRestoreFileRecente
        {
            get { return Settings.Default.PathFileRestore; }
            set
            {
                if (Settings.Default.PathFileRestore != value)
                {
                    Settings.Default.PathFileRestore = value;
                    Settings.Default.Save();

                    OnPropertyChanged("PathRestoreFileRecente");
                }
            }
        }

        public ConfigDatabase()
        {
            InitializeComponent();

            this.DataContext = this;
        }

        private void RestoreMaisRecenteCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(Settings.Default.PathFileRestore))
            {
                if(!ConfPathMaisRecente())
                    RestoreMaisRecente = false;
            }
        }

        private bool ConfPathMaisRecente()
        {
            System.Windows.Forms.FolderBrowserDialog fold = new System.Windows.Forms.FolderBrowserDialog();
            fold.SelectedPath = Environment.GetFolderPath(Environment.SpecialFolder.MyComputer);
            if (fold.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                PathRestoreFileRecente = fold.SelectedPath;
                return true;
            }
            return false;
        }

        private void AoLogarCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(Settings.Default.PathRestoreAoLogar))
            {
                if (!PathAoLogar())
                    RestoreAoLogar = false;
            }
        }

        private bool PathAoLogar()
        {
            System.Windows.Forms.FolderBrowserDialog fold = new System.Windows.Forms.FolderBrowserDialog();
            fold.SelectedPath = Environment.GetFolderPath(Environment.SpecialFolder.MyComputer);
            if (fold.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                PathRestoreAoLogar = fold.SelectedPath;
                return true;
            }
            return false;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        private void AoLogar_Click(object sender, RoutedEventArgs e)
        {
            PathAoLogar();
        }

        private void RestoreRecente_Click(object sender, RoutedEventArgs e)
        {
            ConfPathMaisRecente();
        }
    }
}
