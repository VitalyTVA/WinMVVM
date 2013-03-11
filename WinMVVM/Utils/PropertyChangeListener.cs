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
    interface IPropertyChangeListener {
        void Clear();
    }
    class PropertyChangeListener<T> : DependencyObject, IPropertyChangeListener {
        public static readonly DependencyProperty FakeProperty = DependencyProperty.Register("Fake", typeof(T), typeof(PropertyChangeListener<T>), new WpfPropertyMetadata(default(T), (d, e) => ((PropertyChangeListener<T>)d).OnFakeChanged()));
        readonly Action<object> changedCallback;
        public static PropertyChangeListener<T> Create(WpfBinding binding, Action<object> changedCallback, T initialValue) {
            return new PropertyChangeListener<T>(binding, changedCallback, initialValue);
        }
        PropertyChangeListener(WpfBinding binding, Action<object> changedCallback, T initialValue) {
            Fake = initialValue;
            this.changedCallback = changedCallback;
            WpfBindingOperations.SetBinding(this, FakeProperty, binding);
        }
        public T Fake {
            get { return (T)GetValue(FakeProperty); }
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
