using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Structure
{
   #region Program Work Interfaces
   /// <summary>
   /// ��������� ��������� ��������� (�������� ���� ������)
   /// </summary>
   public interface IEditorColor
   {
      Color GetUpColor();
      Color GetDownColor();

      void SetColor(Color up, Color down);
   }
   /// <summary>
   /// ��������� ����� �������
   /// </summary>
   public interface IElementLine
   {
      Color ElementColor { get; set; }
      DashStyle LineStyle { get; set; }
      int Thickness { get; set; }
      String GetPoints();
      void SetReadPoints(string _parse_str);
   }
   /// <summary>
   /// ������� ��������� ��� ������ � �������
   /// </summary>
   public interface IBaseFormText
   {
       String Text { get; set; }
   }
   /// <summary>
   /// ��������� ��� ������ � �������
   /// </summary>
   public interface IFormText : IBaseFormText
   {
      Font TextFont { get; set; }
      Color ElementColor { get; set; }
      bool VerticalView { get; set; }
   }
   /// <summary>
   /// ��������� ����� �������� ��� ���������� ������
   /// </summary>
   public interface IProcess
   {
      void SetMaxValue(int _quant);
      void SetPerformStep();
      void SetNewError();
   }
   /// <summary>
   /// ��������� ����� ������ ������
   /// </summary>
   public interface IErrorLog
   {
      void SetErrorRecord(TreeNode _error_node);
   }
   #endregion
}
