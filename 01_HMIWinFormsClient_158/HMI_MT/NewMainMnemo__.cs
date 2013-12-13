using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Xml.Linq;
using System.Diagnostics;
using WindowsForms;
using LibraryElements;
using Calculator;
using HMI_MT_Settings;
using InterfaceLibrary;

namespace HMI_MT
{
   public partial class NewMainMnemo : Form
   {
      /// <summary>
      /// класс для работы с панелями текущего режима
      /// </summary>
      CurrentModePanels cmp;
      String filePath;
      SchemaPanel panel;
      MainForm parent;
      //FormulaEval ev;
	  FormulaEvalNDS ev;
      XDocument xdoc_CfgCdp;
      IEnumerable<XElement> xefcs;
      dlgOptionsFormEditor fnm;// панели нормального режима
      /// <summary>
      /// список тегов для привязки
      /// </summary>
      List<ITag> taglist;

      public NewMainMnemo(String _path)
      {
         InitializeComponent();

         SetStyle(ControlStyles.UserPaint, true);
         SetStyle(ControlStyles.AllPaintingInWmPaint, true);
         SetStyle(ControlStyles.DoubleBuffer, true);

         filePath = _path;
      }
      public NewMainMnemo(String _path, MainForm _linkMainForm) : this(_path)
      {
         parent = _linkMainForm;
      }

      /// <summary>
      /// Создание привязок
      /// </summary>
      private void BindingLincks()
      {
         StringBuilder tagident = new StringBuilder();
         taglist = new List<ITag>();

        try
        {
            foreach (ICalculationControl icc in panel.CalculationElements)
            {
                if (icc.Calculation != null)
                    foreach (FormulaTag ft in icc.Calculation.Tags)
                    {
                        try
                        {
                            if (!GetEnableStatusDev(ft.NFC, ft.NDev))
                                break;
                            ev = new FormulaEvalNDS(HMI_MT_Settings.HMI_Settings.CONFIGURATION, "0(" + ft.NFC.ToString() + "." + ft.NDev.ToString() + ".0.60013.0)", "0", "Состояние протокола", "", TypeOfTag.Analog, "");
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(" (253) : HMI_MT.NewMainMnemo.cs : BindingLincks : ошибка : " + ex.Message, this.Name, MessageBoxButtons.OK, MessageBoxIcon.Error);
                            continue;
                        }

                        ev.OnChangeValFormTI += icc.LinkSetTextStatusDev;

                        if (ev.LinkVariableNewDS != null)
                            taglist.Add(ev.LinkVariableNewDS);

                        tagident.Length = 0;
                        tagident.Append(ft.Result);

                        try
                        {
                            ev = new FormulaEvalNDS(HMI_MT_Settings.HMI_Settings.CONFIGURATION, "0(" + tagident + ")", "0", "", "", TypeOfTag.Discret, "");
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(" (268) : HMI_MT.NewMainMnemo.cs : BindingLincks : ошибка : " + ex.Message, this.Name, MessageBoxButtons.OK, MessageBoxIcon.Error);
                            continue;
                        }

                        ev.OnChangeValFormTI += icc.LinkSetText;// привязываем функцию обработки качества тега

                        if (ev.LinkVariableNewDS != null)
                            taglist.Add(ev.LinkVariableNewDS);
                    }

                // выстроим z-порядок
                if (icc is DinamicControl)
                {
                    DinamicControl dc = icc as DinamicControl;
                    if (dc.Parameters.Name == "ОВОД-МД")
                        dc.SendToBack();
                    else
                        dc.BringToFront();
                }
            }

            // подписываемся на обновление тегов
            HMI_MT_Settings.HMI_Settings.CONFIGURATION.CfgReqEntry.SubscribeTags(taglist);
        }
        catch(Exception ex)
        {
            TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG(ex );
        }
      }
      /// <summary>
      /// Извлечь статус включенности устройства в конфигурацию 
      /// (для того чтобы избежать ошибки отсутсвия тега при попытке подключения к нему)
      /// </summary>
      /// <param Name="nfc"></param>
      /// <param Name="ndev"></param>
      /// <returns></returns>
      private bool GetEnableStatusDev(int nfc, int ndev)
      {
            XElement xe_dev;
			try
			{
                xe_dev = HMI_MT_Settings.HMI_Settings.CONFIGURATION.GetDeviceXMLDescription(0, "MOA_ECU", string.Format("{0}.{1}", nfc, ndev));

                if (xe_dev == null)
                    return false;

                if (xe_dev.Attribute("enable").Value == "True")
                    return true;
            }
			catch(Exception ex)
			{
				TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG(ex );
			}

         return false;
      }
      /// <summary>
      /// настройка видимости пунктов контекстного меню блоков
      /// </summary>
      /// <param name="cms">контекстное меню</param>
      /// <param name="compressnumdev">DevGUID устройства</param>
      private void CustomizeContextMenuItems(ContextMenuStrip cms, int compressnumdev)
      {
         try
         {
            IEnumerable<XElement> xecmstrips = (CommonUtils.CommonUtils.GetXElementFrom_PrgDevCFG(compressnumdev)).Element("DescDev").Element("TypeContextMenu").Elements("ContextMenuItem");

            foreach (XElement xecmstrip in xecmstrips)
            {
               if (PTKState.Iinstance.IsAdapterExist(compressnumdev.ToString(), xecmstrip.Attribute("name_adapter4enable").Value))
               {
                  bool rpo = bool.Parse(PTKState.Iinstance.GetValueAsString(compressnumdev.ToString(), xecmstrip.Attribute("name_adapter4enable").Value));
                  if (!rpo)
                     contextMenuStrip1.Items[xecmstrip.Attribute("name").Value].Enabled = false;
                  else
                     contextMenuStrip1.Items[xecmstrip.Attribute("name").Value].Enabled = true;
               }
               else
                  contextMenuStrip1.Items[xecmstrip.Attribute("name").Value].Enabled = true;
            }
         }
         catch (Exception ex)
         {
            TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG(ex);
         }
      }

      private void idc_MouseClick(object sender, MouseEventArgs e)
      {
         if (e.Button == MouseButtons.Right)
            return;

			try
			{
                IDynamicParameters idp = sender as IDynamicParameters;

                if (idp != null && idp.Parameters != null)
                {
                    int nFC = idp.Parameters.FK;
                    int idDev = idp.Parameters.Device;

                    // извлекаем описание из PrgDevCFG.cdp
                    XElement xeDescDev = HMI_MT_Settings.HMI_Settings.CONFIGURATION.GetDeviceXMLDescription(0, "MOA_ECU", string.Format("{0}.{1}", nFC, idDev));

                    if (xeDescDev == null)
                    {
                        MessageBox.Show("Элемент не привязан к устройству в текущей конфигурации.", this.Name, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    string objGUID = xeDescDev.Attribute("objectGUID").Value; 
                    
                    xeDescDev = xeDescDev.Element("DescDev");   // чуть подправили

                    string strNameBlock = xeDescDev.Element("nameR").Value;
                    string strRefDesign = xeDescDev.Element("DescDev").Value;

                    string FrmTagsDescript = Path.GetDirectoryName(HMI_MT_Settings.HMI_Settings.PathToConfigurationFile) + "\\Configuration\\0#DataServer\\Devices\\" + objGUID 
                                       + "#frm" + xeDescDev.Element("nameELowLevel").Value + ".xml";

                    //string FileNameDescript = Path.GetDirectoryName(HMI_MT_Settings.HMI_Settings.PathToConfigurationFile) + "\\Configuration\\0#DataServer\\Devices\\" + objGUID 
                    //   + "#frm" + xeDescDev.Element("nameELowLevel").Value + ".xml";

                    if (!File.Exists(FrmTagsDescript))
                    {
                        MessageBox.Show("Файл описания формы не существует (" + FrmTagsDescript + ")", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    #region по двум условиям проверяем наличие устройства в конфигурации

                    if (idp.Parameters.Cell == 0)
                    {
                        MessageBox.Show("Нулевой номер ячейки для устройства " + idDev.ToString() + ".\n Для активизации устройства исправьте конфигурацию.", this.Name, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                    #endregion

                    Form frm = new Form();

                    string FolderDevDescript = AppDomain.CurrentDomain.BaseDirectory + Path.DirectorySeparatorChar + strNameBlock + Path.DirectorySeparatorChar;
                                                //+ "frm" + xeDescDev.Element("nameELowLevel").Value + ".xml";

                    if (!HMI_Settings.slDevClasses.ContainsKey(strNameBlock))
                        throw new Exception(string.Format("(236) : NewMainMnemo.cs : idc_MouseClick() : Несуществующее имя типа блока = {0}", strNameBlock));

                    XDocument reader = XDocument.Load(FrmTagsDescript);

                    switch (HMI_Settings.slDevClasses[strNameBlock].ToString())
                    {
                        case "ControlSwitch":
                            frm = new frmBMRZ(parent, idp.Parameters.FK, idp.Parameters.Device, idp.Parameters.Cell, FolderDevDescript, FrmTagsDescript);
                            frm.Name = reader.Element("MT").Element("BMRZ").Element("frame").Attribute("Name").Value;
                            break;
                        case "BMRZ_100":
                            frm = new frm4Device(0,uint.Parse(objGUID));
                            frm.Name = reader.Element("MTRADeviceForm").Element("frame").Attribute("TextCaption").Value;
                            break;
                        case "ControlSwitch_Sirius":
                            //frm = new frmSirius_SP(parent, idp.Parameters.FK, idp.Parameters.Device, strP);
                            break;
                        case "ЭКРА":
                            //frm = new frmEkra(parent, idp.Parameters.FK, idp.Parameters.Device, strP);
                            break;
                        case "ОВОД":
                            //frm = new frmOvod_SP(parent, idp.Parameters.FK, idp.Parameters.Device, strP);
                            break;
                        case "Masterpact":
                            //frm = new frmMasterpact(parent, idp.Parameters.FK, idp.Parameters.Device, strP);
                            break;
                        case "ENIP":
                            //frm = new frmEnip(parent, idp.Parameters.FK, idp.Parameters.Device, strP);
                            break;
                        default:
                            return;
                    }

                    frm.Text = CommonUtils.CommonUtils.GetDispCaptionForDevice(idp.Parameters.FK * 256 + idp.Parameters.Device);

                    int devguid = idp.Parameters.FK * 256 + idp.Parameters.Device;

                    bool isconnectState = false;
                    string connectState = PTKState.Iinstance.GetValueAsString(devguid.ToString(), "Связь");

                    if (bool.TryParse(connectState, out isconnectState))
                    {
                        if (!isconnectState)
                            MessageBox.Show("Терминал недоступен или с ним нет связи", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }


                    foreach (Form f in parent.arrFrm)
                    {
                        if (f.Text == frm.Text)
                        {
                            f.Focus();
                            frm.Dispose();
                            return;
                        }
                    }

                    frm.MdiParent = this.MdiParent;
                    frm.MaximumSize = this.Size;
                    frm.Dock = DockStyle.Fill;
                    frm.WindowState = FormWindowState.Maximized;
                    frm.Show();
                    parent.arrFrm.Add(frm);
                }
            }
			catch(Exception ex)
			{
				TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG(ex );
			}
      }
      private void idc_ChangeValue(object sender, EventArgs e)
      {
         if (panel != null)
         {
            panel.Refresh();
            
            //тестирование (попытка принудительно заставить пересоваться элемент)
            if (panel.CalculationElements != null)
               foreach (CalculationControl calc in panel.CalculationElements)
                  calc.Refresh();
         }
      }
      private void NewMainMnemo_Load(object sender, EventArgs e)
      {
         panel = new SchemaPanel(filePath);
         if (!panel.ErrorLoading)
         {
            this.Text = panel.MnenoCaption;
            this.ClientSize = panel.ClientSize;
            panel.Parent = this;
            this.Controls.Add(panel);

            foreach (CalculationControl icc in panel.CalculationElements)
            {
               DinamicControl idc = icc as DinamicControl;
               if (idc != null)
               {
                  if (idc.ElementCore is DynamicElement || idc.ElementCore is Key)
                     idc.MouseClick += new MouseEventHandler(idc_MouseClick);
                  
                  if (idc.ElementCore is Trunk || idc.ElementCore is Transformator)
                     idc.ChangeValue += new EventHandler(idc_ChangeValue);
               }//if

               IDynamicParameters idp = icc as IDynamicParameters;
               if (idp != null && idp.Parameters != null)
               {
                  // извлекаем описание из PrgDevCFG.cdp
                   XElement xeDescDev = HMI_MT_Settings.HMI_Settings.CONFIGURATION.GetDeviceXMLDescription(0, "MOA_ECU", string.Format("{0}.{1}", idp.Parameters.FK, idp.Parameters.Device));   //CommonUtils.CommonUtils.GetXElementFrom_PrgDevCFG(idp.Parameters.FK, idp.Parameters.Device);
                  
                  if (xeDescDev == null)
                     continue;

                  xeDescDev = xeDescDev.Element("DescDev");
                  try
                  {
                     string strNameBlock = xeDescDev.Element("nameR").Value;
                     string strRefDesign = xeDescDev.Element("DescDev").Value;

                     // определим тип контекстного меню
                     string contextmenutype = xeDescDev.Element("TypeContextMenu").Value;

                     switch (contextmenutype)
                     {
                        case "Ekra":
                           icc.ContextMenuStrip = null;
                           break;
                        case "None":
                           icc.ContextMenuStrip = null;
                           break;
                        case "USO_HANDSET":
                           icc.ContextMenuStrip = contextMenuStrip_USO_HANDSET;
                           break;
                        case "contextMenuStrip1":
                           icc.ContextMenuStrip = contextMenuStrip1;
                           break;
                        default:
                           icc.ContextMenuStrip = contextMenuStrip1;
                           break;
                     }//switch

                     string tfn = AppDomain.CurrentDomain.BaseDirectory + Path.DirectorySeparatorChar
                        + strNameBlock + Path.DirectorySeparatorChar + "frm" + xeDescDev.Element("nameELowLevel").Value + ".xml";

                     idp.Parameters.FileNameDescript = tfn;
                     idp.Parameters.Symbol = strRefDesign;
                  }
                  catch (Exception exx)
                  {
                      TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG(exx);
                  }
               }//if
            }//foreach

            this.Refresh(); //testing
         }//if

         // установить текущий каталог
         Directory.SetCurrentDirectory(AppDomain.CurrentDomain.BaseDirectory); //??
         
         // настраиваем контекстное меню формы
         CommonUtils.CommonUtils.TestUserMenuRights(contextMenuStrip1, HMI_MT_Settings.HMI_Settings.arrlUserMenu);

         /*
          * если для данной формы определены панели нормального режима,
          * то выведем их
          */
         //cmp = new CurrentModePanels(parent.UserName, this);
      }
      private void NewMainMnemo_Shown(object sender, EventArgs e)
      {
         if (panel != null && !panel.ErrorLoading)
         {
            this.BindingLincks();
            //panel.Refresh(); //в панели вызывается Refresh() при ее загрузке
         }
      }
      private void NewMainMnemo_FormClosing(object sender, FormClosingEventArgs e)
      {
         if (e.CloseReason == CloseReason.MdiFormClosing)
            e.Cancel = true;

         if (e.Cancel == true)
            return;

         // удаляем ссылки на теги - отписываемся от тегов
         HMI_MT_Settings.HMI_Settings.CONFIGURATION.CfgReqEntry.UnSubscribeTags(taglist);

         if (panel != null)
            panel.Dispose();
      }
      private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
      {
         IDynamicParameters idp = ((ContextMenuStrip)sender).SourceControl as IDynamicParameters;
         if (idp != null && idp.Parameters != null)
         {
            int compressnumdev = idp.Parameters.FK * 256 + idp.Parameters.Device;
            CustomizeContextMenuItems(contextMenuStrip1, compressnumdev);
         }
      }
      /// <summary>
      /// реакция на выбор меню замыкания/размыкания ножа УСО, управляемых вручную
      /// </summary>
      private void toolStripMenuItem_usohandset_On_Click(object sender, EventArgs e)
      {
         //ToolStripDropDownItem tsddi = (ToolStripDropDownItem)sender;
         //ContextMenuStrip tsi = (ContextMenuStrip)tsddi.Owner;
         //IDynamicParameters idp = tsi.SourceControl as IDynamicParameters;

         //if (idp != null && idp.Parameters != null)
         //{
         //   /*
         //    * для идентификации ножа исп номер ячейки, кот передается в качестве параметра и относительно этого номера происходит связка
         //    * ножа и его описания в файле на сервере
         //    */
         //   switch ((sender as ToolStripMenuItem).Text)
         //   {
         //      case "Замкнуть":
         //         if (parent.newKB.ExecuteCommand(idp.Parameters.FK, idp.Parameters.Device, "OCB", String.Empty, BitConverter.GetBytes(idp.Parameters.Cell), parent.toolStripProgressBar1, parent.statusStrip1, parent))
         //            parent.WriteEventToLog(35, "Команда \"Замкнуть\" ушла в сеть. Устройство - "
         //            + idp.Parameters.FK.ToString() + "." + idp.Parameters.FK.ToString(), idp.Parameters.Type, true);//, true, false );
         //         break;
         //      case "Разомкнуть":
         //         if (parent.newKB.ExecuteCommand(idp.Parameters.FK, idp.Parameters.Device, "CCB", String.Empty, BitConverter.GetBytes(idp.Parameters.Cell), parent.toolStripProgressBar1, parent.statusStrip1, parent))
         //            parent.WriteEventToLog(35, "Команда \"Разомкнуть\" ушла в сеть. Устройство - "
         //            + idp.Parameters.FK.ToString() + "." + idp.Parameters.FK.ToString(), idp.Parameters.Type, true);//, true, false );
         //         break;
         //      default:
         //         break;
         //   }
         //}//if 
      }
      
      private void OnToolStripMenuItem_Click(object sender, EventArgs e)
      {
          IDynamicParameters idp = contextMenuStrip1.SourceControl as IDynamicParameters;

          if (idp != null && idp.Parameters != null)
          {
              if (CommonUtils.CommonUtils.IsUserActionBan(CommonUtils.CommonUtils.UserActionType.b00_Control_Switch, HMI_MT_Settings.HMI_Settings.UserRight))
                  return;

              if (parent.isReqPassword)
                  if (!parent.CanAction())
                  {
                      MessageBox.Show("Выполнение действия запрещено");
                      return;
                  }

              ConfirmCommand dlg = new ConfirmCommand();
              dlg.label1.Text = "Включить?";

              if (!(DialogResult.OK == dlg.ShowDialog()))
                  return;

              // выполняем действия по включению выключателя
              // вначале определим устройство
              TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG(TraceEventType.Critical, 618, "(463) NewMainMnemo.cs : включитьToolStripMenuItem_Click() : Поступила команда \"Включить\" для устройства: " + idp.Parameters.Type + "; id = " + idp.Parameters.Device);

              // правильная запись в журнал действий пользователя
              // номер устройства с цчетом фк
              int numdevfc = idp.Parameters.FK * 256 + idp.Parameters.Device;
              CommonUtils.CommonUtils.WriteEventToLog(3, numdevfc.ToString(), true);

              ICommand cmd = HMI_MT_Settings.HMI_Settings.CONFIGURATION.ExecuteCommand(0, (uint)numdevfc, "CCB", new byte[]{} );
          }
      }
      private void OffToolStripMenuItem_Click(object sender, EventArgs e)
      {
          IDynamicParameters idp = contextMenuStrip1.SourceControl as IDynamicParameters;

          if (idp != null && idp.Parameters != null)
          {
              if (CommonUtils.CommonUtils.IsUserActionBan(CommonUtils.CommonUtils.UserActionType.b00_Control_Switch, HMI_MT_Settings.HMI_Settings.UserRight))
                  return;

              if (parent.isReqPassword)
                  if (!parent.CanAction())
                  {
                      MessageBox.Show("Выполнение действия запрещено");
                      return;
                  }

              ConfirmCommand dlg = new ConfirmCommand();
              dlg.label1.Text = "Отключить?";

              if (!(DialogResult.OK == dlg.ShowDialog()))
                  return;

              // выполняем действия по отключению выключателя
              TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG(TraceEventType.Critical, 657, "(497) MainMnemo.cs : отключитьToolStripMenuItem_Click() : Поступила команда \"Отключить\" для устройства: " + idp.Parameters.Type + "; id = " + idp.Parameters.Device);

              // запись в журнал
              int numdevfc = idp.Parameters.FK * 256 + idp.Parameters.Device;
              CommonUtils.CommonUtils.WriteEventToLog(4, numdevfc.ToString(),  true);

              ICommand cmd = HMI_MT_Settings.HMI_Settings.CONFIGURATION.ExecuteCommand(0, (uint)numdevfc, "OCB", new byte[] { });
          }
      }

      private void KvotingToolStripMenuItem_Click(object sender, EventArgs e)
      {
          ToolStripDropDownItem tsddi = (ToolStripDropDownItem)sender;
          ContextMenuStrip tsi = (ContextMenuStrip)tsddi.Owner;
          IDynamicParameters idp = tsi.SourceControl as IDynamicParameters;

          // выполняем действия по квитированию выключателя
          // вначале определим устройство
          if (idp != null && idp.Parameters != null)
          {
              if (CommonUtils.CommonUtils.IsUserActionBan(CommonUtils.CommonUtils.UserActionType.b02_ACK_Signaling, HMI_MT_Settings.HMI_Settings.UserRight))
                  return;

              ConfirmCommand dlg = new ConfirmCommand();
              dlg.label1.Text = "Сбросить сигнализацию?";

              if (!(DialogResult.OK == dlg.ShowDialog()))
                  return;

              TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG(TraceEventType.Critical, 686, "(527) NewMainMnemo.cs : квитироватьToolStripMenuItem_Click() : Поступила команда \"Сбросить сигнализацию\" для устройства: " + idp.Parameters.Type + "; id = " + idp.Parameters.Device);

              // запись в журнал
              int numdevfc = idp.Parameters.FK * 256 + idp.Parameters.Device;
              CommonUtils.CommonUtils.WriteEventToLog(20, numdevfc.ToString(), true);

              ICommand cmd = HMI_MT_Settings.HMI_Settings.CONFIGURATION.ExecuteCommand(0, (uint)numdevfc, "ECC", new byte[] { });
          }
      }
      private void NormalParametersToolStripMenuItem_Click(object sender, EventArgs e)
      {
         //IDynamicParameters idp = contextMenuStrip1.SourceControl as IDynamicParameters;
         //if (idp != null && idp.Parameters != null)
         //{
         //   int devguid = idp.Parameters.FK * 256 + idp.Parameters.Device;

         //   if (cmp.CMPPanelExist(devguid.ToString()))
         //   {
         //      cmp.DoCMPVisible(devguid.ToString());
         //      return;
         //   }

         //   frmSmartPanel fsp = new frmSmartPanel(this, devguid, cmp);
         //   fsp.Name = devguid.ToString();
         //   //fsp.TopLevel = false;
         //   fsp.Text = CommonUtils.CommonUtils.GetDispCaptionForDevice(devguid);
         //   fsp.Width = 100;
         //   fsp.Height = 100;
         //   this.AddOwnedForm(fsp);
         //   fsp.Show();
         //   fsp.sc.FixedPanel = FixedPanel.Panel1;

         //   // расположение формы на экране
         //   fsp.StartPosition = FormStartPosition.Manual;
         //   fsp.Left = (idp as IBaseDinamicControl).Left + (idp as IBaseDinamicControl).Width;
         //   fsp.Top = (idp as IBaseDinamicControl).Top;
      //}
    }
   }
}
