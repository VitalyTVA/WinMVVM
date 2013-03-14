using System;
using System.Collections.Generic;
using System.Linq;

namespace WinMVVM.Utils {
    interface IPropertyChangeListener {
        void Clear();
        void SetValue(object value);
    }
}
