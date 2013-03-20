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
using WinMVVM.Utils;

namespace WinMVVM.Design {
    class BoundElementTypeDescriptionProvider : TypeDescriptionProvider {
        class BoundElementTypeDescriptor : CustomTypeDescriptor {
            class BoundPropertyDescriptor : WrapperPropertyDescriptor {
                public BoundPropertyDescriptor(PropertyDescriptor inner)
                    : base(inner) {
                }
                public override bool ShouldSerializeValue(object component) {
                    return false;
                }
            }

            readonly object instance;
            public BoundElementTypeDescriptor(ICustomTypeDescriptor inner, object instance)
                : base(inner) {
                    this.instance = instance;
            }
            public override PropertyDescriptorCollection GetProperties() {
                return Patch(base.GetProperties());
            }
            public override PropertyDescriptorCollection GetProperties(Attribute[] attributes) {
                return Patch(base.GetProperties(attributes));
            }

            PropertyDescriptorCollection Patch(PropertyDescriptorCollection original) {
                return new PropertyDescriptorCollection(original.Cast<PropertyDescriptor>().Select(x => {
                    var control = instance as Control;
                    if(control != null && (BindingOperations.IsBoundProperty(control, x) || !SerializeHelper.CanSerializeProperty(control, x)))
                        return new BoundPropertyDescriptor(x);
                    return x;
                }).ToArray());
            }
        }
        public BoundElementTypeDescriptionProvider(TypeDescriptionProvider parent)
            : base(parent) {
        }
        public override ICustomTypeDescriptor GetTypeDescriptor(Type objectType, object instance) {
            if(instance is Control)
                return new BoundElementTypeDescriptor(base.GetTypeDescriptor(objectType, instance), instance);
            return base.GetTypeDescriptor(objectType, instance);
        }
    }
}
