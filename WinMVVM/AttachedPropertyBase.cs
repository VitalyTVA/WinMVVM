using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Windows;
using System.Windows.Forms;
using WinMVVM.Utils;

namespace WinMVVM {
    public abstract class AttachedPropertyBase {
        public abstract Type PropertyType { get; }
        public abstract Type OwnerType { get; }
        public abstract string Name { get; }
        internal AttachedPropertyBase() { }
        internal abstract void ClearValue(Control control);
        internal abstract object GetValueInternal(Control control);
        internal abstract void SetValueInternal(Control control, object value);
        internal abstract INotifyPropertyChanged GetTrackableEntry(Control control);
    }
}
