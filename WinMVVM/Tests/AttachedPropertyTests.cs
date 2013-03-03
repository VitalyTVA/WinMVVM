using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace WinMVVM.Tests {
    [TestFixture]
    public class AttachedPropertyTests {
        public static readonly AttachedProperty<object> TestProperty1;
        [Test]
        public void Regisrtation() {
            Assert.That(TestPropertyContainer.TestProperty.Name, Is.EqualTo("Test"));
            Assert.That(TestPropertyContainer.TestProperty.OwnerType, Is.EqualTo(typeof(TestPropertyContainer)));

            Assert.That(TestPropertyContainer.TestPropertyProperty.Name, Is.EqualTo("TestProperty"));
            Assert.That(TestPropertyContainer.TestPropertyProperty.OwnerType, Is.EqualTo(typeof(TestPropertyContainer)));

            Assert.That(TestPropertyContainer.Test2Property.Name, Is.EqualTo("Test2"));
            Assert.That(TestPropertyContainer.Test2Property.OwnerType, Is.EqualTo(typeof(TestPropertyContainer)));

            Assert.Throws<ArgumentException>(
                () => AttachedProperty<object>.Register(() => TestProperty1)
            );
            Assert.Throws<ArgumentOutOfRangeException>(
                () => AttachedProperty<object>.Register(null, typeof(AttachedPropertyTests))
            );
            Assert.Throws<ArgumentOutOfRangeException>(
                () => AttachedProperty<object>.Register(string.Empty, typeof(AttachedPropertyTests))
            );
            Assert.Throws<ArgumentNullException>(
               () => AttachedProperty<object>.Register("NoOwnerType", null)
            );
        }
        [Test]
        public void RegisterInheritableAndNoInheritablPropertiesProperty() {
            using(var form = new Form()) {
                var button1 = new Button();
                var button2 = new Button();
                form.Controls.Add(button1);
                form.Controls.Add(button2);

                Assert.That(button1.GetValue(TestPropertyContainer.TestProperty), Is.Null);
                Assert.That(button2.GetValue(TestPropertyContainer.TestProperty), Is.Null);
                Assert.That(form.GetValue(TestPropertyContainer.TestProperty), Is.Null);

                button1.SetValue(TestPropertyContainer.TestProperty, "button1");
                button1.SetValue(TestPropertyContainer.Test2Property, "button1_");
                Assert.That(button1.GetValue(TestPropertyContainer.TestProperty), Is.EqualTo("button1"));
                Assert.That(button1.GetValue(TestPropertyContainer.Test2Property), Is.EqualTo("button1_"));

                form.SetValue(TestPropertyContainer.TestProperty, "form");
                form.SetValue(TestPropertyContainer.Test2Property, "form2");
                Assert.That(form.GetValue(TestPropertyContainer.TestProperty), Is.EqualTo("form"));
                Assert.That(button1.GetValue(TestPropertyContainer.TestProperty), Is.EqualTo("button1"));
                Assert.That(button2.GetValue(TestPropertyContainer.TestProperty), Is.EqualTo("form"));
                Assert.That(form.GetValue(TestPropertyContainer.Test2Property), Is.EqualTo("form2"));
                Assert.That(button1.GetValue(TestPropertyContainer.Test2Property), Is.EqualTo("button1_"));
                Assert.That(button2.GetValue(TestPropertyContainer.Test2Property), Is.Null);

                Assert.That(button2.HasLocalValue(TestPropertyContainer.TestProperty), Is.False);
                Assert.That(button1.HasLocalValue(TestPropertyContainer.TestProperty), Is.True);

                button1.ClearValue(TestPropertyContainer.TestProperty);
                Assert.That(button1.GetValue(TestPropertyContainer.TestProperty), Is.EqualTo("form"));
                Assert.That(button1.HasLocalValue(TestPropertyContainer.TestProperty), Is.False);

                button1.ClearValue(TestPropertyContainer.Test2Property);
                Assert.That(button1.GetValue(TestPropertyContainer.Test2Property), Is.Null);
                Assert.That(button1.HasLocalValue(TestPropertyContainer.Test2Property), Is.False);

                button1.SetValue(TestPropertyContainer.Test2Property, "button1_");
                form.ClearValue(TestPropertyContainer.Test2Property);
                Assert.That(button1.GetValue(TestPropertyContainer.Test2Property), Is.EqualTo("button1_"));

                form.SetValue(TestPropertyContainer.Test2Property, "form2");
                var button3 = new Button();
                form.Controls.Add(button3);
                Assert.That(button3.GetValue(TestPropertyContainer.Test2Property), Is.Null);

                button3.SetValue(TestPropertyContainer.Test2Property, "button3_");
                form.Controls.Remove(button3);
                Assert.That(button3.GetValue(TestPropertyContainer.Test2Property), Is.EqualTo("button3_"));
            }
        }
        [Test]
        public void DefaultValue() {
            using(var button = new Button()) {
                Assert.That(button.GetValue(TestPropertyContainer.DefaultValueProperty), Is.EqualTo("d"));
                button.SetValue(TestPropertyContainer.DefaultValueProperty, "button");
                Assert.That(button.GetValue(TestPropertyContainer.DefaultValueProperty), Is.EqualTo("button"));
                button.ClearValue(TestPropertyContainer.DefaultValueProperty);
                Assert.That(button.GetValue(TestPropertyContainer.DefaultValueProperty), Is.EqualTo("d"));
            }
        }
    }
    public static class TestPropertyContainer {
        public static readonly AttachedProperty<object> TestProperty = AttachedProperty<object>.Register(() => TestProperty, new PropertyMetadata<object>(null, null, PropertyMetadataOptions.Inherits));
        public static readonly AttachedProperty<object> TestPropertyProperty = AttachedProperty<object>.Register(() => TestPropertyProperty);
        public static readonly AttachedProperty<object> Test2Property = AttachedProperty<object>.Register("Test2", typeof(TestPropertyContainer), new PropertyMetadata<object>(null, null, PropertyMetadataOptions.None));
        public static readonly AttachedProperty<object> DefaultValueProperty = AttachedProperty<object>.Register(() => DefaultValueProperty, new PropertyMetadata<object>("d"));

    }
}
