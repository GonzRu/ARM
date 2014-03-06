using System;
using System.Collections.Generic;
using System.Windows.Forms;
using NormalModeLibrary.Sources;
using NMS = NormalModeLibrary.Sources;
using NMVM = NormalModeLibrary.ViewModel;

namespace NormalModeLibrary.Windows
{
    public partial class UserWindows : Form
    {
        #region Private Fields
        private bool isChange = false;

        private List<NMVM.UserViewModel> _copyUserViewModels;

        // Первая конфигурация пользователя
        private NMVM.BaseCollectionViewModel _userFirstConfiguration;

        private string _userNmae;
        #endregion Private Fields

        #region Constructor
        internal UserWindows( List<NMVM.UserViewModel> users, string currentUserName )
        {
            InitializeComponent();

            _copyUserViewModels = users;
            _userNmae = currentUserName;

            foreach ( NMVM.UserViewModel vmUser in users )
            {
                if (vmUser.Login == currentUserName)
                {
                    //UnSubscribeAll(vmBaseCopy);

                    // Берем только первую конфигурацию, так как фактически конфигурации не работают и все панели добавляются в первую и единственную.
                    _userFirstConfiguration = ((vmUser as NMVM.UserViewModel).Collection[0] as NMVM.BaseCollectionViewModel);

                    foreach (var panelViewModel in _userFirstConfiguration.Collection)
                    {
                        TreeNode userNode = CreateNode(panelViewModel);
                        treeView1.Nodes.Add(userNode);
                    }

                    return;
                }
            }
        }
        #endregion Constructor

        #region Handlers
        /// <summary>
        /// Редакторование компанента
        /// </summary>
        private void button5_Click( object sender, EventArgs e )
        {
            if ( treeView1.SelectedNode == null ) return;

            if ( treeView1.SelectedNode.Tag is NMVM.PanelViewModel )
            {
                NMVM.PanelViewModel vmPanel = (NMVM.PanelViewModel)treeView1.SelectedNode.Tag;
                PanelWindow win = new PanelWindow();
                win.Owner = this;
                win.SetPanel( (NMS.Panel)vmPanel.Core );

                if ( win.ShowDialog() == System.Windows.Forms.DialogResult.OK )
                {
                    isChange = true;
                    win.ApplyData();
                    treeView1.SelectedNode.Text = vmPanel.Core.GetTreeNodeText();
                }
            }
            else if ( treeView1.SelectedNode.Tag is NMVM.BaseSignalViewModel )
            {
                NMVM.BaseSignalViewModel vmSignal = (NMVM.BaseSignalViewModel)treeView1.SelectedNode.Tag;
                SignalWindow win = new SignalWindow();
                win.Owner = this;
                win.SetSignal( (NMS.BaseSignal)vmSignal.Core );

                if ( win.ShowDialog() == System.Windows.Forms.DialogResult.OK )
                {
                    isChange = true;
                    win.ApplyData();
                    treeView1.SelectedNode.Text = vmSignal.Core.GetTreeNodeText();
                }
            }
        }

        /// <summary>
        /// Удаление компанента
        /// </summary>
        private void button6_Click( object sender, EventArgs e )
        {
            if ( treeView1.SelectedNode == null ) return;

            if ( MessageBox.Show( this, "Удаляем?", "Вопрос", MessageBoxButtons.YesNo, MessageBoxIcon.Question ) ==
                System.Windows.Forms.DialogResult.Yes )
            {
                isChange = true;
                TreeNode parentNode = treeView1.SelectedNode.Parent as TreeNode;

                if (parentNode == null)
                {
                    ((Configuration)_userFirstConfiguration.Core).Collection.Remove((treeView1.SelectedNode.Tag as NMVM.PanelViewModel).BasePanel);
                    treeView1.Nodes.Remove(treeView1.SelectedNode);
                }
                else
                    parentNode.Nodes.Remove(treeView1.SelectedNode);
            }
        }

        /// <summary>
        /// Закрытие формы
        /// </summary>
        private void UserWindows_FormClosing( object sender, FormClosingEventArgs e )
        {
            Save();
        }

        private void OkButton_Click(object sender, EventArgs e)
        {
            Save();
            Close();
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            if (isChange)
                ReloadNormalModePanels();

            Close();
        }
        #endregion Handlers

        #region Private Metods
        private static TreeNode CreateNode( NMS.BaseObject baseObject, NMVM.ViewModelBase vmBase )
        {
            TreeNode node = new TreeNode( baseObject.GetTreeNodeText() );
            node.Tag = vmBase;
            return node;
        }
        private static TreeNode CreateNode( NMVM.ViewModelBase vmBase )
        {
            TreeNode node = CreateNode( vmBase.Core, vmBase );
            NMVM.BaseCollectionViewModel vmBaseColl = vmBase as NMVM.BaseCollectionViewModel;

            if ( vmBaseColl != null )
                foreach (NMVM.ViewModelBase model in vmBaseColl.Collection)
                {
                    if (model is NMVM.CaptionViewModel)
                        continue;

                    node.Nodes.Add(CreateNode(model));
                }

            return node;
        }
        private static NMVM.ViewModelBase GetViewModel( TreeNode node )
        {
            NMVM.ViewModelBase vmBase = node.Tag as NMVM.ViewModelBase;
            
            NMVM.BaseCollectionViewModel vmColl = vmBase as NMVM.BaseCollectionViewModel;
            if ( vmColl != null )
            {
                vmColl.Collection.Clear();
                ( (NMS.BaseObjectCollection)vmColl.Core ).Collection.Clear();
                
                foreach ( TreeNode child in node.Nodes )
                {
                    NMVM.ViewModelBase model = GetViewModel(child);
                    vmColl.Collection.Add( model );
                    ( (NMS.BaseObjectCollection)vmColl.Core ).Collection.Add( model.Core );
                }
            }

            return vmBase;
        }
        private static void UnSubscribeAll( NMVM.ViewModelBase vmBase )
        {
            NMVM.BaseCollectionViewModel vmColl = vmBase as NMVM.BaseCollectionViewModel;
            if ( vmColl != null )
                foreach ( NMVM.ViewModelBase model in vmColl.Collection )
                    UnSubscribeAll( model );

            NMVM.BaseSignalViewModel vmSignal = vmBase as NMVM.BaseSignalViewModel;
            if ( vmSignal != null && vmSignal.IsSubscribe )
                vmSignal.UnSubscribe();
        }

        private void Save()
        {
            if (isChange &&
                MessageBox.Show(this, "Внесены изменения, принять?", "Вопрос", MessageBoxButtons.YesNo,
                                MessageBoxIcon.Question) == DialogResult.Yes)
            {
                //сохранение
                List<NMVM.UserViewModel> list = new List<NMVM.UserViewModel>();


                _userFirstConfiguration.Collection.Clear();

                foreach (TreeNode node in treeView1.Nodes)
                    _userFirstConfiguration.Collection.Add((NMVM.PanelViewModel) GetViewModel(node));

                ComponentFactory.SaveXml(_copyUserViewModels);

                ReloadNormalModePanels();

                isChange = false;
            }            
        }

        private void ReloadNormalModePanels()
        {
            ComponentFactory.Factory.DeactivatedMainMnemoForms();
            ComponentFactory.Factory.LoadXml();
            ComponentFactory.Factory.ActivatedMainMnemoForms(Owner);
        }
        #endregion Private Metods
    }
}

