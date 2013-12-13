using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using InterfaceLibrary;

namespace SourceMOA
{
    public class MOASpecificDeviceValue : ISpecificDeviceValue
    {
        /// <summary>
        /// список параметров и значений
        /// string - имя параметра
        /// string - значение
        /// </summary>
        private SortedList<string,string> slNameValueParameters = new SortedList<string,string>();
        /// <summary>
        /// список адресов пакетов уставок и параметров для них
        /// </summary>
        private SortedList<UInt32, object> slUstavkiParameters4Address = new SortedList<uint,object>();
        /// <summary>
        /// получить значение специфич параметра 
        /// устройства по имени параметра
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public string GetValueByName(string name)
        {
            try
            {
                if (!slNameValueParameters.ContainsKey(name))
                    throw new Exception(string.Format("(426) : Configuration.cs : GetTypeBlockArchivData() : Параметр {0} отсутсвует в списке.", name));
            }
            catch (Exception ex)
            {
                TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG(ex);
            }

            return slNameValueParameters[name];
        }
        /// <summary>
        /// установить значение специфич параметра 
        /// устройства по имени параметра
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public void SetValueByName(string name, string value)
        {
            try
            {
                if (slNameValueParameters.ContainsKey(name))
                    slNameValueParameters[name] = value;
                else
                    slNameValueParameters.Add(name, value);
            }
            catch (Exception ex)
            {
                TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG(ex);
            }
        }
        /// <summary>
        /// получить список адресов блоков уставок и доп параметров к ним
        /// </summary>
        /// <returns></returns>
        public SortedList<UInt32, object> GetListUstavkiAddresses()
        {
            return slUstavkiParameters4Address;
        }
        /// <summary>
        /// установить список адресов блоков уставок и доп параметров к ним
        /// </summary>
        /// <returns></returns>
        public void AddBlockAdress4Ustavki(uint adr, object specificparameters)
        {
            if (slUstavkiParameters4Address.ContainsKey(adr))
                slUstavkiParameters4Address[adr] = specificparameters;
            else
                slUstavkiParameters4Address.Add(adr,specificparameters);
        }
    }
}
