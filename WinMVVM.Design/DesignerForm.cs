using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinMVVM.Design {
    public partial class DesignerForm : Form {
        public DesignerForm() {
            InitializeComponent();
        }
        public void SetDesigner(BindingManagerDesigner designer) {
            designerView1.SetDesigner(designer);
        }
    }
}
