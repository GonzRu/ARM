/*#############################################################################
 *    Copyright (C) 2006-2010 Mehanotronika RA Corporation.                            
 *    All rights reserved.                                                     
 *	~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
 *                                                                             
 *	Описание: Содержит класс, для работы с деревом физической конфигурации устройств проекта.
 *	            При выборе конкр устройства загружается специфическая вкладка для этого устройства
 *                                                                             
 *	Файл                     : TreeViewConfig.cs                                          
 *	Тип конечного файла      :                                         
 *	версия ПО для разработки : С#, Framework 3.5                                
 *	Разработчик              : Юров В.И.                                        
 *	Дата начала разработки   : xx.05.2010
 *	Дата посл. корр-ровки    : xx.05.2010
 *	Дата (v1.0)              :                                                  
 ******************************************************************************
 * Изменения:
 * 1. Дата(Автор): ...cодержание...
 *#############################################################################*/

using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Data;
using System.Windows.Forms;
using System.Diagnostics;
using Calculator;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using System.IO;
using InterfaceLibrary;
using HMI_MT_Settings;

namespace HMI_MT
{
   public class TreeViewConfig : TreeView
   {
      TreeView tview;
      /// <summary>
      /// TabControl для размещения дочерних TabControl устройств выбранных в дереве устройств и групп слева 
      /// на главной форме
      /// </summary>
      TabControl tcDevices;
      /// <summary>
      /// таблица для хранения ключа доступа к тегам
      /// ключ в формате:
      /// Ист_Уст_Гр_Адр_БитМаска
      /// </summary>
      Hashtable htTags = new Hashtable();
      /// <summary>
      /// DataSet с таблицами групп тегов устройств
      /// </summary>
      DataSet dsTables = new DataSet();
      IConfiguration CONFIGURATION;
      MainForm parent;

      public TreeViewConfig(TreeView tv, TabControl tcdevices, MainForm prnt) //ArrayList newKB_KB
      {
         tview = tv;
         parent = prnt;
         CONFIGURATION = HMI_MT_Settings.HMI_Settings.CONFIGURATION;
         tcDevices = tcdevices;

         FillTreeView(CONFIGURATION);

         tview.NodeMouseClick += new TreeNodeMouseClickEventHandler(tview_NodeMouseClick);
      }

      #region работа с деревом тегов - иницализация/очистка
      /// <summary>
      /// инициализация дерева на базе конфигурации устройств
      /// </summary>
      /// <param Name="newKB_KB"></param>
      public void FillTreeView(IConfiguration configuration)
      {
			try
			{
                ArrayList arrDSNumbers = HMI_MT_Settings.HMI_Settings.CONFIGURATION.GetDataServerNumbers();

                // формируем корневой элемент с названием подстанции
                TreeNode nodeMain = new TreeNode(HMI_MT_Settings.HMI_Settings.XDoc4PathToPrjFile.Element("Project").Element("NameForTViewInSpeedAccess").Value);

                for (int i = 0; i < arrDSNumbers.Count; i++)
                {
                    XElement xe_ds = HMI_MT_Settings.HMI_Settings.CONFIGURATION.GetDataServerDescription((uint)arrDSNumbers[i]);

                    TreeNode tn = new TreeNode(xe_ds.Attribute("name").Value, 0, 0);

                    IDataServer ds = HMI_MT_Settings.HMI_Settings.CONFIGURATION.GetDataServer((uint)arrDSNumbers[i]);
                    ISourceConfiguration srcconfig = ds.GetSrcCfgByName("MOA_ECU");
                    List<IDevice> lstdev = srcconfig.GetDeviceList4TheSource();
                    List<IDevice> devsecu;

                    foreach( IDevice dev in lstdev )
                    {
                        if (dev is IECU)
                        {   
                            devsecu = (dev as IECU).GetLisEcutDevices();

                            TreeNode tnn = new TreeNode((dev as IECU).StrECUDescription);//, 1, 1);
                            //tnn.Tag = dev;
                            tn.Nodes.Add(tnn);

                            foreach( IDevice devInEcu in devsecu )
                            {
                                TreeNode tnnn = new TreeNode(devInEcu.StrDescriptionAsPhysicalDevice + "    (" + devInEcu.Description + ")");//, 1, 1);
                                tnnn.Tag = devInEcu;
                                tnn.Nodes.Add(tnnn);
                            }
                        }
                    }

                    if (nodeMain == null)
                        tview.Nodes.Add(tn);
                    else
                        nodeMain.Nodes.Add(tn);

                    tn.Collapse();
                }

                if (nodeMain != null)
                    tview.Nodes.Add(nodeMain);

                tview.ExpandAll();
            }
			catch(Exception ex)
			{
				TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG(ex );
			}
      }
      #endregion

      #region Выделение узла, создание формы для устройства
      /// <summary>
      /// обработка клика на узле дерева
      /// </summary>
      /// <param Name="sender"></param>
      /// <param Name="e"></param>
      void tview_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
      {
         FormForDeviceByType ffdbt = new FormForDeviceByType(parent);
         ffdbt.CreateAndLoadDeviceForm(e.Node);
      }
	#endregion
   }
}

