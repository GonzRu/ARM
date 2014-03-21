namespace HMI_MT
{
	partial class dlgSetSystemTime
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		
		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent( )
		{
			this.components = new System.ComponentModel.Container();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.tbCurrTime = new System.Windows.Forms.TextBox();
			this.tbCurData = new System.Windows.Forms.TextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.dtpTime = new System.Windows.Forms.DateTimePicker();
			this.dtpData = new System.Windows.Forms.DateTimePicker();
			this.button1 = new System.Windows.Forms.Button();
			this.label4 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.button2 = new System.Windows.Forms.Button();
			this.timer1 = new System.Windows.Forms.Timer( this.components );
			this.groupBox1.SuspendLayout();
			this.groupBox2.SuspendLayout();
			this.SuspendLayout();
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add( this.tbCurrTime );
			this.groupBox1.Controls.Add( this.tbCurData );
			this.groupBox1.Controls.Add( this.label2 );
			this.groupBox1.Controls.Add( this.label1 );
			this.groupBox1.Location = new System.Drawing.Point( 26, 12 );
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size( 248, 78 );
			this.groupBox1.TabIndex = 0;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Дата / время АРМ";
			// 
			// tbCurrTime
			// 
			this.tbCurrTime.BackColor = System.Drawing.Color.White;
			this.tbCurrTime.Location = new System.Drawing.Point( 108, 48 );
			this.tbCurrTime.Name = "tbCurrTime";
			this.tbCurrTime.ReadOnly = true;
			this.tbCurrTime.Size = new System.Drawing.Size( 100, 20 );
			this.tbCurrTime.TabIndex = 3;
			// 
			// tbCurData
			// 
			this.tbCurData.BackColor = System.Drawing.Color.White;
			this.tbCurData.Location = new System.Drawing.Point( 108, 26 );
			this.tbCurData.Name = "tbCurData";
			this.tbCurData.ReadOnly = true;
			this.tbCurData.Size = new System.Drawing.Size( 100, 20 );
			this.tbCurData.TabIndex = 2;
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point( 14, 51 );
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size( 90, 13 );
			this.label2.TabIndex = 1;
			this.label2.Text = "Текущее время:";
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point( 14, 29 );
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size( 81, 13 );
			this.label1.TabIndex = 0;
			this.label1.Text = "Текущая дата:";
			// 
			// groupBox2
			// 
			this.groupBox2.Controls.Add( this.dtpTime );
			this.groupBox2.Controls.Add( this.dtpData );
			this.groupBox2.Controls.Add( this.button1 );
			this.groupBox2.Controls.Add( this.label4 );
			this.groupBox2.Controls.Add( this.label3 );
			this.groupBox2.Location = new System.Drawing.Point( 27, 96 );
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size( 247, 112 );
			this.groupBox2.TabIndex = 1;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "Установка даты / времени ПТК";
			// 
			// dtpTime
			// 
			this.dtpTime.Format = System.Windows.Forms.DateTimePickerFormat.Time;
			this.dtpTime.Location = new System.Drawing.Point( 108, 56 );
			this.dtpTime.Name = "dtpTime";
			this.dtpTime.ShowUpDown = true;
			this.dtpTime.Size = new System.Drawing.Size( 125, 20 );
			this.dtpTime.TabIndex = 4;
			// 
			// dtpData
			// 
			this.dtpData.Location = new System.Drawing.Point( 108, 28 );
			this.dtpData.Name = "dtpData";
			this.dtpData.ShowUpDown = true;
			this.dtpData.Size = new System.Drawing.Size( 126, 20 );
			this.dtpData.TabIndex = 3;
			// 
			// button1
			// 
			this.button1.Location = new System.Drawing.Point( 132, 83 );
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size( 75, 23 );
			this.button1.TabIndex = 2;
			this.button1.Text = "Установить";
			this.button1.UseVisualStyleBackColor = true;
			this.button1.Click += new System.EventHandler( this.button1_Click );
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point( 16, 58 );
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size( 43, 13 );
			this.label4.TabIndex = 1;
			this.label4.Text = "Время:";
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point( 15, 32 );
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size( 36, 13 );
			this.label3.TabIndex = 0;
			this.label3.Text = "Дата:";
			// 
			// button2
			// 
			this.button2.Location = new System.Drawing.Point( 199, 227 );
			this.button2.Name = "button2";
			this.button2.Size = new System.Drawing.Size( 75, 23 );
			this.button2.TabIndex = 2;
			this.button2.Text = "Закрыть";
			this.button2.UseVisualStyleBackColor = true;
			this.button2.Click += new System.EventHandler( this.button2_Click );
			// 
			// timer1
			// 
			this.timer1.Interval = 1000;
			this.timer1.Tick += new System.EventHandler( this.timer1_Tick );
			// 
			// dlgSetSystemTime
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF( 6F, 13F );
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size( 297, 259 );
			this.ControlBox = false;
			this.Controls.Add( this.button2 );
			this.Controls.Add( this.groupBox2 );
			this.Controls.Add( this.groupBox1 );
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.Name = "dlgSetSystemTime";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Время системы";
			this.Activated += new System.EventHandler( this.dlgSetSystemTime_Activated );
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler( this.dlgSetSystemTime_FormClosing );
			this.groupBox1.ResumeLayout( false );
			this.groupBox1.PerformLayout();
			this.groupBox2.ResumeLayout( false );
			this.groupBox2.PerformLayout();
			this.ResumeLayout( false );

		}

		#endregion

		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.GroupBox groupBox2;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.TextBox tbCurrTime;
		private System.Windows.Forms.TextBox tbCurData;
		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.Button button2;
		private System.Windows.Forms.DateTimePicker dtpTime;
		private System.Windows.Forms.DateTimePicker dtpData;
		private System.Windows.Forms.Timer timer1;
	}
}