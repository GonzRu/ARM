/*#############################################################################
 *    Copyright (C) 2006-2011 Mehanotronika RA                            
 *    All rights reserved.                                                     
 *	~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
 *                                                                             
 *	Описание: класс для отслеживани я состояния 
 *	журнала работы системы
 *                                                                             
 *	Файл                     : X:\Projects\00_DataServer\CommonClasses\LogMonitoring.cs
 *	Тип конечного файла      :                                         
 *	версия ПО для разработки : С#, Framework 4.0                                
 *	Разработчик              : Юров В.И.                                        
 *	Дата начала разработки   : 24.05.2012 
 *	Дата посл. корр-ровки    : xx.хх.201х
 *	Дата (v1.0)              :                                                  
 ******************************************************************************
* Особенности реализации:
 * Используется ...
 *#############################################################################*/


using System;
using System.IO;
using System.Collections.Generic;
using System.Xml.Linq;
using System.Text;

namespace CommonUtils
{
    public class LogMonitoring
    {
	    #region События
	    #endregion

        #region Свойства
        #endregion

        #region public-поля
        #endregion

        #region private-поля
        #endregion

        #region конструктор(ы)
        /// <summary>
        /// создание класса мониторинга лога
        /// </summary>
        /// <param name="textListenerFileName">имя файла лога</param>
        /// <param name="fsizeInKB">максимальный размер файла лога в байтах</param>
        public LogMonitoring(string textListenerFileName, int fmaxsizeInBytes)
        {
            // проверка файла на существование и размер
            if (File.Exists(textListenerFileName))
                VerifyFileSize(textListenerFileName, fmaxsizeInBytes);
        }
        #endregion

        #region Свойства интерфейса Ixxx
        #endregion
        #region public-методы реализации интерфейса Ixxx
        #endregion

        #region public-методы класса
        /// <summary>
        /// удалить файлы по маске *.*
        /// </summary>
        /// <param name="extension"></param>
        public void DeleteFileByMask(string path,string filemask)
        {
            // формирование списка файлов c частями описания устройства
            DirectoryInfo di = new DirectoryInfo(path);
            foreach (FileInfo fi in di.GetFiles(filemask))
                fi.Delete();
        }
        #endregion

        #region protected-методы класса
        #endregion

        #region private-методы класса
        /// <summary>
        /// проверка существования файла и его размера
        /// </summary>
        /// <param name="textListenerFileName"></param>
        /// <param name="fsizeInKB"></param>
        private void VerifyFileSize(string textListenerFileName, int fmaxsizeInBytes)
        {
            // при каждом запуске оставляем прежнюю версию лога и начинаем лог заново
            DeleteFileByMask(Path.GetDirectoryName(textListenerFileName), "*.bak");
            string datar = DateTime.Now.ToString();
            datar = datar.Replace(".", "_");
            datar = datar.Replace(":", "_");
            datar = datar.Replace("/", "_");
            datar = datar.Replace("\\", "_");
            datar = datar.Replace(" ", "_");

            string newfn = Path.GetFileNameWithoutExtension(textListenerFileName) + "_" + datar + "_log.bak";
            File.Move(textListenerFileName, newfn);

            DeleteFileByMask(Path.GetDirectoryName(textListenerFileName), "*.log");

            //return;

            //FileInfo fi = new FileInfo(textListenerFileName);

            //if (fi.Length < fmaxsizeInBytes)
            //    return;

            //dlgAcition4LargeFile dlgact = new dlgAcition4LargeFile();
            //dlgact.FSize = fi.Length;
            //dlgact.ShowDialog();

            //switch (dlgact.DialogResult)
            //{
            //    case System.Windows.Forms.DialogResult.Yes:
            //        DeleteFileByMask(Path.GetDirectoryName(textListenerFileName), "*.log");
            //    break;
            //    case System.Windows.Forms.DialogResult.No:




            //    string datar = DateTime.Now.ToString();
            //    datar = datar.Replace(".", "_");
            //    datar = datar.Replace(":", "_");
            //    datar = datar.Replace("/", "_");
            //    datar = datar.Replace("\\", "_");
            //    datar = datar.Replace(" ", "_");

            //    string newfn = Path.GetFileNameWithoutExtension(textListenerFileName) + "_" + datar +".log";
            //    File.Move(textListenerFileName, newfn);
            //    break;
            //    case System.Windows.Forms.DialogResult.Cancel:
            //    break;
            //    default:
            //    break;
            //}
        }
        #endregion
    }
}
