using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using InterfaceLibrary;

namespace RequsEtntryLib
{
    public class RequestData : IRequestData
    {
        /// <summary>
        /// номер DS
        /// </summary>
        public uint DS
         { get{return dS;} }
        uint dS = 0xffffffff;
        /// <summary>
        /// уник номер устройства
        /// </summary>
        public uint ObjUni { get{return objUni;} }
        uint objUni = 0xffffffff;
        /// <summary>
        /// комментарий к запросу
        /// </summary>
        public string Comment { get{return comment;} }
        string comment = string.Empty;
        /// <summary>
        /// параметры команды
        /// </summary>
        public ArrayList ReqParams { get{return reqParams;} }
        ArrayList reqParams = new ArrayList();
    }
}
