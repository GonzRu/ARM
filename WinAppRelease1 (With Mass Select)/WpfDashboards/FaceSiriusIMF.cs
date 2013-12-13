using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WpfDashboards
{
   /// <summary>
   /// Логика взаимодействия для SiriusIMF1R
   /// </summary>
   public class FaceSiriusIMF1R : FaceSiriusIMF
   {
      public FaceSiriusIMF1R()
      {
         this.title1.Content = "Сириус ИМФ-1Р";
      }
   }
   /// <summary>
   /// Логика взаимодействия для SiriusIMF3R
   /// </summary>
   public class FaceSiriusIMF3R : FaceSiriusIMF
   {
      public FaceSiriusIMF3R()
      {
         this.title1.Content = "Сириус ИМФ-3Р";
      }
   }
}
