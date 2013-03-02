using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using WinMVVM.Utils;

namespace WinMVVM {
    public static class BindingOperations {
        class BindingExpression {
            private readonly Control control;
            private readonly string propertyName;
            public BindingExpression(Control control, string propertyName) {
                this.propertyName = propertyName;
                this.control = control;

                var propertyEntry = control.GetPropertyEntry();
                if(propertyEntry != null) {
                    propertyEntry.PropertyChanged += OnPropertyEntryChanged;
                    UpdateTargetProperty(propertyEntry);
                }
            }
            public void OnPropertyEntryChanged(object sender, PropertyChangedEventArgs e) {
                var propertyEntry = (DataContextProvider.PropertyEntry)sender;
                UpdateTargetProperty(propertyEntry);
            }
            void UpdateTargetProperty(DataContextProvider.PropertyEntry propertyEntry) {
                PropertyDescriptor property = GetProperty(control, propertyName);
                if(property == null)
                    Guard.ArgumentException("propertyName");
                if(propertyEntry.IsValueSet) {
                    property.SetValue(control, propertyEntry.PropertyValue);
                } else {
                    //TODO  CanResetValue
                    property.ResetValue(control);
                }
            }
        }
        public static void SetBinding(this Control control, string propertyName, BindingBase binding) {
            Guard.ArgumentNotNull(control, "control");
            Guard.ArgumentInRange(!string.IsNullOrEmpty(propertyName), "propertyName");
            Guard.ArgumentNotNull(binding, "binding");
            if(GetProperty(control, propertyName) == null)
                Guard.ArgumentException("propertyName");

            new BindingExpression(control, propertyName);

        }

        //public static void ClearBinding(this Control control, string propertyName) {
        //}
        //public static void ClearAllBinding(this Control control, string propertyName) {
        //}

        static PropertyDescriptor GetProperty(Control control, string propertyName) {
            return TypeDescriptor.GetProperties(control)[propertyName];
        }

    }
}
