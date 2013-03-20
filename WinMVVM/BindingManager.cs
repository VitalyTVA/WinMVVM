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
        class ActionCollection : Collection<BindingManagerActionBase> { 
        }

        ActionCollection Actions { get; set; }
        public BindingManager() {
            Actions = new ActionCollection();
        }
        void ISupportInitialize.BeginInit() {
            Clear();
        }
        void ISupportInitialize.EndInit() {
        }

        protected override void Dispose(bool disposing) {
            if(disposing) {
                Clear();       
            }
            base.Dispose(disposing);
        }
        void Clear() {
            foreach(BindingManagerActionBase action in Actions.ToArray()) {
                action.With(x => x as SetBindingAction).Do(x => ClearBindingCore(x.Control, x.Property));
                action.With(x => x as SetValueAction).Do(x => ClearValue(x.Control, ((AttachedPropertyDescriptor)x.Property).Property));
            }
        }
        public void SetValue(Control control, AttachedPropertyBase property, object value) {
            BindingOperations.ValidateAttachedPropertyParameters(control, property);
            property.SetValueInternal(control, value);
            AddAction(new SetValueAction(control, AttachedPropertyDescriptor.FromAttachedProperty(property), value));
        }
        public void ClearValue(Control control, AttachedPropertyBase property) {
            BindingOperations.ValidateAttachedPropertyParameters(control, property);
            property.ClearValue(control);
            PropertyDescriptorBase propertyDescriptor = AttachedPropertyDescriptor.FromAttachedProperty(property);
            if(FindAction(control, propertyDescriptor) is SetBindingAction)
                return;
            RemoveAction(control, propertyDescriptor);
        }

        public void ClearBinding(Control control, string propertyName) {
            ClearBindingCore(control, StandardPropertyDescriptor.FromPropertyName(control, propertyName));
        }
        public void SetBinding(Control control, string propertyName, BindingBase binding) {
            //TODO  do all this only in design time
            var property = BindingOperations.ValidatePropertyNameParameters(control, propertyName);
            this.SetBindingCore(control, property, (Binding)binding);
        }
        public void ClearBinding(Control control, AttachedPropertyBase property) {
            BindingOperations.ValidateAttachedPropertyParameters(control, property);
            this.ClearBindingCore(control, GetPropertyDescriptor(property));
        }
        public void SetBinding(Control control, AttachedPropertyBase property, BindingBase binding) {
            BindingOperations.ValidateAttachedPropertyParameters(control, property);
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
            BindingOperations.SetBindingCore(control, property, binding);
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
    }
}