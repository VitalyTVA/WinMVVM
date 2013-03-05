using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Windows;
using System.Windows.Forms;
using WinMVVM.Utils;

namespace WinMVVM {
    [TypeConverter(typeof(SetBindingActionConverter))]
    public class SetBindingAction {
        public SetBindingAction(Control control, string property, BindingBase binding) {
            Property = property;
            Control = control;
            Binding = binding;
        }
        public BindingBase Binding { get; private set; }
        public Control Control { get; private set; }
        public string Property { get; private set; }
    }
}