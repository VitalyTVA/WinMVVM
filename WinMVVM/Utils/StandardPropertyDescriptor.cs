using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Forms;
using WpfBinding = System.Windows.Data.Binding;

namespace WinMVVM.Utils {
    class StandardPropertyDescriptor : PropertyDescriptorBase {
        public static PropertyDescriptorBase FromPropertyName(Control control, string propertyName) {
            PropertyDescriptor property = TypeDescriptor.GetProperties(control)[propertyName];
            if(property == null)
                Guard.ArgumentException("propertyName");
            return new StandardPropertyDescriptor(property);
        }

        readonly PropertyDescriptor property;
        StandardPropertyDescriptor(PropertyDescriptor property)
            : base(property.Name, property.PropertyType) {
            this.property = property;
        }
        public override int GetHashCode() {
            return property.GetHashCode();
        }
        public override bool Equals(object obj) {
            var other = obj as StandardPropertyDescriptor;
            return other != null && object.Equals(property, other.property);
        }

        internal override object GetValue(Control control) {
            return property.GetValue(control);
        }

        internal override void SetValue(Control control, object value) {
            property.SetValue(control, value);
        }

        internal override void ResetValue(Control control) {
            property.ResetValue(control);
        }

        internal override void AddValueChanged(Control control, EventHandler handler) {
            property.AddValueChanged(control, handler);
        }

        internal override void RemoveValueChanged(Control control, EventHandler handler) {
            property.RemoveValueChanged(control, handler);
        }
    }
}
