using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Windows;
using System.Windows.Forms;
using WinMVVM.Utils;

namespace WinMVVM {
    public static class AttachedPropertyExtensions {
        public static object GetValue(this Control control, AttachedProperty property) {
            return property.GetValue(control);
        }
        public static void SetValue(this Control control, AttachedProperty property, object value) {
            property.SetValue(control, value);
        }
        public static bool HasLocalValue(this Control control, AttachedProperty property) {
            return property.HasLocalValue(control);
        }
        public static void ClearValue(this Control control, AttachedProperty property) {
            property.ClearValue(control);
        }
    }
}
