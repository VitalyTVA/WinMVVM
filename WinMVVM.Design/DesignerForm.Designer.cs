namespace WinMVVM.Design {
    partial class DesignerForm {
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
            this.designerView1 = new WinMVVM.Design.DesignerView();
            this.SuspendLayout();
            // 
            // designerView1
            // 
            this.designerView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.designerView1.Location = new System.Drawing.Point(0, 0);
            this.designerView1.Name = "designerView1";
            this.designerView1.Size = new System.Drawing.Size(670, 377);
            this.designerView1.TabIndex = 0;
            // 
            // DesignerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(670, 377);
            this.Controls.Add(this.designerView1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MinimizeBox = false;
            this.Name = "DesignerForm";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.ResumeLayout(false);

        }

        #endregion

        private DesignerView designerView1;
    }
}