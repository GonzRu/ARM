using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;

namespace WpfDashboards
{
   /// <summary>
   /// Базовая логика взаимодействия блока внешнего вида
   /// </summary>
   public class BaseFace : UserControl, IBaseFace, System.ComponentModel.INotifyPropertyChanged
   {
      #region Member
      string model;
      #endregion

      #region Const Members
      const string bmrz = "бмрз";
      const string ekra = "экра";
      const string siriusCS = "сириус-цс";
      const string ovodMD = "овод-мд";
      const string imf1P = "имф-1р";
      const string imf3P = "имф-3р";
      const string siriusOzz = "сириус-озз";
      public const string bmMRCHA = "бммрч-а";
      #endregion

      #region Class Methods
      public BaseFace()
      {
         Model = "Unknown model";

         LEDPower = new LEDParameter("LEDPower");
         LEDPower.ValueChange += new LEDParameter.ValueChangeDelegate(OnPropertyChanged);
      }
      /// <summary>
      /// Установка значения
      /// </summary>
      /// <param name="_guid">GUID номер</param>
      /// <param name="_bind">Имя привязки</param>
      /// <param name="_value">Значение</param>
      public void SetValue(String _guid, String _bind, Boolean _value)
      {
         if (_guid != Guid)
            return;

         SetFirstValue(_bind, _value);
      }
      #endregion

      #region Virtual Methods
      /// <summary>
      /// Установка первичного значения
      /// </summary>
      /// <param name="_bind">Имя привязки</param>
      /// <param name="_value">Значение</param>
      public virtual void SetFirstValue(String _bind, Boolean _value)
      {
         if (LEDPower.Bind == _bind) LEDPower.Value = _value;
      }
      /// <summary>
      /// Обновление всех свойств на форме
      /// </summary>
      public virtual void UpDateLabels() { }
      #endregion

      #region Static Method
      /// <summary>
      /// Создание известного блока
      /// </summary>
      /// <param name="_name">Имя блока целиком</param>
      /// <returns>Интерфейс блока или null</returns>
      public static IBaseFace CreateBlock(String _name)
      {
         BaseFace bf;
         string subName = _name.ToLower();
         string subStr = string.Empty;

         if (subName.Contains("бмрз"))
            subStr = bmrz;
         else if (subName.Contains("бммрч"))
            subStr = bmMRCHA;
         else if (subName.Contains("экра"))
            subStr = ekra;
         else if (subName.Contains("сириус"))
         {
            if (subName.Contains(siriusCS)) subStr = siriusCS;
            if (subName.Contains(siriusOzz)) subStr = siriusOzz;
            if (subName.Contains(imf1P)) subStr = imf1P;
            if (subName.Contains(imf3P)) subStr = imf3P;
         }
         else if (subName.Contains("овод"))
         {
            if (subName.Contains(ovodMD)) subStr = ovodMD;
         }

         switch (subStr)
         {
            case bmMRCHA: bf = new FaceBMMRCH(); break;
            case bmrz: bf = new FaceBMRZ(); break;
            case ekra: bf = new FaceEKRA(); break;
            case ovodMD: bf = new FaceOVODMD(); break;
            case imf1P: bf = new FaceSiriusIMF1R(); break;
            case imf3P: bf = new FaceSiriusIMF3R(); break;
            case siriusCS: bf = new FaceSiriusCS(); break;
            case siriusOzz: bf = new FaceSiriusOZZ(); break;
            default: bf = null; break;
         }//switch

         if (bf != null)
         {
            bf.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
            bf.VerticalAlignment = System.Windows.VerticalAlignment.Top;
         }//if

         return bf;
      }
      #endregion

      #region Properties
      /// <summary>
      /// Получить или задать GUID номер блока
      /// </summary>
      public String Guid
      {
         get;
         set;
      }
      /// <summary>
      /// Получить или задать модель блока
      /// </summary>      
      public String Model
      {
         get { return model; }
         set { model = value; OnPropertyChanged("Model"); }
      }
      /// <summary>
      /// Получить параметер сигнала питание
      /// </summary>      
      public LEDParameter LEDPower
      {
         get;
         private set;
      }
      #endregion

      #region Implementation of INotifyPropertyChanged
      public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
      public void OnPropertyChanged(string propertyName)
      {
         if (PropertyChanged != null)
            PropertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
      }
      #endregion
   }
}
