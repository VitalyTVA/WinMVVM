using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Forms;
using WinMVVM.Utils;
using WpfBinding = System.Windows.Data.Binding;
namespace WinMVVM.Utils {
    class PropertyEntry : INotifyPropertyChanged {
        static object NotSetValue = new object();
        static readonly PropertyChangedEventArgs Args = new PropertyChangedEventArgs("PropertyValue");
        public static bool IsNotSetValue(object value) {
            return object.Equals(value, NotSetValue);
        }
        private Control Control { get { return (Control)controlReference.Target; } }
        private readonly WeakReference controlReference;
        readonly AttachedProperty property;
        private object propertyValue;
        public object PropertyValue {
            get {
                return IsValueSet ? propertyValue : null;
            }
            set {
                if(IsValueSet && propertyValue == value)
                    return;
                propertyValue = value;
                if(property.Inherits) {
                    foreach(Control child in Control.Controls) {
                        UpdateChildValue(child);
                    }
                }
                if(PropertyChanged != null)
                    PropertyChanged(this, Args);
            }
        }
        public bool IsValueSet { get { return !IsNotSetValue(propertyValue); } }
        public bool IsLocalValue { get; set; }
        Dictionary<BindingExpressionKey, BindingExpression> expressions = new Dictionary<BindingExpressionKey, BindingExpression>();
        public event PropertyChangedEventHandler PropertyChanged;

        public PropertyEntry(AttachedProperty property, WeakReference controlReference) {
            this.property = property;
            this.controlReference = controlReference;
            //TODO - unsubscribe
            //TODO - weak subscription??
            if(property.Inherits) {
                Control.ControlAdded += OnControlAdded;
                Control.ControlRemoved += OnControlRemoved;
            }
        }
        public void AddBinding(BindingExpressionKey key, BindingBase binding) {
            BindingExpression existingExpression;
            if(expressions.TryGetValue(key, out existingExpression)) {
                if(!object.Equals(binding, existingExpression.Binding))
                    existingExpression.Clear();
                else
                    return;
            }
            expressions[key] = new BindingExpression(key, binding, this);
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
            PropertyValue = NotSetValue;
            IsLocalValue = false;
        }
        void ValidateChildAccess() {
            if(!property.Inherits)
                Guard.InvalidOperation();
        }
    }
}
