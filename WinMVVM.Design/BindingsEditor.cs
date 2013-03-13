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
            if(provider != null) {
                IWindowsFormsEditorService service = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));
                IDesignerHost host = provider.GetService(typeof(IDesignerHost)) as IDesignerHost;
                if((service == null) || (host == null)) {
                    return value;
                }

                BindingManager manager = host.Container.Components.Cast<IComponent>().FirstOrDefault(x => x is BindingManager) as BindingManager;
                if(manager == null) {
                    return value;
                }
                BindingManagerDesigner designer = host.GetDesigner(manager) as BindingManagerDesigner;
                if(designer == null) {
                    return value;
                }
                designer.RunDesigner();
            }
            return value;
        }
        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context) {
            return UITypeEditorEditStyle.Modal;
        }
    }
}
