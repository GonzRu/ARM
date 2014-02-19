using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NormalModeLibrary.Sources
{
    public interface IOutOfRangeHandler
    {
        event EventHandler OutOfRangeEvent;
    }
    class OutOfRangeEventArgs : EventArgs
    {
        public OutOfRangeEventArgs( bool outOfRange )
        {
            OutOfRange = outOfRange;
        }
        public Boolean OutOfRange { get; private set; }
    }

    /// <summary>
    /// Базовый класс диапозона
    /// </summary>
    class BaseRange
    {
        /// <summary>
        /// Минимальное значение по умолчанию
        /// </summary>
        public const double DefaultRangeMinValue = 0;
        /// <summary>
        /// Максимальное значение по умолчанию
        /// </summary>
        public const double DefaultRangeMaxValue = 0;

        public BaseRange()
        {
            RangeMinValue = DefaultRangeMinValue;
            RangeMaxValue = DefaultRangeMaxValue;
        }
        /// <summary>
        /// Проверка диапозона
        /// </summary>
        /// <param name="value">Значение</param>
        /// <returns>true - если значение вышло за диапозон</returns>
        public virtual bool OutOfRange( Double value )
        {
            if (RangeMinValue == 0 && RangeMaxValue == 0)
                return false;

            return ( value < RangeMinValue || value > RangeMaxValue );
        }

        /// <summary>
        /// Получить или задать минимальное значение
        /// </summary>
        public double RangeMinValue { get; set; }
        /// <summary>
        /// Получить или задать максимальное значение
        /// </summary>        
        public double RangeMaxValue { get; set; }
    }
    /// <summary>
    /// Диапозон гистерезиса
    /// </summary>
    class HysteresisRange : BaseRange
    {
        /// <summary>
        /// Минимальное значение гистерезиса по умолчанию
        /// </summary>
        public const double DefaultRangeMinHysteresis = 0;
        /// <summary>
        /// Максимальное значение гистерезиса по умолчанию
        /// </summary>        
        public const double DefaultRangeMaxHysteresis = 0;
        private bool _isLastOutOfRange = false; // Для запоминания промежуточного значения
        double rangeMinHysteresis = DefaultRangeMinHysteresis, rangeMaxHysteresis = DefaultRangeMaxHysteresis;

        public HysteresisRange() { }
        /// <summary>
        /// Проверка диапозона
        /// </summary>
        /// <param name="value">Значение</param>
        /// <param name="isOutOfRangenow">Показывает прошлое состояние, от которого зависит текущее</param>
        /// <returns>true - если значение вышло за диапозон</returns>        
        public override bool OutOfRange( Double value)
        {
            bool outDefRange = base.OutOfRange(value);         

            if ( outDefRange )
                return (_isLastOutOfRange = true);
            else
            {
                if (_isLastOutOfRange)
                {
                    bool inHystRange = (value > RangeMaxValue - RangeMaxHysteresis ||
                                        value < RangeMinValue + RangeMinHysteresis);

                    _isLastOutOfRange = inHystRange;
                    return inHystRange;
                }
                else
                    return _isLastOutOfRange;
            }
        }

        /// <summary>
        /// Получить или задать минимальное значение гистерезиса
        /// </summary>
        public double RangeMinHysteresis
        {
            get { return rangeMinHysteresis; }
            set { rangeMinHysteresis = value; }
        }

        /// <summary>
        /// Получить или задать максимальное значение гистерезиса
        /// </summary>        
        public double RangeMaxHysteresis
        {
            get { return rangeMaxHysteresis; }
            set { rangeMaxHysteresis = value; }
        }
    }
}
