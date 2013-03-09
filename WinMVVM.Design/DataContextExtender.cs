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
        public DataContextExtender() {
        }
        public bool CanExtend(object extendee) {
            return extendee is Control;
        }
        [Category("Attached properties")]
        [TypeConverter(typeof(DataContextConverter))]
        public object GetDataContext(Control component) {
            return DataContextProvider.GetDataContext(component);
        }
        public void SetDataContext(Control component, object value) {
            DataContextProvider.SetDataContext(component, value);
        }
    }
}
