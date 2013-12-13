/*#############################################################################
 *    Copyright (C) 2006-2011 Mehanotronika RA                            
 *    All rights reserved.                                                     
 *	~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
 *                                                                             
 *	Описание: Компонент - формирователь пакетов
 *                                                                             
 *	Файл                     : X:\Projects\38_DS4BlockingPrg\ClientWPF\BlockDataComposer\OrdinalBlokDataComposer.cs            
 *	Тип конечного файла      :                                         
 *	версия ПО для разработки : С#, Framework 4.0                                
 *	Разработчик              : Юров В.И.                                        
 *	Дата начала разработки   : 07.02.2011 
 *	Дата посл. корр-ровки    : xx.хх.201х
 *	Дата (v1.0)              :                                                  
 ******************************************************************************
* Особенности реализации:
 * Используется ...
 *#############################################################################*/

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using ProviderCustomerExchangeLib;
using  InterfaceLibrary;

namespace BlockDataComposer
{
	
	public class OrdinalBlokDataComposer : IBlockDataComposer
	{
		#region События
		#endregion

		#region Свойства
		#endregion

		#region public
		#endregion

		#region private
		/// <summary>
		/// ссылка на компонент взаимодействия
		/// </summary>
		IProviderCustomer PROVCUST;
		#endregion

		#region конструктор(ы)
		public OrdinalBlokDataComposer(IProviderCustomer provCust)
		{ 
			PROVCUST = provCust;
		}
		#endregion
					
		#region public-методы реализации интерфейса xxx
		/// <summary>
		/// сформировать и послать пакет запроса значений тегов
		/// </summary>
		/// <param name="ht"></param>
		public void FormAndSaveReqPacket(Hashtable ht)
		{ 
			// для хранения тела запроса, чтобы вычислить длину
			MemoryStream msdatareq = new MemoryStream();
			BinaryWriter bwdatareq = new BinaryWriter(msdatareq);

			try
			{
		        foreach (DictionaryEntry de in ht)
				{
					// формируем тело требования
					string[] strreq = ((string)(de.Key)).Split(new char[]{'.'});

					UInt16 UniDsGuid = Convert.ToUInt16(strreq[0]);
					bwdatareq.Write(UniDsGuid);
					// уник номер объекта в пределах конкретного DataServer
					UInt32 LocObjectGuid = Convert.ToUInt32(strreq[1]);
					bwdatareq.Write(LocObjectGuid);
					// уник номер группы-подгруппы
					UInt16 grGuid = 0;
					bwdatareq.Write(grGuid);
					// уник номера тегов
					UInt32 tag1 = Convert.ToUInt32(strreq[2]);
					bwdatareq.Write(tag1);
		        }

				PROVCUST.SendData(FormPacket(TYPEOFPACKET.RequestData, msdatareq));
			}
			catch(Exception ex)
			{
				TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG(ex );
			}
		}
        /// <summary>
        /// сформировать и послать пакет запроса значений тегов
        /// из конкретного банка значений - ацп-перв-втор
        /// </summary>
        /// <param name="ht"></param>
        public void FormAndSaveSpecificBankReqPacket(Hashtable ht)
        {
            // для хранения тела запроса, чтобы вычислить длину
            MemoryStream msdatareq = new MemoryStream();
            BinaryWriter bwdatareq = new BinaryWriter(msdatareq);
            IDevice dev = null;

            try
            {
                foreach (DictionaryEntry de in ht)
                {
                    // формируем тело требования
                    string[] strreq = ((string)(de.Key)).Split(new char[] { '.' });
                    if (Convert.ToUInt32(strreq[2]) == 0)
                        continue;

                    UInt16 UniDsGuid = Convert.ToUInt16(strreq[0]);
                    bwdatareq.Write(UniDsGuid);
                    // уник номер объекта в пределах конкретного DataServer
                    UInt32 LocObjectGuid = Convert.ToUInt32(strreq[1]);
                    bwdatareq.Write(LocObjectGuid);
                    if (LocObjectGuid == 268)
                        LocObjectGuid = 268;
                    dev = HMI_MT_Settings.HMI_Settings.CONFIGURATION.GetLink2Device(UniDsGuid, LocObjectGuid);
                    if (dev == null)
                        throw new Exception(string.Format("(121) : OrdinalBlokDataComposer.cs : FormAndSaveSpecificBankReqPacket() : запрос тегов несуществующего устройства UniDsGuid = {0}; LocObjectGuid = {1}", UniDsGuid.ToString(), LocObjectGuid.ToString()));
                    InterfaceLibrary.TypeViewTag typeviewtag = dev.TypeTagPriorityView;
                    byte btypeviewtag = (byte) typeviewtag;
                    bwdatareq.Write(btypeviewtag);

                    //if (btypeviewtag != 0)
                    //    btypeviewtag = 0;

                    // уник номер группы-подгруппы
                    UInt16 grGuid = 0;
                    bwdatareq.Write(grGuid);
                    // уник номера тегов
                    UInt32 tag1 = Convert.ToUInt32(strreq[2]);
                    bwdatareq.Write(tag1);
                }

                PROVCUST.SendData(FormPacket(TYPEOFPACKET.RequestSpecificBankData, msdatareq));
            }
            catch (Exception ex)
            {
                TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG(ex);
            }
        }

        /// <summary>
        /// сформировать и послать пакет команды ()
        /// </summary>
        /// <param name="ht"></param>
        public void FormAndSendCMDPacket(ICommand cmd)
        {
            // поток для формирования общего запроса
            MemoryStream msfullreq = new MemoryStream();
            BinaryWriter bwfullreq = new BinaryWriter(msfullreq);
            // для хранения тела запроса, чтобы вычислить длину
            MemoryStream msdatareq = new MemoryStream();
            BinaryWriter bwdatareq = new BinaryWriter(msdatareq);

            try
            {
                byte reqtype = (byte)TYPEOFPACKET.CMD;
                bwfullreq.Write(reqtype);

                /*
                 * идентификатор корреляции:
                 * 0 - не нужно следить за тем пришел 
                 *		ответ на запрос или нет
                 */
                UInt16 id_correlation = 0;
                bwfullreq.Write(id_correlation);

                UInt16 UniDsGuid = Convert.ToUInt16(cmd.DS);
                bwfullreq.Write(UniDsGuid);
                // уник номер объекта в пределах конкретного DataServer
                UInt32 LocObjectGuid = Convert.ToUInt32(cmd.ObjUni);
                bwfullreq.Write(LocObjectGuid);

                /*
                 * далее формируем сам запрос
                 * в отдельном потоке, чтобы 
                 * вычислить его длину и поместить 
                 * в tagGuidLen
                 */
                // сворачиваем идентификатор (имя) команды
                byte[] bytenamecmd = Encoding.UTF8.GetBytes(cmd.CmdName);

                // длина имени команды и само имя в поток
                UInt16 cmdnamelen = Convert.ToUInt16(bytenamecmd.Length);
                bwdatareq.Write(cmdnamelen);
                bwdatareq.Write(bytenamecmd,0,bytenamecmd.Length);	// имя команды

                // если у команды есть параметры, то их в поток, если их нет то в поток записать 0
                UInt16 cmdparamslen = Convert.ToUInt16(cmd.Alparams.Length);
                bwdatareq.Write(cmdparamslen);
                bwdatareq.Write(cmd.Alparams,0,cmd.Alparams.Length);	// параметры команды

                // длину тела в основной поток
                //UInt16 reqlen = Convert.ToUInt16(bwdatareq.BaseStream.Length);
                //bwfullreq.Write(reqlen);

                bwdatareq.BaseStream.Position = 0;

                bwfullreq.Write(msdatareq.ToArray());

                //bwdatareq.Write(cmdnamelen);

                PROVCUST.SendRunCMD(msfullreq.ToArray());
            }
            catch (Exception ex)
            {
                TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG(ex);
            }
        }
        /// <summary>
        /// сформировать и послать запрос на архивные данные
        /// </summary>
        /// <param name="ht"></param>
        public void FormAndSendRequestPacket(IRequestData req)
        {
            // поток для формирования общего запроса
            MemoryStream msfullreq = new MemoryStream();
            BinaryWriter bwfullreq = new BinaryWriter(msfullreq);

            try
            {
                byte reqtype = (byte)TYPEOFPACKET.RequestArchivalData;
                bwfullreq.Write(reqtype);

                /*
                 * идентификатор корреляции:
                 * 0 - не нужно следить за тем пришел 
                 *		ответ на запрос или нет
                 */
                UInt16 id_correlation = 0;
                bwfullreq.Write(id_correlation);

                UInt16 UniDsGuid = Convert.ToUInt16(req.DS);
                bwfullreq.Write(UniDsGuid);
                // уник номер объекта в пределах конкретного DataServer
                UInt32 LocObjectGuid = Convert.ToUInt32(req.ObjUni);
                bwfullreq.Write(LocObjectGuid);

                // уник номер объекта в пределах конкретного DataServer
                UInt16 numgr = Convert.ToUInt16(req.NumGroup);// 0xffff;  // запрос архивного блока
                bwfullreq.Write(numgr);

                // номер архивной записи
                Int32 id_block = (Int32)req.ReqParams[0];
                bwfullreq.Write(id_block);

                // строка подключения
                UInt16 lenstrcnt = (UInt16)((byte[])req.ReqParams[1]).Length;
                bwfullreq.Write(lenstrcnt);
                bwfullreq.Write((byte[])req.ReqParams[1]);

                PROVCUST.SendArhivReq(msfullreq.ToArray());
            }
            catch (Exception ex)
            {
                TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG(ex);
            }
        }
        /// <summary>
        /// сформировать и послать запрос на осциллограмму
        /// </summary>
        /// <param name="ht"></param>
        public void FormAndSendOscRequestPacket(IOscillogramma osc)
        {
            // поток для формирования общего запроса
            MemoryStream msfullreq = new MemoryStream();
            BinaryWriter bwfullreq = new BinaryWriter(msfullreq);

            try
            {
                byte reqtype = (byte)TYPEOFPACKET.RequestOsc;
                bwfullreq.Write(reqtype);

                /*
                 * идентификатор корреляции:
                 * 0 - не нужно следить за тем пришел 
                 *		ответ на запрос или нет
                 */
                UInt16 id_correlation = 0;
                bwfullreq.Write(id_correlation);

                UInt16 UniDsGuid = Convert.ToUInt16(osc.DS);
                bwfullreq.Write(UniDsGuid);

                // идентификатор блока с осциллограммой в БД
                UInt32 idbl = osc.IdInBD;
                bwfullreq.Write(idbl);

                // строка подключения                                
                byte[] str_cnt_in_bytes = Encoding.UTF8.GetBytes(HMI_MT_Settings.HMI_Settings.ProviderPtkSql);
                UInt16 lenstrcnt = (UInt16)((byte[])str_cnt_in_bytes).Length;
                bwfullreq.Write(lenstrcnt);
                bwfullreq.Write((byte[])str_cnt_in_bytes);

                PROVCUST.SendOcsReq(msfullreq.ToArray());
            }
            catch (Exception ex)
            {
                TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG(ex);
            }
        }
        #endregion

		#region public-методы
		#endregion

		#region private-методы
		/// <summary>
		/// формирование пакета запроса заданого вида
		/// </summary>
		/// <param name="tYPEOFPACKET"></param>
		/// <param name="msdatareq"></param>
		/// <returns></returns>
		private byte[] FormPacket(TYPEOFPACKET tYPEOFPACKET, MemoryStream msdatareq)
		{
			byte[] rez = new byte[0];

			switch (tYPEOFPACKET)
			{
				case TYPEOFPACKET.RequestData:
					GetRequestDataPacket(msdatareq, ref rez);
					break;
                case TYPEOFPACKET.RequestSpecificBankData:
					GetRequestSpecificBankDataPacket(msdatareq, ref rez);
					break;
                case TYPEOFPACKET.CMD:
					break;
				case TYPEOFPACKET.testpacket:
					break;
				default:
					throw new Exception(string.Format("OrdinalBlokDataComposer.cs - {0} - неизвестный запрашиваемый тип пакета.", tYPEOFPACKET.ToString()));
			}

			return rez;
		}

		private void GetRequestDataPacket(MemoryStream msdatareq, ref byte[] rez)
		{
			// поток для формирования общего запроса
			MemoryStream msfullreq = new MemoryStream();
			BinaryWriter bwfullreq = new BinaryWriter(msfullreq);

			try
			{
				msdatareq.Position = 0;

                byte reqtype = (byte)TYPEOFPACKET.RequestData; // 1 байт - тип запроса - запрос данных отдельно по каждому тегу
				bwfullreq.Write(reqtype);

				/*
				 * идентификатор корреляции:
				 * 0 - не нужно следить за тем пришел 
				 *		ответ на запрос или нет
				 */
				UInt16 id_correlation = 0;
				bwfullreq.Write(id_correlation);

				/*
				 * далее формируем сам запрос
				 * в отдельном потоке, чтобы 
				 * вычислить его длину и поместить 
				 * в tagGuidLen
				 */

				// длина последовательности запроса тегов
				UInt16 tagGuidLen = Convert.ToUInt16(msdatareq.Length);
				bwfullreq.Write(tagGuidLen);

				bwfullreq.Write(msdatareq.ToArray(), 0, (int)msdatareq.Length);	// тело запроса

				rez = new byte[msfullreq.Length];

				Buffer.BlockCopy(msfullreq.ToArray(),0,rez,0,(int)msfullreq.Length);
			}
			catch(Exception ex)
			{
				TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG(ex );
			}
		}
        private void GetRequestSpecificBankDataPacket(MemoryStream msdatareq, ref byte[] rez)
        {
            // поток для формирования общего запроса
            MemoryStream msfullreq = new MemoryStream();
            BinaryWriter bwfullreq = new BinaryWriter(msfullreq);

            try
            {
                msdatareq.Position = 0;

                byte reqtype = (byte)TYPEOFPACKET.RequestSpecificBankData; // 1 байт - тип запроса - запрос определенного банка значений по каждому тегу
                bwfullreq.Write(reqtype);

                /*
                 * идентификатор корреляции:
                 * 0 - не нужно следить за тем пришел 
                 *		ответ на запрос или нет
                 */
                UInt16 id_correlation = 0;
                bwfullreq.Write(id_correlation);

                /*
                 * далее формируем сам запрос
                 * в отдельном потоке, чтобы 
                 * вычислить его длину и поместить 
                 * в tagGuidLen
                 */

                // длина последовательности запроса тегов
                UInt16 tagGuidLen = Convert.ToUInt16(msdatareq.Length);
                bwfullreq.Write(tagGuidLen);

                bwfullreq.Write(msdatareq.ToArray(), 0, (int)msdatareq.Length);	// тело запроса

                rez = new byte[msfullreq.Length];

                Buffer.BlockCopy(msfullreq.ToArray(), 0, rez, 0, (int)msfullreq.Length);
            }
            catch (Exception ex)
            {
                TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG(ex);
            }
        }
        #endregion
	}
}
