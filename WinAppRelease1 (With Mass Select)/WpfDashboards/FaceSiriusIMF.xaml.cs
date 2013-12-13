using System;

namespace WpfDashboards
{
   /// <summary>
   /// Базовая логика взаимодействия для Sirius IMF
   /// </summary>
   public partial class FaceSiriusIMF : BaseFace, IFaceSiriusIMF
   {
      #region Class Method
      public FaceSiriusIMF()
      {
         LEDRun = new LEDParameter("LEDRun");
         LEDSignal = new LEDParameter("LEDSignal");

         LEDRun.ValueChange += new LEDParameter.ValueChangeDelegate(OnPropertyChanged);
         LEDSignal.ValueChange += new LEDParameter.ValueChangeDelegate(OnPropertyChanged);

         InitializeComponent();
      }
      #endregion

      #region Override Method
      /// <summary>
      /// Установка первичного значения
      /// </summary>
      /// <param name="_bind">Имя привязки</param>
      /// <param name="_value">Значение</param>
      public override void SetFirstValue(string _bind, bool _value)
      {
         base.SetFirstValue(_bind, _value);

         if (LEDRun.Bind == _bind) { LEDRun.Value = _value; return; }
         if (LEDSignal.Bind == _bind) { LEDSignal.Value = _value; return; }
      }
      #endregion

      #region Properties
      /// <summary>
      /// Получить параметер сигнала пуск
      /// </summary>      
      public LEDParameter LEDRun
      {
         get;
         private set;
      }
      /// <summary>
      /// Получить параметер сигнала сигнал
      /// </summary>      
      public LEDParameter LEDSignal
      {
         get;
         private set;
      }
      #endregion
   }
}
