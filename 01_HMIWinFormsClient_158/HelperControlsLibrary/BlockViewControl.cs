using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;

using DebugStatisticLibrary;
using HMI_MT_Settings;
using InterfaceLibrary;

namespace HelperControlsLibrary
{
    /// <summary>
    /// Контрол отображения данных блока
    /// </summary>
    public partial class BlockViewControl : UserControl, ReportLibrary.IReport
    {
        /// <summary>
        /// Описание узла дерева
        /// </summary>
        private class TreeNodeDescription : TreeNode
        {
            /// <summary>
            /// Узел дерева
            /// </summary>
            /// <param name="description">Описание</param>
            public TreeNodeDescription( BaseDescription description ) : this( description, description.Name ) { }
            /// <summary>
            /// Узел дерева
            /// </summary>
            /// <param name="description">Описание</param>
            /// <param name="text">Имя отображения</param>
            public TreeNodeDescription( BaseDescription description, String text ) : base( text ) { Tag = description; }
            /// <summary>
            /// Категория
            /// </summary>
            public Category Category { get { return ( (BaseDescription)Tag ).Category; } }
        }
        
        /// <summary>
        /// Делегат выдачи категории
        /// </summary>
        /// <param name="category">Категория</param>
        public delegate void CategoryDelagete( Category category );
        /// <summary>
        /// Событие выдачи категории
        /// </summary>
        public event CategoryDelagete CategoryEvent;

        /// <summary>
        /// Идентификатор
        /// </summary>
        private readonly UInt32 uniDs, uniDev;
        /// <summary>
        /// Сборщик
        /// </summary>
        private readonly BlockViewCollector collector;
        /// <summary>
        /// Список тэгов выбраной ветви дерева
        /// </summary>
        private readonly List<ITag> subscribes = new List<ITag>( );

        /// <summary>
        /// Отображение данных блока
        /// </summary>
        /// <param name="unids">ID датасервера</param>
        /// <param name="unidev">ID устройства</param>
        public BlockViewControl( UInt32 unids, UInt32 unidev )
        {
            InitializeComponent( );

            this.uniDs = unids;
            this.uniDev = unidev;
            collector = new BlockViewCollector( unids, unidev );
        }
        private void DataGridRowCellEndEdit( object sender, DataGridViewCellEventArgs args )
        {
            var gridRow = this.dataGridView1.Rows[args.RowIndex];
            var tagDesc = gridRow.Tag as TagDescription;
            if ( tagDesc == null || tagDesc.Source == null ) return;

            tagDesc.IsChange = true;

            try
            {
                switch ( tagDesc.Source.TypeOfTagHMI )
                {
                    case TypeOfTag.Combo:
                        {
                            var cbCell =
                                gridRow.Cells[1] as DataGridViewComboBoxCell;
                            if ( cbCell == null )
                                throw new NullReferenceException( "ComboBox Cell is Null" );

                            foreach ( var part in tagDesc.Source.SlEnumsParty )
                                if ( part.Value.Equals( cbCell.Value ) )
                                {
                                    var memX = BitConverter.GetBytes( Convert.ToSingle( part.Key ) );
                                    tagDesc.Source.SetValue( memX, DateTime.Now, VarQualityNewDs.vqGood );
                                    break;
                                }
                        }
                        break;
                    case TypeOfTag.Analog:
                        {
                            var memX = BitConverter.GetBytes( Convert.ToSingle( gridRow.Cells[1].Value ) );
                            tagDesc.Source.SetValue( memX, DateTime.Now, VarQualityNewDs.vqGood );
                        }
                        break;
                    case TypeOfTag.Discret:
                        {
                            var value = string.Empty;
                            if ( gridRow.Cells[1].Value.Equals( "1" ) ) value = "true";
                            if ( gridRow.Cells[1].Value.Equals( "0" ) ) value = "false";

                            var memX = BitConverter.GetBytes( Convert.ToBoolean( value ) );
                            tagDesc.Source.SetValue( memX, DateTime.Now, VarQualityNewDs.vqGood );
                        }
                        break;
                    case TypeOfTag.DateTime:
                        {

                            var memX = BitConverter.GetBytes( Convert.ToInt64( DateTime.Parse( gridRow.Cells[1].Value.ToString( ) ) ) );
                            tagDesc.Source.SetValue( memX, DateTime.Now, VarQualityNewDs.vqGood );
                        }
                        break;
                    default:
                        {
                            var memX = System.Text.Encoding.Default.GetBytes( gridRow.Cells[1].Value.ToString( ) );
                            tagDesc.Source.SetValue( memX, DateTime.Now, VarQualityNewDs.vqGood );
                        }
                        break;
                }
            }
            catch ( Exception exception )
            {
                TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG( exception );
            }
        }
        /// <summary>
        /// Загрузка контрола
        /// </summary>
        private void BlockViewControlLoad( object sender, EventArgs e )
        {
            try
            {
                DebugStatistics.WindowStatistics.AddStatistic( "Инициализация сбора данных блока." );

                var device = HMI_Settings.CONFIGURATION.GetLink2Device( this.uniDs, this.uniDev );
                if ( device == null )
                    throw new Exception(
                        string.Format( "FrmEngine: Нет связанного устройства с данной формой unids = {0}; unidev = {1}",
                                       this.uniDs.ToString( CultureInfo.InvariantCulture ),
                                       this.uniDev.ToString( CultureInfo.InvariantCulture ) ) );

                collector.Collect( device.GetGroupHierarchy( ) );

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
        /// <summary>
        /// Наполнение таблицы данными
        /// </summary>
        /// <param name="node">Узел дерева</param>
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
        /// <summary>
        /// Событие изменения значений данных тэга
        /// </summary>
        /// <param name="format">Полная строка представления тэга</param>
        /// <param name="value">Значение</param>
        /// <param name="type">Тип тэга</param>
        /// <returns>Признак изменения тэга</returns>
        private bool FormulaOnOnChange( string format, object value, TypeOfTag type )
        {
            foreach ( DataGridViewRow row in this.dataGridView1.Rows )
            {
                var tag = (TagDescription)row.Tag;
                if ( tag == null || !tag.Result.Equals( format ) || row.Cells[1].Value.Equals( value ) ) continue;

                row.Cells[1].Value = value.ToString( );
                return true;
            }
            return false;
        }
        /// <summary>
        /// Событие выбора узла дерева
        /// </summary>
        private void TreeView1Click( object sender, EventArgs e )
        {
            var tree = (TreeView)sender;

            foreach ( DataGridViewRow row in this.dataGridView1.Rows )
            {
                var tag = row.Tag as TagDescription;
                if ( tag != null && tag.Formula != null )
                {
                    tag.IsChange = false;
                    tag.Formula.OnChangeValFormTI -= this.FormulaOnOnChange;
                }
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

            var handlerCategory = CategoryEvent;
            if ( handlerCategory != null )
                handlerCategory( treeNode.Category );
        }
        /// <summary>
        /// Подписка тэгов на обновление
        /// </summary>
        internal void Subscribes( )
        {
            HMI_Settings.HmiTagsSubScribes( subscribes, HMI_Settings.SubscribeAction.Subscribe );
        }
        /// <summary>
        /// Отписка тэгов от обновления
        /// </summary>
        internal void UnSubscribe( )
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
        /// <summary>
        /// Отписка тэгов от обновлления и очистка данных
        /// </summary>
        internal void UnSubscribeAndClear( )
        {
            // отписались и обнулились
            HMI_Settings.HmiTagsSubScribes( subscribes, HMI_Settings.SubscribeAction.UnSubscribeAndClear );
            foreach ( DataGridViewRow row in this.dataGridView1.Rows )
                row.Cells[1].Value = DataGridRowClear( (TagDescription)row.Tag );            
        }
        /// <summary>
        /// Переход в режим задания значений тэгам
        /// </summary>
        /// <param name="checked">Режим задания</param>
        internal void ReadWriteChecked( bool @checked )
        {
            if ( @checked )
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
        /// <summary>
        /// Получение источников измененных тэгов преставления
        /// </summary>
        /// <returns>Список источноков</returns>
        internal List<ITag> GetChangedTags( )
        {
            return ( from DataGridViewRow row in this.dataGridView1.Rows
                     select row.Tag as TagDescription
                     into tag where tag != null && tag.Source != null && tag.IsChange select tag.Source ).ToList( );
        }
        /// <summary>
        /// Сброс модификаторов ручного изменения значений
        /// </summary>
        internal void ResetStatusModify( )
        {
            foreach ( DataGridViewRow row in this.dataGridView1.Rows )
            {
                var tagDesc = row.Tag as TagDescription;
                if ( tagDesc != null ) tagDesc.IsChange = false;
            }
        }
        /// <summary>
        /// Печать данных
        /// </summary>
        public void Print( )
        {
            var node = treeView1.SelectedNode as TreeNodeDescription;
            if ( node == null )
            {
                MessageBox.Show( "Не выбрана группа.\nПечать не возможна", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error );
                return;
            }

            collector.Print( (GroupDescription)node.Tag );
        }

        /// <summary>
        /// Задание значения по умолчанию тэгу при очистке
        /// </summary>
        /// <param name="description">тэг</param>
        /// <returns>Значение</returns>
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
        /// <summary>
        /// Создание строки таблицы
        /// </summary>
        /// <param name="tagDescription">Тэг</param>
        /// <returns>Строка таблицы</returns>
        private static DataGridViewRow CreateDataRow( TagDescription tagDescription )
        {
            var newRow = new DataGridViewRow { Tag = tagDescription };
            newRow.Cells.Add( new DataGridViewTextBoxCell { Value = tagDescription.Name } );

            switch ( tagDescription.Source.TypeOfTagHMI )
            {
                case TypeOfTag.Combo:
                    var cell = new DataGridViewComboBoxCell( );
                    newRow.Cells.Add( cell );
                    foreach ( var value in tagDescription.Source.SlEnumsParty )
                        cell.Items.Add( value.Value );
                    cell.Value = tagDescription.Source.ValueAsString;
                    newRow.Cells.Add( new DataGridViewTextBoxCell { Value = tagDescription.Uom } );
                    break;
                case TypeOfTag.Analog:
                case TypeOfTag.Discret:
                    newRow.Cells.Add( new DataGridViewTextBoxCell
                    {
                        Value = ( string.IsNullOrEmpty( tagDescription.Source.ValueAsString ) ) ? "0" : tagDescription.Source.ValueAsString
                    } );
                    newRow.Cells.Add( new DataGridViewTextBoxCell { Value = tagDescription.Uom } );
                    break;
                case TypeOfTag.DateTime:
                    newRow.Cells.Add( new DataGridViewTextBoxCell
                    {
                        Value = ( string.IsNullOrEmpty( tagDescription.Source.ValueAsString ) )
                                    ? DateTime.MinValue.ToString( CultureInfo.InvariantCulture )
                                    : tagDescription.Source.ValueAsString
                    } );
                    newRow.Cells.Add( new DataGridViewTextBoxCell { Value = tagDescription.Uom } );
                    break;
                default:
                    newRow.Cells.Add( new DataGridViewTextBoxCell { Value = tagDescription.Source.ValueAsString } );
                    newRow.Cells.Add( new DataGridViewTextBoxCell { Value = tagDescription.Uom } );
                    break;
            }
            return newRow;
        }
        /// <summary>
        /// Создание узлов дерева
        /// </summary>
        /// <param name="groupDescriptions">Коллекция групп</param>
        /// <returns>Коллекция узлов дерева</returns>
        private static IEnumerable<TreeNode> CreateGroupNodes( IEnumerable<GroupDescription> groupDescriptions )
        {
            if ( groupDescriptions == null || !groupDescriptions.Any( ) ) return null;

            var nodeList = new List<TreeNode>( );
            foreach ( var groupDescription in groupDescriptions )
            {
                var node = new TreeNodeDescription( groupDescription );
                
                var subNodes = CreateGroupNodes( groupDescription.Groups );
                if ( subNodes != null ) node.Nodes.AddRange( subNodes.ToArray( ) );
                
                var tags = CreateTagNodes( groupDescription.Tags );
                if ( tags != null ) node.Nodes.AddRange( tags.ToArray( ) );

                nodeList.Add( node );
            }

            return nodeList;
        }
        /// <summary>
        /// Создание узлов дерева
        /// </summary>
        /// <param name="tagDescriptions">Коллекция тэгов</param>
        /// <returns>Коллекция узлов дерева</returns>
        private static IEnumerable<TreeNode> CreateTagNodes( IEnumerable<TagDescription> tagDescriptions )
        {
            if ( tagDescriptions == null || !tagDescriptions.Any( ) ) return null;

            return tagDescriptions.Select( tag => new TreeNodeDescription( tag, TagDescription.NodeTitle( tag ) ) ).ToList( );
        }
    }
}
