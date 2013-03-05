using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinMVVM.Design {
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
            System.Windows.Forms.MessageBox.Show("Designer");
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
                //PropertyDescriptor member = (properties != null) ? properties[ExpressionHelper.GetPropertyName(() => component.Collection)] : null;
                if((service != null) /*&& (member != null)*/) {
                    service.OnComponentChanging(component, null);
                }
                foreach(SetBindingAction action in component.Collection.Where(action => object.Equals(action.Control, e.Component)).ToArray()) {
                    component.Collection.Remove(action);
                }
                if((service != null)/* && (member != null)*/) {
                    service.OnComponentChanged(component, null, null, null);
                }
            }
        }
    }
}
