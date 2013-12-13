using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace HMI_MT
{
   // NOTE: If you change the class Name "TimeServer" here, you must also update the reference to "TimeServer" in App.config.
   [ServiceBehavior( InstanceContextMode = InstanceContextMode.Single )]
   public class StateServer : ITimeServer
   {
      MainForm mainform;
      public  StateServer( MainForm mf)
      {
         mainform = mf;
      }

      public DateTime GetServerTime( )
      {
          // заглушка  
          return DateTime.MinValue;// mainform.newKB.CRZATimeFC;
      }
   }
}
