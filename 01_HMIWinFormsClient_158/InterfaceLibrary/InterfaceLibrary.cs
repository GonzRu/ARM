using System;

namespace InterfaceLibrary
{
	/// <summary>
	///	Признак качества переменной 
	/// </summary> 
	public enum VarQualityNewDs
	{
		vqUndefined,      // Не определено (не производилось ни одного чтения,
		// нет связи или неверная конфигурация
		vqGood,           // Хорошее качество
		vqArhiv,          // архивная переменная (из БД)
		vqRangeError,     // Выход за пределы диапазона
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
