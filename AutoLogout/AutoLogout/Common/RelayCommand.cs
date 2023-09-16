using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace AutoLogout.Common
{
    public class RelayCommand : ICommand
    {
        private readonly Action _execute; 
        private readonly Func<bool> _canExecute; 
        public RelayCommand(Action execute, Func<bool> canExecute = null)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public bool CanExecute(object parameter)
        {
            return _canExecute == null || _canExecute();
        }

        public void Execute(object parameter)
        {
            _execute();
        }


        //private readonly Action<object> _execute2;
        //private readonly Func<object, bool> _canExecute2;
        //public RelayCommand(Action<object> execute2, Func<object, bool> canExecute = null)
        //{
        //    _execute2 = execute2 ?? throw new ArgumentNullException(nameof(execute2));
        //    _canExecute2 = canExecute;
        //}
    }

    ///// <summary>
    ///// Internal relay command used for Command binding.
    ///// </summary>
    //internal class RelayCommand : ICommand
    //{
    //    private readonly Func<object?, bool> _canExecute;
    //    private readonly Action<object?> _execute;
    //    private bool? _prevCanExecute;

    //    public RelayCommand(Func<object?, bool>? canExecute = null, Action<object?>? execute = null)
    //    {
    //        _canExecute = canExecute ?? (_ => true);
    //        _execute = execute ?? (_ => { });
    //    }

    //    public event EventHandler? CanExecuteChanged;

    //    public bool CanExecute(object? parameter)
    //    {
    //        var ce = _canExecute(parameter);
    //        if (CanExecuteChanged is not null && (!_prevCanExecute.HasValue || ce != _prevCanExecute))
    //        {
    //            CanExecuteChanged(this, EventArgs.Empty);
    //            _prevCanExecute = ce;
    //        }

    //        return ce;
    //    }

    //    public void Execute(object? parameter) => _execute(parameter);
    //}
}
