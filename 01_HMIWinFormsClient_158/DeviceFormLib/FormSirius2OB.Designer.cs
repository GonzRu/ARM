namespace DeviceFormLib
{
    partial class FormSirius2OB
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
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.splitContainer6 = new System.Windows.Forms.SplitContainer();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.mtraNamedFLPanel3 = new LabelTextbox.MTRANamedFLPanel(this.components);
            this.splitContainer7 = new System.Windows.Forms.SplitContainer();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.mtraNamedFLPanel7 = new LabelTextbox.MTRANamedFLPanel(this.components);
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.mtraNamedFLPanel8 = new LabelTextbox.MTRANamedFLPanel(this.components);
            this.tpRunning = new System.Windows.Forms.TabPage();
            this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
            this.splitContainer4 = new System.Windows.Forms.SplitContainer();
            this.lstvRun = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.selectControl1 = new DeviceFormLib.SelectControl();
            this.tpConfig = new System.Windows.Forms.TabPage();
            this.tableLayoutPanel5 = new System.Windows.Forms.TableLayoutPanel();
            this.splitContainer_tpConfig = new System.Windows.Forms.SplitContainer();
            this.lstvConfig = new System.Windows.Forms.ListView();
            this.clmnHDate = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.clmUstComment = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.pnlTPConfig = new System.Windows.Forms.Panel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.readWriteUstavky1 = new DeviceFormLib.ReadWriteUstavkyControl();
            this.selectControl2 = new DeviceFormLib.SelectControl();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.mtraNamedFLStatus = new LabelTextbox.MTRANamedFLPanel(this.components);
            this.panel2 = new System.Windows.Forms.Panel();
            this.tabControl.SuspendLayout();
            this.tpCurrentInfo.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer6)).BeginInit();
            this.splitContainer6.Panel1.SuspendLayout();
            this.splitContainer6.Panel2.SuspendLayout();
            this.splitContainer6.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer7)).BeginInit();
            this.splitContainer7.Panel1.SuspendLayout();
            this.splitContainer7.Panel2.SuspendLayout();
            this.splitContainer7.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.tpRunning.SuspendLayout();
            this.tableLayoutPanel4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer4)).BeginInit();
            this.splitContainer4.Panel1.SuspendLayout();
            this.splitContainer4.Panel2.SuspendLayout();
            this.splitContainer4.SuspendLayout();
            this.tpConfig.SuspendLayout();
            this.tableLayoutPanel5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer_tpConfig)).BeginInit();
            this.splitContainer_tpConfig.Panel1.SuspendLayout();
            this.splitContainer_tpConfig.Panel2.SuspendLayout();
            this.splitContainer_tpConfig.SuspendLayout();
            this.panel1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl
            // 
            this.tabControl.Controls.Add(this.tpCurrentInfo);
            this.tabControl.Controls.Add(this.tpRunning);
            this.tabControl.Controls.Add(this.tpConfig);
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
            this.tpCurrentInfo.Controls.Add(this.tableLayoutPanel3);
            this.tpCurrentInfo.Location = new System.Drawing.Point(4, 22);
            this.tpCurrentInfo.Name = "tpCurrentInfo";
            this.tpCurrentInfo.Padding = new System.Windows.Forms.Padding(3);
            this.tpCurrentInfo.Size = new System.Drawing.Size(1071, 599);
            this.tpCurrentInfo.TabIndex = 0;
            this.tpCurrentInfo.Text = "Контроль";
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.ColumnCount = 1;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.Controls.Add(this.groupBox1, 0, 1);
            this.tableLayoutPanel3.Controls.Add(this.splitContainer6, 0, 0);
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 2;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 110F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(1065, 593);
            this.tableLayoutPanel3.TabIndex = 1;
            // 
            // splitContainer6
            // 
            this.splitContainer6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer6.Location = new System.Drawing.Point(3, 3);
            this.splitContainer6.Name = "splitContainer6";
            // 
            // splitContainer6.Panel1
            // 
            this.splitContainer6.Panel1.Controls.Add(this.groupBox2);
            // 
            // splitContainer6.Panel2
            // 
            this.splitContainer6.Panel2.Controls.Add(this.splitContainer7);
            this.splitContainer6.Size = new System.Drawing.Size(1059, 477);
            this.splitContainer6.SplitterDistance = 353;
            this.splitContainer6.TabIndex = 1;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.mtraNamedFLPanel3);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox2.Location = new System.Drawing.Point(0, 0);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(353, 477);
            this.groupBox2.TabIndex = 0;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Сигнализация:";
            // 
            // mtraNamedFLPanel3
            // 
            this.mtraNamedFLPanel3.AutoScroll = true;
            this.mtraNamedFLPanel3.BackColor = System.Drawing.SystemColors.Control;
            this.mtraNamedFLPanel3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.mtraNamedFLPanel3.Caption = "Сигнализация";
            this.mtraNamedFLPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mtraNamedFLPanel3.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.mtraNamedFLPanel3.Location = new System.Drawing.Point(3, 16);
            this.mtraNamedFLPanel3.Name = "mtraNamedFLPanel3";
            this.mtraNamedFLPanel3.Size = new System.Drawing.Size(347, 458);
            this.mtraNamedFLPanel3.TabIndex = 1;
            // 
            // splitContainer7
            // 
            this.splitContainer7.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer7.Location = new System.Drawing.Point(0, 0);
            this.splitContainer7.Name = "splitContainer7";
            // 
            // splitContainer7.Panel1
            // 
            this.splitContainer7.Panel1.Controls.Add(this.groupBox4);
            // 
            // splitContainer7.Panel2
            // 
            this.splitContainer7.Panel2.Controls.Add(this.groupBox5);
            this.splitContainer7.Size = new System.Drawing.Size(702, 477);
            this.splitContainer7.SplitterDistance = 369;
            this.splitContainer7.TabIndex = 0;
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.mtraNamedFLPanel7);
            this.groupBox4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox4.Location = new System.Drawing.Point(0, 0);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(369, 477);
            this.groupBox4.TabIndex = 0;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Входы:";
            // 
            // mtraNamedFLPanel7
            // 
            this.mtraNamedFLPanel7.AutoScroll = true;
            this.mtraNamedFLPanel7.BackColor = System.Drawing.SystemColors.Control;
            this.mtraNamedFLPanel7.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.mtraNamedFLPanel7.Caption = "Входы";
            this.mtraNamedFLPanel7.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mtraNamedFLPanel7.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.mtraNamedFLPanel7.Location = new System.Drawing.Point(3, 16);
            this.mtraNamedFLPanel7.Name = "mtraNamedFLPanel7";
            this.mtraNamedFLPanel7.Size = new System.Drawing.Size(363, 458);
            this.mtraNamedFLPanel7.TabIndex = 0;
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.mtraNamedFLPanel8);
            this.groupBox5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox5.Location = new System.Drawing.Point(0, 0);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(329, 477);
            this.groupBox5.TabIndex = 0;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Выходы:";
            // 
            // mtraNamedFLPanel8
            // 
            this.mtraNamedFLPanel8.AutoScroll = true;
            this.mtraNamedFLPanel8.BackColor = System.Drawing.SystemColors.Control;
            this.mtraNamedFLPanel8.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.mtraNamedFLPanel8.Caption = "Выходы";
            this.mtraNamedFLPanel8.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mtraNamedFLPanel8.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.mtraNamedFLPanel8.Location = new System.Drawing.Point(3, 16);
            this.mtraNamedFLPanel8.Name = "mtraNamedFLPanel8";
            this.mtraNamedFLPanel8.Size = new System.Drawing.Size(323, 458);
            this.mtraNamedFLPanel8.TabIndex = 0;
            // 
            // tpRunning
            // 
            this.tpRunning.BackColor = System.Drawing.SystemColors.Control;
            this.tpRunning.Controls.Add(this.tableLayoutPanel4);
            this.tpRunning.Location = new System.Drawing.Point(4, 22);
            this.tpRunning.Name = "tpRunning";
            this.tpRunning.Size = new System.Drawing.Size(1071, 599);
            this.tpRunning.TabIndex = 21;
            this.tpRunning.Text = "Состояния";
            // 
            // tableLayoutPanel4
            // 
            this.tableLayoutPanel4.ColumnCount = 1;
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel4.Controls.Add(this.splitContainer4, 0, 0);
            this.tableLayoutPanel4.Controls.Add(this.selectControl1, 0, 1);
            this.tableLayoutPanel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel4.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel4.Name = "tableLayoutPanel4";
            this.tableLayoutPanel4.RowCount = 2;
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 75F));
            this.tableLayoutPanel4.Size = new System.Drawing.Size(1071, 599);
            this.tableLayoutPanel4.TabIndex = 2;
            // 
            // splitContainer4
            // 
            this.splitContainer4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer4.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer4.IsSplitterFixed = true;
            this.splitContainer4.Location = new System.Drawing.Point(3, 3);
            this.splitContainer4.Name = "splitContainer4";
            // 
            // splitContainer4.Panel1
            // 
            this.splitContainer4.Panel1.Controls.Add(this.lstvRun);
            // 
            // splitContainer4.Panel2
            // 
            this.splitContainer4.Panel2.Controls.Add(this.panel2);
            this.splitContainer4.Size = new System.Drawing.Size(1065, 518);
            this.splitContainer4.SplitterDistance = 200;
            this.splitContainer4.TabIndex = 2;
            // 
            // lstvRun
            // 
            this.lstvRun.Activation = System.Windows.Forms.ItemActivation.OneClick;
            this.lstvRun.BackColor = System.Drawing.Color.LightCyan;
            this.lstvRun.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2});
            this.lstvRun.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstvRun.FullRowSelect = true;
            this.lstvRun.GridLines = true;
            this.lstvRun.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.lstvRun.HideSelection = false;
            this.lstvRun.Location = new System.Drawing.Point(0, 0);
            this.lstvRun.MultiSelect = false;
            this.lstvRun.Name = "lstvRun";
            this.lstvRun.Size = new System.Drawing.Size(200, 518);
            this.lstvRun.TabIndex = 1;
            this.lstvRun.UseCompatibleStateImageBehavior = false;
            this.lstvRun.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Дата";
            this.columnHeader1.Width = 195;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Комментарий";
            this.columnHeader2.Width = 200;
            // 
            // selectControl1
            // 
            this.selectControl1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.selectControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.selectControl1.Location = new System.Drawing.Point(3, 527);
            this.selectControl1.Name = "selectControl1";
            this.selectControl1.Size = new System.Drawing.Size(1065, 69);
            this.selectControl1.TabIndex = 0;
            // 
            // tpConfig
            // 
            this.tpConfig.BackColor = System.Drawing.SystemColors.Control;
            this.tpConfig.Controls.Add(this.tableLayoutPanel5);
            this.tpConfig.Location = new System.Drawing.Point(4, 22);
            this.tpConfig.Name = "tpConfig";
            this.tpConfig.Padding = new System.Windows.Forms.Padding(3);
            this.tpConfig.Size = new System.Drawing.Size(1071, 599);
            this.tpConfig.TabIndex = 3;
            this.tpConfig.Text = "Конфигурация и уставки";
            // 
            // tableLayoutPanel5
            // 
            this.tableLayoutPanel5.ColumnCount = 1;
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel5.Controls.Add(this.splitContainer_tpConfig, 0, 0);
            this.tableLayoutPanel5.Controls.Add(this.panel1, 0, 1);
            this.tableLayoutPanel5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel5.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel5.Name = "tableLayoutPanel5";
            this.tableLayoutPanel5.RowCount = 2;
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 75F));
            this.tableLayoutPanel5.Size = new System.Drawing.Size(1065, 593);
            this.tableLayoutPanel5.TabIndex = 1;
            // 
            // splitContainer_tpConfig
            // 
            this.splitContainer_tpConfig.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer_tpConfig.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer_tpConfig.IsSplitterFixed = true;
            this.splitContainer_tpConfig.Location = new System.Drawing.Point(3, 3);
            this.splitContainer_tpConfig.Name = "splitContainer_tpConfig";
            // 
            // splitContainer_tpConfig.Panel1
            // 
            this.splitContainer_tpConfig.Panel1.Controls.Add(this.lstvConfig);
            // 
            // splitContainer_tpConfig.Panel2
            // 
            this.splitContainer_tpConfig.Panel2.Controls.Add(this.pnlTPConfig);
            this.splitContainer_tpConfig.Size = new System.Drawing.Size(1059, 512);
            this.splitContainer_tpConfig.SplitterDistance = 200;
            this.splitContainer_tpConfig.TabIndex = 2;
            // 
            // lstvConfig
            // 
            this.lstvConfig.Activation = System.Windows.Forms.ItemActivation.OneClick;
            this.lstvConfig.BackColor = System.Drawing.Color.LightCyan;
            this.lstvConfig.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.clmnHDate,
            this.clmUstComment});
            this.lstvConfig.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstvConfig.FullRowSelect = true;
            this.lstvConfig.GridLines = true;
            this.lstvConfig.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.lstvConfig.HideSelection = false;
            this.lstvConfig.Location = new System.Drawing.Point(0, 0);
            this.lstvConfig.MultiSelect = false;
            this.lstvConfig.Name = "lstvConfig";
            this.lstvConfig.Size = new System.Drawing.Size(200, 512);
            this.lstvConfig.TabIndex = 1;
            this.lstvConfig.UseCompatibleStateImageBehavior = false;
            this.lstvConfig.View = System.Windows.Forms.View.Details;
            // 
            // clmnHDate
            // 
            this.clmnHDate.Text = "Дата";
            this.clmnHDate.Width = 195;
            // 
            // clmUstComment
            // 
            this.clmUstComment.Text = "Комментарий";
            this.clmUstComment.Width = 200;
            // 
            // pnlTPConfig
            // 
            this.pnlTPConfig.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlTPConfig.Location = new System.Drawing.Point(0, 0);
            this.pnlTPConfig.Name = "pnlTPConfig";
            this.pnlTPConfig.Size = new System.Drawing.Size(855, 512);
            this.pnlTPConfig.TabIndex = 0;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.readWriteUstavky1);
            this.panel1.Controls.Add(this.selectControl2);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(3, 521);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1059, 69);
            this.panel1.TabIndex = 3;
            // 
            // readWriteUstavky1
            // 
            this.readWriteUstavky1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.readWriteUstavky1.Location = new System.Drawing.Point(697, 0);
            this.readWriteUstavky1.Name = "readWriteUstavky1";
            this.readWriteUstavky1.Size = new System.Drawing.Size(293, 60);
            this.readWriteUstavky1.TabIndex = 1;
            // 
            // selectControl2
            // 
            this.selectControl2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.selectControl2.Location = new System.Drawing.Point(0, 0);
            this.selectControl2.Name = "selectControl2";
            this.selectControl2.Size = new System.Drawing.Size(691, 62);
            this.selectControl2.TabIndex = 0;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.mtraNamedFLStatus);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(3, 486);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(1059, 104);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Информация:";
            // 
            // mtraNamedFLStatus
            // 
            this.mtraNamedFLStatus.AutoScroll = true;
            this.mtraNamedFLStatus.BackColor = System.Drawing.SystemColors.Control;
            this.mtraNamedFLStatus.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.mtraNamedFLStatus.Caption = "Информация";
            this.mtraNamedFLStatus.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mtraNamedFLStatus.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.mtraNamedFLStatus.Location = new System.Drawing.Point(3, 16);
            this.mtraNamedFLStatus.Name = "mtraNamedFLStatus";
            this.mtraNamedFLStatus.Size = new System.Drawing.Size(1053, 85);
            this.mtraNamedFLStatus.TabIndex = 0;
            // 
            // panel2
            // 
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(861, 518);
            this.panel2.TabIndex = 1;
            // 
            // FormSirius2OB
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1079, 625);
            this.Controls.Add(this.tabControl);
            this.Name = "FormSirius2OB";
            this.Text = "FormSiriusDevice";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormSiriusDDeviceFormClosing);
            this.Load += new System.EventHandler(this.FormSiriusDDeviceLoad);
            this.tabControl.ResumeLayout(false);
            this.tpCurrentInfo.ResumeLayout(false);
            this.tableLayoutPanel3.ResumeLayout(false);
            this.splitContainer6.Panel1.ResumeLayout(false);
            this.splitContainer6.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer6)).EndInit();
            this.splitContainer6.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.splitContainer7.Panel1.ResumeLayout(false);
            this.splitContainer7.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer7)).EndInit();
            this.splitContainer7.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            this.groupBox5.ResumeLayout(false);
            this.tpRunning.ResumeLayout(false);
            this.tableLayoutPanel4.ResumeLayout(false);
            this.splitContainer4.Panel1.ResumeLayout(false);
            this.splitContainer4.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer4)).EndInit();
            this.splitContainer4.ResumeLayout(false);
            this.tpConfig.ResumeLayout(false);
            this.tableLayoutPanel5.ResumeLayout(false);
            this.splitContainer_tpConfig.Panel1.ResumeLayout(false);
            this.splitContainer_tpConfig.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer_tpConfig)).EndInit();
            this.splitContainer_tpConfig.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage tpCurrentInfo;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.SplitContainer splitContainer6;
        private System.Windows.Forms.GroupBox groupBox2;
        private LabelTextbox.MTRANamedFLPanel mtraNamedFLPanel3;
        private System.Windows.Forms.SplitContainer splitContainer7;
        private System.Windows.Forms.GroupBox groupBox4;
        private LabelTextbox.MTRANamedFLPanel mtraNamedFLPanel7;
        private System.Windows.Forms.GroupBox groupBox5;
        private LabelTextbox.MTRANamedFLPanel mtraNamedFLPanel8;
        private System.Windows.Forms.TabPage tpConfig;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel5;
        private System.Windows.Forms.SplitContainer splitContainer_tpConfig;
        private System.Windows.Forms.ListView lstvConfig;
        private System.Windows.Forms.ColumnHeader clmnHDate;
        private System.Windows.Forms.ColumnHeader clmUstComment;
        private System.Windows.Forms.Panel pnlTPConfig;
        private System.Windows.Forms.Panel panel1;
        private ReadWriteUstavkyControl readWriteUstavky1;
        private SelectControl selectControl2;
        private System.Windows.Forms.TabPage tpRunning;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel4;
        private System.Windows.Forms.SplitContainer splitContainer4;
        private System.Windows.Forms.ListView lstvRun;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private SelectControl selectControl1;
        private System.Windows.Forms.GroupBox groupBox1;
        private LabelTextbox.MTRANamedFLPanel mtraNamedFLStatus;
        private System.Windows.Forms.Panel panel2;
    }
}