using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using NormalModeLibrary.ViewModel;

namespace NormalModeLibrary.Windows
{
    public partial class SelectSignalsWindow : Form
    {
        ViewWindow view;
        InterfaceLibrary.IDevice device;
        PanelViewModel originalPanel, copyPanel;

        /* Автоматический режим работает исправно */
        /* Закоментированно по просьбе Дениса и Алексея */

        internal SelectSignalsWindow()
        {
            InitializeComponent();
            view = new ViewWindow();
            view.IsEditable = true;
            view.Owner = this;
        }
        internal void AddComponents( InterfaceLibrary.IDevice device, PanelViewModel panel )
        {
            this.device = device;
            originalPanel = panel;
            copyPanel = (PanelViewModel)panel.Copy();

            foreach ( BaseSignalViewModel vmSignal in copyPanel.Collection )
                vmSignal.UnSubscribe();

            view.Component = copyPanel;
            textBox1.Text = view.Component.Caption;
            checkBox1.Checked = view.Component.IsAutomaticaly;
            checkBox2.Checked = view.Component.IsVisible;
            checkBox3.Checked = view.Component.IsCaptionVisible;

            foreach ( InterfaceLibrary.IGroup group in device.GetGroupHierarchy() )
                treeView1.Nodes.Add( GetTreeNode( group ) );
            treeView1.ExpandAll();
        }
        public PanelViewModel GetWindowComponent()
        {
            return originalPanel;
        }
        private TreeNode GetTreeNode( InterfaceLibrary.IGroup group )
        {
            // построение групп
            TreeNode node = new TreeNode( group.NameGroup );
            if ( group.SubGroupsList != null && group.SubGroupsList.Count != 0 )
                foreach ( InterfaceLibrary.IGroup gr in group.SubGroupsList )
                    node.Nodes.Add( GetTreeNode( gr ) );
            
            // построение тэгов
            if ( group.SubGroupTagsList != null && group.SubGroupTagsList.Count != 0 )
                foreach ( String strTag in group.SubGroupTagsList )
                {
                    InterfaceLibrary.ITag tag = device.GetTag( Convert.ToUInt32( strTag ) );
                    if ( tag != null )
                    {
                        if (tag.TagName == "")
                            continue;

                        var subNode = CreateTagNode( tag, view.Component );
                        var signal = subNode.Tag as BaseSignalViewModel;
                        if (signal == null || !signal.IsSupported ) continue;
                        node.Nodes.Add( subNode );
                    }
                }
            return node;
        }

        private void treeView1_AfterCheck( object sender, TreeViewEventArgs e )
        {
            ViewModel.BaseSignalViewModel signalModel = (ViewModel.BaseSignalViewModel)e.Node.Tag;

            if ( signalModel != null )
            {
                signalModel.IsChecked = e.Node.Checked;

                if ( e.Node.Checked )
                {
                    ( (Sources.BaseObjectCollection)view.Component.Core ).Collection.Add( signalModel.Core );
                    view.Component.Collection.Add( signalModel );
                }
                else
                {
                    ( (Sources.BaseObjectCollection)view.Component.Core ).Collection.Remove( signalModel.Core );
                    view.Component.Collection.Remove( signalModel );
                }
            }
        }
        private void textBox1_TextChanged( object sender, EventArgs e )
        {
            view.Text = ( (TextBox)sender ).Text;
        }
        private void button1_Click( object sender, EventArgs e )
        {
            originalPanel.Top = view.Top;
            originalPanel.Left = view.Left;
            originalPanel.Width = view.Width;
            originalPanel.Height = view.Height;
            originalPanel.Caption = view.Text;
            originalPanel.IsAutomaticaly = checkBox1.Checked;
            originalPanel.IsVisible = checkBox2.Checked;
            originalPanel.IsCaptionVisible = checkBox3.Checked;

            foreach ( BaseSignalViewModel vmElem in originalPanel.Collection.ToArray() )
            {
                vmElem.UnSubscribe();
                ( (Sources.BaseObjectCollection)originalPanel.Core ).Collection.Remove( vmElem.Core );
                originalPanel.Collection.Remove( vmElem );
            }
            foreach ( BaseSignalViewModel vmElem in copyPanel.Collection )
            {
                vmElem.Subscribe();
                ( (Sources.BaseObjectCollection)originalPanel.Core ).Collection.Add( vmElem.Core );
                originalPanel.Collection.Add( vmElem );
            }

            ComponentFactory.Factory.SaveXml();
        }        
        protected override void OnLoad( EventArgs e )
        {
            base.OnLoad( e );

            view.Show();
        }

        private static TreeNode CreateTagNode( InterfaceLibrary.ITag tag, PanelViewModel panel )
        {
            var node = new TreeNode( tag.TagName );

            // если есть сигнал в списке панели, возвращаем его
            foreach ( BaseSignalViewModel bsModel in panel.Collection )
                if ( bsModel.Guid == tag.TagGUID )
                {
                    node.Checked = bsModel.IsChecked;
                    node.Tag = bsModel;
                    return node;
                }

            //если сигнала нет в списке панели, создаем сигнал и его представление
            Sources.BaseSignal signal = Sources.BaseSignal.CreateSignal( Sources.BaseSignal.GetSignalType( tag.Type ) );
            if ( signal != null )
            {
                signal.type = panel.Type;
                signal.dsGuid = panel.DsGuid;
                signal.objectGuid = panel.ObjectGuid;

                signal.Caption = tag.TagName;
                signal.Dim = tag.Unit;
                signal.Guid = tag.TagGUID;
                BaseSignalViewModel bsvm = BaseSignalViewModel.GetSignalViewModel( signal );
                bsvm.SetTag( tag );
                node.Tag = bsvm;
            }

            if ( !Sources.BaseSignal.CheckSignalType( tag.Type ) )
                node.ForeColor = Color.Gray;

            return node;
        }
    }
}
