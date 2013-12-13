using System.Drawing;

using LibraryElements.CalculationBlocks;

using Structure;

namespace LibraryElements
{
   public class Trunk : PolyLine, ICalculationContext
   {
      public Trunk()
      {
         ElementModel = Model.Dynamic;
         IsSelected = true;
      }

      public override Element CopyElement()
      {
          var create = new Trunk();
          create.CopyElement( this );
          return create;
      }
      /// <summary>
      /// ����� ���������
      /// </summary>
      public override void DrawElement(Graphics g)
      {
          if ( CalculationContext != null )
          {
              var res = CalculationContext.Execute( this, g );
              if ( !res ) ElementColor = Color.Gray;
          }
          else ElementColor = Color.Gray;

         base.DrawElement(g); //��������� ������� ������� �����
      }
      /// <summary>
      /// ����������� ��������
      /// </summary>
      /// <param name="_original">������� �� ������ �������� �������� �����</param>
      public override void CopyElement(Element _original)
      {
          base.CopyElement(_original);

          CalculationContext = CalculationContext.GetCopy( ( (ICalculationContext)_original ).CalculationContext );
      }
      /// <summary>
      /// ��������� ������ 
      /// </summary>
      public CalculationContext CalculationContext { get; set; }
   }
}
