using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using System.Text;

namespace ReportLibrary
{
    public interface IReport
    {
        void Print();
    }

    public static class ReportHelper
    {
        private const string Filter = "Текстовый файл|*.txt";

        public static bool Print( Group writeGroup, string reportName, string accession, string blockName, string blockType, string blockVersion, string userName )
        {
            var dialog = new SaveFileDialog
                             { 
                                 Filter = Filter,
                                 FileName = CreateFileName( accession, reportName, blockType ) 
                             };

            if ( dialog.ShowDialog() != DialogResult.OK )
                return false;

            var stream = new FileStream( dialog.FileName, FileMode.Create, FileAccess.Write );
            using (var writer = new StreamWriter( stream ))
            {
                writer.WriteLine( "Отчет по данным блока" );
                writer.WriteLine( "Формируемые данные: " + reportName );
                writer.WriteLine( "Тип блока: " + blockName );
                writer.WriteLine( "Версия блока: " + blockVersion );
                writer.WriteLine( "Время создания отчета: " + DateTime.Now.ToLocalTime() );
                writer.WriteLine( "Имя учетной записи: " + userName );
                writer.WriteLine( "************************" );

                foreach ( var group in writeGroup.GroupCollection )
                    writer.WriteLine( CreateString( group, 1 ) );
            }
            return true;
        }
        private static String CreateString( Group group, int tabShift )
        {
            var stringBuilder = new StringBuilder();
            var groupLevelTab = CreateTab( tabShift );
            var recordLevelTab = CreateTab( tabShift + 1 );

            stringBuilder.AppendFormat( "{0}Группа сигналов: {1}", groupLevelTab, group.Name );
            stringBuilder.AppendLine();

            foreach ( var readGroup in group.GroupCollection )
                stringBuilder.AppendLine( CreateString( readGroup, tabShift + 2 ) );

            foreach ( var record in group.RecordCollection )
            {
                var @string = string.Format( "{0}{1}: {2}", recordLevelTab, record.Name, record.Value );
                if ( !string.IsNullOrEmpty( record.Unit ) ) @string += " (" + record.Unit + ')';
                stringBuilder.AppendLine( @string );
            }

            return stringBuilder.ToString();
        }
        private static String CreateTab(int tabShift)
        {
            var str = string.Empty;
            for ( var i = 0; i < tabShift; i++ )
                str += '\t';
            return str;
        }
        private static String CreateFileName( string accession, string reportName, string blockType )
        {
            return string.Format( "{0}{1}{2}_{3}{4}{5}@{6}@{7}@{8}.txt", DateTime.Now.Year, DateTime.Now.Month.ToString( "00" ),
                                  DateTime.Now.Day.ToString( "00" ), DateTime.Now.Hour.ToString( "00" ),
                                  DateTime.Now.Minute.ToString( "00" ), DateTime.Now.Second.ToString( "00" ), accession, reportName, blockType );
        }
    }

    public class Group
    {
        public Group(string name )
        {
            Name = name;
            GroupCollection = new List<Group>();
            RecordCollection = new List<Record>();
        }
        public String Name { get; private set; }
        public IList<Group> GroupCollection { get; private set; }
        public IList<Record> RecordCollection { get; private set; }
    }
    public class Record
    {
        public Record( string name, string value, string unit )
        { 
            Name = name; 
            Value = value;
            Unit = unit;
        }
        public String Name { get; private set; }
        public String Value { get; private set; }
        public String Unit { get; private set; }
    }
}
