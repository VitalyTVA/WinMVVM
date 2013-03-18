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
            tvDataContext.AfterSelect += tvDataContext_AfterSelect;
            bClear.Click += bClear_Click;
            bBind.Click += bBind_Click;
            lbBoundProperties.DisplayMember = "Name";
            lbUnboundProperties.DisplayMember = "Name";
            cbMode.Items.AddRange(Enum.GetValues(typeof(BindingMode)).Cast<object>().ToArray());
            cbMode.SelectedIndex = 0;
        }

        void tvDataContext_AfterSelect(object sender, TreeViewEventArgs e) {
            if(e.Node != null) {
                tbPath.Text = GetNodePath(e.Node);
            }
        }
        string GetNodePath(TreeNode node) {
            if(node == null)
                return string.Empty;
            string parentPath = GetNodePath(node.Parent);
            return (string.IsNullOrEmpty(parentPath) ? string.Empty : parentPath + ".") + node.Text;
        }

        void bBind_Click(object sender, EventArgs e) {
            if(SelectedProperty == null || SelectedComponent == null)
                return;
            designer.ChangeComponent(() => {
                Manager.SetBindingCore(SelectedComponent, SelectedProperty, new Binding(tbPath.Text, (BindingMode)cbMode.SelectedItem));
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

            ClearCurrentBindingInfo();
            if(SelectedProperty != null) { //TODO - use MayBe
                SetBindingAction action = Manager.FindAction(SelectedComponent, SelectedProperty) as SetBindingAction;
                if(action != null) {
                    Binding binding = action.Binding as Binding;
                    if(binding != null)
                        tbPath.Text = binding.Path;
                }
                object dataContext = SelectedComponent.GetDataContext();
                if(dataContext != null) {
                    PoplateNodes(tvDataContext.Nodes, dataContext.GetType());
                }
            }
        }
        void PoplateNodes(TreeNodeCollection nodes, Type type) {
            foreach(PropertyDescriptor property in TypeDescriptor.GetProperties(type)) {
                TreeNode node = new TreeNode(property.Name);
                PoplateNodes(node.Nodes, property.PropertyType);
                nodes.Add(node);
            }
        }
        void ClearCurrentBindingInfo() {
            tbPath.Text = null;
            tvDataContext.Nodes.Clear();
        }
        void lbComponentList_SelectedIndexChanged(object sender, EventArgs e) {

            RepopulateProperties();
        }

        void RepopulateProperties() {
            lbUnboundProperties.Items.Clear();
            lbBoundProperties.Items.Clear();
            ClearCurrentBindingInfo();
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
            yield return CommandProvider.CommandParameterProperty;
            if(control is ListBox || control is DataGridView) {
                yield return ItemsSourceProvider.ItemsSourceProperty;
            }
        }
    }
}
