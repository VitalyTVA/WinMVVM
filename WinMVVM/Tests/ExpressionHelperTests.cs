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

            TestViewModel viewModel = null;
            Assert.That(ExpressionHelper.GetPropertyName(() => viewModel.StringProperty), Is.EqualTo("StringProperty"));

            Assert.Throws<ArgumentException>(() => {
                ExpressionHelper.GetPropertyName(() => viewModel.NestedViewModel.NestedStringProperty);
            });
        }

        [Test]
        public void GetPropertyPath() {
            Assert.That(ExpressionHelper.GetPropertyPath(() => MyProperty), Is.EqualTo("MyProperty"));
            Assert.Throws<ArgumentException>(() => {
                ExpressionHelper.GetPropertyPath(() => GetInt());
            });
            Assert.Throws<ArgumentNullException>(() => {
                ExpressionHelper.GetPropertyPath<int>(null);
            });

            TestViewModel viewModel = null;
            Assert.That(ExpressionHelper.GetPropertyPath(() => viewModel.StringProperty), Is.EqualTo("StringProperty"));
            Assert.That(ExpressionHelper.GetPropertyPath(() => viewModel.NestedViewModel.NestedStringProperty), Is.EqualTo("NestedViewModel.NestedStringProperty"));
        }
    }
}