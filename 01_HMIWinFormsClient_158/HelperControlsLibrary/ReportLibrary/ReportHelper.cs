using System;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace HelperControlsLibrary.ReportLibrary
{
    /// <summary>
    /// Интерфейс печати
    /// </summary>
    public interface IReport
    {
        /// <summary>
        /// Печать
        /// </summary>
        void Print( );
    }

    public static class ReportHelper
    {
        private const string Filter = "Текстовый файл|*.txt";

        /// <summary>
        /// Печать
        /// </summary>
        /// <param name="groupDescription">Группа описания</param>
        /// <param name="accession"></param>
        /// <param name="blockName">Имя блока</param>
        /// <param name="blockType">Тип блока</param>
        /// <param name="blockVersion">Версия блока</param>
        /// <param name="userName">Имя пользователя</param>
        /// <returns>Признак того, что печать прошла успешно</returns>
        public static bool Print( GroupDescription groupDescription, string accession, string blockName, string blockType, string blockVersion, string userName )
        {
            var dialog = new SaveFileDialog
            {
                Filter = Filter,
                FileName = CreateFileName( accession, groupDescription.Name, blockType )
            };

            if ( dialog.ShowDialog( ) != DialogResult.OK )
                return false;

            var stream = new FileStream( dialog.FileName, FileMode.Create, FileAccess.Write );
            using ( var writer = new StreamWriter( stream ) )
            {
                writer.WriteLine( "Отчет по данным блока" );
                writer.WriteLine( "Формируемые данные: " + groupDescription.Name );
                writer.WriteLine( "Тип блока: " + blockName );
                writer.WriteLine( "Версия блока: " + blockVersion );
                writer.WriteLine( "Время создания отчета: " + DateTime.Now.ToLocalTime( ) );
                writer.WriteLine( "Имя учетной записи: " + userName );
                writer.WriteLine( "************************" );

                foreach ( var group in groupDescription.Groups )
                    writer.WriteLine( CreateString( group, 1 ) );
            }
            return true;
        }
 
        /// <summary>
        /// Создание имени файла
        /// </summary>
        /// <param name="accession">Присоединение</param>
        /// <param name="reportName">Имя отчета</param>
        /// <param name="blockType">Тип блока</param>
        /// <returns>Имя файла</returns>
        private static String CreateFileName( string accession, string reportName, string blockType )
        {
            return string.Format( "{0}{1}{2}_{3}{4}{5}@{6}@{7}@{8}.txt", DateTime.Now.Year, DateTime.Now.Month.ToString( "00" ),
                                  DateTime.Now.Day.ToString( "00" ), DateTime.Now.Hour.ToString( "00" ),
                                  DateTime.Now.Minute.ToString( "00" ), DateTime.Now.Second.ToString( "00" ), accession, reportName, blockType );
        }
        /// <summary>
        /// Создание строка отчета
        /// </summary>
        /// <param name="group">Группа</param>
        /// <param name="tabShift">Отступ</param>
        /// <returns>Строка отчета</returns>
        private static String CreateString( GroupDescription group, int tabShift )
        {
            var stringBuilder = new StringBuilder( );
            var groupLevelTab = CreateTab( tabShift );
            var recordLevelTab = CreateTab( tabShift + 1 );

            stringBuilder.AppendFormat( "{0}Группа сигналов: {1}", groupLevelTab, group.Name );
            stringBuilder.AppendLine( );

            foreach ( var readGroup in group.Groups )
                stringBuilder.AppendLine( CreateString( readGroup, tabShift + 2 ) );

            foreach ( var record in group.Tags )
            {
                var @string = string.Format( "{0}{1}: {2}", recordLevelTab, record.Name, record.Value );
                if ( !string.IsNullOrEmpty( record.Uom ) ) @string += " (" + record.Uom + ')';
                stringBuilder.AppendLine( @string );
            }

            return stringBuilder.ToString( );
        }
        /// <summary>
        /// Создание отступа
        /// </summary>
        /// <param name="tabShift">Текущий отступ</param>
        /// <returns>Строка отступа</returns>
        private static String CreateTab( int tabShift )
        {
            var str = string.Empty;
            for ( var i = 0; i < tabShift; i++ )
                str += '\t';
            return str;
        }
    }
}
