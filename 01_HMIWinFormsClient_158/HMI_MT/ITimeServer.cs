using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace HMI_MT
{
   // NOTE: If you change the interface Name "ITimeServer" here, you must also update the reference to "ITimeServer" in App.config.
    [ServiceContract]//(SessionMode=SessionMode.Required)
   public interface ITimeServer
   {
      [OperationContract]
      DateTime GetServerTime();
   }   
}
