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
                form.Controls.Add(button);
                var viewModel = new TestViewModel() { StringProperty = "test", StringProperty2 = "test2" };
                form.SetDataContext(viewModel);
                BindingManager manager = new BindingManager();

                manager.SetBinding(form, "Text", new Binding("StringProperty"));
                Assert.That(form.Text, Is.EqualTo("test"));
                Assert.That(manager.GetActions().Count(), Is.EqualTo(1));

                SetBindingAction action = manager.GetActions().First();
                Assert.That(action.Control, Is.EqualTo(form));
                Assert.That(action.Property, Is.EqualTo("Text"));
                Assert.That(((Binding)action.Binding).Path, Is.EqualTo("StringProperty"));

                manager.SetBinding(button, "Text", new Binding("StringProperty2"));
                Assert.That(button.Text, Is.EqualTo("test2"));
                Assert.That(manager.GetActions().Count(), Is.EqualTo(2));

                action = manager.GetActions().ElementAt(1);
                Assert.That(action.Control, Is.EqualTo(button));
                Assert.That(action.Property, Is.EqualTo("Text"));

                manager.SetBinding(form, "Text", new Binding("StringProperty2"));
                Assert.That(form.Text, Is.EqualTo("test2"));
                Assert.That(manager.GetActions().Count(), Is.EqualTo(2));

                action = manager.GetActions().First();
                Assert.That(action.Control, Is.EqualTo(form));
                Assert.That(action.Property, Is.EqualTo("Text"));
                Assert.That(((Binding)action.Binding).Path, Is.EqualTo("StringProperty2"));

                Assert.That(manager.Find(form, "Text"), Is.EqualTo(manager.GetActions().First()));
                Assert.That(manager.Find(button, "Text"), Is.EqualTo(manager.GetActions().ElementAt(1)));
                Assert.That(manager.Find(form, "Tag"), Is.Null);

                manager.Remove(form, "Text");
                Assert.That(form.Text, Is.EqualTo(string.Empty));
                Assert.That(manager.GetActions().Count(), Is.EqualTo(1));

                action = manager.GetActions().First();
                Assert.That(action.Control, Is.EqualTo(button));
                Assert.That(action.Property, Is.EqualTo("Text"));

                manager.SetBinding(form, "Text", new Binding("StringProperty"));
                manager.SetBinding(form, "Tag", new Binding("StringProperty"));
                Assert.That(manager.GetActions().Count(), Is.EqualTo(3));
                manager.RemoveControlActions(form);
                Assert.That(manager.GetActions().Count(), Is.EqualTo(1));

                action = manager.GetActions().First();
                Assert.That(action.Control, Is.EqualTo(button));
                Assert.That(action.Property, Is.EqualTo("Text"));
            }
        }
        [Test]
        public void NullParameters() {
            using(var form = new Form()) {
                BindingManager manager = new BindingManager();

                Assert.Throws<ArgumentNullException>(
                    () => manager.SetBinding(null, "Text", new Binding())
                );
                Assert.Throws<ArgumentOutOfRangeException>(
                    () => manager.SetBinding(form, null, new Binding())
                );
                Assert.Throws<ArgumentOutOfRangeException>(
                    () => manager.SetBinding(form, string.Empty, new Binding())
                );
                Assert.Throws<ArgumentNullException>(
                    () => manager.SetBinding(form, "Text", null)
                );

                Assert.Throws<ArgumentNullException>(
                    () => manager.Remove(null, "Text")
                );
                Assert.Throws<ArgumentOutOfRangeException>(
                    () => manager.Remove(form, null)
                );
                Assert.Throws<ArgumentOutOfRangeException>(
                    () => manager.Remove(form, string.Empty)
                );

                Assert.Throws<ArgumentNullException>(
                    () => manager.RemoveControlActions(null)
                );

                Assert.Throws<ArgumentNullException>(
                    () => manager.Find(null, "Text")
                );
                Assert.Throws<ArgumentOutOfRangeException>(
                    () => manager.Find(form, null)
                );
            }
        }
    }
}
