using System;
using System.Collections.Generic;
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
        SetBindingActionCollection Actions { get; set; }
        public BindingManager() {
            Actions = new SetBindingActionCollection(this);
        }
        void ISupportInitialize.BeginInit() {
        }
        void ISupportInitialize.EndInit() {
        }
        public void Remove(Control control, string property) {
            Find(control, property).Do(x => Actions.Remove(x));
        }
        public SetBindingAction Find(Control control, string property) {
            return Actions.FirstOrDefault(x => x.IsMatchedAction(control, property));
        }
        public void AddOrReplace(Control control, string property, Binding binding) {
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
        public IEnumerable<SetBindingAction> GetActions() {
            return Actions;
        }
        public void SetBinding(Control control, string propertyName, BindingBase binding) {
            //BindingOperations.SetBinding(control, propertyName, binding);
            //TODO  all SetBinding variants and test it
            //TODO  do all this only in design time
            this.AddOrReplace(control, propertyName, (Binding)binding); 
        }
    }
}