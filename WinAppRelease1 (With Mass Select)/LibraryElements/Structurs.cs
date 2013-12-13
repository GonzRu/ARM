using System;
using System.Drawing;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace LibraryElements
{
   #region Structurs
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
   #endregion

   #region Enums
   /// <summary>
   /// ����� ����� �����
   /// </summary>
   public enum SelectPoint
   {
      None,
      Start,
      Finish,
      StartIntermediate,
      FinishIntermediate
   }
   /// <summary>
   /// ������ �����
   /// </summary>
   public enum LineStatus
   {
      None,
      Add,
      Close,
      PointStart,
      PointFinish,
      Intermediate
   }   
   /// <summary>
   /// ����� ��������� �������� ����������� ������
   /// </summary>
   public enum DrawRotate
   {
      Up,
      Down,
      Left,
      Right,

      DownLeft,
      DownRight,
      UpLeft,
      UpRight
   }
   #endregion

   #region SelectMenu
   /// <summary>
   /// ����� ����
   /// </summary>
   public enum SelectMenuItems
   {
      None,
      Custom,
      ItemMenuSingleLine,
      ItemMenuPolyLine,
      ItemMenuGrounding,
      ItemMenuSwitch,
      ItemMenuFloorChassis,
      ItemMenuCapacity,
      ItemMenuZElement,
      ItemMenuUso,
      ItemMenuTrunk,
      ItemMenuTrunkPoint,
      ItemMenuAnimation,
      ItemMenuText,
      ItemMenuRectangle,
      ItemMenuEllipse,
      ItemMenuTriangle,
      ItemMenuKey,
      ItemMenuTransform1,
      ItemMenuTransform2,
      ItemMenuTransform3,
      ItemMenuTransform4,
      ItemMenuTransform5,
      ItemMenuTransform6,
      ItemMenuUnknownElement
   }
   /// <summary>
   /// ����� ����������� ����������� ������
   /// </summary>
   public enum SelectMenuModify
   {
      None,
      Up,
      Down,
      Left,
      Right,

      DownLeft,
      DownRight,
      UpLeft,
      UpRight
   }
   #endregion

   #region Interfaces
   /// <summary>
   /// ��������� ��������� ��������� (�������� ���� ������)
   /// </summary>
   public interface IEditorColor
   {
      Color GetColor();
      void SetColor(Color _clr);
   }
   /// <summary>
   /// ��������� ������������ ��������
   /// </summary>
   public interface IDinamic
   {
      int ControlStatus { get; set; }
      int ProtocolStatus { get; set; }
      Boolean RepairStatus { get; set; }

      String Name { get; set; }
      String Type { get; set; }
      String Symbol { get; set; }
      int FK { get; set; }
      int Section { get; set; }
      int Cell { get; set; }
      int Device { get; set; }
      String Component { get; set; }
      Boolean Orientation { get; set; }
   }
   /// <summary>
   /// ��������� ����� ��������
   /// </summary>
   public interface IFigure
   {
      Rectangle IgetElementLocation();

      void SetPosition(Point _start, Size _size);
   }
   /// <summary>
   /// ��������� �������� � �������� ���������
   /// (�����, ����, �����, ������, �����-�����, �����-������, ����-�����, ����-������)
   /// </summary>
   public interface IRotate
   {
      DrawRotate IgetTurnPosition();
   }
   /// <summary>
   /// ��������� ����� �������
   /// </summary>
   public interface IElementLine
   {
      Color LineColor { get; set; }
      DashStyle LineStyle { get; set; }
      int IgetThickness();
      void IsetThickness(int _num);
   }
   /// <summary>
   /// ��������� ��������� ������ ��������� � �������� ����� ��������� �����
   /// </summary>
   public interface ILine : IElementLine
   {
      String IgetLinePoints();
      void ISetReadPoints(string _parse_str);      
   }
   /// <summary>
   /// ��������� ��������� ������ �� ����� �������� �����
   /// </summary>
   public interface IPolyLine : IElementLine
   {
      String IgetPLPoints();
      void IsetReadPoints(string _parse_str);
   }
   /// <summary>
   /// ��������� ��� ������ � ������ �������
   /// </summary>
   public interface ILineForm : IElementLine
   {
      PointF IgetStartPoint();
      PointF IgetFinishPoint();
      void IsetNewPoints(PointF _start, PointF _finish);
   }
   /// <summary>
   /// ��������� ��� ������ � ������ �������
   /// </summary>
   public interface IPolyLineForm : IElementLine
   {
      List<LinePoints> IgetAllPoints();
   }
   /// <summary>
   /// ��������� ��� ������ � �������
   /// </summary>
   public interface IFormText
   {
      void SetText(string _str);
      
      String Text { get; }
      Font TextFont { get; set; }
      Color TextColor { get; set; }
      bool VerticalView { get; set; }
   }
   /// <summary>
   /// ��������� ��� ������ � ������ �������
   /// </summary>
   public interface ITrunkForm
   {
      Color WithLoading { get; set; }
      Color WithoutLoading { get; set; }
      bool ViewDisign { get; set; }

      void SetTags(string _tags);
      void SetFormula(string _formula);
      void SetTrunkType(string _type);

      String GetTags();
      String GetFormula();
      String GetTrunkType();
   }
   #endregion

   #region Interfaces for Other Program
   /// <summary>
   /// ��������� ������ ���� ������ �� �����
   /// </summary>
   public interface IReadAllElements
   {
      void FileOpen(ref List<Element> _list, ref int _winWidth, ref int _winHeight);
   }
   /// <summary>
   /// ��������� ������ �� ����� ���� ���������
   /// </summary>
   public interface IDrawAllElements
   {
      void DrawElement(Graphics g);
   }
   //��� ������ � ������ ������������ ������ ��������
   //��������������� ����������� "IDinamic"
   #endregion

   #region Form Interfaces
   public interface ILoading
   {
      void SetMaxValue(int _quant);
      void SetPerformStep();
      void SetNewError();
   }
   public interface IErrorLog
   {
      void SetErrorRecord(ListViewItem _error_elem);
   }
   #endregion
}
