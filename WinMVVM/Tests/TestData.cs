using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Input;
using WinMVVM.Utils;

namespace WinMVVM.Tests {
    public class TestData {
        public int IntProperty { get; set; }
        public string TextProperty { get; set; }
        public TestData(int intProperty, string textProperty) {
            IntProperty = intProperty;
            TextProperty = textProperty;
        }
    }
}
