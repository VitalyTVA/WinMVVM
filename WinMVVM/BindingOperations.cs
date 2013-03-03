using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Windows;
using System.Windows.Forms;
using WinMVVM.Utils;
using WpfBinding = System.Windows.Data.Binding;

namespace WinMVVM {
    public static class BindingOperations {
        internal class BindingExpressionKey {
            //TODO clear expressions with dead references
            readonly WeakReference controlReference;
            public Control Control { get { return (Control)controlReference.Target; } }
            public readonly string propertyName;
            public BindingExpressionKey(Control control, string propertyName) {
                this.propertyName = propertyName;
                this.controlReference = new WeakReference(control);

            }
            //TODO - check wheter target is alive
            public override int GetHashCode() {
                return Control.GetHashCode() ^ propertyName.GetHashCode();
            }
            public override bool Equals(object obj) {
                var other = obj as BindingExpressionKey;
                return other != null && other.Control == Control && other.propertyName == propertyName;
            }
        }
        internal class BindingExpression {
            public BindingExpressionKey Key { get; private set; }
            Control Control { get { return Key.Control; } }
            string PropertyName { get { return Key.propertyName; } }
            public BindingBase Binding { get; private set; }
            PropertyChangeListener listener;
            public BindingExpression(BindingExpressionKey key, BindingBase binding, DataContextProvider.PropertyEntry propertyEntry) {
                Binding = binding;
                this.Key = key;
                WpfBinding wpfBinding = new WpfBinding("PropertyValue." + ((Binding)binding).Path) { Source = propertyEntry };
                listener = PropertyChangeListener.Create(wpfBinding, new Action<object>(UpdateTargetProperty));
            }

            void UpdateTargetProperty(object value) {
                PropertyDescriptor property = GetProperty(Control, PropertyName);
                if(property == null)
                    Guard.ArgumentException("propertyName");
                if(!DataContextProvider.PropertyEntry.IsNotSetValue(value)) {
                    property.SetValue(Control, value);
                } else {
                    ClearTargetProperty(property);
                }
            }
            void ClearTargetProperty(PropertyDescriptor property) {
                //TODO  CanResetValue
                property.ResetValue(Control);
            }

            internal void Clear() {
                //if(listener != null)
                    listener.Clear();
            }
        }
        public static void SetBinding<T>(this Control control, Expression<Func<T>> expression, BindingBase binding) {
            control.SetBinding(ExpressionHelper.GetPropertyName(expression), binding);
        }
        public static void SetBinding(this Control control, string propertyName, BindingBase binding) {
            Guard.ArgumentNotNull(control, "control");
            Guard.ArgumentInRange(!string.IsNullOrEmpty(propertyName), "propertyName");
            Guard.ArgumentNotNull(binding, "binding");
            if(GetProperty(control, propertyName) == null)
                Guard.ArgumentException("propertyName");

            BindingExpressionKey key = new BindingExpressionKey(control, propertyName);
            var propertyEntry = control.GetPropertyEntry();
            propertyEntry.AddBinding(key, binding);
        }

        public static void ClearBinding(this Control control, string propertyName) {
            BindingExpressionKey key = new BindingExpressionKey(control, propertyName);

            var propertyEntry = control.GetPropertyEntry();
            BindingExpression expression = propertyEntry.RemoveBinding(key);
            if(expression != null) {
                expression.Clear();
            }
        }
        public static void ClearBinding<T>(this Control control, Expression<Func<T>> expression) {
            control.ClearBinding(ExpressionHelper.GetPropertyName(expression));
        }
        //public static void ClearAllBinding(this Control control, string propertyName) {
        //}

        static PropertyDescriptor GetProperty(Control control, string propertyName) {
            return TypeDescriptor.GetProperties(control)[propertyName];
        }

    }
}
