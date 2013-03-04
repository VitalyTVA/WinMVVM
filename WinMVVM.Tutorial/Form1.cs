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
            button1.SetBinding(() => new Button().Text, new Binding(() => new MainViewModel().MessageText));
            button1.SetBinding(CommandProvider.CommandProperty, new Binding(() => new MainViewModel().ShowMessageCommand));
        }

        private void button1_Click(object sender, EventArgs e) {
            viewModel.MessageText += "+";
        }
    }
}
