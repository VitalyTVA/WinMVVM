using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Windows.Forms;
using WinMVVM.Utils;

namespace WinMVVM {
    [TypeConverter(typeof(BindingConverter))]
    public class Binding : BindingBase {
        public Binding() //TODO this is for serialization, how can we test it? (may just chack constructor exists)
            : this((string)null) {
        }
        public Binding(string path) {
            Path = path;
        }
        public Binding(Expression<Func<object>> expression)
            : this(ExpressionHelper.GetPropertyPath(expression)) {

        }

        public string Path { get; private set; }

        public override bool Equals(object obj) {
            Binding other = obj as Binding;
            return other != null && other.Path == Path;
        }
        public override int GetHashCode() {
            return Path != null ? Path.GetHashCode() : base.GetHashCode();
        }
    }
}
