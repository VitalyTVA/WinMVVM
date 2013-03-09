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
    internal class SetBindingAction {
        public SetBindingAction(Control control, PropertyDescriptorBase property, BindingBase binding) {
            Property = property;
            Control = control;
            Binding = binding;
        }
        public BindingBase Binding { get; private set; }
        public Control Control { get; private set; }
        public PropertyDescriptorBase Property { get; private set; }
        public bool IsMatchedAction(Control control, PropertyDescriptorBase property) {
            return this.Control == control && object.Equals(this.Property, property);
        }
    }
}