using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfDashboards
{
   /// <summary>
   /// Логика взаимодействия для FaceEKRA
   /// </summary>
   public partial class FaceEKRA : BaseFace, IFaceEKRA
   {
      #region Class Methods
      public FaceEKRA()
      {
         LEDFailure = new LEDParameter("LEDFailure");
         LEDExit = new LEDParameter("LEDExit");
         LEDEmpty = new LEDParameter("LEDEmpty");
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

         LEDFailure.ValueChange += new LEDParameter.ValueChangeDelegate(OnPropertyChanged);
         LEDExit.ValueChange += new LEDParameter.ValueChangeDelegate(OnPropertyChanged);
         LEDEmpty.ValueChange += new LEDParameter.ValueChangeDelegate(OnPropertyChanged);
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

         if (LEDFailure.Bind == _bind) { LEDFailure.Value = _value; return; }
         if (LEDExit.Bind == _bind) { LEDExit.Value = _value; return; }
         if (LEDEmpty.Bind == _bind) { LEDEmpty.Value = _value; return; }
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
      }
      /// <summary>
      /// Обновление всех свойств на форме
      /// </summary>
      public override void UpDateLabels()
      {
         //для того, чтоб применились все изменения в конструкторах наследования
         //принудительно сообщаем форме о изменении свойств
         OnPropertyChanged("LEDEmpty");
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
      }
      #endregion

      #region Properties
      /// <summary>
      /// Получить параметер сигнала отказ
      /// </summary>      
      public LEDParameter LEDFailure { get; private set; }
      /// <summary>
      /// Получить параметер сигнала контр. выход
      /// </summary>
      public LEDParameter LEDExit { get; private set; }
      /// <summary>
      /// Получить параметер сигнала пустой ячейки
      /// </summary>
      public LEDParameter LEDEmpty { get; private set; }
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
      #endregion
   }
}
