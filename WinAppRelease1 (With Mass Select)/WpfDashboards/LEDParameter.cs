using System;
using System.Windows.Media;
using System.Timers;

namespace WpfDashboards
{
   /// <summary>
   /// Параметр светодиода
   /// </summary>
   public class LEDParameter
   {
      /// <summary>
      /// Делегат изменения значения
      /// </summary>
      /// <param name="propertyName">Имя свойства</param>
      internal delegate void ValueChangeDelegate(String propertyName);
      /// <summary>
      /// Признак изменения значения
      /// </summary>
      internal event ValueChangeDelegate ValueChange;
      bool ledvalue;
      string name;
      Brush led1, led2;
      
      /// <summary>
      /// Светодиод
      /// </summary>
      /// <param name="_nameProperty">Имя свойства</param>
      internal LEDParameter(String _nameProperty) : this(_nameProperty, Brushes.Green, Brushes.Gray) { }
      /// <summary>
      /// Светодиод
      /// </summary>
      /// <param name="_nameProperty">Имя свойства</param>
      /// <param name="_led1">Цвет сигнала</param>
      /// <param name="_led2">Цвет отсудствия сигнала</param>
      internal LEDParameter(String _nameProperty, Brush _led1, Brush _led2)
      {
         name = _nameProperty;
         Bind = "Unknown bind";
         Label = "Unknown label";
         LED = Brushes.Gray;
         led1 = _led1;
         led2 = _led2;
      }

      /// <summary>
      /// Получить или задать(internal) значение параметра
      /// </summary>
      public Boolean? Value
      {
         get { return ledvalue; }
         internal set
         {
            ledvalue = (value == true) ? true : false;

            switch (ledvalue)
            {
               case true: LED = led1; break;
               case false: LED = led2; break;
               default: LED = Brushes.Gray; break;
            }

            if (ValueChange != null)
               ValueChange(name);
         }
      }
      /// <summary>
      /// Получить цвет светодиода
      /// </summary>
      public Brush LED
      {
         get;
         private set;
      }
      /// <summary>
      /// Получить или задать имя привязки
      /// </summary>
      public String Bind
      {
         get;
         set;
      }
      /// <summary>
      /// Получить или задать имя сигнала
      /// </summary>
      public String Label
      {
         get;
         set;
      }
   }
}