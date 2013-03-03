using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace WinMVVM {
    public delegate void AttachedPropertyChangedCallback<T>(Control sender, AttachedPropertyChangedEventArgs<T> e);
}
