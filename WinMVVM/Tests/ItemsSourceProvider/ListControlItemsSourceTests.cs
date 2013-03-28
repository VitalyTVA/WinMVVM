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
        [Test]
        public void SelectedItemChange() {
            var list = new List<TestData>() { 
                        new TestData(0, "text 0"),
                        new TestData(1, "text 1"),
                    };
            using(var form = new Form()) {
                var control = CreateControl();
                form.Controls.Add(control);
                control.SetItemsSource(list);
                SetSelectedIndex(control, 0);

                //Assert.That(control.GetSelectedItem(), Is.EqualTo(null)); ??? //TODO

                control.SetSelectedItem(list[1]);
                Assert.That(control.GetSelectedItem(), Is.EqualTo(list[1]));
                Assert.That(GetSelectedIndex(control), Is.EqualTo(1));

                control.SetSelectedItem(list[0]);
                Assert.That(control.GetSelectedItem(), Is.EqualTo(list[0]));
                Assert.That(GetSelectedIndex(control), Is.EqualTo(0));

                control.SetSelectedItem(list[1]);
                control.SetSelectedItem("bla");
                Assert.That(control.GetSelectedItem(), Is.EqualTo(list[1]));
                Assert.That(GetSelectedIndex(control), Is.EqualTo(1));
            }
        }
        public class TestViewModel : BindableBase {
            TestData testData;
            public TestData TestData {
                get { return testData; }
                set { SetProperty(ref testData, value, () => TestData); }
            }
        }
        [Test]
        public void SelectedItemBinding() {
            var list = new List<TestData>() { 
                        new TestData(0, "text 0"),
                        new TestData(1, "text 1"),
                    };
            var viewModel = new TestViewModel() { TestData = list[0] };
            using(var form = new Form()) {
                form.SetDataContext(viewModel);
                var control = CreateControl();
                form.Controls.Add(control);
                control.SetItemsSource(list);
                SetSelectedIndex(control, 1);

                control.SetBinding(ItemsSourceProvider.SelectedItemProperty, new Binding(() => new TestViewModel().TestData, BindingMode.TwoWay));
                Assert.That(control.GetSelectedItem(), Is.EqualTo(list[0]));
                Assert.That(viewModel.TestData, Is.EqualTo(list[0]));

                viewModel.TestData = list[1];
                Assert.That(control.GetSelectedItem(), Is.EqualTo(list[1]));
                Assert.That(viewModel.TestData, Is.EqualTo(list[1]));

                SetSelectedIndex(control, 0);
                //Assert.That(control.GetSelectedItem(), Is.EqualTo(list[0]));
                //Assert.That(viewModel.TestData, Is.EqualTo(list[0]));
            }
        }
        //binding when source property is null 
        //binding when source property is equal to currently selected item in control
        //one way binding
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