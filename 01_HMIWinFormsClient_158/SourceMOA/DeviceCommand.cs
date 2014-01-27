/*#############################################################################
 *    Copyright (C) 2006-2011 Mehanotronika RA                            
 *    All rights reserved.                                                     
 *	~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
 *                                                                             
 *	Описание: Класс команды устройства
 *                                                                             
 *	Файл                     : X:\Projects\01_HMIWinFormsClient\SourceMOA\DeviceCommand.cs
 *	Тип конечного файла      :                                         
 *	версия ПО для разработки : С#, Framework 4.0                                
 *	Разработчик              : Юров В.И.                                        
 *	Дата начала разработки   : 15.03.2012
 *	Дата посл. корр-ровки    : xx.хх.201х
 *	Дата (v1.0)              :                                                  
 ******************************************************************************
* Особенности реализации:
 * Используется ...
 *#############################################################################*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using InterfaceLibrary;
using System.Xml.Linq;

namespace SourceMOA
{
    public class Parameter : IParameter
    {
        private string _name;
        private UInt32 _value;

        /// <summary>
        /// Parameter name
        /// </summary>
        public string Name { get { return _name; } }

        /// <summary>
        /// Parameter value
        /// </summary>
        public UInt32 Value { get { return _value; } }

        /// <summary>
        /// Init Parameter from XML element
        /// </summary>
        /// <param name="xeinit"></param>
        public void Init(XElement xeinit)
        {
            _name = xeinit.Attribute("name").Value;
            _value = UInt32.Parse(xeinit.Attribute("value").Value);
        }
    }

    public class DeviceCommand : IDeviceCommand
    {
        private string _IECAddress;
        private List<IParameter> _parametersList;

        /// <summary>
        /// краткое имя команды
        /// </summary>
        public string CmdName { get {return cmdName;} }
        string cmdName = string.Empty;
        /// <summary>
        /// диспетчерское имя команды
        /// для использования в HMI
        /// </summary>
        public string CmdDispatcherName { get{return cmdDispatcherName;} }
        string cmdDispatcherName = string.Empty;
        /// <summary>
        /// инициализация команды
        /// на основе содержимого 
        /// xml-секции команды в файле
        /// описания устройства
        /// </summary>
        /// <param name="xeinit"></param>
        public void Init(XElement xeinit)
        {
            cmdName = xeinit.Attribute("name").Value;
            cmdDispatcherName = xeinit.Element("CMDDescription").Value;

            var IECAddressXElement = xeinit.Element("IECAddress");
            if (IECAddressXElement != null)
                _IECAddress = IECAddressXElement.Value;

            var parametersXElemet = xeinit.Element("Parameters");
            if (parametersXElemet != null)
            {
                _parametersList = new List<IParameter>();
                foreach (var parameter in parametersXElemet.Elements("Parameter"))
                {
                    Parameter p = new Parameter();
                    p.Init(parameter);

                    _parametersList.Add(p);
                }
            }
        }

        #region New parameters for telemechanica
        /// <summary>
        /// Command address
        /// </summary>
        public string IECAddress { get { return _IECAddress; } }

        /// <summary>
        /// Command parameters list
        /// </summary>
        public List<IParameter> Parameters { get { return _parametersList; } }
        #endregion
    }
}
