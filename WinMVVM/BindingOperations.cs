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
            PropertyDescriptor property = ValidateParameters(control, propertyName);
            control.SetBindingCore(property, binding);
        }
        static void SetBindingCore(this Control control, PropertyDescriptor property, BindingBase binding) {
            Guard.ArgumentNotNull(binding, "binding");

            BindingExpressionKey key = new BindingExpressionKey(control, property);
            var propertyEntry = DataContextProvider.DataContextProperty.GetPropertyEntry(control);
            propertyEntry.AddBinding(key, binding, () => new BindingExpression(key, binding, propertyEntry));
        }

        public static void ClearBinding(this Control control, string propertyName) {
            PropertyDescriptor property = ValidateParameters(control, propertyName);
            control.ClearBindingCore(property);
        }
        public static void ClearBinding<T>(this Control control, Expression<Func<T>> expression) {
            control.ClearBinding(ExpressionHelper.GetPropertyName(expression));
        }
        static void ClearBindingCore(this Control control, PropertyDescriptor property) {
            BindingExpressionKey key = new BindingExpressionKey(control, property);

            var propertyEntry = DataContextProvider.DataContextProperty.GetPropertyEntry(control);
            BindingExpression expression = propertyEntry.RemoveBinding(key);
            if(expression != null) {
                expression.Clear();
            }
        }
        //public static void ClearAllBinding(this Control control, string propertyName) { //TODO
        //}

        static PropertyDescriptor GetProperty(Control control, string propertyName) {
            return TypeDescriptor.GetProperties(control)[propertyName];
        }
        static PropertyDescriptor ValidateParameters(Control control, string propertyName) {
            Guard.ArgumentNotNull(control, "control");
            Guard.ArgumentInRange(!string.IsNullOrEmpty(propertyName), "propertyName");
            PropertyDescriptor property = GetProperty(control, propertyName);
            if(property == null)
                Guard.ArgumentException("propertyName");
            return property;
        }
    }
}
