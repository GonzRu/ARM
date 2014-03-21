namespace HMI_MT
{
    partial class frmAutorization
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
        private void InitializeComponent()
        {
			this.tbUser = new System.Windows.Forms.TextBox();
			this.tbPassword = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.btnGotoSystem = new System.Windows.Forms.Button();
			this.btnExitSystem = new System.Windows.Forms.Button();
			this.btnBlockPC = new System.Windows.Forms.Button();
			this.keyboardLayout1 = new HMI_MT.KeyboardLayout();
			this.SuspendLayout();
			// 
			// tbUser
			// 
			this.tbUser.Location = new System.Drawing.Point(109, 12);
			this.tbUser.Name = "tbUser";
			this.tbUser.Size = new System.Drawing.Size(120, 20);
			this.tbUser.TabIndex = 0;
			this.tbUser.Leave += new System.EventHandler(this.tbUser_Leave);
			// 
			// tbPassword
			// 
			this.tbPassword.Location = new System.Drawing.Point(109, 38);
			this.tbPassword.Name = "tbPassword";
			this.tbPassword.Size = new System.Drawing.Size(120, 20);
			this.tbPassword.TabIndex = 1;
			this.tbPassword.UseSystemPasswordChar = true;
			this.tbPassword.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tbPassword_KeyDown);
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(16, 15);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(83, 13);
			this.label1.TabIndex = 2;
			this.label1.Text = "Пользователь:";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(16, 41);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(48, 13);
			this.label2.TabIndex = 3;
			this.label2.Text = "Пароль:";
			// 
			// btnGotoSystem
			// 
			this.btnGotoSystem.AutoSize = true;
			this.btnGotoSystem.Location = new System.Drawing.Point(14, 98);
			this.btnGotoSystem.Name = "btnGotoSystem";
			this.btnGotoSystem.Size = new System.Drawing.Size(101, 23);
			this.btnGotoSystem.TabIndex = 4;
			this.btnGotoSystem.Text = "Войти в систему";
			this.btnGotoSystem.UseVisualStyleBackColor = true;
			this.btnGotoSystem.Click += new System.EventHandler(this.button1_Click);
			// 
			// btnExitSystem
			// 
			this.btnExitSystem.AutoSize = true;
			this.btnExitSystem.Location = new System.Drawing.Point(129, 98);
			this.btnExitSystem.Name = "btnExitSystem";
			this.btnExitSystem.Size = new System.Drawing.Size(110, 23);
			this.btnExitSystem.TabIndex = 5;
			this.btnExitSystem.Text = "Завершить работу";
			this.btnExitSystem.UseVisualStyleBackColor = true;
			this.btnExitSystem.Click += new System.EventHandler(this.button2_Click);
			// 
			// btnBlockPC
			// 
			this.btnBlockPC.Location = new System.Drawing.Point(11, 127);
			this.btnBlockPC.Name = "btnBlockPC";
			this.btnBlockPC.Size = new System.Drawing.Size(228, 23);
			this.btnBlockPC.TabIndex = 6;
			this.btnBlockPC.Text = "Блокировать компьютер?";
			this.btnBlockPC.UseVisualStyleBackColor = true;
			this.btnBlockPC.Visible = false;
			this.btnBlockPC.Click += new System.EventHandler(this.btnBlockPC_Click);
			// 
			// keyboardLayout1
			// 
			this.keyboardLayout1.Location = new System.Drawing.Point(92, 64);
			this.keyboardLayout1.Name = "keyboardLayout1";
			this.keyboardLayout1.Size = new System.Drawing.Size(137, 28);
			this.keyboardLayout1.TabIndex = 7;
			// 
			// frmAutorization
			// 
			this.AcceptButton = this.btnGotoSystem;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(252, 156);
			this.ControlBox = false;
			this.Controls.Add(this.keyboardLayout1);
			this.Controls.Add(this.btnBlockPC);
			this.Controls.Add(this.btnExitSystem);
			this.Controls.Add(this.btnGotoSystem);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.tbPassword);
			this.Controls.Add(this.tbUser);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "frmAutorization";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Вход в систему";
			this.TopMost = true;
			this.Load += new System.EventHandler(this.frmAutorization_Load);
			this.InputLanguageChanged += new System.Windows.Forms.InputLanguageChangedEventHandler(this.frmAutorization_InputLanguageChanged);
			this.ResumeLayout(false);
			this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox tbUser;
        private System.Windows.Forms.TextBox tbPassword;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnGotoSystem;
        private System.Windows.Forms.Button btnExitSystem;
        private System.Windows.Forms.Button btnBlockPC;
        private KeyboardLayout keyboardLayout1;
    }
}