using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinMVVM.Utils.Adapter {
    [Flags]
    internal enum ItemPropertyNotificationMode {
        None = 0,
        PropertyChanged = 1
    }
}
