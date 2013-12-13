using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;
using System.Collections;
using NetCrzaDevices;
using System.Threading;
using DataModule;
using NSNetNetManager;
using System.Windows.Forms;
using System.ComponentModel;
using System.Globalization;
using System.Xml;
using System.IO;
using System.Xml.XPath;
using System.Reflection;
using NSNetTcpFromUdpManager;
using System.Net;
using System.Xml.Linq;
using NSNetSecondUDPFromUdpManager;
using CRZADevices;

namespace HMI_MT
{
   /// <summary>
   /// class Configurator
   /// класс, описывающий конкретную конфигурацию
   /// </summary>
   public class Configurator : INetManagerSink
   {
      /// <summary>
      /// сборка (верхний и нижний уровень)
      /// </summary>
      Assembly theAssembly;
      /// <summary>
      /// сборка CRZADevices.dll
      /// </summary>
      Assembly theAssemblyFCDesc;


      // конфигурация
      public static ArrayList MTD;
      public static ArrayList KB;

      // поток чтения из ФК
      private Thread readFCThread;

      // признак потери связи
      //private bool isLostConnection = false;
      private bool isLostConnection = false;
      public INetManager netman;

      private DateTime m_crzaTimeFC;

      BackgroundWorker bcw;	// поток, обновляющий данные верхнего уровня

      public DateTime CRZATimeFC
      {
          set 
          {
              m_crzaTimeFC = value;
          }
         get
         {
            return m_crzaTimeFC;
         }
      }

      /// <summary>
      /// конструктор
      /// </summary>
      public Configurator( )
      {
         bcw = new BackgroundWorker( );
         bcw.DoWork += new DoWorkEventHandler( ReceivePacketInThread );
      }

      private XPathNodeIterator GetNodeList( string exp, string FILE_NAME )
      {
         XPathDocument doc = new XPathDocument( FILE_NAME );
         XPathNavigator nav = doc.CreateNavigator( );
         XPathNodeIterator myNodes;
         myNodes = nav.Select( exp );
         return myNodes;
      }

      /// <summary>
      /// public ArrayList GetConfigurator()
      /// Функция создает конфигурацию блоков
      /// </summary>
      /// <returns></returns>
      public ArrayList GetConfigurator( string pathPrjCfgFile, string role )
      {
         // конфигурация блоков
         KB = new ArrayList( );

         // логическая конфигурация устройств
         string path = Environment.CurrentDirectory + Path.DirectorySeparatorChar + "HMIDevices.dll";
         string pathFCDesc = Environment.CurrentDirectory + Path.DirectorySeparatorChar + "CRZADevices.dll";
         if ( !File.Exists( path ) )
            throw new Exception( "Файл " + path + " не существует" );
         if ( !File.Exists( pathFCDesc ) )
            throw new Exception( "Файл " + pathFCDesc + " не существует" );

         // получаем сборку
         theAssembly = Assembly.LoadFile( path );
         theAssemblyFCDesc = Assembly.LoadFile( pathFCDesc );

         // перечисляем устройства и создаем экземпляры соответсвующих классов

         // открываем файл конфигурации устройств проекта
         string pathToF_PrgDevCFG_cdp = Environment.CurrentDirectory + Path.DirectorySeparatorChar + "Project" + Path.DirectorySeparatorChar + "PrgDevCFG.cdp";

         if (!File.Exists(pathToF_PrgDevCFG_cdp))
            throw new Exception("Файл " + pathToF_PrgDevCFG_cdp + " не существует");

         XmlTextReader reader = new XmlTextReader(pathToF_PrgDevCFG_cdp);
         XmlDocument doc = new XmlDocument( );
         doc.Load( reader );
         reader.Close( );

         // теперь нужно получать список узлов с описанием ФК и устройств проекта
         XPathNodeIterator xpni;
         XPathNodeIterator xpniCh;
         XPathNavigator cd;
         XPathNavigator cdd;

         xpni = GetNodeList("/MT/Configuration/FC", pathToF_PrgDevCFG_cdp);	// список узлов с ФК
         Type blockType;
         Type[ ] argTypes;
         ConstructorInfo ctor;
         int numCurFC;
         int numCurDev;
         string strAdrFC = String.Empty;

         while ( xpni.MoveNext( ) )
         {
            cd = xpni.Current;
            // формируем описание ФК
            // получим тип 
			blockType = theAssemblyFCDesc.GetType("CRZADevices.FC");//theAssembly
            argTypes = new Type[ ] { typeof( int ), typeof( string ) };
            ctor = blockType.GetConstructor( argTypes );

            numCurFC = Convert.ToInt32( cd.GetAttribute( "numFC", "" ) );
            strAdrFC = cd.GetAttribute( "fcadr", "" );

            //вызываем конструктор
            FC fc = ( FC ) ctor.Invoke( new object[ ] { numCurFC, strAdrFC } );	// FC FC1 = new FC(0);  //создаем ФК
            //   					 Convert.ToInt32( cd.GetAttribute( "numdev", "" ) ) 
            // получим тип 
            blockType = theAssemblyFCDesc.GetType( "CRZADevices." + cd.GetAttribute( "nameEHighLevel", "" ) );
            argTypes = new Type [ ] { typeof( int ), typeof( int ), typeof( int ), typeof( string )};//, typeof( int ) 
            ctor = blockType.GetConstructor( argTypes );
            fc.Devices.Add( ctor.Invoke( new object [ ] { Convert.ToInt32( cd.GetAttribute( "numFC", "" ) ), 0, 0, cd.GetAttribute( "describe", "" ) } ) );//numdev

            // теперь пробежим по устройствам данного ФК и добавим их
            xpniCh = cd.Select( "FCDevices/Device" );
            while ( xpniCh.MoveNext( ) )
            {
               cdd = xpniCh.Current;
               if ( cdd.GetAttribute( "enable", "" ).ToLower() == "false" )
                  continue;	// пропускаем устройство

               // формируем номер устройства с учетом его ФК
               numCurDev = Convert.ToInt32( cdd.SelectSingleNode( "NumDev" ).Value );
               //numCurDev += numCurFC * 256;

               // формируем описание устройства
               // получим тип 
               blockType = theAssembly.GetType( "CRZADevices." + cdd.SelectSingleNode( "nameEHighLevel" ).Value );
               argTypes = new Type[ ] { typeof( int ), typeof( int ), typeof( int ), typeof( int ), typeof( string ) };
               try
               {
                  ctor = blockType.GetConstructor( argTypes );
               }
               catch
               {
                  MessageBox.Show( "Неверное описание устройства:" + cdd.SelectSingleNode( "nameEHighLevel" ).Value, "Ошибка в CRZADevices.cs", MessageBoxButtons.OK, MessageBoxIcon.Error );
                  continue;
               }
               //вызываем конструктор
               try 
               {
                  fc.Devices.Add(ctor.Invoke(new object[] { numCurFC,
                                                            numCurDev,
                                                            Convert.ToInt32( cdd.SelectSingleNode( "NumLock" ).Value ),
                                                            Convert.ToInt32( cdd.SelectSingleNode( "NumSec" ).Value ),
                                                            cdd.SelectSingleNode( "DescDev" ).Value }));
               }catch(Exception ex)
               {
                   MessageBox.Show("Ошибка при создании класса устройсва (уровень АСУ) : " + ex.Message + "\nFC = " + numCurFC.ToString() + "\ndev = " + numCurDev.ToString() + "\n" + ex, "Configurator.cs", MessageBoxButtons.OK, MessageBoxIcon.Error);
               }
            }

            // добавляем ФК к конфигурации блоков
            KB.Add( fc );
         }

         // добавляем устройства по доп. источникам
         XDocument xdocCDP = XDocument.Load(pathToF_PrgDevCFG_cdp);

         FormSrcDescribePS(ref KB, xdocCDP);

         // запускаем процесс обновления - настраиваем интерфейсную ссылку на источних первичных (raw) данных
		NetTcpFromUdpManager ntcpnetman = new NetTcpFromUdpManager( );
        netman = ntcpnetman;

         MTD = netman.SetConfig( pathPrjCfgFile );
         netman.Advise( this );

        // создаем поток чтения из ФК в массив raw-данных MTD
         readFCThread = new Thread( new ThreadStart( netman.getdata ) );
         readFCThread.Name = "ReaderFC";
         readFCThread.Start( );

         // связываем для оповещения
         // все устройства в online
         foreach (DataSource aFC in KB)
            for ( int i = 0 ; i < aFC.Devices.Count ; i++ )
            {
               TCRZADirectDevice aDev = ( TCRZADirectDevice ) aFC.Devices[ i ];
               foreach (MTDevice aBMRZ in MTD)
                  if( aDev.NumDev == aBMRZ.NDev && aBMRZ.ToString() != "FC_net" && aDev.NumFC == aBMRZ.NFC )
                  //if ( aDev.NumDev == aBMRZ.NDev )
                  {
                     aBMRZ.OnChangeNetDevState += aDev.ChCrzaDevStat;
                     aBMRZ.NetDeviceState = NetDeviceState.netdstOnline;
                     break;
                  }
               aDev.crzaDeviceState = CRZADeviceState.CRZAdstOnline;
            }

        // связываем для обновления переменных
         foreach (DataSource aFc in KB)
            foreach (TCRZADirectDevice tdd in aFc)
            {
               foreach (MTDevice aBMRZ in MTD)
                  if (tdd.NumDev == 0 && aBMRZ.ToString() == "FC_net" && tdd.NumFC == aFc.NumFC)
                     foreach (TCRZAGroup tdg in tdd)
                        foreach (TCRZAVariable tgv in tdg)
                           if (aBMRZ.varDev[tgv.RegInDev] is Real_FieldMT)
                              ((Real_FieldMT)aBMRZ.varDev[tgv.RegInDev]).OnChangeNetVarState += tgv.ExtractVarFrom;
                           else if (aBMRZ.varDev[tgv.RegInDev] is Int_FieldMT)
                              ((Int_FieldMT)aBMRZ.varDev[tgv.RegInDev]).OnChangeNetVarState += tgv.ExtractVarFrom;
                           else if (aBMRZ.varDev[tgv.RegInDev] is Bit_FieldMT)
                              ((Bit_FieldMT)aBMRZ.varDev[tgv.RegInDev]).OnChangeNetVarState += tgv.ExtractVarFrom;
                           else if (aBMRZ.varDev[tgv.RegInDev] is UInt_FieldMT)
                              ((UInt_FieldMT)aBMRZ.varDev[tgv.RegInDev]).OnChangeNetVarState += tgv.ExtractVarFrom;
                           else if (aBMRZ.varDev[tgv.RegInDev] is DInt_FieldMT)
                              ((DInt_FieldMT)aBMRZ.varDev[tgv.RegInDev]).OnChangeNetVarState += tgv.ExtractVarFrom;
                           else if (aBMRZ.varDev[tgv.RegInDev] is UDInt_FieldMT)
                              ((UDInt_FieldMT)aBMRZ.varDev[tgv.RegInDev]).OnChangeNetVarState += tgv.ExtractVarFrom;
                           else if (aBMRZ.varDev[tgv.RegInDev] is UDInt_FieldMT_0123)
                              ((UDInt_FieldMT_0123)aBMRZ.varDev[tgv.RegInDev]).OnChangeNetVarState += tgv.ExtractVarFrom;
                           else if (aBMRZ.varDev[tgv.RegInDev] is Byte_FieldMT)
                              ((Byte_FieldMT)aBMRZ.varDev[tgv.RegInDev]).OnChangeNetVarState += tgv.ExtractVarFrom;
                           else if (aBMRZ.varDev[tgv.RegInDev] is DateTime4_FieldMT)
                              ((DateTime4_FieldMT)aBMRZ.varDev[tgv.RegInDev]).OnChangeNetVarState += tgv.ExtractVarFrom;
                           else if (aBMRZ.varDev[tgv.RegInDev] is DateTime3_FieldMT)
                              ((DateTime3_FieldMT)aBMRZ.varDev[tgv.RegInDev]).OnChangeNetVarState += tgv.ExtractVarFrom;
                           else if (aBMRZ.varDev[tgv.RegInDev] is DateTimeUTC_FieldMT)
                              ((DateTimeUTC_FieldMT)aBMRZ.varDev[tgv.RegInDev]).OnChangeNetVarState += tgv.ExtractVarFrom;
                           else if (aBMRZ.varDev[tgv.RegInDev] is DateTimeSirius_FieldMT)
                              ((DateTimeSirius_FieldMT)aBMRZ.varDev[tgv.RegInDev]).OnChangeNetVarState += tgv.ExtractVarFrom;
                           else if (aBMRZ.varDev[tgv.RegInDev] is Stringz_FieldMT)
                              ((Stringz_FieldMT)aBMRZ.varDev[tgv.RegInDev]).OnChangeNetVarState += tgv.ExtractVarFrom;
                           else if (aBMRZ.varDev[tgv.RegInDev] is BCDPack_FieldMT)
                              ((BCDPack_FieldMT)aBMRZ.varDev[tgv.RegInDev]).OnChangeNetVarState += tgv.ExtractVarFrom;
                           else if (aBMRZ.varDev[tgv.RegInDev] is BCD_FieldMT)
                              ((BCD_FieldMT)aBMRZ.varDev[tgv.RegInDev]).OnChangeNetVarState += tgv.ExtractVarFrom;
                           else if (aBMRZ.varDev[tgv.RegInDev] is IPAdress_FieldMT)
                              ((IPAdress_FieldMT)aBMRZ.varDev[tgv.RegInDev]).OnChangeNetVarState += tgv.ExtractVarFrom;

               foreach (MTDevice aBMRZ in MTD)
                  if (tdd.NumDev == aBMRZ.NDev && aBMRZ.ToString() != "FC_net" && tdd.NumFC == aBMRZ.NFC)
                     foreach (TCRZAGroup tdg in tdd)
                        foreach (TCRZAVariable tgv in tdg)
                           if (aBMRZ.varDev[tgv.RegInDev] is Real_FieldMT)
                              ((Real_FieldMT)aBMRZ.varDev[tgv.RegInDev]).OnChangeNetVarState += tgv.ExtractVarFrom;
                           else if (aBMRZ.varDev[tgv.RegInDev] is Int_FieldMT)
                              ((Int_FieldMT)aBMRZ.varDev[tgv.RegInDev]).OnChangeNetVarState += tgv.ExtractVarFrom;
                           else if (aBMRZ.varDev[tgv.RegInDev] is Bit_FieldMT)
                              ((Bit_FieldMT)aBMRZ.varDev[tgv.RegInDev]).OnChangeNetVarState += tgv.ExtractVarFrom;
                           else if (aBMRZ.varDev[tgv.RegInDev] is UInt_FieldMT)
                              ((UInt_FieldMT)aBMRZ.varDev[tgv.RegInDev]).OnChangeNetVarState += tgv.ExtractVarFrom;
                           else if (aBMRZ.varDev[tgv.RegInDev] is DInt_FieldMT)
                              ((DInt_FieldMT)aBMRZ.varDev[tgv.RegInDev]).OnChangeNetVarState += tgv.ExtractVarFrom;
                           else if (aBMRZ.varDev[tgv.RegInDev] is UDInt_FieldMT)
                              ((UDInt_FieldMT)aBMRZ.varDev[tgv.RegInDev]).OnChangeNetVarState += tgv.ExtractVarFrom;
                           else if (aBMRZ.varDev[tgv.RegInDev] is UDInt_FieldMT_0123)
                              ((UDInt_FieldMT_0123)aBMRZ.varDev[tgv.RegInDev]).OnChangeNetVarState += tgv.ExtractVarFrom;
                           else if (aBMRZ.varDev[tgv.RegInDev] is Byte_FieldMT)
                              ((Byte_FieldMT)aBMRZ.varDev[tgv.RegInDev]).OnChangeNetVarState += tgv.ExtractVarFrom;
                           else if (aBMRZ.varDev[tgv.RegInDev] is DateTime4_FieldMT)
                              ((DateTime4_FieldMT)aBMRZ.varDev[tgv.RegInDev]).OnChangeNetVarState += tgv.ExtractVarFrom;
                           else if (aBMRZ.varDev[tgv.RegInDev] is DateTime3_FieldMT)
                              ((DateTime3_FieldMT)aBMRZ.varDev[tgv.RegInDev]).OnChangeNetVarState += tgv.ExtractVarFrom;
                           else if (aBMRZ.varDev[tgv.RegInDev] is DateTimeUTC_FieldMT)
                              ((DateTimeUTC_FieldMT)aBMRZ.varDev[tgv.RegInDev]).OnChangeNetVarState += tgv.ExtractVarFrom;
                           else if (aBMRZ.varDev[tgv.RegInDev] is DateTimeSirius_FieldMT)
                              ((DateTimeSirius_FieldMT)aBMRZ.varDev[tgv.RegInDev]).OnChangeNetVarState += tgv.ExtractVarFrom;
                           else if (aBMRZ.varDev[tgv.RegInDev] is Stringz_FieldMT)
                              ((Stringz_FieldMT)aBMRZ.varDev[tgv.RegInDev]).OnChangeNetVarState += tgv.ExtractVarFrom;
                           else if (aBMRZ.varDev[tgv.RegInDev] is BCDPack_FieldMT)
                              ((BCDPack_FieldMT)aBMRZ.varDev[tgv.RegInDev]).OnChangeNetVarState += tgv.ExtractVarFrom;
                           else if (aBMRZ.varDev[tgv.RegInDev] is BCD_FieldMT)
                              ((BCD_FieldMT)aBMRZ.varDev[tgv.RegInDev]).OnChangeNetVarState += tgv.ExtractVarFrom;
                           else if (aBMRZ.varDev[tgv.RegInDev] is IPAdress_FieldMT)
                              ((IPAdress_FieldMT)aBMRZ.varDev[tgv.RegInDev]).OnChangeNetVarState += tgv.ExtractVarFrom;
            }
         // связываем фк как устройство с остальными устройствами данного фк
         foreach ( DataSource aFc in KB )
         {
            if (netman.IP_FCs[aFc.IpAddrFC] is FC_net)
               aFc.FCNet = ( FC_net ) netman.IP_FCs[ aFc.IpAddrFC ];  // привязываемся к описанию фк нижнего уровня
            else if (netman.IP_FCs[aFc.IpAddrFC] is PS_net)
               aFc.PSNet = (PS_net)netman.IP_FCs[aFc.IpAddrFC];  // привязываемся к описанию фк нижнего уровня

            aFc.isFCConnection = true; // для начала устанавливаем, что связь есть

            foreach ( TCRZADirectDevice tdd in aFc )
            {
               if ( tdd.NumDev == 0 ) // то есть это фк-устройство
               {
                  FCasDev fcd = ( FCasDev ) tdd;
                  fcd.Devices = aFc.Devices;
                  foreach (TCRZADirectDevice tddd in fcd.Devices)
                     tddd.FC4ThisTCRZADirectDev = fcd;
               }
            }
         }

         return KB;
      }

      private void FormSrcDescribePS(ref ArrayList KBloc, XDocument xdoc)
      {
         Type blockType;
         Type[] argTypes;
         ConstructorInfo ctor;
         int numCurFC;
         string strAdrFC = String.Empty;

         // список узлов с источниками srcName;
         IEnumerable<XElement> xpnis = xdoc.Element("MT").Element("Configuration").Elements("PS");

         foreach (XElement cd in xpnis)
         {
            //cd = xpni.Current;
            // формируем описание ФК
            // получим тип 
			 blockType = theAssemblyFCDesc.GetType("CRZADevices." + "PS");//theAssembly
            argTypes = new Type[] { typeof(int), typeof(string) };
            ctor = blockType.GetConstructor(argTypes);

            numCurFC = Convert.ToInt32(cd.Attribute("numFC").Value);
            strAdrFC = cd.Attribute("psadr").Value;

            //вызываем конструктор - создаем ФК
            PS ps = (PS)ctor.Invoke(new object[] { numCurFC, strAdrFC });
            // получим тип 
            blockType = theAssemblyFCDesc.GetType("CRZADevices." + cd.Attribute("nameEHighLevel").Value);
            argTypes = new Type[] { typeof(int), typeof(int), typeof(int), typeof(string) };
            ctor = blockType.GetConstructor(argTypes);
            ps.Devices.Add(ctor.Invoke(new object[] { Convert.ToInt32(cd.Attribute("numFC").Value), 0, 0, cd.Attribute("describe").Value }));

            // теперь пробежим по устройствам данного ФК и добавим их
            ps.Devices.AddRange(SetConfigASUFCDevices(numCurFC, cd.Element("PSDevices").Elements("Device")));

            // добавляем ФК к конфигурации блоков
            KBloc.Add(ps);
         }
      }


      private ArrayList SetConfigASUFCDevices(int ncfc, IEnumerable<XElement> xefc)
      {
         Type blockType;
         Type[] argTypes;
         ConstructorInfo ctor;
         int numCurDev;
         string strAdrFC = String.Empty;

         ArrayList fcdevs = new ArrayList();

         foreach (XElement cdd in xefc)
         {
            if (cdd.Attribute("enable").Value.ToLower() == "false")
               continue;	// пропускаем устройство

            // формируем номер устройства с учетом его ФК
            numCurDev = Convert.ToInt32(cdd.Element("NumDev").Value);
            //numCurDev += numCurFC * 256;

            // формируем описание устройства
            // получим тип 
            blockType = theAssembly.GetType("CRZADevices." + cdd.Element("nameEHighLevel").Value);
            argTypes = new Type[] { typeof(int), typeof(int), typeof(int), typeof(int), typeof(string) };
            try
            {
               ctor = blockType.GetConstructor(argTypes);
            }
            catch
            {
               MessageBox.Show("Неверное описание устройства:" + cdd.Element("nameEHighLevel").Value, "Ошибка в CRZADevices.cs", MessageBoxButtons.OK, MessageBoxIcon.Error);
               continue;
            }
            //вызываем конструктор
            fcdevs.Add(ctor.Invoke(new object[] { ncfc,
                                                            numCurDev,
                                                            Convert.ToInt32( cdd.Element( "NumLock" ).Value ),
                                                            Convert.ToInt32( cdd.Element( "NumSec" ).Value ),
                                                            cdd.Element( "DescDev" ).Value }));
         }
         return fcdevs;
      }


      public void SendPacketsToFC( byte[ ] arrpacket )
      {
         netman._listener_NewMessage( arrpacket );
      }


      /// <summary>
      /// public void SetIPLogger( string ip )
      /// установить адрес логгера
      /// </summary>
      /// <param Name="ip"></param>
      public void SetIPLogger( string ip )
      {
         netman.SetIP_Logger( ip );
      }

      /// <summary>
      /// ПРИНУДИТЕЛЬНОЕ чтение пакетов с нижнего на верхний уровень - это нужно в ситуации, когда устройства нет,
      /// а историческая информация по нему есть
      /// </summary>
      /// <param Name="nFC"></param>
      /// <param Name="idDev"></param>
      public void ReceivePacketForce( int nFC, int idDev )
      {
         foreach ( DataSource aFC in KB )
            if ( aFC.NumFC == nFC )
               for ( int i = 0 ; i < aFC.Devices.Count ; i++ )
               {
                  TCRZADirectDevice aDev = ( TCRZADirectDevice ) aFC.Devices[ i ];
                  if ( aDev.NumDev == idDev )
                  {
                     // извлечем raw-пакеты из устройства
                     foreach ( MTDevice aBMRZ in MTD )
                        if( aDev.NumDev == aBMRZ.NDev && aBMRZ.ToString() != "FC_net" && aDev.NumFC == aBMRZ.NFC )
                        //if ( aDev.NumDev == aBMRZ.NDev )
                        {
                           aDev.CRZAMemDev = aBMRZ.memDev;
                           break;
                        }

                     foreach ( TCRZAGroup aGroup in aDev )
                        foreach ( TCRZAVariable aVariable in aGroup )
                        {
                           //aVariable.SetQuality( VarQuality.vqArhiv );
                           aVariable.Quality = VarQuality.vqArhiv;
                           aVariable.ExtractVarFrom( MTD );
                        }

                     break;
                  }
               }
      }

      /// <summary>
      /// public void ReceivePacket()
      /// Функция заполняет дерево устройств данными в отдельном потоке ReceivePacketInThread()
      /// </summary>
      public void ReceivePacket( )
      {
         while ( !bcw.IsBusy )
            bcw.RunWorkerAsync( );
      }

      private void ReceivePacketInThread( object sender, DoWorkEventArgs e )
      {
         //--------------------------------------------------------------------
         // проверяем связь с ФК - если связи нет то обнуляем данные по этому фк
         //int cn =  netman.GetCountLastPacketFromFC();
         //for( int ij = 0 ;ij < netman.IP_FCs.Count ;ij++ )
         foreach ( DataSource aFC in KB )
         {
            //if ( !aFC.IsFCConnectHMI( ) )
            if (!aFC.isFCConnection)
            {
               if ( !aFC.isLostConnection )   // предотвращаем повторную очистку
               {
                  aFC.isLostConnection = true;	//  связи потеряна

                  // обнуляем теги
                  for ( int i = 0 ; i < aFC.Devices.Count ; i++ )
                  {
                     TCRZADirectDevice aDev = ( TCRZADirectDevice ) aFC.Devices[ i ];
                     foreach ( MTDevice aBMRZ in MTD )
                        if( aDev.NumDev == aBMRZ.NDev && aBMRZ.ToString() != "FC_net" && aDev.NumFC == aBMRZ.NFC )
                        //if ( aDev.NumDev == aBMRZ.NDev )
                        {
                           aDev.CRZAMemDev = aBMRZ.memDev;
                           break;
                        }
                     foreach ( TCRZAGroup aGroup in aDev )
                        foreach ( TCRZAVariable aVariable in aGroup )
                        {
                           if (aVariable.Quality != VarQuality.vqArhiv)
                           { 
                              aVariable.Quality = VarQuality.vqUndefined;
                              aVariable.firstRead = false;
                           }

                           aVariable.ExtractVarFrom( MTD );
                        }
                  }
               }
            }
            else
            {
               if ( aFC.isLostConnection )	// если связь была потеряна
               {
                  aFC.isLostConnection = false;
                  // устанавливаем всем тегам хорошее качество
                  foreach ( TCRZADirectDevice tdd in aFC )
                  {
                     tdd.crzaDeviceState = CRZADeviceState.CRZAdstOnline;

                     foreach ( TCRZAGroup tdg in tdd )
                        foreach (TCRZAVariable tgv in tdg)
                        { 
                           tgv.Quality = VarQuality.vqGood;
                        }
                  }
                  netman.ClearNetPackQ( );
               }
               if (!HMI_Settings.IsTCPClient)
                m_crzaTimeFC = netman.TimeFC;		// устанавливаем время из ФК
               // для клиента время устанавливается в MainForm.timerDataTimeUpdateInThread()
               if (!HMI_Settings.IsUDPSecondClient)
                  m_crzaTimeFC = netman.TimeFC;		// устанавливаем время из ФК
               // для вторичного клиента время устанавливается через wcf SecondClient
            }

            //--------------------------------------------------------------------

            // читаем первичные данные в KB

            if ( aFC.isLostConnection )
               continue;

            for ( int i = 0 ; i < aFC.Devices.Count ; i++ )
            {
               TCRZADirectDevice aDev = ( TCRZADirectDevice ) aFC.Devices[ i ];

               if ( aDev.crzaDeviceState == CRZADeviceState.CRZAdstOnline )
               {
                  // извлечем raw-пакеты из устройства
                  foreach ( MTDevice aBMRZ in MTD )
                     if( aDev.NumDev == aBMRZ.NDev && aBMRZ.ToString() != "FC_net" && aDev.NumFC == aBMRZ.NFC )
                     //if ( aDev.NumDev == aBMRZ.NDev )
                     {
                        aDev.CRZAMemDev = aBMRZ.memDev;
                        break;
                     }
               }
            }
         }
      }

      /// <summary>
      /// public void ResetGroup( int idFC, int idDev, int idGroup )
      /// сбросить группу (установить плохое качество тегов) idGroup устройства idDev, принадлежацего ФК idFC
      /// </summary>
      /// <param Name="idDev"></param>
      /// <param Name="idGroup"></param>
      public void ResetGroup( int idFC, int idDev, int idGroup )
      {
         foreach ( DataSource aFC in KB )
            if ( aFC.NumFC == idFC )
               foreach ( TCRZADirectDevice tdd in aFC )
               {
                  if ( tdd.NumDev == idDev )
                     if ( idGroup == 0xffff )
                     {
                        foreach ( TCRZAGroup tdg in tdd )
                        {
                           if ( tdg.Id == 0 )   // группу с состоянием шасу - не трогать
                              continue;

                           foreach ( TCRZAVariable tgv in tdg )
                           {
                              tgv.Quality = VarQuality.vqUndefined;
                              tgv.PlaceVarTo( MTD, VarQuality.vqUndefined );
                              tgv.firstRead = false;
                           }
                        }
                     }
                     else
                     {
                        for ( int j = 0 ; j < tdd.Groups.Count ; j++ )
                        {
                           TCRZAGroup aGroup = ( TCRZAGroup ) tdd.Groups[ j ];
                           if ( aGroup.Id == idGroup && aGroup.Id != 0 )       // группу с состоянием шасу - не трогать
                              for ( int p = 0 ; p < aGroup.Variables.Count ; p++ )
                              {
                                 TCRZAVariable aVariable = ( TCRZAVariable ) aGroup.Variables[ p ];
                                 aVariable.PlaceVarTo( MTD, VarQuality.vqUndefined );
                                 aVariable.firstRead = false;
                              }
                        }
                     }

               }
         //--------------------------------------------------------------------
         // перечитаем данные в KB
         foreach ( DataSource aFC in KB )
            for ( int i = 0 ; i < aFC.Devices.Count ; i++ )
            {
               TCRZADirectDevice aDev = ( TCRZADirectDevice ) aFC.Devices[ i ];

               // извлечем raw-пакеты из устройства
               foreach ( MTDevice aBMRZ in MTD )
                  if( aDev.NumDev == aBMRZ.NDev && aBMRZ.ToString() != "FC_net" && aDev.NumFC == aBMRZ.NFC )
                  {
                     aDev.CRZAMemDev = aBMRZ.memDev;
                     break;
                  }

               foreach ( TCRZAGroup aGroup in aDev )
                  foreach ( TCRZAVariable aVariable in aGroup )
                     aVariable.ExtractVarFrom( MTD );
            }
      }

      public void GetDataForce( )
      {
         //--------------------------------------------------------------------
         // перечитаем данные в KB
         foreach ( DataSource aFC in KB )
            for ( int i = 0 ; i < aFC.Devices.Count ; i++ )
            {
               TCRZADirectDevice aDev = ( TCRZADirectDevice ) aFC.Devices[ i ];

               // извлечем raw-пакеты из устройства
               foreach ( MTDevice aBMRZ in MTD )
                  if( aDev.NumDev == aBMRZ.NDev && aBMRZ.ToString() != "FC_net" && aDev.NumFC == aBMRZ.NFC )
                  //if ( aDev.NumDev == aBMRZ.NDev )
                  {
                     aDev.CRZAMemDev = aBMRZ.memDev;
                     break;
                  }

               foreach ( TCRZAGroup aGroup in aDev )
                  foreach ( TCRZAVariable aVariable in aGroup )
                  {
                     aVariable.firstRead = false;
                     aVariable.ExtractVarFrom( MTD );
                  }
            }
      }
      /// <summary>
      /// public bool ExecuteCommand( )
      /// принимает команду от верхнего уровня и передает устройству на низний уровень
      /// </summary>
      /// <param Name="iFC"></param>
      /// <param Name="iDev"></param>
      /// <param Name="cmd"></param>
      /// <param Name="parameters"></param>
      /// <param Name="aProgressBar"></param>
      /// <returns></returns>
      public bool ExecuteCommand( int iFC, int iDev, string cmd, string extraName, byte[ ] parameters, ToolStripProgressBar aProgressBar, StatusStrip statusStrip, Form prnt )
      {
         // ищем устройство
         foreach ( DataSource aFC in KB )
            if ( aFC.NumFC == iFC )
               for ( int i = 0 ; i < aFC.Devices.Count ; i++ )
               {
                  TCRZADirectDevice aDev = ( TCRZADirectDevice ) aFC.Devices[ i ];
                  if (aDev.NumDev == iDev)
                  {
                     aDev.ExecuteCommand(cmd, extraName, parameters, aProgressBar, statusStrip, prnt, aFC.IpAddrFC, aFC.NumFC); //.FCNet.numFC
                     return true;
                  }
               }
            else
               continue;

         return false;    // команда не выполнена
      }

      /// <summary>
      /// public void PacketToQueDev(byte[] pack, int adr , int iIDDev)
      ///       пакет  по адресу adr устройства iIDDev
      /// </summary>
      /// <param Name="pack"></param>
      /// <param Name="adr"></param>
      /// <param Name="iIDDev"></param>
      public void PacketToQueDev( byte[ ] pack, ushort adr,int iIDFC, int iIDDev )
      {
         /*транслируем пакет конкретному устройству 
          * здесь может быть 2 варианта дейтсвий:
          * - 1 пакет
          * - несколько пакетов по разным адресам
          * в первом случае адрес пакету присваивается то, что был
          * аргументах процедуры
          * во втором случае первому пакету из пачки
          * присваивается адрес из аргумента процедуры
          * адреса остальных пакетов извлекаются из тел самих пакетов
          */
         byte[] pack_small;
         int cur_pos = 0;
         
         // извлечем длину пакета
         if (pack == null)
            return;

         ushort lenPackSmall = BitConverter.ToUInt16(pack,cur_pos);
         pack_small = new byte[lenPackSmall];
         Buffer.BlockCopy( pack, cur_pos, pack_small, 0, lenPackSmall );
         // посылаем 1 пакет
         //netman.PacketToQueDev( pack, adr, iIDFC,iIDDev );
         netman.PacketToQueDev( pack_small, adr, iIDFC, iIDDev );
         cur_pos += lenPackSmall;

         // смотрим есть ли еще пакеты в пачке
         if( pack.Length > lenPackSmall )
         {
            while( cur_pos < pack.Length )
            {
               lenPackSmall = BitConverter.ToUInt16( pack, cur_pos );
               pack_small = new byte [ lenPackSmall ];
               Buffer.BlockCopy( pack, cur_pos, pack_small, 0, lenPackSmall );
               adr = BitConverter.ToUInt16( pack_small, 4 );
               netman.PacketToQueDev( pack_small, adr, iIDFC, iIDDev );
               cur_pos += lenPackSmall;
            }
         }
      }

      /// <summary>
      /// int GetCountLastPacketFromFC( int interval )
      /// оценка связи с ФК - число пакетов за интервал
      /// </summary>
      /// <param Name="interval"></param>
      /// <returns>число пакетов за интервал</returns>
      public int GetCountLastPacketFromFC( )
      {
         // транслируем
         return netman.GetCountLastPacketFromFC( );
      }
   }
}
