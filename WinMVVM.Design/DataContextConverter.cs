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
    public class DataContextConverter : TypeConverter {
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType) {
            return sourceType == typeof(string) || base.CanConvertFrom(context, sourceType);
        }
        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType) {
            return destinationType == typeof(string) || base.CanConvertTo(context, destinationType);
        }
        public override object ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destinationType) {
            if(destinationType == typeof(string) && value != null)
                return value.ToString();
            return base.ConvertTo(context, culture, value, destinationType);
        }
        public override object ConvertFrom(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value) {
            if(value is string) {
                ITypeDiscoveryService service = context.GetService(typeof(ITypeDiscoveryService)) as ITypeDiscoveryService;
                if(service != null) {
                    foreach(Type type in service.GetTypes(typeof(object), true)) {
                        if(type.Name == (string)value)
                            return Activator.CreateInstance(type);
                    }
                }
            }
            return base.ConvertFrom(context, culture, value);
        }
    }
}
