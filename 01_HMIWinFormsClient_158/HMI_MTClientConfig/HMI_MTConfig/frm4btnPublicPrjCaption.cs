using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace HMI_MTConfig
{
   public partial class frm4btnPublicPrjCaption : CustomizeForm
   {
       #region Данные для Отмены
       string old_NamePrg = string.Empty;
       string old_NamePTK = string.Empty;
       string old_NamePTKStatusBar = string.Empty;
       string old_Divg = string.Empty;
       #endregion

      /// <summary>
      /// таблица с общими названиями проекта
      /// </summary>
      DataTable dtPrjCaption;

      public frm4btnPublicPrjCaption()
      {
         InitializeComponent();
         InitForm();
      }

      /// <summary>
      /// Формирование формы для исходной группы параметров
      /// </summary>
      private void InitForm()
      {
          try
          {
              dtPrjCaption = new DataTable();

              dtPrjCaption.Columns.Add("NameXML", typeof(System.String));
              dtPrjCaption.Columns["NameXML"].Caption = "Имя xml-тега";

              dtPrjCaption.Columns.Add("Target", typeof(System.String));
              dtPrjCaption.Columns["Target"].Caption = "Назначение";

              dtPrjCaption.Columns.Add("CaptionValue", typeof(System.String));
              dtPrjCaption.Columns["CaptionValue"].Caption = "UI-значение";

              dgwPrjCaption.DataSource = dtPrjCaption;

              foreach (DataGridViewColumn dgvc in dgwPrjCaption.Columns)
                  dgvc.HeaderText = dtPrjCaption.Columns[dgvc.Index].Caption;

              #region заполняем форму
              if (!String.IsNullOrWhiteSpace((string)Program.xdoc_Project_cfg.Element("Project").Element("NamePrg")))
              {
                  AddRowInDT("NamePrg");
                  old_NamePrg = Program.xdoc_Project_cfg.Element("Project").Element("NamePrg").Value;
              }
              if (!String.IsNullOrWhiteSpace((string)Program.xdoc_Project_cfg.Element("Project").Element("NamePTK")))
              {
                  AddRowInDT("NamePTK");
                  old_NamePTK = Program.xdoc_Project_cfg.Element("Project").Element("NamePTK").Value;
              }
              if (!String.IsNullOrWhiteSpace((string)Program.xdoc_Project_cfg.Element("Project").Element("NamePTKStatusBar")))
              {
                  AddRowInDT("NamePTKStatusBar");
                  old_NamePTKStatusBar = Program.xdoc_Project_cfg.Element("Project").Element("NamePTKStatusBar").Value;
              }
              if (!String.IsNullOrWhiteSpace((string)Program.xdoc_Project_cfg.Element("Project").Element("Divg")))
              {
                  AddRowInDT("Divg");
                  old_Divg = Program.xdoc_Project_cfg.Element("Project").Element("Divg").Value;
              }
              #endregion
          }
          catch (Exception ex)
          {
              SetError(ex.Message);
              return;
          }
      }

      private void AddRowInDT(string nametag)
      {
         DataRow dr = dtPrjCaption.NewRow();

         dr["NameXML"] = nametag;

          if (Program.xdoc_Project_cfg.Element("Project").Element(nametag).Attribute("UIComment") == null)
              throw new Exception("В описании тега отсутствует аттрибут =UIComment=");

         dr["Target"] = Program.xdoc_Project_cfg.Element("Project").Element(nametag).Attribute("UIComment").Value;
         dr["CaptionValue"] = Program.xdoc_Project_cfg.Element("Project").Element(nametag).Value;

         dtPrjCaption.Rows.Add(dr);
      }

      private void dgwPrjCaption_MouseClick(object sender, MouseEventArgs e)
      {
          IsDgwEdit = true;
      }

       public override void AplayChangesCF()
      {
         SaveNewValue();
      }

       /// <summary>
       /// сохранить новые значения
       /// </summary>
       public override void SaveNewValue()
      {
         foreach (DataRow dr in dtPrjCaption.Rows)
         {
            Program.xdoc_Project_cfg.Element("Project").Element(dr["NameXML"].ToString()).Value = dr["CaptionValue"].ToString();
         }
         IsDgwEdit = false;
      }
      /// <summary>
      /// Отменить из главного окна
      /// </summary>
      public override void CancelChangesCF()
      {
          SaveOldValue();
      }
      /// <summary>
      /// Собственно действие по Отменить
      /// </summary>
      private void SaveOldValue()
      {
          Program.xdoc_Project_cfg.Element("Project").Element("NamePrg").Value = old_NamePrg;
          Program.xdoc_Project_cfg.Element("Project").Element("NamePTK").Value = old_NamePTK;
          Program.xdoc_Project_cfg.Element("Project").Element("NamePTKStatusBar").Value = old_NamePTKStatusBar;
          Program.xdoc_Project_cfg.Element("Project").Element("Divg").Value = old_Divg;
          IsDgwEdit = false;
      }
   }
}
