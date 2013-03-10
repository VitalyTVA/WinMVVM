using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Windows;
using System.Windows.Forms;

namespace WinMVVM.Utils {
    internal class SetValueAction : BindingManagerActionBase {
        public SetValueAction(Control control, PropertyDescriptorBase property, object value)
            : base(control, property) {
            Value = value;
        }
        public object Value { get; private set; }
    }
}
