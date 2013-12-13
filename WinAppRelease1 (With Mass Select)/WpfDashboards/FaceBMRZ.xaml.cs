using System;

namespace WpfDashboards
{
   /// <summary>
   /// Базовая логика взаимодействия для BMRZ
   /// </summary>
   public partial class FaceBMRZ : BaseFace, IFaceBMRZ
   {
      #region Class Method
      public FaceBMRZ()
      {
         LEDOn = new LEDParameter("LEDOn");
         LEDOff = new LEDParameter("LEDOff");
         LEDFailure = new LEDParameter("LEDFailure");
         LEDRun = new LEDParameter("LEDRun");
         LEDOperation = new LEDParameter("LEDOperation");
         LEDCall = new LEDParameter("LEDCall");
         LEDLocal = new LEDParameter("LEDLocal");

         LEDOn.ValueChange += new LEDParameter.ValueChangeDelegate(OnPropertyChanged);
         LEDOff.ValueChange += new LEDParameter.ValueChangeDelegate(OnPropertyChanged);
         LEDFailure.ValueChange += new LEDParameter.ValueChangeDelegate(OnPropertyChanged);
         LEDRun.ValueChange += new LEDParameter.ValueChangeDelegate(OnPropertyChanged);
         LEDOperation.ValueChange += new LEDParameter.ValueChangeDelegate(OnPropertyChanged);
         LEDCall.ValueChange += new LEDParameter.ValueChangeDelegate(OnPropertyChanged);
         LEDLocal.ValueChange += new LEDParameter.ValueChangeDelegate(OnPropertyChanged);

         InitializeComponent();
      }
      #endregion

      #region Override Method
      /// <summary>
      /// Установка первичного значения
      /// </summary>
      /// <param name="_bind">Имя привязки</param>
      /// <param name="_value">Значение</param>
      public override void SetFirstValue(String _bind, Boolean _value)
      {
         base.SetFirstValue(_bind, _value);

         if (LEDOn.Bind == _bind) { LEDOn.Value = _value; return; }
         if (LEDOff.Bind == _bind) { LEDOff.Value = _value; return; }
         if (LEDFailure.Bind == _bind) { LEDFailure.Value = _value; return; }
         if (LEDRun.Bind == _bind) { LEDRun.Value = _value; return; }
         if (LEDOperation.Bind == _bind) { LEDOperation.Value = _value; return; }
         if (LEDCall.Bind == _bind) { LEDCall.Value = _value; return; }
         if (LEDLocal.Bind == _bind) { LEDLocal.Value = _value; return; }
      }
      #endregion

      #region Properties
      /// <summary>
      /// Получить параметер сигнала вкл
      /// </summary>      
      public LEDParameter LEDOn { get; private set; }
      /// <summary>
      /// Получить параметер сигнала выкл
      /// </summary>      
      public LEDParameter LEDOff { get; private set; }
      /// <summary>
      /// Получить параметер сигнала отказ
      /// </summary>      
      public LEDParameter LEDFailure { get; private set; }
      /// <summary>
      /// Получить параметер сигнала пуск
      /// </summary>      
      public LEDParameter LEDRun { get; private set; }
      /// <summary>
      /// Получить параметер сигнала выполнение
      /// </summary>      
      public LEDParameter LEDOperation { get; private set; }
      /// <summary>
      /// Получить параметер сигнала вызов
      /// </summary>      
      public LEDParameter LEDCall { get; private set; }
      /// <summary>
      /// Получить параметер сигнала местного управления
      /// </summary>      
      public LEDParameter LEDLocal { get; private set; }
      #endregion
   }
}
