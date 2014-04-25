using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;

using Calculator;
using DataBaseLib;
using HMI_MT_Settings;
using InterfaceLibrary;

namespace HelperControlsLibrary
{
    /// <summary>
    /// Сборщик описания устройства
    /// </summary>
    public class BlockViewCollector
    {
        readonly UInt32 uniDs, uniDev;
        /// <summary>
        /// Список групп
        /// </summary>
        private readonly List<GroupDescription> groupDescriptions = new List<GroupDescription>( );

        /// <summary>
        /// Сборщик
        /// </summary>
        /// <param name="uniDs">ID датасервера</param>
        /// <param name="uniDev">ID устройства</param>
        public BlockViewCollector( UInt32 uniDs, UInt32 uniDev )
        {
            this.uniDs = uniDs;
            this.uniDev = uniDev;
        }
        /// <summary>
        /// Собирание
        /// </summary>
        /// <param name="group">Группа источник</param>
        public void Collect( IGroup group ) { if ( group.IsEnable ) this.Collect( group, groupDescriptions ); }
        /// <summary>
        /// Собирание
        /// </summary>
        /// <param name="groups">Список групп источников</param>
        public void Collect( IEnumerable<IGroup> groups ) { this.Collect( groups, groupDescriptions ); }
        /// <summary>
        /// Собирание
        /// </summary>
        /// <param name="group">Группа источник</param>
        /// <param name="descriptions">Список групп</param>
        /// <param name="category">Категория</param>
        private void Collect( IGroup group, ICollection<GroupDescription> descriptions, Category category = Category.NaN )
        {
            var description = new GroupDescription { Name = @group.NameGroup };

            var xElem = @group.GroupXElement.Attribute( "category" );
            if ( xElem != null ) description.Category = (Category)Enum.Parse( typeof( Category ), xElem.Value );
            else description.Category = category;

            Collect( group.SubGroupsList, description.Groups, description.Category );
            CollectTags( group, description.Tags, description.Category );
            descriptions.Add( description );
        }
        /// <summary>
        /// Собирание
        /// </summary>
        /// <param name="groups">Список групп источников</param>
        /// <param name="descriptions">Список групп</param>
        /// <param name="category">Категория</param>
        private void Collect( IEnumerable<IGroup> groups, ICollection<GroupDescription> descriptions, Category category = Category.NaN )
        {
            foreach ( var @group in groups.Where( @group => @group.IsEnable ) )
                this.Collect( @group, descriptions, category );
        }
        /// <summary>
        /// Собирание тэгов
        /// </summary>
        /// <param name="group">Группа источник</param>
        /// <param name="descriptions">Список групп</param>
        /// <param name="category">Категория</param>
        private void CollectTags( IGroup group, ICollection<TagDescription> descriptions, Category category = Category.NaN )
        {
            var xTags = group.GroupXElement.Element( "Tags" );
            if ( xTags == null ) return;

            foreach ( var xTagDescription in xTags.Elements( ) )
            {
                try
                {
                    var xmlAttribute = xTagDescription.Attribute( "enable" );
                    if ( xmlAttribute == null || !bool.Parse( xmlAttribute.Value ) ) continue;

                    xmlAttribute = xTagDescription.Attribute( "value" );
                    if ( xmlAttribute == null ) continue;

                    // дополнительный контроль описания тэгов на признак включения или отсудствия имени у настоящего описания тэга
                    var source = HMI_Settings.CONFIGURATION.GetLink2Tag( uniDs, uniDev, uint.Parse( xmlAttribute.Value ) );
                    if ( source == null || !source.IsEnable || string.IsNullOrEmpty( source.TagName ) ) continue;

                    var tag = new TagDescription { Source = source, Category = category };

                    var xElement = xTagDescription.Element( "gui_variables_describe" );
                    if ( xElement != null )
                    {
                        var xElem = xElement.Element( "var_title" );
                        tag.Name = ( xElem != null ) ? xElem.Value : tag.Source.TagName;
                        xElem = xElement.Element( "UOM" );
                        tag.Uom = ( xElem != null ) ? xElem.Value : tag.Source.Unit;
                    }
                    else
                    {
                        tag.Name = tag.Source.TagName;
                        tag.Uom = tag.Source.Unit;
                    }

                    xElement = xTagDescription.Element( "HMI_Format_describe" );
                    if ( xElement != null )
                    {
                        xElement = xElement.Element( "HMIPosPoint" );
                        if ( xElement != null )
                        {
                            ushort tmp;
                            ushort.TryParse( xElement.Value, out tmp );

                            var itagDim = tag.Source as ITagDim;
                            if ( itagDim != null ) itagDim.ValueDim = tmp;
                        }
                    }

                    // дополнительный контроль описания тэгов на наличие или отсудствия имени у описаного тэга
                    if ( string.IsNullOrEmpty( tag.Name ) ) continue;

                    tag.CreateFormulaTag( );
                    descriptions.Add( tag );
                }
                catch ( Exception ex )
                {
                    TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG( ex );
                }
            }
        }
        /// <summary>
        /// Коллекция групп
        /// </summary>
        public IEnumerable<GroupDescription> Groups { get { return groupDescriptions; } }
        /// <summary>
        /// Сбор списка архивных записей аварий/уставок
        /// </summary>
        /// <param name="uniDev">ID устройства</param>
        /// <param name="startDate">Начало выборки</param>
        /// <param name="endDate">Конец выборки</param>
        /// <param name="type">Тип записи</param>
        /// <returns>Список архивных записей</returns>
        public static List<DataGridViewRow> AvarUstavkyDataBase( uint uniDev, DateTime startDate, DateTime endDate, TypeBlockData4Req type )
        {
            DataBaseReq dbs = null;
            
            try
            {
                dbs = new DataBaseReq( HMI_Settings.ProviderPtkSql, "ShowDataLog2" );
                var tbd = CommonUtils.CommonUtils.GetTypeBlockData4ThisDev( type, uniDev );

                // входные параметры
                // 1. ip FC
                dbs.AddCMDParams( new DataBaseParameter( "@IP", ParameterDirection.Input, SqlDbType.BigInt, 0 ) );
                // 2. id устройства
                dbs.AddCMDParams( new DataBaseParameter( "@id", ParameterDirection.Input, SqlDbType.Int, uniDev ) );
                // 3. начальное время
                dbs.AddCMDParams( new DataBaseParameter( "@dt_start", ParameterDirection.Input, SqlDbType.DateTime, startDate ) );
                // 2. конечное время
                dbs.AddCMDParams( new DataBaseParameter( "@dt_end", ParameterDirection.Input, SqlDbType.DateTime, endDate ) );
                // 5. тип записи
                // информация по авариям
                //var tbd = CommonUtils.CommonUtils.GetTypeBlockData4ThisDev( type, uniDev );
                dbs.AddCMDParams( new DataBaseParameter( "@type", ParameterDirection.Input, SqlDbType.Int, tbd ) );
                // 6. ид записи журнала
                dbs.AddCMDParams( new DataBaseParameter( "@id_record", ParameterDirection.Input, SqlDbType.Int, 0 ) );
                // извлекаем данные по авариям
                var dt = dbs.GetTableAsResultCMD( );

                var list = new List<DataGridViewRow>( );
                for ( var curRow = 0; curRow < dt.Rows.Count; curRow++ )
                {
                    var t = (DateTime)dt.Rows[curRow]["TimeBlock"];
                    var dgRow = new DataGridViewRow { Tag = dt.Rows[curRow]["ID"] };
                    dgRow.Cells.Add( new DataGridViewTextBoxCell { Value = CommonUtils.CommonUtils.GetTimeInMTRACustomFormat( t ) } );
                    dgRow.Cells.Add( new DataGridViewTextBoxCell { Value = dt.Rows[curRow][/*"Arg1"*/"BlockID"].ToString() } ); // корректировка для 304
                    list.Add( dgRow );
                }
                return list;
            }
            catch( Exception exception )
            {
                TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG( exception );
            }
            finally
            {
                if ( dbs != null ) 
                    dbs.CloseConnection( );
            }

            return null;
        }
        /// <summary>
        /// Печать данных
        /// </summary>
        /// <param name="groupDescription">Группа</param>
        public void Print( GroupDescription groupDescription )
        {
            var dev = HMI_Settings.CONFIGURATION.GetLink2Device( uniDs, uniDev );
            if ( dev == null )
            {
                TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG( TraceEventType.Warning, (int)uniDev, "GetLink2Device вернул Null" );
                MessageBox.Show( "Ошибка поиска устройста.\nПечать не возможна", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error );
                return;
            }

            string messageText;
            try
            {
                var printStatus = ReportLibrary.ReportHelper.Print( groupDescription, dev.Description, dev.Name,
                                                                    dev.TypeName, dev.Version, HMI_Settings.UserName );

                messageText = printStatus ? "Запись отчета завершена." : "Запись отчета не выполнена."; // Вкладка не поддерживается системой печати.
            }
            catch ( Exception )
            {
                messageText = "Ошибка при формировании отчета.";
            }

            MessageBox.Show( messageText, "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information );
        }
    }

    /// <summary>
    /// Категории групп описания сигналов
    /// </summary>
    public enum Category
    {
        /// <summary>
        /// Не определено
        /// </summary>
        NaN,
        /// <summary>
        /// Аварии
        /// </summary>
        Crush,
        /// <summary>
        /// Уставки
        /// </summary>
        Ustavki,
        /// <summary>
        /// Максметр
        /// </summary>
        MaxMeter,
        /// <summary>
        /// Накопитель
        /// </summary>
        StorageDevice,
        /// <summary>
        /// Кнопки действия
        /// </summary>
        Buttons
    }
    /// <summary>
    /// Базовый класс описания
    /// </summary>
    public abstract class BaseDescription
    {
        /// <summary>
        /// Имя
        /// </summary>
        public String Name;
        /// <summary>
        /// Категория
        /// </summary>
        public Category Category;
    }
    /// <summary>
    /// Описание тэга
    /// </summary>
    public class TagDescription : BaseDescription
    {
        /// <summary>
        /// Размерность
        /// </summary>
        public String Uom;
        /// <summary>
        /// Источник
        /// </summary>
        public ITag Source;
        /// <summary>
        /// Формула привязки получения данных
        /// </summary>
        public FormulaEvalNds Formula;
        /// <summary>
        /// Призная изменяемости тега
        /// </summary>
        public Boolean IsChange
        {
            get { return this.Source != null && this.Source.IsHMIChange; }
            set
            {
                if ( Source == null ) return;
                Source.IsHMIChange = value;
            }
        }

        /// <summary>
        /// Создание формулы привязки
        /// </summary>
        public void CreateFormulaTag( )
        {
            if ( this.Source == null ) throw new ArgumentNullException( string.Format( "Нет сигнала: {0}", Name ) );

            var dsGuid = this.Source.Device.UniDS_GUID;
            var devGuid = this.Source.Device.UniObjectGUID;
            var tagGuid = this.Source.TagGUID;

            if ( this.Source.TypeOfTagHMI == TypeOfTag.NaN )
            {
                throw new ArgumentNullException(
                    string.Format(
                        "\n*********** Нет сигнала: \"{0}\" DS: {1} Device: {2} GUID: {3} Тип: {4} ***********\n",
                        Name, dsGuid, devGuid, tagGuid, this.Source.TypeOfTagHMI ), "Tag is not exist" );
            }

            this.Formula = new FormulaEvalNds( HMI_Settings.CONFIGURATION,
                                               string.Format( "{0}.{1}.{2}", dsGuid, devGuid, tagGuid ), Name, this.Uom );
        }
        /// <summary>
        /// Результирующая строка описания тега
        /// </summary>
        public String Result
        {
            get
            {
                return ( this.Source == null )
                           ? "0.0.0"
                           : string.Format( "{0}.{1}.{2}", this.Source.Device.UniDS_GUID,
                                            this.Source.Device.UniObjectGUID, this.Source.TagGUID );
            }
        }
        /// <summary>
        /// Значение тэга
        /// </summary>
        public String Value { get { return ( Source == null ) ? "NaN" : Source.ValueAsString; } }
        /// <summary>
        /// Описание тэга для древовидного представления
        /// </summary>
        /// <param name="description">Тэг</param>
        /// <returns>Составное имя ветви TreeNode</returns>
        public static String NodeTitle( TagDescription description )
        {
            return description.Name;
            
            // планировалось сделать дублирующий вывод данных в дерево
            //return ( string.IsNullOrEmpty( description.Uom ) )
            //           ? string.Format( "{0}:   {1}", description.Name, description.Source.ValueAsString )
            //           : string.Format( "{0}:   {1} ({2})", description.Name, description.Source.ValueAsString, description.Uom );
        }
    }
    /// <summary>
    /// Описание группы
    /// </summary>
    public class GroupDescription : BaseDescription
    {
        /// <summary>
        /// Список тэгов
        /// </summary>
        public readonly List<TagDescription> Tags = new List<TagDescription>();
        /// <summary>
        /// Список групп
        /// </summary>
        public readonly List<GroupDescription> Groups = new List<GroupDescription>();
    }
}
