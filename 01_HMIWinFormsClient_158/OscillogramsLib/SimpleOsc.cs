using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using InterfaceLibrary;

namespace OscillogramsLib
{
    public class SimpleOsc : IOscillogramma
    {
        /// <summary>
        /// класс для замера времени
        /// </summary>
        Stopwatch stopwatch;

        /// <summary>
        /// событие завершения получения осциллограммы
        /// </summary>
        public event OSCReadyHandler OnOscReady;

        /// <summary>
        /// номер DS
        /// </summary>
        public UInt32 DS { get{return ds;} }
        UInt32 ds = 0xFFFFFFFF; 
        /// <summary>
        /// Идентификатор блока с осциллограммой 
        /// в таблице DataLog базы данных
        /// </summary>
        public UInt32 IdInBD 
        { get {return idInBD;} }
        UInt32 idInBD = 0;

        /// <summary>
        /// содержимое осциллограммы
        /// </summary>
        public byte[] ContentBlockOsc { get{return contentBlockOsc;} set{contentBlockOsc = value;} }
        byte[] contentBlockOsc = new byte[]{} ;
        
        public SimpleOsc(UInt32 ds, UInt32 identInBD)
        {
            this.ds = ds;
            idInBD = identInBD;
            stopwatch = new Stopwatch();
            stopwatch.Start();
        }

        /// <summary>
        /// возвращает строку с временем в сек
        /// прошедшим с начала формирования осциллограмм(ы)
        /// </summary>
        /// <returns></returns>
        public string GetStrTimeReadOSC()
        {
            return Convert.ToString(stopwatch.ElapsedTicks / Stopwatch.Frequency);
        }
        /// <summary>
        /// осциллограмма получена
        /// </summary>
        public void OSC_Received()
        {
            if (OnOscReady != null)
                OnOscReady(this);
        }
    }
}
