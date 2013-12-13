using System;

namespace WpfDashboards
{
   /// <summary>
   /// Логика взаимодействия для FaceBMMRCH
   /// </summary>
   public partial class FaceBMMRCH : BaseFace, IFaceBMMRCH
   {
      #region Class Method
      public FaceBMMRCH()
      {
         LEDRun = new LEDParameter("LEDRun");
         LEDLabel1 = new LEDParameter("LEDLabel1");
         LEDLabel2 = new LEDParameter("LEDLabel2");
         LEDLabel3 = new LEDParameter("LEDLabel3");
         LEDLabel4 = new LEDParameter("LEDLabel4");
         LEDLabel5 = new LEDParameter("LEDLabel5");
         LEDLabel6 = new LEDParameter("LEDLabel6");
         LEDLabel7 = new LEDParameter("LEDLabel7");
         LEDLabel8 = new LEDParameter("LEDLabel8");

         LEDRun.ValueChange += new LEDParameter.ValueChangeDelegate(OnPropertyChanged);
         LEDLabel1.ValueChange += new LEDParameter.ValueChangeDelegate(OnPropertyChanged);
         LEDLabel2.ValueChange += new LEDParameter.ValueChangeDelegate(OnPropertyChanged);
         LEDLabel3.ValueChange += new LEDParameter.ValueChangeDelegate(OnPropertyChanged);
         LEDLabel4.ValueChange += new LEDParameter.ValueChangeDelegate(OnPropertyChanged);
         LEDLabel5.ValueChange += new LEDParameter.ValueChangeDelegate(OnPropertyChanged);
         LEDLabel6.ValueChange += new LEDParameter.ValueChangeDelegate(OnPropertyChanged);
         LEDLabel7.ValueChange += new LEDParameter.ValueChangeDelegate(OnPropertyChanged);
         LEDLabel8.ValueChange += new LEDParameter.ValueChangeDelegate(OnPropertyChanged);
         
         InitializeComponent();
      }
      #endregion

      #region Override Methods
      /// <summary>
      /// Установка первичного значения
      /// </summary>
      /// <param name="_bind">Имя привязки</param>
      /// <param name="_value">Значение</param>
      public override void SetFirstValue(string _bind, bool _value)
      {
         base.SetFirstValue(_bind, _value);

         if (LEDRun.Bind == _bind) { LEDRun.Value = _value; return; }
         if (LEDLabel1.Bind == _bind) { LEDLabel1.Value = _value; return; }
         if (LEDLabel2.Bind == _bind) { LEDLabel2.Value = _value; return; }
         if (LEDLabel3.Bind == _bind) { LEDLabel3.Value = _value; return; }
         if (LEDLabel4.Bind == _bind) { LEDLabel4.Value = _value; return; }
         if (LEDLabel5.Bind == _bind) { LEDLabel5.Value = _value; return; }
         if (LEDLabel6.Bind == _bind) { LEDLabel6.Value = _value; return; }
         if (LEDLabel7.Bind == _bind) { LEDLabel7.Value = _value; return; }
         if (LEDLabel8.Bind == _bind) { LEDLabel8.Value = _value; return; }
      }
      /// <summary>
      /// Обновление всех свойств на форме
      /// </summary>
      public override void UpDateLabels()
      {
         //для того, чтоб применились все изменения в конструкторах наследования
         //принудительно сообщаем форме о изменении свойств
         OnPropertyChanged("LEDLabel1");
         OnPropertyChanged("LEDLabel2");
         OnPropertyChanged("LEDLabel3");
         OnPropertyChanged("LEDLabel4");
         OnPropertyChanged("LEDLabel5");
         OnPropertyChanged("LEDLabel6");
         OnPropertyChanged("LEDLabel7");
         OnPropertyChanged("LEDLabel8");
      }
      #endregion

      #region Properties
      /// <summary>
      /// Получить параметер сигнала пуск
      /// </summary>      
      public LEDParameter LEDRun { get; private set; }
      /// <summary>
      /// Получить параметр сигнала Label1
      /// </summary>      
      public LEDParameter LEDLabel1 { get; private set; }
      /// <summary>
      /// Получить параметр сигнала Label2
      /// </summary>      
      public LEDParameter LEDLabel2 { get; private set; }
      /// <summary>
      /// Получить параметр сигнала Label3
      /// </summary>      
      public LEDParameter LEDLabel3 { get; private set; }
      /// <summary>
      /// Получить параметр сигнала Label4
      /// </summary>      
      public LEDParameter LEDLabel4 { get; private set; }
      /// <summary>
      /// Получить параметр сигнала Label5
      /// </summary>      
      public LEDParameter LEDLabel5 { get; private set; }
      /// <summary>
      /// Получить параметр сигнала Label6
      /// </summary>      
      public LEDParameter LEDLabel6 { get; private set; }
      /// <summary>
      /// Получить параметр сигнала Label7
      /// </summary>      
      public LEDParameter LEDLabel7 { get; private set; }
      /// <summary>
      /// Получить параметр сигнала Label8
      /// </summary>      
      public LEDParameter LEDLabel8 { get; private set; }
      #endregion
   }
}
