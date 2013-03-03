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
            return null;
        }
        public static void SetValue(this Control control, AttachedProperty property, object value) {
        }
    }
}
