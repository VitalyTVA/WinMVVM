using System;
using System.Collections.Generic;
using System.Linq;
using WinMVVM.Utils;

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
            Guard.ArgumentInRange(reference.IsAlive, "reference");
            return reference.Target.GetHashCode();
        }
    }
}
