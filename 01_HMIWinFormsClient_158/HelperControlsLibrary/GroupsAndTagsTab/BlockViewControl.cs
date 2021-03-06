﻿using System;
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
        #region Class TreeNodeDescription
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
        #endregion

        #region CategoryEvent
        /// <summary>
        /// Делегат выдачи категории
        /// </summary>
        /// <param name="category">Категория</param>
        public delegate void CategoryDelagete( Category category );
        /// <summary>
        /// Событие выдачи категории
        /// </summary>
        public event CategoryDelagete CategoryEvent;
        #endregion

        #region private
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
        #endregion

        #region Constructor
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
        #endregion

        #region Private-Metods

        #region Методы иницилизации контрола

        /// <summary>
        /// Загрузка контрола
        /// </summary>
        private void BlockViewControlLoad(object sender, EventArgs e)
        {
            try
            {
                DebugStatistics.WindowStatistics.AddStatistic("Инициализация сбора данных блока.");

                var device = HMI_Settings.CONFIGURATION.GetLink2Device(this.uniDs, this.uniDev);
                if (device == null)
                    throw new Exception(
                        string.Format("FrmEngine: Нет связанного устройства с данной формой unids = {0}; unidev = {1}",
                                       this.uniDs.ToString(CultureInfo.InvariantCulture),
                                       this.uniDev.ToString(CultureInfo.InvariantCulture)));

                collector.Collect(device.GetGroupHierarchy());

                DebugStatistics.WindowStatistics.AddStatistic("Инициализация сбора данных блока завершена.");

                DebugStatistics.WindowStatistics.AddStatistic("Инициализация построения дерева данных.");

                var nodes = CreateGroupNodes(collector.Groups);
                if (nodes != null)
                    groupsTreeView.Nodes.AddRange(nodes.ToArray());

                DebugStatistics.WindowStatistics.AddStatistic("Инициализация построения дерева данных завершена.");
            }
            catch (Exception ex)
            {
                TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG(ex);
            }
        }

        #endregion

        #region Методы работы с таблицей

        /// <summary>
        /// Наполнение таблицы данными
        /// </summary>
        /// <param name="node">Узел дерева</param>
        private void InsertToTable(TreeNode node)
        {
            var groupDescription = node.Tag as GroupDescription;
            if (groupDescription != null)
                foreach (TreeNode subNode in node.Nodes)
                {
                    if (subNode.Tag is GroupDescription)
                    {
                        var cell = new DataGridViewTextBoxCell()
                        {
                            Value = subNode.Text,
                        };

                        var row = new DataGridViewRow();
                        row.Cells.Add(cell);
                        row.DefaultCellStyle = new DataGridViewCellStyle() { BackColor = System.Drawing.Color.Green };

                        tagsTableDataGridView.Rows.Add(row);
                    }
                    this.InsertToTable(subNode);
                }

            var tagDescriptions = groupDescription.Tags;
            if (tagDescriptions == null) return;

            foreach (var tagDescription in tagDescriptions)
            {
                this.tagsTableDataGridView.Rows.Add(CreateDataRow(tagDescription));
                this.subscribes.Add(tagDescription.Source);
            }

            tagsTableDataGridView.ClearSelection();
        }

        /// <summary>
        /// Событие выбора узла дерева
        /// </summary>
        private void TreeView1Click(object sender, EventArgs e)
        {
            var tree = (TreeView)sender;

            ClearAndUnsubscribeTagsTable();

            var treeNode = (TreeNodeDescription)tree.GetNodeAt(((MouseEventArgs)e).Location);
            this.InsertToTable(treeNode);
            this.tagsTableDataGridView.Columns[1].ReadOnly = true;

            SubscribeTagsInTagsTable();

            tagsTableDataGridView.Columns[3].Visible = false;

            var handlerCategory = CategoryEvent;
            if (handlerCategory != null)
                handlerCategory(treeNode.Category);
        }

        /// <summary>
        /// Редактирование ячейки в режиме задания уставок
        /// </summary>
        private void DataGridRowCellEndEdit(object sender, DataGridViewCellEventArgs args)
        {
            var gridRow = this.tagsTableDataGridView.Rows[args.RowIndex];
            var tagDesc = gridRow.Tag as TagDescription;
            if (tagDesc == null || tagDesc.Source == null) return;

            try
            {
                switch (tagDesc.Source.TypeOfTagHMI)
                {
                    case TypeOfTag.Combo:
                        {
                            var cbCell =
                                gridRow.Cells[3] as DataGridViewComboBoxCell;
                            if (cbCell == null)
                                throw new NullReferenceException("ComboBox Cell is Null");

                            foreach (var part in tagDesc.Source.SlEnumsParty)
                                if (part.Value.Equals(cbCell.Value))
                                {
                                    var memX = BitConverter.GetBytes(Convert.ToSingle(part.Key));
                                    tagDesc.Source.SetValue(memX, DateTime.Now, VarQualityNewDs.vqGood);
                                    break;
                                }
                        }
                        break;
                    case TypeOfTag.Analog:
                        {
                            try
                            {
                                var memX = BitConverter.GetBytes(Convert.ToSingle(gridRow.Cells[3].Value));
                                tagDesc.Source.SetValue(memX, DateTime.Now, VarQualityNewDs.vqGood);
                            }
                            catch (Exception)
                            {
                                MessageBox.Show("Введенное значение некорректно", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                gridRow.Cells[3].Value = gridRow.Cells[1].Value;
                                return;
                            }                                                        
                        }
                        break;
                    case TypeOfTag.Discret:
                        {
                            Boolean? value = null;
                            if (gridRow.Cells[3].Value.Equals("1")) value = true;
                            if (gridRow.Cells[3].Value.Equals("0")) value = false;

                            if (value == null)
                            {
                                gridRow.Cells[3].Value = gridRow.Cells[1].Value;
                                MessageBox.Show("Неверное значение для данного сигнала.", "Ошибка!",
                                                MessageBoxButtons.OK, MessageBoxIcon.Error);

                                return;
                            }

                            var memX = BitConverter.GetBytes(Convert.ToBoolean(value));
                            tagDesc.Source.SetValue(memX, DateTime.Now, VarQualityNewDs.vqGood);
                        }
                        break;
                    case TypeOfTag.DateTime:
                        {

                            var memX = BitConverter.GetBytes(Convert.ToInt64(DateTime.Parse(gridRow.Cells[3].Value.ToString())));
                            tagDesc.Source.SetValue(memX, DateTime.Now, VarQualityNewDs.vqGood);
                        }
                        break;
                    default:
                        {
                            var memX = System.Text.Encoding.Default.GetBytes(gridRow.Cells[3].Value.ToString());
                            tagDesc.Source.SetValue(memX, DateTime.Now, VarQualityNewDs.vqGood);
                        }
                        break;
                }

                tagDesc.IsChange = true;
            }
            catch (Exception exception)
            {
                TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG(exception);
            }
        }

        /// <summary>
        /// Событие изменения значений данных тэга
        /// </summary>
        /// <param name="format">Полная строка представления тэга</param>
        /// <param name="value">Значение</param>
        /// <param name="type">Тип тэга</param>
        /// <returns>Признак изменения тэга</returns>
        private bool FormulaOnOnChange(string format, object value, TypeOfTag type)
        {
            foreach (DataGridViewRow row in this.tagsTableDataGridView.Rows)
            {
                var tag = (TagDescription)row.Tag;
                if (tag == null || !tag.Result.Equals(format) || row.Cells[1].Value.Equals(value)) continue;

                if (tag.Source.TypeOfTagHMI == TypeOfTag.Discret)
                    row.Cells[1].Value = value;
                else
                    row.Cells[1].Value = tag.Source.ValueAsString;

                return true;
            }
            return false;
        }

        #endregion

        #region Методы отписки/подписки

        /// <summary>
        /// Отписывается от обновления таблицы и получения обновлений от роутера + очистка таблицы
        /// </summary>
        private void ClearAndUnsubscribeTagsTable()
        {
            foreach (DataGridViewRow row in this.tagsTableDataGridView.Rows)
            {
                var tag = row.Tag as TagDescription;
                if (tag != null && tag.Formula != null)
                {
                    tag.IsChange = false;
                    tag.Formula.OnChangeValFormTI -= this.FormulaOnOnChange;
                }
            }
            HMI_Settings.HmiTagsSubScribes(subscribes, HMI_Settings.SubscribeAction.UnSubscribe);
            this.tagsTableDataGridView.Rows.Clear();
            this.subscribes.Clear();
        }

        /// <summary>
        /// Подписываеися на получение обновлений от роутера + обновление таблицы
        /// </summary>
        private void SubscribeTagsInTagsTable()
        {
            foreach (DataGridViewRow row in this.tagsTableDataGridView.Rows)
            {
                var tag = row.Tag as TagDescription;
                if (tag != null && tag.Formula != null)
                    tag.Formula.OnChangeValFormTI += this.FormulaOnOnChange;
            }
            HMI_Settings.HmiTagsSubScribes(subscribes, HMI_Settings.SubscribeAction.Subscribe);
        }

        /// <summary>
        /// Подписываемся на события для обновления значений таблицы
        /// </summary>
        private void SubscribeToUpdateGrid()
        {
            foreach (DataGridViewRow row in this.tagsTableDataGridView.Rows)
            {
                var tag = row.Tag as TagDescription;
                if (tag != null && tag.Formula != null)
                    tag.Formula.OnChangeValFormTI += this.FormulaOnOnChange;
            }
        }


        /// <summary>
        /// Отписываемя от обновления таблицы
        /// </summary>
        private void UnsubscribeFromUpdateGrid()
        {
            foreach (DataGridViewRow row in this.tagsTableDataGridView.Rows)
            {
                var tag = row.Tag as TagDescription;
                if (tag != null && tag.Formula != null)
                {
                    tag.IsChange = false;
                    tag.Formula.OnChangeValFormTI -= this.FormulaOnOnChange;
                }
            }
        }

        #endregion

        #endregion

        #region Public & Internal Metods

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
            foreach ( DataGridViewRow row in this.tagsTableDataGridView.Rows )
            {
                var tag = row.Tag as TagDescription;
                if ( tag != null && tag.Formula != null )
                    tag.Formula.OnChangeValFormTI -= this.FormulaOnOnChange;
            }
            this.tagsTableDataGridView.Rows.Clear( );
            HMI_Settings.HmiTagsSubScribes( subscribes, HMI_Settings.SubscribeAction.UnSubscribe );            
        }

        /// <summary>
        /// Отписка тэгов от обновлления и очистка данных
        /// </summary>
        internal void UnSubscribeAndClear( )
        {
            // отписались и обнулились
            HMI_Settings.HmiTagsSubScribes( subscribes, HMI_Settings.SubscribeAction.UnSubscribeAndClear );
            foreach ( DataGridViewRow row in this.tagsTableDataGridView.Rows )
               if (row.Tag != null)
                row.Cells[1].Value = DataGridRowClear( (TagDescription)row.Tag );            
        }

        #region Методы для работы с записью уставок

        /// <summary>
        /// Переход в режим задания значений тэгам
        /// </summary>
        /// <param name="checked">Режим задания</param>
        internal void ReadWriteChecked( bool @checked )
        {
            if ( @checked )
            {

                foreach (var row in tagsTableDataGridView.Rows)
                {
                    var dataGridRow = row as DataGridViewRow;
                    TagDescription tagDescription;

                    if (dataGridRow.Tag == null)
                        continue;

                    // ReadOnly уставки
                    tagDescription = dataGridRow.Tag as TagDescription;
                    if (tagDescription.Source.AccessToValue == "r")
                    {
                        dataGridRow.Cells[3].ReadOnly = true;
                        continue;
                    }                    

                    var c = dataGridRow.Cells[1];

                    // Всплывающая подсказка
                    if (!String.IsNullOrWhiteSpace(tagDescription.Source.MinValue) && !String.IsNullOrWhiteSpace(tagDescription.Source.MaxValue))
                        c.ToolTipText = String.Format("{0} - {1}", tagDescription.Source.MinValue, tagDescription.Source.MaxValue);
                    else
                        c.ToolTipText = "Нет данных об ограничениях на значение";

                    if (c.Value == null)
                        continue;

                    var newCell = c.Clone() as DataGridViewCell;
                    newCell.Style = new DataGridViewCellStyle() { BackColor = System.Drawing.Color.LightGreen };
                    newCell.Value = c.Value;

                    dataGridRow.Cells[3] = newCell;
                }

                this.tagsTableDataGridView.Columns[3].Visible = true;

                UnsubscribeFromUpdateGrid();
                HMI_Settings.HmiTagsSubScribes(subscribes, HMI_Settings.SubscribeAction.UnSubscribe);
            }
            else
            {
                this.tagsTableDataGridView.Columns[3].Visible = false;

                SubscribeToUpdateGrid();
                HMI_Settings.HmiTagsSubScribes(subscribes, HMI_Settings.SubscribeAction.Subscribe);
            }            
        }

        /// <summary>
        /// Получение источников измененных тэгов преставления
        /// </summary>
        /// <returns>Список источноков</returns>
        internal List<ITag> GetChangedTags( )
        {
            return ( from DataGridViewRow row in this.tagsTableDataGridView.Rows
                     select row.Tag as TagDescription
                     into tag where tag != null && tag.Source != null && tag.IsChange select tag.Source ).ToList( );
        }

        #endregion

        /// <summary>
        /// Сброс модификаторов ручного изменения значений
        /// </summary>
        internal void ResetStatusModify( )
        {
            foreach ( DataGridViewRow row in this.tagsTableDataGridView.Rows )
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
            var node = groupsTreeView.SelectedNode as TreeNodeDescription;
            if ( node == null )
            {
                MessageBox.Show( "Не выбрана группа.\nПечать не возможна", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error );
                return;
            }

            collector.Print( (GroupDescription)node.Tag );
        }

        /// <summary>
        /// Show tags from Group with Category=groupCategory
        /// </summary>
        public void ActiveAndShowTreeGroupWithCategory(Category groupCategory)
        {
            //Поиск группы и раскрытие ее
            TreeNode treeNode = null;
            foreach (TreeNode node in groupsTreeView.Nodes)
            {
                var nodeDecription = (TreeNodeDescription)node;
                if (nodeDecription.Category == groupCategory)
                {
                    treeNode = node;

                    ClearAndUnsubscribeTagsTable();

                    InsertToTable(treeNode);
                    SubscribeTagsInTagsTable();

                    if (!treeNode.IsExpanded)
                        treeNode.Expand();
                    groupsTreeView.SelectedNode = treeNode;

                    var handlerCategory = CategoryEvent;
                    if (handlerCategory != null)
                        handlerCategory(nodeDecription.Category);

                    return;
                }
            }
        }
        #endregion

        #region Private-Static-Metods
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
                    foreach (var value in tagDescription.Source.SlEnumsParty)
                        cell.Items.Add(value.Value);

                    cell.Value = tagDescription.Source.ValueAsString;

                    newRow.Cells.Add( new DataGridViewTextBoxCell { Value = tagDescription.Uom } );                    

                    break;
                case TypeOfTag.Analog:
                    var analogCell = new DataGridViewTextBoxCell();
                    analogCell.Value = (string.IsNullOrEmpty(tagDescription.Source.ValueAsString))
                                           ? "0"
                                           : tagDescription.Source.ValueAsString;
                    newRow.Cells.Add(analogCell);

                    newRow.Cells.Add(new DataGridViewTextBoxCell { Value = tagDescription.Uom });

                    break;
                case TypeOfTag.Discret:
                    var discretCell = new DataGridViewTextBoxCell();
                    discretCell.Value = (string.IsNullOrEmpty(tagDescription.Source.ValueAsString))
                                           ? "0"
                                           : tagDescription.Source.ValueAsString.Equals("True", StringComparison.OrdinalIgnoreCase) ? "1" : "0";
                    newRow.Cells.Add(discretCell);

                    newRow.Cells.Add( new DataGridViewTextBoxCell { Value = tagDescription.Uom } );

                    break;
                case TypeOfTag.DateTime:
                    var dateCell = new DataGridViewTextBoxCell();
                    dateCell.Value = (string.IsNullOrEmpty(tagDescription.Source.ValueAsString))
                                                 ? DateTime.MinValue.ToString(CultureInfo.InvariantCulture)
                                                 : tagDescription.Source.ValueAsString;
                    newRow.Cells.Add(dateCell);

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
        #endregion
    }
}
