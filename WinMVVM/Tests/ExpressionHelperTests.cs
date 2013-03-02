using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using WinMVVM.Utils;

namespace WinMVVM.Tests {
    [TestFixture]
    public class ExpressionHelperTests {
        public int MyProperty { get; set; }
        public int GetInt() { return 0; }
        [Test]
        public void GetPropertyName() {
            Assert.That(ExpressionHelper.GetPropertyName(() => MyProperty), Is.EqualTo("MyProperty"));
            Assert.Throws<ArgumentException>(() => { 
                ExpressionHelper.GetPropertyName(() => GetInt()); 
            });
            Assert.Throws<ArgumentNullException>(() => {
                ExpressionHelper.GetPropertyName<int>(null);
            });
        }
    }
}