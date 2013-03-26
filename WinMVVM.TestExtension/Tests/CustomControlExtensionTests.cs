using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using WinMVVM.Features;

namespace WinMVVM.TestExtension.Tests {
    [TestFixture]
    public class CustomControlExtensionTests {
        [Test]
        public void Register() {
            using (var customControl = new CustomControl()) {
                var customControlItemsSourceFeature = FeatureProvider<IItemsSourceFeature>.GetFeature(customControl) as CustomControlItemsSourceFeature;
                Assert.That(customControlItemsSourceFeature, Is.Not.Null);
            }
        }
    }
}
