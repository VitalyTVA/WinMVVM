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
            }

            using(var button2 = new Button()) {
                Assert.That(button2.GetDataContext(), Is.Null);
                button2.SetDataContext(13);
                Assert.That(button2.GetDataContext(), Is.EqualTo(13));
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

                //button3.ClearDataContext(); //TODO
                //Assert.That(button3.GetDataContext(), Is.EqualTo("test"));
                //Assert.That(button3.HasLocalDataContext(), Is.False);

                button1.SetDataContext("button1");
                Assert.That(button1.GetDataContext(), Is.EqualTo("button1"));
                button2.SetDataContext(null);
                Assert.That(button2.GetDataContext(), Is.Null);
                Assert.That(button1.HasLocalDataContext(), Is.True);
                Assert.That(button2.HasLocalDataContext(), Is.True);
           }
        }
        [Test]
        public void ChangeInheritedDataContext() {
            using(var form = new Form()) {
                var button1 = new Button();
                var button2 = new Button();
                form.Controls.Add(button1);
                form.SetDataContext("test");
                Assert.That(button1.GetDataContext(), Is.EqualTo("test"));
                Assert.That(button1.HasLocalDataContext(), Is.False);
                form.SetDataContext("test2");
                Assert.That(button1.GetDataContext(), Is.EqualTo("test2"));
                Assert.That(button1.HasLocalDataContext(), Is.False);
            }
        }
    }
}