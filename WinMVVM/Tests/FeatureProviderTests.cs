using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Linq.Expressions;
using WinMVVM.Utils;
using NUnit.Framework;
using WinMVVM.Features;
using System.Windows.Forms;

namespace WinMVVM.Tests {
    [TestFixture]
    public class FeatureProviderTests {
        interface ITestFeature : IFeature {
            
        }
        public class TestFeature : ITestFeature {
            public static int InstanceCount { get; private set; }
            public TestFeature() {
                InstanceCount++;
                Type = typeof(Button);
            }
            public Type Type { get; set; }
            bool IFeature.IsSupportedControl(System.Windows.Forms.Control control) {
                return Type.IsAssignableFrom(control.GetType());
            }
        }
        [Test]
        public void RegisterOnceAndLazyCreation() {
            FeatureProvider<ITestFeature>.ThrowOnDuplicateRegistration = false;
            FeatureProvider<ITestFeature>.RegisterFeature<TestFeature>();
            Assert.That(TestFeature.InstanceCount, Is.EqualTo(0));
            using(var button = new Button()) {
                var testFeature = (TestFeature)FeatureProvider<ITestFeature>.GetFeature(button);
                Assert.That(TestFeature.InstanceCount, Is.EqualTo(1));
                Assert.That(testFeature, Is.Not.Null);
                testFeature.Type = typeof(ListBox);
                Assert.That(FeatureProvider<ITestFeature>.GetFeature(button), Is.Null);
                Assert.That(TestFeature.InstanceCount, Is.EqualTo(1));
                FeatureProvider<ITestFeature>.RegisterFeature<TestFeature>();
                Assert.That(FeatureProvider<ITestFeature>.GetFeature(button), Is.Null);
                Assert.That(TestFeature.InstanceCount, Is.EqualTo(1));
            }
        }
    }
}
