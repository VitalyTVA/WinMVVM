using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Windows;
using System.Windows.Forms;
using WinMVVM.Utils;

namespace WinMVVM {
    public sealed class PropertyMetadata {
        public PropertyMetadata(object defaultValue = null, PropertyMetadataOptions options = PropertyMetadataOptions.None) {
            DefaultValue = defaultValue;
            Options = options;
        }
        public PropertyMetadataOptions Options { get; private set; }
        public object DefaultValue { get; private set; }
    }
}
