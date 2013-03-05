using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinMVVM.Design {
    public class BindingManagerActionList : DesignerActionList {
        public BindingManagerActionList(IComponent component)
            : base(component) {
        }
        public override DesignerActionItemCollection GetSortedActionItems() {
            DesignerActionItemCollection result = new DesignerActionItemCollection();
            result.Add(new DesignerActionMethodItem(this, "RunDesigner", "Show designer"));
            return result;
        }

        public void RunDesigner() {
            System.Windows.Forms.MessageBox.Show("Run designer");
        }
        public override bool AutoShow {
            get {
                return true;
            }
            set {
                //base.AutoShow = value;
            }
        }
    }
    public class BindingManagerDesigner : System.ComponentModel.Design.ComponentDesigner {
        DesignerVerbCollection verbs;
        public override DesignerVerbCollection Verbs {
            get {
                if(verbs == null) {
                    verbs = new DesignerVerbCollection(new DesignerVerb[] {
                        new DesignerVerb("Run Designer", new EventHandler(OnRunDesigner))
                    });
                }
                return verbs;
            }
        }

        private void OnRunDesigner(object sender, EventArgs e) {
            System.Windows.Forms.MessageBox.Show("Run designer");
        }

        DesignerActionListCollection actionLists;
        public override DesignerActionListCollection ActionLists {
            get {
                if(actionLists == null) {
                    actionLists = new DesignerActionListCollection();
                    actionLists.Add(new BindingManagerActionList(Component));
                }
                return actionLists;
            }
        }
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
