using System;
using System.Collections.Generic;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
//using CRZADevices;

namespace HMI_MT
{
    public partial class frmTreeViewSelectDev : Form
    {
        ArrayList KB;
       DataTable dtStatus;
       /// <summary>
       /// конструктор
       /// </summary>
       /// <param Name="kb">массив устройств</param>
       /// <param Name="dt">таблица состояний устройств для удал обмена</param> 
       public frmTreeViewSelectDev(ArrayList kb, DataTable dt)
        {
            InitializeComponent();
            KB = kb;
            InitTreeView();
            dtStatus = dt;
        }
        /// <summary>
        /// заполняем дерево устройств
        /// </summary>
        private void InitTreeView()
        {
             //foreach( FC aFc in KB )
             //{
             //    TreeNode tn = new TreeNode("ФК - " + aFc.NumFC.ToString(), 0, 0);
             //    tn.Checked = aFc.isCollectDataForRemoteARM;
             //    foreach (TCRZADirectDevice tdd in aFc)
             //    {
             //        TreeNode tnn = new TreeNode(tdd.ToString() + " (ид. уст. " + tdd.NumDev.ToString() + ")", 1, 1);
             //        tnn.Checked = tdd.isCollectDataForRemoteARM;
             //        tnn.Tag = tdd;
             //        tn.Nodes.Add(tnn);
             //    }
             //    tn.Tag = aFc;
             //    treeViewSelectDevice.Nodes.Add(tn);
             //    treeViewSelectDevice.ExpandAll();
             //}
        }
        /// <summary>
        /// заполняем конфигурацию в соответсвии с состоянием TreeView
        /// </summary>
        private void InitKB()
        {
            //перечисляем дерево
            //foreach (TreeNode tnv in treeViewSelectDevice.Nodes)
            EnumerateTVC(treeViewSelectDevice.Nodes);
        }
        /// <summary>
        /// рекурсивная функция для перечисления узлов и записи состояния сбора в устройства
        /// </summary>
        /// <param Name="tn">очередной TreeNode</param>
        private void EnumerateTVC(TreeNodeCollection tnCol) 
        {
            //foreach (TreeNode tn in tnCol)
            //{
            //    if (tn.Tag is FC)
            //    {
            //        FC fc = (FC)tn.Tag;
            //        fc.isCollectDataForRemoteARM = tn.Checked;
            //    }
            //    //else if (tn.Tag is FCasDev)
            //    //{
            //    //    FCasDev tcfc = (FCasDev)tn.Tag;
            //    //    tcfc.isCollectDataForRemoteARM = tn.Checked;
            //    //}
            //    else if (tn.Tag is TCRZADirectDevice)
            //    {
            //        TCRZADirectDevice tcdd = (TCRZADirectDevice)tn.Tag;
            //        tcdd.isCollectDataForRemoteARM = tn.Checked;
            //    }
            //    else
            //        throw new Exception("Исключение в EnumerateTVC()");
            //    // перечисляем вложенные узлы
            //    if (tn.Nodes.Count != 0)
            //    {
            //        EnumerateTVC(tn.Nodes);
            //    }
            //}
        }
        /// <summary>
        /// Сохранить
        /// </summary>
        /// <param Name="sender"></param>
        /// <param Name="e"></param>
        private void btnSave_Click(object sender, EventArgs e)
        {
            InitKB();
            Close();
        }
        /// <summary>
        /// Отменить
        /// </summary>
        /// <param Name="sender"></param>
        /// <param Name="e"></param>
        private void btnCancel_Click(object sender, EventArgs e)
        {
           // устанавливаем предыдущее состояние устройств для удаленной передачи другому АРМ
           for( int i = 0 ;i < dtStatus.Rows.Count ;i++ )
           {
              //foreach( FC aFc in KB )
              //{
              //   foreach( TCRZADirectDevice tdd in aFc )
              //   {
              //      if( tdd.NumDev == Convert.ToInt32( dtStatus.Rows[ i ][ "Devices" ] ) )
              //      {
              //         tdd.isCollectDataForRemoteARM = Convert.ToBoolean( dtStatus.Rows[ i ][ "PrevStatus" ] );
              //         break;
              //      }
              //   }
              //}
           }
            Close();
        }

        // признак одиночного изменения узла
        bool isSingleChangeTVN = false;

        /// <summary>
        /// Обработчик изменения состояния checkbox у конкретного TreeNode
        /// </summary>
        /// <param Name="sender"></param>
        /// <param Name="e"></param>
        private void treeViewSelectDevice_AfterCheck(object sender, TreeViewEventArgs e)
        {
            if (!isSingleChangeTVN)
                SelectAllSubnodes(e.Node);

            if (!e.Node.Checked && (e.Node.Parent != null))
            // снимаем галки у вышестоящих узлов
            {
                isSingleChangeTVN = true;
                SetStatusParentMode(e.Node.Parent, e.Node.Checked);
            }
            isSingleChangeTVN = false;
        }
        /// <summary>
        /// рекурсивная функция установки состояния родительских узлов
        /// </summary>
        /// <param Name="val"></param>
        private void SetStatusParentMode(TreeNode tn, bool val)
        {
            if (tn.Parent != null)
                SetStatusParentMode(tn.Parent, val);

            tn.Checked = val;

        }
        /// <summary>
        /// Метод для установки галочки для всех подузлов. 
        /// </summary>
        /// <param Name="treeNode"></param>
        void SelectAllSubnodes(TreeNode treeNode)
        {
            // Ставим или убираем отметку со всех подузлов.
            foreach (TreeNode treeSubNode in treeNode.Nodes)
            {
                treeSubNode.Checked = treeNode.Checked;
            }
        }
    }
}