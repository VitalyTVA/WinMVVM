using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using WinMVVM.Utils;

namespace WinMVVM {
    public class Binding : BindingBase {
        public Binding() //TODO this is for serialization, how can we test it? (may just chack constructor exists)
            : this((string)null) {
        }
        public Binding(string path, BindingMode mode) {
            Mode = mode;
            Path = path;
        }
        public Binding(string path) 
            : this(path, BindingMode.OneWay) {
        }
        public Binding(Expression<Func<object>> expression, BindingMode mode = BindingMode.OneWay)
            : this(ExpressionHelper.GetPropertyPath(expression), mode) {

        }

        public string Path { get; private set; }
        public BindingMode Mode { get; private set; }

        public override bool Equals(object obj) {
            Binding other = obj as Binding;
            return other != null && other.Path == Path && other.Mode == Mode;
        }
        public override int GetHashCode() {
            return (Path != null ? Path.GetHashCode() : base.GetHashCode()) ^ Mode.GetHashCode();
        }
    }
}
