using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SalveTPM1.ViewModel
{
    public class DelegateCommand<T> : ICommand
    {
        private Action<T> Comando;

        public DelegateCommand(Action<T> Comando)
        {
            this.Comando = Comando;
        }
        public bool CanExecute(object parameter)
        {
            //return (parameter != null && parameter.GetType() == typeof(T));
            return true;
        }

        public event EventHandler CanExecuteChanged;

        public void Execute(object parameter)
        {
            if (CanExecute(parameter))
                this.Comando((T)parameter);
        }
    }
}
