using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Windows;
using System.Windows.Forms;
using WinMVVM.Utils;

namespace WinMVVM {
    public class SetBindingActionCollection : Collection<SetBindingAction> {
        readonly BindingManager owner;
        public SetBindingActionCollection(BindingManager owner) {
            this.owner = owner;
        }
        protected override void InsertItem(int index, SetBindingAction item) {
            base.InsertItem(index, item);
            //BindingOperations.SetBinding(item.Control, item.Property, item.Binding);
        }
        protected override void ClearItems() {
            base.ClearItems();
        }
        protected override void RemoveItem(int index) {
            base.RemoveItem(index);
        }
        protected override void SetItem(int index, SetBindingAction item) {
            base.SetItem(index, item);
        }
    }
}
