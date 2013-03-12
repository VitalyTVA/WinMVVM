using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Forms;
using WpfBinding = System.Windows.Data.Binding;

namespace WinMVVM.Utils {
    internal interface IWeakEventHandler<THandler> {
        THandler Handler { get; }
    }
}
