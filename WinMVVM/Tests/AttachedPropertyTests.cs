using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace WinMVVM.Tests {
    [TestFixture]
    public class AttachedPropertyTests {
        public static readonly AttachedProperty TestProperty1;
        [Test]
        public void Regisrtation() {
            Assert.That(TestPropertyContainer.TestProperty.Name, Is.EqualTo("Test"));
            Assert.That(TestPropertyContainer.TestProperty.OwnerType, Is.EqualTo(typeof(TestPropertyContainer)));

            Assert.That(TestPropertyContainer.TestPropertyProperty.Name, Is.EqualTo("TestProperty"));
            Assert.That(TestPropertyContainer.TestPropertyProperty.OwnerType, Is.EqualTo(typeof(TestPropertyContainer)));

            Assert.That(TestPropertyContainer.Test2Property.Name, Is.EqualTo("Test2"));
            Assert.That(TestPropertyContainer.Test2Property.OwnerType, Is.EqualTo(typeof(TestPropertyContainer)));

            Assert.Throws<ArgumentException>(
                () => AttachedProperty.Register(() => TestProperty1)
            );
            Assert.Throws<ArgumentOutOfRangeException>(
                () => AttachedProperty.Register(null, typeof(AttachedPropertyTests))
            );
            Assert.Throws<ArgumentOutOfRangeException>(
                () => AttachedProperty.Register(string.Empty, typeof(AttachedPropertyTests))
            );
            Assert.Throws<ArgumentNullException>(
               () => AttachedProperty.Register("NoOwnerType", null)
            );
        }
        [Test]
        public void RegisterInheritableProperty() {
            using(var form = new Form()) {
                var button1 = new Button();
                var button2 = new Button();
                form.Controls.Add(button1);
                form.Controls.Add(button2);

                Assert.That(button1.GetValue(TestPropertyContainer.TestProperty), Is.Null);
                Assert.That(button2.GetValue(TestPropertyContainer.TestProperty), Is.Null);
                Assert.That(form.GetValue(TestPropertyContainer.TestProperty), Is.Null);

                button1.SetValue(TestPropertyContainer.TestProperty, "button1");
                Assert.That(button1.GetValue(TestPropertyContainer.TestProperty), Is.EqualTo("button1"));

                form.SetValue(TestPropertyContainer.TestProperty, "form");
                Assert.That(form.GetValue(TestPropertyContainer.TestProperty), Is.EqualTo("form"));
                Assert.That(button1.GetValue(TestPropertyContainer.TestProperty), Is.EqualTo("button1"));
                Assert.That(button2.GetValue(TestPropertyContainer.TestProperty), Is.EqualTo("form"));

                Assert.That(button2.HasLocalValue(TestPropertyContainer.TestProperty), Is.False);
                Assert.That(button1.HasLocalValue(TestPropertyContainer.TestProperty), Is.True);

                button1.ClearValue(TestPropertyContainer.TestProperty);
                Assert.That(button1.GetValue(TestPropertyContainer.TestProperty), Is.EqualTo("form"));
                Assert.That(button1.HasLocalValue(TestPropertyContainer.TestProperty), Is.False);
            }
        }
    }
    public static class TestPropertyContainer {
        public static readonly AttachedProperty TestProperty = AttachedProperty.Register(() => TestProperty);
        public static readonly AttachedProperty TestPropertyProperty = AttachedProperty.Register(() => TestPropertyProperty);
        public static readonly AttachedProperty Test2Property = AttachedProperty.Register("Test2", typeof(TestPropertyContainer));

    }
}
