using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;
using WinMVVM.Features.ItemsSource;
using WinMVVM.Utils;
using WinMVVM.Utils.Adapter;

namespace WinMVVM.Features {
    public interface IFeature {
        bool IsSupportedControl(Control control);
    }
}
