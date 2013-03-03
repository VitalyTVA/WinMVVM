using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

using WpfBinding = System.Windows.Data.Binding;
using WpfBindingOperations = System.Windows.Data.BindingOperations;
using WpfPropertyMetadata = System.Windows.PropertyMetadata;

namespace WinMVVM.Utils {
    class PropertyChangeListener : DependencyObject {
        public static readonly DependencyProperty FakeProperty = DependencyProperty.Register("Fake", typeof(object), typeof(PropertyChangeListener), new WpfPropertyMetadata(null, (d, e) => ((PropertyChangeListener)d).OnFakeChanged()));
        readonly Action<object> changedCallback;
        public static PropertyChangeListener Create(WpfBinding binding, Action<object> changedCallback, object initialValue) {
            return new PropertyChangeListener(binding, changedCallback, initialValue);
        }
        PropertyChangeListener(WpfBinding binding, Action<object> changedCallback, object initialValue) {
            Fake = initialValue;
            this.changedCallback = changedCallback;
            WpfBindingOperations.SetBinding(this, FakeProperty, binding);
        }
        public object Fake {
            get { return GetValue(FakeProperty); }
            set { SetValue(FakeProperty, value); }
        }
        public void Clear() {
            WpfBindingOperations.ClearBinding(this, FakeProperty);
        }
        void OnFakeChanged() {
            if(changedCallback != null)
                changedCallback(Fake);
        }
    }
}
