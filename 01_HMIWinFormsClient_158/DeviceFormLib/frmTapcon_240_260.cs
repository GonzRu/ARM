using System;
using System.IO;
using System.Collections.Generic;
using System.Collections;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Calculator;
using LabelTextbox;
using CRZADevices;
using CommonUtils;
using System.Xml.Linq;
using System.Data.Common;
using System.Data.SqlClient;
using FileManager;
using LibraryElements;
using WindowsForms;
using Structure;
using System.Text.RegularExpressions;

namespace HMI_MT
{
   public partial class frmTapcon_240_260 : frmBMRZbase
   {
      StringBuilder sb = new StringBuilder();
      byte[] memXOut;

      public frmTapcon_240_260()
      {
         InitializeComponent();
      }

      public frmTapcon_240_260(MainForm linkMainForm, int iFC, int iIDDev, string fxml)
         : base( linkMainForm, iFC, iIDDev, fxml )
      {
         InitializeComponent( );
      }

      private void btnRSE_Click(object sender, EventArgs e)
      {
         switch ((sender as Button).Name)
         {
            case "btnRSE":
               if (parent.newKB.ExecuteCommand(IFC, IIDDev, "RSE", String.Empty, null, parent.toolStripProgressBar1, parent.statusStrip1, parent))
                  parent.WriteEventToLog(35, "Команда \"RSE\" ушла в сеть. Устройство - "
                     + IFC.ToString() + "." + IIDDev.ToString(), this.Name, true);// true, false);
               break;

            case "btnLWR":
               if (parent.newKB.ExecuteCommand(IFC, IIDDev, "LWR", String.Empty, null, parent.toolStripProgressBar1, parent.statusStrip1, parent))
                  parent.WriteEventToLog(35, "Команда \"LWR\" ушла в сеть. Устройство - "
                     + IFC.ToString() + "." + IIDDev.ToString(), this.Name, true);// true, false);
               break;

            case "btnMAN":
               using (dlgTapconCMD dtcmd = new dlgTapconCMD())
               {
                  //if (parent.newKB.ExecuteCommand(IFC, IIDDev, "MAN", String.Empty, null, parent.toolStripProgressBar1, parent.statusStrip1, parent))
                  //   parent.WriteEventToLog(35, "Команда \"MAN\" ушла в сеть. Устройство - "
                  //      + IFC.ToString() + "." + IIDDev.ToString(), this.Name, true);// true, false);

                  dtcmd.pnlRB.Visible = true;
                  dtcmd.pnlRB.Dock = DockStyle.Fill;
                  dtcmd.pnlTB.Visible = false;
                  dtcmd.Text = "Параметры команды MAN - Auto/Manual mode";
                  dtcmd.rbOn.Text = "ON – set Auto mode";
                  dtcmd.rbOff.Text = "OFF – set Manual mode";

                  if (DialogResult.Cancel == dtcmd.ShowDialog())
                     return;

                  if (dtcmd.rbOn.Checked)
                  {
                     sb.Length = 0;
                     sb.Append("ON" + "\0");
                     /* 
                      * необходимость использования двух массивов в том,
                      * что GetBytes() обрезает выходной массив до фактической длины строки
                      * и для сохранния соглашения о длине поля параметров необходимо 
                      * расширять эту строку до нужной длины
                      * строка "ON" + "\0" будет 3 байта, а нужно 4 байта
                      */
                     byte[] memxout = new byte[4];
                     memxout = Encoding.UTF8.GetBytes(sb.ToString());
                     memXOut = new byte[4];
                     Buffer.BlockCopy(memxout, 0, memXOut, 0, memxout.Length);
                  }
                  else if (dtcmd.rbOff.Checked)
                  {
                     sb.Length = 0;
                     sb.Append("OFF" + "\0");
                     memXOut = new byte[4];
                     memXOut = Encoding.UTF8.GetBytes(sb.ToString());
                  }
               }

               if (parent.newKB.ExecuteCommand(IFC, IIDDev, "MAN", String.Empty, memXOut, parent.toolStripProgressBar1, parent.statusStrip1, parent))
                  parent.WriteEventToLog(35, "Команда \"MAN\" ушла в сеть. Устройство - " + IIDDev.ToString(), this.Name, true);//, true, false );
               // документирование действия пользователя
               parent.WriteEventToLog(6, IIDDev.ToString(), this.Name, true);
               break;
            case "btnVLR":
               using (dlgTapconCMD dtcmd = new dlgTapconCMD())
               {
                  dtcmd.pnlRB.Visible = true;
                  dtcmd.pnlRB.Dock = DockStyle.Fill;
                  dtcmd.pnlTB.Visible = false;
                  dtcmd.Text = "Параметры команды VLR - Voltage level raise/lower ";
                  dtcmd.rbOn.Text = "ON – initiate a raise command";
                  dtcmd.rbOff.Text = "OFF – initiate a lower command.";

                  if (DialogResult.Cancel == dtcmd.ShowDialog())
                     return;

                  if (dtcmd.rbOn.Checked)
                  {
                     sb.Length = 0;
                     sb.Append("ON" + "\0");
                     byte[] memxout = new byte[4];
                     memxout = Encoding.UTF8.GetBytes(sb.ToString());
                     memXOut = new byte[4];
                     Buffer.BlockCopy(memxout, 0, memXOut, 0, memxout.Length);
                  }
                  else if (dtcmd.rbOff.Checked)
                  {
                     sb.Length = 0;
                     sb.Append("OFF" + "\0");
                     memXOut = new byte[4];
                     memXOut = Encoding.UTF8.GetBytes(sb.ToString());
                  }
               }

               if (parent.newKB.ExecuteCommand(IFC, IIDDev, "VLR", String.Empty, memXOut, parent.toolStripProgressBar1, parent.statusStrip1, parent))
                  parent.WriteEventToLog(35, "Команда \"VLR\" ушла в сеть. Устройство - " + IIDDev.ToString(), this.Name, true);//, true, false );
               // документирование действия пользователя
               parent.WriteEventToLog(6, IIDDev.ToString(), this.Name, true);
               break;
            case "btnVL1":
               if (parent.newKB.ExecuteCommand(IFC, IIDDev, "VL1", String.Empty, null, parent.toolStripProgressBar1, parent.statusStrip1, parent))
                  parent.WriteEventToLog(35, "Команда \"VL1\" ушла в сеть. Устройство - "
                     + IFC.ToString() + "." + IIDDev.ToString(), this.Name, true);// true, false);
               break;
            case "btnVL2":
               if (parent.newKB.ExecuteCommand(IFC, IIDDev, "VL2", String.Empty, null, parent.toolStripProgressBar1, parent.statusStrip1, parent))
                  parent.WriteEventToLog(35, "Команда \"VL2\" ушла в сеть. Устройство - "
                     + IFC.ToString() + "." + IIDDev.ToString(), this.Name, true);// true, false);
               break;
            case "btnVL3":
               if (parent.newKB.ExecuteCommand(IFC, IIDDev, "VL3", String.Empty, null, parent.toolStripProgressBar1, parent.statusStrip1, parent))
                  parent.WriteEventToLog(35, "Команда \"VL3\" ушла в сеть. Устройство - "
                     + IFC.ToString() + "." + IIDDev.ToString(), this.Name, true);// true, false);
               break;
            case "btnSI1":
               using (dlgTapconCMD dtcmd = new dlgTapconCMD())
               {
                  dtcmd.pnlRB.Visible = true;
                  dtcmd.pnlRB.Dock = DockStyle.Fill;
                  dtcmd.pnlTB.Visible = false;
                  dtcmd.Text = "Параметры команды SI 1 - set/reset command  ";
                  dtcmd.rbOn.Text = "ON – initiate a set command";
                  dtcmd.rbOff.Text = "OFF – initiate a reset command";

                  if (DialogResult.Cancel == dtcmd.ShowDialog())
                     return;

                  if (dtcmd.rbOn.Checked)
                  {
                     sb.Length = 0;
                     sb.Append("ON" + "\0");
                     byte[] memxout = new byte[4];
                     memxout = Encoding.UTF8.GetBytes(sb.ToString());
                     memXOut = new byte[4];
                     Buffer.BlockCopy(memxout, 0, memXOut, 0, memxout.Length);
                  }
                  else if (dtcmd.rbOff.Checked)
                  {
                     sb.Length = 0;
                     sb.Append("OFF" + "\0");
                     memXOut = new byte[4];
                     memXOut = Encoding.UTF8.GetBytes(sb.ToString());
                  }
               }

               if (parent.newKB.ExecuteCommand(IFC, IIDDev, "SI1", String.Empty, memXOut, parent.toolStripProgressBar1, parent.statusStrip1, parent))
                  parent.WriteEventToLog(35, "Команда \"SI1\" ушла в сеть. Устройство - " + IIDDev.ToString(), this.Name, true);//, true, false );
               // документирование действия пользователя
               parent.WriteEventToLog(6, IIDDev.ToString(), this.Name, true);
               break;
            case "btnSI2":
               using (dlgTapconCMD dtcmd = new dlgTapconCMD())
               {
                  dtcmd.pnlRB.Visible = true;
                  dtcmd.pnlRB.Dock = DockStyle.Fill;
                  dtcmd.pnlTB.Visible = false;
                  dtcmd.Text = "Параметры команды SI 2 - set/reset command  ";
                  dtcmd.rbOn.Text = "ON – initiate a set command";
                  dtcmd.rbOff.Text = "OFF – initiate a reset command";

                  if (DialogResult.Cancel == dtcmd.ShowDialog())
                     return;

                  if (dtcmd.rbOn.Checked)
                  {
                     sb.Length = 0;
                     sb.Append("ON" + "\0");
                     byte[] memxout = new byte[4];
                     memxout = Encoding.UTF8.GetBytes(sb.ToString());
                     memXOut = new byte[4];
                     Buffer.BlockCopy(memxout, 0, memXOut, 0, memxout.Length);
                  }
                  else if (dtcmd.rbOff.Checked)
                  {
                     sb.Length = 0;
                     sb.Append("OFF" + "\0");
                     memXOut = new byte[4];
                     memXOut = Encoding.UTF8.GetBytes(sb.ToString());
                  }
               }


               if (parent.newKB.ExecuteCommand(IFC, IIDDev, "SI2", String.Empty, memXOut, parent.toolStripProgressBar1, parent.statusStrip1, parent))
                  parent.WriteEventToLog(35, "Команда \"SI2\" ушла в сеть. Устройство - " + IIDDev.ToString(), this.Name, true);//, true, false );
               // документирование действия пользователя
               parent.WriteEventToLog(6, IIDDev.ToString(), this.Name, true);
               break;
            case "btnSI3":
               using (dlgTapconCMD dtcmd = new dlgTapconCMD())
               {
                  dtcmd.pnlRB.Visible = true;
                  dtcmd.pnlRB.Dock = DockStyle.Fill;
                  dtcmd.pnlTB.Visible = false;
                  dtcmd.Text = "Параметры команды SI 3 - set/reset command  ";
                  dtcmd.rbOn.Text = "ON – initiate a set command";
                  dtcmd.rbOff.Text = "OFF – initiate a reset command";

                  if (DialogResult.Cancel == dtcmd.ShowDialog())
                     return;

                  if (dtcmd.rbOn.Checked)
                  {
                     sb.Length = 0;
                     sb.Append("ON" + "\0");
                     byte[] memxout = new byte[4];
                     memxout = Encoding.UTF8.GetBytes(sb.ToString());
                     memXOut = new byte[4];
                     Buffer.BlockCopy(memxout, 0, memXOut, 0, memxout.Length);
                  }
                  else if (dtcmd.rbOff.Checked)
                  {
                     sb.Length = 0;
                     sb.Append("OFF" + "\0");
                     memXOut = new byte[4];
                     memXOut = Encoding.UTF8.GetBytes(sb.ToString());
                  }
               }

               if (parent.newKB.ExecuteCommand(IFC, IIDDev, "SI3", String.Empty, memXOut, parent.toolStripProgressBar1, parent.statusStrip1, parent))
                  parent.WriteEventToLog(35, "Команда \"SI3\" ушла в сеть. Устройство - " + IIDDev.ToString(), this.Name, true);//, true, false );
               // документирование действия пользователя
               parent.WriteEventToLog(6, IIDDev.ToString(), this.Name, true);
               break;
            case "btnSVL":
               ushort tbval = 0;
 
               using (dlgTapconCMD dtcmd = new dlgTapconCMD())
               {
                  dtcmd.pnlRB.Visible = false;
                  dtcmd.pnlRB.Dock = DockStyle.Fill;
                  dtcmd.pnlTB.Visible = true;
                  dtcmd.pnlTB.Dock = DockStyle.Fill;
                  dtcmd.Text = "Параметры команды SVL - Set value of Voltage Level    ";
                  dtcmd.lblDescribe.Text = "Set value of Voltage Level";
                  dtcmd.lblComment.Text = "Если требуется задать уровень напряжения в 100 Вольт, численное значения параметра должно быть равным 1000";

                  if (DialogResult.Cancel == dtcmd.ShowDialog())
                     return;
                  if (!ushort.TryParse(dtcmd.tbValue.Text, out tbval))
                  {
                     MessageBox.Show("Введено неправильное значение напряжения", this.Name, MessageBoxButtons.OK,MessageBoxIcon.Error);
                     return;
                  }
               }

               memXOut = new byte[4];
               byte[] us = new byte[2];
               us = BitConverter.GetBytes(tbval);
               Array.Reverse(us);
                Buffer.BlockCopy(us, 0, memXOut, 0, 2);

               if (parent.newKB.ExecuteCommand(IFC, IIDDev, "SVL", String.Empty, memXOut, parent.toolStripProgressBar1, parent.statusStrip1, parent))
                  parent.WriteEventToLog(35, "Команда \"SVL\" ушла в сеть. Устройство - " + IIDDev.ToString(), this.Name, true);//, true, false );
               // документирование действия пользователя
               parent.WriteEventToLog(6, IIDDev.ToString(), this.Name, true);
               break;
         }
      }

      private void frmTapcon_240_260_Load(object sender, EventArgs e)
      {
         splitContainer1.Panel2Collapsed = true;

         tabPage1.Enter += new EventHandler(tabPage1_Enter);
         slTPtoArrVars.Add(tabPage1.Text, new ArrayList());

         #region по источнику и номеру устройства опрделяем пути к файлам
         path2PrgDevCFG = AppDomain.CurrentDomain.BaseDirectory + "Project" + Path.DirectorySeparatorChar + "PrgDevCFG.cdp";

         xdoc = XDocument.Load(path2PrgDevCFG);

         IEnumerable<XElement> xes = xdoc.Descendants("FC");
         var xe = (from nn in
                      (from n in xes
                       where n.Attribute("numFC").Value == StrFC
                       select n).Descendants("Device")
                   where nn.Element("NumDev").Value == (IIDDev - (256 * IFC)).ToString()
                   select nn).First();

         path2DeviceCFG = AppDomain.CurrentDomain.BaseDirectory + xe.Element("nameR").Value + Path.DirectorySeparatorChar + "Device.cfg";
         path2FrmDev = AppDomain.CurrentDomain.BaseDirectory + xe.Element("nameR").Value + Path.DirectorySeparatorChar + "frm" + xe.Element("nameELowLevel").Value + ".xml";
         if (!File.Exists(path2DeviceCFG))
         {
            MessageBox.Show("Файл =" + path2DeviceCFG + " = не существует.", this.Name, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
         }
         if (!File.Exists(path2FrmDev))
         {
            MessageBox.Show("Файл =" + path2FrmDev + " = не существует.", this.Name, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
         }
         #endregion

         #region формируем сортированный список с панелями
         xdoc = XDocument.Load(path2DeviceCFG);
         DevPanelTypes = new SortedList();

         if (!String.IsNullOrEmpty((string)xdoc.Element("Device").Element("TypeOfPanelSections")))
         {
            IEnumerable<XElement> etypes = xdoc.Element("Device").Element("TypeOfPanelSections").Elements("TypeOfPanel");

            foreach (XElement xr in etypes)
               // определим вариант формата секции TypeOfPanel
               if ((string)xr.Element("Name") == null)
                  DevPanelTypes.Add(xr.Value, String.Empty);
               else
                  DevPanelTypes.Add(xr.Element("Name").Value, xr.Element("Caption").Value);
         }
         else
            MessageBox.Show("(307) Типы панелей в файле Device.cfg отсутсвуют", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);

         GetCCforFLP((ControlCollection)this.Controls);
         #endregion

      }

      #region Обработчики входов на вкладки
      void tabPage1_Enter(object sender, EventArgs e)
      {
         TabPage tp_this = (TabPage)sender;
         ArrayList arrVars = (ArrayList)slTPtoArrVars[tp_this.Text];
         if (arrVars.Count != 0)
            return;

         PrepareTabPagesForGroup(tp_this.Text, tp_this, ref arrVars, null);
         slTPtoArrVars[tp_this.Text] = arrVars; // ref не отрабатывает (?)
         //PrepareAdditionalFLP( pnlCurrent.Controls );
      }
      #endregion
   }
}
