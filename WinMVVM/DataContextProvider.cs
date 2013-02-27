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
            if(dictionary.TryGetValue(new WeakReference(control), out result))
                return result;
            return null;
        }
        public static void SetDataContext(this Control control, object value) {
            Guard.ArgumentNotNull(control, "control");
            dictionary[new WeakReference(control)] = value;
        }
    }
}