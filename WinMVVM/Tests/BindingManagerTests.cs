using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using WinMVVM.Utils;

namespace WinMVVM.Tests {
    [TestFixture]
    public class BindingManagerTests {
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
                Assert.That(action.Property.Name, Is.EqualTo("Text"));
                Assert.That(action.Property, Is.InstanceOf<StandardPropertyDescriptor>());
                Assert.That(((Binding)action.Binding).Path, Is.EqualTo("StringProperty"));

                manager.SetBinding(button, "Text", new Binding("StringProperty2"));
                Assert.That(button.Text, Is.EqualTo("test2"));
                Assert.That(manager.GetActions().Count(), Is.EqualTo(2));

                action = manager.GetActions().ElementAt(1);
                Assert.That(action.Control, Is.EqualTo(button));
                Assert.That(action.Property.Name, Is.EqualTo("Text"));

                manager.SetBinding(form, "Text", new Binding("StringProperty2"));
                Assert.That(form.Text, Is.EqualTo("test2"));
                Assert.That(manager.GetActions().Count(), Is.EqualTo(2));

                action = manager.GetActions().First();
                Assert.That(action.Control, Is.EqualTo(form));
                Assert.That(action.Property.Name, Is.EqualTo("Text"));
                Assert.That(((Binding)action.Binding).Path, Is.EqualTo("StringProperty2"));

                manager.Remove(form, "Text");
                Assert.That(form.Text, Is.EqualTo(string.Empty));
                Assert.That(manager.GetActions().Count(), Is.EqualTo(1));

                action = manager.GetActions().First();
                Assert.That(action.Control, Is.EqualTo(button));
                Assert.That(action.Property.Name, Is.EqualTo("Text"));

                manager.SetBinding(form, "Text", new Binding("StringProperty"));
                manager.SetBinding(form, "Tag", new Binding("StringProperty"));
                Assert.That(manager.GetActions().Count(), Is.EqualTo(3));
                manager.ClearAllBindings(form);
                Assert.That(manager.GetActions().Count(), Is.EqualTo(1));

                action = manager.GetActions().First();
                Assert.That(action.Control, Is.EqualTo(button));
                Assert.That(action.Property.Name, Is.EqualTo("Text"));

                manager.SetBinding(form, TextProperty, new Binding("StringProperty"));
                Assert.That(form.GetValue(TextProperty), Is.EqualTo("test"));
                Assert.That(manager.GetActions().Count(), Is.EqualTo(2));

                action = manager.GetActions().ElementAt(1);
                Assert.That(action.Control, Is.EqualTo(form));
                Assert.That(action.Property.Name, Is.EqualTo("Text"));
                Assert.That(action.Property, Is.InstanceOf<AttachedPropertyDescriptor<string>>());
                Assert.That(((Binding)action.Binding).Path, Is.EqualTo("StringProperty"));

                manager.ClearBinding(form, TextProperty);
                Assert.That(form.GetValue(TextProperty), Is.EqualTo(null));
                Assert.That(manager.GetActions().Count(), Is.EqualTo(1));
                Assert.That(manager.GetActions().First().Property, Is.InstanceOf<StandardPropertyDescriptor>());

                manager.SetBinding(button, TextProperty, new Binding("StringProperty2"));
                Assert.That(button.GetValue(TextProperty), Is.EqualTo("test2"));
                Assert.That(manager.GetActions().Count(), Is.EqualTo(2));

                manager.ClearAllBindings(button);
                Assert.That(manager.GetActions().Count(), Is.EqualTo(0));
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
                    () => manager.SetBinding(form, (string)null, new Binding())
                );
                Assert.Throws<ArgumentOutOfRangeException>(
                    () => manager.SetBinding(form, string.Empty, new Binding())
                );
                Assert.Throws<ArgumentNullException>(
                    () => manager.SetBinding(form, "Text", null)
                );

                Assert.Throws<ArgumentException>(
                    () => manager.Remove(null, "Text")
                );
                Assert.Throws<ArgumentNullException>(
                    () => manager.Remove(form, (string)null)
                );
                Assert.Throws<ArgumentException>(
                    () => manager.Remove(form, string.Empty)
                );

                Assert.Throws<ArgumentNullException>(
                    () => manager.ClearAllBindings(null)
                );
            }
        }
        static AttachedProperty<string> TextProperty = AttachedProperty<string>.Register(() => TextProperty);
        static AttachedProperty<string> TagProperty = AttachedProperty<string>.Register(() => TagProperty);
    }
}
