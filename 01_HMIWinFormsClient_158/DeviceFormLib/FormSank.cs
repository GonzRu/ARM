using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using InterfaceLibrary;
using HMI_MT_Settings;
using DeviceFormLib.BlockTabs;

namespace DeviceFormLib
{
    public partial class FormSank : Form, IDeviceForm
    {
        private TabPage tpCurrent;
        private readonly frmEngine frmengine;
        private readonly UInt32 uniDev = 0xffffffff;
        private readonly ITag tag;
        
        public FormSank( UInt32 unids, UInt32 unidev )
        {
            InitializeComponent();
            try
            {
                uniDev = unidev;
                frmengine = new frmEngine( unids, unidev, this );

                tabControl.Controls.Add( new EventBlockTabPage( unidev ) );
                tabControl.Controls.Add( new InformationTabPage( unidev ) );
                tabControl.Controls.Add( new DataBaseFilesLibrary.TabPageDbFile( uniDev ) );

                listView1.Tag = string.Empty;
                tag = frmengine.GetTag( "2000000" );
                if ( tag != null )
                    tag.OnChangeVar += TagOnOnChangeVar;
            }
            catch ( Exception ex )
            {
                TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG( ex );
            }
        }
        /// <summary>
        /// Открытие формы
        /// </summary>
        private void FormBmrzDeviceLoad( object sender, EventArgs e )
        {
            try
            {
                frmengine.InitFrm( this, tabControl, null );
                InitInterfaceElementsClick();
            }
            catch ( Exception ex )
            {
                TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG( ex );
            }
        }
        /// <summary>
        /// Закрытие формы
        /// </summary>
        private void FormBmrzDeviceFormClosing( object sender, FormClosingEventArgs e )
        {
            // отписываемся от тегов
            HMI_Settings.HMIControlsTagReNew( tpCurrent, HMI_Settings.SubscribeAction.UnSubscribe );

            if ( tag != null )
                tag.OnChangeVar -= TagOnOnChangeVar;
        }
        public void CreateDeviceForm() { }
        public void InitInterfaceElementsClick()
        {
            this.tpCurrentInfo.Enter += this.TpCurrentInfoEnter;
        }
        public void ActivateTabPage( string typetabpage ) { }
        public void reqAvar_OnReqExecuted( IRequestData req ) { }

        private void TpCurrentInfoEnter( object sender, EventArgs e ) { tpCurrent = HMI_Settings.SetTagsSubscribe4TPCurrent( tpCurrentInfo ); }
        private void TagOnOnChangeVar( Tuple<string, byte[], DateTime, VarQualityNewDS> @var )
        {
            try
            {
                // проверка совпадения колонок
                var isColumnsChanged = CheckColumns( this.listView1.Tag.ToString(), @var.Item1 );
                // проверка совпадения данных
                var isDataChanged = CheckData( this.listView1.Tag.ToString(), @var.Item1 );

                // построение колонок
                if ( isColumnsChanged )
                {
                    listView1.Columns.Clear();
                    listView1.Columns.AddRange( GetColumns( @var.Item1 ) );
                    
                    foreach ( ColumnHeader header in listView1.Columns )
                        header.AutoResize( ColumnHeaderAutoResizeStyle.HeaderSize );
                }

                // если данные не менялись, тогда уходим
                if ( !isDataChanged )
                    return;

                // построение данных
                listView1.Items.Clear();
                listView1.Items.AddRange( GetData( @var.Item1 ) );

                // запоминаем новые данные
                this.listView1.Tag = @var.Item1;
            }
            catch
            {
                throw new ArgumentException( "Ошибка разбора данных при построении таблицы" );
            }
        }
        /// <summary>
        /// Выбор вкладки
        /// </summary>
        private void TabControlSelected( object sender, TabControlEventArgs e )
        {
            HMI_Settings.HMIControlsTagReNew( tpCurrent, HMI_Settings.SubscribeAction.UnSubscribe );
            tpCurrent.Tag = false; // признак отписки от тегов для данной TabPage
            tpCurrent = e.TabPage;  // запомним новую текущую вкладку
        }

        /// <summary>
        /// Проверка совпадения имен колонок
        /// </summary>
        /// <param name="oldValues">Строка с данными</param>
        /// <param name="newValues">Новая строка с данными</param>
        /// <returns>true - если данные не совпадают</returns>
        private static Boolean CheckColumns( string oldValues, string newValues )
        {
            var oldRows = oldValues.Split( new[] { '@' }, StringSplitOptions.RemoveEmptyEntries ); // получаем массив старых значений
            var newRows = newValues.Split( new[] { '@' }, StringSplitOptions.RemoveEmptyEntries ); // получаем массив новых значений

            if ( oldRows.Count() != newRows.Count() ) // сравниваем кол-во строк
                return true;

            return oldRows.ElementAt( 0 ) != newRows.ElementAt( 0 ); // сравниваем имена колонок
        }
        /// <summary>
        /// Проверка совпадения данных
        /// </summary>
        /// <param name="oldValues">Строка с данными</param>
        /// <param name="newValues">Новая строка с данными</param>
        /// <returns>true - если данные не совпадают</returns>
        private static Boolean CheckData(  string oldValues, string newValues  )
        {
            var oldRows = oldValues.Split( new[] { '@' }, StringSplitOptions.RemoveEmptyEntries ); // получаем массив старых значений
            var newRows = newValues.Split( new[] { '@' }, StringSplitOptions.RemoveEmptyEntries ); // получаем массив новых значений

            if ( oldRows.Count() != newRows.Count() ) // сравниваем кол-во строк
                return true;

            for ( var i = 0; i < oldRows.Count(); i++ )
                if ( oldRows[i] != newRows[i] )
                    return true;

            return false;
        }
        /// <summary>
        /// Получение массива колонок
        /// </summary>
        /// <param name="values">Строка с данными</param>
        /// <returns>Массив колонок</returns>
        private static ColumnHeader[] GetColumns( string values )
        {
            // берем строку с именами колонок
            var stringColumns = values.Split( new[] { '@' }, StringSplitOptions.RemoveEmptyEntries ).ElementAt( 0 );
            // берем массив имен колонок
            var columns = stringColumns.Split( new[] { ';' }, StringSplitOptions.RemoveEmptyEntries );
            // возвращаем массив колонок
            return columns.Select( column => new ColumnHeader { Text = column } ).ToArray();
        }
        /// <summary>
        /// Получение массива строк данных
        /// </summary>
        /// <param name="values">Строка с данными</param>
        /// <returns>Массив данных таблицы</returns>
        private static ListViewItem[] GetData( string values )
        {
            var list = new List<ListViewItem>();
            // берем массив со всеми данными
            var rows = values.Split( new[] { '@' }, StringSplitOptions.RemoveEmptyEntries );
            // пропускаем первую строку, т.к. там имена колонок
            for ( var i = 1; i < rows.Count(); ++i )
                // помещаем массив данных в контейнер строки и добавляем в список данных для отображения
                list.Add( new ListViewItem( rows[i].Split( new[] { ';' }, StringSplitOptions.RemoveEmptyEntries ) ) );

            // возвращаем массив данных
            return list.ToArray();
        }
        /// <summary>
        /// Идентификатор блока
        /// </summary>
        public uint Guid { get { return uniDev; } }
    }
}
