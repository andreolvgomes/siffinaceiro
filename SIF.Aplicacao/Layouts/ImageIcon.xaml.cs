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

namespace SIF.Aplicacao.Layouts
{
    /// <summary>
    /// Interaction logic for ImageIcon.xaml
    /// </summary>
    public partial class ImageIcon : UserControl
    {
        //public ImageSource IconSource
        //{
        //    get { return (ImageSource)GetValue(IconSourceProperty); }
        //    set { SetValue(IconSourceProperty, value); }
        //}

        //// Using a DependencyProperty as the backing store for IconSource.  This enables animation, styling, binding, etc...
        //public static readonly DependencyProperty IconSourceProperty =
        //    DependencyProperty.Register("IconSource", typeof(ImageSource), typeof(ImageIcon), new PropertyMetadata(new BitmapImage(new Uri("/Icones/default.png", UriKind.Relative))));




        public ImageSource IconSource
        {
            get { return (ImageSource)GetValue(IconSourceProperty); }
            set { SetValue(IconSourceProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IconSource.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IconSourceProperty =
            DependencyProperty.Register("IconSource", typeof(ImageSource), typeof(ImageIcon));

        

        

        public string Titulo
        {
            get { return (string)GetValue(TituloProperty); }
            set { SetValue(TituloProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Titulo.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TituloProperty =
            DependencyProperty.Register("Titulo", typeof(string), typeof(ImageIcon), new PropertyMetadata("icon default"));




        public string ToolTipDescricao
        {
            get { return (string)GetValue(ToolTipDescricaoProperty); }
            set { SetValue(ToolTipDescricaoProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ToolTipDescricao.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ToolTipDescricaoProperty =
            DependencyProperty.Register("ToolTipDescricao", typeof(string), typeof(ImageIcon));


        public ImageIcon()
        {
            InitializeComponent();

            //    DependencyProperty.Register("IconSource", typeof(ImageSource), typeof(ImageIcon), new PropertyMetadata(new BitmapImage(new Uri("pack://application:,,,/SIFAplicacao;component/Icones/default.png"))));

            this.DataContext = this;
        }
    }
}
