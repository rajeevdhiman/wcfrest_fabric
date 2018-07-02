using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace Reader.ViewModels.Base
{
    public class RelayCommand : ICommand
    {
        private Action methodToExecute;
        private Func<bool> canExecuteEvaluator;

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public RelayCommand(Action methodToExecute, Func<bool> canExecuteEvaluator)
        {
            this.methodToExecute = methodToExecute;
            this.canExecuteEvaluator = canExecuteEvaluator;
        }

        public RelayCommand(Action methodToExecute)
            : this(methodToExecute, null)
        {
        }

        public bool CanExecute(object parameter)
        {
            if (this.canExecuteEvaluator == null) return true;
            return this.canExecuteEvaluator.Invoke();
        }

        public void Execute(object parameter)
        {
            this.methodToExecute.Invoke();
        }
    }

    public class RelayCommand<T> : ICommand
    {
        private Action<T> methodToExecute;
        private Func<T, bool> canExecuteEvaluator;

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public RelayCommand(Action<T> methodToExecute, Func<T, bool> canExecuteEvaluator)
        {
            this.methodToExecute = methodToExecute;
            this.canExecuteEvaluator = canExecuteEvaluator;
        }

        public RelayCommand(Action<T> methodToExecute)
            : this(methodToExecute, null)
        {
        }

        private bool CanExecute(T parameter)
        {
            if (this.canExecuteEvaluator == null) return true;

            return this.canExecuteEvaluator.Invoke(parameter);
        }

        private void Execute(T parameter)
        {
            this.methodToExecute.Invoke(parameter);
        }

        public bool CanExecute(object parameter)
        {
            return this.CanExecute((T)parameter);
        }

        public void Execute(object parameter)
        {
            this.Execute((T)parameter);
        }
    }
}