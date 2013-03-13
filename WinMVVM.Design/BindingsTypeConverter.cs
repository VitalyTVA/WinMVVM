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
using System.Drawing.Design;
using System.Globalization;
using System.Windows.Forms.Design;

namespace WinMVVM.Design {
    public class BindingsTypeConverter : TypeConverter {
        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType) {
            if(destinationType == typeof(string)) {
                return string.Empty;
            }
            return base.ConvertTo(context, culture, value, destinationType);
        }
        public override bool GetPropertiesSupported(ITypeDescriptorContext context) {
            return true;
        }
        public override PropertyDescriptorCollection GetProperties(ITypeDescriptorContext context, object value, Attribute[] attributes) {
            BindingManagerDesigner designer = BindingManagerDesigner.FromTypeDescriptorContext(context);
            if(designer == null)
                return base.GetProperties(context, value, attributes);
            PropertyDescriptorCollection properties = new PropertyDescriptorCollection(new PropertyDescriptor[] {
                new BindablePropertyDescriptor(designer, DataContextProvider.DataContextProperty)
            });
            return  properties;
        }
    }
}
