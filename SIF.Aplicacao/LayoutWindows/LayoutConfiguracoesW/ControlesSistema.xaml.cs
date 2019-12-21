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

namespace SIF.Aplicacao.LayoutWindows.LayoutConfiguracoesW
{
    /// <summary>
    /// Interaction logic for ConfigFinanceiro.xaml
    /// </summary>
    public partial class ControlesSistema : UserControl
    {
        public CONSULTAS_SELECTED SelectedConsul
        {
            get { return (CONSULTAS_SELECTED)GetValue(SelectedConsulProperty); }
            set { SetValue(SelectedConsulProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SelectedConsul.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SelectedConsulProperty =
            DependencyProperty.Register("SelectedConsul", typeof(CONSULTAS_SELECTED), typeof(ControlesSistema), new PropertyMetadata(new PropertyChangedCallback(OnSelectedPropertyChangedCallback)));

        private static void OnSelectedPropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            CONSULTAS_SELECTED value = (CONSULTAS_SELECTED)e.NewValue;
            switch (value)
            {
                case CONSULTAS_SELECTED.COM_VIEW_EM_EXECUCAO:
                    SistemaGlobal.Sis.ConsultaType = CONSULTAS_SELECTED.COM_VIEW_EM_EXECUCAO;
                    break;
                case CONSULTAS_SELECTED.COM_VIEW_E_SP:                    
                    SistemaGlobal.Sis.ConsultaType = CONSULTAS_SELECTED.COM_VIEW_E_SP;
                    break;
                default:
                    SistemaGlobal.Sis.ConsultaType = CONSULTAS_SELECTED.COM_SELECT_QUERY_PURO;
                    break;
            }
        }

        public ControlesSistema()
        {
            InitializeComponent();

            this.DataContext = this;

            SelectedConsul = SistemaGlobal.Sis.ConsultaType;
        }
    }
}
