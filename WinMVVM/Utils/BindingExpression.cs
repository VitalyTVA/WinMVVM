using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Forms;
using WpfBinding = System.Windows.Data.Binding;
using WpfBindingMode = System.Windows.Data.BindingMode;

namespace WinMVVM.Utils {
    class BindingExpression {
        public BindingExpressionKey Key { get; private set; }
        Control Control { get { return Key.Control; } }
        PropertyDescriptorBase Property { get { return Key.property; } }
        public BindingBase Binding { get; private set; }
        IPropertyChangeListener listener;
        readonly PropertyEntry<object> propertyEntry;
        bool IsTwoWayBinding { get { return ((Binding)this.Binding).Mode == BindingMode.TwoWay; } }
        public BindingExpression(BindingExpressionKey key, BindingBase binding, PropertyEntry<object> propertyEntry) {
            this.propertyEntry = propertyEntry;
            Binding = binding;
            this.Key = key;
            WpfBindingMode mode = IsTwoWayBinding ? WpfBindingMode.TwoWay : WpfBindingMode.OneWay;
            WpfBinding wpfBinding = new WpfBinding("PropertyValue." + ((Binding)binding).Path) { Source = propertyEntry, Mode = mode};

            if(Property == null)
                Guard.ArgumentException("propertyName");
            MethodInfo createMethod = typeof(PropertyChangeListener<>).MakeGenericType(key.property.PropertyType).GetMethod("Create", BindingFlags.Static | BindingFlags.Public);
            listener = (IPropertyChangeListener)createMethod.Invoke(null, new object[] { wpfBinding, new Action<object>(UpdateTargetProperty), Property.GetValue(Control) });

            if(IsTwoWayBinding)
                Property.AddValueChanged(Control, OnTargerPropertyChanged);
        }

        void OnTargerPropertyChanged(object sender, EventArgs e) {
            listener.SetValue(Property.GetValue(Control));
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
            if(IsTwoWayBinding)
                Property.RemoveValueChanged(Control, OnTargerPropertyChanged);
            listener.Clear();
        }
    }
}
