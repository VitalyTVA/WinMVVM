using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Forms;
using WpfBinding = System.Windows.Data.Binding;

namespace WinMVVM.Utils {
    class BindingExpression {
        public BindingExpressionKey Key { get; private set; }
        Control Control { get { return Key.Control; } }
        string PropertyName { get { return Key.propertyName; } }
        public BindingBase Binding { get; private set; }
        PropertyChangeListener listener;
        public BindingExpression(BindingExpressionKey key, BindingBase binding, PropertyEntry propertyEntry) {
            Binding = binding;
            this.Key = key;
            WpfBinding wpfBinding = new WpfBinding("PropertyValue." + ((Binding)binding).Path) { Source = propertyEntry };
            listener = PropertyChangeListener.Create(wpfBinding, new Action<object>(UpdateTargetProperty));
        }

        void UpdateTargetProperty(object value) {
            PropertyDescriptor property = BindingOperations.GetProperty(Control, PropertyName);
            if(property == null)
                Guard.ArgumentException("propertyName");
            if(!PropertyEntry.IsNotSetValue(value)) {
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
}
