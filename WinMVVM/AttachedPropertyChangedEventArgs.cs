using System;
using System.Collections.Generic;
using System.Linq;

namespace WinMVVM {
    public class AttachedPropertyChangedEventArgs<T> : EventArgs {
        public AttachedPropertyChangedEventArgs(T oldValue, T newValue) {
            NewValue = newValue;
            OldValue = oldValue;
        }
        public T OldValue { get; private set; }
        public T NewValue { get; private set; }
    }
}
