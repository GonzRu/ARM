using System;

namespace InterfaceLibrary
{
	/// <summary>
	///	Признак качества переменной 
	/// </summary> 
	public enum VarQualityNewDs
	{
        vqUndefined = 0,        // Не определено (не производилось ни одного чтения, нет связи)
        vqGood = 1,             // Хорошее качество
        vqArhiv = 2,            // архивная переменная (из БД)
        vqRangeError = 3,       // Выход за пределы диапазона
        vqHandled = 4,          // Ручной ввод данных
        vqUknownTag = 5,        // несуществующий тег (? что значит не существующий тег - м.б. это может исп. в ответах на запросы когда запрашивается тег кот. нет, тогда возвращ его ид и это знач качества)
        vqErrorConverted = 6,   // ошибка преобразования в целевой тип
        vqNonExistDevice = 7,   // несуществующее устройство
        vqTagLengthIs0 = 8,     // длина запрашиваемого тега нулевая
        vqUknownError = 9,      // неизвестная ошибка при попытке получения значения тега
        /*
         * тег неактуален из-за
         * нарушения связи между
         * Dsr и Ds 
         * (это качество формируется на роутере)
         */
        vqDsr2DsBadConnection = 10
	};

    /// <summary>
    /// Тип сигнала/тэга
    /// </summary>
    public enum TypeOfTag
    {
        NaN, // первый элемент - вне категории
        Analog, // аналоговый
        Discret, // дискретный
        Combo, //  ComboBox
        DateTime, //  DateTime
        String // Строка
    }

    // делегат на изменение тега
    public delegate void ChVarNewDs( Tuple<string, byte[], DateTime, VarQualityNewDs> var );
    public delegate void ChVarNewDsNew( Tuple<string, byte[], DateTime, VarQualityNewDs> var, TypeOfTag type );
}
