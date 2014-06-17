using System;
using System.Diagnostics;
using System.IO;
using System.ComponentModel;
using System.Linq;
using System.ServiceModel;
using System.Timers;
using System.Xml.Linq;

using HMI_MT_Settings;
using System.Collections.Generic;
using InterfaceLibrary;
using ProviderCustomerExchangeLib.DSRouterService;

namespace ProviderCustomerExchangeLib.WCF
{
	public class ClientServerOnWCF : IProviderCustomer, IWcfProvider
    {
        #region События
        /// <summary>
        /// событие изменения значения тега
        /// </summary>
        public event Action<UInt16, UInt32, UInt32, object, DateTime, VarQualityNewDs> OnTagValueChanged;	    
        #endregion

        #region private

        /// <summary>
        /// Proxy для роутера
        /// </summary>        
        private IDSRouter WCFproxy;

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
                                             timer.Stop();

                                             var idch = WCFproxy as DSRouterClient;
                                                if ( idch != null && ( idch.State == CommunicationState.Faulted || idch.State == CommunicationState.Closed ) )
                                             {
                                                 WCFproxy = null;
                                                 if (this.CreateProxyFromCode())
                                                 {
                                                     if (OnProxyRecreated != null)
                                                         OnProxyRecreated();

                                                     if (OnDSCommunicationLoss != null)
                                                         OnDSCommunicationLoss(false);
                                                 }
                                             }
                                         }
                                         catch ( Exception exception )
                                         {
                                             TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG( exception );
                                         }
                                         finally
                                         {
                                             timer.Start();
                                         }
                                     };
                timer.Interval = 3000;
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
        private bool CreateProxyFromCode( )
        {
            try
            {
                var endPointAddr = string.Format( "net.tcp://{0}:{1}/DSRouter.DSRouterService/DSRouterService.svc", HMI_Settings.IPADDRES_SERVER, HMI_Settings.PORTin );
                var tcpBinding = new NetTcpBinding { TransactionFlow = false };
                tcpBinding.Security.Transport.ProtectionLevel = System.Net.Security.ProtectionLevel.EncryptAndSign;
                tcpBinding.Security.Transport.ClientCredentialType = TcpClientCredentialType.Windows;
                tcpBinding.Security.Mode = SecurityMode.None;
                tcpBinding.MaxReceivedMessageSize = int.MaxValue; // 150000000;
                tcpBinding.MaxBufferSize = int.MaxValue; // 1500000;
                tcpBinding.ReaderQuotas.MaxArrayLength = int.MaxValue; // 1500000;

                var endpointAddress = new EndpointAddress( endPointAddr );
                WCFproxy = new DSRouterClient(new InstanceContext(handler), tcpBinding, endpointAddress);

                handler.OnNewError += NewErrorFromCallBackHandler;

                (WCFproxy as DSRouterClient).GetTagsValueCompleted += OnGetTagsValueCompleted;
                (WCFproxy as DSRouterClient).GetTagsValuesUpdatedCompleted += OnGetTagsValuesUpdatedCompleted;

                (WCFproxy as DSRouterClient).Open();
            }
            catch ( Exception ex )
            {
                TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG( ex );

                if (OnDSCommunicationLoss != null)
                    OnDSCommunicationLoss(true);

                return false;
            }

            return true;
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
                var rezz = WCFproxy.GetDSOscByIdInBD(0, pq);

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
                WCFproxy.SetReq2ArhivInfo(0, pq);               
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
               WCFproxy.RunCMDMOA(0, pq);
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
                var rez = WCFproxy.GetDSValueAsByteBuffer( 0, e.Argument as byte[] );

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

        #region Реализация методов интерфейса IWcfProvider
        public event Action OnProxyRecreated;

        public void GetTagsValue(string[] tagsArrayToRequest)
        {
            //NewTagValueHandler(WCFproxy.GetTagsValue(tagsArrayToRequest));
            (WCFproxy as DSRouterClient).GetTagsValueAsync(tagsArrayToRequest);
        }

        public void GetTagsValuesUpdated()
        {
            //NewTagValueHandler(WCFproxy.GetTagsValuesUpdated());
            (WCFproxy as DSRouterClient).GetTagsValuesUpdatedAsync();
        }

        #region DSRouterClient Async Metods Handlers
        private void OnGetTagsValuesUpdatedCompleted(object sender, GetTagsValuesUpdatedCompletedEventArgs getTagsValuesUpdatedCompletedEventArgs)
        {
            try
            {
                if (getTagsValuesUpdatedCompletedEventArgs.Error == null)
                {
                    if (getTagsValuesUpdatedCompletedEventArgs.Result.Count > 0)
                    {
                        Console.WriteLine("Пришло обновление тегов");
                        NewTagValueHandler(getTagsValuesUpdatedCompletedEventArgs.Result);
                    }
                    else
                    {
                        Console.WriteLine("Обновлений не пришло");
                        TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG(TraceEventType.Verbose, 0, "Обновлений не пришло");
                    }
                }
                else
                {
                    if (OnDSCommunicationLoss != null)
                        OnDSCommunicationLoss(true);
                    TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG(TraceEventType.Warning, 0, "При выполнении GetTagsValuesUpdatedAsync произошла ошабка.");
                }
            }
            catch (Exception ex)
            {
                TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG(ex);
            }
        }

        private void OnGetTagsValueCompleted(object sender, GetTagsValueCompletedEventArgs getTagsValueCompletedEventArgs)
        {
            try
            {
                if (getTagsValueCompletedEventArgs.Error == null)
                {
                    if (getTagsValueCompletedEventArgs.Result.Count > 0)
                    {
                        NewTagValueHandler(getTagsValueCompletedEventArgs.Result);
                    }
                    return;
                }
                else
                {
                    TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG(TraceEventType.Warning, 0, "При выполнении GetTagsValueAsync произошла ошабка.");
                }
            }
            catch (Exception ex)
            {
                TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG(ex);
            }
        }
        #endregion

        #endregion

        #region Обработчики событий из CallBack'a wcf.
        /// <summary>
        /// Обработчик события в Callback при появлении нового значения
        /// </summary>
        private void NewTagValueHandler(Dictionary<string, DSRouterTagValue> tv)
        {
            #if DEBUG
            tv = (from pair in tv orderby pair.Key select pair).ToDictionary(pair => pair.Key, pair => pair.Value);
            
            Console.WriteLine("Порция данных");
            foreach (KeyValuePair<string, DSRouterTagValue> kvp in tv)
                if (kvp.Value.VarValueAsObject == null)
                    Console.WriteLine(string.Format("{0} : {1}", kvp.Key, "null"));
                else
                    Console.WriteLine(string.Format("{0} : {1}  {2}  {3}", kvp.Key, kvp.Value.VarValueAsObject.ToString(), kvp.Value.VarValueAsObject.GetType(), (VarQualityNewDs)kvp.Value.VarQuality));
            #endif

            foreach (var tag in tv)
            {
                // VarQualityNewDs
                VarQualityNewDs tagQuality = (VarQualityNewDs)tag.Value.VarQuality;
                if (tagQuality != VarQualityNewDs.vqGood && tagQuality != VarQualityNewDs.vqHandled)
                    continue;

                if (tag.Value.VarValueAsObject == null)
                    continue;

                string key = tag.Key.ToString();
                var split = key.Split('.');

                try
                {
                    UInt16 dsGuid = UInt16.Parse(split[0]);
                    UInt32 devGuid = UInt32.Parse(split[1]);
                    UInt32 tagGuid = UInt32.Parse(split[2]);

                    if (OnTagValueChanged != null)
                        OnTagValueChanged(dsGuid, devGuid, tagGuid, tag.Value.VarValueAsObject, DateTime.Now, tagQuality);                    
                }
                catch
                {
                    TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG(TraceEventType.Error, 0,
                                                                         String.Format(
                                                                             "ProviderCustomerExchange.ClientServerOnWCF::NewTagValueHandler: Ошибка при разборе нового значения тега: {0}",
                                                                             key));
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
