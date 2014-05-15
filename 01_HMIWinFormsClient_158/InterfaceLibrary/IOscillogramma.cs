using System;
using System.Collections.Generic;
using System.Text;

namespace InterfaceLibrary
{
    public delegate void OSCReadyHandler(IOscillogramma osc);

    public interface IOscillogramma
    {
        /// <summary>
        /// номер DS
        /// </summary>
        UInt32 DS { get; }
        /// <summary>
        /// Идентификатор блока с осциллограммой 
        /// в таблице DataLog базы данных
        /// </summary>
        UInt32 IdInBD {get;}
        /// <summary>
        /// содержимое осциллограммы
        /// </summary>
        byte[] ContentBlockOsc {get;set;}
        /// <summary>
        /// событие завершения получения осциллограммы
        /// </summary>
        event OSCReadyHandler OnOscReady;
        /// <summary>
        /// возвращает строку с временем в сек
        /// прошедшим с начала формирования осциллограмм(ы)
        /// </summary>
        /// <returns></returns>
        string GetStrTimeReadOSC();
        /// <summary>
        /// осциллограмма получена
        /// </summary>
        void OSC_Received();
    }
}
