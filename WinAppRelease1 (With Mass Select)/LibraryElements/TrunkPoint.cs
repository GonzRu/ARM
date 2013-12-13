using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace LibraryElements
{
   public class TrunkPoint : Figure
   {
      #region Class Methods
      public TrunkPoint()
      {
         this.IsSelected = true;
         this.IsModify = true;
         this.elem_rec.Width = 10;
         this.elem_rec.Height = 10;
      }
      /// <summary>
      /// ����� ���������
      /// </summary>
      private void DrawMethod(Graphics g)
      {
         using (LinearGradientBrush tmp = new LinearGradientBrush(elem_rec, Color.White, Color.Gray, LinearGradientMode.Vertical))
         {
            g.FillEllipse(tmp, elem_rec);
         }
         Pen pn = CreateViewPenElement(this.ElementColor);
         g.DrawEllipse(pn, elem_rec);
         pn.Dispose();
      }
      #endregion

      #region Override Methods
      public override Element CopyElement()
      {
          var create = new TrunkPoint();
          create.CopyElement( this );
          return create;
      }
      /// <summary>
      /// ����� ���������
      /// </summary>
      public override void DrawElement(Graphics g)
      {
         this.DrawMethod(g);

         DrawSelected(g);         
      }
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
      /// <summary>
      /// ����������� ��� ������ ������ ����
      /// </summary>
      /// <param name="_factor_x">�������� ��� ���� �� X</param>
      /// <param name="_factor_y">�������� ��� ���� �� Y</param>
      public override void ConverttoNewSize(float _factor_x, float _factor_y)
      {
         //�������� ���������� ������� � �������� ��� ������
         int w = elem_rec.Width;
         int h = elem_rec.Height;

         base.ConverttoNewSize(_factor_x, _factor_y);

         elem_rec.Width = w;
         elem_rec.Height = h;
         elem_rec.X -= w / 4;
         elem_rec.Y -= h / 4;
      }
      #endregion
   }
}
