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
            var propertyEntry = DataContextProvider.DataContextProperty.GetPropertyEntry(control);
            propertyEntry.AddBinding(key, binding, () => new BindingExpression(key, binding, propertyEntry));
        }

        public static void ClearBinding(this Control control, string propertyName) {
            BindingExpressionKey key = new BindingExpressionKey(control, propertyName);

            var propertyEntry = DataContextProvider.DataContextProperty.GetPropertyEntry(control);
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

        internal static PropertyDescriptor GetProperty(Control control, string propertyName) {
            return TypeDescriptor.GetProperties(control)[propertyName];
        }

    }
}
