using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using WinMVVM.Utils;

namespace WinMVVM.Tests {
    [TestFixture]
    public class BindingManagerTests {
        [Test]
        public void SetClearBindings() {
            using(var form = new Form()) {
                var button = new Button();
                form.Controls.Add(button);
                var viewModel = new TestViewModel() { StringProperty = "test", StringProperty2 = "test2" };
                form.SetDataContext(viewModel);
                BindingManager manager = new BindingManager();

                manager.SetBinding(form, "Text", new Binding("StringProperty"));
                Assert.That(form.Text, Is.EqualTo("test"));
                Assert.That(manager.GetBindingActions().Count(), Is.EqualTo(1));
                Assert.That(manager.GetValueActions().Count(), Is.EqualTo(0));

                SetBindingAction action = manager.GetBindingActions().First();
                Assert.That(action.Control, Is.EqualTo(form));
                Assert.That(action.Property.Name, Is.EqualTo("Text"));
                Assert.That(action.Property, Is.InstanceOf<StandardPropertyDescriptor>());
                Assert.That(((Binding)action.Binding).Path, Is.EqualTo("StringProperty"));

                manager.SetBinding(button, "Text", new Binding("StringProperty2"));
                Assert.That(button.Text, Is.EqualTo("test2"));
                Assert.That(manager.GetBindingActions().Count(), Is.EqualTo(2));

                action = manager.GetBindingActions().ElementAt(1);
                Assert.That(action.Control, Is.EqualTo(button));
                Assert.That(action.Property.Name, Is.EqualTo("Text"));

                manager.SetBinding(form, "Text", new Binding("StringProperty2"));
                Assert.That(form.Text, Is.EqualTo("test2"));
                Assert.That(manager.GetBindingActions().Count(), Is.EqualTo(2));

                action = manager.GetBindingActions().First();
                Assert.That(action.Control, Is.EqualTo(form));
                Assert.That(action.Property.Name, Is.EqualTo("Text"));
                Assert.That(((Binding)action.Binding).Path, Is.EqualTo("StringProperty2"));

                manager.ClearBinding(form, "Text");
                Assert.That(form.Text, Is.EqualTo(string.Empty));
                Assert.That(manager.GetBindingActions().Count(), Is.EqualTo(1));

                action = manager.GetBindingActions().First();
                Assert.That(action.Control, Is.EqualTo(button));
                Assert.That(action.Property.Name, Is.EqualTo("Text"));

                manager.SetBinding(form, "Text", new Binding("StringProperty"));
                manager.SetBinding(form, "Tag", new Binding("StringProperty"));
                Assert.That(manager.GetBindingActions().Count(), Is.EqualTo(3));
                manager.ClearAllBindings(form);
                Assert.That(manager.GetBindingActions().Count(), Is.EqualTo(1));

                action = manager.GetBindingActions().First();
                Assert.That(action.Control, Is.EqualTo(button));
                Assert.That(action.Property.Name, Is.EqualTo("Text"));

                manager.SetBinding(form, TextProperty, new Binding("StringProperty"));
                Assert.That(form.GetValue(TextProperty), Is.EqualTo("test"));
                Assert.That(manager.GetBindingActions().Count(), Is.EqualTo(2));

                action = manager.GetBindingActions().ElementAt(1);
                Assert.That(action.Control, Is.EqualTo(form));
                Assert.That(action.Property.Name, Is.EqualTo("Text"));
                Assert.That(action.Property, Is.InstanceOf<AttachedPropertyDescriptor>());
                Assert.That(((Binding)action.Binding).Path, Is.EqualTo("StringProperty"));

                manager.ClearBinding(form, TextProperty);
                Assert.That(form.GetValue(TextProperty), Is.EqualTo(null));
                Assert.That(manager.GetBindingActions().Count(), Is.EqualTo(1));
                Assert.That(manager.GetBindingActions().First().Property, Is.InstanceOf<StandardPropertyDescriptor>());

                manager.SetBinding(button, TextProperty, new Binding("StringProperty2"));
                Assert.That(button.GetValue(TextProperty), Is.EqualTo("test2"));
                Assert.That(manager.GetBindingActions().Count(), Is.EqualTo(2));

                manager.ClearAllBindings(button);
                Assert.That(manager.GetBindingActions().Count(), Is.EqualTo(0));
            }
        }
        [Test]
        public void SetClearValue() {
            using(var form = new Form()) {
                var button = new Button();
                form.Controls.Add(button);
                var viewModel = new TestViewModel() { StringProperty = "test", StringProperty2 = "test2" };
                form.SetDataContext(viewModel);
                BindingManager manager = new BindingManager();

                manager.SetBinding(button, DataContextProvider.DataContextProperty, new Binding(() => viewModel.StringProperty));
                manager.SetValue(button, DataContextProvider.DataContextProperty, "value");
                Assert.That(button.GetDataContext(), Is.EqualTo("value"));

                Assert.That(manager.GetBindingActions().Count(), Is.EqualTo(0));
                Assert.That(manager.GetValueActions().Count(), Is.EqualTo(1));
                SetValueAction action = manager.GetValueActions().First();
                Assert.That(action.Control, Is.EqualTo(button));
                Assert.That(((AttachedPropertyDescriptor)action.Property).Property, Is.EqualTo(DataContextProvider.DataContextProperty));
                Assert.That(action.Value, Is.EqualTo("value"));
                Assert.That(manager.FindAction(button, AttachedPropertyDescriptor.FromAttachedProperty(DataContextProvider.DataContextProperty)), Is.EqualTo(action));

                manager.SetValue(button, DataContextProvider.DataContextProperty, "value2");
                Assert.That(manager.GetBindingActions().Count(), Is.EqualTo(0));
                Assert.That(manager.GetValueActions().Count(), Is.EqualTo(1));
                Assert.That(button.GetDataContext(), Is.EqualTo("value2"));
                action = manager.GetValueActions().First();
                Assert.That(action.Control, Is.EqualTo(button));
                Assert.That(((AttachedPropertyDescriptor)action.Property).Property, Is.EqualTo(DataContextProvider.DataContextProperty));
                Assert.That(action.Value, Is.EqualTo("value2"));

                manager.ClearValue(button, DataContextProvider.DataContextProperty);
                Assert.That(manager.GetBindingActions().Count(), Is.EqualTo(0));
                Assert.That(manager.GetValueActions().Count(), Is.EqualTo(0));
                Assert.That(button.GetDataContext(), Is.Null);
            }
        }
        [Test]
        public void UndoSetBinding() {
            using(var form = new Form()) {
                //form initialized
                BindingManager manager = new BindingManager();
                ((ISupportInitialize)manager).BeginInit();
                ((ISupportInitialize)manager).EndInit();
                //binding created with wizard
                manager.SetBinding(form, TextProperty, new Binding());
                //deserialization on undo
                ((ISupportInitialize)manager).BeginInit();
                ((ISupportInitialize)manager).EndInit();

                Assert.That(manager.GetBindingActions().Count(), Is.EqualTo(0));
                Assert.That(manager.GetValueActions().Count(), Is.EqualTo(0));

                form.SetDataContext("test");
                Assert.That(form.GetValue(TextProperty), Is.Null);
            }
        }
        [Test]
        public void UndoSetBindingAndValue() {
            using(var form = new Form()) {
                //form initialized
                BindingManager manager = new BindingManager();
                ((ISupportInitialize)manager).BeginInit();
                ((ISupportInitialize)manager).EndInit();
                //binding created with wizard
                manager.SetBinding(form, TextProperty, new Binding());
                //value set with property grid
                manager.SetValue(form, TextProperty, "test");
                //deserialization on undo
                ((ISupportInitialize)manager).BeginInit();
                Assert.That(form.GetValue(TextProperty), Is.Null);
                manager.SetBinding(form, TextProperty, new Binding());
                ((ISupportInitialize)manager).EndInit();
                //undo from property grid
                manager.ClearValue(form, TextProperty);

                Assert.That(manager.GetBindingActions().Count(), Is.EqualTo(1));
                Assert.That(manager.GetValueActions().Count(), Is.EqualTo(0));

                form.SetDataContext("Test");
                Assert.That(form.GetValue(TextProperty), Is.EqualTo("Test"));
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
                Assert.Throws<ArgumentNullException>(
                    () => manager.SetBinding(form, (AttachedPropertyBase)null, new Binding())
                );

                Assert.Throws<ArgumentException>(
                    () => manager.ClearBinding(null, "Text")
                );
                Assert.Throws<ArgumentNullException>(
                    () => manager.ClearBinding(form, (string)null)
                );
                Assert.Throws<ArgumentNullException>(
                    () => manager.ClearBinding(form, (AttachedPropertyBase)null)
                );
                Assert.Throws<ArgumentException>(
                    () => manager.ClearBinding(form, string.Empty)
                );

                Assert.Throws<ArgumentNullException>(
                    () => manager.ClearAllBindings(null)
                );

                Assert.Throws<ArgumentNullException>(
                    () => manager.SetValue(null, TextProperty, string.Empty)
                );
                Assert.Throws<ArgumentNullException>(
                    () => manager.SetValue(form, (AttachedPropertyBase)null, string.Empty)
                );
                Assert.Throws<ArgumentNullException>(
                    () => manager.ClearValue(null, TextProperty)
                );
                Assert.Throws<ArgumentNullException>(
                    () => manager.ClearValue(form, (AttachedPropertyBase)null)
                );
            }
        }
        static AttachedProperty<string> TextProperty = AttachedProperty<string>.Register(() => TextProperty);
        static AttachedProperty<string> TagProperty = AttachedProperty<string>.Register(() => TagProperty);
    }
}
