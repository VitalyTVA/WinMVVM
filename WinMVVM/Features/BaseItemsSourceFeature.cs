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

namespace WinMVVM.Features {
    public abstract class BaseItemsSourceFeature<TControl> : BaseFeature<TControl>, IItemsSourceFeature where TControl : Control {
        void IItemsSourceFeature.SetDataSource(Control control, object value) {
            SetDataSource((TControl)control, value);
        }
        string[] IItemsSourceFeature.GetItemsSourceAffectedProperties() {
            return GetItemsSourceAffectedProperties();
        }

        protected abstract string[] GetItemsSourceAffectedProperties();
        protected abstract void SetDataSource(TControl control, object value);
    }
}
