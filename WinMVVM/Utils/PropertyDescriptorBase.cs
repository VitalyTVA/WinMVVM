using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Forms;
using WpfBinding = System.Windows.Data.Binding;

namespace WinMVVM.Utils {
    abstract class PropertyDescriptorBase {
        public PropertyDescriptorBase(string name, Type propertyType) {
            PropertyType = propertyType;
            Name = name;
        }
        public string Name { get; private set; }
        public Type PropertyType { get; private set; }
        internal abstract void ResetValue(Control control);
        internal abstract object GetValue(Control control);
        internal abstract void SetValue(Control control, object value);
    }
}
