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
        public static T GetValue<T>(this Control control, AttachedProperty<T> property) {
            return property.GetValue(control);
        }
        public static void SetValue<T>(this Control control, AttachedProperty<T> property, T value) {
            property.SetValue(control, value);
        }
        public static bool HasLocalValue<T>(this Control control, AttachedProperty<T> property) {
            return property.HasLocalValue(control);
        }
        public static void ClearValue<T>(this Control control, AttachedProperty<T> property) {
            property.ClearValue(control);
        }
    }
}
