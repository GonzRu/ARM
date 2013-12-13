using System;

namespace LibraryElements.Sources
{
    /// <summary>
    /// ��� �������������
    /// </summary>
    public class SignalMatchRecord : DataRecord
    {
        /// <summary>
        /// ����������� ���������� ����
        /// </summary>
        /// <param name="name">��� �������</param>
        /// <param name="dsGuid">������������� DataServer'a</param>
        /// <param name="devGuid">������������� ����������</param>
        /// <param name="tagGuid">������������� ����</param>
        public SignalMatchRecord( String name, String dsGuid = "0", String devGuid = "0", String tagGuid = "0" )
            : base( name, RecordTypes.Tag )
        {
            this.DsGuid = Convert.ToUInt32( dsGuid );
            this.DevGuid = Convert.ToUInt32( devGuid );
            this.TagGuid = Convert.ToUInt32( tagGuid );
            Value = "NaN";
        }
        /// <summary>
        /// ���������� �����������
        /// </summary>
        /// <param name="original">������ �� ������ ������� �������� �����</param>
        internal SignalMatchRecord( SignalMatchRecord original )
            : base( original )
        {
            if ( original == null )
                return;

            this.DsGuid = original.DsGuid;
            this.DevGuid = original.DevGuid;
            this.TagGuid = original.TagGuid;
        }
        /// <summary>
        /// ������������� DataServer'a
        /// </summary>
        public UInt32 DsGuid { get; set; }
        /// <summary>
        /// ������������� ����������
        /// </summary>
        public UInt32 DevGuid { get; set; }
        /// <summary>
        /// ������������� ����
        /// </summary>
        public UInt32 TagGuid { get; set; }
        /// <summary>
        /// �������� �������������� ������
        /// </summary>
        public String Result { get { return string.Format( "{0}.{1}.{2}", this.DsGuid, this.DevGuid, this.TagGuid ); } }
    }
}