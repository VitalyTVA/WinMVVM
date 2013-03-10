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
    [ProvideProperty("DataContext", typeof(Control))]
    public class DataContextExtender : System.ComponentModel.IExtenderProvider {
        readonly BindingManagerDesigner designer;
        public DataContextExtender(BindingManagerDesigner designer) {
            this.designer = designer;
        }
        public bool CanExtend(object extendee) {
            return extendee is Control;
        }
        [Category("Attached properties")]
        [TypeConverter(typeof(DataContextConverter))]
        public object GetDataContext(Control control) {
            return DataContextProvider.GetDataContext(control);
        }
        public void SetDataContext(Control control, object value) {
            designer.ChangeComponent(() => {
                var manager = designer.Component as BindingManager;
                if(value != null)
                    manager.SetValue(control, DataContextProvider.DataContextProperty, value);
                else
                    manager.ClearValue(control, DataContextProvider.DataContextProperty);
            });
        }
        public void ResetDataContext(Control control) {
            designer.ChangeComponent(() => {
                var manager = designer.Component as BindingManager;
                manager.ClearValue(control, DataContextProvider.DataContextProperty);
            });
        }
    }
}