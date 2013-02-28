using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace WinMVVM.Tests {
    [TestFixture]
    public class BindingOperationsTests {
        [Test]
        public void NullControl() {
            Assert.Throws<ArgumentNullException>(() => BindingOperations.SetBinding(null, "Test", new Binding()));
            using(var button = new Button()) {
                Assert.Throws<ArgumentOutOfRangeException>(() => BindingOperations.SetBinding(button, null, new Binding()));
                Assert.Throws<ArgumentOutOfRangeException>(() => BindingOperations.SetBinding(button, string.Empty, new Binding()));
                Assert.Throws<ArgumentException>(() => BindingOperations.SetBinding(button, "Foo", new Binding()));
                Assert.Throws<ArgumentNullException>(() => BindingOperations.SetBinding(button, "Text", null));
            }
        }
        [Test]
        public void SetSimpleBindingBeforeSetDataContext() {
            using(var button = new Button()) {
                button.SetBinding("Text", new Binding());
                Assert.That(button.HasLocalDataContext(), Is.False);

                button.SetDataContext("test");
                Assert.That(button.Text, Is.EqualTo("test"));
                Assert.That(button.HasLocalDataContext(), Is.True);

                button.SetDataContext("test2");
                Assert.That(button.Text, Is.EqualTo("test2"));
            }
        }
        [Test]
        public void SetSimpleBindingAfterSetDataContext() {
            using(var button = new Button()) {
                button.SetDataContext("test");
                button.SetBinding("Text", new Binding());
                Assert.That(button.Text, Is.EqualTo("test"));

                button.SetDataContext("test2");
                Assert.That(button.Text, Is.EqualTo("test2"));

                int textChangedCount = 0;
                button.TextChanged += (sender, e) => {
                    textChangedCount++;
                };
                button.SetDataContext("test2");
                Assert.That(textChangedCount, Is.EqualTo(0));
            }
        }
        public class TestButton : Button {
            public int Text2SetCount;
            string text2;
            public string Text2 { 
                get { return text2; } 
                set { 
                    text2 = value; 
                    Text2SetCount++; 
                } 
            }
        }
        [Test]
        public void NoExtraBindingUpdates() {
            using(var button = new TestButton()) {
                button.SetDataContext("test");
                button.SetBinding("Text2", new Binding());
                Assert.That(button.Text2SetCount, Is.EqualTo(1));
                button.SetDataContext("test");
                Assert.That(button.Text2SetCount, Is.EqualTo(1));
            }
        }
        [Test]
        public void InheritedDataContextBinding_BindAfterAddToTree() {
            using(var form = new Form()) {
                var button1 = new Button();
                var button2 = new Button();
                form.Controls.Add(button1);
                form.SetDataContext("test");
                form.Controls.Add(button2);

                //button1.SetBinding("Text", new Binding());
                //Assert.That(button1.Text, Is.EqualTo("test"));
                //Assert.That(button1.HasLocalDataContext(), Is.False);

                button2.SetBinding("Text", new Binding());
                Assert.That(button2.Text, Is.EqualTo("test"));
                Assert.That(button2.HasLocalDataContext(), Is.False);


                //Assert.That(button1.GetDataContext(), Is.EqualTo("test"));
                //Assert.That(button2.GetDataContext(), Is.EqualTo("test"));
                //Assert.That(button1.HasLocalDataContext(), Is.False);
                //Assert.That(button2.HasLocalDataContext(), Is.False);

                //button1.SetDataContext("button1");
                //Assert.That(button1.GetDataContext(), Is.EqualTo("button1"));
                //button2.SetDataContext(null);
                //Assert.That(button2.GetDataContext(), Is.Null);
                //Assert.That(button1.HasLocalDataContext(), Is.True);
                //Assert.That(button2.HasLocalDataContext(), Is.True);
            }
        }
    }
}
