using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

using Structure;

namespace LibraryElements
{
   /// <summary>
   /// ����� ������
   /// </summary>
   public class FormText : Figure, IFormText
   {
      #region Parameters
      private bool tRotfig;
      private Font tFont;
      private StringFormat tStrform;
      #endregion

      #region Class Methods
      public FormText() : base(1600,1600)
      {
         this.IsSelected = true;
         this.IsModify = true;

         Text = "Unknown";
         this.tFont = new Font("Tahoma", 8);

         this.elem_rec.Width = 65;
         this.elem_rec.Height = 20;
         this.tRotfig = false;
         this.tStrform = new StringFormat();
      }
      /// <summary>
      /// ����������� ����� ��� ������ ������
      /// </summary>
      private void InitRectangle(Graphics g)
      {
         if (this.tRotfig)//������������ ����������� ������
            this.tStrform.FormatFlags = StringFormatFlags.DirectionVertical;
         else              //�������������� ����������� ������
            this.tStrform.FormatFlags = StringFormatFlags.NoClip;

         //������������ �������������� ��� ������ ������
         //(������������� ��������� ����������� ��������������� ���������,
         //������� � "ResizeCollision" ���������� "false"
         SizeF tmp = g.MeasureString(this.Text, this.tFont, 1600, this.tStrform);
         elem_rec.Width = Convert.ToInt32(tmp.Width + 4);
         elem_rec.Height = Convert.ToInt32(tmp.Height + 4);
      }
      /// <summary>
      /// ��������� ������
      /// </summary>
      private void DrawText(Graphics g)
      {
         //���������������
         Matrix mtx = new Matrix();
         mtx.Scale(Scale.X, Scale.Y);
         g.Transform = mtx;

         InitRectangle(g);

         SolidBrush sb = this.CreateViewBrushElement(this.ElementColor);

         if (this.tRotfig)
         { 
            mtx.RotateAt(-90, new PointF(elem_rec.Right, elem_rec.Bottom));
            g.Transform = mtx;
            g.DrawString(this.Text, this.tFont, sb, elem_rec.Right+2, elem_rec.Bottom-elem_rec.Width+2);
         }
         else
            g.DrawString(this.Text, this.tFont, sb, this.elem_rec.X+2, this.elem_rec.Y+2);

         sb.Dispose();

         g.ResetTransform();
      }
      #endregion

      #region Properties
      /// <summary>
       /// �������� ��� ������ �����
       /// </summary>
      public String Text { get; set; }
      /// <summary>
      /// �������� ��� ������ �����
      /// </summary>
      public Font TextFont
      {
         get { return this.tFont; }
         set { this.tFont = value; }
      }
      /// <summary>
      /// �������� ��� ������ ������� ������������ ������ �����������
      /// </summary>
      public bool VerticalView
      {
         get { return this.tRotfig; }
         set { this.tRotfig = value; }
      }
      #endregion

      #region Override Methods
      /// <summary>
      /// �������� ��������� ������� ����
      /// � ������� ���������
      /// </summary>
      /// <param name="pnt">������ ����(X,Y)</param>
      /// <returns>true ���� ������ ����� � ������ ��� ��������� �������</returns>
      public override bool ResizeCollision(Point pnt)
      {
         return false;
      }
      public override Element CopyElement()
      {
          var create = new FormText();
          create.CopyElement( this );
          return create;
      }
      /// <summary>
      /// ����� ���������
      /// </summary>
      public override void DrawElement(Graphics g)
      {  
         this.DrawText(g);

         this.DrawSelected(g);
      }
      /// <summary>
      /// ����������� ��� ������ ������ ����
      /// </summary>
      /// <param name="_factor_x">�������� ��� ���� �� X</param>
      /// <param name="_factor_y">�������� ��� ���� �� Y</param>
      public override void ConverttoNewSize(float _factor_x, float _factor_y)
      {
         base.ConverttoNewSize(_factor_x, _factor_y);

         Font newfont = new Font(this.tFont.Name, this.tFont.SizeInPoints * _factor_x, this.tFont.Style);
         this.tFont = newfont;
      }
      /// <summary>
      /// ����������� ��������
      /// </summary>
      /// <param name="_original">������� �� ������ �������� �������� �����</param>
      public override void CopyElement(Element _original)
      {
         base.CopyElement(_original);
         var txt = (FormText)_original;

         this.Text = txt.Text;
         this.tFont = txt.tFont;
      
         this.tRotfig = txt.tRotfig;
         this.tStrform = txt.tStrform;
      }
      #endregion
   }
}
