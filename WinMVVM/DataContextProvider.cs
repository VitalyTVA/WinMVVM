using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;
using WinMVVM.Utils;

namespace WinMVVM {
    public static class DataContextProvider {
        public static readonly AttachedProperty<object> DataContextProperty = AttachedProperty<object>.Register(() => DataContextProperty, new PropertyMetadata<object>(null, null, PropertyMetadataOptions.Inherits));
        public static object GetDataContext(this Control control) {
            return control.GetValue(DataContextProperty);
        }
        public static void SetDataContext(this Control control, object value) {
            control.SetValue(DataContextProperty, value);
        }
        public static bool HasLocalDataContext(this Control control) {
            return control.HasLocalValue(DataContextProperty);
        }
        public static void ClearDataContext(this Control control) {
            control.ClearValue(DataContextProperty);
        }
    }
}