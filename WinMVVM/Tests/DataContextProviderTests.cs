using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinMVVM.Tests {
    [TestFixture]
    public class DataContextProviderTests {
        [Test]
        public void SetAndGetDataContextForControl() { 
            Button button1 = new Button();
            Assert.That(button1.GetDataContext(), Is.Null);
            button1.SetDataContext("test");
            Assert.That(button1.GetDataContext(), Is.EqualTo("test"));

            Button button2 = new Button();
            Assert.That(button2.GetDataContext(), Is.Null);
            button2.SetDataContext(13);
            Assert.That(button2.GetDataContext(), Is.EqualTo(13));
        }
        [Test]
        public void CollectControlWithDataContextSet() {
            var reference = DoCollectControlWithDataContextSet();
            TestUtils.GarbageCollect();
            Assert.That(reference.IsAlive, Is.False);
        }
        WeakReference DoCollectControlWithDataContextSet() {
            Button button = new Button();
            button.SetDataContext("test");
            return new WeakReference(button);
        }
        [Test]
        public void NullControl() {
            Assert.Throws<ArgumentNullException>(() => DataContextProvider.GetDataContext(null));
            Assert.Throws<ArgumentNullException>(() => DataContextProvider.SetDataContext(null, 5));
        }
    }
}