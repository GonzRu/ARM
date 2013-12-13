using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;

namespace Structure
{
   /// <summary>
   /// Структура точек (линия)
   /// </summary>
   public struct LinePoints
   {
      private PointF stpos;
      private PointF fnpos;
      
      /// <summary>
      /// Получить или задать точку начала линии
      /// </summary>
      public PointF Start
      {
         get { return stpos; }
         set { stpos = value; }
      }
      /// <summary>
      /// Получить или задать точку конца линии
      /// </summary>
      public PointF Finish
      {
         get { return fnpos; }
         set { fnpos = value; }
      }
   }
   /// <summary>
   /// Класс конвертации строк с данными
   /// </summary>
   public static class ConvertMethods
   {
      /// <summary>
      /// Чтение данных о повороте фигуры в ту или иную сторону
      /// </summary>
      /// <param name="_turn">строка поворота</param>
      /// <returns>структурное значение поворота</returns>
      public static DrawRotate GetTurnPosition(string _turn)
      {
         switch (_turn)
         {
            case "Up": return DrawRotate.Up;
            case "Down": return DrawRotate.Down;
            case "Left": return DrawRotate.Left;
            case "Right": return DrawRotate.Right;
            default: return DrawRotate.Down;
         }
      }
      /// <summary>
      /// Получить строку цвета
      /// </summary>
      /// <param name="_clr">Цвет в значениях</param>
      /// <returns>строка</returns>
      public static String GetColorString( Color _clr )
      {
          return string.Format( "{0} {1} {2}", _clr.R, _clr.G, _clr.B );
      }
      /// <summary>
      /// Получение цвета из строки
      /// </summary>
      /// <param name="_str_clr">строка содержащая цвет</param>
      /// <returns>цвет</returns>
      public static Color GetParseColor(string _str_clr)
      {
          if ( string.IsNullOrEmpty( _str_clr ) )
              return Color.Black;

          var strs = _str_clr.Split( ' ' );
          var r = Convert.ToInt32( strs[0] );
          var g = Convert.ToInt32( strs[1] );
          var b = Convert.ToInt32( strs[2] );
          
          return Color.FromArgb(r, g, b);
      }
      /// <summary>
      /// Получить true или false из строки
      /// </summary>
      /// <param name="_str_status">считаная строка</param>
      /// <returns>значение объекта true или false</returns>
      public static Boolean GetBooleanStatus(string _str_status)
      {
         switch ( _str_status.ToLower() )
         {
             case "true": return true;
             default: return false;
         }
      }
      /// <summary>
      /// Получение стиля линии
      /// </summary>
      /// <param name="_str">строка с текстом</param>
      /// <returns>стиль линии</returns>
      public static DashStyle ParseLineStyle(string _str)
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
   /// <summary>
   /// Класс расширений файлов для программы
   /// </summary>
   public static class ProgrammExtensions
   {
      const string Prefix = ".";
      const string Strschema = "mnm";
      const string Str1 = "jpg";
      const string Str2 = "bmp";
      const string Str3 = "png";
      const string Str4 = "emf";
      const string Str5 = "xml";
      const string Any = "|Any files|*.*";

      /// <summary>
      /// Фильтр всех файлов
      /// </summary>
      public static String GetAnyFilesFilter()
      {
         return Any;
      }
      /// <summary>
      /// Фильтр графических файлов
      /// </summary>
      public static String GetImageFilter()
      {  //Image files|*.jpg;*.bmp;*.png;*.wmf
         string str = "Image files";
         str += "|*" + Prefix + Str1;
         str += ";*" + Prefix + Str2;
         str += ";*" + Prefix + Str3;
         str += ";*" + Prefix + Str4;
         return str;
      }
      /// <summary>
      /// Фильтр файла схемы
      /// </summary>
      public static String GetSchemaFilter()
      {  //Schema files|*.mnm|Xml files|*.xml
         return "Schema files|*" + Prefix + Strschema + "|Xml files|*" + Prefix + Str5;
      }
      /// <summary>
      /// Фильтр файла Xml
      /// </summary>
      public static String GetXmlFilter()
      {  //Xml files|*.xml
         return "Xml files|*" + Prefix + Str5;
      }
   }
   /// <summary>
   /// Класс матиматических функций
   /// </summary>
   public static class MathMethods
   {
      /// <summary>
      /// Получить радиус фигуры по координатам
      /// </summary>
      /// <param name="_x">координата X</param>
      /// <param name="_y">координата Y</param>
      /// <returns>радиус</returns>
      public static double GetRadius(int _x, int _y)
      {
         return Math.Sqrt((_x * _x) + (_y * _y));
      }
      /// <summary>
      /// Перевод градусовой меры в радианы
      /// </summary>
      /// <param name="_degrees">градусы</param>
      /// <returns>радианы</returns>
      public static double GetRadians(double _degrees)
      {
         return _degrees * (Math.PI / 180);
      }
      /// <summary>
      /// Перевод радиановой меры в градусы
      /// </summary>
      /// <param name="_radians">радианы</param>
      /// <returns>градусы</returns>
      public static double GetDegrees(double _radians)
      {
         return _radians * 180 / Math.PI;
      }
      /// <summary>
      /// Взять координату X
      /// </summary>
      /// <param name="_radius">радиус фигуры</param>
      /// <param name="_angle">угол</param>
      /// <returns>координата X</returns>
      public static int GetCoordX(double _radius, double _angle)
      {
         return Convert.ToInt32(_radius * Math.Cos(_angle));
      }
      /// <summary>
      /// Взять координату Y
      /// </summary>
      /// <param name="_radius">радиус фигуры</param>
      /// <param name="_angle">угол</param>
      /// <returns>координата Y</returns>
      public static int GetCoordY(double _radius, double _angle)
      {
         return Convert.ToInt32(_radius * Math.Sin(_angle));
      }
   }
   /// <summary>
   /// Класс работы с файлами
   /// </summary>
   public static class WorkFile
   {
       /// <summary>
       /// Проверка на существование файла
       /// </summary>
       /// <param name="fullPath">путь до файла</param>
       /// <returns>true если файл существует</returns>
       public static bool CheckExistFile( string fullPath )
       {
           try
           {
               return new FileInfo( fullPath ).Exists;
           }
           catch
           {
               return false;
           }
       }
       /// <summary>
       /// Проверка действительности пути с проверкой
       /// в корневой дирректории программы
       /// </summary>
       /// <param name="pathValue">Путь до файла</param>
       /// <returns>Действительный путь</returns>
       private static String CheckPathValue( String pathValue )
       {
           var flag = false;

           if ( Path.GetPathRoot( pathValue ) != String.Empty )
               flag = CheckExistFile( pathValue );

           var newpath = pathValue;//если путь действителен, сохраняем его, если нет - тоже сохраняем

           if ( !flag )
           {
               newpath = Environment.CurrentDirectory + pathValue;       //сохраняем и проверяем этот путь
               flag = CheckExistFile( newpath ); //"\\temp\\temp1.jpg"
           }
           if ( !flag )
           {
               newpath = Environment.CurrentDirectory + "\\" + pathValue;//сохраняем и проверяем этот путь
               flag = CheckExistFile( newpath );//"temp\\temp1.jpg"
           }
           if ( !flag )
               newpath = String.Empty;

           return newpath;
       }
       /// <summary>
       /// Удаление подстроки содержащий путь до исполняемого файла программы
       /// </summary>
       /// <param name="fullpath">Полный путь до файла</param>
       /// <returns>Путь до файла без пути до исполняемого файла</returns>
       public static String GetWayPart( String fullpath )
       {
           if ( string.IsNullOrEmpty( fullpath ) )
               return String.Empty;

           //Смотрим есть ли путь до программы в пути до файла
           //если да - то удаляем его, иначе - возвращаем полный путь
           if ( fullpath.Contains( Environment.CurrentDirectory ) )
               return fullpath.Substring( Environment.CurrentDirectory.Length );
           return fullpath;
       }
       /// <summary>
       /// Чтение графического файла
       /// </summary>
       /// <param name="filePath">Путь до файла</param>
       /// <returns>Графический файл</returns>
       public static Image ReadImageFile( String filePath )
       {
           return Image.FromFile( CheckPathValue( filePath ) );
       }
   }
}