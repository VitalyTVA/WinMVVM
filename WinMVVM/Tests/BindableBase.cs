using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Linq.Expressions;
using WinMVVM.Utils;

namespace WinMVVM.Tests {
    public abstract class BindableBase : INotifyPropertyChanged {
        public event PropertyChangedEventHandler PropertyChanged;
        protected bool SetProperty<T>(ref T storage, T value, string propertyName, Action changedCallback) {
            if(object.Equals(storage, value)) return false;
            T oldValue = storage;
            storage = value;
            if(changedCallback != null)
                changedCallback();
            RaisePropertyChanged(propertyName);
            return true;
        }
        protected bool SetProperty<T>(ref T storage, T value, string propertyName) {
            return SetProperty<T>(ref storage, value, propertyName, null);
        }
        protected void RaisePropertyChanged(string propertyName) {
            if(PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
        protected void RaisePropertyChanged() {
            RaisePropertiesChanged(null);
        }
        protected void RaisePropertyChanged<T>(Expression<Func<T>> expression) {
            RaisePropertyChanged(ExpressionHelper.GetPropertyName(expression));
        }
        protected void RaisePropertiesChanged(params string[] propertyNames) {
            foreach(string propertyName in propertyNames) {
                RaisePropertyChanged(propertyName);
            }
        }
        protected bool SetProperty<T>(ref T storage, T value, Expression<Func<T>> expression, Action changedCallback) {
            string propertyName = ExpressionHelper.GetPropertyName<T>(expression);
            return SetProperty(ref storage, value, propertyName, changedCallback);
        }
        protected bool SetProperty<T>(ref T storage, T value, Expression<Func<T>> expression) {
            return SetProperty<T>(ref storage, value, expression, null);
        }
    }
}