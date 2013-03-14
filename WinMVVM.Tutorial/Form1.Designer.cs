namespace WinMVVM.Tutorial {
    partial class Form1 {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if(disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.button1 = new System.Windows.Forms.Button();
            this.bindingManager1 = new WinMVVM.BindingManager();
            this.textBox1 = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.bindingManager1)).BeginInit();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(90, 68);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(164, 42);
            this.button1.TabIndex = 0;
            this.button1.UseVisualStyleBackColor = true;
            this.bindingManager1.SetValue(this, WinMVVM.DataContextProvider.DataContextProperty, new WinMVVM.Tutorial.MainViewModel());
            this.bindingManager1.SetBinding(this.textBox1, "Text", new WinMVVM.Binding("MessageText", WinMVVM.BindingMode.TwoWay));
            this.bindingManager1.SetBinding(this.button1, "Text", new WinMVVM.Binding("MessageText"));
            this.bindingManager1.SetBinding(this.button1, WinMVVM.CommandProvider.CommandProperty, new WinMVVM.Binding("ShowMessageCommand"));
            this.bindingManager1.SetBinding(this.button1, WinMVVM.CommandProvider.CommandParameterProperty, new WinMVVM.Binding("MessageText"));
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(90, 160);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(164, 20);
            this.textBox1.TabIndex = 1;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(808, 359);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.button1);
            this.Name = "Form1";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.bindingManager1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private BindingManager bindingManager1;
        private System.Windows.Forms.TextBox textBox1;
    }
}

