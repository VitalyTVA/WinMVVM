using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Input;
using WinMVVM.Utils;

namespace WinMVVM.Tests.ItemsSource {
    [TestFixture]
    public class ListBoxItemsSourceTests : ItemsSourceTestsBase<ListBox> {
        protected override IList GetItems(ListBox control) {
            return control.Items;
        }
        protected override object GetDataSource(ListBox control) {
            return control.DataSource;
        }
    }
}
