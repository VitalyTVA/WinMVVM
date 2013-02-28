using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using WinMVVM.Utils;

namespace WinMVVM {
    public static class DataContextProvider {
        public class PropertyEntry : INotifyPropertyChanged {
            static PropertyChangedEventArgs Args = new PropertyChangedEventArgs(string.Empty);
            private object propertyValue;
            public object PropertyValue {
                get {
                    return propertyValue;
                }
                set {
                    if(IsValueSet && propertyValue == value)
                        return;
                    IsValueSet = true;
                    propertyValue = value;
                    if(PropertyChanged != null)
                        PropertyChanged(this, Args);
                }
            }
            public event PropertyChangedEventHandler PropertyChanged;

            public bool IsValueSet { get; private set; }
        }
        static readonly Dictionary<WeakReference, PropertyEntry> dictionary = new Dictionary<WeakReference, PropertyEntry>(WeakReferenceComparer.Instance);
        public static object GetDataContext(this Control control) {
            Guard.ArgumentNotNull(control, "control");
            PropertyEntry result;
            if(!GetPropertyEntryCore(control, out result)) {
                if(control.Parent != null)
                    return GetDataContext(control.Parent);
            }
            return result != null ? result.PropertyValue : null;
        }
        public static void SetDataContext(this Control control, object value) {
            Guard.ArgumentNotNull(control, "control");
            PropertyEntry entry = GetPropertyEntry(control);
            entry.PropertyValue = value;
        }
        public static bool HasLocalDataContext(this Control control) {
            Guard.ArgumentNotNull(control, "control");
            PropertyEntry entry;
            if(!GetPropertyEntryCore(control, out entry))
                return false;
            return entry.IsValueSet;
        }
        internal static PropertyEntry GetPropertyEntry(this Control control) {
            PropertyEntry entry;
            if(!GetPropertyEntryCore(control, out entry)) {
                dictionary[GetWeakReference(control)] = (entry = new PropertyEntry());
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