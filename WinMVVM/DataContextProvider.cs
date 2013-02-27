using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using WinMVVM.Utils;

namespace WinMVVM {
    public static class DataContextProvider {
        static readonly Dictionary<WeakReference, object> dictionary = new Dictionary<WeakReference, object>(WeakReferenceComparer.Instance);
        public static object GetDataContext(this Control control) {
            Guard.ArgumentNotNull(control, "control");
            object result;
            if(!dictionary.TryGetValue(GetWeakReference(control), out result)) {
                if(control.Parent != null)
                    return GetDataContext(control.Parent);
            }
            return result;
        }
        public static void SetDataContext(this Control control, object value) {
            Guard.ArgumentNotNull(control, "control");
            dictionary[GetWeakReference(control)] = value;
        }
        public static bool HasLocalDataContext(this Control control) {
            Guard.ArgumentNotNull(control, "control");
            return dictionary.ContainsKey(GetWeakReference(control));
        }
        private static WeakReference GetWeakReference(Control control) {
            return new WeakReference(control);
        }
    }
}