﻿#pragma checksum "..\..\..\..\LayoutWindows\LayoutPainelW\AlteracaoSenha.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "590246649CCDA53E9614DC77E99137F7"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using SIF.Aplicacao.UserControls;
using SIF.WPF.Styles.Presentation;
using SIF.WPF.Styles.Windows;
using SIF.WPF.Styles.Windows.Controls;
using SIF.WPF.Styles.Windows.Converters;
using SIF.WPF.Styles.Windows.Navigation;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Media.TextFormatting;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Shell;


namespace SIF.Aplicacao.LayoutConfiguracaoW {
    
    
    /// <summary>
    /// AlteracaoSenha
    /// </summary>
    public partial class AlteracaoSenha : SIF.WPF.Styles.Windows.Controls.ModernWindow, System.Windows.Markup.IComponentConnector {
        
        
        #line 26 "..\..\..\..\LayoutWindows\LayoutPainelW\AlteracaoSenha.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Border imgFoto;
        
        #line default
        #line hidden
        
        
        #line 45 "..\..\..\..\LayoutWindows\LayoutPainelW\AlteracaoSenha.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.PasswordBox pwdSenhaAtual;
        
        #line default
        #line hidden
        
        
        #line 47 "..\..\..\..\LayoutWindows\LayoutPainelW\AlteracaoSenha.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.PasswordBox pwdNovaSenha;
        
        #line default
        #line hidden
        
        
        #line 49 "..\..\..\..\LayoutWindows\LayoutPainelW\AlteracaoSenha.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.PasswordBox pwdNovaSenhaConf;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/SIF - Sistema Financeiro;component/layoutwindows/layoutpainelw/alteracaosenha.xa" +
                    "ml", System.UriKind.Relative);
            
            #line 1 "..\..\..\..\LayoutWindows\LayoutPainelW\AlteracaoSenha.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            this.imgFoto = ((System.Windows.Controls.Border)(target));
            return;
            case 2:
            this.pwdSenhaAtual = ((System.Windows.Controls.PasswordBox)(target));
            return;
            case 3:
            this.pwdNovaSenha = ((System.Windows.Controls.PasswordBox)(target));
            return;
            case 4:
            this.pwdNovaSenhaConf = ((System.Windows.Controls.PasswordBox)(target));
            return;
            case 5:
            
            #line 52 "..\..\..\..\LayoutWindows\LayoutPainelW\AlteracaoSenha.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.SalvarSenha);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

