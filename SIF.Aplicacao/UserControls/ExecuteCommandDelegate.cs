using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SIF.Aplicacao.UserControls
{
    public delegate void DelegateExecuteButton(ButtonType button);

    public class ExecuteCommandDelegate : ICommand
    {
        private readonly DelegateExecuteButton _actionExecute;
        private readonly ButtonType _button;

        public ExecuteCommandDelegate(DelegateExecuteButton actionExecute, ButtonType button)
        {
            this._actionExecute = actionExecute;
            this._button = button;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public void Execute(object parameter)
        {
            _actionExecute(_button);
        }
    }
}
