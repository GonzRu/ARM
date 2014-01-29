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

            return ( value < RangeMinValue || value > RangeMaxValue ) ? true : false;
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
        bool outRange = false; // Для запоминания промежуточного значения
        double rangeMinHysteresis = DefaultRangeMinHysteresis, rangeMaxHysteresis = DefaultRangeMaxHysteresis;

        public HysteresisRange() { }
        /// <summary>
        /// Проверка диапозона
        /// </summary>
        /// <param name="value">Значение</param>
        /// <returns>true - если значение вышло за диапозон</returns>        
        public override bool OutOfRange( Double value )
        {
            bool outDefRange = base.OutOfRange( value );
            bool inHystRange = ( value > RangeMinHysteresis && value < RangeMaxHysteresis ) ? true : false;

            if ( outDefRange )
            {
                outRange = true;
                return true;
            }
            else if ( inHystRange )
            {
                if (RangeMaxValue == 0 && rangeMinHysteresis == 0)
                    return false;

                outRange = false;
                return false;
            }
            else return outRange;
        }
        /// <summary>
        /// Получить или задать минимальное значение гистерезиса
        /// </summary>
        public double RangeMinHysteresis
        {
            get { return rangeMinHysteresis; }
            set
            {
                rangeMinHysteresis = value;

                if ( RangeMinValue > rangeMinHysteresis )
                    RangeMinValue = rangeMinHysteresis;
            }
        }
        /// <summary>
        /// Получить или задать максимальное значение гистерезиса
        /// </summary>        
        public double RangeMaxHysteresis
        {
            get { return rangeMaxHysteresis; }
            set
            {
                rangeMaxHysteresis = value;

                if ( RangeMaxValue < rangeMaxHysteresis )
                    RangeMaxValue = rangeMaxHysteresis;
            }
        }
    }
}
