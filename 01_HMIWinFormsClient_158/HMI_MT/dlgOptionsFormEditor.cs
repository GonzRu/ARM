/*#############################################################################
 *    Copyright (C) 2006-2011 Mehanotronika RA                            
 *    All rights reserved.                                                     
 *	~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
 *                                                                             
 *	Описание: панель для выбора\удаления тегов для мониторинга
 *                                                                             
 *	Файл                     : X:\Projects\99_MTRAhmi\Client\HMI_MT\dlgOptionsFormEditor.cs
 *	Тип конечного файла      :                                         
 *	версия ПО для разработки : С#, Framework 4.0                                
 *	Разработчик              : Юров В.И.                                        
 *	Дата начала разработки   : 05.05.2011 
 *	Дата посл. корр-ровки    : xx.хх.201х
 *	Дата (v1.0)              :                                                  
 ******************************************************************************
 * Особенности реализации:
 * Используется ...
 *#############################################################################*/
using System;
using System.Collections.Generic;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using LabelTextbox;
using System.Windows.Forms;
using System.Xml;
using System.IO;
using Calculator;
using FileManager;
using LibraryElements;
using WindowsForms;
using Structure;
using InterfaceLibrary;

namespace HMI_MT
{
	public partial class dlgOptionsFormEditor : Form
	{
		/// <summary>
		/// выбранный тег
		/// </summary>
		public ITag SelectionVar
		{
			get { return selectionVar; }
			set { selectionVar = value; }
		}
		ITag selectionVar;

		SortedList se = new SortedList();	// не используется
		StringBuilder sbse = new StringBuilder();
		int devguid = -1;

		public DataTable dtTags;

		ErrorProvider erp = new ErrorProvider( );

		public dlgOptionsFormEditor(int devguid)
		{
			InitializeComponent();

			treeViewCfg4Cmp.NodeMouseDoubleClick += new TreeNodeMouseClickEventHandler(treeViewCfg4Cmp_NodeMouseDoubleClick);

			if (devguid == -1)
			{
				FillTreeView4AllDev();
				return;
			}

			this.Text += " (" + CommonUtils.CommonUtils.GetDispCaptionForDevice(devguid) + ")";

			this.devguid = devguid;

			FillTreeView4DevByDevGuid();
		}

		void treeViewCfg4Cmp_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
		{
			devguid = (int)e.Node.Tag;
			treeViewCfg4Cmp.CheckBoxes = true;
			PopulateTreeView(e.Node);
		}
		/// <summary>
		/// формирование дерева для всей конфигурации
		/// </summary>
		private void FillTreeView4AllDev()
		{
            //string CaptionOld = this.Text;
            //this.Text = "Выберите устройство";
            //treeViewCfg4Cmp.CheckBoxes = false;

            //TreeViewLogicalConfig tvwlc = new TreeViewLogicalConfig(treeViewCfg4Cmp);
		}

		#region заполнить дерево тегами устройства
		private void FillTreeView4DevByDevGuid()
		{
			treeViewCfg4Cmp.Nodes.Add(CommonUtils.CommonUtils.GetDispCaptionForDevice(devguid));
			PopulateTreeView(treeViewCfg4Cmp.Nodes[0]);
			treeViewCfg4Cmp.ExpandAll();
		}

		/// <summary>
		/// public void PopulateTreeView( TreeNode parentNode )
		/// заполнение узлов TreeView
		/// </summary>
		/// <param Name="parentNode"></param>
		public void PopulateTreeView(TreeNode parentNode )
		{
            try
            {
                throw new Exception(string.Format("Заглушка : Вызов функции {0}.{1}", @"X:\Projects\40_Tumen_GPP09\Client\HMI_MT\dlgOptionsFormEditor.cs", "PopulateTreeView())"));
			}
			catch(Exception ex)
			{
			   TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG(ex );
			}

			int fc = devguid / 256;
			int dev = devguid % 256;

            //foreach (DataSource aFC in Configurator.KB)
            //{
            //    if (aFC.NumFC != fc)
            //        continue;

            //    foreach (TCRZADirectDevice aDev in aFC)
            //    {
            //        if (aDev.NumDev != dev)
            //            continue;

            //        TreeNode aDevNode = new TreeNode("[ " + aDev.NumDev.ToString() + " ] " + aDev.ToString());
            //        parentNode.Nodes.Add(aDevNode);
            //        foreach (TCRZAGroup aGroup in aDev)
            //        {
            //            TreeNode aGroupNode = new TreeNode(aGroup.Name);
            //            aGroupNode.Tag = aGroup;
            //            aDevNode.Nodes.Add(aGroupNode);
            //            foreach (TCRZAVariable aVariable in aGroup)
            //            {
            //                TreeNode aVariableNode = new TreeNode(aVariable.Name);
            //                aVariableNode.Tag = aVariable;
            //                aGroupNode.Nodes.Add(aVariableNode);
            //                // распознать по типам и присвоить иконку для битовых и аналоговых сигналов
            //            }
            //        }
            //    }
            //}
		}		
		#endregion

		private void btnCancel_Click( object sender, EventArgs e )
		{
			selectionVar = null;
			this.Close();
		}

		private void btnOK_Click( object sender, EventArgs e )
		{
			// перебираем узлы ищем отмеченный
			count = 0;
			FindCheckedNodes(treeViewCfg4Cmp.Nodes); //заполняет count - число отмеченных узлов

			if ( count > 1)
			{
				MessageBox.Show("Можно выбрать только один тег", "Выбор тега", MessageBoxButtons.OK, MessageBoxIcon.Error);
				ClearTreeViewChecked(treeViewCfg4Cmp.Nodes);
				return;
			}
			this.Hide();	//скрываем форму настройки параметров норм реж
		}

		private void SaveCMPTags()
		{
			throw new NotImplementedException();
		}

		List<ITag> TCRZAVariableList = new List<ITag>();

		int count = 0;	// число отмеченных узлов
		/// <summary>
		/// поиск отмеченных узлов с теками и формирование списка для сохранения
		/// </summary>
		/// <param name="treeNodeCollection"></param>
		private void FindCheckedNodes(TreeNodeCollection treeNodeCollection)
		{
			foreach ( TreeNode tn in treeNodeCollection )
			{
				if (tn.Nodes.Count > 0)
					FindCheckedNodes(tn.Nodes);
				else if (tn.Checked)
				{
					selectionVar = tn.Tag as ITag;
					count++;
				}
			}
		}

		/// <summary>
		/// сброс выделения всех узлов
		/// </summary>
		private void ClearTreeViewChecked(TreeNodeCollection treeNodeCollection)
		{
			foreach (TreeNode tn in treeNodeCollection)
			{
				if (tn.Nodes.Count > 0)
					ClearTreeViewChecked(tn.Nodes);
				else if (tn.Checked)
					tn.Checked = false;
			}
		}
	}
}