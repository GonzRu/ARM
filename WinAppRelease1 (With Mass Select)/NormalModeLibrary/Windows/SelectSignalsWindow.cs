using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using NormalModeLibrary.ViewModel;
using InterfaceLibrary;

namespace NormalModeLibrary.Windows
{
    public partial class SelectSignalsWindow : Form
    {
        IDevice device;
        private INormalModePanel _view;
        private string _originalCaption;
        PanelViewModel originalPanel;

        /* Автоматический режим работает исправно */
        /* Закоментированно по просьбе Дениса и Алексея */

        internal SelectSignalsWindow()
        {
            InitializeComponent();
        }
        internal void AddComponents( IDevice device, INormalModePanel view )
        {
            this.device = device;
            this._view = view;

            originalPanel = view.Component;
            view.Component = (PanelViewModel)view.Component.Copy();

            _originalCaption = view.Component.Caption;
            captionTextBox.Text = view.Component.Caption;
            FontSizeNumericUpDown.Value = view.Component.FontSize;

            if (!view.Component.IsVisible)
                workModeComboBox.SelectedIndex = 2;
            else if (view.Component.IsAutomaticaly)
                workModeComboBox.SelectedIndex = 0;
            else
                workModeComboBox.SelectedIndex = 1;


            checkBox3.Checked = view.Component.IsCaptionVisible;
            captionTextBox.Enabled = view.Component.IsCaptionVisible;

            foreach (IGroup group in device.GetGroupHierarchy())
                if (group.IsEnable)
                    treeView1.Nodes.Add(GetTreeNode(group));
            treeView1.ExpandAll();
        }

        public PanelViewModel GetOriginalWindowComponent()
        {
            return originalPanel;
        }

        #region PrivateMetods
        #region Building TreeView with groups and tags
        private TreeNode GetTreeNode( IGroup group )
        {
            // построение групп
            TreeNode node = new TreeNode( group.NameGroup );
            if (group.SubGroupsList != null && group.SubGroupsList.Count != 0)
                foreach (IGroup gr in group.SubGroupsList)
                    if (gr.IsEnable)
                    {
                        var newNode = GetTreeNode(gr);
                        if (newNode.Nodes.Count != 0)
                            node.Nodes.Add(GetTreeNode(gr));
                    }

            // построение тэгов
            if ( group.SubGroupTagsList != null && group.SubGroupTagsList.Count != 0 )
                foreach ( String strTag in group.SubGroupTagsList )
                {
                    ITag tag = device.GetTag( Convert.ToUInt32( strTag ) );
                    if ( tag != null )
                    {
                        if (tag.TagName == "")
                            continue;

                        var subNode = CreateTagNode( tag, _view.Component );
                        var signal = subNode.Tag as BaseSignalViewModel;
                        if (signal == null || !signal.IsSupported ) continue;
                        node.Nodes.Add( subNode );
                    }
                }

            return node;
        }

        private static TreeNode CreateTagNode( ITag tag, PanelViewModel panel )
        {
            var node = new TreeNode( tag.TagName );

            // если есть сигнал в списке панели, возвращаем его
            foreach ( var model in panel.Collection )
            {
                var bsModel = model as BaseSignalViewModel;
                if (bsModel != null)
                if (bsModel.Guid == tag.TagGUID)
                {
                    node.Checked = bsModel.IsChecked;
                    node.Tag = bsModel;
                    return node;
                }
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
        #endregion
        #endregion

        #region Handlers
        private void okButtonClickHandler(object sender, EventArgs e)
        {
            _view.Component.Caption = captionTextBox.Text;
            _view.UpdateWorkMode();
        }  

        private void cancelButtonClickHandler(object sender, EventArgs e)
        {
            _view.Component = originalPanel;
            //_view.Text = _originalCaption;
        }

        private void captionTextBoxTextChangedHandler(object sender, EventArgs e)
        {
            var captionViewModel = _view.Component.Collection.First() as CaptionViewModel;
            if (captionViewModel != null)
                captionViewModel.CaptionText = captionTextBox.Text;
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            var chechBox = (CheckBox)sender;

            if (chechBox.Checked)
            {
                label5.Enabled = true;
                captionTextBox.Enabled = true;
                _view.Component.IsCaptionVisible = true;

                if (!(_view.Component.Collection.First() is CaptionViewModel))
                {
                    var captionViewModel = new CaptionViewModel(captionTextBox.Text);
                    captionViewModel.FontSize = (UInt16) FontSizeNumericUpDown.Value;
                    _view.Component.Collection.Insert(0, captionViewModel);
                }
            }
            else
            {
                label5.Enabled = false;
                captionTextBox.Enabled = false;
                _view.Component.IsCaptionVisible = false;

                if (_view.Component.Collection.First() is CaptionViewModel)
                    _view.Component.Collection.RemoveAt(0);
            }
        }

        private void workModeComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (workModeComboBox.SelectedIndex)
            {
                case 0:
                    _view.Component.IsVisible = true;
                    _view.Component.IsAutomaticaly = true;
                    break;
                case 1:
                    _view.Component.IsVisible = true;
                    _view.Component.IsAutomaticaly = false;
                    break;
                case 2:
                    _view.Component.IsVisible = false;
                    break;
            }
        }

        private void treeView1_AfterCheck(object sender, TreeViewEventArgs e)
        {
            ViewModel.BaseSignalViewModel signalModel = (ViewModel.BaseSignalViewModel)e.Node.Tag;

            if (signalModel != null)
            {
                signalModel.IsChecked = e.Node.Checked;

                if (e.Node.Checked)
                {

                    ((Sources.BaseObjectCollection)_view.Component.Core).Collection.Add(signalModel.Core);
                    signalModel.FontSize = (UInt16)(FontSizeNumericUpDown.Value);
                    signalModel.Subscribe();
                    _view.Component.Collection.Add(signalModel);
                }
                else
                {
                    ((Sources.BaseObjectCollection)_view.Component.Core).Collection.Remove(signalModel.Core);
                    signalModel.UnSubscribe();
                    _view.Component.Collection.Remove(signalModel);
                }
            }
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            _view.SetOnEditMode();
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);

            _view.SetOffEditMode();
        }

        private void FontSizeNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            _view.Component.FontSize = (UInt16)(FontSizeNumericUpDown.Value);
        }
        #endregion
    }
}
