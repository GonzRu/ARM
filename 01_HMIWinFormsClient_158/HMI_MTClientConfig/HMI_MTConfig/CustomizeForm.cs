using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace HMI_MTConfig
{
   public class CustomizeForm : Form
   {
       public CustomizeForm()
       { 
           this.FormClosing +=new FormClosingEventHandler(CustomizeForm_FormClosing);
       }
       /// <summary>
       /// Признак того что dgw редактировался и его нужно сохранить
       /// </summary>
       public bool IsDgwEdit = false;

       public string Caption4MessageBox = "Настройка конфигурации проекта";

       /// <summary>
       /// Действия по применить для конкр формы
       /// </summary>
      public virtual void AplayChangesCF()
      {}
       /// <summary>
       /// Действия по Отменить
       /// </summary>
      public virtual void CancelChangesCF()
      { }
      public virtual void SetError(string p)
      {
          MessageBox.Show(p,Caption4MessageBox, MessageBoxButtons.OK, MessageBoxIcon.Error);
      }

      private void InitializeComponent()
      {
          this.SuspendLayout();
          // 
          // CustomizeForm
          // 
          this.ClientSize = new System.Drawing.Size(292, 266);
          this.Name = "CustomizeForm";
          this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.CustomizeForm_FormClosing);
          this.ResumeLayout(false);

      }

      private void CustomizeForm_FormClosing(object sender, FormClosingEventArgs e)
      {
          if (!IsDgwEdit)
              return;

          switch (MessageBox.Show("Запомнить изменения?", Caption4MessageBox + " : Конфигурирование" + this.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1).ToString())
          {
              case "Yes":
                  SaveNewValue();
                  break;
              case "No":
                  break;
          }
      }
      public virtual void SaveNewValue()
      {
      }
   }
}
