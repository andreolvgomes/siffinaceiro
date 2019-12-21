using SIF.WPF.Styles.Presentation;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Media;

namespace SIF.Aplicacao.Layouts
{
    /// <summary>
    /// A simple view model for configuring theme, font and accent colors.
    /// </summary>
    public class SettingsAppearanceViewModel : NotifyPropertyChanged
    {
        private const string FontSmall = "small";
        private const string FontLarge = "large";

        private const string PaletteMetro = "metro";
        private const string PaletteWP = "windows phone";

        // 20 accent colors from Windows Phone 8
        private Color[] wpAccentColors = new Color[]{
            Color.FromRgb(0xa4, 0xc4, 0x00),   // lime
            Color.FromRgb(0x60, 0xa9, 0x17),   // green
            Color.FromRgb(0x00, 0x8a, 0x00),   // emerald
            Color.FromRgb(0x00, 0xab, 0xa9),   // teal
            Color.FromRgb(0x1b, 0xa1, 0xe2),   // cyan
            Color.FromRgb(0x00, 0x50, 0xef),   // cobalt
            Color.FromRgb(0x6a, 0x00, 0xff),   // indigo
            Color.FromRgb(0xaa, 0x00, 0xff),   // violet
            Color.FromRgb(0xf4, 0x72, 0xd0),   // pink
            Color.FromRgb(0xd8, 0x00, 0x73),   // magenta
            Color.FromRgb(0xa2, 0x00, 0x25),   // crimson
            Color.FromRgb(0xe5, 0x14, 0x00),   // red
            Color.FromRgb(0xfa, 0x68, 0x00),   // orange
            Color.FromRgb(0xf0, 0xa3, 0x0a),   // amber
            Color.FromRgb(0xe3, 0xc8, 0x00),   // yellow
            Color.FromRgb(0x82, 0x5a, 0x2c),   // brown
            Color.FromRgb(0x6d, 0x87, 0x64),   // olive
            Color.FromRgb(0x64, 0x76, 0x87),   // steel
            Color.FromRgb(0x76, 0x60, 0x8a),   // mauve
            Color.FromRgb(0x87, 0x79, 0x4e),   // taupe
            Color.FromRgb(0x33, 0x99, 0xff),   // blue
            Color.FromRgb(0x00, 0xab, 0xa9),   // teal
            Color.FromRgb(0x33, 0x99, 0x33),   // green
            Color.FromRgb(0x8c, 0xbf, 0x26),   // lime
            Color.FromRgb(0xf0, 0x96, 0x09),   // orange
            Color.FromRgb(0xff, 0x45, 0x00),   // orange red
            Color.FromRgb(0xe5, 0x14, 0x00),   // red
            Color.FromRgb(0xff, 0x00, 0x97),   // magenta
            Color.FromRgb(0xa2, 0x00, 0xff),   // purple  
        };

        private Color selectedAccentColor;
        private LinkCollection themes = new LinkCollection();
        //private Link selectedTheme;
        private string selectedFontSize;

        public SettingsAppearanceViewModel()
        {
            //// add the default themes
            //this.themes.Add(new Link { DisplayName = "dark", Source = AppearanceManager.DarkThemeSource });
            //this.themes.Add(new Link { DisplayName = "light", Source = AppearanceManager.LightThemeSource });

            //// add additional themes
            //this.themes.Add(new Link { DisplayName = "bing image", Source = new Uri("/SIFAplSIF.Aplicacaoicacao;component/Fundo/ModernUI.BingImage.xaml", UriKind.Relative) });
            //this.themes.Add(new Link { DisplayName = "hello kitty", Source = new Uri("/SIF.Aplicacao;component/Fundo/ModernUI.HelloKitty.xaml", UriKind.Relative) });
            //this.themes.Add(new Link { DisplayName = "love", Source = new Uri("/SIF.Aplicacao;component/Fundo/ModernUI.Love.xaml", UriKind.Relative) });
            //this.themes.Add(new Link { DisplayName = "snowflakes", Source = new Uri("/SIF.Aplicacao;component/Fundo/ModernUI.Snowflakes.xaml", UriKind.Relative) });

            //this.SelectedFontSize = AppearanceManager.Current.FontSize == FontSize.Large ? FontLarge : FontSmall;
            SyncThemeAndColor();

            AppearanceManager.Current.PropertyChanged += OnAppearanceManagerPropertyChanged;
        }

        private void SyncThemeAndColor()
        {
            // synchronizes the selected viewmodel theme with the actual theme used by the appearance manager.
            //this.SelectedTheme = this.themes.FirstOrDefault(l => l.Source.Equals(AppearanceManager.Current.ThemeSource));

            // and make sure accent color is up-to-date
            //this.SelectedAccentColor = AppearanceManager.Current.AccentColor;

            this.SelectedAccentColor = (Color)System.Windows.Media.ColorConverter.ConvertFromString(Properties.Settings.Default.Cor);
            this.SelectedFontSize = Properties.Settings.Default.Fonte;
        }

        private void OnAppearanceManagerPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "ThemeSource" || e.PropertyName == "AccentColor")
            {
                SyncThemeAndColor();
            }
        }

        public LinkCollection Themes
        {
            get { return this.themes; }
        }

        public string[] FontSizes
        {
            get { return new string[] { FontSmall, FontLarge }; }
        }

        public string[] Palettes
        {
            get { return new string[] { PaletteMetro, PaletteWP }; }
        }

        //private Color[] cores()
        //{
        //    Type colorsType = typeof(System.Windows.Media.Colors);
        //    PropertyInfo[] colorsTypePropertyInfos = colorsType.GetProperties(BindingFlags.Public | BindingFlags.Static);

        //    foreach (PropertyInfo colorsTypePropertyInfo in colorsTypePropertyInfos)
        //         Debug.WriteLine(colorsTypePropertyInfo.Name);
        //}

        //public Color[] AccentColors
        //{
        //    get 
        //    { 
        //        return this.wpAccentColors;
        //    }
        //}

        public List<Color> AccentColors
        {
            get
            {
                List<Color> ls = new List<Color>();

                Type colorsType = typeof(System.Windows.Media.Colors);
                PropertyInfo[] colores = colorsType.GetProperties(BindingFlags.Public | BindingFlags.Static);
                foreach (Color cor in wpAccentColors)
                    ls.Add(cor);
                foreach (PropertyInfo info in colores)
                    ls.Add((Color)System.Windows.Media.ColorConverter.ConvertFromString(info.Name));
                return ls;
            }
        }

        //public string SelectedPalette
        //{
        //    get { return this.selectedPalette; }
        //    set
        //    {
        //        if (this.selectedPalette != value)
        //        {
        //            this.selectedPalette = value;
        //            OnPropertyChanged("AccentColors");

        //            this.SelectedAccentColor = this.AccentColors.FirstOrDefault();
        //        }
        //    }
        //}

        //public Link SelectedTheme
        //{
        //    get { return this.selectedTheme; }
        //    set
        //    {
        //        if (this.selectedTheme != value)
        //        {
        //            this.selectedTheme = value;
        //            OnPropertyChanged("SelectedTheme");

        //            // and update the actual theme
        //            AppearanceManager.Current.ThemeSource = value.Source;
        //        }
        //    }
        //}

        public string SelectedFontSize
        {
            get { return this.selectedFontSize; }
            set
            {
                if (this.selectedFontSize != value)
                {
                    this.selectedFontSize = value;
                    OnPropertyChanged("SelectedFontSize");
                    
                    /// salva na máquina
                    /// 
                    Properties.Settings.Default.Fonte = value;
                    Properties.Settings.Default.Save();

                    AppearanceManager.Current.FontSize = value == FontLarge ? FontSize.Large : FontSize.Small;
                }
            }
        }

        public Color SelectedAccentColor
        {
            get { return this.selectedAccentColor; }
            set
            {
                if (this.selectedAccentColor != value)
                {
                    this.selectedAccentColor = value;
                    OnPropertyChanged("SelectedAccentColor");
                    
                    ///salva na máquina
                    ///
                    Properties.Settings.Default.Cor = value.ToString();
                    Properties.Settings.Default.Save();

                    AppearanceManager.Current.AccentColor = value;
                }
            }
        }
    }
}
