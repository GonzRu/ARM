using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using NMS = NormalModeLibrary.Sources;
using NMVM = NormalModeLibrary.ViewModel;

namespace NormalModeLibrary.Windows
{
    public partial class UserWindows : Form
    {
        bool isChange = false;
        internal UserWindows( List<NMVM.UserViewModel> users )
        {
            InitializeComponent();

            foreach ( NMVM.UserViewModel vmUser in users )
            {
                NMVM.ViewModelBase vmBaseCopy = vmUser.Copy();
                UnSubscribeAll( vmBaseCopy );
                TreeNode userNode = CreateNode( vmBaseCopy );
                treeView1.Nodes.Add( userNode );
            }
        }
        /// <summary>
        /// Загрузка окна
        /// </summary>
        private void UserWindows_Load( object sender, EventArgs e )
        {
            treeView1_AfterSelect( treeView1, null );
        }
        /// <summary>
        /// Выбор элемента
        /// </summary>
        private void treeView1_AfterSelect( object sender, TreeViewEventArgs e )
        {
            if ( treeView1.SelectedNode == null ||
                treeView1.SelectedNode.Tag is NMVM.DigitalViewModel ||
                treeView1.SelectedNode.Tag is NMVM.AnalogViewModel )
            {
                button2.Enabled = false;
                button3.Enabled = false;
                button4.Enabled = false;
                return;
            }
            if ( treeView1.SelectedNode.Tag is NMVM.UserViewModel )
            {
                button2.Enabled = true;
                button3.Enabled = false;
                button4.Enabled = false;
            }
            if ( treeView1.SelectedNode.Tag is NMVM.ConfigurationViewModel )
            {
                button2.Enabled = false;
                button3.Enabled = true;
                button4.Enabled = false;
            }
            if ( treeView1.SelectedNode.Tag is NMVM.PanelViewModel )
            {
                button2.Enabled = false;
                button3.Enabled = false;
                button4.Enabled = true;
            }
        }
        /// <summary>
        /// Создание пользователя
        /// </summary>
        private void button1_Click( object sender, EventArgs e )
        {
            UserWindow win = new UserWindow();
            win.Owner = this;
            if ( win.ShowDialog() == System.Windows.Forms.DialogResult.OK )
            {
                isChange = true;
                NMS.User user = win.GetUser();
                treeView1.Nodes.Add( CreateNode( user, new NMVM.UserViewModel( user ) ) );
            }
        }
        /// <summary>
        /// Создание конфигурации
        /// </summary>
        private void button2_Click( object sender, EventArgs e )
        {
            if ( treeView1.SelectedNode == null ) return;

            ConfigurationWindow win = new ConfigurationWindow();
            win.Owner = this;
            if ( win.ShowDialog() == System.Windows.Forms.DialogResult.OK )
            {
                isChange = true;
                NMS.Configuration config = win.GetConfiguration();
                treeView1.SelectedNode.Nodes.Add( CreateNode( config, new NMVM.ConfigurationViewModel( config ) ) );
            }
        }
        /// <summary>
        /// Создание панели компонентов
        /// </summary>
        private void button3_Click( object sender, EventArgs e )
        {
            if ( treeView1.SelectedNode == null ) return;

            PanelWindow win = new PanelWindow();
            win.Owner = this;
            if ( win.ShowDialog() == System.Windows.Forms.DialogResult.OK )
            {
                isChange = true;
                NMS.Panel panel = win.GetPanel();
                treeView1.SelectedNode.Nodes.Add( CreateNode( panel, new NMVM.PanelViewModel( panel ) ) );
            }
        }
        /// <summary>
        /// Создание компонентов
        /// </summary>
        private void button4_Click( object sender, EventArgs e )
        {
            if ( treeView1.SelectedNode == null ) return;

            SignalWindow win = new SignalWindow();
            win.Owner = this;
            if ( win.ShowDialog() == System.Windows.Forms.DialogResult.OK )
            {
                isChange = true;
                Sources.BaseSignal signal = win.GetSignal();
                treeView1.SelectedNode.Nodes.Add( CreateNode( signal, NMVM.BaseSignalViewModel.GetSignalViewModel( signal ) ) );
            }
        }
        /// <summary>
        /// Редакторование компанента
        /// </summary>
        private void button5_Click( object sender, EventArgs e )
        {
            if ( treeView1.SelectedNode == null ) return;

            if ( treeView1.SelectedNode.Tag is NMVM.UserViewModel )
            {
                NMVM.UserViewModel vmUser = (NMVM.UserViewModel)treeView1.SelectedNode.Tag;
                UserWindow win = new UserWindow();
                win.Owner = this;
                win.SetUser( (NMS.User)vmUser.Core );
                
                if ( win.ShowDialog() == System.Windows.Forms.DialogResult.OK )
                {
                    isChange = true;
                    win.ApplyData();
                    treeView1.SelectedNode.Text = vmUser.Core.GetTreeNodeText();
                }
            }
            else if ( treeView1.SelectedNode.Tag is NMVM.ConfigurationViewModel )
            {
                NMVM.ConfigurationViewModel vmConfig = (NMVM.ConfigurationViewModel)treeView1.SelectedNode.Tag;
                ConfigurationWindow win = new ConfigurationWindow();
                win.Owner = this;
                win.SetConfiguration( (NMS.Configuration)vmConfig.Core );
                
                if ( win.ShowDialog() == System.Windows.Forms.DialogResult.OK )
                {
                    isChange = true;
                    win.ApplyData();
                    treeView1.SelectedNode.Text = vmConfig.Core.GetTreeNodeText();
                }
            }
            else if ( treeView1.SelectedNode.Tag is NMVM.PanelViewModel )
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

                if ( parentNode == null )
                    treeView1.Nodes.Remove( treeView1.SelectedNode );
                else
                    parentNode.Nodes.Remove( treeView1.SelectedNode );
            }
        }
        /// <summary>
        /// Закрытие формы
        /// </summary>
        private void UserWindows_FormClosing( object sender, FormClosingEventArgs e )
        {
            if ( isChange &&
                MessageBox.Show( this, "Внесены изменения, принять?", "Вопрос", MessageBoxButtons.YesNo, MessageBoxIcon.Question ) ==
                System.Windows.Forms.DialogResult.Yes )
            {
                //сохранение
                List<NMVM.UserViewModel> list = new List<NMVM.UserViewModel>();
                foreach ( TreeNode node in treeView1.Nodes )
                    list.Add( (NMVM.UserViewModel)GetViewModel( node ) );

                if ( ComponentFactory.SaveXml( list ) )
                    MessageBox.Show( this, "Конфигурация панелей пересохранена.\nПерезагрузите клиент.", "Информация",
                        MessageBoxButtons.OK, MessageBoxIcon.Information );
            }
        }

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
                foreach ( NMVM.ViewModelBase model in vmBaseColl.Collection )
                    node.Nodes.Add( CreateNode( model ) );

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
    }
}
