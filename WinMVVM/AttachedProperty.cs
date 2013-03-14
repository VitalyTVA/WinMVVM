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
    public sealed class AttachedProperty<T> : AttachedPropertyBase {
        public static AttachedProperty<T> Register(Expression<Func<AttachedProperty<T>>> property, PropertyMetadata<T> metadata = null) {
            MemberInfo mi = ExpressionHelper.GetMemberInfo(property, MemberTypes.Field);
            //TODO check existing properties
            const string Suffix = "Property";
            if(!mi.Name.EndsWith(Suffix)) {
                Guard.ArgumentException("property");
            }
            int index = mi.Name.Length - Suffix.Length;
            return Register(mi.Name.Remove(index, Suffix.Length), mi.DeclaringType, metadata);
        }
        public static AttachedProperty<T> Register(string name, Type ownerType, PropertyMetadata<T> metadata = null) {
            Guard.ArgumentInRange(!string.IsNullOrEmpty(name), "name");
            Guard.ArgumentNotNull(ownerType, "ownerType");
            return new AttachedProperty<T>(name, ownerType, metadata ?? new PropertyMetadata<T>());
        }

        public AttachedProperty(string name, Type ownerType, PropertyMetadata<T> metadata) {
            Metadata = metadata;
            this.name = name;
            this.ownerType = ownerType;
        }
        public override Type PropertyType { get { return typeof(T); } }
        private string name;
        readonly Type ownerType;
        public override Type OwnerType { get { return ownerType; } }
        public override string Name { get { return name; } }
        public PropertyMetadata<T> Metadata { get; private set; }
        internal bool Inherits { get { return (Metadata.Options & PropertyMetadataOptions.Inherits) != 0; } }

        readonly Dictionary<WeakReference, PropertyEntry<T>> dictionary = new Dictionary<WeakReference, PropertyEntry<T>>(WeakReferenceComparer.Instance);
        internal T GetValue(Control control) {
            Guard.ArgumentNotNull(control, "control");
            PropertyEntry<T> result;
            return GetPropertyEntryCore(control, out result) ? result.PropertyValue : Metadata.DefaultValue;
        }
        internal void SetValue(Control control, T value) {
            SetValueCore(control, value, true);
        }
        internal bool HasLocalValue(Control control) {
            Guard.ArgumentNotNull(control, "control");
            PropertyEntry<T> entry;
            if(!GetPropertyEntryCore(control, out entry))
                return false;
            return entry.IsValueSet && entry.IsLocalValue;
        }
        internal override void ClearValue(Control control) {
            Guard.ArgumentNotNull(control, "control");
            ClearValueCore(control, true);
            if(Inherits && control.Parent != null) {
                PropertyEntry<T> parentEntry;
                if(GetPropertyEntryCore(control.Parent, out parentEntry))
                    parentEntry.UpdateChildValue(control);
            }
        }
        internal void ClearValueCore(Control control, bool isLocalValue) {
            PropertyEntry<T> entry;
            if(GetPropertyEntryCore(control, out entry)) {
                if(entry.CanChangeValue(isLocalValue)) {
                    entry.ClearValue();
                    if(Inherits)
                        entry.ClearChildrenValue();
                }
            }
        }
        internal void SetValueCore(Control control, T value, bool isLocalValue) {
            Guard.ArgumentNotNull(control, "control");
            PropertyEntry<T> entry = GetPropertyEntry(control);
            if(entry.CanChangeValue(isLocalValue)) {
                entry.IsLocalValue = isLocalValue;
                entry.PropertyValue = value;
            }
        }
        internal PropertyEntry<T> GetPropertyEntry(Control control) {
            PropertyEntry<T> entry;
            if(!GetPropertyEntryCore(control, out entry)) {
                dictionary[GetWeakReference(control)] = (entry = new PropertyEntry<T>(this, new WeakReference(control)));
            }
            return entry;
        }
        bool GetPropertyEntryCore(Control control, out PropertyEntry<T> result) {
            return dictionary.TryGetValue(GetWeakReference(control), out result);
        }
        private static WeakReference GetWeakReference(Control control) {
            return new WeakReference(control);
        }


        internal override object GetValueInternal(Control control) {
            return GetValue(control);
        }

        internal override void SetValueInternal(Control control, object value) {
            SetValue(control, (T)value);
        }

        internal override INotifyPropertyChanged GetTrackableEntry(Control control) {
            return GetPropertyEntry(control);
        }
    }
    //TODO - generic attached property
}
