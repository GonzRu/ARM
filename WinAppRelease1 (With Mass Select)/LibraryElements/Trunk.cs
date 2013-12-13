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
      /// Метод отрисовки
      /// </summary>
      public override void DrawElement(Graphics g)
      {
          if ( CalculationContext != null )
          {
              var res = CalculationContext.Execute( this, g );
              if ( !res ) ElementColor = Color.Gray;
          }
          else ElementColor = Color.Gray;

         base.DrawElement(g); //отрисовка методом ломаной линии
      }
      /// <summary>
      /// Копирование элемента
      /// </summary>
      /// <param name="_original">Элемент на основе которого делается копия</param>
      public override void CopyElement(Element _original)
      {
          base.CopyElement(_original);

          CalculationContext = CalculationContext.GetCopy( ( (ICalculationContext)_original ).CalculationContext );
      }
      /// <summary>
      /// Расчетные данные 
      /// </summary>
      public CalculationContext CalculationContext { get; set; }
   }
}
