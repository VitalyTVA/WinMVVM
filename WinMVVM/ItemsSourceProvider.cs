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
using WinMVVM.Features;
using WinMVVM.Features.ItemsSource;

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
            IItemsSourceFeature feature = sender.GetItemsSourceFeature();
            if(feature != null) {
                IEnumerable dataSource = (e.NewValue is INotifyCollectionChanged && e.NewValue is IList) ? BindingListAdapter.CreateFromList((IList)e.NewValue) : e.NewValue;//TODO
                feature.SetDataSource(sender, dataSource.With(x => new BindingSource() { DataSource = dataSource }));
            }
        }

        public static readonly AttachedProperty<object> SelectedItemProperty = AttachedProperty<object>.Register(() => SelectedItemProperty, new PropertyMetadata<object>(null, OnSelectedItemChanged));
        public static object GetSelectedItem(this Control control) {
            return control.GetValue(SelectedItemProperty);
        }
        public static void SetSelectedItem(this Control control, object value) {
            control.SetValue(SelectedItemProperty, value);
        }
        static readonly AttachedProperty<Action<Control>> HandlerProperty = AttachedProperty<Action<Control>>.Register(() => HandlerProperty);
        static void OnSelectedItemChanged(Control sender, AttachedPropertyChangedEventArgs<object> e) {
            IItemsSourceFeature feature = sender.GetItemsSourceFeature();
            if(feature != null) {
                //TODO test it
                //Action selectionChangedHandler = sender.GetValue(HandlerProperty);
                //if(selectionChangedHandler == null) {
                //    selectionChangedHandler = new Action(OnSelectionChanged);
                //    sender.SetValue(HandlerProperty, selectionChangedHandler);
                //    feature.AddSelectionChangedCallback(sender, selectionChangedHandler);
                //}

                feature.AddSelectionChangedCallback(sender, OnSelectionChanged);

                feature.SetSelectedItem(sender, e.NewValue);
                sender.SetSelectedItem(feature.GetSelectedItem(sender));
                //TODO test recursion or add assert
            }
        }
        static void OnSelectionChanged(Control sender) {
            IItemsSourceFeature feature = sender.GetItemsSourceFeature();
            sender.SetSelectedItem(feature.GetSelectedItem(sender));
            //TODO test recursion or add assert
        }
        internal static IItemsSourceFeature GetItemsSourceFeature(this Control sender) {
            return FeatureProvider<IItemsSourceFeature>.GetFeature(sender);
        }
    }
}
