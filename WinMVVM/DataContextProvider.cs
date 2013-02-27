using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinMVVM {
    [
    ProvideProperty("DataContext",typeof(Control)),
    ]
    public class DataContextProvider : Component, IExtenderProvider {
        bool IExtenderProvider.CanExtend(object extendee) {
            return true;
        }
        public string GetDataContext(Control control) {
            return "Test";
        }
        public void SetDataContext(Control control, string value) { 
        }
    }
}
