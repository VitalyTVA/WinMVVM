using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Forms;
using WinMVVM.Utils;

namespace WinMVVM {
    public static class DataContextProvider {
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
        internal static void ClearDataContextCore(this Control control, bool isLocalValue) {
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