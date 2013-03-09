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
        public void Remove(Control control, string stringProperty) {
            BindingOperations.ClearBinding(control, stringProperty);
            Find(control, stringProperty).Do(x => Actions.Remove(x));
        }
        public SetBindingAction Find(Control control, string stringProperty) {
            Guard.ArgumentNotNull(control, "control");
            Guard.ArgumentInRange(!string.IsNullOrEmpty(stringProperty), "property");
            return Actions.FirstOrDefault(x => x.IsMatchedAction(control, stringProperty));
        }
        public void SetBinding(Control control, string propertyName, BindingBase binding) {
            BindingOperations.SetBinding(control, propertyName, binding);
            //TODO  all SetBinding variants and test it
            //TODO  do all this only in design time
            this.AddOrReplace(control, propertyName, (Binding)binding);
        }
        public void SetBinding(Control control, AttachedPropertyBase property, BindingBase binding) {
            if(setAttachedPropertyBindingMethodInfo == null) {
                setAttachedPropertyBindingMethodInfo = typeof(BindingOperations).GetMethods(BindingFlags.Public | BindingFlags.Static).
                    First(x => x.Name == "SetBinding" && x.GetParameters().ElementAt(1).ParameterType.Name == typeof(AttachedProperty<>).Name); //TODO use lambda
            }
            MethodInfo mi = setAttachedPropertyBindingMethodInfo.MakeGenericMethod(property.PropertyType);
            mi.Invoke(null, new object[] { control, property, binding });
            //this.AddOrReplace(control, null, property, (Binding)binding);
        }

        public void RemoveControlActions(Control control) {
            Guard.ArgumentNotNull(control, "control");
            foreach(SetBindingAction action in GetActions().Where(action => object.Equals(action.Control, control)).ToArray()) {
                Remove(action.Control, action.StringProperty);
            }
        }

        internal IEnumerable<SetBindingAction> GetActions() {
            return Actions;
        }

        void AddOrReplace(Control control, string stringProperty, Binding binding) {
            SetBindingAction newAction = new SetBindingAction(control, stringProperty, binding);
            int index = -1;
            SetBindingAction oldAction = Find(control, stringProperty);
            if(oldAction != null)
                index = Actions.IndexOf(oldAction);
            if(index >= 0)
                Actions[index] = newAction;
            else
                Actions.Add(newAction);
        }
    }
}