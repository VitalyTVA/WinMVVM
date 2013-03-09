using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.ComponentModel.Design.Serialization;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Windows;
using System.Windows.Forms;
using WinMVVM.Utils;

namespace WinMVVM {
    [DesignerSerializer(SR.BindingManagerSerializerAssemblyQualifiedName, SR.CodeDomSerializerAssemblyQualifiedName)]
    [Designer(SR.BindingManagerDesignerAssemblyQualifiedName)]
    public class BindingManager : Component, ISupportInitialize {
        static MethodInfo setAttachedPropertyBindingMethodInfo;
        static MethodInfo clearAttachedPropertyBindingMethodInfo;
        class SetBindingActionCollection : Collection<SetBindingAction> { 
        }

        SetBindingActionCollection Actions { get; set; }
        public BindingManager() {
            Actions = new SetBindingActionCollection();
        }
        void ISupportInitialize.BeginInit() {
        }
        void ISupportInitialize.EndInit() {
        }
        public void Remove(Control control, string propertyName) {
            RemoveCore(control, StandardPropertyDescriptor.FromPropertyName(control, propertyName));
        }
        public void SetBinding(Control control, string propertyName, BindingBase binding) {
            BindingOperations.SetBinding(control, propertyName, binding);
            //TODO  all SetBinding variants and test it
            //TODO  do all this only in design time
            this.AddOrReplace(control, StandardPropertyDescriptor.FromPropertyName(control, propertyName), (Binding)binding);
        }
        public void ClearBinding(Control control, AttachedPropertyBase property) {
            if(clearAttachedPropertyBindingMethodInfo == null) {
                clearAttachedPropertyBindingMethodInfo = typeof(BindingOperations).GetMethods(BindingFlags.Public | BindingFlags.Static).
                    First(x => x.Name == "ClearBinding" && x.GetParameters().ElementAt(1).ParameterType.Name == typeof(AttachedProperty<>).Name); //TODO use lambda
            }
            MethodInfo clearBindingActionMethod = clearAttachedPropertyBindingMethodInfo.MakeGenericMethod(property.PropertyType);
            clearBindingActionMethod.Invoke(null, new object[] { control, property });

            this.RemoveCore(control, GetPropertyDescriptor(property));
        }
        public void SetBinding(Control control, AttachedPropertyBase property, BindingBase binding) {
            if(setAttachedPropertyBindingMethodInfo == null) {
                setAttachedPropertyBindingMethodInfo = typeof(BindingOperations).GetMethods(BindingFlags.Public | BindingFlags.Static).
                    First(x => x.Name == "SetBinding" && x.GetParameters().ElementAt(1).ParameterType.Name == typeof(AttachedProperty<>).Name); //TODO use lambda
            }
            MethodInfo setBindingActionMethod = setAttachedPropertyBindingMethodInfo.MakeGenericMethod(property.PropertyType);
            setBindingActionMethod.Invoke(null, new object[] { control, property, binding });

            this.AddOrReplace(control, GetPropertyDescriptor(property), (Binding)binding);
        }
        public void ClearAllBindings(Control control) {
            Guard.ArgumentNotNull(control, "control");
            foreach(SetBindingAction action in GetActions().Where(action => object.Equals(action.Control, control)).ToArray()) {
                RemoveCore(action.Control, action.Property);
            }
        }

        static PropertyDescriptorBase GetPropertyDescriptor(AttachedPropertyBase property) {
            MethodInfo createPropertyMethod = typeof(AttachedPropertyDescriptor<>).MakeGenericType(property.PropertyType).GetMethod("FromAttachedProperty", BindingFlags.Public | BindingFlags.Static); //TODO use lambda
            return (PropertyDescriptorBase)createPropertyMethod.Invoke(null, new[] { property });
        }
        void RemoveCore(Control control, PropertyDescriptorBase property) {
            BindingOperations.ClearBindingCore(control, property);
            Find(control, property).Do(x => Actions.Remove(x));
        }
        internal SetBindingAction Find(Control control, PropertyDescriptorBase property) {
            Guard.ArgumentNotNull(control, "control");
            Guard.ArgumentNotNull(property, "property");
            return Actions.FirstOrDefault(x => x.IsMatchedAction(control, property));
        }

        internal IEnumerable<SetBindingAction> GetActions() {
            return Actions;
        }

        void AddOrReplace(Control control, PropertyDescriptorBase property, Binding binding) {
            SetBindingAction newAction = new SetBindingAction(control, property, binding);
            int index = -1;
            SetBindingAction oldAction = Find(control, property);
            if(oldAction != null)
                index = Actions.IndexOf(oldAction);
            if(index >= 0)
                Actions[index] = newAction;
            else
                Actions.Add(newAction);
        }
    }
}