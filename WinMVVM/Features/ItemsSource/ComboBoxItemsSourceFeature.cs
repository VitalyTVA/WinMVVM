using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;
using WinMVVM.Utils;
using WinMVVM.Utils.Adapter;

namespace WinMVVM.Features.ItemsSource {
    internal class ComboBoxItemsSourceFeature : ListControlItemsSourceFeature<ComboBox> {
        protected override void SetSelectedItem(ComboBox tControl, object value) {
            throw new NotImplementedException();
        }
        protected override object GetSelectedItem(ComboBox control) {
            throw new NotImplementedException();
        }
    }
}
