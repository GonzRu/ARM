using System;

namespace WpfDashboards
{
   /// <summary>
   /// Логика взаимодействия для FaceSiriusCS
   /// </summary>
   public partial class FaceSiriusCS : BaseFace, IFaceSiriusCS
   {
      #region Class Method
      public FaceSiriusCS()
      {
         LEDTracking = new LEDParameter("LEDTracking");
         LEDProgramming = new LEDParameter("LEDProgramming");
         LEDViewInformation = new LEDParameter("LEDViewInformation");
         LEDResetInformation = new LEDParameter("LEDResetInformation");
         LEDStateTireAS = new LEDParameter("LEDStateTireAS");
         LEDStateTirePS = new LEDParameter("LEDStateTirePS");
         LEDNewInformationAS = new LEDParameter("LEDNewInformationAS");
         LEDNewInformationPS = new LEDParameter("LEDNewInformationPS");
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
         LEDLabel25 = new LEDParameter("LEDLabel25");
         LEDLabel26 = new LEDParameter("LEDLabel26");
         LEDLabel27 = new LEDParameter("LEDLabel27");
         LEDLabel28 = new LEDParameter("LEDLabel28");
         LEDLabel29 = new LEDParameter("LEDLabel29");
         LEDLabel30 = new LEDParameter("LEDLabel30");
         LEDLabel31 = new LEDParameter("LEDLabel31");
         LEDLabel32 = new LEDParameter("LEDLabel32");

         LEDTracking.ValueChange += new LEDParameter.ValueChangeDelegate(OnPropertyChanged);
         LEDProgramming.ValueChange += new LEDParameter.ValueChangeDelegate(OnPropertyChanged);
         LEDViewInformation.ValueChange += new LEDParameter.ValueChangeDelegate(OnPropertyChanged);
         LEDResetInformation.ValueChange += new LEDParameter.ValueChangeDelegate(OnPropertyChanged);
         LEDStateTireAS.ValueChange += new LEDParameter.ValueChangeDelegate(OnPropertyChanged);
         LEDStateTirePS.ValueChange += new LEDParameter.ValueChangeDelegate(OnPropertyChanged);
         LEDNewInformationAS.ValueChange += new LEDParameter.ValueChangeDelegate(OnPropertyChanged);
         LEDNewInformationPS.ValueChange += new LEDParameter.ValueChangeDelegate(OnPropertyChanged);
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
         LEDLabel25.ValueChange += new LEDParameter.ValueChangeDelegate(OnPropertyChanged);
         LEDLabel26.ValueChange += new LEDParameter.ValueChangeDelegate(OnPropertyChanged);
         LEDLabel27.ValueChange += new LEDParameter.ValueChangeDelegate(OnPropertyChanged);
         LEDLabel28.ValueChange += new LEDParameter.ValueChangeDelegate(OnPropertyChanged);
         LEDLabel29.ValueChange += new LEDParameter.ValueChangeDelegate(OnPropertyChanged);
         LEDLabel30.ValueChange += new LEDParameter.ValueChangeDelegate(OnPropertyChanged);
         LEDLabel31.ValueChange += new LEDParameter.ValueChangeDelegate(OnPropertyChanged);
         LEDLabel32.ValueChange += new LEDParameter.ValueChangeDelegate(OnPropertyChanged);

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

         if (LEDTracking.Bind == _bind) { LEDTracking.Value = _value; return; }
         if (LEDProgramming.Bind == _bind) { LEDProgramming.Value = _value; return; }
         if (LEDViewInformation.Bind == _bind) { LEDViewInformation.Value = _value; return; }
         if (LEDResetInformation.Bind == _bind) { LEDResetInformation.Value = _value; return; }
         if (LEDStateTireAS.Bind == _bind) { LEDStateTireAS.Value = _value; return; }
         if (LEDStateTirePS.Bind == _bind) { LEDStateTirePS.Value = _value; return; }
         if (LEDNewInformationAS.Bind == _bind) { LEDNewInformationAS.Value = _value; return; }
         if (LEDNewInformationPS.Bind == _bind) { LEDNewInformationPS.Value = _value; return; }
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
         if (LEDLabel25.Bind == _bind) { LEDLabel25.Value = _value; return; }
         if (LEDLabel26.Bind == _bind) { LEDLabel26.Value = _value; return; }
         if (LEDLabel27.Bind == _bind) { LEDLabel27.Value = _value; return; }
         if (LEDLabel28.Bind == _bind) { LEDLabel28.Value = _value; return; }
         if (LEDLabel29.Bind == _bind) { LEDLabel29.Value = _value; return; }
         if (LEDLabel30.Bind == _bind) { LEDLabel30.Value = _value; return; }
         if (LEDLabel31.Bind == _bind) { LEDLabel31.Value = _value; return; }
         if (LEDLabel32.Bind == _bind) { LEDLabel32.Value = _value; return; }
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
         OnPropertyChanged("LEDLabel25");
         OnPropertyChanged("LEDLabel26");
         OnPropertyChanged("LEDLabel27");
         OnPropertyChanged("LEDLabel28");
         OnPropertyChanged("LEDLabel29");
         OnPropertyChanged("LEDLabel30");
         OnPropertyChanged("LEDLabel31");
         OnPropertyChanged("LEDLabel32");
      }
      #endregion

      #region Properties
      /// <summary>
      /// Получить параметр сигнала слежение
      /// </summary>
      public LEDParameter LEDTracking { get; private set; }
      /// <summary>
      /// Получить параметр сигнала программирование
      /// </summary>      
      public LEDParameter LEDProgramming { get; private set; }
      /// <summary>
      /// Получить параметр сигнала просмотр информации
      /// </summary>      
      public LEDParameter LEDViewInformation { get; private set; }
      /// <summary>
      /// Получить параметр сигнала сброс информации
      /// </summary>      
      public LEDParameter LEDResetInformation { get; private set; }
      /// <summary>
      /// Получить параметр сигнала состояние шинной АС
      /// </summary>      
      public LEDParameter LEDStateTireAS { get; private set; }
      /// <summary>
      /// Получить параметр сигнала состояние шинной ПС
      /// </summary>      
      public LEDParameter LEDStateTirePS { get; private set; }
      /// <summary>
      /// Получить параметр сигнала новая информация АС
      /// </summary>      
      public LEDParameter LEDNewInformationAS { get; private set; }
      /// <summary>
      /// Получить параметр сигнала новая информация ПС
      /// </summary>      
      public LEDParameter LEDNewInformationPS { get; private set; }
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
      /// <summary>
      /// Получить параметр сигнала Label9
      /// </summary>      
      public LEDParameter LEDLabel9 { get; private set; }
      /// <summary>
      /// Получить параметр сигнала Label0
      /// </summary>      
      public LEDParameter LEDLabel10 { get; private set; }
      /// <summary>
      /// Получить параметр сигнала Label11
      /// </summary>      
      public LEDParameter LEDLabel11 { get; private set; }
      /// <summary>
      /// Получить параметр сигнала Label12
      /// </summary>      
      public LEDParameter LEDLabel12 { get; private set; }
      /// <summary>
      /// Получить параметр сигнала Label13
      /// </summary>      
      public LEDParameter LEDLabel13 { get; private set; }
      /// <summary>
      /// Получить параметр сигнала Label14
      /// </summary>      
      public LEDParameter LEDLabel14 { get; private set; }
      /// <summary>
      /// Получить параметр сигнала Label15
      /// </summary>      
      public LEDParameter LEDLabel15 { get; private set; }
      /// <summary>
      /// Получить параметр сигнала Label16
      /// </summary>      
      public LEDParameter LEDLabel16 { get; private set; }
      /// <summary>
      /// Получить параметр сигнала Label17
      /// </summary>      
      public LEDParameter LEDLabel17 { get; private set; }
      /// <summary>
      /// Получить параметр сигнала Label18
      /// </summary>      
      public LEDParameter LEDLabel18 { get; private set; }
      /// <summary>
      /// Получить параметр сигнала Label19
      /// </summary>      
      public LEDParameter LEDLabel19 { get; private set; }
      /// <summary>
      /// Получить параметр сигнала Label20
      /// </summary>      
      public LEDParameter LEDLabel20 { get; private set; }
      /// <summary>
      /// Получить параметр сигнала Label21
      /// </summary>      
      public LEDParameter LEDLabel21 { get; private set; }
      /// <summary>
      /// Получить параметр сигнала Label22
      /// </summary>      
      public LEDParameter LEDLabel22 { get; private set; }
      /// <summary>
      /// Получить параметр сигнала Label23
      /// </summary>      
      public LEDParameter LEDLabel23 { get; private set; }
      /// <summary>
      /// Получить параметр сигнала Label24
      /// </summary>      
      public LEDParameter LEDLabel24 { get; private set; }
      /// <summary>
      /// Получить параметр сигнала Label25
      /// </summary>      
      public LEDParameter LEDLabel25 { get; private set; }
      /// <summary>
      /// Получить параметр сигнала Label26
      /// </summary>      
      public LEDParameter LEDLabel26 { get; private set; }
      /// <summary>
      /// Получить параметр сигнала Label27
      /// </summary>      
      public LEDParameter LEDLabel27 { get; private set; }
      /// <summary>
      /// Получить параметр сигнала Label28
      /// </summary>      
      public LEDParameter LEDLabel28 { get; private set; }
      /// <summary>
      /// Получить параметр сигнала Label29
      /// </summary>      
      public LEDParameter LEDLabel29 { get; private set; }
      /// <summary>
      /// Получить параметр сигнала Label30
      /// </summary>      
      public LEDParameter LEDLabel30 { get; private set; }
      /// <summary>
      /// Получить параметр сигнала Label31
      /// </summary>      
      public LEDParameter LEDLabel31 { get; private set; }
      /// <summary>
      /// Получить параметр сигнала Label32
      /// </summary>      
      public LEDParameter LEDLabel32 { get; private set; }
      #endregion
   }
}
