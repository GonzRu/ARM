namespace DeviceFormLib
{
    partial class FormOrion2SP
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
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.splitContainer6 = new System.Windows.Forms.SplitContainer();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.mtraNamedFLPanel1 = new LabelTextbox.MTRANamedFLPanel(this.components);
            this.splitContainer7 = new System.Windows.Forms.SplitContainer();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.mtraNamedFLPanel2 = new LabelTextbox.MTRANamedFLPanel(this.components);
            this.splitContainer11 = new System.Windows.Forms.SplitContainer();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.mtraNamedFLPanel3 = new LabelTextbox.MTRANamedFLPanel(this.components);
            this.splitContainer3 = new System.Windows.Forms.SplitContainer();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.mtraNamedFLPanel4 = new LabelTextbox.MTRANamedFLPanel(this.components);
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.mtraNamedFLPanel8 = new LabelTextbox.MTRANamedFLPanel(this.components);
            this.splitContainer9 = new System.Windows.Forms.SplitContainer();
            this.groupBox12 = new System.Windows.Forms.GroupBox();
            this.splitContainer4 = new System.Windows.Forms.SplitContainer();
            this.mtraNamedFLStatus = new LabelTextbox.MTRANamedFLPanel(this.components);
            this.mtraNamedFLPanel22 = new LabelTextbox.MTRANamedFLPanel(this.components);
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.mtraNamedFLPanel5 = new LabelTextbox.MTRANamedFLPanel(this.components);
            this.tpAccumulationInfo = new System.Windows.Forms.TabPage();
            this.mtraNamedFLPanel6 = new LabelTextbox.MTRANamedFLPanel(this.components);
            this.tabControl.SuspendLayout();
            this.tpCurrentInfo.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer6)).BeginInit();
            this.splitContainer6.Panel1.SuspendLayout();
            this.splitContainer6.Panel2.SuspendLayout();
            this.splitContainer6.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer7)).BeginInit();
            this.splitContainer7.Panel1.SuspendLayout();
            this.splitContainer7.Panel2.SuspendLayout();
            this.splitContainer7.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer11)).BeginInit();
            this.splitContainer11.Panel1.SuspendLayout();
            this.splitContainer11.Panel2.SuspendLayout();
            this.splitContainer11.SuspendLayout();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).BeginInit();
            this.splitContainer3.Panel1.SuspendLayout();
            this.splitContainer3.Panel2.SuspendLayout();
            this.splitContainer3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer9)).BeginInit();
            this.splitContainer9.Panel1.SuspendLayout();
            this.splitContainer9.Panel2.SuspendLayout();
            this.splitContainer9.SuspendLayout();
            this.groupBox12.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer4)).BeginInit();
            this.splitContainer4.Panel1.SuspendLayout();
            this.splitContainer4.Panel2.SuspendLayout();
            this.splitContainer4.SuspendLayout();
            this.groupBox6.SuspendLayout();
            this.tpAccumulationInfo.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl
            // 
            this.tabControl.Controls.Add(this.tpCurrentInfo);
            this.tabControl.Controls.Add(this.tpAccumulationInfo);
            this.tabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl.Location = new System.Drawing.Point(0, 0);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(1193, 612);
            this.tabControl.TabIndex = 2;
            this.tabControl.Selected += new System.Windows.Forms.TabControlEventHandler(this.TabControlSelected);
            // 
            // tpCurrentInfo
            // 
            this.tpCurrentInfo.BackColor = System.Drawing.SystemColors.Control;
            this.tpCurrentInfo.Controls.Add(this.splitContainer1);
            this.tpCurrentInfo.Location = new System.Drawing.Point(4, 22);
            this.tpCurrentInfo.Name = "tpCurrentInfo";
            this.tpCurrentInfo.Padding = new System.Windows.Forms.Padding(3);
            this.tpCurrentInfo.Size = new System.Drawing.Size(1185, 586);
            this.tpCurrentInfo.TabIndex = 0;
            this.tpCurrentInfo.Text = "Текущая информация";
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(3, 3);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.splitContainer6);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.splitContainer9);
            this.splitContainer1.Panel2Collapsed = true;
            this.splitContainer1.Size = new System.Drawing.Size(1179, 580);
            this.splitContainer1.SplitterDistance = 486;
            this.splitContainer1.TabIndex = 2;
            // 
            // splitContainer6
            // 
            this.splitContainer6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer6.Location = new System.Drawing.Point(0, 0);
            this.splitContainer6.Name = "splitContainer6";
            // 
            // splitContainer6.Panel1
            // 
            this.splitContainer6.Panel1.Controls.Add(this.groupBox1);
            // 
            // splitContainer6.Panel2
            // 
            this.splitContainer6.Panel2.Controls.Add(this.splitContainer7);
            this.splitContainer6.Size = new System.Drawing.Size(1179, 580);
            this.splitContainer6.SplitterDistance = 231;
            this.splitContainer6.TabIndex = 1;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.mtraNamedFLPanel1);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(231, 580);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Параметры сети:";
            // 
            // mtraNamedFLPanel1
            // 
            this.mtraNamedFLPanel1.AutoScroll = true;
            this.mtraNamedFLPanel1.BackColor = System.Drawing.SystemColors.Control;
            this.mtraNamedFLPanel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.mtraNamedFLPanel1.Caption = "Параметры сети";
            this.mtraNamedFLPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mtraNamedFLPanel1.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.mtraNamedFLPanel1.Location = new System.Drawing.Point(3, 16);
            this.mtraNamedFLPanel1.Name = "mtraNamedFLPanel1";
            this.mtraNamedFLPanel1.Size = new System.Drawing.Size(225, 561);
            this.mtraNamedFLPanel1.TabIndex = 0;
            // 
            // splitContainer7
            // 
            this.splitContainer7.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer7.Location = new System.Drawing.Point(0, 0);
            this.splitContainer7.Name = "splitContainer7";
            // 
            // splitContainer7.Panel1
            // 
            this.splitContainer7.Panel1.Controls.Add(this.groupBox2);
            // 
            // splitContainer7.Panel2
            // 
            this.splitContainer7.Panel2.Controls.Add(this.splitContainer11);
            this.splitContainer7.Size = new System.Drawing.Size(944, 580);
            this.splitContainer7.SplitterDistance = 235;
            this.splitContainer7.TabIndex = 0;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.mtraNamedFLPanel2);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox2.Location = new System.Drawing.Point(0, 0);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(235, 580);
            this.groupBox2.TabIndex = 0;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Расчетные параметры сети:";
            // 
            // mtraNamedFLPanel2
            // 
            this.mtraNamedFLPanel2.AutoScroll = true;
            this.mtraNamedFLPanel2.BackColor = System.Drawing.SystemColors.Control;
            this.mtraNamedFLPanel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.mtraNamedFLPanel2.Caption = "Расчетные параметры сети";
            this.mtraNamedFLPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mtraNamedFLPanel2.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.mtraNamedFLPanel2.Location = new System.Drawing.Point(3, 16);
            this.mtraNamedFLPanel2.Name = "mtraNamedFLPanel2";
            this.mtraNamedFLPanel2.Size = new System.Drawing.Size(229, 561);
            this.mtraNamedFLPanel2.TabIndex = 0;
            // 
            // splitContainer11
            // 
            this.splitContainer11.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer11.Location = new System.Drawing.Point(0, 0);
            this.splitContainer11.Name = "splitContainer11";
            // 
            // splitContainer11.Panel1
            // 
            this.splitContainer11.Panel1.Controls.Add(this.groupBox3);
            // 
            // splitContainer11.Panel2
            // 
            this.splitContainer11.Panel2.Controls.Add(this.splitContainer3);
            this.splitContainer11.Size = new System.Drawing.Size(705, 580);
            this.splitContainer11.SplitterDistance = 229;
            this.splitContainer11.TabIndex = 0;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.mtraNamedFLPanel3);
            this.groupBox3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox3.Location = new System.Drawing.Point(0, 0);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(229, 580);
            this.groupBox3.TabIndex = 0;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Системные выходы:";
            // 
            // mtraNamedFLPanel3
            // 
            this.mtraNamedFLPanel3.AutoScroll = true;
            this.mtraNamedFLPanel3.BackColor = System.Drawing.SystemColors.Control;
            this.mtraNamedFLPanel3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.mtraNamedFLPanel3.Caption = "Системные выходы";
            this.mtraNamedFLPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mtraNamedFLPanel3.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.mtraNamedFLPanel3.Location = new System.Drawing.Point(3, 16);
            this.mtraNamedFLPanel3.Name = "mtraNamedFLPanel3";
            this.mtraNamedFLPanel3.Size = new System.Drawing.Size(223, 561);
            this.mtraNamedFLPanel3.TabIndex = 0;
            // 
            // splitContainer3
            // 
            this.splitContainer3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer3.Location = new System.Drawing.Point(0, 0);
            this.splitContainer3.Name = "splitContainer3";
            // 
            // splitContainer3.Panel1
            // 
            this.splitContainer3.Panel1.Controls.Add(this.groupBox4);
            // 
            // splitContainer3.Panel2
            // 
            this.splitContainer3.Panel2.Controls.Add(this.groupBox5);
            this.splitContainer3.Size = new System.Drawing.Size(472, 580);
            this.splitContainer3.SplitterDistance = 236;
            this.splitContainer3.TabIndex = 1;
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.mtraNamedFLPanel4);
            this.groupBox4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox4.Location = new System.Drawing.Point(0, 0);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(236, 580);
            this.groupBox4.TabIndex = 0;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Дискретные входы:";
            // 
            // mtraNamedFLPanel4
            // 
            this.mtraNamedFLPanel4.AutoScroll = true;
            this.mtraNamedFLPanel4.BackColor = System.Drawing.SystemColors.Control;
            this.mtraNamedFLPanel4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.mtraNamedFLPanel4.Caption = "Дискретные входы";
            this.mtraNamedFLPanel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mtraNamedFLPanel4.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.mtraNamedFLPanel4.Location = new System.Drawing.Point(3, 16);
            this.mtraNamedFLPanel4.Name = "mtraNamedFLPanel4";
            this.mtraNamedFLPanel4.Size = new System.Drawing.Size(230, 561);
            this.mtraNamedFLPanel4.TabIndex = 0;
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.mtraNamedFLPanel8);
            this.groupBox5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox5.Location = new System.Drawing.Point(0, 0);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(232, 580);
            this.groupBox5.TabIndex = 0;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Дискретные выходы:";
            // 
            // mtraNamedFLPanel8
            // 
            this.mtraNamedFLPanel8.AutoScroll = true;
            this.mtraNamedFLPanel8.BackColor = System.Drawing.SystemColors.Control;
            this.mtraNamedFLPanel8.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.mtraNamedFLPanel8.Caption = "Дискретные выходы";
            this.mtraNamedFLPanel8.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mtraNamedFLPanel8.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.mtraNamedFLPanel8.Location = new System.Drawing.Point(3, 16);
            this.mtraNamedFLPanel8.Name = "mtraNamedFLPanel8";
            this.mtraNamedFLPanel8.Size = new System.Drawing.Size(226, 561);
            this.mtraNamedFLPanel8.TabIndex = 0;
            // 
            // splitContainer9
            // 
            this.splitContainer9.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer9.Location = new System.Drawing.Point(0, 0);
            this.splitContainer9.Name = "splitContainer9";
            // 
            // splitContainer9.Panel1
            // 
            this.splitContainer9.Panel1.Controls.Add(this.groupBox12);
            // 
            // splitContainer9.Panel2
            // 
            this.splitContainer9.Panel2.Controls.Add(this.groupBox6);
            this.splitContainer9.Size = new System.Drawing.Size(150, 46);
            this.splitContainer9.SplitterDistance = 111;
            this.splitContainer9.TabIndex = 1;
            // 
            // groupBox12
            // 
            this.groupBox12.Controls.Add(this.splitContainer4);
            this.groupBox12.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox12.Location = new System.Drawing.Point(0, 0);
            this.groupBox12.Name = "groupBox12";
            this.groupBox12.Size = new System.Drawing.Size(111, 46);
            this.groupBox12.TabIndex = 2;
            this.groupBox12.TabStop = false;
            this.groupBox12.Text = "Статус:";
            // 
            // splitContainer4
            // 
            this.splitContainer4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer4.Location = new System.Drawing.Point(3, 16);
            this.splitContainer4.Name = "splitContainer4";
            // 
            // splitContainer4.Panel1
            // 
            this.splitContainer4.Panel1.Controls.Add(this.mtraNamedFLStatus);
            // 
            // splitContainer4.Panel2
            // 
            this.splitContainer4.Panel2.Controls.Add(this.mtraNamedFLPanel22);
            this.splitContainer4.Size = new System.Drawing.Size(105, 27);
            this.splitContainer4.SplitterDistance = 74;
            this.splitContainer4.TabIndex = 2;
            // 
            // mtraNamedFLStatus
            // 
            this.mtraNamedFLStatus.AutoScroll = true;
            this.mtraNamedFLStatus.BackColor = System.Drawing.SystemColors.Control;
            this.mtraNamedFLStatus.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.mtraNamedFLStatus.Caption = "Статус";
            this.mtraNamedFLStatus.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mtraNamedFLStatus.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.mtraNamedFLStatus.Location = new System.Drawing.Point(0, 0);
            this.mtraNamedFLStatus.Name = "mtraNamedFLStatus";
            this.mtraNamedFLStatus.Size = new System.Drawing.Size(74, 27);
            this.mtraNamedFLStatus.TabIndex = 1;
            // 
            // mtraNamedFLPanel22
            // 
            this.mtraNamedFLPanel22.AutoScroll = true;
            this.mtraNamedFLPanel22.BackColor = System.Drawing.SystemColors.Control;
            this.mtraNamedFLPanel22.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.mtraNamedFLPanel22.Caption = "Статус2";
            this.mtraNamedFLPanel22.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mtraNamedFLPanel22.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.mtraNamedFLPanel22.Location = new System.Drawing.Point(0, 0);
            this.mtraNamedFLPanel22.Name = "mtraNamedFLPanel22";
            this.mtraNamedFLPanel22.Size = new System.Drawing.Size(27, 27);
            this.mtraNamedFLPanel22.TabIndex = 0;
            // 
            // groupBox6
            // 
            this.groupBox6.Controls.Add(this.mtraNamedFLPanel5);
            this.groupBox6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox6.Location = new System.Drawing.Point(0, 0);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(35, 46);
            this.groupBox6.TabIndex = 1;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "Информация:";
            // 
            // mtraNamedFLPanel5
            // 
            this.mtraNamedFLPanel5.AutoScroll = true;
            this.mtraNamedFLPanel5.BackColor = System.Drawing.SystemColors.Control;
            this.mtraNamedFLPanel5.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.mtraNamedFLPanel5.Caption = "Информация";
            this.mtraNamedFLPanel5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mtraNamedFLPanel5.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.mtraNamedFLPanel5.Location = new System.Drawing.Point(3, 16);
            this.mtraNamedFLPanel5.Name = "mtraNamedFLPanel5";
            this.mtraNamedFLPanel5.Size = new System.Drawing.Size(29, 27);
            this.mtraNamedFLPanel5.TabIndex = 0;
            // 
            // tpAccumulationInfo
            // 
            this.tpAccumulationInfo.BackColor = System.Drawing.SystemColors.Control;
            this.tpAccumulationInfo.Controls.Add(this.mtraNamedFLPanel6);
            this.tpAccumulationInfo.Location = new System.Drawing.Point(4, 22);
            this.tpAccumulationInfo.Name = "tpAccumulationInfo";
            this.tpAccumulationInfo.Padding = new System.Windows.Forms.Padding(3);
            this.tpAccumulationInfo.Size = new System.Drawing.Size(1185, 586);
            this.tpAccumulationInfo.TabIndex = 2;
            this.tpAccumulationInfo.Text = "Накопительная информация";
            // 
            // mtraNamedFLPanel6
            // 
            this.mtraNamedFLPanel6.AutoScroll = true;
            this.mtraNamedFLPanel6.BackColor = System.Drawing.SystemColors.Control;
            this.mtraNamedFLPanel6.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.mtraNamedFLPanel6.Caption = "Накопители";
            this.mtraNamedFLPanel6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mtraNamedFLPanel6.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.mtraNamedFLPanel6.Location = new System.Drawing.Point(3, 3);
            this.mtraNamedFLPanel6.Name = "mtraNamedFLPanel6";
            this.mtraNamedFLPanel6.Size = new System.Drawing.Size(1179, 580);
            this.mtraNamedFLPanel6.TabIndex = 0;
            // 
            // FormOrionDevice
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1193, 612);
            this.Controls.Add(this.tabControl);
            this.Name = "FormOrionDevice";
            this.Text = "FormOrionDevice";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormBmrzDeviceFormClosing);
            this.Load += new System.EventHandler(this.FormBmrzDeviceLoad);
            this.tabControl.ResumeLayout(false);
            this.tpCurrentInfo.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.splitContainer6.Panel1.ResumeLayout(false);
            this.splitContainer6.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer6)).EndInit();
            this.splitContainer6.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.splitContainer7.Panel1.ResumeLayout(false);
            this.splitContainer7.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer7)).EndInit();
            this.splitContainer7.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.splitContainer11.Panel1.ResumeLayout(false);
            this.splitContainer11.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer11)).EndInit();
            this.splitContainer11.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.splitContainer3.Panel1.ResumeLayout(false);
            this.splitContainer3.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).EndInit();
            this.splitContainer3.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            this.groupBox5.ResumeLayout(false);
            this.splitContainer9.Panel1.ResumeLayout(false);
            this.splitContainer9.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer9)).EndInit();
            this.splitContainer9.ResumeLayout(false);
            this.groupBox12.ResumeLayout(false);
            this.splitContainer4.Panel1.ResumeLayout(false);
            this.splitContainer4.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer4)).EndInit();
            this.splitContainer4.ResumeLayout(false);
            this.groupBox6.ResumeLayout(false);
            this.tpAccumulationInfo.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        internal System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage tpCurrentInfo;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.SplitContainer splitContainer6;
        private System.Windows.Forms.SplitContainer splitContainer7;
        private System.Windows.Forms.SplitContainer splitContainer11;
        private System.Windows.Forms.SplitContainer splitContainer9;
        private System.Windows.Forms.GroupBox groupBox12;
        private System.Windows.Forms.SplitContainer splitContainer4;
        private LabelTextbox.MTRANamedFLPanel mtraNamedFLStatus;
        private LabelTextbox.MTRANamedFLPanel mtraNamedFLPanel22;
        private System.Windows.Forms.GroupBox groupBox6;
        private LabelTextbox.MTRANamedFLPanel mtraNamedFLPanel5;
        private System.Windows.Forms.TabPage tpAccumulationInfo;
        private System.Windows.Forms.GroupBox groupBox1;
        private LabelTextbox.MTRANamedFLPanel mtraNamedFLPanel1;
        private System.Windows.Forms.GroupBox groupBox2;
        private LabelTextbox.MTRANamedFLPanel mtraNamedFLPanel2;
        private System.Windows.Forms.GroupBox groupBox3;
        private LabelTextbox.MTRANamedFLPanel mtraNamedFLPanel3;
        private System.Windows.Forms.SplitContainer splitContainer3;
        private System.Windows.Forms.GroupBox groupBox4;
        private LabelTextbox.MTRANamedFLPanel mtraNamedFLPanel4;
        private System.Windows.Forms.GroupBox groupBox5;
        private LabelTextbox.MTRANamedFLPanel mtraNamedFLPanel8;
        private LabelTextbox.MTRANamedFLPanel mtraNamedFLPanel6;
    }
}