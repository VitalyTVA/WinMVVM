using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace WinMVVM.Design {
    class WrapperPropertyDescriptor : PropertyDescriptor {
        readonly PropertyDescriptor inner;
        public WrapperPropertyDescriptor(PropertyDescriptor inner)
            : base(inner, null) {
            this.inner = inner;
        }
        public override string ToString() {
            return inner.ToString();
        }
        public override bool SupportsChangeEvents {
            get { return inner.SupportsChangeEvents; }
        }
        public override void RemoveValueChanged(object component, EventHandler handler) {
            inner.RemoveValueChanged(component, handler);
        }
        public override string Name {
            get { return inner.Name; }
        }
        public override bool IsLocalizable {
            get { return inner.IsLocalizable; }
        }
        public override bool IsBrowsable {
            get { return inner.IsBrowsable; }
        }
        public override int GetHashCode() {
            return inner.GetHashCode();
        }
        public override object GetEditor(Type editorBaseType) {
            return inner.GetEditor(editorBaseType);
        }
        public override PropertyDescriptorCollection GetChildProperties(object instance, Attribute[] filter) {
            return inner.GetChildProperties(instance, filter);
        }
        public override bool Equals(object obj) {
            return inner.Equals(obj);
        }
        public override string DisplayName {
            get { return inner.DisplayName; }
        }
        public override bool DesignTimeOnly {
            get { return inner.DesignTimeOnly; }
        }
        public override string Description {
            get { return inner.Description; }
        }
        public override TypeConverter Converter {
            get { return inner.Converter; }
        }
        public override string Category {
            get { return inner.Category; }
        }
        public override AttributeCollection Attributes {
            get { return inner.Attributes; }
        }
        public override void AddValueChanged(object component, EventHandler handler) {
            inner.AddValueChanged(component, handler);
        }
        public override bool CanResetValue(object component) {
            return inner.CanResetValue(component);
        }
        public override Type ComponentType {
            get { return inner.ComponentType; }
        }
        public override object GetValue(object component) {
            return inner.GetValue(component);
        }
        public override bool IsReadOnly {
            get { return inner.IsReadOnly; }
        }
        public override Type PropertyType {
            get { return inner.PropertyType; }
        }
        public override void ResetValue(object component) {
            inner.ResetValue(component);
        }
        public override void SetValue(object component, object value) {
            inner.SetValue(component, value);
        }
        public override bool ShouldSerializeValue(object component) {
            return inner.ShouldSerializeValue(component);
        }
    }
}
