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
    internal class ListBoxItemsSourceFeature : ListControlItemsSourceFeature<ListBox> {
        protected override void SetSelectedItem(ListBox control, object value) {
            control.SelectedItem = value;
        }
        protected override object GetSelectedItem(ListBox control) {
            return control.SelectedItem;
        }
        protected override void AddSelectionChangedCallback(ListBox control, Action<Control> action) {
            control.SelectedIndexChanged += (o, e) => action((Control)o);
        }
    }
}
