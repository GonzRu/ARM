namespace DeviceFormLib
{
    partial class FormSank
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
            this.splitContainer6 = new System.Windows.Forms.SplitContainer();
            this.splitContainer3 = new System.Windows.Forms.SplitContainer();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.mtraNamedFLPanel2 = new LabelTextbox.MTRANamedFLPanel(this.components);
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.mtraNamedFLPanel4 = new LabelTextbox.MTRANamedFLPanel(this.components);
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.mtraNamedFLPanel3 = new LabelTextbox.MTRANamedFLPanel(this.components);
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.listView1 = new System.Windows.Forms.ListView();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.mtraNamedFLPanel1 = new LabelTextbox.MTRANamedFLPanel(this.components);
            this.tabControl.SuspendLayout();
            this.tpCurrentInfo.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer6)).BeginInit();
            this.splitContainer6.Panel1.SuspendLayout();
            this.splitContainer6.Panel2.SuspendLayout();
            this.splitContainer6.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).BeginInit();
            this.splitContainer3.Panel1.SuspendLayout();
            this.splitContainer3.Panel2.SuspendLayout();
            this.splitContainer3.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl
            // 
            this.tabControl.Controls.Add(this.tpCurrentInfo);
            this.tabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl.Location = new System.Drawing.Point(0, 0);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(1170, 652);
            this.tabControl.TabIndex = 0;
            this.tabControl.Selected += new System.Windows.Forms.TabControlEventHandler(this.TabControlSelected);
            // 
            // tpCurrentInfo
            // 
            this.tpCurrentInfo.BackColor = System.Drawing.SystemColors.Control;
            this.tpCurrentInfo.Controls.Add(this.splitContainer6);
            this.tpCurrentInfo.Location = new System.Drawing.Point(4, 22);
            this.tpCurrentInfo.Name = "tpCurrentInfo";
            this.tpCurrentInfo.Padding = new System.Windows.Forms.Padding(3);
            this.tpCurrentInfo.Size = new System.Drawing.Size(1162, 626);
            this.tpCurrentInfo.TabIndex = 0;
            this.tpCurrentInfo.Text = "Текущая информация";
            // 
            // splitContainer6
            // 
            this.splitContainer6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer6.Location = new System.Drawing.Point(3, 3);
            this.splitContainer6.Name = "splitContainer6";
            // 
            // splitContainer6.Panel1
            // 
            this.splitContainer6.Panel1.Controls.Add(this.splitContainer3);
            // 
            // splitContainer6.Panel2
            // 
            this.splitContainer6.Panel2.Controls.Add(this.splitContainer2);
            this.splitContainer6.Size = new System.Drawing.Size(1156, 620);
            this.splitContainer6.SplitterDistance = 568;
            this.splitContainer6.TabIndex = 1;
            // 
            // splitContainer3
            // 
            this.splitContainer3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer3.Location = new System.Drawing.Point(0, 0);
            this.splitContainer3.Name = "splitContainer3";
            this.splitContainer3.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer3.Panel1
            // 
            this.splitContainer3.Panel1.Controls.Add(this.groupBox3);
            // 
            // splitContainer3.Panel2
            // 
            this.splitContainer3.Panel2.Controls.Add(this.groupBox5);
            this.splitContainer3.Size = new System.Drawing.Size(568, 620);
            this.splitContainer3.SplitterDistance = 468;
            this.splitContainer3.TabIndex = 1;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.mtraNamedFLPanel2);
            this.groupBox3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox3.Location = new System.Drawing.Point(0, 0);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(568, 468);
            this.groupBox3.TabIndex = 0;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Состояние:";
            // 
            // mtraNamedFLPanel2
            // 
            this.mtraNamedFLPanel2.AutoScroll = true;
            this.mtraNamedFLPanel2.BackColor = System.Drawing.SystemColors.Control;
            this.mtraNamedFLPanel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.mtraNamedFLPanel2.Caption = "Состояние";
            this.mtraNamedFLPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mtraNamedFLPanel2.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.mtraNamedFLPanel2.Location = new System.Drawing.Point(3, 16);
            this.mtraNamedFLPanel2.Name = "mtraNamedFLPanel2";
            this.mtraNamedFLPanel2.Size = new System.Drawing.Size(562, 449);
            this.mtraNamedFLPanel2.TabIndex = 0;
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.mtraNamedFLPanel4);
            this.groupBox5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox5.Location = new System.Drawing.Point(0, 0);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(568, 148);
            this.groupBox5.TabIndex = 0;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Сетевой адрес:";
            // 
            // mtraNamedFLPanel4
            // 
            this.mtraNamedFLPanel4.AutoScroll = true;
            this.mtraNamedFLPanel4.BackColor = System.Drawing.SystemColors.Control;
            this.mtraNamedFLPanel4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.mtraNamedFLPanel4.Caption = "Сетевой адрес";
            this.mtraNamedFLPanel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mtraNamedFLPanel4.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.mtraNamedFLPanel4.Location = new System.Drawing.Point(3, 16);
            this.mtraNamedFLPanel4.Name = "mtraNamedFLPanel4";
            this.mtraNamedFLPanel4.Size = new System.Drawing.Size(562, 129);
            this.mtraNamedFLPanel4.TabIndex = 0;
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.groupBox2);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.splitContainer1);
            this.splitContainer2.Size = new System.Drawing.Size(584, 620);
            this.splitContainer2.SplitterDistance = 170;
            this.splitContainer2.TabIndex = 3;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.mtraNamedFLPanel3);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox2.Location = new System.Drawing.Point(0, 0);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(584, 170);
            this.groupBox2.TabIndex = 0;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Данные ОЗЗ:";
            // 
            // mtraNamedFLPanel3
            // 
            this.mtraNamedFLPanel3.AutoScroll = true;
            this.mtraNamedFLPanel3.BackColor = System.Drawing.SystemColors.Control;
            this.mtraNamedFLPanel3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.mtraNamedFLPanel3.Caption = "Данные ОЗЗ";
            this.mtraNamedFLPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mtraNamedFLPanel3.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.mtraNamedFLPanel3.Location = new System.Drawing.Point(3, 16);
            this.mtraNamedFLPanel3.Name = "mtraNamedFLPanel3";
            this.mtraNamedFLPanel3.Size = new System.Drawing.Size(578, 151);
            this.mtraNamedFLPanel3.TabIndex = 0;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.groupBox4);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.groupBox1);
            this.splitContainer1.Panel2Collapsed = true;
            this.splitContainer1.Size = new System.Drawing.Size(584, 446);
            this.splitContainer1.SplitterDistance = 421;
            this.splitContainer1.TabIndex = 2;
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.listView1);
            this.groupBox4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox4.Location = new System.Drawing.Point(0, 0);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(584, 446);
            this.groupBox4.TabIndex = 1;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Журнал ОЗЗ:";
            // 
            // listView1
            // 
            this.listView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listView1.FullRowSelect = true;
            this.listView1.GridLines = true;
            this.listView1.Location = new System.Drawing.Point(3, 16);
            this.listView1.MultiSelect = false;
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(578, 427);
            this.listView1.TabIndex = 0;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.Details;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.mtraNamedFLPanel1);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(150, 46);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Журнал ОЗЗ (Raw data):";
            // 
            // mtraNamedFLPanel1
            // 
            this.mtraNamedFLPanel1.AutoScroll = true;
            this.mtraNamedFLPanel1.BackColor = System.Drawing.SystemColors.Control;
            this.mtraNamedFLPanel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.mtraNamedFLPanel1.Caption = "Журнал ОЗЗ (Raw data)";
            this.mtraNamedFLPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mtraNamedFLPanel1.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.mtraNamedFLPanel1.Location = new System.Drawing.Point(3, 16);
            this.mtraNamedFLPanel1.Name = "mtraNamedFLPanel1";
            this.mtraNamedFLPanel1.Size = new System.Drawing.Size(144, 27);
            this.mtraNamedFLPanel1.TabIndex = 0;
            // 
            // Frm4DeviceSank
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1170, 652);
            this.Controls.Add(this.tabControl);
            this.Name = "Frm4DeviceSank";
            this.Text = "frm4DeviceSank";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormBmrzDeviceFormClosing);
            this.Load += new System.EventHandler(this.FormBmrzDeviceLoad);
            this.tabControl.ResumeLayout(false);
            this.tpCurrentInfo.ResumeLayout(false);
            this.splitContainer6.Panel1.ResumeLayout(false);
            this.splitContainer6.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer6)).EndInit();
            this.splitContainer6.ResumeLayout(false);
            this.splitContainer3.Panel1.ResumeLayout(false);
            this.splitContainer3.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).EndInit();
            this.splitContainer3.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox5.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage tpCurrentInfo;
        private System.Windows.Forms.SplitContainer splitContainer6;
        private System.Windows.Forms.GroupBox groupBox3;
        private LabelTextbox.MTRANamedFLPanel mtraNamedFLPanel2;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.GroupBox groupBox1;
        private LabelTextbox.MTRANamedFLPanel mtraNamedFLPanel1;
        private System.Windows.Forms.SplitContainer splitContainer3;
        private System.Windows.Forms.GroupBox groupBox5;
        private LabelTextbox.MTRANamedFLPanel mtraNamedFLPanel4;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.GroupBox groupBox2;
        private LabelTextbox.MTRANamedFLPanel mtraNamedFLPanel3;
    }
}