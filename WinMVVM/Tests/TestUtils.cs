using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace WinMVVM.Tests {
    static class TestUtils {
        public static void GarbageCollect() {
            for(int i = 0; i < 10; i++) {
                GC.GetTotalMemory(true);
                GC.WaitForPendingFinalizers();
            }
        }
    }
    [TestFixture]
    public class ApiTests {
        [Test]
        public void PublicClassesOnlyInRootNameSpace() {
            foreach(Type type in Assembly.GetExecutingAssembly().GetTypes()) {
                Assert.That(type.Namespace.StartsWith("WinMVVM"), Is.True);
                if(type.Namespace == "WinMVVM") {
                    if(type.IsNested)
                        Assert.That(type.IsPublic, Is.False);
                    else
                        Assert.That(type.IsPublic, Is.True);
                } else if(!type.Namespace.StartsWith("WinMVVM.Tests")) {
                    Assert.That(type.IsPublic, Is.False);
                }
            }
            
        }
    }
}
