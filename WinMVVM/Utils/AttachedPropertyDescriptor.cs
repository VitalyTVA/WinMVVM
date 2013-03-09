using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Forms;
using WpfBinding = System.Windows.Data.Binding;

namespace WinMVVM.Utils {
    class AttachedPropertyDescriptor<T> : PropertyDescriptorBase {
        public static PropertyDescriptorBase FromAttachedProperty(AttachedProperty<T> property) {
            return new AttachedPropertyDescriptor<T>(property);
        }
        private readonly AttachedProperty<T> property;
        AttachedPropertyDescriptor(AttachedProperty<T> property)
            : base(property.Name, property.PropertyType) {
                this.property = property;
            
        }
        internal override object GetValue(Control control) {
            return control.GetValue(property);
        }

        //public override Type PropertyType {
        //    get { return typeof(T); } //TODO
        //}

        internal override void SetValue(Control control, object value) {
            control.SetValue(property, (T)value);
        }

        internal override void ResetValue(Control control) {
            throw new NotImplementedException();
        }

        public override int GetHashCode() {
            return property.GetHashCode();
        }
        public override bool Equals(object obj) {
            var other = obj as AttachedPropertyDescriptor<T>;
            return other != null && other.property == property;
        }
    }
}
