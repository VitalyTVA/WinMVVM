using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Windows;
using System.Windows.Forms;
using WinMVVM.Utils;

namespace WinMVVM {
    public static class BindingOperations {
        internal class BindingExpression {
            //TODO clear expressions with dead references
            readonly WeakReference controlReference;
            Control Control { get { return (Control)controlReference.Target; } }
            readonly string propertyName;
            public BindingExpression(Control control, string propertyName) {
                this.propertyName = propertyName;
                this.controlReference = new WeakReference(control);

            }
            public void Initialize() {
                var propertyEntry = Control.GetPropertyEntry();
                if(propertyEntry.AddListener(this)) {
                    UpdateTargetProperty(propertyEntry);
                }
            }
            public void Clear() {
                var propertyEntry = Control.GetPropertyEntry();
                propertyEntry.RemoveListener(this);
                PropertyDescriptor property = GetProperty(Control, propertyName);
                ClearTargetProperty(property);
            }

            public void UpdateTargetProperty(DataContextProvider.PropertyEntry propertyEntry) {
                PropertyDescriptor property = GetProperty(Control, propertyName);
                if(property == null)
                    Guard.ArgumentException("propertyName");
                if(propertyEntry.IsValueSet) {
                    property.SetValue(Control, propertyEntry.PropertyValue);
                } else {
                    ClearTargetProperty(property);
                }
            }
            void ClearTargetProperty(PropertyDescriptor property) {
                //TODO  CanResetValue
                property.ResetValue(Control);
            }
            //TODO - check wheter target is alive
            public override int GetHashCode() {
                return Control.GetHashCode() ^ propertyName.GetHashCode();
            }
            public override bool Equals(object obj) {
                var other = obj as BindingExpression;
                return other != null && other.Control == Control && other.propertyName == propertyName;
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

            BindingExpression bindingExpression = new BindingExpression(control, propertyName);
            bindingExpression.Initialize();
        }

        public static void ClearBinding(this Control control, string propertyName) {
            BindingExpression bindingExpression = new BindingExpression(control, propertyName);
            bindingExpression.Clear();
        }
        //public static void ClearAllBinding(this Control control, string propertyName) {
        //}

        static PropertyDescriptor GetProperty(Control control, string propertyName) {
            return TypeDescriptor.GetProperties(control)[propertyName];
        }

    }
}
