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
    public class BindingsEditor : UITypeEditor {
        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value) {
            //IWindowsFormsEditorService service = (IWindowsFormsEditorService)context.GetService(typeof(IWindowsFormsEditorService)); //TODO
            BindingManagerDesigner designer = BindingManagerDesigner.FromTypeDescriptorContext(context);
            if(designer == null) {
                return value;
            }
            designer.RunDesigner();
            return value;
        }
        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context) {
            return UITypeEditorEditStyle.Modal;
        }
    }
}
