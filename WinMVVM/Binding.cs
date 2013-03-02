using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using WinMVVM.Utils;

namespace WinMVVM {
    public class Binding : BindingBase {
        public Binding(string path = null) {
            Path = path;
        }
        public string Path { get; private set; }
    }
}
