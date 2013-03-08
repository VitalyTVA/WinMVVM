using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.ComponentModel.Design.Serialization;
using System.Linq;
using System.Linq.Expressions;
using System.Windows;
using System.Windows.Forms;
using WinMVVM.Utils;

namespace WinMVVM {
    [DesignerSerializer(SR.BindingManagerSerializerAssemblyQualifiedName, SR.CodeDomSerializerAssemblyQualifiedName)]
    [Designer(SR.BindingManagerDesignerAssemblyQualifiedName)]
    public class BindingManager : Component, ISupportInitialize {
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
        public void Remove(Control control, string property) {
            BindingOperations.ClearBinding(control, property);
            Find(control, property).Do(x => Actions.Remove(x));
        }
        public SetBindingAction Find(Control control, string property) {
            Guard.ArgumentNotNull(control, "control");
            Guard.ArgumentInRange(!string.IsNullOrEmpty(property), "property");
            return Actions.FirstOrDefault(x => x.IsMatchedAction(control, property));
        }
        public void SetBinding(Control control, string propertyName, BindingBase binding) {
            BindingOperations.SetBinding(control, propertyName, binding);
            //TODO  all SetBinding variants and test it
            //TODO  do all this only in design time
            this.AddOrReplace(control, propertyName, (Binding)binding);
        }
        public void SetBinding(Control control, AttachedPropertyBase property, BindingBase binding) {
        }

        public void RemoveControlActions(Control control) {
            Guard.ArgumentNotNull(control, "control");
            foreach(SetBindingAction action in GetActions().Where(action => object.Equals(action.Control, control)).ToArray()) {
                Remove(action.Control, action.Property);
            }
        }

        internal IEnumerable<SetBindingAction> GetActions() {
            return Actions;
        }

        void AddOrReplace(Control control, string property, Binding binding) {
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