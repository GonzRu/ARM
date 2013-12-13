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
       /// �����������
       /// </summary>
       /// <param Name="kb">������ ���������</param>
       /// <param Name="dt">������� ��������� ��������� ��� ���� ������</param> 
       public frmTreeViewSelectDev(ArrayList kb, DataTable dt)
        {
            InitializeComponent();
            KB = kb;
            InitTreeView();
            dtStatus = dt;
        }
        /// <summary>
        /// ��������� ������ ���������
        /// </summary>
        private void InitTreeView()
        {
             //foreach( FC aFc in KB )
             //{
             //    TreeNode tn = new TreeNode("�� - " + aFc.NumFC.ToString(), 0, 0);
             //    tn.Checked = aFc.isCollectDataForRemoteARM;
             //    foreach (TCRZADirectDevice tdd in aFc)
             //    {
             //        TreeNode tnn = new TreeNode(tdd.ToString() + " (��. ���. " + tdd.NumDev.ToString() + ")", 1, 1);
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
        /// ��������� ������������ � ����������� � ���������� TreeView
        /// </summary>
        private void InitKB()
        {
            //����������� ������
            //foreach (TreeNode tnv in treeViewSelectDevice.Nodes)
            EnumerateTVC(treeViewSelectDevice.Nodes);
        }
        /// <summary>
        /// ����������� ������� ��� ������������ ����� � ������ ��������� ����� � ����������
        /// </summary>
        /// <param Name="tn">��������� TreeNode</param>
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
            //        throw new Exception("���������� � EnumerateTVC()");
            //    // ����������� ��������� ����
            //    if (tn.Nodes.Count != 0)
            //    {
            //        EnumerateTVC(tn.Nodes);
            //    }
            //}
        }
        /// <summary>
        /// ���������
        /// </summary>
        /// <param Name="sender"></param>
        /// <param Name="e"></param>
        private void btnSave_Click(object sender, EventArgs e)
        {
            InitKB();
            Close();
        }
        /// <summary>
        /// ��������
        /// </summary>
        /// <param Name="sender"></param>
        /// <param Name="e"></param>
        private void btnCancel_Click(object sender, EventArgs e)
        {
           // ������������� ���������� ��������� ��������� ��� ��������� �������� ������� ���
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

        // ������� ���������� ��������� ����
        bool isSingleChangeTVN = false;

        /// <summary>
        /// ���������� ��������� ��������� checkbox � ����������� TreeNode
        /// </summary>
        /// <param Name="sender"></param>
        /// <param Name="e"></param>
        private void treeViewSelectDevice_AfterCheck(object sender, TreeViewEventArgs e)
        {
            if (!isSingleChangeTVN)
                SelectAllSubnodes(e.Node);

            if (!e.Node.Checked && (e.Node.Parent != null))
            // ������� ����� � ����������� �����
            {
                isSingleChangeTVN = true;
                SetStatusParentMode(e.Node.Parent, e.Node.Checked);
            }
            isSingleChangeTVN = false;
        }
        /// <summary>
        /// ����������� ������� ��������� ��������� ������������ �����
        /// </summary>
        /// <param Name="val"></param>
        private void SetStatusParentMode(TreeNode tn, bool val)
        {
            if (tn.Parent != null)
                SetStatusParentMode(tn.Parent, val);

            tn.Checked = val;

        }
        /// <summary>
        /// ����� ��� ��������� ������� ��� ���� ��������. 
        /// </summary>
        /// <param Name="treeNode"></param>
        void SelectAllSubnodes(TreeNode treeNode)
        {
            // ������ ��� ������� ������� �� ���� ��������.
            foreach (TreeNode treeSubNode in treeNode.Nodes)
            {
                treeSubNode.Checked = treeNode.Checked;
            }
        }
    }
}