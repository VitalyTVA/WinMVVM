using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace WinMVVM.Design {
    public class BindingManagerDesigner : System.ComponentModel.Design.ComponentDesigner {
        class BindingManagerActionList : DesignerActionList {
            readonly BindingManagerDesigner designer;
            public BindingManagerActionList(BindingManagerDesigner designer)
                : base(designer.Component) {
                this.designer = designer;
            }
            public override DesignerActionItemCollection GetSortedActionItems() {
                DesignerActionItemCollection result = new DesignerActionItemCollection();
                result.Add(new DesignerActionMethodItem(this, "RunDesigner", SR_Design.RunDesigner)); //TODO  use lambda to get method name
                return result;
            }

            public void RunDesigner() {
                designer.RunDesigner();
            }
        }

        public static BindingManagerDesigner FromTypeDescriptorContext(ITypeDescriptorContext context) {
            if(context != null) {
                
                IDesignerHost host = context.GetService(typeof(IDesignerHost)) as IDesignerHost;
                if(host == null) {
                    return null;
                }

                BindingManager manager = host.Container.Components.Cast<IComponent>().FirstOrDefault(x => x is BindingManager) as BindingManager;
                if(manager == null) {
                    return null;
                }
                return host.GetDesigner(manager) as BindingManagerDesigner;
            }
            return null;
        }

        DesignerVerbCollection verbs;
        public override DesignerVerbCollection Verbs {
            get {
                if(verbs == null) {
                    verbs = new DesignerVerbCollection(new DesignerVerb[] {
                        new DesignerVerb(SR_Design.RunDesigner, new EventHandler(OnRunDesigner))
                    });
                }
                return verbs;
            }
        }

        void OnRunDesigner(object sender, EventArgs e) {
            RunDesigner();
        }
        internal void RunDesigner() {
            using(var form = new DesignerForm()) {
                form.SetDesigner(this);
                form.ShowDialog(NativeWindow.FromHandle(NativeHelper.GetActiveWindow()));
            }
        }
        CommonPropertiesExtender extender;
        IExtenderProviderService localExtenderServiceReference;
        DesignerActionListCollection actionLists;
        public override DesignerActionListCollection ActionLists {
            get {
                if(actionLists == null) {
                    actionLists = new DesignerActionListCollection();
                    actionLists.Add(new BindingManagerActionList(this));
                }
                return actionLists;
            }
        }
        protected override void Dispose(bool disposing) {
            if(disposing) {
                IComponentChangeService service = (IComponentChangeService)this.GetService(typeof(IComponentChangeService));
                if(service != null) {
                    service.ComponentRemoving -= new ComponentEventHandler(this.OnComponentRemoving);
                    service.ComponentAdding -= OnComponentAdding;
                }
                if(localExtenderServiceReference != null) {
                    localExtenderServiceReference.RemoveExtenderProvider(extender);
                    localExtenderServiceReference = null;
                }

                //foreach(var item in Component.Site.Container.Components) {
                //    RemoveProvider(item);
                //}
            }
            base.Dispose(disposing);
        }
        public override void Initialize(System.ComponentModel.IComponent component) {
            base.Initialize(component);

            foreach(var item in Component.Site.Container.Components) {
                AddProvider(item);
            }

            IComponentChangeService service = (IComponentChangeService)this.GetService(typeof(IComponentChangeService));
            if(service != null) {
                service.ComponentRemoving += OnComponentRemoving;
                service.ComponentAdding += OnComponentAdding;
            }

            IExtenderProviderService extenderService = (IExtenderProviderService)component.Site.GetService(typeof(IExtenderProviderService));
            if(extenderService != null) {
                extender = new CommonPropertiesExtender(this);
                extenderService.AddExtenderProvider(extender);
                localExtenderServiceReference = extenderService;
            }

        }

        static void AddProvider(object item) {
            TypeDescriptor.AddProvider(new BoundElementTypeDescriptionProvider(TypeDescriptor.GetProvider(item)), item);
        }
        //static void RemoveProvider(object item) {
        //    BoundElementTypeDescriptionProvider provider = TypeDescriptor.GetProvider(item) as BoundElementTypeDescriptionProvider;
        //    if(provider != null)
        //        TypeDescriptor.RemoveProvider(provider, item);
        //}
        void OnComponentAdding(object sender, ComponentEventArgs e) {
            AddProvider(e.Component);
        }
        void OnComponentRemoving(object sender, ComponentEventArgs e) {
            //RemoveProvider(e.Component);
            ChangeComponent(() => {
                BindingManager bindingManager = base.Component as BindingManager;
                if(bindingManager != null && e.Component is Control)
                    bindingManager.ClearAllBindings(e.Component as Control);
            });
        }
        public void ChangeComponent(Action changeAction) {
            BindingManager component = base.Component as BindingManager;
            if(component != null) {
                IComponentChangeService service = (IComponentChangeService)this.GetService(typeof(IComponentChangeService));
                if((service != null)) {
                    service.OnComponentChanging(component, null);
                }
                changeAction();
                if((service != null)) {
                    service.OnComponentChanged(component, null, null, null);
                }
            }
        }
    }
}
