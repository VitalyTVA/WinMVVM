using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WinMVVM;

namespace WinMVVM.Tutorial {
    public partial class Form1 : Form {
        MainViewModel viewModel = new MainViewModel();
        public Form1() {
            InitializeComponent();

            this.SetDataContext(viewModel);
            button1.SetBinding(() => button1.Text, new Binding("MessageText"));
        }

        private void button1_Click(object sender, EventArgs e) {
            viewModel.MessageText += "+";
        }
    }
}
