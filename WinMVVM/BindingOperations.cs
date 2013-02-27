using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using WinMVVM.Utils;

namespace WinMVVM {
    public static class BindingOperations {
        public static void SetBinding(this Control control, string propertyName, BindingBase binding) {
            Guard.ArgumentNotNull(control, "control");
            Guard.ArgumentInRange(!string.IsNullOrEmpty(propertyName), "propertyName");
            Guard.ArgumentNotNull(binding, "binding");

            PropertyDescriptor property = TypeDescriptor.GetProperties(control)[propertyName];
            if(property == null)
                Guard.ArgumentException("propertyName");
            property.SetValue(control, control.GetDataContext());
        }
        //public static void ClearBinding(this Control control, string propertyName) {
        //}
        //public static void ClearAllBinding(this Control control, string propertyName) {
        //}

    }
}
