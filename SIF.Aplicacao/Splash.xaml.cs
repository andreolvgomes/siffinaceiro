using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace SIF.Aplicacao
{
    public partial class Splash : Window
    {
        public string Mensagem
        {
            get { return (string)GetValue(MensagemProperty); }
            set { SetValue(MensagemProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Mensagem.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MensagemProperty =
            DependencyProperty.Register("Mensagem", typeof(string), typeof(Splash), new PropertyMetadata("Aguarde ..."));


        private System.Windows.Media.Effects.Effect EffectOrig;

        public Splash(Window owner, string mensagem)
        {
            InitializeComponent();

            if (owner != null)
            {
                this.Owner = owner;
                this.EffectOrig = owner.Effect;
                this.Owner.Effect = new BlurEffect() { Radius = 3, KernelType = KernelType.Gaussian };                
            }

            this.Mensagem = mensagem;
            this.DataContext = this;

            this.Closing += new System.ComponentModel.CancelEventHandler(W_Closing);
        }

        private void W_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (this.Owner != null)
                this.Owner.Effect = EffectOrig;
        }
        
        public void Executa(Action action)
        {
            Thread syn = new Thread(new ParameterizedThreadStart(ExecutaSync));
            syn.Start(action);
            this.ShowDialog();
        }

        private void ExecutaSync(object obj)
        {
            Action ac = obj as Action;
            ac();

            this.Dispatcher.BeginInvoke(new Action(
                () =>
                {
                    this.Close();
                }));
        }

        internal void CloseSync()
        {
            if (this.Dispatcher.Thread != System.Threading.Thread.CurrentThread)
            {
                this.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Send, (System.Threading.SendOrPostCallback)delegate { CloseSync(); }, null);
            }
            else
            {
                this.Close();
            }
        }        
    }
}
