using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Forms;
using WinMVVM.Features;
using WpfBinding = System.Windows.Data.Binding;

namespace WinMVVM.Utils {
    internal static class SerializeHelper {
        internal static bool CanSerializeProperty(Control control, PropertyDescriptor property) {
            IItemsSourceFeature feature = FeatureProvider<IItemsSourceFeature>.GetFeature(control);
            if(feature != null) {
                if(control.GetItemsSource() == null)
                    return true;
                return !feature.GetItemsSourceAffectedProperties().Any(x => (x == property.Name));
            }
            return true;
        }
    }
}
