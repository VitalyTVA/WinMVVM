using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WinMVVM.Utils {
    internal class BindingManagerActionBase {
        public BindingManagerActionBase(Control control, PropertyDescriptorBase property) {
            Property = property;
            Control = control;
        }
        public Control Control { get; private set; }
        public PropertyDescriptorBase Property { get; private set; }
        public bool IsMatchedAction(Control control, PropertyDescriptorBase property) {
            return this.Control == control && object.Equals(this.Property, property);
        }
    }
}
