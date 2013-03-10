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
        class SetBindingActionCollection : Collection<BindingManagerActionBase> { 
        }

        SetBindingActionCollection Actions { get; set; }
        public BindingManager() {
            Actions = new SetBindingActionCollection();
        }
        void ISupportInitialize.BeginInit() {
        }
        void ISupportInitialize.EndInit() {
        }

        public void SetValue(Control control, AttachedPropertyBase property, object value) {
            ValidateAttachedPropertyArguments(control, property);
            property.SetValueInternal(control, value);
            AddAction(new SetValueAction(control, AttachedPropertyDescriptor.FromAttachedProperty(property), value));
        }
        public void ClearValue(Control control, AttachedPropertyBase property) {
            ValidateAttachedPropertyArguments(control, property);
            property.ClearValue(control);
            RemoveAction(control, AttachedPropertyDescriptor.FromAttachedProperty(property));
        }

        public void ClearBinding(Control control, string propertyName) {
            ClearBindingCore(control, StandardPropertyDescriptor.FromPropertyName(control, propertyName));
        }
        public void SetBinding(Control control, string propertyName, BindingBase binding) {
            BindingOperations.SetBinding(control, propertyName, binding);
            //TODO  all SetBinding variants and test it
            //TODO  do all this only in design time
            this.SetBindingCore(control, StandardPropertyDescriptor.FromPropertyName(control, propertyName), (Binding)binding);
        }
        public void ClearBinding(Control control, AttachedPropertyBase property) {
            ValidateAttachedPropertyArguments(control, property);
            if(clearAttachedPropertyBindingMethodInfo == null) {
                clearAttachedPropertyBindingMethodInfo = typeof(BindingOperations).GetMethods(BindingFlags.Public | BindingFlags.Static).
                    First(x => x.Name == "ClearBinding" && x.GetParameters().ElementAt(1).ParameterType.Name == typeof(AttachedProperty<>).Name); //TODO use lambda
            }
            MethodInfo clearBindingActionMethod = clearAttachedPropertyBindingMethodInfo.MakeGenericMethod(property.PropertyType);
            clearBindingActionMethod.Invoke(null, new object[] { control, property });

            this.ClearBindingCore(control, GetPropertyDescriptor(property));
        }
        public void SetBinding(Control control, AttachedPropertyBase property, BindingBase binding) {
            ValidateAttachedPropertyArguments(control, property);
            if(setAttachedPropertyBindingMethodInfo == null) {
                setAttachedPropertyBindingMethodInfo = typeof(BindingOperations).GetMethods(BindingFlags.Public | BindingFlags.Static).
                    First(x => x.Name == "SetBinding" && x.GetParameters().ElementAt(1).ParameterType.Name == typeof(AttachedProperty<>).Name); //TODO use lambda
            }
            MethodInfo setBindingActionMethod = setAttachedPropertyBindingMethodInfo.MakeGenericMethod(property.PropertyType);
            setBindingActionMethod.Invoke(null, new object[] { control, property, binding });

            this.SetBindingCore(control, GetPropertyDescriptor(property), (Binding)binding);
        }
        public void ClearAllBindings(Control control) {
            Guard.ArgumentNotNull(control, "control");
            foreach(SetBindingAction action in GetBindingActions().Where(action => object.Equals(action.Control, control)).ToArray()) {
                ClearBindingCore(action.Control, action.Property);
            }
        }

        static PropertyDescriptorBase GetPropertyDescriptor(AttachedPropertyBase property) {
            return AttachedPropertyDescriptor.FromAttachedProperty(property);
        }
        internal void ClearBindingCore(Control control, PropertyDescriptorBase property) {
            BindingOperations.ClearBindingCore(control, property);
            RemoveAction(control, property);
        }
        internal BindingManagerActionBase FindAction(Control control, PropertyDescriptorBase property) {
            Guard.ArgumentNotNull(control, "control");
            Guard.ArgumentNotNull(property, "property");
            return Actions.FirstOrDefault(x => x.IsMatchedAction(control, property));
        }

        internal IEnumerable<SetBindingAction> GetBindingActions() {
            return Actions.Where(x => x is SetBindingAction).Cast<SetBindingAction>();
        }
        internal IEnumerable<SetValueAction> GetValueActions() {
            return Actions.Where(x => x is SetValueAction).Cast<SetValueAction>();
        }

        internal void SetBindingCore(Control control, PropertyDescriptorBase property, Binding binding) {
            AddAction(new SetBindingAction(control, property, binding));
        }
        void AddAction(BindingManagerActionBase newAction) {
            int index = -1;
            BindingManagerActionBase oldAction = FindAction(newAction.Control, newAction.Property);
            if(oldAction != null)
                index = Actions.IndexOf(oldAction);
            if(index >= 0)
                Actions[index] = newAction;
            else
                Actions.Add(newAction);
        }
        void RemoveAction(Control control, PropertyDescriptorBase property) {
            FindAction(control, property).Do(x => Actions.Remove(x));
        }
        static void ValidateAttachedPropertyArguments(Control control, AttachedPropertyBase property) {
            Guard.ArgumentNotNull(control, "control");
            Guard.ArgumentNotNull(property, "property");
        }
    }
}