namespace WinMVVM.Design {
    partial class DesignerView {
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.lbComponentList = new System.Windows.Forms.ListBox();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.lbBoundProperties = new System.Windows.Forms.ListBox();
            this.lbUnboundProperties = new System.Windows.Forms.ListBox();
            this.label1 = new System.Windows.Forms.Label();
            this.tbPath = new System.Windows.Forms.TextBox();
            this.bBind = new System.Windows.Forms.Button();
            this.bClear = new System.Windows.Forms.Button();
            this.tvDataContext = new System.Windows.Forms.TreeView();
            this.cbMode = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Location = new System.Drawing.Point(23, 27);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(204, 317);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.lbComponentList);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(196, 291);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "List";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // lbComponentList
            // 
            this.lbComponentList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbComponentList.FormattingEnabled = true;
            this.lbComponentList.Location = new System.Drawing.Point(3, 3);
            this.lbComponentList.Name = "lbComponentList";
            this.lbComponentList.Size = new System.Drawing.Size(190, 285);
            this.lbComponentList.TabIndex = 0;
            // 
            // tabPage2
            // 
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(196, 291);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Tree";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(250, 27);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(88, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Bound Properties";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(250, 158);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(101, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "Unbound Properties";
            // 
            // lbBoundProperties
            // 
            this.lbBoundProperties.FormattingEnabled = true;
            this.lbBoundProperties.Location = new System.Drawing.Point(253, 49);
            this.lbBoundProperties.Name = "lbBoundProperties";
            this.lbBoundProperties.Size = new System.Drawing.Size(160, 95);
            this.lbBoundProperties.TabIndex = 3;
            // 
            // lbUnboundProperties
            // 
            this.lbUnboundProperties.FormattingEnabled = true;
            this.lbUnboundProperties.Location = new System.Drawing.Point(253, 179);
            this.lbUnboundProperties.Name = "lbUnboundProperties";
            this.lbUnboundProperties.Size = new System.Drawing.Size(160, 160);
            this.lbUnboundProperties.TabIndex = 4;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(453, 27);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(29, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "Path";
            // 
            // tbPath
            // 
            this.tbPath.Location = new System.Drawing.Point(456, 49);
            this.tbPath.Name = "tbPath";
            this.tbPath.Size = new System.Drawing.Size(179, 20);
            this.tbPath.TabIndex = 6;
            // 
            // bBind
            // 
            this.bBind.Location = new System.Drawing.Point(456, 313);
            this.bBind.Name = "bBind";
            this.bBind.Size = new System.Drawing.Size(84, 23);
            this.bBind.TabIndex = 7;
            this.bBind.Text = "Bind";
            this.bBind.UseVisualStyleBackColor = true;
            // 
            // bClear
            // 
            this.bClear.Location = new System.Drawing.Point(556, 313);
            this.bClear.Name = "bClear";
            this.bClear.Size = new System.Drawing.Size(79, 23);
            this.bClear.TabIndex = 8;
            this.bClear.Text = "Clear";
            this.bClear.UseVisualStyleBackColor = true;
            // 
            // tvDataContext
            // 
            this.tvDataContext.Location = new System.Drawing.Point(456, 76);
            this.tvDataContext.Name = "tvDataContext";
            this.tvDataContext.Size = new System.Drawing.Size(179, 170);
            this.tvDataContext.TabIndex = 9;
            // 
            // cbMode
            // 
            this.cbMode.FormattingEnabled = true;
            this.cbMode.Location = new System.Drawing.Point(456, 274);
            this.cbMode.Name = "cbMode";
            this.cbMode.Size = new System.Drawing.Size(179, 21);
            this.cbMode.TabIndex = 10;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(453, 254);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(34, 13);
            this.label4.TabIndex = 11;
            this.label4.Text = "Mode";
            // 
            // DesignerView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.label4);
            this.Controls.Add(this.cbMode);
            this.Controls.Add(this.tvDataContext);
            this.Controls.Add(this.bClear);
            this.Controls.Add(this.bBind);
            this.Controls.Add(this.tbPath);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lbUnboundProperties);
            this.Controls.Add(this.lbBoundProperties);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.tabControl1);
            this.Name = "DesignerView";
            this.Size = new System.Drawing.Size(737, 367);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.ListBox lbComponentList;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ListBox lbBoundProperties;
        private System.Windows.Forms.ListBox lbUnboundProperties;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tbPath;
        private System.Windows.Forms.Button bBind;
        private System.Windows.Forms.Button bClear;
        private System.Windows.Forms.TreeView tvDataContext;
        private System.Windows.Forms.ComboBox cbMode;
        private System.Windows.Forms.Label label4;
    }
}
