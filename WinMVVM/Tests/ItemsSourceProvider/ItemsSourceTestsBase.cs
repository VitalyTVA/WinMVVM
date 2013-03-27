using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Windows.Forms;
using System.Windows.Input;
using WinMVVM.Utils;

namespace WinMVVM.Tests.ItemsSource {
    public abstract class ItemsSourceTestsBase<TControl> where TControl : Control, new() {
        protected static string GetPropertyName<T>(Expression<Func<T>> expression) {
            return ExpressionHelper.GetPropertyName(expression);
        }
        protected abstract IList GetItems(TControl control);
        protected abstract object GetDataSource(TControl control);
        protected abstract string[] GetItemsSourceDependentProperties();
        protected abstract int GetSelectedIndex(TControl control);
        protected abstract void SetSelectedIndex(TControl control, int value);

        protected virtual TControl CreateControl() {
            return new TControl();
        }
        //TODO what to do with synchronization in different controls bound to same collection?

        [Test]
        public void BindToSimpleList() {
            var list = new List<TestData>() { 
                        new TestData(0, "text 0"),
                        new TestData(1, "text 1"),
                    };
            using(var form = new Form()) {
                var control = CreateControl();
                form.Controls.Add(control);
                AssertCanSerializeProperties(control, true, GetItemsSourceDependentProperties());
                control.SetItemsSource(list);
                AssertCanSerializeProperties(control, false, GetItemsSourceDependentProperties());
                IList items = GetItems(control);
                Assert.That(items.Count, Is.EqualTo(2));
                Assert.That(items[0], Is.EqualTo(list[0]));
                Assert.That(items[1], Is.EqualTo(list[1]));

                control.SetItemsSource(null);
                AssertCanSerializeProperties(control, true, GetItemsSourceDependentProperties());
                items = GetItems(control);
                Assert.That(items.Count, Is.EqualTo(0));
                Assert.That(GetDataSource(control), Is.Null);
            }
        }
        void AssertCanSerializeProperties(Control control, bool canSerialize, params string[] properties) {
            foreach(string property in properties) {
                Assert.That(TypeDescriptor.GetProperties(control)[property], Is.Not.Null);
            }
            foreach(PropertyDescriptor property in TypeDescriptor.GetProperties(control)) {
                bool actualCanSerialize = true;
                if(!canSerialize)
                    actualCanSerialize = !properties.Contains(property.Name);
                Assert.That(SerializeHelper.CanSerializeProperty(control, property), Is.EqualTo(actualCanSerialize));
            }
        }
        [Test]
        public void BindToBindingList() {
            var list = new BindingList<TestData>() { 
                        new TestData(0, "text 0"),
                        new TestData(1, "text 1"),
                    };
            using(var form = new Form()) {
                var control = CreateControl();
                form.Controls.Add(control);
                control.SetItemsSource(list);
                IList items = GetItems(control);
                Assert.That(items.Count, Is.EqualTo(2));
                Assert.That(items[0], Is.EqualTo(list[0]));
                Assert.That(items[1], Is.EqualTo(list[1]));

                list.Add(new TestData(9, "new"));
                items = GetItems(control);
                Assert.That(items.Count, Is.EqualTo(3));
                Assert.That(items[0], Is.EqualTo(list[0]));
                Assert.That(items[1], Is.EqualTo(list[1]));
                Assert.That(items[2], Is.EqualTo(list[2]));

                control.SetItemsSource(null);
                items = GetItems(control);
                Assert.That(items.Count, Is.EqualTo(0));
            }
        }
        [Test]
        public void BindToObservableCollection() {
            var list = new ObservableCollection<TestData>() { 
                        new TestData(0, "text 0"),
                        new TestData(1, "text 1"),
                    };
            using(var form = new Form()) {
                var control = CreateControl();
                form.Controls.Add(control);
                control.SetItemsSource(list);
                IList items = GetItems(control);
                Assert.That(items.Count, Is.EqualTo(2));
                Assert.That(items[0], Is.EqualTo(list[0]));
                Assert.That(items[1], Is.EqualTo(list[1]));

                list.Add(new TestData(9, "new"));
                items = GetItems(control);
                Assert.That(items.Count, Is.EqualTo(3));
                Assert.That(items[0], Is.EqualTo(list[0]));
                Assert.That(items[1], Is.EqualTo(list[1]));
                Assert.That(items[2], Is.EqualTo(list[2]));

                control.SetItemsSource(null);
                items = GetItems(control);
                Assert.That(items.Count, Is.EqualTo(0));
            }
        }
        [Test]
        public void CurrentItemIsNotSynchronizedInDifferentControls() {
            var list = new List<TestData>() { 
                        new TestData(0, "text 0"),
                        new TestData(1, "text 1"),
                    };
            using(var form = new Form()) {
                var control1 = CreateControl();
                var control2 = CreateControl();
                form.Controls.Add(control1);
                form.Controls.Add(control2);
                control1.SetItemsSource(list);
                control2.SetItemsSource(list);

                SetSelectedIndex(control1, 1);
                Assert.That(GetSelectedIndex(control1), Is.EqualTo(1));
                Assert.That(GetSelectedIndex(control2), Is.Not.EqualTo(1));

                SetSelectedIndex(control1, 0);
                Assert.That(GetSelectedIndex(control1), Is.EqualTo(0));

                SetSelectedIndex(control2, 1);
                Assert.That(GetSelectedIndex(control2), Is.EqualTo(1));
                Assert.That(GetSelectedIndex(control1), Is.EqualTo(0));
            }
        }
        //TODO IsSynchronizedWithCurrentItem
    }
}
