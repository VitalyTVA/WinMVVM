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
    internal abstract class ListControlItemsSourceFeature<TListControl> : BaseItemsSourceFeature<TListControl> where TListControl : ListControl {
        static readonly string[] AffectedProperties = new string[] { "Items", "DataSource" };
        protected override void SetDataSource(TListControl control, object value) {
            control.DataSource = value;
        }
        protected override string[] GetItemsSourceAffectedProperties() {
            return AffectedProperties;
        }
    }
}
