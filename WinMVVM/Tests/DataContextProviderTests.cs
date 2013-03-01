using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace WinMVVM.Tests {
    [TestFixture]
    public class DataContextProviderTests {
        [Test]
        public void SetAndGetDataContextForControl() {
            using(var button1 = new Button()) {
                Assert.That(button1.GetDataContext(), Is.Null);
                Assert.That(button1.HasLocalDataContext(), Is.False);
                button1.SetDataContext("test");
                Assert.That(button1.GetDataContext(), Is.EqualTo("test"));
                Assert.That(button1.HasLocalDataContext(), Is.True);

                button1.ClearDataContext();
                Assert.That(button1.GetDataContext(), Is.Null);
                Assert.That(button1.HasLocalDataContext(), Is.False);
            }

            using(var button2 = new Button()) {
                Assert.That(button2.GetDataContext(), Is.Null);
                button2.SetDataContext(13);
                Assert.That(button2.GetDataContext(), Is.EqualTo(13));
            }

            using(var button3 = new Button()) {
                button3.ClearDataContext();
                Assert.That(button3.GetDataContext(), Is.Null);
                Assert.That(button3.HasLocalDataContext(), Is.False);
            }
        }
        [Test]
        public void CollectControlWithDataContextSet() {
            var reference = DoCollectControlWithDataContextSet();
            TestUtils.GarbageCollect();
            Assert.That(reference.IsAlive, Is.False);
        }
        WeakReference DoCollectControlWithDataContextSet() {
            var button = new Button();
            button.SetDataContext("test");
            return new WeakReference(button);
        }
        [Test]
        public void NullControl() {
            Assert.Throws<ArgumentNullException>(() => DataContextProvider.GetDataContext(null));
            Assert.Throws<ArgumentNullException>(() => DataContextProvider.SetDataContext(null, 5));
            Assert.Throws<ArgumentNullException>(() => DataContextProvider.HasLocalDataContext(null));
            Assert.Throws<ArgumentNullException>(() => DataContextProvider.ClearDataContext(null));
        }
        [Test]
        public void InheritDataContextFromParent() {
            using(var form = new Form()) {
                var button1 = new Button();
                var button2 = new Button();
                var button3 = new Button();
                button3.SetDataContext("button3");
                form.Controls.Add(button1);
                form.SetDataContext("test");
                form.Controls.Add(button2);
                form.Controls.Add(button3);
                Assert.That(button1.GetDataContext(), Is.EqualTo("test"));
                Assert.That(button2.GetDataContext(), Is.EqualTo("test"));
                Assert.That(button3.GetDataContext(), Is.EqualTo("button3"));
                Assert.That(button1.HasLocalDataContext(), Is.False);
                Assert.That(button2.HasLocalDataContext(), Is.False);
                Assert.That(button3.HasLocalDataContext(), Is.True);

                button1.SetDataContext("button1");
                Assert.That(button1.GetDataContext(), Is.EqualTo("button1"));
                button2.SetDataContext(null);
                Assert.That(button2.GetDataContext(), Is.Null);
                Assert.That(button1.HasLocalDataContext(), Is.True);
                Assert.That(button2.HasLocalDataContext(), Is.True);

                button3.ClearDataContext();
                Assert.That(button3.GetDataContext(), Is.EqualTo("test"));
                Assert.That(button3.HasLocalDataContext(), Is.False);
           }
        }
        [Test]
        public void ClearDataContextWhenThereIsNoInheritedValueFromParent() {
            using(var form = new Form()) {
                var button1 = new Button();
                button1.SetDataContext("button3");
                form.Controls.Add(button1);
                button1.ClearDataContext();
                Assert.That(button1.GetDataContext(), Is.Null);
                Assert.That(button1.HasLocalDataContext(), Is.False);
                Assert.That(form.HasLocalDataContext(), Is.False);
            }
        }
        [Test]
        public void ChangeInheritedDataContext() {
            using(var form = new Form()) {
                var button1 = new Button();
                var button2 = new Button();
                form.Controls.Add(button1);
                form.Controls.Add(button2);
                form.SetDataContext("test");
                Assert.That(button1.GetDataContext(), Is.EqualTo("test"));
                Assert.That(button1.HasLocalDataContext(), Is.False);
                form.SetDataContext("test2");
                Assert.That(button1.GetDataContext(), Is.EqualTo("test2"));
                Assert.That(button1.HasLocalDataContext(), Is.False);

                button2.SetDataContext("button2");

                form.ClearDataContext();
                Assert.That(button1.GetDataContext(), Is.Null);
                Assert.That(button1.HasLocalDataContext(), Is.False);
                Assert.That(button2.GetDataContext(), Is.EqualTo("button2"));
                Assert.That(button2.HasLocalDataContext(), Is.True);
            }
        }
        [Test]
        public void MultiLevelDataContextInheritance1() {
            using(var form = new Form()) {
                var panel = new Panel();
                var button1 = new Button();
                var button2 = new Button();
                panel.Controls.Add(button1);
                panel.Controls.Add(button2);
                form.Controls.Add(panel);
                form.SetDataContext("test");

                Assert.That(button1.GetDataContext(), Is.EqualTo("test"));
                Assert.That(button1.HasLocalDataContext(), Is.False);
                form.SetDataContext("test2");
                Assert.That(button1.GetDataContext(), Is.EqualTo("test2"));
                Assert.That(button1.HasLocalDataContext(), Is.False);

                button2.SetDataContext("button2");

                form.ClearDataContext();
                Assert.That(button1.GetDataContext(), Is.Null);
                Assert.That(button1.HasLocalDataContext(), Is.False);
                Assert.That(button2.GetDataContext(), Is.EqualTo("button2"));
                Assert.That(button2.HasLocalDataContext(), Is.True);

                form.SetDataContext("test2");
                Assert.That(button1.GetDataContext(), Is.EqualTo("test2"));
                Assert.That(button2.GetDataContext(), Is.EqualTo("button2"));
                Assert.That(button2.HasLocalDataContext(), Is.True);
                Assert.That(button1.HasLocalDataContext(), Is.False);

                panel.SetDataContext("panel");
                Assert.That(button1.GetDataContext(), Is.EqualTo("panel"));
                Assert.That(button1.HasLocalDataContext(), Is.False);
                Assert.That(button2.GetDataContext(), Is.EqualTo("button2"));
                Assert.That(button2.HasLocalDataContext(), Is.True);

                form.ClearDataContext();
                Assert.That(panel.GetDataContext(), Is.EqualTo("panel"));
                Assert.That(panel.HasLocalDataContext(), Is.True);
                Assert.That(button1.GetDataContext(), Is.EqualTo("panel"));
                Assert.That(button1.HasLocalDataContext(), Is.False);
                Assert.That(button2.GetDataContext(), Is.EqualTo("button2"));
                Assert.That(button2.HasLocalDataContext(), Is.True);

                form.SetDataContext("test2");
                Assert.That(panel.GetDataContext(), Is.EqualTo("panel"));
                Assert.That(panel.HasLocalDataContext(), Is.True);
                Assert.That(button1.GetDataContext(), Is.EqualTo("panel"));
                Assert.That(button1.HasLocalDataContext(), Is.False);
                Assert.That(button2.GetDataContext(), Is.EqualTo("button2"));
                Assert.That(button2.HasLocalDataContext(), Is.True);

                panel.ClearDataContext();
                Assert.That(panel.GetDataContext(), Is.EqualTo("test2"));
                Assert.That(panel.HasLocalDataContext(), Is.False);
                Assert.That(button1.GetDataContext(), Is.EqualTo("test2"));
                Assert.That(button1.HasLocalDataContext(), Is.False);
                Assert.That(button2.GetDataContext(), Is.EqualTo("button2"));
                Assert.That(button2.HasLocalDataContext(), Is.True);
            }
        }
        [Test]
        public void MultiLevelDataContextInheritance2() {
            using(var form = new Form()) {
                var panel = new Panel();
                var button = new Button();
                panel.Controls.Add(button);
                form.SetDataContext("test");
                form.Controls.Add(panel);

                Assert.That(button.GetDataContext(), Is.EqualTo("test"));
                Assert.That(button.HasLocalDataContext(), Is.False);
                form.SetDataContext("test2");
                Assert.That(button.GetDataContext(), Is.EqualTo("test2"));
                Assert.That(button.HasLocalDataContext(), Is.False);

                panel.SetDataContext("panel");
                Assert.That(button.GetDataContext(), Is.EqualTo("panel"));
                Assert.That(button.HasLocalDataContext(), Is.False);
            }
        }
    }
}