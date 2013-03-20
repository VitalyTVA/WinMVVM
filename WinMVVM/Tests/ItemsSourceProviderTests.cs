using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Input;
using WinMVVM.Utils;

namespace WinMVVM.Tests {
    [TestFixture]
    public class ItemsSourceProviderTests {
        [Test]
        public void SetListToNotItemsControl() {
            var list = new List<TestData>();
            using(var button = new Button()) {
                button.SetItemsSource(list);
            }
        }
        [Test]
        public void BindToSimpleList() {
            var list = new List<TestData>() { 
                new TestData(0, "text 0"),
                new TestData(1, "text 1"),
            };
            using(var form = new Form()) {
                var listBox = new ListBox();
                form.Controls.Add(listBox);
                AssertCanSerializeProperties(listBox, true, "Items", "DataSource");
                listBox.SetItemsSource(list);
                AssertCanSerializeProperties(listBox, false, "Items", "DataSource");
                Assert.That(listBox.Items.Count, Is.EqualTo(2));
                Assert.That(listBox.Items[0], Is.EqualTo(list[0]));
                Assert.That(listBox.Items[1], Is.EqualTo(list[1]));

                listBox.SetItemsSource(null);
                AssertCanSerializeProperties(listBox, true, "Items", "DataSource");
                Assert.That(listBox.Items.Count, Is.EqualTo(0));
                Assert.That(listBox.DataSource, Is.Null);
            }
        }
        void AssertCanSerializeProperties(Control control, bool canSerialize, params string[] properties) {
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
                var listBox = new ListBox();
                form.Controls.Add(listBox);
                listBox.SetItemsSource(list);
                Assert.That(listBox.Items.Count, Is.EqualTo(2));
                Assert.That(listBox.Items[0], Is.EqualTo(list[0]));
                Assert.That(listBox.Items[1], Is.EqualTo(list[1]));

                list.Add(new TestData(9, "new"));
                Assert.That(listBox.Items.Count, Is.EqualTo(3));
                Assert.That(listBox.Items[0], Is.EqualTo(list[0]));
                Assert.That(listBox.Items[1], Is.EqualTo(list[1]));
                Assert.That(listBox.Items[2], Is.EqualTo(list[2]));

                listBox.SetItemsSource(null);
                Assert.That(listBox.Items.Count, Is.EqualTo(0));
            }
        }
        [Test]
        public void BindToObservableCollection() {
            var list = new ObservableCollection<TestData>() { 
                new TestData(0, "text 0"),
                new TestData(1, "text 1"),
            };
            using(var form = new Form()) {
                var listBox = new ListBox();
                form.Controls.Add(listBox);
                listBox.SetItemsSource(list);
                Assert.That(listBox.Items.Count, Is.EqualTo(2));
                Assert.That(listBox.Items[0], Is.EqualTo(list[0]));
                Assert.That(listBox.Items[1], Is.EqualTo(list[1]));

                list.Add(new TestData(9, "new"));
                Assert.That(listBox.Items.Count, Is.EqualTo(3));
                Assert.That(listBox.Items[0], Is.EqualTo(list[0]));
                Assert.That(listBox.Items[1], Is.EqualTo(list[1]));
                Assert.That(listBox.Items[2], Is.EqualTo(list[2]));

                listBox.SetItemsSource(null);
                Assert.That(listBox.Items.Count, Is.EqualTo(0));
            }
        }
    }
    public class TestData {
        public int IntProperty { get; set; }
        public string TextProperty { get; set; }
        public TestData(int intProperty, string textProperty) {
            IntProperty = intProperty;
            TextProperty = textProperty;
        }
    }

}
