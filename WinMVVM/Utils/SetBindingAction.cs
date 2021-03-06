using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Windows;
using System.Windows.Forms;
using WinMVVM.Utils;

namespace WinMVVM.Utils {
    internal class SetBindingAction : BindingManagerActionBase {
        public SetBindingAction(Control control, PropertyDescriptorBase property, BindingBase binding) 
            : base(control, property) {
            Binding = binding;
        }
        public BindingBase Binding { get; private set; }
    }
}