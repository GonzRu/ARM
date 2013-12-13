using System;

namespace LibraryElements.Sources
{
    /// <summary>
    /// Тег сопоставления
    /// </summary>
    public class SignalMatchRecord : DataRecord
    {
        /// <summary>
        /// Конструктор расчетного тэга
        /// </summary>
        /// <param name="name">Имя сигнала</param>
        /// <param name="dsGuid">Идентификатор DataServer'a</param>
        /// <param name="devGuid">Идентификатор устройства</param>
        /// <param name="tagGuid">Идентификатор тэга</param>
        public SignalMatchRecord( String name, String dsGuid = "0", String devGuid = "0", String tagGuid = "0" )
            : base( name, RecordTypes.Tag )
        {
            this.DsGuid = Convert.ToUInt32( dsGuid );
            this.DevGuid = Convert.ToUInt32( devGuid );
            this.TagGuid = Convert.ToUInt32( tagGuid );
            Value = "NaN";
        }
        /// <summary>
        /// Копирующий конструктор
        /// </summary>
        /// <param name="original">Данные на основе которых делается копия</param>
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
        /// Идентификатор DataServer'a
        /// </summary>
        public UInt32 DsGuid { get; set; }
        /// <summary>
        /// Идентификатор устройства
        /// </summary>
        public UInt32 DevGuid { get; set; }
        /// <summary>
        /// Идентификатор тэга
        /// </summary>
        public UInt32 TagGuid { get; set; }
        /// <summary>
        /// Получить результирующую строку
        /// </summary>
        public String Result { get { return string.Format( "{0}.{1}.{2}", this.DsGuid, this.DevGuid, this.TagGuid ); } }
    }
}