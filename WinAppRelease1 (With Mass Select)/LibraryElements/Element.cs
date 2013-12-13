using System;
using System.Drawing;

using Structure;

namespace LibraryElements
{
   /// <summary>
   /// ����� �������� ��'�����
   /// </summary>
   public abstract class Element
   {
      #region Class Parameters
      public static Color BackGroundColor = Color.FromArgb( 192, 192, 192 );
      /// <summary>
      /// ��� ��������
      /// </summary>
      private string elem_name;
      /// <summary>
      /// �������� "true" ���� ������ ��������� �� �����
      /// </summary>
      private bool elem_dragged;
      /// <summary>
      /// �������� "true" ���� ������ ������ ���� ������
      /// </summary>
      private bool elem_modify;
      /// <summary>
      /// �������� "true" ���� ������ ��������
      /// </summary>
      private bool elem_selected;
      /// <summary>
      /// �������� true - ���� ������� ����� �������
      /// </summary>
      private bool elem_enable_rotate;
      /// <summary>
      /// ������� �������� (��� ����������� �����)
      /// </summary>
      private int elem_level;
      /// <summary>
      /// ������� ������
      /// (100% - 1.0f, 50% - 0.5f)
      /// </summary>
      private PointF elem_scale;
      /// <summary>
      /// ������ ��������
      /// </summary>
      private Model model;
      /// <summary>
      /// ����
      /// </summary>
      private Color elem_color;
      /// <summary>
      /// �������� ������������ ����
      /// </summary>
      protected Point elem_mouseoffset;
      #endregion

      #region Abstract Method
      public abstract Element CopyElement();
      /// <summary>
      /// �������� ��������
      /// </summary>
      /// <param name="_shift_x">�������� �� x</param>
      /// <param name="_shift_y">�������� �� y</param>
      public abstract void MoveElementtoShift( int _shift_x, int _shift_y );
      #endregion

      #region Virtual methods
      /// <summary>
      /// ����� ���������
      /// </summary>
      public virtual void DrawElement(Graphics g) { }
      /// <summary>
      /// ������ �������� ��������
      /// ������������ ����
      /// </summary>
      public virtual void MouseOffSet(Point _pnt) { }
      /// <summary>
      /// ����� ��������� ��������� ��������
      /// </summary>
      protected virtual void DrawSelected(Graphics _g) { }
      /// <summary>
      /// ����������� ��������
      /// </summary>
      /// <param name="_original">������� �� ������ �������� �������� �����</param>
      public virtual void CopyElement(Element _original)
      {
         elem_name = _original.elem_name;
         elem_enable_rotate = _original.elem_enable_rotate;
         elem_scale = _original.elem_scale;
         elem_color = _original.elem_color;

         elem_selected = false;
         elem_modify = false;
      }
      /// <summary>
      /// �������� ��������� ���� �� ��������
      /// </summary>
      /// <param name="_pnt">������ ����(X,Y)</param>
      /// <returns>true ���� ������ �� ��������</returns>
      public virtual bool Collision(Rectangle _pnt) { return false; }
      /// <summary>
      /// �������� ��������� ������� ����
      /// � ������� ���������
      /// </summary>
      /// <param name="pnt">������ ����(X,Y)</param>
      /// <returns>true ���� ������ ����� � ������ ��� ��������� �������</returns>
      public virtual bool ResizeCollision(Point pnt) { return false; }
      /// <summary>
      /// ����������� ��� ������ ������ ����
      /// </summary>
      /// <param name="_factor_x">�������� ��� ���� �� X</param>
      /// <param name="_factor_y">�������� ��� ���� �� Y</param>
      public virtual void ConverttoNewSize(float _factor_x, float _factor_y) { }
      #endregion

      #region General methods
      /// <summary>
      /// �������� ��� �������� (������� �������� ���� protected)
      /// </summary>
      public String ElementName
      {
         get { return elem_name; }
         protected set { elem_name = value; }
      }
      /// <summary>
      /// �������� ��� ������ ����� ����������� ��������
      /// </summary>
      public Boolean IsModify
      {
         get { return elem_modify; }
         set { elem_modify = value; }
      }
      /// <summary>
      /// �������� ��� ������ ����� �������������� ��������
      /// </summary>
      public Boolean IsDragged
      {
         get { return elem_dragged; }
         set { elem_dragged = value; }
      }
      /// <summary>
      /// �������� ��� ������ ����� ������ ��������
      /// </summary>
      public Boolean IsSelected
      {
         get { return elem_selected; }
         set { elem_selected = value; }
      }
      /// <summary>
      /// �������� ��� ������ ������� ������
      /// ��������: �� 0.1 �� 1.0
      /// </summary>
      public PointF Scale
      {
         get { return elem_scale; }
         set { elem_scale = value; }
      }
      /// <summary>
      /// �������� ������� ������������ ��������
      /// ��������: �� 0 �� N (������� �������� ���� protected)
      /// </summary>
      public int Level
      {
         get { return elem_level; }
         protected set { elem_level = value; }
      }
      /// <summary>
      /// �������� ������ �������� (������� �������� ���� protected)
      /// </summary>
      public Model ElementModel
      {
         get { return model; }
         protected set { model = value; }
      }
      /// <summary>
      /// �������� ��� ������ ����
      /// </summary>
      public Color ElementColor
      {
         get { return elem_color; }
         set { elem_color = value; }
      }
      #endregion
   }
}
