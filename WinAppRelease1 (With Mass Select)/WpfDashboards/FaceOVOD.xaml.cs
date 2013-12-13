using System;

namespace WpfDashboards
{
   /// <summary>
   /// Логика взаимодействия для FaceOVOD
   /// </summary>
   public partial class FaceOVOD : BaseFace, IFaceOVOD
   {
      #region Class Method
      public FaceOVOD()
      {
         LEDOperation = new LEDParameter("LEDOperation");
         LEDFailure = new LEDParameter("LEDFailure");
         LEDControlOfCurrentDrawn = new LEDParameter("LEDControlOfCurrentDrawn");
         LEDDisconnectedSensors = new LEDParameter("LEDDisconnectedSensors");

         LEDFailure.ValueChange += new LEDParameter.ValueChangeDelegate(OnPropertyChanged);
         LEDOperation.ValueChange += new LEDParameter.ValueChangeDelegate(OnPropertyChanged);
         LEDControlOfCurrentDrawn.ValueChange += new LEDParameter.ValueChangeDelegate(OnPropertyChanged);
         LEDDisconnectedSensors.ValueChange += new LEDParameter.ValueChangeDelegate(OnPropertyChanged);

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

         if (LEDOperation.Bind == _bind) { LEDOperation.Value = _value; return; }
         if (LEDFailure.Bind == _bind) { LEDFailure.Value = _value; return; }
         if (LEDControlOfCurrentDrawn.Bind == _bind) { LEDControlOfCurrentDrawn.Value = _value; return; }
         if (LEDDisconnectedSensors.Bind == _bind) { LEDDisconnectedSensors.Value = _value; return; }
      }
      #endregion

      #region Properties
      /// <summary>
      /// Получить параметер сигнала выполнение
      /// </summary>      
      public LEDParameter LEDOperation { get; private set; }
      /// <summary>
      /// Получить параметер сигнала отказ
      /// </summary>      
      public LEDParameter LEDFailure { get; private set; }
      /// <summary>
      /// Получить параметер сигнала контроль по току выведен
      /// </summary>      
      public LEDParameter LEDControlOfCurrentDrawn { get; private set; }
      /// <summary>
      /// Получить параметер сигнала отключенные датчики
      /// </summary>      
      public LEDParameter LEDDisconnectedSensors { get; private set; }
      #endregion
   }
}
