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
    public sealed class AttachedProperty {
        public static AttachedProperty Register(Expression<Func<AttachedProperty>> property, PropertyMetadata metadata = null) {
            MemberInfo mi = ExpressionHelper.GetMemberInfo(property, MemberTypes.Field);
            //TODO check string not empty, check existing properties
            const string Suffix = "Property";
            if(!mi.Name.EndsWith(Suffix)) {
                Guard.ArgumentException("property");
            }
            int index = mi.Name.Length - Suffix.Length;
            return Register(mi.Name.Remove(index, Suffix.Length), mi.DeclaringType, metadata);
        }
        public static AttachedProperty Register(string name, Type ownerType, PropertyMetadata metadata = null) {
            Guard.ArgumentInRange(!string.IsNullOrEmpty(name), "name");
            Guard.ArgumentNotNull(ownerType, "ownerType");
            return new AttachedProperty(name, ownerType, metadata ?? new PropertyMetadata(PropertyMetadataOptions.None));
        }

        public AttachedProperty(string name, Type ownerType, PropertyMetadata metadata) {
            Metadata = metadata;
            Name = name;
            OwnerType = ownerType;
        }
        public Type OwnerType { get; private set; }
        public string Name { get; private set; }
        public PropertyMetadata Metadata { get; private set; }
        internal bool Inherits { get { return (Metadata.Options & PropertyMetadataOptions.Inherits) != 0; } }

        readonly Dictionary<WeakReference, PropertyEntry> dictionary = new Dictionary<WeakReference, PropertyEntry>(WeakReferenceComparer.Instance);
        internal object GetValue(Control control) {
            Guard.ArgumentNotNull(control, "control");
            PropertyEntry result;
            return GetPropertyEntryCore(control, out result) ? result.PropertyValue : null;
        }
        internal void SetValue(Control control, object value) {
            SetValueCore(control, value, true);
        }
        internal bool HasLocalValue(Control control) {
            Guard.ArgumentNotNull(control, "control");
            PropertyEntry entry;
            if(!GetPropertyEntryCore(control, out entry))
                return false;
            return entry.IsValueSet && entry.IsLocalValue;
        }
        internal void ClearValue(Control control) {
            Guard.ArgumentNotNull(control, "control");
            ClearValueCore(control, true);
            if(Inherits && control.Parent != null) {
                PropertyEntry parentEntry;
                if(GetPropertyEntryCore(control.Parent, out parentEntry))
                    parentEntry.UpdateChildValue(control);
            }
        }
        internal void ClearValueCore(Control control, bool isLocalValue) {
            PropertyEntry entry;
            if(GetPropertyEntryCore(control, out entry)) {
                if(entry.CanChangeValue(isLocalValue)) {
                    entry.ClearValue();
                    if(Inherits)
                        entry.ClearChildrenValue();
                }
            }
        }
        internal void SetValueCore(Control control, object value, bool isLocalValue) {
            Guard.ArgumentNotNull(control, "control");
            PropertyEntry entry = GetPropertyEntry(control);
            if(entry.CanChangeValue(isLocalValue)) {
                entry.IsLocalValue = isLocalValue;
                entry.PropertyValue = value;
            }
        }
        internal PropertyEntry GetPropertyEntry(Control control) {
            PropertyEntry entry;
            if(!GetPropertyEntryCore(control, out entry)) {
                dictionary[GetWeakReference(control)] = (entry = new PropertyEntry(this, new WeakReference(control)));
            }
            return entry;
        }
        bool GetPropertyEntryCore(Control control, out PropertyEntry result) {
            return dictionary.TryGetValue(GetWeakReference(control), out result);
        }
        private static WeakReference GetWeakReference(Control control) {
            return new WeakReference(control);
        }

    }
    //TODO - generic attached property
}
