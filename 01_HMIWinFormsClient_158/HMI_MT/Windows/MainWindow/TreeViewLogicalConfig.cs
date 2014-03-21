/*#############################################################################
 *    Copyright (C) 2006-2010 Mehanotronika RA Corporation.                            
 *    All rights reserved.                                                     
 *	~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
 *                                                                             
 *	Описание: Содержит класс, для работы с деревом логической конфигурации проекта                                                   
 *                                                                             
 *	Файл                     : TreeViewLogicalConfig.cs                                          
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
//using CRZADevices;
using Calculator;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using InterfaceLibrary;

namespace HMI_MT
{
   public delegate void ChangeTabpage(string tpname);

   class TreeViewLogicalConfig : TreeView
   {
	   /// <summary>
	   /// выделенное устройство
	   /// </summary>
	   public int SelectedDevGuid
	   {
		   get { return selectedDevGuid; }
		   set { selectedDevGuid = value; }
	   }
	   int selectedDevGuid; 

	   public event ChangeTabpage OnChangeTabpage;

      /// <summary>
      /// список устройств, упорядоченный по уник номерам
      /// </summary>
      //SortedList<int, XElement> slDevicesByGUID;
      TreeView tview;
      MainForm parent;

	   /// <summary>
	   /// конструктор общий
	   /// </summary>
	   /// <param name="tv"></param>
	  public TreeViewLogicalConfig(TreeView tv)
	  {
		  Init(tv);
	  }

	   /// <summary>
	   /// конструктор для гл формы
	   /// </summary>
	   /// <param name="tv"></param>
	   /// <param name="prnt"></param>
	   public TreeViewLogicalConfig(TreeView tv, MainForm prnt)
      {
         parent = prnt;
		 Init(tv);
		 tview.NodeMouseClick += new TreeNodeMouseClickEventHandler(tview_NodeMouseClick);
      }

	   private void Init(TreeView tv)
	   {
			try
			{
                tview = tv;

                FillTreeView();  // заполняем дерево
            }
			catch(Exception ex)
			{
				TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG(ex );
			}
	   }
      
      #region работа с деревом тегов - иницализация/очистка
      /// <summary>
      /// инициализация дерева на базе конфигурации устройств
      /// </summary>
      /// <param Name="newKB_KB"></param>
      public void FillTreeView()
      {
          try
          {
              ArrayList arrDSNumbers = HMI_MT_Settings.HMI_Settings.CONFIGURATION.GetDataServerNumbers();

              // формируем корневой элемент с названием подстанции
              TreeNode nodeMain = new TreeNode(HMI_MT_Settings.HMI_Settings.XDoc4PathToPrjFile.Element("Project").Element("NameForTViewInSpeedAccess").Value);

              for (int i = 0; i < arrDSNumbers.Count; i++)
              {
                  XElement xe_ds = HMI_MT_Settings.HMI_Settings.CONFIGURATION.GetDataServerDescription((uint)arrDSNumbers[i]);

                  // создали узел с именем DS
                  TreeNode tn = new TreeNode(xe_ds.Attribute("name").Value, 0, 0);

                  if (nodeMain == null)
                      tview.Nodes.Add(tn);
                  else
                      nodeMain.Nodes.Add(tn);

                  #region работа с конфигурацией быстрого доступа из файла \Project\Configuration.cfg
                  // формирование групп/подгрупп, если они есть
                  IEnumerable<XElement> xeSections = HMI_MT_Settings.HMI_Settings.XDoc4PathToConfigurationFile.Element("Project").Element("SectionConfiguration").Elements("Section");
                  TreeNode tnnew;
                  foreach (XElement xeSection in xeSections)
                  {
                      //if (nodeMain != null)
                      //    tnnew = nodeMain.Nodes.Add(xeSection.Attribute("name").Value);
                      //else
                          //tnnew = tview.Nodes.Add(xeSection.Attribute("name").Value);

                      tnnew = tn.Nodes.Add(xeSection.Attribute("name").Value);

                      CreateTVNodeForSubgroup(xeSection, tnnew);
                      // добавление устройств вне подгрупп
                      CreateTVNodeForDevice(xeSection, tnnew);
                      tnnew.Collapse();
                  }

                  if (nodeMain != null)
                      nodeMain.ExpandAll();
                  else
                      tview.ExpandAll();                  
                  #endregion

                  tn.Collapse();
              }

              if (nodeMain != null)
                  tview.Nodes.Add(nodeMain);

              tview.ExpandAll();
          }
          catch (Exception ex)
          {
              TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG(ex);
          }
      }
      
      /// <summary>
      /// рекурсивная функция добавления подгрупп в дерево дог конфигурации
      /// </summary>
      /// <param Name="xeparent"></param>
      void CreateTVNodeForSubgroup(XElement xeparent, TreeNode tn)
      {
			try
			{
                IEnumerable<XElement> xeSections = xeparent.Elements("SubSection");

                foreach (XElement xeSection in xeSections)
                {
                    TreeNode tnnew = tn.Nodes.Add(xeSection.Attribute("name").Value);

                    // добавить устройства в узел
                    CreateTVNodeForDevice(xeSection, tnnew);
                }
                tn.Collapse();
            }
			catch(Exception ex)
			{
				TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG(ex );
			}
      }

      /// <summary>
      /// Добавить устройства в текущий узел
      /// </summary>
      /// <param Name="xeparent"></param>
      /// <param Name="tn"></param>
      void CreateTVNodeForDevice(XElement xeparent, TreeNode tn)
      {
			try
			{
                IEnumerable<XElement> xeDevInSections = xeparent.Elements("Bay");
                TreeNode tndev = null;
                foreach (XElement xeDevInSection in xeDevInSections)
                {
                    tndev = new TreeNode(CommonUtils.CommonUtils.GetDispCaptionForDevice(Convert.ToInt32(xeDevInSection.Attribute("key").Value)));

                    tndev.Tag = Convert.ToInt32(xeDevInSection.Attribute("key").Value);
                    tn.Nodes.Add(tndev);
                }
                tn.Collapse();
            }
			catch(Exception ex)
			{
				TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG(ex );
			}
      }
      #endregion

      #region Выделение узла
      /// <summary>
      /// обработка клика на узле дерева
      /// </summary>
      /// <param Name="sender"></param>
      /// <param Name="e"></param>
      void tview_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
      {
			try
			{
                if ((e.Node as TreeNode).Tag == null)
                {
                    /*
                     *  посмотрим есть ли вкладка быстрого доступа с таким именем, 
                     *  если есть, то открываем ее, 
                     *  активизация нужной вкладки через событие
                     */
                    if (OnChangeTabpage != null)
                        SendEventChangeTabPage(e.Node.Text);

                    return;
                }

                selectedDevGuid = Convert.ToInt32((e.Node as TreeNode).Tag);

                if (parent != null)
                {
                    FormForDeviceByType ffdbt = new FormForDeviceByType(parent);
                    ffdbt.CreateAndLoadDeviceForm(Convert.ToInt32((e.Node as TreeNode).Tag));
                }
            }
			catch(Exception ex)
			{
				TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG(ex );
			}
	  }

      /// <summary>
      /// сменить tabpage по событию
      /// </summary>
      /// <param Name="e_Node_Text"></param>
      private void SendEventChangeTabPage(string e_Node_Text)
      {
			try
			{
                OnChangeTabpage(e_Node_Text);
            }
			catch(Exception ex)
			{
				TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG(ex );
			}
      }
      #endregion
   }
}
