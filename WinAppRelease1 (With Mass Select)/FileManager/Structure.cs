using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;

using LibraryElements;

namespace FileManager
{
   /// <summary>
   /// Структура динамических данных для записи
   /// </summary>
   public struct DataTags
   {
      const string name = "Name";                    // имя экземпляра
      const string block_name = "strNameBlock";      // тип блока
      const string symbol = "strRefDesign";          // условное обозначение - для удобства пользователя
      const string num_fk = "nFC";                   // номер ФК
      const string num_section = "nSection";         // номер секции
      const string num_cell = "nLoc";                // номер клетки
      const string num_device = "idDev";             // номер девайса
      const string component_name = "ComponentName"; // имя компонента

      public String TagName
      {
         get { return name; }
      }
      public String TagBlockName
      {
         get { return block_name; }
      }
      public String TagSymbol
      {
         get { return symbol; }
      }
      public String TagNumFK
      {
         get { return num_fk; }
      }
      public String TagNumSection
      {
         get { return num_section; }
      }
      public String TagNumCell
      {
         get { return num_cell; }
      }
      public String TagNumDevice
      {
         get { return num_device; }
      }
      public String TagCompName
      {
         get { return component_name; }
      }
   }
   /// <summary>
   /// Структура данных имен вариантов отображения
   /// </summary>
   public struct Location
   {
      #region Constants
      const string name_prefix = "Положение" + " ";
      const string name0 = "Вниз";
      const string name1 = "Вверх";
      const string name2 = "Влево";
      const string name3 = "Вправо";
      const string name4 = "Вверх-влево";
      const string name5 = "Вверх-вправо";
      const string name6 = "Вниз-влево";
      const string name7 = "Вниз-вправо";

      const string name_stat0 = "Включен";
      const string name_stat1 = "Отключен";
      const string name_stat2 = "Не определен";
      const string name_stat3 = "Нет соединения";
      #endregion

      #region Static Properties
      public String NameDown
      {
         get { return name0; }
      }
      public String NameUp
      {
         get { return name1; }
      }
      public String NameLeft
      {
         get { return name2; }
      }
      public String NameRight
      {
         get { return name3; }
      }
      public String NameUpLeft
      {
         get { return name4; }
      }
      public String NameUpRight
      {
         get { return name5; }
      }
      public String NameDownLeft
      {
         get { return name6; }
      }
      public String NameDownRight
      {
         get { return name7; }
      }
      #endregion

      #region Dinamic Properties
      public String StatOn
      {
         get { return name_stat0; }
      }
      public String StatOff
      {
         get { return name_stat1; }
      }
      public String StatNotDetermined
      {
         get { return name_stat2; }
      }
      public String StatNoConnection
      {
         get { return name_stat3; }
      }
      #endregion

      #region Struct Methods
      /// <summary>
      /// Получить префикс
      /// </summary>
      public String GetPrefix
      {
         get { return name_prefix; }
      }
      /// <summary>
      /// Получить английское имя варианта отображения для записи в файл
      /// </summary>
      /// <param name="_rus_name">имя панели на русском</param>
      /// <returns>имя панели на английском</returns>
      public String GetEnglishName(string _rus_name)
      {
         switch (_rus_name)
         {
            case name0: return DrawRotate.Down.ToString();
            case name1: return DrawRotate.Up.ToString();
            case name2: return DrawRotate.Left.ToString();
            case name3: return DrawRotate.Right.ToString();
            case name4: return DrawRotate.UpLeft.ToString();
            case name5: return DrawRotate.UpRight.ToString();
            case name6: return DrawRotate.DownLeft.ToString();
            case name7: return DrawRotate.DownRight.ToString();

            case name_stat0: return "Status_on";
            case name_stat1: return "Status_off";
            case name_stat2: return "Not_determined";
            case name_stat3: return "No_connection";
            default: return "null";
         }
      }
      /// <summary>
      /// Получить русское имя варианта отображения
      /// </summary>
      /// <param name="_rus_name">имя панели на английском</param>
      /// <returns>имя панели на русском</returns>      
      public String GetRussianName(string _eng_name)
      {
         switch (_eng_name)
         {
            case "Down": return name0;
            case "Up": return name1;
            case "Left": return name2;
            case "Right": return name3;
            case "UpLeft": return name4;
            case "UpRight": return name5;
            case "DownLeft": return name6;
            case "DownRight": return name7;

            case "Status_on": return name_stat0;
            case "Status_off": return name_stat1;
            case "Not_determined": return name_stat2;
            case "No_connection": return name_stat3;
            default: return "null";
         }
      }
      /// <summary>
      /// Получить строку варианта отображения фигуры
      /// </summary>
      /// <param name="_turn">строка варианта поворота</param>
      /// <returns>текст с вариантом поворота фигуры</returns>
      public String GetTurn(string _turn)
      {
         switch (_turn)
         {
            case name_prefix + name0: return DrawRotate.Down.ToString();
            case name_prefix + name1: return DrawRotate.Up.ToString();
            case name_prefix + name2: return DrawRotate.Left.ToString();
            case name_prefix + name3: return DrawRotate.Right.ToString();
            case name_prefix + name4: return DrawRotate.DownLeft.ToString();
            case name_prefix + name5: return DrawRotate.DownRight.ToString();
            case name_prefix + name6: return DrawRotate.UpLeft.ToString();
            case name_prefix + name7: return DrawRotate.UpRight.ToString();
            default: return "null";
         }
      }
      #endregion
   }
   /// <summary>
   /// Структура данных о папках и файлах
   /// использующихся в программе
   /// </summary>
   public struct StreamPath
   {
      const string setting_folder = "\\Settings\\";
      const string custom_folder = "\\Custom elements\\";
      const string setting_file = "Toolbar_custom.xml";
      const string custom_file = "Custom_elements.xml";

      /// <summary>
      /// Получить папку \\Settings\\
      /// </summary>
      public String SettingFolder
      {
         get { return setting_folder; }
      }
      /// <summary>
      /// Получить папку \\Custom elements\\
      /// </summary>
      public String CustomFolder
      {
         get { return custom_folder; }
      }
      /// <summary>
      /// Получить файл Toolbar_custom.xml
      /// </summary>
      public String SettingTbarFile
      {
         get { return setting_file; }
      }
      /// <summary>
      /// Получить файл Custom_elements.xml
      /// </summary>
      public String CustomFile
      {
         get { return custom_file; }
      }
   }
   /// <summary>
   /// Модель создаваемого/читаемого элемента
   /// </summary>
   public enum Model
   {
      Static,
      Dinamic
   }
   /// <summary>
   /// Служебные данные
   /// </summary>
   public struct ModelString
   {
      const string model1 = "static";
      const string model2 = "dinamic";
      const string prefix = "_pic";
      Model model;

      public String StaticModel
      {
         get { return model1; }
      }
      public String DinamicModel
      {
         get { return model2; }
      }
      public String Prefix
      {
         get { return prefix; }
      }
      public Model ElementModel
      {
         get { return model; }
         set { model = value; }
      }
      public Model GetModel(string _str)
      {
         switch (_str)
         {
            case model1: return Model.Static;
            case model2: return Model.Dinamic;
            default: return Model.Static;
         }
      }
      public String GetStrModel(Model _mod)
      {
         switch (_mod)
         {
            case Model.Static: return model1;
            case Model.Dinamic: return model2;
            default: return model1;
         }
      }
   }
   /// <summary>
   /// Структура комплексного хранения данных о элементе
   /// </summary>
   public struct ElemParam
   {
      string name;
      Model model;
      int width;
      int height;

      public String ElemName
      {
         get { return name; }
         set { name = value; }
      }
      public Model ElemModel
      {
         get { return model; }
         set { model = value; }
      }
      public int ElemWidth
      {
         get { return width; }
         set { width = value; }
      }
      public int ElemHeight
      {
         get { return height; }
         set { height = value; }
      }
   }
   /// <summary>
   /// Структура конвертации строк с данными
   /// </summary>
   public struct ConvertMethods
   {
      /// <summary>
      /// Чтение данных о повороте фигуры в ту или иную сторону
      /// </summary>
      /// <param name="_turn">строка поворота</param>
      /// <returns>структурное значение поворота</returns>
      public DrawRotate GetTurnPosition(string _turn)
      {
         switch (_turn)
         {
            case "Up": return DrawRotate.Up;
            case "Down": return DrawRotate.Down;
            case "Left": return DrawRotate.Left;
            case "Right": return DrawRotate.Right;
            case "UpLeft": return DrawRotate.UpLeft;
            case "UpRight": return DrawRotate.UpRight;
            case "DownLeft": return DrawRotate.DownLeft;
            case "DownRight": return DrawRotate.DownRight;
            default: return DrawRotate.Down;
         }
      }
      /// <summary>
      /// Получить строку цвета
      /// </summary>
      /// <param name="_clr">Цвет в значениях</param>
      /// <returns>строка</returns>
      public string GetColorString(Color _clr)
      {
         return (_clr.R.ToString() + " " + _clr.G.ToString() + " " + _clr.B.ToString());
      }
      /// <summary>
      /// Получение цвета из строки
      /// </summary>
      /// <param name="_str_clr">строка содержащая цвет</param>
      /// <returns>цвет</returns>
      public Color GetParseColor(string _str_clr)
      {
         string str = "";
         int r = 0, g = 0, b = 0, num = 0, step = 0;

         if (_str_clr == null) return Color.Black;

         for (int i = 0; i < _str_clr.Length; i++)
         {
            str += _str_clr[i];
            if ((_str_clr.Length == i + 1) || (_str_clr[i + 1] == ' '))
            {
               num = Convert.ToInt32(str);
               str = "";
               switch (step)
               {
                  case 0: r = num; break;
                  case 1: g = num; break;
                  case 2: b = num; break;
               }
               step++;
            }//if
         }//for

         return Color.FromArgb(r, g, b);
      }
      /// <summary>
      /// Получить true или false из строки
      /// </summary>
      /// <param name="_str_status">считаная строка</param>
      /// <returns>значение объекта true или false</returns>
      public bool GetBooleanStatus(string _str_status)
      {
         switch (_str_status)
         {
            case "True":
            case "true": return true;
            default: return false;
         }
      }
      /// <summary>
      /// Получение стиля линии
      /// </summary>
      /// <param name="_str">строка с текстом</param>
      /// <returns>стиль линии</returns>
      public DashStyle ParseLineStyle(string _str)
      {
         switch (_str)
         {
            case "Solid": return DashStyle.Solid;
            case "Dot": return DashStyle.Dot;
            case "DashDotDot": return DashStyle.DashDotDot;
            case "DashDot": return DashStyle.DashDot;
            case "Dash": return DashStyle.Dash;
            case "Custom": return DashStyle.Custom;
            default: return DashStyle.Solid;
         }
      }
   }
}
