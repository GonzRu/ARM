namespace LabelTextbox
{
    partial class frmSmartLabelCustom
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param Name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose( bool disposing )
        {
            if( disposing && ( components != null ) )
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
        private void InitializeComponent( )
        {
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tbpCustom = new System.Windows.Forms.TabPage();
            this.btnOk = new System.Windows.Forms.Button();
            this.btnChangeTextColor = new System.Windows.Forms.Button();
            this.btnChangeBackColor = new System.Windows.Forms.Button();
            this.btnChangeFont = new System.Windows.Forms.Button();
            this.lblTest = new System.Windows.Forms.Label();
            this.tbpTreeView = new System.Windows.Forms.TabPage();
            this.treeViewKB = new System.Windows.Forms.TreeView();
            this.btnCancel = new System.Windows.Forms.Button();
            this.colorDialog1 = new System.Windows.Forms.ColorDialog();
            this.fontDialog1 = new System.Windows.Forms.FontDialog();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.rbWoFrame = new System.Windows.Forms.RadioButton();
            this.rbFlatFrame = new System.Windows.Forms.RadioButton();
            this.rb3DFrame = new System.Windows.Forms.RadioButton();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tbpCustom.SuspendLayout();
            this.tbpTreeView.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point( 0, 0 );
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add( this.tabControl1 );
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add( this.btnCancel );
            this.splitContainer1.Size = new System.Drawing.Size( 292, 492 );
            this.splitContainer1.SplitterDistance = 461;
            this.splitContainer1.TabIndex = 0;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add( this.tbpCustom );
            this.tabControl1.Controls.Add( this.tbpTreeView );
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point( 0, 0 );
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size( 292, 461 );
            this.tabControl1.TabIndex = 0;
            // 
            // tbpCustom
            // 
            this.tbpCustom.BackColor = System.Drawing.SystemColors.Control;
            this.tbpCustom.Controls.Add( this.groupBox1 );
            this.tbpCustom.Controls.Add( this.btnOk );
            this.tbpCustom.Controls.Add( this.btnChangeTextColor );
            this.tbpCustom.Controls.Add( this.btnChangeBackColor );
            this.tbpCustom.Controls.Add( this.btnChangeFont );
            this.tbpCustom.Controls.Add( this.lblTest );
            this.tbpCustom.Location = new System.Drawing.Point( 4, 22 );
            this.tbpCustom.Name = "tbpCustom";
            this.tbpCustom.Padding = new System.Windows.Forms.Padding( 3 );
            this.tbpCustom.Size = new System.Drawing.Size( 284, 435 );
            this.tbpCustom.TabIndex = 0;
            this.tbpCustom.Text = "Вид";
            // 
            // btnOk
            // 
            this.btnOk.BackColor = System.Drawing.Color.LightPink;
            this.btnOk.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.btnOk.Location = new System.Drawing.Point( 3, 409 );
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size( 278, 23 );
            this.btnOk.TabIndex = 4;
            this.btnOk.Text = "Применить";
            this.btnOk.UseVisualStyleBackColor = false;
            this.btnOk.Click += new System.EventHandler( this.btnOk_Click );
            // 
            // btnChangeTextColor
            // 
            this.btnChangeTextColor.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnChangeTextColor.Location = new System.Drawing.Point( 85, 254 );
            this.btnChangeTextColor.Name = "btnChangeTextColor";
            this.btnChangeTextColor.Size = new System.Drawing.Size( 104, 23 );
            this.btnChangeTextColor.TabIndex = 3;
            this.btnChangeTextColor.Text = "Цвет текста";
            this.btnChangeTextColor.UseVisualStyleBackColor = true;
            this.btnChangeTextColor.Click += new System.EventHandler( this.btnChangeTextColor_Click );
            // 
            // btnChangeBackColor
            // 
            this.btnChangeBackColor.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnChangeBackColor.Location = new System.Drawing.Point( 85, 225 );
            this.btnChangeBackColor.Name = "btnChangeBackColor";
            this.btnChangeBackColor.Size = new System.Drawing.Size( 104, 23 );
            this.btnChangeBackColor.TabIndex = 2;
            this.btnChangeBackColor.Text = "Цвет фона";
            this.btnChangeBackColor.UseVisualStyleBackColor = true;
            this.btnChangeBackColor.Click += new System.EventHandler( this.btnChangeBackColor_Click );
            // 
            // btnChangeFont
            // 
            this.btnChangeFont.AutoSize = true;
            this.btnChangeFont.Location = new System.Drawing.Point( 85, 196 );
            this.btnChangeFont.Name = "btnChangeFont";
            this.btnChangeFont.Size = new System.Drawing.Size( 104, 23 );
            this.btnChangeFont.TabIndex = 1;
            this.btnChangeFont.Text = "Изменить шрифт";
            this.btnChangeFont.UseVisualStyleBackColor = true;
            this.btnChangeFont.Click += new System.EventHandler( this.btnChangeFont_Click );
            // 
            // lblTest
            // 
            this.lblTest.AutoSize = true;
            this.lblTest.Location = new System.Drawing.Point( 105, 107 );
            this.lblTest.Name = "lblTest";
            this.lblTest.Size = new System.Drawing.Size( 64, 13 );
            this.lblTest.TabIndex = 0;
            this.lblTest.Text = "Тест: 0,123";
            // 
            // tbpTreeView
            // 
            this.tbpTreeView.Controls.Add( this.treeViewKB );
            this.tbpTreeView.Location = new System.Drawing.Point( 4, 22 );
            this.tbpTreeView.Name = "tbpTreeView";
            this.tbpTreeView.Padding = new System.Windows.Forms.Padding( 3 );
            this.tbpTreeView.Size = new System.Drawing.Size( 284, 435 );
            this.tbpTreeView.TabIndex = 1;
            this.tbpTreeView.Text = "Привязка";
            this.tbpTreeView.UseVisualStyleBackColor = true;
            // 
            // treeViewKB
            // 
            this.treeViewKB.BackColor = System.Drawing.SystemColors.Control;
            this.treeViewKB.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeViewKB.Location = new System.Drawing.Point( 3, 3 );
            this.treeViewKB.Name = "treeViewKB";
            this.treeViewKB.Size = new System.Drawing.Size( 278, 429 );
            this.treeViewKB.TabIndex = 0;
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point( 205, 3 );
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size( 75, 23 );
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "Отменить";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler( this.btnCancel_Click );
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add( this.rb3DFrame );
            this.groupBox1.Controls.Add( this.rbFlatFrame );
            this.groupBox1.Controls.Add( this.rbWoFrame );
            this.groupBox1.Location = new System.Drawing.Point( 84, 291 );
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size( 104, 89 );
            this.groupBox1.TabIndex = 5;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Вид рамки";
            // 
            // rbWoFrame
            // 
            this.rbWoFrame.AutoSize = true;
            this.rbWoFrame.Location = new System.Drawing.Point( 6, 19 );
            this.rbWoFrame.Name = "rbWoFrame";
            this.rbWoFrame.Size = new System.Drawing.Size( 79, 17 );
            this.rbWoFrame.TabIndex = 0;
            this.rbWoFrame.TabStop = true;
            this.rbWoFrame.Text = "Без рамки";
            this.rbWoFrame.UseVisualStyleBackColor = true;
            this.rbWoFrame.Click += new System.EventHandler( this.rbWoFrame_Click );
            // 
            // rbFlatFrame
            // 
            this.rbFlatFrame.AutoSize = true;
            this.rbFlatFrame.Location = new System.Drawing.Point( 6, 42 );
            this.rbFlatFrame.Name = "rbFlatFrame";
            this.rbFlatFrame.Size = new System.Drawing.Size( 69, 17 );
            this.rbFlatFrame.TabIndex = 1;
            this.rbFlatFrame.TabStop = true;
            this.rbFlatFrame.Text = "Плоская";
            this.rbFlatFrame.UseVisualStyleBackColor = true;
            this.rbFlatFrame.Click += new System.EventHandler( this.rbWoFrame_Click );
            // 
            // rb3DFrame
            // 
            this.rb3DFrame.AutoSize = true;
            this.rb3DFrame.Location = new System.Drawing.Point( 6, 65 );
            this.rb3DFrame.Name = "rb3DFrame";
            this.rb3DFrame.Size = new System.Drawing.Size( 78, 17 );
            this.rb3DFrame.TabIndex = 2;
            this.rb3DFrame.TabStop = true;
            this.rb3DFrame.Text = "Объемная";
            this.rb3DFrame.UseVisualStyleBackColor = true;
            this.rb3DFrame.Click += new System.EventHandler( this.rbWoFrame_Click );
            // 
            // frmSmartLabelCustom
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF( 6F, 13F );
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size( 292, 492 );
            this.ControlBox = false;
            this.Controls.Add( this.splitContainer1 );
            this.DoubleBuffered = true;
            this.Name = "frmSmartLabelCustom";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Настройка";
            this.Load += new System.EventHandler( this.frmSmartLabelCustom_Load );
            this.splitContainer1.Panel1.ResumeLayout( false );
            this.splitContainer1.Panel2.ResumeLayout( false );
            this.splitContainer1.ResumeLayout( false );
            this.tabControl1.ResumeLayout( false );
            this.tbpCustom.ResumeLayout( false );
            this.tbpCustom.PerformLayout();
            this.tbpTreeView.ResumeLayout( false );
            this.groupBox1.ResumeLayout( false );
            this.groupBox1.PerformLayout();
            this.ResumeLayout( false );

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tbpCustom;
        private System.Windows.Forms.TabPage tbpTreeView;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.TreeView treeViewKB;
        private System.Windows.Forms.Button btnChangeFont;
        private System.Windows.Forms.Label lblTest;
        private System.Windows.Forms.Button btnChangeTextColor;
        private System.Windows.Forms.Button btnChangeBackColor;
        private System.Windows.Forms.ColorDialog colorDialog1;
        private System.Windows.Forms.FontDialog fontDialog1;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton rb3DFrame;
        private System.Windows.Forms.RadioButton rbFlatFrame;
        private System.Windows.Forms.RadioButton rbWoFrame;
    }
}