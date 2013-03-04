using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Forms;
using WpfBinding = System.Windows.Data.Binding;

namespace WinMVVM.Utils {
    class AttachedPropertyDescriptor<T> : PropertyDescriptor {
        private readonly AttachedProperty<T> property;
        public AttachedPropertyDescriptor(AttachedProperty<T> property)
            : base(property.Name, null) {
                this.property = property;
            
        }
        public override bool CanResetValue(object component) {
            throw new NotImplementedException();
        }

        public override Type ComponentType {
            get { throw new NotImplementedException(); }
        }

        public override object GetValue(object component) {
            return ((Control)component).GetValue(property);
        }

        public override bool IsReadOnly {
            get { throw new NotImplementedException(); }
        }

        public override Type PropertyType {
            get { return typeof(T); } //TODO
        }

        public override void ResetValue(object component) {
            throw new NotImplementedException();
        }

        public override void SetValue(object component, object value) {
            ((Control)component).SetValue(property, (T)value);
        }

        public override bool ShouldSerializeValue(object component) {
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
