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
    public class AttachedProperty {
        public static AttachedProperty Register(Expression<Func<AttachedProperty>> property) {
            MemberInfo mi = ExpressionHelper.GetMemberInfo(property, MemberTypes.Field);
            //TODO check string not empty, check existing properties
            const string Suffix = "Property";
            if(!mi.Name.EndsWith(Suffix)) {
                Guard.ArgumentException("property");
            }
            int index = mi.Name.Length - Suffix.Length;
            return Register(mi.Name.Remove(index, Suffix.Length), mi.DeclaringType);
        }
        public static AttachedProperty Register(string name, Type ownerType) {
            Guard.ArgumentInRange(!string.IsNullOrEmpty(name), "name");
            Guard.ArgumentNotNull(ownerType, "ownerType");
            return new AttachedProperty(name, ownerType);
        }

        public AttachedProperty(string name, Type ownerType) {
            Name = name;
            OwnerType = ownerType;
        }
        public Type OwnerType { get; private set; }
        public string Name { get; private set; }
    }
    public static class AttachedPropertyExtensions {
        public static object GetValue(this Control control, AttachedProperty property) {
            return null;
        }
        public static void SetValue(this Control control, AttachedProperty property, object value) {
        }
    }
    //TODO - generic attached property
}
