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
            if(listBox != null) {
                IEnumerable dataSource = (e.NewValue is INotifyCollectionChanged && e.NewValue is IList) ? BindingListAdapter.CreateFromList((IList)e.NewValue) : e.NewValue;//TODO
                listBox.DataSource = dataSource;
            }

            //var gridView = sender as DataGridView;
            //if(gridView != null) {
            //    IEnumerable dataSource = (e.NewValue is INotifyCollectionChanged && e.NewValue is IList) ? BindingListAdapter.CreateFromList((IList)e.NewValue) : e.NewValue;//TODO
            //    gridView.DataSource = dataSource != null ? new BindingSource(dataSource, null) : null;
            //}

        }

    }
}
