using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Windows;
using System.Windows.Forms;
using WinMVVM.Utils;

namespace WinMVVM.Utils {
    class SetBindingActionConverter : TypeConverter {
        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType) {
            return destinationType == typeof(InstanceDescriptor) || base.CanConvertTo(context, destinationType);
        }
        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType) {
            if(destinationType == typeof(InstanceDescriptor) && value is SetBindingAction) {
                var action = value as SetBindingAction;
                ConstructorInfo constructor = typeof(SetBindingAction).GetConstructor(new[] { typeof(Control), typeof(string), typeof(BindingBase) });
                return new InstanceDescriptor(constructor, new object[] { action.Control, action.Property, action.Binding }, true);
            }
            return base.ConvertTo(context, culture, value, destinationType);
        }
    }
    class BindingConverter : TypeConverter {
        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType) {
            return destinationType == typeof(InstanceDescriptor) || base.CanConvertTo(context, destinationType);
        }
        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType) {
            if(destinationType == typeof(InstanceDescriptor) && value is Binding) {
                var binding = value as Binding;
                if(string.IsNullOrEmpty(binding.Path)) {
                    ConstructorInfo constructor = typeof(Binding).GetConstructor(Type.EmptyTypes);
                    return new InstanceDescriptor(constructor, new object[] { }, true);
                } else {
                    ConstructorInfo constructor = typeof(Binding).GetConstructor(new[] { typeof(string) });
                    return new InstanceDescriptor(constructor, new object[] { binding.Path }, true);
                }
            }
            return base.ConvertTo(context, culture, value, destinationType);
        }
    }

}
