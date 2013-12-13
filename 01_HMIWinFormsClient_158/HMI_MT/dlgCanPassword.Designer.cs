namespace HMI_MT
{
	partial class dlgCanPassword
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
			this.tbNameCurrentUser = new System.Windows.Forms.TextBox();
			this.tbPasswordCurrentUser = new System.Windows.Forms.TextBox();
			this.btnCheckRight = new System.Windows.Forms.Button();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.button1 = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// tbNameCurrentUser
			// 
			this.tbNameCurrentUser.Location = new System.Drawing.Point( 185, 27 );
			this.tbNameCurrentUser.Name = "tbNameCurrentUser";
			this.tbNameCurrentUser.Size = new System.Drawing.Size( 141, 20 );
			this.tbNameCurrentUser.TabIndex = 0;
			// 
			// tbPasswordCurrentUser
			// 
			this.tbPasswordCurrentUser.Location = new System.Drawing.Point( 185, 63 );
			this.tbPasswordCurrentUser.Name = "tbPasswordCurrentUser";
			this.tbPasswordCurrentUser.Size = new System.Drawing.Size( 141, 20 );
			this.tbPasswordCurrentUser.TabIndex = 1;
			this.tbPasswordCurrentUser.UseSystemPasswordChar = true;
			// 
			// btnCheckRight
			// 
			this.btnCheckRight.AutoSize = true;
			this.btnCheckRight.Location = new System.Drawing.Point( 122, 104 );
			this.btnCheckRight.Name = "btnCheckRight";
			this.btnCheckRight.Size = new System.Drawing.Size( 123, 23 );
			this.btnCheckRight.TabIndex = 2;
			this.btnCheckRight.Text = "Выполнить проверку";
			this.btnCheckRight.UseVisualStyleBackColor = true;
			this.btnCheckRight.Click += new System.EventHandler( this.btnCheckRight_Click );
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point( 22, 30 );
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size( 157, 13 );
			this.label1.TabIndex = 3;
			this.label1.Text = "Имя текущего пользователя:";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point( 22, 70 );
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size( 48, 13 );
			this.label2.TabIndex = 4;
			this.label2.Text = "Пароль:";
			// 
			// button1
			// 
			this.button1.Location = new System.Drawing.Point( 251, 104 );
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size( 75, 23 );
			this.button1.TabIndex = 5;
			this.button1.Text = "Отменить";
			this.button1.UseVisualStyleBackColor = true;
			this.button1.Click += new System.EventHandler( this.button1_Click );
			// 
			// dlgCanPassword
			// 
			this.AcceptButton = this.btnCheckRight;
			this.AutoScaleDimensions = new System.Drawing.SizeF( 6F, 13F );
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size( 338, 139 );
			this.ControlBox = false;
			this.Controls.Add( this.button1 );
			this.Controls.Add( this.label2 );
			this.Controls.Add( this.label1 );
			this.Controls.Add( this.btnCheckRight );
			this.Controls.Add( this.tbPasswordCurrentUser );
			this.Controls.Add( this.tbNameCurrentUser );
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.Name = "dlgCanPassword";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Безопасность";
			this.ResumeLayout( false );
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.TextBox tbNameCurrentUser;
		private System.Windows.Forms.TextBox tbPasswordCurrentUser;
		private System.Windows.Forms.Button btnCheckRight;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button button1;
	}
}