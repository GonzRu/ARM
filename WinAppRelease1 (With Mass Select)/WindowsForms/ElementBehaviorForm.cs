using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

using LibraryElements;
using FileManager;
using LibraryElements.CalculationBlocks;
using LibraryElements.Sources;
using Structure;

namespace WindowsForms
{
    public partial class ElementBehaviorForm : Form
    {
        private readonly bool isFullImagePath;
        private string openFile;
        private CalculationContext newCalculation;

        public ElementBehaviorForm()
        {
            InitializeComponent();

            listBox1.Items.AddRange( GetCalculationNames() );
            listBox1.SelectedIndex = 0;
            button1.Visible = button2.Visible = false;
            isFullImagePath = true;

            listBox1.DoubleClick += ListBox1OnDoubleClick;
            dataGridView1.CellClick += DataGridViewOnCellClick;
            dataGridView2.CellClick += DataGridViewOnCellClick;
        }
        public ElementBehaviorForm( ICalculationContext element )
        {
            InitializeComponent();

            listBox1.Items.AddRange( GetCalculationNames( element ) );
            listBox1.SelectedIndex = 0;
            menuItem2ToolStripMenuItem.Enabled = false;
            newCalculation = element.CalculationContext;

            if ( newCalculation != null )
                listBox1.SelectedItem = GetCalculationName( newCalculation.Context );

            InitData();

            listBox1.DoubleClick += ListBox1OnDoubleClick;
            dataGridView1.CellClick += DataGridViewOnCellClick;
            dataGridView2.CellClick += DataGridViewOnCellClick;
        }
        private void InitData()
        {
            if ( newCalculation == null ) return;

            dataGridView1.Rows.Clear();
            foreach ( var dataRecord in newCalculation.Context.GetTags() )
                dataGridView1.Rows.Add( CreateRow( dataRecord ) );

            dataGridView2.Rows.Clear();
            foreach ( var dataRecord in newCalculation.Context.GetOptions() )
                dataGridView2.Rows.Add( CreateRow( dataRecord ) );
        }
        private void DataGridViewOnCellClick( object sender, DataGridViewCellEventArgs eventArgs )
        {
            if ( eventArgs.ColumnIndex == 0 || eventArgs.RowIndex == -1 )
                return;

            var dataGrid = (DataGridView)sender;
            var element = (DataRecord)dataGrid.Rows[eventArgs.RowIndex].Tag;

            switch ( element.RecordType )
            {
                case DataRecord.RecordTypes.Color:
                    {
                        var cfd = new ColorDialog { Color = (Color)element.Value };
                        if ( cfd.ShowDialog() == DialogResult.OK )
                            element.Value = cfd.Color;

                        dataGrid.Rows[eventArgs.RowIndex].Cells[1].Value = element.Value;
                        dataGrid.Rows[eventArgs.RowIndex].Cells[1].Style.BackColor = (Color)element.Value;
                    }
                    break;
                case DataRecord.RecordTypes.Image:
                    {
                        var ofd = new OpenFileDialog
                        {
                            FileName = BuildFormula.FormulaBlock,
                            Filter = ProgrammExtensions.GetImageFilter(),
                            Multiselect = false
                        };
                        if (ofd.ShowDialog() != DialogResult.OK)
                            return;

                        dataGrid.Rows[eventArgs.RowIndex].Cells[1].Value = ( isFullImagePath )
                                                                               ? ofd.FileName.Substring( Environment.CurrentDirectory.Length )
                                                                               : ofd.FileName;
                    }
                    break;
            }

            //dataGrid.UpdateCellValue( eventArgs.ColumnIndex, eventArgs.RowIndex );
        }
        private void ListBox1OnDoubleClick( object sender, EventArgs eventArgs )
        {
            var context = ElementCalculation.DefineElement( TranslateForDefine( listBox1.SelectedItem.ToString() ) );
            if ( context == null )
            {
                dataGridView1.Rows.Clear();
                dataGridView2.Rows.Clear();
                newCalculation = null;
                return;
            }
            newCalculation = new CalculationContext( context );
            this.InitData();
        }
        internal CalculationContext GetNewCalculationContext()
        {
            return newCalculation;
        }

        /// <summary>
        /// Открыть
        /// </summary>
        private void MenuItem2ToolStripMenuItemClick( object sender, EventArgs e )
        {
            var ofd = new OpenFileDialog
                {
                    FileName = BuildFormula.FormulaBlock,
                    Filter = ProgrammExtensions.GetXmlFilter(),
                    Multiselect = false
                };
            if (ofd.ShowDialog() != DialogResult.OK)
                return;

            using ( var build = new BuildFormula() )
            {
                build.LoadFile( ofd.FileName );
                if ( build.Error_Status )
                {
                    MessageBox.Show( this, "Ошибка загрузки фала", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error );
                    return;
                }
                build.ParceDataFromFile();
                newCalculation = build.GetData();
                InitData();
                this.openFile = ofd.FileName;
            }
        }
        /// <summary>
        /// Сохранить
        /// </summary>
        private void MenuItem3ToolStripMenuItemClick( object sender, EventArgs e )
        {
            if ( string.IsNullOrEmpty( openFile ) )
            {
                MenuItem4ToolStripMenuItemClick( sender, e );
                return;
            }

            using ( var create = new CreateFormula() )
            {
                create.CreateCompleateTree( newCalculation );
                create.SaveFile( openFile );
            }
            MessageBox.Show( this, "Запись файла завершена", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information );
        }
        /// <summary>
        /// Сохранить как
        /// </summary>
        private void MenuItem4ToolStripMenuItemClick( object sender, EventArgs e )
        {
            var sfd = new SaveFileDialog
                {
                    FileName = BuildFormula.FormulaBlock,
                    Filter = ProgrammExtensions.GetXmlFilter(),
                    InitialDirectory = Environment.CurrentDirectory,
                };

            if ( sfd.ShowDialog() != DialogResult.OK )
            {
                MessageBox.Show( this, "Запись файла отменена", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information );
                return;
            }

            using ( var create = new CreateFormula() )
            {
                create.CreateCompleateTree( newCalculation );
                create.SaveFile( sfd.FileName );
            }
            MessageBox.Show( this, "Запись файла завершена", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information );
        }
        /// <summary>
        /// Применить
        /// </summary>
        private void Button1Click( object sender, EventArgs e )
        {
            foreach ( DataGridViewRow row in dataGridView1.Rows ) SetValues( row );
            foreach ( DataGridViewRow row in dataGridView2.Rows ) SetValues( row );
        }

        /// <summary>
        /// Создание строки
        /// </summary>
        /// <param name="record">Данные</param>
        /// <returns>Строка таблицы</returns>
        private static DataGridViewRow CreateRow( DataRecord record )
        {
            var newRow = new DataGridViewRow { Tag = record };
            newRow.Cells.Add( new DataGridViewTextBoxCell { Value = record.Name } );
            newRow.Cells[0].ReadOnly = true;

            switch ( record.RecordType )
            {
                case DataRecord.RecordTypes.Tag:
                    {
                        var signalRecord = (SignalMatchRecord)record;
                        newRow.Cells.Add( new DataGridViewTextBoxCell { Value = signalRecord.DsGuid } );
                        newRow.Cells.Add( new DataGridViewTextBoxCell { Value = signalRecord.DevGuid } );
                        newRow.Cells.Add( new DataGridViewTextBoxCell { Value = signalRecord.TagGuid } );
                    }
                    break;
                case DataRecord.RecordTypes.Color: newRow.Cells.Add( new DataGridViewButtonCell { Value = record.Value, Style = { BackColor = (Color)record.Value } } ); break;
                case DataRecord.RecordTypes.Image: newRow.Cells.Add( new DataGridViewButtonCell { Value = ( record.Value != null ) ? ( (ImageData)record.Value ).Path : string.Empty } ); break;
                case DataRecord.RecordTypes.StateProtocol:
                    {
                        var cell = new DataGridViewComboBoxCell
                            {
                                Value = record.Value,
                                DataSource = new[] { ProtocolStatus.Bad, ProtocolStatus.Good },
                                ValueType = typeof ( ProtocolStatus )
                            };
                        newRow.Cells.Add( cell );
                    }
                    break;
                case DataRecord.RecordTypes.Rotate:
                    {
                        var cell = new DataGridViewComboBoxCell
                            {
                                Value = record.Value,
                                DataSource = new[] { DrawRotate.Up, DrawRotate.Down, DrawRotate.Left, DrawRotate.Right },
                                ValueType = typeof ( DrawRotate )
                            };
                        newRow.Cells.Add( cell );
                    }
                    break;
                case DataRecord.RecordTypes.Boolean: newRow.Cells.Add( new DataGridViewCheckBoxCell( false ) { Value = record.Value } ); break;
                default: newRow.Cells.Add( new DataGridViewTextBoxCell { Value = record.Value } ); break;
            }

            return newRow;
        }
        /// <summary>
        /// Запись значений
        /// </summary>
        /// <param name="row">Строка таблицы</param>
        private static void SetValues( DataGridViewRow row )
        {
            var element = (DataRecord)row.Tag;
            switch ( element.RecordType )
            {
                case DataRecord.RecordTypes.Tag:
                    {
                        var signalRecord = (SignalMatchRecord)row.Tag;
                        signalRecord.DsGuid = Convert.ToUInt32( row.Cells[1].Value );
                        signalRecord.DevGuid = Convert.ToUInt32( row.Cells[2].Value );
                        signalRecord.TagGuid = Convert.ToUInt32( row.Cells[3].Value );
                    }
                    break;
                case DataRecord.RecordTypes.Image:
                    {
                        try
                        {
                            if ( string.IsNullOrEmpty( row.Cells[1].Value.ToString() ) )
                                break;

                            element.Value = new ImageData( WorkFile.ReadImageFile( row.Cells[1].Value.ToString() ),
                                                           row.Cells[1].Value.ToString() );
                        }
                        catch ( Exception )
                        {
                            MessageBox.Show( "Ошибка загрузки фала изображения", "Ошибка", MessageBoxButtons.OK,
                                             MessageBoxIcon.Error );
                        }
                    }
                    break;
                default:
                    element.Value = row.Cells[1].Value;
                    break;
            }
        }
        /// <summary>
        /// Получить имена вариантов расчетных данных
        /// </summary>
        /// <returns>Массив имен</returns>
        private static Object[] GetCalculationNames( ICalculationContext element )
        {
            var list = new List<Object> { "Отсудствует" };

            if ( element is DynamicElement )
            {
                list.Add( "BMRZ данные" );
                list.Add( "Sirius данные" );
                list.Add( "ЭКРА данные");
                list.Add( "Бреслер данные");
                list.Add( "Данные ключа" );
                list.Add( "Данные трансформатора" );
                list.Add( "Данные изображения" );
                list.Add( "Данные сигнального блока" );
                list.Add( "Данные Uso блока" );
                list.Add( "Данные текстового сигнала" );
            }
            if ( element is Trunk )
                list.Add( "Данные ключа цвета" );

            return list.ToArray();
        }
        /// <summary>
        /// Получить имена вариантов
        /// </summary>
        /// <returns>Массив имен</returns>
        private static Object[] GetCalculationNames()
        {
            return new object[]
                {
                    "Отсудствует", "BMRZ данные", "Sirius данные", "Данные ключа", "Данные трансформатора",
                    "Данные изображения", "Данные сигнального блока", "Данные Uso блока", "Данные текстового сигнала",
                    "Данные ключа цвета"
                };
        }
        /// <summary>
        /// Получить имена вариантов расчетных данных
        /// </summary>
        /// <returns>Имя расчетных данных</returns>
        private static Object GetCalculationName( ElementCalculation calculation )
        {
            var name = "Отсудствует";
            if ( calculation is BmrzCalculation ) name = "BMRZ данные";
            if ( calculation is SiriusCalculation ) name = "Sirius данные";
            if ( calculation is EkraCalculation) name = "ЭКРА данные";
            if ( calculation is BreslerCalculation ) name = "Бреслер данные";
            if ( calculation is KeyCalculation ) name = "Данные ключа";
            if ( calculation is BlockSignalCalculation ) name = "Данные сигнального блока";
            if ( calculation is TransformatorCalculation ) name = "Данные трансформатора";
            if ( calculation is TrunkCalculation ) name = "Данные ключа цвета";
            if ( calculation is ImageCalculation ) name = "Данные изображения";
            if ( calculation is UsoSignalCalculation ) name = "Данные Uso блока";
            if ( calculation is TextSignalCalculation ) name = "Данные текстового сигнала";
            return name;
        }
        /// <summary>
        /// Перевод визуального имени в имя для определения расчетных данных
        /// </summary>
        /// <param name="viewName">Визуальное имя расчетных данных</param>
        /// <returns>Внутреннее имя расчетных данных</returns>
        private static String TranslateForDefine( String viewName )
        {
            switch ( viewName )
            {
                case "BMRZ данные": return "BmrzCalculation";
                case "Sirius данные": return "SiriusCalculation";
                case "ЭКРА данные": return "EkraCalculation";
                case "Бреслер данные": return "BreslerCalculation";
                case "Данные ключа": return "KeyCalculation";
                case "Данные сигнального блока": return "BlockSignalCalculation";
                case "Данные трансформатора": return "TransformatorCalculation";
                case "Данные ключа цвета": return "TrunkCalculation";
                case "Данные изображения": return "ImageCalculation";
                case "Данные Uso блока": return "UsoSignalCalculation";
                case "Данные текстового сигнала": return "TextSignalCalculation";
                default: return viewName;
            }
        }
    }
}
