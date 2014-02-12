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

            #warning DsGuid = 0
              DevicesLibrary.DeviceFormFactory.CreateForm( parent, 0, uint.Parse(objGUID), parent.arrFrm );
            }
            catch (Exception ex)
            {
                TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG(ex);
            }

      }
   }
}
