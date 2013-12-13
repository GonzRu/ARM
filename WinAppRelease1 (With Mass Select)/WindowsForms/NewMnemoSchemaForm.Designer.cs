namespace WindowsForms
{
   partial class NewMnemoSchemaForm
   {
      /// <summary>
      /// Required designer variable.
      /// </summary>
      private System.ComponentModel.IContainer components = null;

      /// <summary>
      /// Clean up any resources being used.
      /// </summary>
      /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
      protected override void Dispose(bool disposing)
      {
         if (disposing && (components != null))
         {
            components.Dispose();
         }
         base.Dispose(disposing);
      }

      #region Windows Form Designer generated code

      /// <summary>
      /// Required method for Designer support - do not modify
      /// the contents of this method with the code editor.
      /// </summary>
      private void InitializeComponent()
      {
         this.groupBox1 = new System.Windows.Forms.GroupBox();
         this.radioButton4 = new System.Windows.Forms.RadioButton();
         this.radioButton3 = new System.Windows.Forms.RadioButton();
         this.radioButton2 = new System.Windows.Forms.RadioButton();
         this.radioButton1 = new System.Windows.Forms.RadioButton();
         this.label1 = new System.Windows.Forms.Label();
         this.numericUpDown2 = new System.Windows.Forms.NumericUpDown();
         this.numericUpDown1 = new System.Windows.Forms.NumericUpDown();
         this.button1 = new System.Windows.Forms.Button();
         this.button2 = new System.Windows.Forms.Button();
         this.groupBox1.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.numericUpDown2)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).BeginInit();
         this.SuspendLayout();
         // 
         // groupBox1
         // 
         this.groupBox1.Controls.Add(this.radioButton4);
         this.groupBox1.Controls.Add(this.radioButton3);
         this.groupBox1.Controls.Add(this.radioButton2);
         this.groupBox1.Controls.Add(this.radioButton1);
         this.groupBox1.Controls.Add(this.label1);
         this.groupBox1.Controls.Add(this.numericUpDown2);
         this.groupBox1.Controls.Add(this.numericUpDown1);
         this.groupBox1.Location = new System.Drawing.Point(12, 12);
         this.groupBox1.Name = "groupBox1";
         this.groupBox1.Size = new System.Drawing.Size(164, 159);
         this.groupBox1.TabIndex = 0;
         this.groupBox1.TabStop = false;
         this.groupBox1.Text = "Разрешение";
         // 
         // radioButton4
         // 
         this.radioButton4.AutoSize = true;
         this.radioButton4.FlatStyle = System.Windows.Forms.FlatStyle.System;
         this.radioButton4.Location = new System.Drawing.Point(41, 102);
         this.radioButton4.Name = "radioButton4";
         this.radioButton4.Size = new System.Drawing.Size(109, 18);
         this.radioButton4.TabIndex = 4;
         this.radioButton4.Text = "Другой размер";
         this.radioButton4.UseVisualStyleBackColor = true;
         this.radioButton4.CheckedChanged += new System.EventHandler(this.radioButton4_CheckedChanged);
         // 
         // radioButton3
         // 
         this.radioButton3.AutoSize = true;
         this.radioButton3.FlatStyle = System.Windows.Forms.FlatStyle.System;
         this.radioButton3.Location = new System.Drawing.Point(41, 79);
         this.radioButton3.Name = "radioButton3";
         this.radioButton3.Size = new System.Drawing.Size(84, 18);
         this.radioButton3.TabIndex = 3;
         this.radioButton3.Text = "1600x1200";
         this.radioButton3.UseVisualStyleBackColor = true;
         // 
         // radioButton2
         // 
         this.radioButton2.AutoSize = true;
         this.radioButton2.FlatStyle = System.Windows.Forms.FlatStyle.System;
         this.radioButton2.Location = new System.Drawing.Point(41, 56);
         this.radioButton2.Name = "radioButton2";
         this.radioButton2.Size = new System.Drawing.Size(84, 18);
         this.radioButton2.TabIndex = 2;
         this.radioButton2.Text = "1280x1024";
         this.radioButton2.UseVisualStyleBackColor = true;
         // 
         // radioButton1
         // 
         this.radioButton1.AutoSize = true;
         this.radioButton1.Checked = true;
         this.radioButton1.FlatStyle = System.Windows.Forms.FlatStyle.System;
         this.radioButton1.Location = new System.Drawing.Point(41, 33);
         this.radioButton1.Name = "radioButton1";
         this.radioButton1.Size = new System.Drawing.Size(78, 18);
         this.radioButton1.TabIndex = 1;
         this.radioButton1.TabStop = true;
         this.radioButton1.Text = "1024x768";
         this.radioButton1.UseVisualStyleBackColor = true;
         // 
         // label1
         // 
         this.label1.AutoSize = true;
         this.label1.Location = new System.Drawing.Point(76, 127);
         this.label1.Name = "label1";
         this.label1.Size = new System.Drawing.Size(12, 13);
         this.label1.TabIndex = 6;
         this.label1.Text = "x";
         // 
         // numericUpDown2
         // 
         this.numericUpDown2.Enabled = false;
         this.numericUpDown2.Location = new System.Drawing.Point(94, 125);
         this.numericUpDown2.Maximum = new decimal(new int[] {
            3600,
            0,
            0,
            0});
         this.numericUpDown2.Minimum = new decimal(new int[] {
            480,
            0,
            0,
            0});
         this.numericUpDown2.Name = "numericUpDown2";
         this.numericUpDown2.Size = new System.Drawing.Size(64, 20);
         this.numericUpDown2.TabIndex = 6;
         this.numericUpDown2.Value = new decimal(new int[] {
            480,
            0,
            0,
            0});
         // 
         // numericUpDown1
         // 
         this.numericUpDown1.Enabled = false;
         this.numericUpDown1.Location = new System.Drawing.Point(6, 125);
         this.numericUpDown1.Maximum = new decimal(new int[] {
            4800,
            0,
            0,
            0});
         this.numericUpDown1.Minimum = new decimal(new int[] {
            640,
            0,
            0,
            0});
         this.numericUpDown1.Name = "numericUpDown1";
         this.numericUpDown1.Size = new System.Drawing.Size(64, 20);
         this.numericUpDown1.TabIndex = 5;
         this.numericUpDown1.Value = new decimal(new int[] {
            640,
            0,
            0,
            0});
         // 
         // button1
         // 
         this.button1.DialogResult = System.Windows.Forms.DialogResult.OK;
         this.button1.Location = new System.Drawing.Point(12, 177);
         this.button1.Name = "button1";
         this.button1.Size = new System.Drawing.Size(75, 23);
         this.button1.TabIndex = 7;
         this.button1.Text = "Принять";
         this.button1.UseVisualStyleBackColor = true;
         // 
         // button2
         // 
         this.button2.DialogResult = System.Windows.Forms.DialogResult.Cancel;
         this.button2.Location = new System.Drawing.Point(101, 177);
         this.button2.Name = "button2";
         this.button2.Size = new System.Drawing.Size(75, 23);
         this.button2.TabIndex = 8;
         this.button2.Text = "Отменить";
         this.button2.UseVisualStyleBackColor = true;
         // 
         // Form2
         // 
         this.AccessibleRole = System.Windows.Forms.AccessibleRole.Window;
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.ClientSize = new System.Drawing.Size(188, 212);
         this.ControlBox = false;
         this.Controls.Add(this.button2);
         this.Controls.Add(this.button1);
         this.Controls.Add(this.groupBox1);
         this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
         this.MaximizeBox = false;
         this.MinimizeBox = false;
         this.Name = "Form2";
         this.ShowIcon = false;
         this.ShowInTaskbar = false;
         this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
         this.Text = "Схема";
         this.TopMost = true;
         this.groupBox1.ResumeLayout(false);
         this.groupBox1.PerformLayout();
         ((System.ComponentModel.ISupportInitialize)(this.numericUpDown2)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).EndInit();
         this.ResumeLayout(false);

      }

      #endregion

      private System.Windows.Forms.GroupBox groupBox1;
      private System.Windows.Forms.Button button1;
      private System.Windows.Forms.Button button2;
      private System.Windows.Forms.Label label1;
      private System.Windows.Forms.NumericUpDown numericUpDown2;
      private System.Windows.Forms.NumericUpDown numericUpDown1;
      private System.Windows.Forms.RadioButton radioButton4;
      private System.Windows.Forms.RadioButton radioButton3;
      private System.Windows.Forms.RadioButton radioButton2;
      private System.Windows.Forms.RadioButton radioButton1;
   }
}