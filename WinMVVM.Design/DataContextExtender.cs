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
    [ProvideProperty("Bindings", typeof(Control))]
    public class CommonPropertiesExtender : System.ComponentModel.IExtenderProvider {
        readonly BindingManagerDesigner designer;
        public CommonPropertiesExtender(BindingManagerDesigner designer) {
            this.designer = designer;
        }
        public bool CanExtend(object extendee) {
            return extendee is Control;
        }
        [Category(SR_Design.AttachedPropertiesCategory)]
        [ParenthesizePropertyName(true)]
        [TypeConverter(typeof(BindingsTypeConverter))]
        [Editor(typeof(BindingsEditor), typeof(UITypeEditor))]
        public object GetBindings(Control control) {
            return control;
        }
        public void SetBindings(Control control, object value) {
        }
    }
}
