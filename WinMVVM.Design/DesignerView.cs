using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WinMVVM.Utils;

namespace WinMVVM.Design {
    public partial class DesignerView : UserControl {
        BindingManagerDesigner designer;
        BindingManager Manager { get { return (BindingManager)designer.Component; } }
        private PropertyDescriptorBase SelectedProperty {
            get {
                return (lbUnboundProperties.SelectedItem ?? lbBoundProperties.SelectedItem) as PropertyDescriptorBase;
            }
        }
        private Control SelectedComponent {
            get {
                return lbComponentList.SelectedItem as Control;
            }
        }
        public DesignerView() {
            InitializeComponent();
            lbComponentList.SelectedIndexChanged += lbComponentList_SelectedIndexChanged;
            lbBoundProperties.SelectedIndexChanged += lbUnboundProperties_SelectedIndexChanged;
            lbUnboundProperties.SelectedIndexChanged += lbUnboundProperties_SelectedIndexChanged;
            bClear.Click += bClear_Click;
            bBind.Click += bBind_Click;
            lbBoundProperties.DisplayMember = "Name";
            lbUnboundProperties.DisplayMember = "Name";
        }

        void bBind_Click(object sender, EventArgs e) {
            if(SelectedProperty == null || SelectedComponent == null)
                return;
            designer.ChangeComponent(() => {
                Manager.SetBindingCore(SelectedComponent, SelectedProperty, new Binding(tbPath.Text));
                RepopulateProperties();
            });

        }

        void bClear_Click(object sender, EventArgs e) {
            if(SelectedProperty == null || SelectedComponent == null)
                return;
            designer.ChangeComponent(() => {
                Manager.ClearBindingCore(SelectedComponent, SelectedProperty);
                RepopulateProperties();
            });
        }

        void lbUnboundProperties_SelectedIndexChanged(object sender, EventArgs e) {
            if(sender == lbUnboundProperties && lbUnboundProperties.SelectedItem != null) {
                lbBoundProperties.SelectedItem = null;
            }
            if(sender == lbBoundProperties && lbBoundProperties.SelectedItem != null) {
                lbUnboundProperties.SelectedItem = null;
            }

            tbPath.Text = null;
            if(SelectedProperty != null) { //TODO - use MayBe
                SetBindingAction action = Manager.FindAction(SelectedComponent, SelectedProperty) as SetBindingAction;
                if(action != null) {
                    Binding binding = action.Binding as Binding;
                    if(binding != null)
                        tbPath.Text = binding.Path;
                }
            }
        }
        void lbComponentList_SelectedIndexChanged(object sender, EventArgs e) {

            RepopulateProperties();
        }

        void RepopulateProperties() {
            lbUnboundProperties.Items.Clear();
            lbBoundProperties.Items.Clear();
            tbPath.Text = null;
            if(SelectedComponent != null) {
                IEnumerable<PropertyDescriptorBase> orderedProperties = GetProperties(SelectedComponent);
                foreach(PropertyDescriptorBase property in orderedProperties) {
                    SetBindingAction existingAction = Manager.FindAction(SelectedComponent, property) as SetBindingAction;
                    if(existingAction == null)
                        lbUnboundProperties.Items.Add(property);
                    else
                        lbBoundProperties.Items.Add(property);
                }
            }
        }
        internal void SetDesigner(BindingManagerDesigner designer) {
            this.designer = designer;

            foreach(IComponent item in designer.Component.Site.Container.Components) {
                lbComponentList.Items.Add(item);
            }
        }
        static IEnumerable<PropertyDescriptorBase> GetProperties(Control control) {
            IEnumerable<PropertyDescriptorBase> attachedProperties = GetAttachedProperties(control).Select(x => AttachedPropertyDescriptor.FromAttachedProperty(x));
            IEnumerable<PropertyDescriptorBase> standardProperties = TypeDescriptor.GetProperties(control).Cast<PropertyDescriptor>().Select(x => StandardPropertyDescriptor.FromPropertyName(control, x.Name)).OrderBy(pd => pd.Name);
            return attachedProperties.Union(standardProperties);
        }
        static IEnumerable<AttachedPropertyBase> GetAttachedProperties(Control control) {
            yield return DataContextProvider.DataContextProperty;
            yield return CommandProvider.CommandProperty;
        }
    }
}
