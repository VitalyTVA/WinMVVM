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
        PropertyDescriptor Property { get { return Key.property; } }
        public BindingBase Binding { get; private set; }
        PropertyChangeListener listener;
        readonly PropertyEntry<object> propertyEntry;
        public BindingExpression(BindingExpressionKey key, BindingBase binding, PropertyEntry<object> propertyEntry) {
            this.propertyEntry = propertyEntry;
            Binding = binding;
            this.Key = key;
            WpfBinding wpfBinding = new WpfBinding("PropertyValue." + ((Binding)binding).Path) { Source = propertyEntry };

            if(Property == null)
                Guard.ArgumentException("propertyName");
            listener = PropertyChangeListener.Create(wpfBinding, UpdateTargetProperty, Property.GetValue(Control));
        }

        void UpdateTargetProperty(object value) {
            if(Property == null)
                Guard.ArgumentException("propertyName");
            if(propertyEntry.IsValueSet) {
                Property.SetValue(Control, value);
            } else {
                ClearTargetProperty();
            }
        }
        void ClearTargetProperty() {
            //TODO  CanResetValue
            Property.ResetValue(Control);
        }

        internal void Clear() {
            //if(listener != null)
            listener.Clear();
        }
    }
}
