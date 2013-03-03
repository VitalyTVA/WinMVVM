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
        public PropertyMetadata(PropertyMetadataOptions options) {
            Options = options;
        }
        public PropertyMetadataOptions Options { get; private set; }
    }
}
