using System;

namespace WpfDashboards
{
   /// <summary>
   /// Логика взаимодействия для Sirius-OZZ
   /// </summary>
   public partial class FaceSiriusOZZ : BaseFace, IFaceSiriusOZZ
   {
      #region Class Method
      public FaceSiriusOZZ()
      {
         LEDLocateOZZ = new LEDParameter("LEDLocateOZZ");
         LEDGroundOneSection = new LEDParameter("LEDGroundOneSection");
         LEDGroundTwoSection = new LEDParameter("LEDGroundTwoSection");
         LEDLabel1 = new LEDParameter("LEDLabel1");
         LEDLabel2 = new LEDParameter("LEDLabel2");
         LEDLabel3 = new LEDParameter("LEDLabel3");
         LEDLabel4 = new LEDParameter("LEDLabel4");
         LEDLabel5 = new LEDParameter("LEDLabel5");
         LEDLabel6 = new LEDParameter("LEDLabel6");
         LEDLabel7 = new LEDParameter("LEDLabel7");
         LEDLabel8 = new LEDParameter("LEDLabel8");
         LEDLabel9 = new LEDParameter("LEDLabel9");
         LEDLabel10 = new LEDParameter("LEDLabel10");
         LEDLabel11 = new LEDParameter("LEDLabel11");
         LEDLabel12 = new LEDParameter("LEDLabel12");
         LEDLabel13 = new LEDParameter("LEDLabel13");
         LEDLabel14 = new LEDParameter("LEDLabel14");
         LEDLabel15 = new LEDParameter("LEDLabel15");
         LEDLabel16 = new LEDParameter("LEDLabel16");
         LEDLabel17 = new LEDParameter("LEDLabel17");
         LEDLabel18 = new LEDParameter("LEDLabel18");
         LEDLabel19 = new LEDParameter("LEDLabel19");
         LEDLabel20 = new LEDParameter("LEDLabel20");
         LEDLabel21 = new LEDParameter("LEDLabel21");
         LEDLabel22 = new LEDParameter("LEDLabel22");
         LEDLabel23 = new LEDParameter("LEDLabel23");
         LEDLabel24 = new LEDParameter("LEDLabel24");

         LEDLocateOZZ.ValueChange += new LEDParameter.ValueChangeDelegate(OnPropertyChanged);
         LEDGroundOneSection.ValueChange += new LEDParameter.ValueChangeDelegate(OnPropertyChanged);
         LEDGroundTwoSection.ValueChange += new LEDParameter.ValueChangeDelegate(OnPropertyChanged);
         LEDLabel1.ValueChange += new LEDParameter.ValueChangeDelegate(OnPropertyChanged);
         LEDLabel2.ValueChange += new LEDParameter.ValueChangeDelegate(OnPropertyChanged);
         LEDLabel3.ValueChange += new LEDParameter.ValueChangeDelegate(OnPropertyChanged);
         LEDLabel4.ValueChange += new LEDParameter.ValueChangeDelegate(OnPropertyChanged);
         LEDLabel5.ValueChange += new LEDParameter.ValueChangeDelegate(OnPropertyChanged);
         LEDLabel6.ValueChange += new LEDParameter.ValueChangeDelegate(OnPropertyChanged);
         LEDLabel7.ValueChange += new LEDParameter.ValueChangeDelegate(OnPropertyChanged);
         LEDLabel8.ValueChange += new LEDParameter.ValueChangeDelegate(OnPropertyChanged);
         LEDLabel9.ValueChange += new LEDParameter.ValueChangeDelegate(OnPropertyChanged);
         LEDLabel10.ValueChange += new LEDParameter.ValueChangeDelegate(OnPropertyChanged);
         LEDLabel11.ValueChange += new LEDParameter.ValueChangeDelegate(OnPropertyChanged);
         LEDLabel12.ValueChange += new LEDParameter.ValueChangeDelegate(OnPropertyChanged);
         LEDLabel13.ValueChange += new LEDParameter.ValueChangeDelegate(OnPropertyChanged);
         LEDLabel14.ValueChange += new LEDParameter.ValueChangeDelegate(OnPropertyChanged);
         LEDLabel15.ValueChange += new LEDParameter.ValueChangeDelegate(OnPropertyChanged);
         LEDLabel16.ValueChange += new LEDParameter.ValueChangeDelegate(OnPropertyChanged);
         LEDLabel17.ValueChange += new LEDParameter.ValueChangeDelegate(OnPropertyChanged);
         LEDLabel18.ValueChange += new LEDParameter.ValueChangeDelegate(OnPropertyChanged);
         LEDLabel19.ValueChange += new LEDParameter.ValueChangeDelegate(OnPropertyChanged);
         LEDLabel20.ValueChange += new LEDParameter.ValueChangeDelegate(OnPropertyChanged);
         LEDLabel21.ValueChange += new LEDParameter.ValueChangeDelegate(OnPropertyChanged);
         LEDLabel22.ValueChange += new LEDParameter.ValueChangeDelegate(OnPropertyChanged);
         LEDLabel23.ValueChange += new LEDParameter.ValueChangeDelegate(OnPropertyChanged);
         LEDLabel24.ValueChange += new LEDParameter.ValueChangeDelegate(OnPropertyChanged);

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

         if (LEDLocateOZZ.Bind == _bind) { LEDLocateOZZ.Value = _value; return; }
         if (LEDGroundOneSection.Bind == _bind) { LEDGroundOneSection.Value = _value; return; }
         if (LEDGroundTwoSection.Bind == _bind) { LEDGroundTwoSection.Value = _value; return; }
         if (LEDLabel1.Bind == _bind) { LEDLabel1.Value = _value; return; }
         if (LEDLabel2.Bind == _bind) { LEDLabel2.Value = _value; return; }
         if (LEDLabel3.Bind == _bind) { LEDLabel3.Value = _value; return; }
         if (LEDLabel4.Bind == _bind) { LEDLabel4.Value = _value; return; }
         if (LEDLabel5.Bind == _bind) { LEDLabel5.Value = _value; return; }
         if (LEDLabel6.Bind == _bind) { LEDLabel6.Value = _value; return; }
         if (LEDLabel7.Bind == _bind) { LEDLabel7.Value = _value; return; }
         if (LEDLabel8.Bind == _bind) { LEDLabel8.Value = _value; return; }
         if (LEDLabel9.Bind == _bind) { LEDLabel9.Value = _value; return; }
         if (LEDLabel10.Bind == _bind) { LEDLabel10.Value = _value; return; }
         if (LEDLabel11.Bind == _bind) { LEDLabel11.Value = _value; return; }
         if (LEDLabel12.Bind == _bind) { LEDLabel12.Value = _value; return; }
         if (LEDLabel13.Bind == _bind) { LEDLabel13.Value = _value; return; }
         if (LEDLabel14.Bind == _bind) { LEDLabel14.Value = _value; return; }
         if (LEDLabel15.Bind == _bind) { LEDLabel15.Value = _value; return; }
         if (LEDLabel16.Bind == _bind) { LEDLabel16.Value = _value; return; }
         if (LEDLabel17.Bind == _bind) { LEDLabel17.Value = _value; return; }
         if (LEDLabel18.Bind == _bind) { LEDLabel18.Value = _value; return; }
         if (LEDLabel19.Bind == _bind) { LEDLabel19.Value = _value; return; }
         if (LEDLabel20.Bind == _bind) { LEDLabel20.Value = _value; return; }
         if (LEDLabel21.Bind == _bind) { LEDLabel21.Value = _value; return; }
         if (LEDLabel22.Bind == _bind) { LEDLabel22.Value = _value; return; }
         if (LEDLabel23.Bind == _bind) { LEDLabel23.Value = _value; return; }
         if (LEDLabel24.Bind == _bind) { LEDLabel24.Value = _value; return; }
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
         OnPropertyChanged("LEDLabel9");
         OnPropertyChanged("LEDLabel10");
         OnPropertyChanged("LEDLabel11");
         OnPropertyChanged("LEDLabel12");
         OnPropertyChanged("LEDLabel13");
         OnPropertyChanged("LEDLabel14");
         OnPropertyChanged("LEDLabel15");
         OnPropertyChanged("LEDLabel16");
         OnPropertyChanged("LEDLabel17");
         OnPropertyChanged("LEDLabel18");
         OnPropertyChanged("LEDLabel19");
         OnPropertyChanged("LEDLabel20");
         OnPropertyChanged("LEDLabel21");
         OnPropertyChanged("LEDLabel22");
         OnPropertyChanged("LEDLabel23");
         OnPropertyChanged("LEDLabel24");
      }
      #endregion

      #region Properties
      /// <summary>
      /// Получить параметр сигнала озз обнаружено
      /// </summary>
      public LEDParameter LEDLocateOZZ
      {
         get;
         private set;
      }
      /// <summary>
      /// Получить параметр сигнала земля на первой секции
      /// </summary> 
      public LEDParameter LEDGroundOneSection
      {
         get;
         private set;
      }
      /// <summary>
      /// Получить параметр сигнала земля на второй секции
      /// </summary>
      public LEDParameter LEDGroundTwoSection
      {
         get;
         private set;
      }
      /// <summary>
      /// Получить параметр сигнала Label1
      /// </summary> 
      public LEDParameter LEDLabel1
      {
         get;
         private set;
      }
      /// <summary>
      /// Получить параметр сигнала Label2
      /// </summary>
      public LEDParameter LEDLabel2
      {
         get;
         private set;
      }
      /// <summary>
      /// Получить параметр сигнала Label3
      /// </summary> 
      public LEDParameter LEDLabel3
      {
         get;
         private set;
      }
      /// <summary>
      /// Получить параметр сигнала Label4
      /// </summary>
      public LEDParameter LEDLabel4
      {
         get;
         private set;
      }
      /// <summary>
      /// Получить параметр сигнала Label5
      /// </summary>
      public LEDParameter LEDLabel5
      {
         get;
         private set;
      }
      /// <summary>
      /// Получить параметр сигнала Label6
      /// </summary>
      public LEDParameter LEDLabel6
      {
         get;
         private set;
      }
      /// <summary>
      /// Получить параметр сигнала Label1
      /// </summary>
      public LEDParameter LEDLabel7
      {
         get;
         private set;
      }
      /// <summary>
      /// Получить параметр сигнала Label8
      /// </summary>
      public LEDParameter LEDLabel8
      {
         get;
         private set;
      }
      /// <summary>
      /// Получить параметр сигнала Label9
      /// </summary>
      public LEDParameter LEDLabel9
      {
         get;
         private set;
      }
      /// <summary>
      /// Получить параметр сигнала Label10
      /// </summary>
      public LEDParameter LEDLabel10
      {
         get;
         private set;
      }
      /// <summary>
      /// Получить параметр сигнала Label11
      /// </summary>
      public LEDParameter LEDLabel11
      {
         get;
         private set;
      }
      /// <summary>
      /// Получить параметр сигнала Label12
      /// </summary>
      public LEDParameter LEDLabel12
      {
         get;
         private set;
      }
      /// <summary>
      /// Получить параметр сигнала Label13
      /// </summary>
      public LEDParameter LEDLabel13
      {
         get;
         private set;
      }
      /// <summary>
      /// Получить параметр сигнала Label14
      /// </summary>
      public LEDParameter LEDLabel14
      {
         get;
         private set;
      }
      /// <summary>
      /// Получить параметр сигнала Label15
      /// </summary>
      public LEDParameter LEDLabel15
      {
         get;
         private set;
      }
      /// <summary>
      /// Получить параметр сигнала Label16
      /// </summary>
      public LEDParameter LEDLabel16
      {
         get;
         private set;
      }
      /// <summary>
      /// Получить параметр сигнала Label17
      /// </summary>
      public LEDParameter LEDLabel17
      {
         get;
         private set;
      }
      /// <summary>
      /// Получить параметр сигнала Label18
      /// </summary>
      public LEDParameter LEDLabel18
      {
         get;
         private set;
      }
      /// <summary>
      /// Получить параметр сигнала Label19
      /// </summary>
      public LEDParameter LEDLabel19
      {
         get;
         private set;
      }
      /// <summary>
      /// Получить параметр сигнала Label20
      /// </summary>
      public LEDParameter LEDLabel20
      {
         get;
         private set;
      }
      /// <summary>
      /// Получить параметр сигнала Label21
      /// </summary>
      public LEDParameter LEDLabel21
      {
         get;
         private set;
      }
      /// <summary>
      /// Получить параметр сигнала Label22
      /// </summary>
      public LEDParameter LEDLabel22
      {
         get;
         private set;
      }
      /// <summary>
      /// Получить параметр сигнала Label23
      /// </summary>
      public LEDParameter LEDLabel23
      {
         get;
         private set;
      }
      /// <summary>
      /// Получить параметр сигнала Label24
      /// </summary>
      public LEDParameter LEDLabel24
      {
         get;
         private set;
      }
      #endregion
   }
}
