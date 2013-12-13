using Structure;

namespace LibraryElements.CalculationBlocks
{
    /// <summary>
    /// Интерфейс статус-протокола
    /// </summary>
    public interface IStateProtocol
    {
        /// <summary>
        /// Состояние протокола
        /// </summary>
        ProtocolStatus StateProtocol { get; set; }
    }
}
