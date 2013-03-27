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
    public abstract class ListControlItemsSourceTests<TListControl> : ItemsSourceTestsBase<TListControl> where TListControl : ListControl, new() {
        protected override object GetDataSource(TListControl control) {
            return control.DataSource;
        }
        protected override string[] GetItemsSourceDependentProperties() {
            return new string[] { GetPropertyName(() => new ListBox().Items), GetPropertyName(() => new TListControl().DataSource) };
        }
        protected override int GetSelectedIndex(TListControl control) {
            return control.SelectedIndex;
        }
        protected override void SetSelectedIndex(TListControl control, int value) {
            control.SelectedIndex = value;
        }
    }
    [TestFixture]
    public class ListBoxItemsSourceTests : ListControlItemsSourceTests<ListBox> {
        protected override IList GetItems(ListBox control) {
            return control.Items;
        }
    }

    public class CustomListBox : ListBox { 
    }
    [TestFixture]
    public class CustomListBoxItemsSourceTests : ListControlItemsSourceTests<CustomListBox> {
        protected override IList GetItems(CustomListBox control) {
            return control.Items;
        }
    }

    [TestFixture]
    public class ComboBoxItemsSourceTests : ListControlItemsSourceTests<ComboBox> {
        protected override IList GetItems(ComboBox control) {
            return control.Items;
        }
    }
}
