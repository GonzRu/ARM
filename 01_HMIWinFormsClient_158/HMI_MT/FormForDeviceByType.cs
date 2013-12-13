/*#############################################################################
 *    Copyright (C) 2006-2010 Mehanotronika RA Corporation.                            
 *    All rights reserved.                                                     
 *	~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
 *                                                                             
 *	Описание: Содержит класс поддерживающий процесс создания формы для устройства
 *	            по его типу
 *                                                                             
 *	Файл                     : FormForDeviceByType.cs                                          
 *	Тип конечного файла      :                                         
 *	версия ПО для разработки : С#, Framework 3.5                                
 *	Разработчик              : Юров В.И.                                        
 *	Дата начала разработки   : xx.07.2010
 *	Дата посл. корр-ровки    : xx.07.2010
 *	Дата (v1.0)              :                                                  
 ******************************************************************************
 *#############################################################################*/

using System;
using System.Collections;
using System.Windows.Forms;
//using CRZADevices;
using System.Xml.Linq;
using InterfaceLibrary;

namespace HMI_MT
{
   public class FormForDeviceByType
   {
      ArrayList KB;
      MainForm parent;

      public FormForDeviceByType( MainForm prnt )
      {
         parent = prnt;
      }

      public void CreateAndLoadDeviceForm( TreeNode tn )
      {
        if (tn.Tag == null)
            return;

			try
			{
                IDevice tcdd = tn.Tag as IDevice;

                if( tcdd == null )
                    throw new Exception(string.Format("(54) : RTU_MOA.cs : ParsePacketRawData() : Несуществующее устройство = {0}", tn.Text));

                // извлекаем описание из PrgDevCFG.cdp

                XElement xeDescDev = CommonUtils.CommonUtils.GetXElementFrom_PrgDevCFG((int)tcdd.UniObjectGUID);//( tcdd.NumFC, tcdd.NumDev );

                CreateForm(xeDescDev);
            }
			catch(Exception ex)
			{
				TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG(ex );
			}
     }

      public void CreateAndLoadDeviceForm( int devguid )
      {
         // извлекаем описание из PrgDevCFG.cdp
        XElement xeDescDev = CommonUtils.CommonUtils.GetXElementFrom_PrgDevCFG( devguid );

        CreateForm( xeDescDev );
      }

      /// <summary>
      /// создать форму по описанию устройства в секции файла PrgDevCFG.cdp
      /// </summary>
      /// <param Name="xeDescDev"></param>
      private void CreateForm( XElement xeDescDev )
      {
          try
          {
              string objGUID = xeDescDev.Attribute("objectGUID").Value;

              //DeviceFormFactory.CreateForm( parent, int.Parse(objGUID), PTKStateLib.PTKState.Iinstance, parent.arrFrm );
              DevicesLibrary.DeviceFormFactory.CreateForm( parent, int.Parse(objGUID), parent.arrFrm );

              #region старый код
              //xeDescDev = xeDescDev.Element("DescDev");   // чуть подправили

              //string strNameBlock = xeDescDev.Element("nameR").Value;
              //string strRefDesign = xeDescDev.Element("DescDev").Value;

              //string FrmTagsDescript = Path.GetDirectoryName(HMI_MT_Settings.HMI_Settings.PathToConfigurationFile) + "\\Configuration\\0#DataServer\\Devices\\" + objGUID
              //                   + "#frm" + xeDescDev.Element("nameELowLevel").Value + ".xml";

              ////string FileNameDescript = Path.GetDirectoryName(HMI_MT_Settings.HMI_Settings.PathToConfigurationFile) + "\\Configuration\\0#DataServer\\Devices\\" + objGUID 
              ////   + "#frm" + xeDescDev.Element("nameELowLevel").Value + ".xml";

              //if (!File.Exists(FrmTagsDescript))
              //{
              //    MessageBox.Show("Файл описания формы не существует (" + FrmTagsDescript + ")", "FormForDeviceByType.cs", MessageBoxButtons.OK, MessageBoxIcon.Error);
              //    return;
              //}

              //#region по двум условиям проверяем наличие устройства в конфигурации

              ////if (idp.Parameters.Cell == 0)
              ////{
              ////    MessageBox.Show("Нулевой номер ячейки для устройства " + idDev.ToString() + ".\n Для активизации устройства исправьте конфигурацию.", this.Name, MessageBoxButtons.OK, MessageBoxIcon.Information);
              ////    return;
              ////}
              //#endregion

              //Form frm = new Form();

              //string FolderDevDescript = AppDomain.CurrentDomain.BaseDirectory + Path.DirectorySeparatorChar + strNameBlock + Path.DirectorySeparatorChar;
              ////+ "frm" + xeDescDev.Element("nameELowLevel").Value + ".xml";

              //XDocument reader = XDocument.Load(FrmTagsDescript); 

              //switch (HMI_Settings.slDevClasses[strNameBlock].ToString())
              //{
              //    case "ControlSwitch":
              //        frm = new frmBMRZ(parent, FK, Device,  FolderDevDescript, FrmTagsDescript);
              //        frm.Name = reader.Element("MT").Element("BMRZ").Element("frame").Attribute("Name").Value;
              //        break;
              //    case "BMRZ_100":
              //        frm = new frm4Device(0, uint.Parse(objGUID));
              //        frm.Name = reader.Element("MTRADeviceForm").Element("frame").Attribute("TextCaption").Value;
              //        break;
              //    case "BMCS":
              //        frm = new frmBMCS(parent, FK, Device, 1, FolderDevDescript, FrmTagsDescript);
              //        frm.Name = reader.Element("MT").Element("BMRZ").Element("frame").Attribute("Name").Value;
              //        break;
              //    case "DUGA":
              //        frm = new frmDuga(parent, FK, Device, 1, FolderDevDescript, FrmTagsDescript);
              //        frm.Name = reader.Element("MT").Element("BMRZ").Element("frame").Attribute("Name").Value;
              //        break;
              //    case "СИРИУС-ОЗЗ":
              //        frm = new frmSirius_OZZ(parent, FK, Device, FolderDevDescript, FrmTagsDescript);
              //        frm.Name = reader.Element("MT").Element("BMRZ").Element("frame").Attribute("Name").Value;
              //        break;
              //    case "ЭКРА":
              //        frm = new frm4DeviceEkra(0, uint.Parse(objGUID));
              //        frm.Name = reader.Element("MTRADeviceForm").Element("frame").Attribute("TextCaption").Value;
              //        break;
              //    case "ControlSwitch_Sirius":
              //        //frm = new frmSirius_SP(parent, idp.Parameters.FK, idp.Parameters.Device, strP);
              //        break;
              //    case "ОВОД":
              //        //frm = new frmOvod_SP(parent, idp.Parameters.FK, idp.Parameters.Device, strP);
              //        break;
              //    case "Masterpact":
              //        //frm = new frmMasterpact(parent, idp.Parameters.FK, idp.Parameters.Device, strP);
              //        break;
              //    case "ENIP":
              //        //frm = new frmEnip(parent, idp.Parameters.FK, idp.Parameters.Device, strP);
              //        break;
              //    default:
              //        return;
              //}

              //frm.Text = CommonUtils.CommonUtils.GetDispCaptionForDevice(FK * 256 + Device);

              //int devguid = FK * 256 + Device;

              //bool isconnectState = false;
              //string connectState = PTKStateLib.PTKState.Iinstance.GetValueAsString(devguid.ToString(), "Связь");

              //if (bool.TryParse(connectState, out isconnectState))
              //{
              //    if (!isconnectState)
              //        MessageBox.Show("Терминал недоступен или с ним нет связи", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
              //}

              //foreach (Form f in parent.arrFrm)
              //{
              //    if (f.Text == frm.Text)
              //    {
              //        f.Focus();
              //        frm.Dispose();
              //        return;
              //    }
              //}

              //frm.MdiParent = parent;
              //frm.MaximumSize = parent.Size;
              //frm.Dock = DockStyle.Fill;
              //frm.WindowState = FormWindowState.Maximized;
              //frm.Show();
              //parent.arrFrm.Add(frm); 
              #endregion
            }
            catch (Exception ex)
            {
                TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG(ex);
            }

      }
   }
}
