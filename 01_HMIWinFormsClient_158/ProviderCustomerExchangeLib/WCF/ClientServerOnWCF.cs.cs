﻿using System;
using System.IO;
using System.ComponentModel;
using System.ServiceModel;
using System.Timers;
using System.Xml.Linq;

using HMI_MT_Settings;
using System.Collections.Generic;
using DSRouter;
using InterfaceLibrary;

namespace ProviderCustomerExchangeLib
{
	public class ClientServerOnWCF : IProviderCustomer
    {
        #region События
        /// <summary>
        /// событие изменения значения тега
        /// </summary>
        public event Action<UInt16, UInt32, UInt32, byte[], DateTime, VarQualityNewDs> OnTagValueChanged;
        #endregion

        #region private
        /// <summary>
		/// массив для входных пакетов
		/// </summary>
		readonly ArrayForExchange arrForReceiveData;
	    private readonly BackgroundWorker bgw = new BackgroundWorker( );
        private readonly CallbackHandler handler = new CallbackHandler( );

	    private readonly Timer timer = new Timer( 30000 );
		#endregion

        #region конструктор(ы)
        public ClientServerOnWCF( XElement srcinfo )
		{
            try
            {
                HMI_Settings.IPADDRES_SERVER = srcinfo.Element("IPAddress").Attribute("value").Value;
                HMI_Settings.PORTin = int.Parse(srcinfo.Element("Port").Attribute("value").Value);

                CreateProxyFromCode( );

                arrForReceiveData = new ArrayForExchange( );
                arrForReceiveData.packetAppearance += this.ArrForReceiveDataPacketAppearance;

                bgw.DoWork += this.BgwDoWork;

                timer.Elapsed += delegate
                                     {
                                         try
                                         {
                                             var idch = HMI_Settings.WCFproxy as IClientChannel;
                                             if ( idch != null && ( idch.State == CommunicationState.Faulted || idch.State == CommunicationState.Closed ) )
                                             {
                                                 HMI_Settings.WCFproxy = null;
                                                 this.CreateProxyFromCode( );
                                             }
                                         }
                                         catch ( Exception exception )
                                         {
                                             TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG( exception );
                                         }
                                     };
                timer.Start( );
            }
            catch (Exception ex)
            {
                TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG(ex);
            }
        }
		#endregion

        /// <summary>
        /// создание прокси из кода
        /// </summary>
        private void CreateProxyFromCode( )
        {
            try
            {
                var endPointAddr = string.Format( "net.tcp://{0}:{1}/DSRouter.DSRouterService", HMI_Settings.IPADDRES_SERVER, HMI_Settings.PORTin );
                var tcpBinding = new NetTcpBinding { TransactionFlow = false };
                tcpBinding.Security.Transport.ProtectionLevel = System.Net.Security.ProtectionLevel.EncryptAndSign;
                tcpBinding.Security.Transport.ClientCredentialType = TcpClientCredentialType.Windows;
                tcpBinding.Security.Mode = SecurityMode.None;
                tcpBinding.MaxReceivedMessageSize = int.MaxValue; // 150000000;
                tcpBinding.MaxBufferSize = int.MaxValue; // 1500000;
                tcpBinding.ReaderQuotas.MaxArrayLength = int.MaxValue; // 1500000;

                var endpointAddress = new EndpointAddress( endPointAddr );
                HMI_Settings.WCFproxy = DuplexChannelFactory<IDSRouter>.CreateChannel( handler, tcpBinding, endpointAddress );

                handler.OnNewError += NewErrorFromCallBackHandler;
                handler.OnNewTagValues += NewTagValueHandler;
            }
            catch ( Exception ex )
            {
                TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG( ex );
            }
        }

		#region public-методы реализации интерфейса IProviderCustomer
	    public PacketQueque NetPackQ { get; set; }

	    /// <summary>
		/// событие появления данных на входе клиента(потребителя)
		/// </summary>
		public event ByteArrayPacketAppearance OnByteArrayPacketAppearance;
        /// <summary>
        /// событие потери связи с DataServer
        /// </summary>
        public event DSCommunicationLoss OnDSCommunicationLoss;

		/// <summary>
		/// посылка запроса на 
        /// текущие данные поставщику данных
		/// </summary>
		/// <param name="pq"></param>
		public void SendData(byte[] pq)
		{
            if (!bgw.IsBusy)
                bgw.RunWorkerAsync(pq);
		}
        /// <summary>
        /// посылка запроса на
        /// осциллограмму поставщику данных
        /// </summary>
        /// <param name="pq"></param>
        public void SendOcsReq(byte[] pq)
        {            
            try
            {
                var rezz = HMI_Settings.WCFproxy.GetDSOscByIdInBD(0, pq);

                var rez = new MemoryStream(rezz);

                if (rez.Length != 0)
                {
                    var brrez = new BinaryReader(rez);
                    // пакет с осциллограммой
                    // тип пакета
                    /*byte reqtype = */brrez.ReadByte();
                    // код ошибки
                    /*byte codeError = */brrez.ReadByte();

                    /*
                    * идентификатор корреляции:
                    * 0 - не нужно следить за тем пришел 
                    *		ответ на запрос или нет
                    */
                    /*ushort id_correlation_out = */brrez.ReadUInt16();

                    /*ushort UniDsGuid = */brrez.ReadUInt16();
                    var idbl = brrez.ReadUInt32();
                    var lenosc = brrez.ReadUInt32();

                    var rezosc = new byte[] { };

                    if (lenosc > 0)
                        rezosc = brrez.ReadBytes((int)lenosc);

                    HMI_Settings.CONFIGURATION.ActiveOscillograms.RemoveOsc(idbl, rezosc);
                }
            }
            catch (Exception ex)
            {
                TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG(ex);
            }
        }
        /// <summary>
        /// посылка запроса на
        /// архивные данные
        /// </summary>
        /// <param name="pq"></param>
        public void SendArhivReq(byte[] pq)
        {
            try
            {
                HMI_Settings.WCFproxy.SetReq2ArhivInfo(0, pq);               
            }
            catch (Exception ex)
            {
                TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG(ex);
            }
        }

        /// <summary>
        /// послать запрос на 
        /// выполнение команды
        /// </summary>
        /// <param name="pq"></param>
        public void SendRunCMD(byte[] pq)
        {
            try
            {
               HMI_Settings.WCFproxy.RunCMDMOA(0, pq);
            }
            catch (Exception ex)
            {
                TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG(ex);
            }
        }

        private void BgwDoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                var rez = HMI_Settings.WCFproxy.GetDSValueAsByteBuffer( 0, e.Argument as byte[] );

                if ( rez != null && rez.Length != 0 )
                {
                    arrForReceiveData.Add( rez );
                    if ( this.OnDSCommunicationLoss != null ) this.OnDSCommunicationLoss( false );
                }
            }
            catch ( Exception ex )
            {
                TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG( ex );
                if ( this.OnDSCommunicationLoss != null ) this.OnDSCommunicationLoss( true );
            }
        }

	    /// <summary>
		/// инициирование события прихода данных
		/// </summary>
		/// <param name="pq"></param>
		private void ArrForReceiveDataPacketAppearance(byte[] pq)
		{
			try
			{
				if (OnByteArrayPacketAppearance != null)
					OnByteArrayPacketAppearance(pq);
			}
			catch (Exception ex)
			{
				TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG(ex);
			}
		}
		#endregion

        #region Обработчики событий из CallBack'a wcf.
        /// <summary>
        /// Обработчик события в Callback при появлении нового значения
        /// </summary>
        private void NewTagValueHandler(Dictionary<string, DSTagValue> tv)
        {
            /* 
            Console.WriteLine("Порция данных");
            foreach (KeyValuePair<string, DSRouter.DSTagValue> kvp in tv)
                if (kvp.Value.VarValueAsObject == null)
                    Console.WriteLine(string.Format("{0} : {1}", kvp.Key, "null"));
                else
                    Console.WriteLine(string.Format("{0} : {1}", kvp.Key, kvp.Value.VarValueAsObject.ToString()));
             */

            foreach (var tag in tv)
            {
                string key = tag.Key.ToString();
                var split = key.Split('.');

                try
                {
                    UInt16 dsGuid = UInt16.Parse(split[0]);
                    UInt32 devGuid = UInt32.Parse(split[1]);
                    UInt32 tagGuid = UInt32.Parse(split[2]);

                    // Перевод значения тега в byte []                    
                    if (tag.Value.VarValueAsObject == null)
                        continue;

                    string strTagValue = tag.Value.VarValueAsObject.ToString();

                    byte[] byteArrTagValue;
                    if (strTagValue == "True" || strTagValue == "False")
                        byteArrTagValue = BitConverter.GetBytes(Boolean.Parse(strTagValue));
                    else
                        byteArrTagValue = BitConverter.GetBytes(Single.Parse(strTagValue));

                    // VarQualityNewDs
                    string tagQualityStr = tag.Value.VarQuality.ToString();
                    VarQualityNewDs tagQuality = tagQualityStr == "1"
                                                     ? VarQualityNewDs.vqGood
                                                     : VarQualityNewDs.vqUndefined;

                    if (OnTagValueChanged != null)
                        OnTagValueChanged(dsGuid, devGuid, tagGuid, byteArrTagValue, DateTime.Now, tagQuality);
                }
                catch
                {
                    Console.WriteLine("NewTagValueHandler: Ошибка при разборе нового значения тега: " + key);
                }
            }
        }


        private void NewErrorFromCallBackHandler(string errorStr)
        {
            throw new NotImplementedException("NewErrorFromCallBackHandler");
        }
        #endregion
    }
}