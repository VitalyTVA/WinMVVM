using System;
using System.Collections.Generic;
using System.Linq;

namespace WinMVVM {
    public class WeakReferenceComparer : IEqualityComparer<WeakReference> {
        public static readonly IEqualityComparer<WeakReference> Instance = new WeakReferenceComparer();
        WeakReferenceComparer() { }
        bool IEqualityComparer<WeakReference>.Equals(WeakReference x, WeakReference y) {
            if(!x.IsAlive || !y.IsAlive)
                return false;
            return object.Equals(x.Target, y.Target);
        }
        int IEqualityComparer<WeakReference>.GetHashCode(WeakReference reference) {
            if(!reference.IsAlive)
                throw new ArgumentException("reference");
            return reference.Target.GetHashCode();
        }
    }
}
