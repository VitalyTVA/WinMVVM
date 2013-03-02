using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Forms;
using WinMVVM.Utils;

namespace WinMVVM {
    public static class DataContextProvider {
        internal class PropertyEntry {
            static object NotSetValue = new object();
            static readonly PropertyChangedEventArgs Args = new PropertyChangedEventArgs(string.Empty);
            private Control Control { get { return (Control)controlReference.Target; } }
            private readonly WeakReference controlReference;
            private object propertyValue;
            public object PropertyValue {
                get {
                    return IsValueSet ? propertyValue : null;
                }
                set {
                    if(IsValueSet && propertyValue == value)
                        return;
                    propertyValue = value;
                    foreach(Control child in Control.Controls) {
                        UpdateChildValue(child);
                    }
                    foreach(BindingOperations.BindingExpression expression in listeners.Keys) {
                        expression.UpdateTargetProperty(this);
                    }
                }
            }
            public bool IsValueSet { get { return !object.Equals(propertyValue, NotSetValue); } }
            public bool IsLocalValue { get; set; }
            Dictionary<BindingOperations.BindingExpression, object> listeners = new Dictionary<BindingOperations.BindingExpression, object>();

            public PropertyEntry(WeakReference controlReference) {
                this.controlReference = controlReference;
                //TODO - unsubscribe
                //TODO - weak subscription??
                Control.ControlAdded += OnControlAdded;
                Control.ControlRemoved += OnControlRemoved;
            }
            public bool AddListener(BindingOperations.BindingExpression expression) {
                if(!listeners.ContainsKey(expression)) {
                    listeners[expression] = null;
                    return true;
                }
                return false;
            }
            public void RemoveListener(BindingOperations.BindingExpression expression) {
                listeners.Remove(expression);
            }
            void OnControlAdded(object sender, ControlEventArgs e) {
                UpdateChildValue(e.Control);
            }
            void OnControlRemoved(object sender, ControlEventArgs e) {
                ClearChildValue(e.Control);
            }
            public void UpdateChildValue(Control child) {
                child.SetDataContextCore(PropertyValue, false);
            }
            void ClearChildValue(Control child) {
                child.ClearDataContextCore(false);
            }
            public void ClearChildrenDataContext() {
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
        }
        static readonly Dictionary<WeakReference, PropertyEntry> dictionary = new Dictionary<WeakReference, PropertyEntry>(WeakReferenceComparer.Instance);
        public static object GetDataContext(this Control control) {
            Guard.ArgumentNotNull(control, "control");
            PropertyEntry result;
            return GetPropertyEntryCore(control, out result) ? result.PropertyValue : null;
        }
        public static void SetDataContext(this Control control, object value) {
            SetDataContextCore(control, value, true);
        }
        public static bool HasLocalDataContext(this Control control) {
            Guard.ArgumentNotNull(control, "control");
            PropertyEntry entry;
            if(!GetPropertyEntryCore(control, out entry))
                return false;
            return entry.IsValueSet && entry.IsLocalValue;
        }
        public static void ClearDataContext(this Control control) {
            Guard.ArgumentNotNull(control, "control");
            ClearDataContextCore(control, true);
            if(control.Parent != null) {
                PropertyEntry parentEntry;
                if(GetPropertyEntryCore(control.Parent, out parentEntry))
                    parentEntry.UpdateChildValue(control);
            }
        }
        static void ClearDataContextCore(this Control control, bool isLocalValue) {
            PropertyEntry entry;
            if(GetPropertyEntryCore(control, out entry)) {
                if(entry.CanChangeValue(isLocalValue)) {
                    entry.ClearValue();
                    entry.ClearChildrenDataContext();
                    //dictionary.Remove(GetWeakReference(control));
                }
            }
        }
        internal static void SetDataContextCore(this Control control, object value, bool isLocalValue) {
            Guard.ArgumentNotNull(control, "control");
            PropertyEntry entry = GetPropertyEntry(control);
            if(entry.CanChangeValue(isLocalValue)) {
                entry.IsLocalValue = isLocalValue;
                entry.PropertyValue = value;
            }
        }
        internal static PropertyEntry GetPropertyEntry(this Control control) {
            PropertyEntry entry;
            if(!GetPropertyEntryCore(control, out entry)) {
                dictionary[GetWeakReference(control)] = (entry = new PropertyEntry(new WeakReference(control)));
            }
            return entry;
        }
        static bool GetPropertyEntryCore(Control control, out PropertyEntry result) {
            return dictionary.TryGetValue(GetWeakReference(control), out result);
        }
        private static WeakReference GetWeakReference(Control control) {
            return new WeakReference(control);
        }
    }
}