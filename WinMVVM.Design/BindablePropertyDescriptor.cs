using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WinMVVM.Design {
    class BindablePropertyDescriptor : PropertyDescriptor {
        readonly BindingManagerDesigner designer;
        readonly AttachedPropertyBase property;
        public BindablePropertyDescriptor(BindingManagerDesigner designer, AttachedPropertyBase property)
            : base(property.Name, new Attribute [] { new TypeConverterAttribute(typeof(DataContextConverter)) }) {
                this.designer = designer;
                this.property = property;
            
        }
        public override bool CanResetValue(object component) {
            return true;
        }

        public override Type ComponentType {
            get { return typeof(object); }
        }

        public override object GetValue(object component) {
            return property.GetValueInternal((Control)component);
        }

        public override bool IsReadOnly {
            get { return false; }
        }

        public override Type PropertyType {
            get { return typeof(object); }
        }

        public override void ResetValue(object component) {
            designer.ChangeComponent(() => {
                var manager = designer.Component as BindingManager;
                manager.ClearValue((Control)component, property);
            });
        }

        public override void SetValue(object component, object value) {
            designer.ChangeComponent(() => {
                var manager = designer.Component as BindingManager;
                if(value != null)
                    manager.SetValue((Control)component, property, value);
                else
                    manager.ClearValue((Control)component, property);
            });
        }

        public override bool ShouldSerializeValue(object component) {
            return false;
        }
    }
}
