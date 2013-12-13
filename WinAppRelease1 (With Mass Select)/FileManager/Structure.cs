using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;

using LibraryElements;

namespace FileManager
{
   /// <summary>
   /// ��������� ������������ ������ ��� ������
   /// </summary>
   public struct DataTags
   {
      const string name = "Name";                    // ��� ����������
      const string block_name = "strNameBlock";      // ��� �����
      const string symbol = "strRefDesign";          // �������� ����������� - ��� �������� ������������
      const string num_fk = "nFC";                   // ����� ��
      const string num_section = "nSection";         // ����� ������
      const string num_cell = "nLoc";                // ����� ������
      const string num_device = "idDev";             // ����� �������
      const string component_name = "ComponentName"; // ��� ����������

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
   /// ��������� ������ ���� ��������� �����������
   /// </summary>
   public struct Location
   {
      #region Constants
      const string name_prefix = "���������" + " ";
      const string name0 = "����";
      const string name1 = "�����";
      const string name2 = "�����";
      const string name3 = "������";
      const string name4 = "�����-�����";
      const string name5 = "�����-������";
      const string name6 = "����-�����";
      const string name7 = "����-������";

      const string name_stat0 = "�������";
      const string name_stat1 = "��������";
      const string name_stat2 = "�� ���������";
      const string name_stat3 = "��� ����������";
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
      /// �������� �������
      /// </summary>
      public String GetPrefix
      {
         get { return name_prefix; }
      }
      /// <summary>
      /// �������� ���������� ��� �������� ����������� ��� ������ � ����
      /// </summary>
      /// <param name="_rus_name">��� ������ �� �������</param>
      /// <returns>��� ������ �� ����������</returns>
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
      /// �������� ������� ��� �������� �����������
      /// </summary>
      /// <param name="_rus_name">��� ������ �� ����������</param>
      /// <returns>��� ������ �� �������</returns>      
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
      /// �������� ������ �������� ����������� ������
      /// </summary>
      /// <param name="_turn">������ �������� ��������</param>
      /// <returns>����� � ��������� �������� ������</returns>
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
   /// ��������� ������ � ������ � ������
   /// �������������� � ���������
   /// </summary>
   public struct StreamPath
   {
      const string setting_folder = "\\Settings\\";
      const string custom_folder = "\\Custom elements\\";
      const string setting_file = "Toolbar_custom.xml";
      const string custom_file = "Custom_elements.xml";

      /// <summary>
      /// �������� ����� \\Settings\\
      /// </summary>
      public String SettingFolder
      {
         get { return setting_folder; }
      }
      /// <summary>
      /// �������� ����� \\Custom elements\\
      /// </summary>
      public String CustomFolder
      {
         get { return custom_folder; }
      }
      /// <summary>
      /// �������� ���� Toolbar_custom.xml
      /// </summary>
      public String SettingTbarFile
      {
         get { return setting_file; }
      }
      /// <summary>
      /// �������� ���� Custom_elements.xml
      /// </summary>
      public String CustomFile
      {
         get { return custom_file; }
      }
   }
   /// <summary>
   /// ������ ������������/��������� ��������
   /// </summary>
   public enum Model
   {
      Static,
      Dinamic
   }
   /// <summary>
   /// ��������� ������
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
   /// ��������� ������������ �������� ������ � ��������
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
   /// ��������� ����������� ����� � �������
   /// </summary>
   public struct ConvertMethods
   {
      /// <summary>
      /// ������ ������ � �������� ������ � �� ��� ���� �������
      /// </summary>
      /// <param name="_turn">������ ��������</param>
      /// <returns>����������� �������� ��������</returns>
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
      /// �������� ������ �����
      /// </summary>
      /// <param name="_clr">���� � ���������</param>
      /// <returns>������</returns>
      public string GetColorString(Color _clr)
      {
         return (_clr.R.ToString() + " " + _clr.G.ToString() + " " + _clr.B.ToString());
      }
      /// <summary>
      /// ��������� ����� �� ������
      /// </summary>
      /// <param name="_str_clr">������ ���������� ����</param>
      /// <returns>����</returns>
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
      /// �������� true ��� false �� ������
      /// </summary>
      /// <param name="_str_status">�������� ������</param>
      /// <returns>�������� ������� true ��� false</returns>
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
      /// ��������� ����� �����
      /// </summary>
      /// <param name="_str">������ � �������</param>
      /// <returns>����� �����</returns>
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
