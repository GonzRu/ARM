using System;

namespace LibraryElements.Sources
{
    /// <summary>
    /// ����� ������
    /// </summary>
    public class DataRecord
    {
        public enum RecordTypes
        {
            Unknown,
            Text,
            Tag,
            Color,
            Image,
            Rotate,
            Boolean,
            StateProtocol
        }
        public DataRecord( String name = "Unknown", RecordTypes type = RecordTypes.Unknown )
        {
            this.Name = name;
            this.RecordType = type;
        }
        internal DataRecord( DataRecord original )
        {
            if ( original == null )
                return;

            this.Name = original.Name;
            this.Value = original.Value;
            this.RecordType = original.RecordType;
        }
        /// <summary>
        /// ��� ������
        /// </summary>
        public String Name { get; private set; }
        /// <summary>
        /// �������� ������
        /// </summary>
        public Object Value { get; set; }
        /// <summary>
        /// ��� ������
        /// </summary>
        public RecordTypes RecordType { get; private set; }

        internal static DataRecord GetCopy( DataRecord original )
        {
            return original is SignalMatchRecord
                       ? new SignalMatchRecord( (SignalMatchRecord)original )
                       : new DataRecord( original );
        }
    }
}