using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;

using DebugStatisticLibrary;

using HMI_MT_Settings;

using HelperControlsLibrary;
using InterfaceLibrary;

namespace DeviceFormLib.BlockTabs
{
    public partial class UstavkiViewControl : UserControl
    {
        private class TreeNodeDescription : TreeNode
        {
            public TreeNodeDescription( BaseDescription description ) : this( description, description.Name ) { }
            public TreeNodeDescription( BaseDescription description, String text ) : base( text ) { Tag = description; }
            public Category Category { get { return ( (BaseDescription)Tag ).Category; } }
        }

        private readonly BlockViewCollector collector;
        private readonly List<ITag> subscribes = new List<ITag>();
        private readonly DataGridView dBGridView;
        private readonly SelectControl selectControl;
        private readonly ReadWriteUstavkyControl readWriteUstavkyControl;
        private readonly FlowLayoutPanel flowLayoutPanel;
        private readonly Form parent;
        private readonly UInt32 uniDs;
        private readonly Category category;
        private readonly String groupName;

        public UstavkiViewControl( Form parent, UInt32 unids, UInt32 unidev, Category category, String groupName )
        {
            InitializeComponent();

            this.parent = parent;
            this.uniDs = unids;
            this.category = category;
            this.groupName = groupName;
            Guid = unidev;
            collector = new BlockViewCollector( unids, unidev );

            dBGridView = new DataGridView
            {
                Dock = DockStyle.Fill,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                RowHeadersVisible = false,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                AllowUserToResizeRows = false,
                ReadOnly = true
            };
            dBGridView.Columns.Add( new DataGridViewTextBoxColumn { HeaderText = "Дата", Width = 190 } );
            dBGridView.Columns.Add( new DataGridViewTextBoxColumn { HeaderText = "Комментарий", Width = 100 } );
            dBGridView.DoubleClick += this.DbGridViewOnDoubleClick;

            selectControl = new SelectControl { BorderStyle = BorderStyle.FixedSingle };
            selectControl.btnUpdate.Click += BtnUpdateOnClick;

            readWriteUstavkyControl = new ReadWriteUstavkyControl { BorderStyle = BorderStyle.FixedSingle };
            readWriteUstavkyControl.btnReadUstFC.Click += BtnReadUstFcOnClick;
            readWriteUstavkyControl.btnWriteUst.Click += BtnWriteUstOnClick;
            readWriteUstavkyControl.btnResetValues.Click += BtnResetValuesOnClick;
            readWriteUstavkyControl.btnFix4Change.CheckedChanged += BtnFix4ChangeOnCheckedChanged;

            flowLayoutPanel = new FlowLayoutPanel { Dock = DockStyle.Fill };
            flowLayoutPanel.Controls.Add( selectControl );
            flowLayoutPanel.Controls.Add( readWriteUstavkyControl );
        }
        public void BlockViewControlClosing( object sender, FormClosingEventArgs e )
        {
            foreach ( DataGridViewRow row in this.dataGridView1.Rows )
            {
                var tag = row.Tag as TagDescription;
                if ( tag != null && tag.Formula != null )
                    tag.Formula.OnChangeValFormTI -= this.FormulaOnOnChange;
            }
            this.dataGridView1.Rows.Clear( );
            HMI_Settings.HmiTagsSubScribes( subscribes, HMI_Settings.SubscribeAction.UnSubscribe );
        }
        private void BlockViewControlLoad( object sender, EventArgs e )
        {
            try
            {
                DebugStatistics.WindowStatistics.AddStatistic( "Инициализация сбора данных блока." );

                var device = HMI_Settings.CONFIGURATION.GetLink2Device( this.uniDs, this.Guid );
                if ( device == null )
                    throw new Exception(
                        string.Format( "FrmEngine: Нет связанного устройства с данной формой unids = {0}; unidev = {1}",
                                       this.uniDs.ToString( CultureInfo.InvariantCulture ),
                                       this.Guid.ToString( CultureInfo.InvariantCulture ) ) );

                var group = device.GetGroupHierarchy( ).FirstOrDefault( g => g.NameGroup == this.groupName );
                if ( group != null )
                    collector.Collect( group );

                DebugStatistics.WindowStatistics.AddStatistic( "Инициализация сбора данных блока завершена." );

                DebugStatistics.WindowStatistics.AddStatistic( "Инициализация построения дерева данных." );
                
                var nodes = CreateGroupNodes( collector.Groups );
                if ( nodes != null )
                    treeView1.Nodes.AddRange( nodes.ToArray( ) );
                
                DebugStatistics.WindowStatistics.AddStatistic( "Инициализация построения дерева данных завершена." );
            }
            catch ( Exception ex )
            {
                TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG( ex );
            }
        }
        private void InsertToTable( TreeNode node )
        {
            var groupDescription = node.Tag as GroupDescription;
            if ( groupDescription != null )
                foreach ( TreeNode subNode in node.Nodes )
                    this.InsertToTable( subNode );

            var tagDescription = node.Tag as TagDescription;
            if ( tagDescription == null || tagDescription.Source == null ) return;

            this.dataGridView1.Rows.Add( CreateDataRow( tagDescription ) );
            this.subscribes.Add( tagDescription.Source );
        }
        private bool FormulaOnOnChange( string format, object value, TypeOfTag type )
        {
            foreach ( DataGridViewRow row in this.dataGridView1.Rows )
            {
                var tag = (TagDescription)row.Tag;
                if ( tag == null || !tag.Result.Equals( format ) || row.Cells[1].Value.Equals( value ) ) continue;

                row.Cells[1].Value = value.ToString();
                return true;
            }
            return false;
        }
        private void TreeView1Click( object sender, EventArgs e )
        {
            var tree = (TreeView)sender;

            foreach ( DataGridViewRow row in this.dataGridView1.Rows )
            {
                var tag = row.Tag as TagDescription;
                if ( tag != null && tag.Formula != null )
                    tag.Formula.OnChangeValFormTI -= this.FormulaOnOnChange;
            }
            HMI_Settings.HmiTagsSubScribes( subscribes, HMI_Settings.SubscribeAction.UnSubscribe );
            this.dataGridView1.Rows.Clear( );
            this.subscribes.Clear( );

            var treeNode = (TreeNodeDescription)tree.GetNodeAt( ( (MouseEventArgs)e ).Location );
            this.InsertToTable( treeNode );
            this.dataGridView1.Columns[1].ReadOnly = true;

            foreach ( DataGridViewRow row in this.dataGridView1.Rows )
            {
                var tag = row.Tag as TagDescription;
                if ( tag != null && tag.Formula != null )
                    tag.Formula.OnChangeValFormTI += this.FormulaOnOnChange;
            }
            HMI_Settings.HmiTagsSubScribes( subscribes, HMI_Settings.SubscribeAction.Subscribe );

            this.ActivateSelectComponents( );
            this.dBGridView.Tag = treeNode.Category;
        }
        private void ActivateSelectComponents( )
        {
            switch ( category )
            {
                case Category.Crush:
                    {
                        if ( this.tableLayoutPanel1.ColumnCount == 1 )
                        {
                            this.tableLayoutPanel1.ColumnCount = 2;
                            this.tableLayoutPanel1.ColumnStyles.Add( new ColumnStyle( SizeType.Absolute, 200 ) );

                            this.tableLayoutPanel1.Controls.Add( dBGridView, 2, 0 );
                            this.dBGridView.Rows.Clear();
                        }
                        if ( this.tableLayoutPanel1.RowCount == 1 )
                        {
                            this.tableLayoutPanel1.RowCount = 2;
                            this.tableLayoutPanel1.RowStyles.Add( new RowStyle( SizeType.Absolute, 70 ) );

                            this.tableLayoutPanel1.Controls.Add( flowLayoutPanel, 0, 1 );
                        }

                        this.readWriteUstavkyControl.Visible = false;
                        this.dBGridView.Rows.Clear( );
                    }
                    break;
                case Category.Ustavki:
                    {
                        if ( this.tableLayoutPanel1.ColumnCount == 1 )
                        {
                            this.tableLayoutPanel1.ColumnCount = 2;
                            this.tableLayoutPanel1.ColumnStyles.Add( new ColumnStyle( SizeType.Absolute, 200 ) );

                            this.tableLayoutPanel1.Controls.Add( dBGridView, 2, 0 );
                            this.dBGridView.Rows.Clear();
                        }
                        if ( this.tableLayoutPanel1.RowCount == 1 )
                        {
                            this.tableLayoutPanel1.RowCount = 2;
                            this.tableLayoutPanel1.RowStyles.Add( new RowStyle( SizeType.Absolute, 70 ) );

                            this.tableLayoutPanel1.Controls.Add( flowLayoutPanel, 0, 1 );
                        }

                        this.readWriteUstavkyControl.Visible = true;
                        this.dBGridView.Rows.Clear( );
                    }
                    break;
                default:
                    {
                        if ( this.tableLayoutPanel1.ColumnCount == 2 )
                        {
                            this.tableLayoutPanel1.ColumnCount = 1;
                            this.tableLayoutPanel1.ColumnStyles.RemoveAt( 1 );

                            this.tableLayoutPanel1.Controls.Remove( dBGridView );
                        }
                        if ( this.tableLayoutPanel1.RowCount == 2 )
                        {
                            this.tableLayoutPanel1.RowCount = 1;
                            this.tableLayoutPanel1.RowStyles.RemoveAt( 1 );

                            this.tableLayoutPanel1.Controls.Remove( flowLayoutPanel );
                        }
                    }
                    break;
            }
        }
        private void DbGridViewOnDoubleClick( object sender, EventArgs eventArgs )
        {
            if ( this.dBGridView.SelectedRows.Count == 0 ) return;
            
            try
            {
                var idBlock = (int)this.dBGridView.SelectedRows[0].Tag;
                var arparam = new System.Collections.ArrayList
                                  {
                                      idBlock,
                                      System.Text.Encoding.UTF8.GetBytes( HMI_Settings.ProviderPtkSql )
                                  };

                switch ( (Category)this.dBGridView.Tag )
                {
                    case Category.Crush:
                        HMI_Settings.CONFIGURATION.GetData( 0, Guid, "ArhivAvariBlockData", arparam, idBlock );
                        break;
                    case Category.Ustavki:
                        HMI_Settings.CONFIGURATION.GetData( 0, Guid, "ArhivUstavkiBlockData", arparam, idBlock );
                        break;
                }
            }
            catch ( Exception ex )
            {
                TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG( ex );
            }
        }
        /// <summary>
        /// Обновление списка архивных записей
        /// </summary>
        private void BtnUpdateOnClick( object sender, EventArgs eventArgs )
        {
            var typeBlock = TypeBlockData4Req.TypeBlockData4Req_Unknown;
            switch ( (Category)dBGridView.Tag )
            {
                case Category.Crush: typeBlock = TypeBlockData4Req.TypeBlockData4Req_Srabat; break;
                case Category.Ustavki: typeBlock = TypeBlockData4Req.TypeBlockData4Req_Ustavki; break;
            }

            var list = BlockViewCollector.AvarUstavkyDataBase( Guid, selectControl.StartDateCollapsed,
                                                               selectControl.EndTimeCollapsed, typeBlock );
            if ( list == null )
            {
                MessageBox.Show( "Ошибка работы с базой данных.", Text, MessageBoxButtons.OK, MessageBoxIcon.Error );
                return;
            }

            if ( list.Count == 0 )
                MessageBox.Show( "Архивных данных для этого устройства нет.", Text, MessageBoxButtons.OK, MessageBoxIcon.Information );
            else
            {
                dBGridView.Rows.Clear( );
                foreach ( var row in list )
                    this.dBGridView.Rows.Add( row );
            }
        }
        /// <summary>
        /// Чтение уставок
        /// </summary>
        private void BtnReadUstFcOnClick( object sender, EventArgs eventArgs )
        {
            // правильная запись в журнал действий пользователя номер устройства с цчетом фк
            CommonUtils.CommonUtils.WriteEventToLog( 7, Guid.ToString( CultureInfo.InvariantCulture ), true );
            // выдача команда IMP
            HMI_Settings.CONFIGURATION.ExecuteCommand( 0, Guid, "IMP", new byte[] { }, parent );

            this.BtnResetValuesOnClick( sender, eventArgs );
            HMI_Settings.HmiTagsSubScribes( subscribes, HMI_Settings.SubscribeAction.Subscribe );
        }
        /// <summary>
        /// Запись уставок
        /// </summary>
        private void BtnWriteUstOnClick( object sender, EventArgs eventArgs )
        {
            try
            {
                //// определим теги которые изменились
                //var lstModifiedTags = engineNew.GetChangedTags( pnlTPConfig );

                //if ( !lstModifiedTags.Any() )
                //{
                //    MessageBox.Show( "Уставки не изменялись. \nВыполнение команды отменено.", "Сообщение", MessageBoxButtons.OK, MessageBoxIcon.Information );
                //    HMI_Settings.SetTagsSubscribe4TPCurrent( tpConfig );
                //    return;
                //}

                //if ( CommonUtils.CommonUtils.IsUserActionBan( CommonUtils.CommonUtils.UserActionType.b05_Set_ustav_config, HMI_Settings.UserRight ) )
                //{
                //    HMI_Settings.SetTagsSubscribe4TPCurrent( tpConfig );
                //    return;
                //}

                //if ( HMI_Settings.isRegPass && CommonUtils.CommonUtils.CanAction() )
                //{
                //    if ( MessageBox.Show( "Записать уставки?", "Предупреждение", MessageBoxButtons.YesNo, MessageBoxIcon.Question ) == DialogResult.No )
                //    {
                //        HMI_Settings.SetTagsSubscribe4TPCurrent( tpConfig );
                //        return;
                //    }
                //    // правильная запись в журнал действий пользователя номер устройства с учетом фк
                //    CommonUtils.CommonUtils.WriteEventToLog( 6, Guid.ToString( CultureInfo.InvariantCulture ), true );
                //    // выдача команда WCP
                //    HMI_Settings.CONFIGURATION.ExecuteCommand( 0, Guid, "WCP", CommonUtils.CommonUtils.GetUstConfMemXPacket( (List<ITag>)lstModifiedTags ), this );

                //    // сбрасываем признаки изменения тега
                //    CommonUtils.CommonUtils.ResetIndicationModifiedTag( tpConfig );
                //    HMI_Settings.SetTagsSubscribe4TPCurrent( tpConfig );
                //}
            }
            catch ( Exception ex )
            {
                TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG( ex );
            }
        }
        /// <summary>
        /// Очистить поля формы
        /// </summary>
        private void BtnResetValuesOnClick( object sender, EventArgs eventArgs )
        {
            // отписались и обнулились
            HMI_Settings.HmiTagsSubScribes( subscribes, HMI_Settings.SubscribeAction.UnSubscribeAndClear );
            foreach ( DataGridViewRow row in this.dataGridView1.Rows )
                row.Cells[1].Value = DataGridRowClear( (TagDescription)row.Tag );
        }
        /// <summary>
        /// Проверка режима задания уставок
        /// </summary>
        private void BtnFix4ChangeOnCheckedChanged( object sender, EventArgs eventArgs )
        {
            var chBox = (CheckBox)sender;
            if ( chBox.Checked )
            {
                HMI_Settings.HmiTagsSubScribes( subscribes, HMI_Settings.SubscribeAction.UnSubscribe );
                this.dataGridView1.Columns[1].ReadOnly = false;
            }
            else
            {
                HMI_Settings.HmiTagsSubScribes( subscribes, HMI_Settings.SubscribeAction.Subscribe );
                this.dataGridView1.Columns[1].ReadOnly = true;
            }
        }

        public UInt32 Guid { get; private set; }

        private static object DataGridRowClear( TagDescription description )
        {
            switch ( description.Source.TypeOfTagHMI )
            {
                case TypeOfTag.Combo: return description.Source.SlEnumsParty[0];
                case TypeOfTag.Analog:
                case TypeOfTag.Discret: return 0;
                case TypeOfTag.DateTime: return DateTime.MinValue.ToString( CultureInfo.InvariantCulture );
                default: return string.Empty;
            }
        }
        private static DataGridViewRow CreateDataRow( TagDescription tagDescription )
        {
            var newRow = new DataGridViewRow { Tag = tagDescription };
            switch ( tagDescription.Source.TypeOfTagHMI )
            {
                case TypeOfTag.Combo:
                    newRow.Cells.Add( new DataGridViewTextBoxCell { Value = tagDescription.Name } );
                    var cell = new DataGridViewComboBoxCell();
                    newRow.Cells.Add( cell );
                    foreach ( var value in tagDescription.Source.SlEnumsParty )
                        cell.Items.Add( value.Value );
                    cell.Value = tagDescription.Source.ValueAsString;
                    newRow.Cells.Add( new DataGridViewTextBoxCell { Value = tagDescription.Uom } );
                    break;
                case TypeOfTag.Analog:
                case TypeOfTag.Discret:
                    newRow.Cells.Add( new DataGridViewTextBoxCell { Value = tagDescription.Name } );
                    newRow.Cells.Add( new DataGridViewTextBoxCell
                    {
                        Value = ( string.IsNullOrEmpty( tagDescription.Source.ValueAsString ) ) ? "0" : tagDescription.Source.ValueAsString
                    } );
                    newRow.Cells.Add( new DataGridViewTextBoxCell { Value = tagDescription.Uom } );
                    break;
                case TypeOfTag.DateTime:
                    newRow.Cells.Add( new DataGridViewTextBoxCell { Value = tagDescription.Name } );
                    newRow.Cells.Add( new DataGridViewTextBoxCell
                    {
                        Value = ( string.IsNullOrEmpty( tagDescription.Source.ValueAsString ) )
                                    ? DateTime.MinValue.ToString( CultureInfo.InvariantCulture )
                                    : tagDescription.Source.ValueAsString
                    } );
                    newRow.Cells.Add( new DataGridViewTextBoxCell { Value = tagDescription.Uom } );
                    break;
                default:
                    newRow.Cells.Add( new DataGridViewTextBoxCell { Value = tagDescription.Name } );
                    newRow.Cells.Add( new DataGridViewTextBoxCell { Value = tagDescription.Source.ValueAsString } );
                    newRow.Cells.Add( new DataGridViewTextBoxCell { Value = tagDescription.Uom } );
                    break;
            }
            return newRow;
        }
        private static IEnumerable<TreeNode> CreateGroupNodes( IEnumerable<GroupDescription> groupDescriptions )
        {
            if ( groupDescriptions == null || !groupDescriptions.Any() ) return null;

            var nodeList = new List<TreeNode>();
            foreach ( var groupDescription in groupDescriptions )
            {
                var node = new TreeNodeDescription( groupDescription );
                var subNodes = CreateGroupNodes( groupDescription.Groups );
                if ( subNodes != null ) node.Nodes.AddRange( subNodes.ToArray() );
                var tags = CreateTagNodes( groupDescription.Tags );
                if ( tags != null ) node.Nodes.AddRange( tags.ToArray() );

                nodeList.Add( node );
            }

            return nodeList;
        }
        private static IEnumerable<TreeNode> CreateTagNodes( IEnumerable<TagDescription> tagDescriptions )
        {
            if ( tagDescriptions == null || !tagDescriptions.Any() ) return null;

            return
                tagDescriptions.Select(
                    tagDescription =>
                    new TreeNodeDescription( tagDescription, TagDescription.NodeTitle( tagDescription ) ) ).
                    ToList();
        }
    }

    public class UstavkiViewTabPage : TabPage
    {
        public readonly UstavkiViewControl Control;

        public UstavkiViewTabPage( Form parent, UInt32 unids, UInt32 unidev, string text = "Уставки" )
            : base( text )
        {
            this.Control = new UstavkiViewControl( parent, unids, unidev, Category.Ustavki, "Уставки" ) { Dock = DockStyle.Fill };
            Controls.Add( this.Control );
        }
    }
}
