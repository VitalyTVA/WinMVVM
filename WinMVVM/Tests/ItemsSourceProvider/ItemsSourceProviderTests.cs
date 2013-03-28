using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Input;
using WinMVVM.Utils;

namespace WinMVVM.Tests.ItemsSource {
    [TestFixture]
    public class ItemsSourceProviderTests {
        [Test]
        public void SetListToNotItemsControl() {
            var list = new List<TestData>();
            using(var button = new Button()) {
                button.SetItemsSource(list);
                Assert.That(button.GetItemsSource(), Is.EqualTo(list));
                button.SetSelectedItem("foo");
                Assert.That(button.GetSelectedItem(), Is.EqualTo("foo"));
            }
        }
    }
}
