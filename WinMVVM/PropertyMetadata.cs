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
    public sealed class PropertyMetadata<T> {
        public PropertyMetadata(T defaultValue = default(T), PropertyMetadataOptions options = PropertyMetadataOptions.None) {
            DefaultValue = defaultValue;
            Options = options;
        }
        public PropertyMetadataOptions Options { get; private set; }
        public T DefaultValue { get; private set; }
    }
}
