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
                Manager.SetBinding(SelectedComponent, SelectedProperty.Name, new Binding(tbPath.Text));
                RepopulateProperties();
            });

        }

        void bClear_Click(object sender, EventArgs e) {
            if(SelectedProperty == null || SelectedComponent == null)
                return;
            designer.ChangeComponent(() => {
                Manager.Remove(SelectedComponent, SelectedProperty.Name);
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
                SetBindingAction action = Manager.Find(SelectedComponent, SelectedProperty.Name);
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
                foreach(PropertyDescriptor property in TypeDescriptor.GetProperties(SelectedComponent).Cast<PropertyDescriptor>().OrderBy(pd => pd.Name)) {
                    SetBindingAction existingAction = Manager.Find(SelectedComponent, property.Name);
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
