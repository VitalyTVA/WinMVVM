using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinMVVM.Tests {
    [TestFixture]
    public class WeakReferenceComparerTests {
        [Test]
        public void Equals() { 
            Assert.That(WeakReferenceComparer.Instance.Equals(GetDeadWeakReference(), GetDeadWeakReference()), Is.False);
            Assert.That(WeakReferenceComparer.Instance.Equals(GetDeadWeakReference(), new WeakReference("test")), Is.False);
            Assert.That(WeakReferenceComparer.Instance.Equals(new WeakReference("test"), GetDeadWeakReference()), Is.False);
            Assert.That(WeakReferenceComparer.Instance.Equals(new WeakReference("test"), new WeakReference("test1")), Is.False);
            Assert.That(WeakReferenceComparer.Instance.Equals(new WeakReference("test"), new WeakReference("test")), Is.True);
        }
        [Test]
        public void GetHashCode() { 
            string s = "test";
            WeakReference reference = new WeakReference(s);
            Assert.That(WeakReferenceComparer.Instance.GetHashCode(reference), Is.EqualTo(s.GetHashCode()));
            Assert.Throws<ArgumentException>(() => WeakReferenceComparer.Instance.GetHashCode(GetDeadWeakReference()));
        }
        WeakReference GetDeadWeakReference() {
            WeakReference reference = CreateWeakReference();
            TestUtils.GarbageCollect();
            return reference;
        }
        private static WeakReference CreateWeakReference() {
            return new WeakReference(new object());
        }
    }
}
