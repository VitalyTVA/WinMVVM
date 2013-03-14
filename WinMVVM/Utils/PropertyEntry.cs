using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Forms;
using WinMVVM.Utils;
using WpfBinding = System.Windows.Data.Binding;
namespace WinMVVM.Utils {
    interface IPropertyEntry {
        event EventHandler Changed;
    }
    class PropertyEntry<T> : INotifyPropertyChanged, IPropertyEntry {
        private bool isValueSet;
        static readonly PropertyChangedEventArgs Args = new PropertyChangedEventArgs("PropertyValue");
        private Control Control { get { return (Control)controlReference.Target; } }
        private readonly WeakReference controlReference;
        readonly AttachedProperty<T> property;
        private T propertyValue;
        public T PropertyValue {
            get {
                return IsValueSet ? propertyValue : property.Metadata.DefaultValue;
            }
            set {
                if(IsValueSet && object.Equals(propertyValue, value))
                    return;
                T oldValue = PropertyValue;
                isValueSet = true;
                propertyValue = value;
                if(property.Inherits) {
                    foreach(Control child in Control.Controls) {
                        UpdateChildValue(child);
                    }
                }
                NotifyPropertyValueChanged(oldValue);
            }
        }
        public bool IsValueSet {
            get { return isValueSet; }
            set {
                if(isValueSet == value)
                    return;
                T oldValue = PropertyValue;
                isValueSet = value;
                NotifyPropertyValueChanged(oldValue);
            }
        }
        public bool IsLocalValue { get; set; }
        Dictionary<BindingExpressionKey, BindingExpression> expressions = new Dictionary<BindingExpressionKey, BindingExpression>();
        public event PropertyChangedEventHandler PropertyChanged;
        public event EventHandler Changed;
#if DEBUG
        public int ChangedSubscribeCount { get { return Changed != null ? Changed.GetInvocationList().Count() : 0; } }
#endif

        public PropertyEntry(AttachedProperty<T> property, WeakReference controlReference) {
            this.property = property;
            this.controlReference = controlReference;
            //TODO - unsubscribe
            //TODO - weak subscription??
            if(property.Inherits) {
                Control.ControlAdded += OnControlAdded;
                Control.ControlRemoved += OnControlRemoved;
            }
        }
        void NotifyPropertyValueChanged(T oldValue) {
            if(property.Metadata.Callback != null)
                property.Metadata.Callback(Control, new AttachedPropertyChangedEventArgs<T>(oldValue, PropertyValue));
            if(PropertyChanged != null)
                PropertyChanged(this, Args);
            if(Changed != null)
                Changed(this, EventArgs.Empty);
        }
        public void AddBinding(BindingExpressionKey key, BindingBase binding, Func<BindingExpression> createExpression) {
            BindingExpression existingExpression;
            if(expressions.TryGetValue(key, out existingExpression)) {
                if(!object.Equals(binding, existingExpression.Binding))
                    existingExpression.Clear();
                else
                    return;
            }
            expressions[key] = createExpression();
        }
        public BindingExpression RemoveBinding(BindingExpressionKey key) {
            BindingExpression result;
            if(expressions.TryGetValue(key, out result)) {
                expressions.Remove(key);
            }
            return result;
        }
        void OnControlAdded(object sender, ControlEventArgs e) {
            ValidateChildAccess();
            UpdateChildValue(e.Control);
        }
        void OnControlRemoved(object sender, ControlEventArgs e) {
            ValidateChildAccess();
            ClearChildValue(e.Control);
        }
        public void UpdateChildValue(Control child) {
            ValidateChildAccess();
            property.SetValueCore(child, PropertyValue, false);
        }
        void ClearChildValue(Control child) {
            ValidateChildAccess();
            property.ClearValueCore(child, false);
        }
        public void ClearChildrenValue() {
            ValidateChildAccess();
            foreach(Control child in Control.Controls) {
                ClearChildValue(child);
            }
        }
        public bool CanChangeValue(bool isLocalValue) {
            return isLocalValue || !IsLocalValue;
        }
        public void ClearValue() {
            IsValueSet = false;
            IsLocalValue = false;
            propertyValue = default(T);
        }
        void ValidateChildAccess() {
            if(!property.Inherits)
                Guard.InvalidOperation();
        }
    }
}
