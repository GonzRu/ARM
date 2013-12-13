namespace DeviceFormLib
{
    partial class FormUsoMtd
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
            this.splitContainer9 = new System.Windows.Forms.SplitContainer();
            this.groupBox12 = new System.Windows.Forms.GroupBox();
            this.mtraNamedFLStatus = new LabelTextbox.MTRANamedFLPanel(this.components);
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.mtraNamedFLPanel5 = new LabelTextbox.MTRANamedFLPanel(this.components);
            this.splitContainer6 = new System.Windows.Forms.SplitContainer();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.mtraNamedFLPanel2 = new LabelTextbox.MTRANamedFLPanel(this.components);
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.mtraNamedFLPanel3 = new LabelTextbox.MTRANamedFLPanel(this.components);
            this.tabSystem = new System.Windows.Forms.TabPage();
            this.splitContainer3 = new System.Windows.Forms.SplitContainer();
            this.groupBox11 = new System.Windows.Forms.GroupBox();
            this.mtraNamedFLPanel21 = new LabelTextbox.MTRANamedFLPanel(this.components);
            this.gbTest = new System.Windows.Forms.GroupBox();
            this.mtraNamedFLPanel20 = new LabelTextbox.MTRANamedFLPanel(this.components);
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.tabControl.SuspendLayout();
            this.tpCurrentInfo.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer9)).BeginInit();
            this.splitContainer9.Panel1.SuspendLayout();
            this.splitContainer9.Panel2.SuspendLayout();
            this.splitContainer9.SuspendLayout();
            this.groupBox12.SuspendLayout();
            this.groupBox6.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer6)).BeginInit();
            this.splitContainer6.Panel1.SuspendLayout();
            this.splitContainer6.Panel2.SuspendLayout();
            this.splitContainer6.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.tabSystem.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).BeginInit();
            this.splitContainer3.Panel1.SuspendLayout();
            this.splitContainer3.Panel2.SuspendLayout();
            this.splitContainer3.SuspendLayout();
            this.groupBox11.SuspendLayout();
            this.gbTest.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl
            // 
            this.tabControl.Controls.Add(this.tpCurrentInfo);
            this.tabControl.Controls.Add(this.tabSystem);
            this.tabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl.Location = new System.Drawing.Point(0, 0);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(1167, 602);
            this.tabControl.TabIndex = 1;
            this.tabControl.Selected += new System.Windows.Forms.TabControlEventHandler(this.TabControlSelected);
            // 
            // tpCurrentInfo
            // 
            this.tpCurrentInfo.BackColor = System.Drawing.SystemColors.Control;
            this.tpCurrentInfo.Controls.Add(this.tableLayoutPanel1);
            this.tpCurrentInfo.Location = new System.Drawing.Point(4, 22);
            this.tpCurrentInfo.Name = "tpCurrentInfo";
            this.tpCurrentInfo.Padding = new System.Windows.Forms.Padding(3);
            this.tpCurrentInfo.Size = new System.Drawing.Size(1159, 576);
            this.tpCurrentInfo.TabIndex = 0;
            this.tpCurrentInfo.Text = "Текущая информация";
            // 
            // splitContainer9
            // 
            this.splitContainer9.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer9.Location = new System.Drawing.Point(3, 457);
            this.splitContainer9.Name = "splitContainer9";
            // 
            // splitContainer9.Panel1
            // 
            this.splitContainer9.Panel1.Controls.Add(this.groupBox12);
            // 
            // splitContainer9.Panel2
            // 
            this.splitContainer9.Panel2.Controls.Add(this.groupBox6);
            this.splitContainer9.Size = new System.Drawing.Size(971, 104);
            this.splitContainer9.SplitterDistance = 723;
            this.splitContainer9.TabIndex = 1;
            // 
            // groupBox12
            // 
            this.groupBox12.Controls.Add(this.mtraNamedFLStatus);
            this.groupBox12.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox12.Location = new System.Drawing.Point(0, 0);
            this.groupBox12.Name = "groupBox12";
            this.groupBox12.Size = new System.Drawing.Size(723, 104);
            this.groupBox12.TabIndex = 2;
            this.groupBox12.TabStop = false;
            this.groupBox12.Text = "Статус:";
            // 
            // mtraNamedFLStatus
            // 
            this.mtraNamedFLStatus.AutoScroll = true;
            this.mtraNamedFLStatus.BackColor = System.Drawing.SystemColors.Control;
            this.mtraNamedFLStatus.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.mtraNamedFLStatus.Caption = "Статус";
            this.mtraNamedFLStatus.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mtraNamedFLStatus.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.mtraNamedFLStatus.Location = new System.Drawing.Point(3, 16);
            this.mtraNamedFLStatus.Name = "mtraNamedFLStatus";
            this.mtraNamedFLStatus.Size = new System.Drawing.Size(717, 85);
            this.mtraNamedFLStatus.TabIndex = 1;
            // 
            // groupBox6
            // 
            this.groupBox6.Controls.Add(this.mtraNamedFLPanel5);
            this.groupBox6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox6.Location = new System.Drawing.Point(0, 0);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(244, 104);
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
            this.mtraNamedFLPanel5.Size = new System.Drawing.Size(238, 85);
            this.mtraNamedFLPanel5.TabIndex = 0;
            // 
            // splitContainer6
            // 
            this.splitContainer6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer6.Location = new System.Drawing.Point(3, 3);
            this.splitContainer6.Name = "splitContainer6";
            // 
            // splitContainer6.Panel1
            // 
            this.splitContainer6.Panel1.Controls.Add(this.groupBox3);
            // 
            // splitContainer6.Panel2
            // 
            this.splitContainer6.Panel2.Controls.Add(this.groupBox4);
            this.splitContainer6.Size = new System.Drawing.Size(971, 448);
            this.splitContainer6.SplitterDistance = 470;
            this.splitContainer6.TabIndex = 1;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.mtraNamedFLPanel2);
            this.groupBox3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox3.Location = new System.Drawing.Point(0, 0);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(470, 448);
            this.groupBox3.TabIndex = 0;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Дискретные входы:";
            // 
            // mtraNamedFLPanel2
            // 
            this.mtraNamedFLPanel2.AutoScroll = true;
            this.mtraNamedFLPanel2.BackColor = System.Drawing.SystemColors.Control;
            this.mtraNamedFLPanel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.mtraNamedFLPanel2.Caption = "Дискретные входы";
            this.mtraNamedFLPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mtraNamedFLPanel2.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.mtraNamedFLPanel2.Location = new System.Drawing.Point(3, 16);
            this.mtraNamedFLPanel2.Name = "mtraNamedFLPanel2";
            this.mtraNamedFLPanel2.Size = new System.Drawing.Size(464, 429);
            this.mtraNamedFLPanel2.TabIndex = 0;
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.mtraNamedFLPanel3);
            this.groupBox4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox4.Location = new System.Drawing.Point(0, 0);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(497, 448);
            this.groupBox4.TabIndex = 1;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Дискретные выходы:";
            // 
            // mtraNamedFLPanel3
            // 
            this.mtraNamedFLPanel3.AutoScroll = true;
            this.mtraNamedFLPanel3.BackColor = System.Drawing.SystemColors.Control;
            this.mtraNamedFLPanel3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.mtraNamedFLPanel3.Caption = "Дискретные выходы";
            this.mtraNamedFLPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mtraNamedFLPanel3.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.mtraNamedFLPanel3.Location = new System.Drawing.Point(3, 16);
            this.mtraNamedFLPanel3.Name = "mtraNamedFLPanel3";
            this.mtraNamedFLPanel3.Size = new System.Drawing.Size(491, 429);
            this.mtraNamedFLPanel3.TabIndex = 0;
            // 
            // tabSystem
            // 
            this.tabSystem.BackColor = System.Drawing.SystemColors.Control;
            this.tabSystem.Controls.Add(this.splitContainer3);
            this.tabSystem.Location = new System.Drawing.Point(4, 22);
            this.tabSystem.Name = "tabSystem";
            this.tabSystem.Padding = new System.Windows.Forms.Padding(3);
            this.tabSystem.Size = new System.Drawing.Size(1159, 576);
            this.tabSystem.TabIndex = 21;
            this.tabSystem.Text = "Система";
            // 
            // splitContainer3
            // 
            this.splitContainer3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer3.Location = new System.Drawing.Point(3, 3);
            this.splitContainer3.Name = "splitContainer3";
            // 
            // splitContainer3.Panel1
            // 
            this.splitContainer3.Panel1.Controls.Add(this.groupBox11);
            // 
            // splitContainer3.Panel2
            // 
            this.splitContainer3.Panel2.Controls.Add(this.gbTest);
            this.splitContainer3.Size = new System.Drawing.Size(1153, 570);
            this.splitContainer3.SplitterDistance = 561;
            this.splitContainer3.TabIndex = 0;
            // 
            // groupBox11
            // 
            this.groupBox11.Controls.Add(this.mtraNamedFLPanel21);
            this.groupBox11.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox11.Location = new System.Drawing.Point(0, 0);
            this.groupBox11.Name = "groupBox11";
            this.groupBox11.Size = new System.Drawing.Size(561, 570);
            this.groupBox11.TabIndex = 0;
            this.groupBox11.TabStop = false;
            this.groupBox11.Text = "Неисправность БМРЗ";
            // 
            // mtraNamedFLPanel21
            // 
            this.mtraNamedFLPanel21.AutoScroll = true;
            this.mtraNamedFLPanel21.BackColor = System.Drawing.SystemColors.Control;
            this.mtraNamedFLPanel21.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.mtraNamedFLPanel21.Caption = "Неисправность БМРЗ";
            this.mtraNamedFLPanel21.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mtraNamedFLPanel21.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.mtraNamedFLPanel21.Location = new System.Drawing.Point(3, 16);
            this.mtraNamedFLPanel21.Name = "mtraNamedFLPanel21";
            this.mtraNamedFLPanel21.Size = new System.Drawing.Size(555, 551);
            this.mtraNamedFLPanel21.TabIndex = 2;
            // 
            // gbTest
            // 
            this.gbTest.Controls.Add(this.mtraNamedFLPanel20);
            this.gbTest.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gbTest.Location = new System.Drawing.Point(0, 0);
            this.gbTest.Name = "gbTest";
            this.gbTest.Size = new System.Drawing.Size(588, 570);
            this.gbTest.TabIndex = 2;
            this.gbTest.TabStop = false;
            this.gbTest.Text = "Результаты тестирования БМРЗ:";
            // 
            // mtraNamedFLPanel20
            // 
            this.mtraNamedFLPanel20.AutoScroll = true;
            this.mtraNamedFLPanel20.BackColor = System.Drawing.SystemColors.Control;
            this.mtraNamedFLPanel20.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.mtraNamedFLPanel20.Caption = "Результат тестирования БМРЗ";
            this.mtraNamedFLPanel20.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mtraNamedFLPanel20.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.mtraNamedFLPanel20.Location = new System.Drawing.Point(3, 16);
            this.mtraNamedFLPanel20.Name = "mtraNamedFLPanel20";
            this.mtraNamedFLPanel20.Size = new System.Drawing.Size(582, 551);
            this.mtraNamedFLPanel20.TabIndex = 1;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 170F));
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.flowLayoutPanel1, 1, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1153, 570);
            this.tableLayoutPanel1.TabIndex = 4;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 1;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Controls.Add(this.splitContainer9, 0, 1);
            this.tableLayoutPanel2.Controls.Add(this.splitContainer6, 0, 0);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 2;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 110F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(977, 564);
            this.tableLayoutPanel2.TabIndex = 0;
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(986, 3);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(164, 564);
            this.flowLayoutPanel1.TabIndex = 1;
            // 
            // FormUsoMtd
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1167, 602);
            this.Controls.Add(this.tabControl);
            this.Name = "FormUsoMtd";
            this.Text = "BMRZ Serial Device";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormBmrzDeviceFormClosing);
            this.Load += new System.EventHandler(this.FormBmrzDeviceLoad);
            this.tabControl.ResumeLayout(false);
            this.tpCurrentInfo.ResumeLayout(false);
            this.splitContainer9.Panel1.ResumeLayout(false);
            this.splitContainer9.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer9)).EndInit();
            this.splitContainer9.ResumeLayout(false);
            this.groupBox12.ResumeLayout(false);
            this.groupBox6.ResumeLayout(false);
            this.splitContainer6.Panel1.ResumeLayout(false);
            this.splitContainer6.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer6)).EndInit();
            this.splitContainer6.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            this.tabSystem.ResumeLayout(false);
            this.splitContainer3.Panel1.ResumeLayout(false);
            this.splitContainer3.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).EndInit();
            this.splitContainer3.ResumeLayout(false);
            this.groupBox11.ResumeLayout(false);
            this.gbTest.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        internal System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage tpCurrentInfo;
        private System.Windows.Forms.SplitContainer splitContainer6;
        private System.Windows.Forms.GroupBox groupBox3;
        private LabelTextbox.MTRANamedFLPanel mtraNamedFLPanel2;
        private System.Windows.Forms.GroupBox groupBox4;
        private LabelTextbox.MTRANamedFLPanel mtraNamedFLPanel3;
        private System.Windows.Forms.TabPage tabSystem;
        private System.Windows.Forms.GroupBox gbTest;
        private LabelTextbox.MTRANamedFLPanel mtraNamedFLPanel20;
        private System.Windows.Forms.SplitContainer splitContainer9;
        private LabelTextbox.MTRANamedFLPanel mtraNamedFLStatus;
        private System.Windows.Forms.GroupBox groupBox6;
        private LabelTextbox.MTRANamedFLPanel mtraNamedFLPanel5;
        private System.Windows.Forms.SplitContainer splitContainer3;
        private System.Windows.Forms.GroupBox groupBox11;
        private LabelTextbox.MTRANamedFLPanel mtraNamedFLPanel21;
        private System.Windows.Forms.GroupBox groupBox12;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
    }
}