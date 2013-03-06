using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinMVVM.Design {
    public partial class DesignerView : UserControl {
        BindingManagerDesigner designer;
        BindingManager Manager { get { return (BindingManager)designer.Component; } }
        private PropertyDescriptor SelectedProperty {
            get {
                return (lbUnboundProperties.SelectedItem ?? lbBoundProperties.SelectedItem) as PropertyDescriptor;
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
                SetBindingAction oldAction = GetSelectedAction();
                SetBindingAction newAction = new SetBindingAction(SelectedComponent, SelectedProperty.Name, new Binding(tbPath.Text));
                if(oldAction != null) {
                    int index = Manager.Collection.IndexOf(oldAction);
                    Manager.Collection[index] = newAction;
                } else {
                    Manager.Collection.Add(newAction);
                }
                RepopulateProperties();
            });

        }

        void bClear_Click(object sender, EventArgs e) {
            if(SelectedProperty == null || SelectedComponent == null)
                return;
            SetBindingAction oldAction = GetSelectedAction();
            if(oldAction == null)
                return;
            designer.ChangeComponent(() => {
                Manager.Collection.Remove(oldAction);
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
                SetBindingAction action = GetSelectedAction();
                if(action != null) {
                    Binding binding = action.Binding as Binding;
                    if(binding != null)
                        tbPath.Text = binding.Path;
                }
            }
        }

        SetBindingAction GetSelectedAction() {
            return GetExistingAction(SelectedComponent, SelectedProperty.Name);
        }
        SetBindingAction GetExistingAction(Control control, string propertyName) {
            return Manager.Collection.FirstOrDefault(a => a.Control == control && a.Property == propertyName);
        }
        void lbComponentList_SelectedIndexChanged(object sender, EventArgs e) {

            RepopulateProperties();
        }

        void RepopulateProperties() {
            lbUnboundProperties.Items.Clear();
            lbBoundProperties.Items.Clear();
            tbPath.Text = null;
            if(SelectedComponent != null) {
                foreach(PropertyDescriptor property in TypeDescriptor.GetProperties(SelectedComponent).Cast<PropertyDescriptor>().OrderBy(pd => pd.Name)) {
                    SetBindingAction existingAction = GetExistingAction(SelectedComponent, property.Name);
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
    }
}
