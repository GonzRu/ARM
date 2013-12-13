using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;

namespace Structure
{
   /// <summary>
   /// ��������� ����� (�����)
   /// </summary>
   public struct LinePoints
   {
      private PointF stpos;
      private PointF fnpos;
      
      /// <summary>
      /// �������� ��� ������ ����� ������ �����
      /// </summary>
      public PointF Start
      {
         get { return stpos; }
         set { stpos = value; }
      }
      /// <summary>
      /// �������� ��� ������ ����� ����� �����
      /// </summary>
      public PointF Finish
      {
         get { return fnpos; }
         set { fnpos = value; }
      }
   }
   /// <summary>
   /// ����� ����������� ����� � �������
   /// </summary>
   public static class ConvertMethods
   {
      /// <summary>
      /// ������ ������ � �������� ������ � �� ��� ���� �������
      /// </summary>
      /// <param name="_turn">������ ��������</param>
      /// <returns>����������� �������� ��������</returns>
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
      /// �������� ������ �����
      /// </summary>
      /// <param name="_clr">���� � ���������</param>
      /// <returns>������</returns>
      public static String GetColorString( Color _clr )
      {
          return string.Format( "{0} {1} {2}", _clr.R, _clr.G, _clr.B );
      }
      /// <summary>
      /// ��������� ����� �� ������
      /// </summary>
      /// <param name="_str_clr">������ ���������� ����</param>
      /// <returns>����</returns>
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
      /// �������� true ��� false �� ������
      /// </summary>
      /// <param name="_str_status">�������� ������</param>
      /// <returns>�������� ������� true ��� false</returns>
      public static Boolean GetBooleanStatus(string _str_status)
      {
         switch ( _str_status.ToLower() )
         {
             case "true": return true;
             default: return false;
         }
      }
      /// <summary>
      /// ��������� ����� �����
      /// </summary>
      /// <param name="_str">������ � �������</param>
      /// <returns>����� �����</returns>
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
   /// ����� ���������� ������ ��� ���������
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
      /// ������ ���� ������
      /// </summary>
      public static String GetAnyFilesFilter()
      {
         return Any;
      }
      /// <summary>
      /// ������ ����������� ������
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
      /// ������ ����� �����
      /// </summary>
      public static String GetSchemaFilter()
      {  //Schema files|*.mnm|Xml files|*.xml
         return "Schema files|*" + Prefix + Strschema + "|Xml files|*" + Prefix + Str5;
      }
      /// <summary>
      /// ������ ����� Xml
      /// </summary>
      public static String GetXmlFilter()
      {  //Xml files|*.xml
         return "Xml files|*" + Prefix + Str5;
      }
   }
   /// <summary>
   /// ����� �������������� �������
   /// </summary>
   public static class MathMethods
   {
      /// <summary>
      /// �������� ������ ������ �� �����������
      /// </summary>
      /// <param name="_x">���������� X</param>
      /// <param name="_y">���������� Y</param>
      /// <returns>������</returns>
      public static double GetRadius(int _x, int _y)
      {
         return Math.Sqrt((_x * _x) + (_y * _y));
      }
      /// <summary>
      /// ������� ���������� ���� � �������
      /// </summary>
      /// <param name="_degrees">�������</param>
      /// <returns>�������</returns>
      public static double GetRadians(double _degrees)
      {
         return _degrees * (Math.PI / 180);
      }
      /// <summary>
      /// ������� ���������� ���� � �������
      /// </summary>
      /// <param name="_radians">�������</param>
      /// <returns>�������</returns>
      public static double GetDegrees(double _radians)
      {
         return _radians * 180 / Math.PI;
      }
      /// <summary>
      /// ����� ���������� X
      /// </summary>
      /// <param name="_radius">������ ������</param>
      /// <param name="_angle">����</param>
      /// <returns>���������� X</returns>
      public static int GetCoordX(double _radius, double _angle)
      {
         return Convert.ToInt32(_radius * Math.Cos(_angle));
      }
      /// <summary>
      /// ����� ���������� Y
      /// </summary>
      /// <param name="_radius">������ ������</param>
      /// <param name="_angle">����</param>
      /// <returns>���������� Y</returns>
      public static int GetCoordY(double _radius, double _angle)
      {
         return Convert.ToInt32(_radius * Math.Sin(_angle));
      }
   }
   /// <summary>
   /// ����� ������ � �������
   /// </summary>
   public static class WorkFile
   {
       /// <summary>
       /// �������� �� ������������� �����
       /// </summary>
       /// <param name="fullPath">���� �� �����</param>
       /// <returns>true ���� ���� ����������</returns>
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
       /// �������� ���������������� ���� � ���������
       /// � �������� ����������� ���������
       /// </summary>
       /// <param name="pathValue">���� �� �����</param>
       /// <returns>�������������� ����</returns>
       private static String CheckPathValue( String pathValue )
       {
           var flag = false;

           if ( Path.GetPathRoot( pathValue ) != String.Empty )
               flag = CheckExistFile( pathValue );

           var newpath = pathValue;//���� ���� ������������, ��������� ���, ���� ��� - ���� ���������

           if ( !flag )
           {
               newpath = Environment.CurrentDirectory + pathValue;       //��������� � ��������� ���� ����
               flag = CheckExistFile( newpath ); //"\\temp\\temp1.jpg"
           }
           if ( !flag )
           {
               newpath = Environment.CurrentDirectory + "\\" + pathValue;//��������� � ��������� ���� ����
               flag = CheckExistFile( newpath );//"temp\\temp1.jpg"
           }
           if ( !flag )
               newpath = String.Empty;

           return newpath;
       }
       /// <summary>
       /// �������� ��������� ���������� ���� �� ������������ ����� ���������
       /// </summary>
       /// <param name="fullpath">������ ���� �� �����</param>
       /// <returns>���� �� ����� ��� ���� �� ������������ �����</returns>
       public static String GetWayPart( String fullpath )
       {
           if ( string.IsNullOrEmpty( fullpath ) )
               return String.Empty;

           //������� ���� �� ���� �� ��������� � ���� �� �����
           //���� �� - �� ������� ���, ����� - ���������� ������ ����
           if ( fullpath.Contains( Environment.CurrentDirectory ) )
               return fullpath.Substring( Environment.CurrentDirectory.Length );
           return fullpath;
       }
       /// <summary>
       /// ������ ������������ �����
       /// </summary>
       /// <param name="filePath">���� �� �����</param>
       /// <returns>����������� ����</returns>
       public static Image ReadImageFile( String filePath )
       {
           return Image.FromFile( CheckPathValue( filePath ) );
       }
   }
}