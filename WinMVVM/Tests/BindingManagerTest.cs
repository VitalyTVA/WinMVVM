using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

namespace WinMVVM.Tests {
    [TestFixture]
    public class BindingManagerTest {
        [Test]
        public void AddRemoveBindings() {
            using(var form = new Form()) {
                var button = new Button();
                var viewModel = new TestViewModel() { StringProperty = "test", StringProperty2 = "test2" };
                form.SetDataContext(viewModel);
                BindingManager manager = new BindingManager();

                manager.SetBinding(form, "Text", new Binding("StringProperty"));
                Assert.That(form.Text, Is.EqualTo("test"));

                //SetBindingAction action = new
            }
        }
    }
}
