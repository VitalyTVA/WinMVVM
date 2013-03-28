using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;
using WinMVVM.Utils;
using WinMVVM.Utils.Adapter;

namespace WinMVVM.Features {
    public abstract class BaseItemsSourceFeature<TControl> : BaseFeature<TControl>, IItemsSourceFeature where TControl : Control {
        protected static string GetPropertyName<T>(Expression<Func<T>> expression) {
            return ExpressionHelper.GetPropertyName(expression);
        }
        void IItemsSourceFeature.SetDataSource(Control control, object value) {
            SetDataSource((TControl)control, value);
        }
        string[] IItemsSourceFeature.GetItemsSourceAffectedProperties() {
            return GetItemsSourceAffectedProperties();
        }
        void IItemsSourceFeature.SetSelectedItem(Control control, object value) {
            SetSelectedItem((TControl)control, value);
        }
        object IItemsSourceFeature.GetSelectedItem(Control control) {
            return GetSelectedItem((TControl)control);
        }
        void IItemsSourceFeature.AddSelectionChangedCallback(Control control, Action<Control> action) {
            AddSelectionChangedCallback((TControl)control, action);
        }

        protected virtual void AddSelectionChangedCallback(TControl control, Action<Control> action) {
            throw new NotImplementedException();
        }

        protected abstract object GetSelectedItem(TControl tControl);
        protected abstract void SetSelectedItem(TControl tControl, object value);
        protected abstract string[] GetItemsSourceAffectedProperties();
        protected abstract void SetDataSource(TControl control, object value);


    }
}
