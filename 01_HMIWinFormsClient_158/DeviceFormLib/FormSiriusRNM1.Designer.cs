namespace DeviceFormLib
{
    partial class FormSiriusRNM1
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
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.mtraNamedFLPanel2 = new LabelTextbox.MTRANamedFLPanel(this.components);
            this.splitContainer7 = new System.Windows.Forms.SplitContainer();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.mtraNamedFLPanel7 = new LabelTextbox.MTRANamedFLPanel(this.components);
            this.splitContainer11 = new System.Windows.Forms.SplitContainer();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.mtraNamedFLPanel8 = new LabelTextbox.MTRANamedFLPanel(this.components);
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.groupBox7 = new System.Windows.Forms.GroupBox();
            this.mtraNamedFLPanel9 = new LabelTextbox.MTRANamedFLPanel(this.components);
            this.groupBox8 = new System.Windows.Forms.GroupBox();
            this.mtraNamedFLPanel10 = new LabelTextbox.MTRANamedFLPanel(this.components);
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
            this.tabControl.SuspendLayout();
            this.tpCurrentInfo.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer6)).BeginInit();
            this.splitContainer6.Panel1.SuspendLayout();
            this.splitContainer6.Panel2.SuspendLayout();
            this.splitContainer6.SuspendLayout();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer7)).BeginInit();
            this.splitContainer7.Panel1.SuspendLayout();
            this.splitContainer7.Panel2.SuspendLayout();
            this.splitContainer7.SuspendLayout();
            this.groupBox4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer11)).BeginInit();
            this.splitContainer11.Panel1.SuspendLayout();
            this.splitContainer11.Panel2.SuspendLayout();
            this.splitContainer11.SuspendLayout();
            this.groupBox5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.groupBox7.SuspendLayout();
            this.groupBox8.SuspendLayout();
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
            this.splitContainer6.Panel1.Controls.Add(this.groupBox3);
            // 
            // splitContainer6.Panel2
            // 
            this.splitContainer6.Panel2.Controls.Add(this.splitContainer7);
            this.splitContainer6.Size = new System.Drawing.Size(1059, 477);
            this.splitContainer6.SplitterDistance = 334;
            this.splitContainer6.TabIndex = 1;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.mtraNamedFLPanel2);
            this.groupBox3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox3.Location = new System.Drawing.Point(0, 0);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(334, 477);
            this.groupBox3.TabIndex = 0;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Измерения:";
            // 
            // mtraNamedFLPanel2
            // 
            this.mtraNamedFLPanel2.AutoScroll = true;
            this.mtraNamedFLPanel2.BackColor = System.Drawing.SystemColors.Control;
            this.mtraNamedFLPanel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.mtraNamedFLPanel2.Caption = "Измерения";
            this.mtraNamedFLPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mtraNamedFLPanel2.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.mtraNamedFLPanel2.Location = new System.Drawing.Point(3, 16);
            this.mtraNamedFLPanel2.Name = "mtraNamedFLPanel2";
            this.mtraNamedFLPanel2.Size = new System.Drawing.Size(328, 458);
            this.mtraNamedFLPanel2.TabIndex = 0;
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
            this.splitContainer7.Panel2.Controls.Add(this.splitContainer11);
            this.splitContainer7.Size = new System.Drawing.Size(721, 477);
            this.splitContainer7.SplitterDistance = 249;
            this.splitContainer7.TabIndex = 0;
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.mtraNamedFLPanel7);
            this.groupBox4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox4.Location = new System.Drawing.Point(0, 0);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(249, 477);
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
            this.mtraNamedFLPanel7.Size = new System.Drawing.Size(243, 458);
            this.mtraNamedFLPanel7.TabIndex = 0;
            // 
            // splitContainer11
            // 
            this.splitContainer11.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer11.Location = new System.Drawing.Point(0, 0);
            this.splitContainer11.Name = "splitContainer11";
            // 
            // splitContainer11.Panel1
            // 
            this.splitContainer11.Panel1.Controls.Add(this.groupBox5);
            // 
            // splitContainer11.Panel2
            // 
            this.splitContainer11.Panel2.Controls.Add(this.splitContainer2);
            this.splitContainer11.Size = new System.Drawing.Size(468, 477);
            this.splitContainer11.SplitterDistance = 240;
            this.splitContainer11.TabIndex = 0;
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.mtraNamedFLPanel8);
            this.groupBox5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox5.Location = new System.Drawing.Point(0, 0);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(240, 477);
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
            this.mtraNamedFLPanel8.Size = new System.Drawing.Size(234, 458);
            this.mtraNamedFLPanel8.TabIndex = 0;
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
            this.splitContainer2.Panel1.Controls.Add(this.groupBox7);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.groupBox8);
            this.splitContainer2.Size = new System.Drawing.Size(224, 477);
            this.splitContainer2.SplitterDistance = 238;
            this.splitContainer2.TabIndex = 0;
            // 
            // groupBox7
            // 
            this.groupBox7.Controls.Add(this.mtraNamedFLPanel9);
            this.groupBox7.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox7.Location = new System.Drawing.Point(0, 0);
            this.groupBox7.Name = "groupBox7";
            this.groupBox7.Size = new System.Drawing.Size(224, 238);
            this.groupBox7.TabIndex = 0;
            this.groupBox7.TabStop = false;
            this.groupBox7.Text = "Неисправности:";
            // 
            // mtraNamedFLPanel9
            // 
            this.mtraNamedFLPanel9.AutoScroll = true;
            this.mtraNamedFLPanel9.BackColor = System.Drawing.SystemColors.Control;
            this.mtraNamedFLPanel9.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.mtraNamedFLPanel9.Caption = "Неисправности";
            this.mtraNamedFLPanel9.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mtraNamedFLPanel9.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.mtraNamedFLPanel9.Location = new System.Drawing.Point(3, 16);
            this.mtraNamedFLPanel9.Name = "mtraNamedFLPanel9";
            this.mtraNamedFLPanel9.Size = new System.Drawing.Size(218, 219);
            this.mtraNamedFLPanel9.TabIndex = 0;
            // 
            // groupBox8
            // 
            this.groupBox8.Controls.Add(this.mtraNamedFLPanel10);
            this.groupBox8.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox8.Location = new System.Drawing.Point(0, 0);
            this.groupBox8.Name = "groupBox8";
            this.groupBox8.Size = new System.Drawing.Size(224, 235);
            this.groupBox8.TabIndex = 0;
            this.groupBox8.TabStop = false;
            this.groupBox8.Text = "Контроль терминала:";
            // 
            // mtraNamedFLPanel10
            // 
            this.mtraNamedFLPanel10.AutoScroll = true;
            this.mtraNamedFLPanel10.BackColor = System.Drawing.SystemColors.Control;
            this.mtraNamedFLPanel10.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.mtraNamedFLPanel10.Caption = "Контроль терминала";
            this.mtraNamedFLPanel10.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mtraNamedFLPanel10.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.mtraNamedFLPanel10.Location = new System.Drawing.Point(3, 16);
            this.mtraNamedFLPanel10.Name = "mtraNamedFLPanel10";
            this.mtraNamedFLPanel10.Size = new System.Drawing.Size(218, 216);
            this.mtraNamedFLPanel10.TabIndex = 0;
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
            this.groupBox1.Text = "Светодиоды:";
            // 
            // mtraNamedFLStatus
            // 
            this.mtraNamedFLStatus.AutoScroll = true;
            this.mtraNamedFLStatus.BackColor = System.Drawing.SystemColors.Control;
            this.mtraNamedFLStatus.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.mtraNamedFLStatus.Caption = "Светодиоды";
            this.mtraNamedFLStatus.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mtraNamedFLStatus.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.mtraNamedFLStatus.Location = new System.Drawing.Point(3, 16);
            this.mtraNamedFLStatus.Name = "mtraNamedFLStatus";
            this.mtraNamedFLStatus.Size = new System.Drawing.Size(1053, 85);
            this.mtraNamedFLStatus.TabIndex = 0;
            // 
            // FormSiriusRNM1201Device
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1079, 625);
            this.Controls.Add(this.tabControl);
            this.Name = "FormSiriusRNM1201Device";
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
            this.groupBox3.ResumeLayout(false);
            this.splitContainer7.Panel1.ResumeLayout(false);
            this.splitContainer7.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer7)).EndInit();
            this.splitContainer7.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            this.splitContainer11.Panel1.ResumeLayout(false);
            this.splitContainer11.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer11)).EndInit();
            this.splitContainer11.ResumeLayout(false);
            this.groupBox5.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.groupBox7.ResumeLayout(false);
            this.groupBox8.ResumeLayout(false);
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
        private System.Windows.Forms.GroupBox groupBox3;
        private LabelTextbox.MTRANamedFLPanel mtraNamedFLPanel2;
        private System.Windows.Forms.SplitContainer splitContainer7;
        private System.Windows.Forms.GroupBox groupBox4;
        private LabelTextbox.MTRANamedFLPanel mtraNamedFLPanel7;
        private System.Windows.Forms.SplitContainer splitContainer11;
        private System.Windows.Forms.GroupBox groupBox5;
        private LabelTextbox.MTRANamedFLPanel mtraNamedFLPanel8;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.GroupBox groupBox7;
        private LabelTextbox.MTRANamedFLPanel mtraNamedFLPanel9;
        private System.Windows.Forms.GroupBox groupBox8;
        private LabelTextbox.MTRANamedFLPanel mtraNamedFLPanel10;
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
        private System.Windows.Forms.GroupBox groupBox1;
        private LabelTextbox.MTRANamedFLPanel mtraNamedFLStatus;
    }
}