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
    public interface IItemsSourceFeature : IFeature {
        void SetDataSource(Control control, object value);
        string[] GetItemsSourceAffectedProperties();
    }
}
