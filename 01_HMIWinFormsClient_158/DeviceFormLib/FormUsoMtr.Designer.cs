namespace DeviceFormLib
{
    partial class FormUsoMtr
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose( bool disposing )
        {
            if ( disposing && ( components != null ) )
            {
                components.Dispose();
            }
            base.Dispose( disposing );
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.tabControl = new System.Windows.Forms.TabControl();
            this.tpCurrentInfo = new System.Windows.Forms.TabPage();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.mtraNamedFLPanel1 = new LabelTextbox.MTRANamedFLPanel(this.components);
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.mtraNamedFLPanel2 = new LabelTextbox.MTRANamedFLPanel(this.components);
            this.splitContainer3 = new System.Windows.Forms.SplitContainer();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.mtraNamedFLPanel3 = new LabelTextbox.MTRANamedFLPanel(this.components);
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.mtraNamedFLPanel4 = new LabelTextbox.MTRANamedFLPanel(this.components);
            this.tabControl.SuspendLayout();
            this.tpCurrentInfo.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).BeginInit();
            this.splitContainer3.Panel1.SuspendLayout();
            this.splitContainer3.Panel2.SuspendLayout();
            this.splitContainer3.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl
            // 
            this.tabControl.Controls.Add(this.tpCurrentInfo);
            this.tabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl.Location = new System.Drawing.Point(0, 0);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(1079, 625);
            this.tabControl.TabIndex = 1;
            // 
            // tpCurrentInfo
            // 
            this.tpCurrentInfo.BackColor = System.Drawing.SystemColors.Control;
            this.tpCurrentInfo.Controls.Add(this.tableLayoutPanel1);
            this.tpCurrentInfo.Location = new System.Drawing.Point(4, 22);
            this.tpCurrentInfo.Name = "tpCurrentInfo";
            this.tpCurrentInfo.Padding = new System.Windows.Forms.Padding(3);
            this.tpCurrentInfo.Size = new System.Drawing.Size(1071, 599);
            this.tpCurrentInfo.TabIndex = 0;
            this.tpCurrentInfo.Text = "Контроль";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 170F));
            this.tableLayoutPanel1.Controls.Add(this.flowLayoutPanel1, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.splitContainer1, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1065, 593);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(898, 3);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(164, 587);
            this.flowLayoutPanel1.TabIndex = 0;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(3, 3);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.groupBox1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.splitContainer2);
            this.splitContainer1.Size = new System.Drawing.Size(889, 587);
            this.splitContainer1.SplitterDistance = 217;
            this.splitContainer1.TabIndex = 1;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.mtraNamedFLPanel1);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(217, 587);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Физические входы:";
            // 
            // mtraNamedFLPanel1
            // 
            this.mtraNamedFLPanel1.AutoScroll = true;
            this.mtraNamedFLPanel1.BackColor = System.Drawing.SystemColors.Control;
            this.mtraNamedFLPanel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.mtraNamedFLPanel1.Caption = "Физические входы";
            this.mtraNamedFLPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mtraNamedFLPanel1.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.mtraNamedFLPanel1.Location = new System.Drawing.Point(3, 16);
            this.mtraNamedFLPanel1.Name = "mtraNamedFLPanel1";
            this.mtraNamedFLPanel1.Size = new System.Drawing.Size(211, 568);
            this.mtraNamedFLPanel1.TabIndex = 0;
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.groupBox2);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.splitContainer3);
            this.splitContainer2.Size = new System.Drawing.Size(668, 587);
            this.splitContainer2.SplitterDistance = 222;
            this.splitContainer2.TabIndex = 0;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.mtraNamedFLPanel2);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox2.Location = new System.Drawing.Point(0, 0);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(222, 587);
            this.groupBox2.TabIndex = 0;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Логические входы:";
            // 
            // mtraNamedFLPanel2
            // 
            this.mtraNamedFLPanel2.AutoScroll = true;
            this.mtraNamedFLPanel2.BackColor = System.Drawing.SystemColors.Control;
            this.mtraNamedFLPanel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.mtraNamedFLPanel2.Caption = "Логические входы";
            this.mtraNamedFLPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mtraNamedFLPanel2.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.mtraNamedFLPanel2.Location = new System.Drawing.Point(3, 16);
            this.mtraNamedFLPanel2.Name = "mtraNamedFLPanel2";
            this.mtraNamedFLPanel2.Size = new System.Drawing.Size(216, 568);
            this.mtraNamedFLPanel2.TabIndex = 0;
            // 
            // splitContainer3
            // 
            this.splitContainer3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer3.Location = new System.Drawing.Point(0, 0);
            this.splitContainer3.Name = "splitContainer3";
            // 
            // splitContainer3.Panel1
            // 
            this.splitContainer3.Panel1.Controls.Add(this.groupBox3);
            // 
            // splitContainer3.Panel2
            // 
            this.splitContainer3.Panel2.Controls.Add(this.groupBox4);
            this.splitContainer3.Size = new System.Drawing.Size(442, 587);
            this.splitContainer3.SplitterDistance = 223;
            this.splitContainer3.TabIndex = 0;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.mtraNamedFLPanel3);
            this.groupBox3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox3.Location = new System.Drawing.Point(0, 0);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(223, 587);
            this.groupBox3.TabIndex = 0;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Выходы:";
            // 
            // mtraNamedFLPanel3
            // 
            this.mtraNamedFLPanel3.AutoScroll = true;
            this.mtraNamedFLPanel3.BackColor = System.Drawing.SystemColors.Control;
            this.mtraNamedFLPanel3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.mtraNamedFLPanel3.Caption = "Выходы";
            this.mtraNamedFLPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mtraNamedFLPanel3.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.mtraNamedFLPanel3.Location = new System.Drawing.Point(3, 16);
            this.mtraNamedFLPanel3.Name = "mtraNamedFLPanel3";
            this.mtraNamedFLPanel3.Size = new System.Drawing.Size(217, 568);
            this.mtraNamedFLPanel3.TabIndex = 0;
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.mtraNamedFLPanel4);
            this.groupBox4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox4.Location = new System.Drawing.Point(0, 0);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(215, 587);
            this.groupBox4.TabIndex = 0;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Счетчики:";
            // 
            // mtraNamedFLPanel4
            // 
            this.mtraNamedFLPanel4.AutoScroll = true;
            this.mtraNamedFLPanel4.BackColor = System.Drawing.SystemColors.Control;
            this.mtraNamedFLPanel4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.mtraNamedFLPanel4.Caption = "Счетчики";
            this.mtraNamedFLPanel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mtraNamedFLPanel4.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.mtraNamedFLPanel4.Location = new System.Drawing.Point(3, 16);
            this.mtraNamedFLPanel4.Name = "mtraNamedFLPanel4";
            this.mtraNamedFLPanel4.Size = new System.Drawing.Size(209, 568);
            this.mtraNamedFLPanel4.TabIndex = 0;
            // 
            // FormUsoMtr
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1079, 625);
            this.Controls.Add(this.tabControl);
            this.Name = "FormUsoMtr";
            this.Text = "FormSiriusDevice";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormSiriusDDeviceFormClosing);
            this.Load += new System.EventHandler(this.FormSiriusDDeviceLoad);
            this.tabControl.ResumeLayout(false);
            this.tpCurrentInfo.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.splitContainer3.Panel1.ResumeLayout(false);
            this.splitContainer3.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).EndInit();
            this.splitContainer3.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage tpCurrentInfo;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.GroupBox groupBox1;
        private LabelTextbox.MTRANamedFLPanel mtraNamedFLPanel1;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.GroupBox groupBox2;
        private LabelTextbox.MTRANamedFLPanel mtraNamedFLPanel2;
        private System.Windows.Forms.SplitContainer splitContainer3;
        private System.Windows.Forms.GroupBox groupBox3;
        private LabelTextbox.MTRANamedFLPanel mtraNamedFLPanel3;
        private System.Windows.Forms.GroupBox groupBox4;
        private LabelTextbox.MTRANamedFLPanel mtraNamedFLPanel4;
    }
}