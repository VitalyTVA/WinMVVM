using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Forms;
using WpfBinding = System.Windows.Data.Binding;

namespace WinMVVM.Utils {
    class BindingExpressionKey {
        //TODO clear expressions with dead references
        readonly WeakReference controlReference;
        public Control Control { get { return (Control)controlReference.Target; } }
        public readonly PropertyDescriptorBase property;
        public BindingExpressionKey(Control control, PropertyDescriptorBase property) {
            this.property = property;
            this.controlReference = new WeakReference(control);

        }
        //TODO - check wheter target is alive
        public override int GetHashCode() {
            return Control.GetHashCode() ^ property.GetHashCode();
        }
        public override bool Equals(object obj) {
            var other = obj as BindingExpressionKey;
            return other != null && other.Control == Control && object.Equals(other.property, property);
        }
    }
}
