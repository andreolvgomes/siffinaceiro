using SIF.WPF.Styles.Presentation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Effects;

namespace SIF.WPF.Styles.Windows.Controls
{
    /// <summary>
    /// Represents a Modern UI styled dialog window.
    /// </summary>
    public class ModernDialog : DpiAwareWindow
    {
        /// <summary>
        /// Identifies the BackgroundContent dependency property.
        /// </summary>
        public static readonly DependencyProperty BackgroundContentProperty = DependencyProperty.Register("BackgroundContent", typeof(object), typeof(ModernDialog));
        /// <summary>
        /// Identifies the Buttons dependency property.
        /// </summary>
        public static readonly DependencyProperty ButtonsProperty = DependencyProperty.Register("Buttons", typeof(IEnumerable<Button>), typeof(ModernDialog));

        private ICommand closeCommand;

        private Button okButton;
        private Button cancelButton;
        private Button yesButton;
        private Button noButton;
        private Button closeButton;

        private MessageBoxResult messageBoxResult = MessageBoxResult.None;

        /// <summary>
        /// Initializes a new instance of the <see cref="ModernDialog"/> class.
        /// </summary>
        public ModernDialog()
        {
            this.DefaultStyleKey = typeof(ModernDialog);

            this.MinHeight = 200;
            this.MinWidth = 500;
            this.MaxHeight = 480;
            this.MaxWidth = 640;

            this.closeCommand = new RelayCommand(o =>
            {
                var result = o as MessageBoxResult?;
                if (result.HasValue)
                {
                    this.messageBoxResult = result.Value;

                    // sets the Window.DialogResult as well
                    if (result.Value == MessageBoxResult.OK || result.Value == MessageBoxResult.Yes)
                        this.DialogResult = true;
                    else if (result.Value == MessageBoxResult.Cancel || result.Value == MessageBoxResult.No)
                        this.DialogResult = false;
                    else
                        this.DialogResult = null;
                }
                Close();
            });

            this.Buttons = new Button[] { this.CloseButton };

            // set the default owner to the app main window (if possible)
            // remove andré
            //if (Application.Current != null && Application.Current.MainWindow != this)
            //{
            //    this.Owner = Application.Current.MainWindow;
            //}
        }

        private Button CreateCloseDialogButton(string content, bool isDefault, bool isCancel, MessageBoxResult result)
        {
            return new Button
            {
                Content = content,
                Command = this.CloseCommand,
                CommandParameter = result,
                IsDefault = isDefault,
                IsCancel = isCancel,
                MinHeight = 21,
                MinWidth = 120,
                Margin = new Thickness(4, 0, 0, 0)
            };
        }

        /// <summary>
        /// Gets the close window command.
        /// </summary>
        public ICommand CloseCommand
        {
            get { return this.closeCommand; }
        }

        /// <summary>
        /// Gets the Ok button.
        /// </summary>
        public Button OkButton
        {
            get
            {
                if (this.okButton == null)
                {
                    this.okButton = CreateCloseDialogButton(SIF.WPF.Styles.Resources.Ok, true, false, MessageBoxResult.OK);
                }
                return this.okButton;
            }
        }

        /// <summary>
        /// Gets the Cancel button.
        /// </summary>
        public Button CancelButton
        {
            get
            {
                if (this.cancelButton == null)
                {
                    this.cancelButton = CreateCloseDialogButton(SIF.WPF.Styles.Resources.Cancel, false, true, MessageBoxResult.Cancel);
                }
                return this.cancelButton;
            }
        }

        /// <summary>
        /// Gets the Yes button.
        /// </summary>
        public Button YesButton
        {
            get
            {
                if (this.yesButton == null)
                {
                    this.yesButton = CreateCloseDialogButton(SIF.WPF.Styles.Resources.Yes, true, false, MessageBoxResult.Yes);
                }
                return this.yesButton;
            }
        }

        /// <summary>
        /// Gets the No button.
        /// </summary>
        public Button NoButton
        {
            get
            {
                if (this.noButton == null)
                {
                    this.noButton = CreateCloseDialogButton(SIF.WPF.Styles.Resources.No, false, true, MessageBoxResult.No);
                }
                return this.noButton;
            }
        }

        /// <summary>
        /// Gets the Close button.
        /// </summary>
        public Button CloseButton
        {
            get
            {
                if (this.closeButton == null)
                {
                    this.closeButton = CreateCloseDialogButton(SIF.WPF.Styles.Resources.Close, true, false, MessageBoxResult.None);
                }
                return this.closeButton;
            }
        }

        /// <summary>
        /// Gets or sets the background content of this window instance.
        /// </summary>
        public object BackgroundContent
        {
            get { return GetValue(BackgroundContentProperty); }
            set { SetValue(BackgroundContentProperty, value); }
        }

        /// <summary>
        /// Gets or sets the dialog buttons.
        /// </summary>
        public IEnumerable<Button> Buttons
        {
            get { return (IEnumerable<Button>)GetValue(ButtonsProperty); }
            set { SetValue(ButtonsProperty, value); }
        }

        /// <summary>
        /// Gets the message box result.
        /// </summary>
        /// <value>
        /// The message box result.
        /// </value>
        public MessageBoxResult MessageBoxResult
        {
            get { return this.messageBoxResult; }
        }

        private IEnumerable<Button> GetButtons(ModernDialog owner, MessageBoxButton button)
        {
            if (button == MessageBoxButton.OK)
            {
                yield return owner.OkButton;
            }
            else if (button == MessageBoxButton.OKCancel)
            {
                yield return owner.OkButton;
                yield return owner.CancelButton;
            }
            else if (button == MessageBoxButton.YesNo)
            {
                yield return owner.YesButton;
                yield return owner.NoButton;
            }
            else if (button == MessageBoxButton.YesNoCancel)
            {
                yield return owner.YesButton;
                yield return owner.NoButton;
                yield return owner.CancelButton;
            }
        }

        /// <summary>
        /// Mostra msg
        /// </summary>
        /// <param name="mensagem"></param>
        /// <param name="titulo"></param>
        /// <param name="button"></param>
        /// <param name="owner"></param>
        /// <returns></returns>
        public System.Windows.MessageBoxResult ShowResult(string mensagem, string titulo, MessageBoxButton button, Window owner)
        {
            return this.ShowResult(mensagem, titulo, button, owner, MessageBoxImage.None);
        }

        /// <summary>
        /// Mostra msg
        /// </summary>
        /// <param name="mensagem"></param>
        /// <param name="titulo"></param>
        /// <param name="button"></param>
        /// <param name="owner"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public System.Windows.MessageBoxResult ShowResult(string mensagem, string titulo, MessageBoxButton button, Window owner, MessageBoxImage type)
        {
            ModernDialog msg = new ModernDialog();
            msg.Title = titulo;
            msg.Content = new BBCodeBlock { BBCode = mensagem, Margin = new Thickness(0, 0, 0, 8), FontSize = 15 };

            Color color = AppearanceManager.Current.AccentColor;
            switch (type)
            {
                case MessageBoxImage.Asterisk:
                    break;
                case MessageBoxImage.Error:
                    AppearanceManager.Current.AccentColor = (Color)System.Windows.Media.ColorConverter.ConvertFromString("Red");
                    break;
                case MessageBoxImage.Exclamation:
                    break;
                case MessageBoxImage.Question:
                    break;
            }

            if (owner != null)
            {
                msg.WindowStartupLocation = WindowStartupLocation.CenterOwner;
                eff = owner.Effect;
                owner.Effect = new BlurEffect() { Radius = 3, KernelType = KernelType.Gaussian };
                msg.Owner = owner;
            }
            else
            {
                msg.WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
            }

            msg.Buttons = msg.GetButtons(msg, button);
            msg.ShowDialog();
            if (owner != null)
                owner.Effect = eff;

            // volta cor original
            if (AppearanceManager.Current.AccentColor != color)
                AppearanceManager.Current.AccentColor = color;
            return msg.messageBoxResult;
        }

        /// <summary>
        /// 
        /// </summary>
        public System.Windows.Media.Effects.Effect eff { get; set; }
    }
}
