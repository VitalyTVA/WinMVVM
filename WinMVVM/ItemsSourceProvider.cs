using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;
using WinMVVM.Utils;

namespace WinMVVM {
    public static class ItemsSourceProvider {
        public static readonly AttachedProperty<IEnumerable> ItemsSourceProperty = AttachedProperty<IEnumerable>.Register(() => ItemsSourceProperty, new PropertyMetadata<IEnumerable>(null, OnItemsSourceChanged));
        public static IEnumerable GetItemsSource(this Control control) {
            return control.GetValue(ItemsSourceProperty);
        }
        public static void SetItemsSource(this Control control, IEnumerable value) {
            control.SetValue(ItemsSourceProperty, value);
        }
        static void OnItemsSourceChanged(Control sender, AttachedPropertyChangedEventArgs<IEnumerable> e) {
            var listBox = sender as ListBox;
            if(listBox == null)
                return;
            listBox.DataSource = e.NewValue;
        }
    }
}
