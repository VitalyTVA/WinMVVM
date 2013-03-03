using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Input;

namespace WinMVVM.Tests {
    public partial class DelegateCommand<T> : ICommand {
        private readonly Action<T> executeMethod = null;
        private readonly Func<T, bool> canExecuteMethod = null;

        public DelegateCommand(Action<T> executeMethod)
            : this(executeMethod, null) {
        }

        public DelegateCommand(Action<T> executeMethod, Func<T, bool> canExecuteMethod) {
            if(executeMethod == null && canExecuteMethod == null)
                throw new ArgumentNullException("executeMethod");

            this.executeMethod = executeMethod;
            this.canExecuteMethod = canExecuteMethod;
        }

        public bool CanExecute(T parameter) {
            if(canExecuteMethod == null) return true;
            return canExecuteMethod(parameter);
        }

        public void Execute(T parameter) {
            if(executeMethod == null) return;
            executeMethod(parameter);
        }

        bool ICommand.CanExecute(object parameter) {
            T typedParameter;
            try {
                typedParameter = parameter == null ? default(T) : (T)parameter;
            } catch {
                typedParameter = default(T);
            }
            return CanExecute(typedParameter);
        }

        public event EventHandler CanExecuteChanged;
        void ICommand.Execute(object parameter) {
            Execute((T)parameter);
        }

        protected virtual void OnCanExecuteChanged() {
            if(CanExecuteChanged != null)
                CanExecuteChanged(this, EventArgs.Empty);
        }

        public void RaiseCanExecuteChanged() {
            OnCanExecuteChanged();
        }
    }
    public class DelegateCommand : DelegateCommand<object> {
        public DelegateCommand(Action executeMethod)
            : base(o => executeMethod()) {
        }

        public DelegateCommand(Action executeMethod, Func<bool> canExecuteMethod)
            : base(o => executeMethod(), o => canExecuteMethod()) {
        }
    }
}
