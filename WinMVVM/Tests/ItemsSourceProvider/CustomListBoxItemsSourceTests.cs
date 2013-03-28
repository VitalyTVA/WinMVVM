using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using System.Windows.Input;
using WinMVVM.Features;
using WinMVVM.Features.ItemsSource;
using WinMVVM.Utils;

namespace WinMVVM.Tests.ItemsSource {
    public class CustomListBox : ListBox {
        const string EventAccessorName = "EVENT_SELECTEDINDEXCHANGED";
        public int GetSelectionChangedSuscribeCount() {
            object accessor = typeof(ListBox).GetField(EventAccessorName, BindingFlags.Static | BindingFlags.NonPublic).GetValue(null);
            EventHandler handler = (EventHandler)base.Events[accessor];
            return handler != null ? handler.GetInvocationList().Count() : 0;
        }
    }
    [TestFixture]
    public class CustomListBoxItemsSourceTests : ListControlItemsSourceTests<CustomListBox> {
        [Test]
        public void SubscribeSelectionChangedOnlyOnce() {
            var list = new List<TestData>() { 
                            new TestData(0, "text 0"),
                            new TestData(1, "text 1"),
                        };
            using(var form = new Form()) {
                var control = new CustomListBox();
                form.Controls.Add(control);
                control.SetItemsSource(list);
                Assert.That(control.GetSelectionChangedSuscribeCount(), Is.EqualTo(0));
                control.SetSelectedItem(list[1]);
                Assert.That(control.GetSelectionChangedSuscribeCount(), Is.EqualTo(1));
                control.SetSelectedItem(list[0]);
                Assert.That(control.GetSelectionChangedSuscribeCount(), Is.EqualTo(1));
            }

        }
        protected override IList GetItems(CustomListBox control) {
            return control.Items;
        }
    }
}
