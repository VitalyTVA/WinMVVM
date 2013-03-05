using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Linq;
using System.Linq.Expressions;
using System.Windows;
using System.Windows.Forms;
using WinMVVM.Utils;

namespace WinMVVM {
    [Designer(typeof(BindingManagerDesigner))]
    //[Designer("WinMVVM.Design.BindingManagerDesigner, WinMVVM.Design, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")]
    public class BindingManager : Component, ISupportInitialize {
        [
        DesignerSerializationVisibility(DesignerSerializationVisibility.Content)
        ]
        public SetBindingActionCollection Collection { get; private set; }

        public BindingManager() {
            Collection = new SetBindingActionCollection(this);
        }
        void ISupportInitialize.BeginInit() {
        }
        void ISupportInitialize.EndInit() {
        }
    }
    public class BindingManagerDesigner : System.ComponentModel.Design.ComponentDesigner {
        protected override void Dispose(bool disposing) {
            if(disposing) {
                IComponentChangeService service = (IComponentChangeService)this.GetService(typeof(IComponentChangeService));
                if(service != null) {
                    service.ComponentRemoving -= new ComponentEventHandler(this.OnComponentRemoving);
                }
            }
            base.Dispose(disposing);
        }
        public override void Initialize(System.ComponentModel.IComponent component) {
            base.Initialize(component);
            IComponentChangeService service = (IComponentChangeService)this.GetService(typeof(IComponentChangeService));
            if(service != null) {
                service.ComponentRemoving += new ComponentEventHandler(this.OnComponentRemoving);
            }
        }
        private void OnComponentRemoving(object sender, ComponentEventArgs e) {
            BindingManager component = base.Component as BindingManager;
            if(component != null) {
                IComponentChangeService service = (IComponentChangeService)this.GetService(typeof(IComponentChangeService));
                PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(component);
                PropertyDescriptor member = (properties != null) ? properties[ExpressionHelper.GetPropertyName(() => component.Collection)] : null;
                if((service != null) && (member != null)) {
                    service.OnComponentChanging(component, null);
                }
                foreach(SetBindingAction action in component.Collection.Where(action => action.Control == e.Component).ToArray()) {
                    component.Collection.Remove(action);
                }
                if((service != null) && (member != null)) {
                    service.OnComponentChanged(component, null, null, null);
                }
            }
        }

 

    }
}
