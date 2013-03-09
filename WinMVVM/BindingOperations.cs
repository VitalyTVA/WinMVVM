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
        public static void SetBinding<T>(this Control control, AttachedProperty<T> property, BindingBase binding) {
            ValidateAttachedPropertyParameters<T>(control, property);
            Guard.ArgumentNotNull(binding, "binding");
            control.SetBindingCore(AttachedPropertyDescriptor.FromAttachedProperty(property), binding);
        }
        public static void SetBinding<T>(this Control control, Expression<Func<T>> expression, BindingBase binding) {
            control.SetBinding(ExpressionHelper.GetPropertyName(expression), binding);
        }
        public static void SetBinding(this Control control, string propertyName, BindingBase binding) {
            PropertyDescriptorBase property = ValidatePropertyNameParameters(control, propertyName);
            control.SetBindingCore(property, binding);
        }
        static void SetBindingCore(this Control control, PropertyDescriptorBase property, BindingBase binding) {
            Guard.ArgumentNotNull(binding, "binding");

            BindingExpressionKey key = new BindingExpressionKey(control, property);
            var propertyEntry = DataContextProvider.DataContextProperty.GetPropertyEntry(control);
            propertyEntry.AddBinding(key, binding, () => new BindingExpression(key, binding, propertyEntry));
        }
        public static void ClearBinding<T>(this Control control, AttachedProperty<T> property) {
            ValidateAttachedPropertyParameters<T>(control, property);
            control.ClearBindingCore(AttachedPropertyDescriptor.FromAttachedProperty(property));
        }
        public static void ClearBinding(this Control control, string propertyName) {
            PropertyDescriptorBase property = ValidatePropertyNameParameters(control, propertyName);
            control.ClearBindingCore(property);
        }
        public static void ClearBinding<T>(this Control control, Expression<Func<T>> expression) {
            control.ClearBinding(ExpressionHelper.GetPropertyName(expression));
        }
        internal static void ClearBindingCore(this Control control, PropertyDescriptorBase property) {
            BindingExpressionKey key = new BindingExpressionKey(control, property);

            var propertyEntry = DataContextProvider.DataContextProperty.GetPropertyEntry(control);
            BindingExpression expression = propertyEntry.RemoveBinding(key);
            if(expression != null) {
                expression.Clear();
            }
        }
        //public static void ClearAllBinding(this Control control, string propertyName) { //TODO
        //}

        static PropertyDescriptorBase ValidatePropertyNameParameters(Control control, string propertyName) {
            Guard.ArgumentNotNull(control, "control");
            Guard.ArgumentInRange(!string.IsNullOrEmpty(propertyName), "propertyName");
            PropertyDescriptorBase property = StandardPropertyDescriptor.FromPropertyName(control, propertyName);
            return property;
        }
        private static void ValidateAttachedPropertyParameters<T>(Control control, AttachedProperty<T> property) {
            Guard.ArgumentNotNull(control, "control");
            Guard.ArgumentNotNull(property, "property");
        }
    }
}
