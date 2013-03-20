using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Forms;
using WpfBinding = System.Windows.Data.Binding;

namespace WinMVVM.Utils {
    internal static class SerializeHelper {
        internal static bool CanSerializeProperty(Control control, PropertyDescriptor property) {
            if(control is ListBox) {
                if(control.GetItemsSource() != null) //TODO - can't use local value
                    return property.Name != "Items" && property.Name != "DataSource";
            }
            return true;
        }
    }
}
