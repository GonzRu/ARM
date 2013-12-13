using System;
using System.Collections.Generic;
using System.Text;

namespace Calculator
{
   class MTRAMathLib
   {
   }

   public class UgolIA
   {
      /// <summary>
      /// вычисление угла тока и напряжения для блоков Сириус
      /// </summary>
      /// <param Name="Uay">проекция базовкого угла Ua на ось Y</param>
      /// <param Name="Uax">проекция базовкого угла Ua на ось X</param>
      /// <param Name="Yy">проекция тока/напряжения на ось Y</param>
      /// <param Name="Xx">проекция тока/напряжения на ось X</param>
      /// <returns></returns>
      public static double Evaluate( object Uay, object Uax, object Yy, object Xx )
      {
         /*
          * расчет базового вектора Ua
          * ugol_b = arctg(Uay, Uax)
          */

         double ugol_b = Math.Atan2( Convert.ToDouble( Uay ), Convert.ToDouble( Uax ) );
         double at = Math.Atan2(Convert.ToDouble(Yy), Convert.ToDouble(Xx));
         double rez_radians =  at - ugol_b; // результат в радианах
         double rez_angle = rez_radians * ( 180 / Math.PI );

         // корректировка результата
         if( rez_angle > 180 )
            rez_angle -= 360;
         if( rez_angle < -180 )
            rez_angle += 360;

         return rez_angle;
      }
   }
}
