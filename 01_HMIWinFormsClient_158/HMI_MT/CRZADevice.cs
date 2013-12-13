/*############################################################################## *
 *    Copyright (C) 2007 Mehanotronika Corporation.                             *
 *    All rights reserved.                                                      *
 *	~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~*
 *                                                                              *
 *	Описание: Содержит классы представляющие ФК, устройства, логические группы  *
 *            переменных для включения в конфигурацию верхнего уровня.          *
 *                                                                              *
 *	Файл                     : CRZADevices.cs                                   *
 *	Тип конечного файла      : CRZADevices.dll                                  *
 *	версия ПО для разработки : С#, Framework 2.0                                *
 *	Разработчик              : Юров В.И.                                        *
 *	Дата начала разработки   : xx.02.2007                                       *
 *	Дата (v1.0)              :                                                  *
 *******************************************************************************
 * Изменения:
 * 1. Дата(Автор): ...cодержание...
 *#############################################################################*/

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

// признак качества переменной
/*public enum TVarQuality
{
	vqUndefined,    // не определено (не производилось ни одного чтения,
	// нет связи или неверная конфигурация
	vqGood,         // хорошее качество
	vqRangeError,   // выход за пределы диапазона
};*/

namespace CRZADevices
{
   // состояние устройства на верхнем уровне
   public enum CRZADeviceState
   {
      CRZAdstNone = 0,
      CRZAdstUndefine = 1, // нет связи с устройством
      //dstOffline,  // 
      //dstBlocked,  // 
      CRZAdstOnline = 2    // с устройством поддерживается нормальная связь
   }
   // делегат на изменение тега
   public delegate void ChVar();

   /// <summary>
   /// class Configurator
   /// класс, описывающий конкретную конфигурацию
   /// </summary>
   public class Configurator : INetManagerSink
   {
      // конфигурация
      public static ArrayList MTD; 
      public ArrayList KB;

      // поток чтения из ФК
      private Thread readFCThread;

		// признак потери связи
       //private bool isLostConnection = false;
		 private bool isLostConnection = false;
      public INetManager netman;

      // значение таймера обновления данных
       int timeInt = 0;

      //uint countActiveCmd = 0;  // счетчик незавершенных команд;

		 private DateTime m_crzaTimeFC;

		 BackgroundWorker bcw;	// поток, обновляющий данные верхнего уровня

		 public DateTime CRZATimeFC
		 {
			 get
			 {
				 return m_crzaTimeFC;
			 }
		 }

		 private bool m_crzaIsLoggerAlive;

		 public bool CRZA_IsLoggerAlive
		 {
			 set
			 {
				 m_crzaIsLoggerAlive = value;
				 netman.IsLoggerAlive = value;
			 }
			 get
			 {
				 return m_crzaIsLoggerAlive;
			 }
		 }

		 private int m_crzaLoggerTimer;

		 public int CRZA_LoggerTimer
		 {
			 get
			 {
				 return m_crzaLoggerTimer;
			 }
		 }	   

      /// <summary>
      /// конструктор
      /// </summary>
     
      public Configurator()
      {
         timeInt = 500; // таймер обновления данных (мсек)
         bcw = new BackgroundWorker();
         bcw.DoWork += new DoWorkEventHandler( ReceivePacketInThread );
      }

      private XPathNodeIterator GetNodeList( string exp, string FILE_NAME )
      {
         XPathDocument doc = new XPathDocument( FILE_NAME );
         XPathNavigator nav = doc.CreateNavigator();
         XPathNodeIterator myNodes;
         myNodes = nav.Select( exp );
         return myNodes;
      }

      /// <summary>
      /// public ArrayList GetConfigurator()
      /// Функция создает конфигурацию блоков
      /// </summary>
      /// <returns></returns>
      public ArrayList GetConfigurator(string pathPrjCfgFile, string role)
      {
         // конфигурация блоков
         KB = new ArrayList( );

         // логическая конфигурация устройств
         string path = Environment.CurrentDirectory + Path.DirectorySeparatorChar + "CRZADevices.dll";
         if( !File.Exists( path ) )
            throw new Exception( "Файл " + path + " не существует" );

         // получаем сборку
         Assembly theAssembly = Assembly.LoadFile( path );
   		
         // перечисляем устройства и создаем экземпляры соответсвующих классов
         
         // открываем файл конфигурации устройств проекта
         string pathToF = Environment.CurrentDirectory + Path.DirectorySeparatorChar + "Project" + Path.DirectorySeparatorChar + "Project.cfg";
         
         if( !File.Exists( pathToF ) )
            throw new Exception( "Файл " + pathToF + " не существует" );

         XmlTextReader reader = new XmlTextReader( pathToF );
         XmlDocument doc = new XmlDocument();
         doc.Load( reader );
         reader.Close();
         
         // выделим узел по условию
         XmlNode xn;
         XmlElement root = doc.DocumentElement;
         xn = root.SelectSingleNode( "/Project/DeviceConfig/PathDeviceConfig" );
         string PathToFDC = xn.InnerText;	// файл конфигурации устройств проекта
         string cdir = Environment.CurrentDirectory;
         
         if( !File.Exists( PathToFDC ) )
         {
            MessageBox.Show( "Файл конфигурации устройств проекта не найден (уровень АСУ)", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error );
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.DefaultExt = "cdp";
            openFileDialog1.Filter = "Файл конфигурации устройств проекта(*.cdp)|*.cdp";
            openFileDialog1.InitialDirectory = Application.StartupPath;	// Application.StartupPath;
            if( DialogResult.OK != openFileDialog1.ShowDialog() )
            {
               throw new Exception( "Не задан файл конфигурации устройств проекта " );
            }
            PathToFDC = openFileDialog1.FileName;
            //запомнить файл
            if( DialogResult.Yes == MessageBox.Show( "Запомнить файл конфигурации для будущего использования", "Сообщение", MessageBoxButtons.YesNo, MessageBoxIcon.Question ) )
            {
               // запоминаем в файле проекта - смотрим есть ли в файле проекта путь к файлу с конфигурацией устройств
               XmlTextReader rreader = new XmlTextReader( pathToF );
               XmlDocument rdoc = new XmlDocument();
               rdoc.Load( rreader );
               rreader.Close();

               // выделим узел по условию
               XmlNode rxn;
               XmlElement rroot = rdoc.DocumentElement;
               rxn = rroot.SelectSingleNode( "/Project/DeviceConfig/PathDeviceConfig" );
               rxn.InnerText = PathToFDC;
               rdoc.Save( pathToF );
            }
         }
         Environment.CurrentDirectory = cdir;
         // теперь нужно получать список узлов с описанием ФК и устройств проекта
         XPathNodeIterator xpni;
         XPathNodeIterator xpniCh;
         XPathNavigator cd;
         XPathNavigator cdd;

         xpni = GetNodeList( "/MT/Configuration/FC", PathToFDC );	// список узлов с ФК
         Type blockType;
         Type[] argTypes;
         ConstructorInfo ctor;
         int numCurFC;
         int numCurDev;
         string strAdrFC = String.Empty;

         while( xpni.MoveNext() )
         {
            cd = xpni.Current;
            // формируем описание ФК
            // получим тип 
            blockType = theAssembly.GetType( "CRZADevices.FC");
            argTypes = new Type[] { typeof( int ), typeof(string) };
            ctor = blockType.GetConstructor( argTypes );

            numCurFC = Convert.ToInt32( cd.GetAttribute( "numFC", "" ) );
            strAdrFC = cd.GetAttribute( "fcadr", "" );
            //вызываем конструктор
            FC fc = ( FC ) ctor.Invoke( new object[] { numCurFC, strAdrFC} );	// FC FC1 = new FC(0);  //создаем ФК
            //   					 Convert.ToInt32( cd.GetAttribute( "numdev", "" ) ) 
            // получим тип 
            blockType = theAssembly.GetType( "CRZADevices." + cd.GetAttribute( "nameEHighLevel", "" ) );
            argTypes = new Type[] { typeof( int ), typeof( int ), typeof( int ), typeof( string ) };
            ctor = blockType.GetConstructor( argTypes );
            //FCasDev dev0_FC = new FCasDev(0,0, 0, "ФК 0");
            fc.Devices.Add( ctor.Invoke( new object[] { Convert.ToInt32( cd.GetAttribute( "numdev", "" )), 0, 0, cd.GetAttribute( "describe", "" )  } ) );	
   					
            // теперь пробежим по устройствам данного ФК и добавим их
            xpniCh = cd.Select( "FCDevices/Device" );
            while( xpniCh.MoveNext() )
            {
               cdd = xpniCh.Current;
               if( cdd.GetAttribute( "enable", "" ) == "False" )
                  continue;	// пропускаем устройство

               // формируем номер устройства с учетом его ФК
               numCurDev = Convert.ToInt32( cdd.SelectSingleNode( "NumDev" ).Value );
               numCurDev += numCurFC * 256;

               // формируем описание устройства
               // получим тип 
               blockType = theAssembly.GetType( "CRZADevices." + cdd.SelectSingleNode( "nameEHighLevel" ).Value );
               argTypes = new Type[] { typeof( int ), typeof( int ), typeof( int ), typeof( int ), typeof( string ) };
               ctor = blockType.GetConstructor( argTypes );
               //вызываем конструктор
               fc.Devices.Add( ctor.Invoke( new object[] { numCurFC,
                                                            numCurDev,//Convert.ToInt32( cdd.SelectSingleNode( "NumDev" ).Value )
                                                            Convert.ToInt32( cdd.SelectSingleNode( "NumLock" ).Value ),
                                                            Convert.ToInt32( cdd.SelectSingleNode( "NumSec" ).Value ),
                                                            cdd.SelectSingleNode( "DescDev" ).Value } ) );
            }
            
            // добавляем ФК к конфигурации блоков
            KB.Add(fc);				
         }
         
         // запускаем процесс обновления - настраиваем интерфейсную ссылку на источних первичных (raw) данных
            switch( role )
            {
               case "IsTCPServer":

         NetNetManager nnetman = new NetNetManager();
         netman = nnetman;
                  break;
               case "IsTCPClient":                  
                  NetTcpFromUdpManager ntcpnetman = new NetTcpFromUdpManager( );
                  netman = ntcpnetman;
                  break;
               default:
                  NetNetManager nnmnetman = new NetNetManager( );
                  netman = nnmnetman;
                  break;
            }


         MTD = netman.SetConfig( pathPrjCfgFile );
         netman.Advise(this);

         // создаем поток чтения из ФК в массив raw-данных MTD
         readFCThread = new Thread(new ThreadStart(netman.getdata));
         readFCThread.Name = "ReaderFC";
         readFCThread.Start();

         // связываем для оповещения
         //// все устройства в online
         foreach( FC aFC in KB )
            for( int i = 0 ;i < aFC.Devices.Count ;i++ )
            {
               TCRZADirectDevice aDev = ( TCRZADirectDevice ) aFC.Devices[ i ];
               foreach( BMRZ aBMRZ in MTD )
                  if( aDev.NumDev == aBMRZ.NDev )
                  {
                     aBMRZ.OnChangeNetDevState += aDev.ChCrzaDevStat;
                     aBMRZ.NetDeviceState = NetDeviceState.netdstOnline;
                     break;
                  }
               aDev.crzaDeviceState = CRZADeviceState.CRZAdstOnline;
            }
            
            // связываем для обновления переменных
            foreach( FC aFc in KB )
               foreach( TCRZADirectDevice tdd in aFc )
                  foreach( BMRZ aBMRZ in MTD )
                     if( tdd.NumDev == aBMRZ.NDev )
                       foreach( TCRZAGroup tdg in tdd )
                        foreach( TCRZAVariable tgv in tdg )
                           if( aBMRZ.varDev[ tgv.RegInDev ] is Real_FieldMT )
                              ( ( Real_FieldMT ) aBMRZ.varDev[ tgv.RegInDev ] ).OnChangeNetVarState += tgv.ExtractVarFrom;
                           else if( aBMRZ.varDev[ tgv.RegInDev ] is Int_FieldMT )
                              ( ( Int_FieldMT ) aBMRZ.varDev[ tgv.RegInDev ] ).OnChangeNetVarState += tgv.ExtractVarFrom;
                           else if( aBMRZ.varDev[ tgv.RegInDev ] is Bit_FieldMT )
                              ( ( Bit_FieldMT ) aBMRZ.varDev[ tgv.RegInDev ] ).OnChangeNetVarState += tgv.ExtractVarFrom;
                           else if( aBMRZ.varDev[ tgv.RegInDev ] is UInt_FieldMT )
                              ( ( UInt_FieldMT ) aBMRZ.varDev[ tgv.RegInDev ] ).OnChangeNetVarState += tgv.ExtractVarFrom;
                           else if( aBMRZ.varDev[ tgv.RegInDev ] is DInt_FieldMT )
                              ( ( DInt_FieldMT ) aBMRZ.varDev[ tgv.RegInDev ] ).OnChangeNetVarState += tgv.ExtractVarFrom;
                           else if( aBMRZ.varDev[ tgv.RegInDev ] is UDInt_FieldMT )
                              ( ( UDInt_FieldMT ) aBMRZ.varDev[ tgv.RegInDev ] ).OnChangeNetVarState += tgv.ExtractVarFrom;
                           else if( aBMRZ.varDev[ tgv.RegInDev ] is Byte_FieldMT )
                              ( ( Byte_FieldMT ) aBMRZ.varDev[ tgv.RegInDev ] ).OnChangeNetVarState += tgv.ExtractVarFrom;
                           else if( aBMRZ.varDev[ tgv.RegInDev ] is DateTime4_FieldMT )
                              ( ( DateTime4_FieldMT ) aBMRZ.varDev[ tgv.RegInDev ] ).OnChangeNetVarState += tgv.ExtractVarFrom;
                           else if( aBMRZ.varDev[ tgv.RegInDev ] is DateTime3_FieldMT )
                              ( ( DateTime3_FieldMT ) aBMRZ.varDev[ tgv.RegInDev ] ).OnChangeNetVarState += tgv.ExtractVarFrom;
                           else if( aBMRZ.varDev[ tgv.RegInDev ] is DateTimeUTC_FieldMT )
                              ( ( DateTimeUTC_FieldMT ) aBMRZ.varDev[ tgv.RegInDev ] ).OnChangeNetVarState += tgv.ExtractVarFrom;
                           else if( aBMRZ.varDev[ tgv.RegInDev ] is Stringz_FieldMT )
                              ( ( Stringz_FieldMT ) aBMRZ.varDev[ tgv.RegInDev ] ).OnChangeNetVarState += tgv.ExtractVarFrom;
                           else if( aBMRZ.varDev[ tgv.RegInDev ] is BCDPack_FieldMT )
                              ( ( BCDPack_FieldMT ) aBMRZ.varDev[ tgv.RegInDev ] ).OnChangeNetVarState += tgv.ExtractVarFrom;
                           else if( aBMRZ.varDev[ tgv.RegInDev ] is BCD_FieldMT )
                              ( ( BCD_FieldMT ) aBMRZ.varDev[ tgv.RegInDev ] ).OnChangeNetVarState += tgv.ExtractVarFrom;

         // связываем фк как устройство с остальными устройствами данного фк
         foreach( FC aFc in KB )
         {
            aFc.FCNet = (FC_net) netman.IP_FCs[aFc.IpAddrFC];  // привязываемся к описанию фк нижнего уровня
            aFc.isFCConnection = true; // для начала устанавливаем, что связь есть

            foreach( TCRZADirectDevice tdd in aFc )
            {
               if( tdd.NumDev == 0 ) // то есть это фк-устройство
               {
                  FCasDev fcd = ( FCasDev ) tdd;
                  fcd.Devices = aFc.Devices;
               }
            }
         }

         return KB;      
      }
       public void SendPacketsToFC( byte[] arrpacket)
      {
         netman._listener_NewMessage( arrpacket );
      }


      /// <summary>
      /// public void SetIPLogger( string ip )
      /// установить адрес логгера
      /// </summary>
      /// <param name="ip"></param>
      public void SetIPLogger( string ip )
      {
         netman.SetIP_Logger( ip );
      }

      /// <summary>
      /// ПРИНУДИТЕЛЬНОЕ чтение пакетов с нижнего на верхний уровень - это нужно в ситуации, когда устройства нет,
      /// а историческая информация по нему есть
      /// </summary>
      /// <param name="nFC"></param>
      /// <param name="idDev"></param>
      public void ReceivePacketForce(int nFC, int idDev)
      {
         foreach( FC aFC in KB )
         if( aFC.NumFC == nFC )
            for( int i = 0 ;i < aFC.Devices.Count ;i++ )
            {
               TCRZADirectDevice aDev = ( TCRZADirectDevice ) aFC.Devices[ i ];
               if( aDev.NumDev == idDev )
               {
                  // извлечем raw-пакеты из устройства
                  foreach( BMRZ aBMRZ in MTD )
                  if( aDev.NumDev == aBMRZ.NDev )
                  {
                     aDev.CRZAMemDev = aBMRZ.memDev;
                     break;
                  }

                  foreach( TCRZAGroup aGroup in aDev )
                     foreach( TCRZAVariable aVariable in aGroup )
                     {
                        aVariable.SetQuality( VarQuality.vqArhiv );
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
      public void ReceivePacket()
      {
         while(! bcw.IsBusy )
            bcw.RunWorkerAsync();
      }
      private void ReceivePacketInThread(object sender, DoWorkEventArgs e)
      {
         //--------------------------------------------------------------------
         // проверяем связь с ФК - если связи нет то обнуляем данные по этому фк
         //int cn =  netman.GetCountLastPacketFromFC();
         //for( int ij = 0 ;ij < netman.IP_FCs.Count ;ij++ )
         foreach( FC aFC in KB )
         {
            if( !aFC.IsFCConnectHMI() )
            {
               if( !aFC.isLostConnection )   // предотвращаем повторную очистку
               {
                  aFC.isLostConnection = true;	//  связи потеряна

                  // обнуляем теги
                  for( int i = 0 ;i < aFC.Devices.Count ;i++ )
                  {
                     TCRZADirectDevice aDev = ( TCRZADirectDevice ) aFC.Devices[ i ];
                     foreach( BMRZ aBMRZ in MTD )
                        if( aDev.NumDev == aBMRZ.NDev )
                        {
                           aDev.CRZAMemDev = aBMRZ.memDev;
                           break;
                        }
                     foreach( TCRZAGroup aGroup in aDev )
                        foreach( TCRZAVariable aVariable in aGroup )
                        {
                           if( aVariable.Quality != VarQuality.vqArhiv )
                              aVariable.SetQuality( VarQuality.vqUndefined );

                           aVariable.ExtractVarFrom( MTD );
                        }
                   }
                }  
            }
            else
            {
               if( aFC.isLostConnection )	// если связь была потеряна
               {
                  aFC.isLostConnection = false;
                  // устанавливаем всем тегам хорошее качество
                  foreach( TCRZADirectDevice tdd in aFC )
                  {
                     tdd.crzaDeviceState = CRZADeviceState.CRZAdstOnline;

                     foreach( TCRZAGroup tdg in tdd )
                        foreach( TCRZAVariable tgv in tdg )
                           tgv.SetQuality( VarQuality.vqGood );
                  }
                  netman.ClearNetPackQ( );
               }
               m_crzaTimeFC = netman.TimeFC;		// устанавливаем время из ФК
            }
         
         //--------------------------------------------------------------------
         // состояние логгера
         m_crzaIsLoggerAlive = netman.IsLoggerAlive;	// живет ли логгер
         m_crzaLoggerTimer = netman.LoggerTimer;		// таймер логгера

         // читаем первичные данные в KB

         if( aFC.isLostConnection )
            continue;
            
         for( int i = 0 ; i < aFC.Devices.Count ; i++ )
         {
            TCRZADirectDevice aDev = ( TCRZADirectDevice ) aFC.Devices[i];

            if( aDev.crzaDeviceState == CRZADeviceState.CRZAdstOnline )
            {
               // извлечем raw-пакеты из устройства
               foreach( BMRZ aBMRZ in MTD )
               if( aDev.NumDev == aBMRZ.NDev )
               {
                  aDev.CRZAMemDev = aBMRZ.memDev;
                  break;
               }
               continue;

                  // данные теперь читаются по мере их прихода
                  //foreach( TCRZAGroup aGroup in aDev )
                  //   foreach( TCRZAVariable aVariable in aGroup )
                  //      aVariable.ExtractVarFrom( MTD );
               }
            }			 
        }
      }

      /// <summary>
      /// public void ResetGroup( int idFC, int idDev, int idGroup )
      /// сбросить группу (установить плохое качество тегов) idGroup устройства idDev, принадлежацего ФК idFC
      /// </summary>
      /// <param name="idDev"></param>
      /// <param name="idGroup"></param>
      public void ResetGroup( int idFC, int idDev, int idGroup )
      {
         foreach( FC aFC in KB )
         if( aFC.NumFC == idFC )
            foreach( TCRZADirectDevice tdd in aFC )
            {
               if( tdd.NumDev == idDev )
                  if( idGroup == 0xffff )
                  {
                     foreach( TCRZAGroup tdg in tdd )
                     {
                        if( tdg.Id == 0 )   // группу с состоянием шасу - не трогать
                        continue;

                        foreach( TCRZAVariable tgv in tdg )
                        {
                           tgv.SetQuality( VarQuality.vqUndefined );
                           tgv.PlaceVarTo( MTD, VarQuality.vqUndefined );
                        }
                     }
                  }
                  else
                  {
                     for( int j = 0 ; j < tdd.Groups.Count ; j++ )
                     {
                        TCRZAGroup aGroup = ( TCRZAGroup ) tdd.Groups[j];
                        if( aGroup.Id == idGroup && aGroup.Id != 0 )       // группу с состоянием шасу - не трогать
                           for( int p = 0 ; p < aGroup.Variables.Count ; p++ )
                           {
                              TCRZAVariable aVariable = ( TCRZAVariable ) aGroup.Variables[p];
                              aVariable.PlaceVarTo( MTD, VarQuality.vqUndefined );
                           }
                     }
                  }
   					 
            }
            //--------------------------------------------------------------------
            // перечитаем данные в KB
            foreach( FC aFC in KB )
            for( int i = 0 ; i < aFC.Devices.Count ; i++ )
            {
               TCRZADirectDevice aDev = ( TCRZADirectDevice ) aFC.Devices[i];

               // извлечем raw-пакеты из устройства
               foreach( BMRZ aBMRZ in MTD )
                  if( aDev.NumDev == aBMRZ.NDev )
                  {
                     aDev.CRZAMemDev = aBMRZ.memDev;
                     break;
                  }

               foreach( TCRZAGroup aGroup in aDev )
                  foreach( TCRZAVariable aVariable in aGroup )
                     aVariable.ExtractVarFrom( MTD );
            }
      }

      /// <summary>
      /// public bool ExecuteCommand( )
      /// принимает команду от верхнего уровня и передает устройству на низний уровень
      /// </summary>
      /// <param name="iFC"></param>
      /// <param name="iDev"></param>
      /// <param name="cmd"></param>
      /// <param name="parameters"></param>
      /// <param name="aProgressBar"></param>
      /// <returns></returns>
      public bool ExecuteCommand( int iFC, int iDev, string cmd, string extraName, byte[] parameters, ToolStripProgressBar aProgressBar, StatusStrip statusStrip, Form prnt )
      {
         // ищем устройство
         foreach( FC aFC in KB )
            if( aFC.NumFC == iFC )
               for( int i = 0; i < aFC.Devices.Count; i++ )
               {
                  TCRZADirectDevice aDev = ( TCRZADirectDevice ) aFC.Devices[i];
                  if( aDev.NumDev == iDev )
                     return aDev.ExecuteCommand( netman, cmd, extraName, parameters, aProgressBar, statusStrip, prnt );
               }
            else
               continue;

         return false;    // команда не выполнена
      }

      /// <summary>
      /// public void PacketToQueDev(byte[] pack, int adr , int iIDDev)
      ///       пакет  по адресу adr устройства iIDDev
      /// </summary>
      /// <param name="pack"></param>
      /// <param name="adr"></param>
      /// <param name="iIDDev"></param>
      public void PacketToQueDev( byte[] pack, ushort adr, int iIDDev )
      {
         // транслируем 
         netman.PacketToQueDev( pack, adr , iIDDev);
      }

      /// <summary>
      /// int GetCountLastPacketFromFC( int interval )
      /// оценка связи с ФК - число пакетов за интервал
      /// </summary>
      /// <param name="interval"></param>
      /// <returns>число пакетов за интервал</returns>
      public int GetCountLastPacketFromFC( )
      {
         // транслируем
         return netman.GetCountLastPacketFromFC();
      }
   }

	/// <summary>
	/// class FC
	/// класс, описывающий функциональный контроллер
	/// </summary>
	public class FC : IEnumerable
	{
		public int NumFC;          // номер функционального контроллера
      public string IpAddrFC;    // ip-адрес фк
		public ArrayList Devices;  // массив устройств
      public FC_net FCNet;       // ссылка на описание фк нижнего уровня
      public bool isLostConnection;  // признак потери связи ( для работы с пакетами )
      public bool isFCConnection;  // признак потери связи для MainForm, чтобы не формировать лишние сообщения
        // признак подлежит ли устройство сбору пакетов для пересылки на удаленный АРМ
        public bool isCollectDataForRemoteARM = false;


      /// <summary>
		/// конструктор
		/// </summary>
		/// <param name="numFC"></param>
		public FC( int numFC, string ipadrFC )
		{
         this.Devices = new ArrayList();
			NumFC = numFC;
         IpAddrFC = ipadrFC;
		}

		/// <summary>
		/// public IEnumerator GetEnumerator()
		/// энумератор устройств
		/// </summary>
		/// <returns></returns>
		public IEnumerator GetEnumerator( )
		{
			return Devices.GetEnumerator();
		}
		/// <summary>
		/// public int GetCountLastPacketFromFC( int interval )
		/// оценка качества связи с ФК
		/// </summary>
		/// <param name="interval"></param>
		/// интервал оценки (в миллисекундах)
		/// <returns> количество пакетов полученных за интервал от ФК - если 0, то связи с ФК нет</returns>
		public bool IsFCConnectHMI( )
		{
			return FCNet.isFCConnectNET();
		}
	}

   /// <summary>
   /// class CRZADevice
   /// класс, описывающий устройство
   /// </summary>
   public class CRZADevice
	{
	}

	public enum DeviceState
	{
		dstNone = 0,
		dstUndefine = 1, // нет связи с устройством
		//dstOffline,  // 
		//dstBlocked,  // 
		dstOnline = 2,    // с устройством поддерживается нормальная связь
	}
	#region class TCRZADirectDevice
	/// <summary>
   /// class CRZADevice
   /// класс, описывающий устройство БМРЗ
   /// </summary>
   public class TCRZADirectDevice : CRZADevice, IEnumerable
	{
      public DeviceState StateDev;	// состояние устройства
      public bool isRemont = false;	// устройство в ремонтном состоянии?
      public int NumFC;             // номер ФК
      public int NumDev;
      public int NumSlot;				// номер ячейки
      public string RefDesign;			// условное название устройства, определяет пользователь вместо его типа
      public ArrayList Groups;			// группы устройства
      public ArrayList Commands;		// команды устройства
      public SortedList CRZAMemDev;  // память устройства
      public string LastCommand = String.Empty;	//последняя выполненная команда
      public string LastCommandFC = String.Empty;	//последняя выполненная команда ФК
      
      public int CodeCmdFailure = 0;	// код отказа в выполнении команды
      public CRZADeviceState crzaDeviceState;	// состояние устройства на верхнем уровне
        // признак подлежит ли устройство сбору пакетов для пересылки на удаленный АРМ
        public bool isCollectDataForRemoteARM = false;


      /// <summary>
      /// конструктор 
      /// </summary>
      /// <param name="numDev"></param>
      /// <param name="numSlot"></param>
      public TCRZADirectDevice(int numFC, int numDev, int numSlot, int numSection, string refdes)
      {
         NumFC = numFC;
         NumDev = numDev;
         NumSlot = numSlot;
         RefDesign = refdes;
         CRZAMemDev = new SortedList();

         Configure();
      }

      /// <summary>
      /// public virtual void Configure()
      /// конфигурирование устройства
      /// </summary>
      public virtual void Configure()
		{
		}

      /// <summary>
      /// public IEnumerator GetEnumerator()
      /// энумератор для foreach
      /// </summary>
      /// <returns></returns>
      public IEnumerator GetEnumerator()
      {
         return Groups.GetEnumerator();
      }

      /// <summary>
      /// public bool ExecuteCommand()
      /// выполнение команд
      /// </summary>
      /// <param name="netman"></param>
      /// <param name="cmd"></param>
      /// <param name="parameters"></param>
      /// <param name="aProgressBar"></param>
      /// <returns></returns>
      public bool ExecuteCommand( INetManager netman, string cmd, string extraName, byte[] parameters, ToolStripProgressBar aProgressBar, StatusStrip statusStrip, Form prnt )
      {
         //ищем команду в устройстве и передаем ему на выполнение
         foreach( TCRZACommand aCmd in Commands )
         {
            if( aCmd.Name == cmd )
               return aCmd.Execute( netman, aCmd.Name, extraName, parameters, aProgressBar, statusStrip, prnt );
         }

         return false;     // неудачное выполнение команды
      }

      /// <summary>
      /// для отслеживания последней выполняемой команды
      /// </summary>
      /// <param name="Value"></param>
      /// <param name="format"></param>
      public void LinkSetText( object Value, string format )
		{
         // извлечь имя команды
         foreach( TCRZAGroup gr in Groups )
            if( gr.Id == 0 )
            {
               foreach( TCRZAVariable var in gr.Variables )
                  if( var.RegInDev == 60058 )
                  {
                     TStringVariable strVarVal = ( TStringVariable ) var;
                     LastCommand = strVarVal.Value;
                  }
                  else if( var.RegInDev == 60056 )
                  {
                     TIntVariable icf = ( TIntVariable ) var;
                     CodeCmdFailure = icf.Value;
                  }
            }
            else if( gr.Id == 1 ) // для команд FC
               foreach( TCRZAVariable var in gr.Variables )
                  if( var.RegInDev == 58 )
                  {
                     TStringVariable strVarVal = ( TStringVariable ) var;
                     LastCommandFC = strVarVal.Value;

                     FCasDev devFC = ( FCasDev ) this;                     
                     foreach( TCRZADirectDevice dev in devFC.Devices )
                        dev.LastCommandFC = LastCommandFC;
                  }
      }

      /// <summary>
      /// callback-функция для события изменения состояния устройства на нижнем уровне
      /// </summary>
      /// <param name="nfc"></param>
      /// <param name="ndev"></param>
      /// <param name="ndstat"></param>
      public void ChCrzaDevStat( int nfc, int ndev, NetDeviceState ndstat)
      {
         switch(ndstat)
         {
            case NetDeviceState.netdstNone:
               crzaDeviceState = CRZADeviceState.CRZAdstNone;
               //StateDev = DeviceState.dstNone;
               break;
            case NetDeviceState.netdstOnline:
               crzaDeviceState = CRZADeviceState.CRZAdstOnline;
               foreach( TCRZAGroup tcg in Groups )
                  foreach( TCRZAVariable tcv in tcg.Variables )
                     tcv.SetQuality( VarQuality.vqGood );
               //StateDev = DeviceState.dstOnline;
               break;
            case NetDeviceState.netdstUndefine:
               crzaDeviceState = CRZADeviceState.CRZAdstUndefine;
               foreach( TCRZAGroup tcg in Groups )
               {
                  foreach( TCRZAVariable tcv in tcg.Variables )
                  tcv.SetQuality( VarQuality.vqUndefined );
               }
               //StateDev = DeviceState.dstUndefine;
               break;
            default:
               break;
         }
      }
	}
	#endregion
	#region ФК как устройство
	/// <summary>
   /// class FC
   /// класс, описывающий функциональный контроллер
   /// </summary>
   public class FCasDev : TCRZADirectDevice, IEnumerable
   {
      public int NumFC;           // номер функционального контроллера
	   public ArrayList Devices;   // массив устройств

	   /*public ArrayList Groups;      // группы устройства
	   public ArrayList Commands;    // команды устройства
	   public SortedList CRZAMemDev;    // память устройства*/

	   /// <summary>
	   /// конструктор
	   /// </summary>
	   /// <param name="numFC"> - номер ФК</param>
	   /// <param name="numSlot"> - номер ячейки - для фк не имеет значения</param>          
	   public FCasDev( int numFC, int numSlot, int numSection, string refdes )
			: base( numFC, 0, numSlot, numSection, refdes )
	   {
		   this.Devices = new ArrayList();
		   NumFC = numFC;
	   }

	   /// <summary>
	   /// public IEnumerator GetEnumerator()
	   /// энумератор устройств
	   /// </summary>
	   /// <returns></returns>
	   public IEnumerator GetEnumerator( )
	   {
		   return Devices.GetEnumerator();
      }
	   /// <summary>
	   /// public int GetCountLastPacketFromFC( int interval )
	   /// оценка качества связи с ФК
	   /// </summary>
	   /// <param name="interval"></param>
	   /// интервал оценки (в миллисекундах)
	   /// <returns> количество пакетов полученных за интервал от ФК - если 0, то связи с ФК нет</returns>
	   public int GetCountLastPacketFromFC( int interval )
	   {
		   return 1;
	   }

	   /// <summary>
	   /// public virtual void Configure()
	   /// конфигурирование устройства FC
	   /// </summary>
	   public override void Configure( )
	   {
		   // создаем логические группы для переменных:
		   Groups = new ArrayList();  // создает массив групп устройства

		   TCRZAGroup G;

		   // создаем массив для хранения команд:
		   Commands = new ArrayList();

		   TCRZACommand Cmd;

		   // группа "Сведения о принятых пакетах, слове состояния и идентификаторе ECU"
		   G = new TCRZAGroup( this, "Сведения о принятых пакетах, слове состояния и идентификаторе ECU", 0 );
		   // добавляем переменные
			G.Variables.Add( new TBitFieldVariable( G, 00000, "0001", "Повторное чтение данных счетчиков", "", "" ) ); // 0 - запрещено; 1 - разрешено
			G.Variables.Add( new TBitFieldVariable( G, 00000, "0002", "Повторное чтение данных максметра", "", "" ) ); // 0 - запрещено; 1 - разрешено

		   // три бита значения, представляющего состояние связи с приемником СНС (рассшифровка в документации)
			G.Variables.Add( new TBitFieldVariable( G, 00000, "0100", "бит 08", "", "" ) );
			G.Variables.Add( new TBitFieldVariable( G, 00000, "0200", "бит 09", "", "" ) );
			G.Variables.Add( new TBitFieldVariable( G, 00000, "0400", "бит 10", "", "" ) );

		   G.Variables.Add( new TUIntVariable( G, 00001, "Счетчик пакетов неизвестного формата", "", "" ) );
		   G.Variables.Add( new TUDIntVariable( G, 00002, "IP-адрес ведущего", "", "" ) );
		   G.Variables.Add( new TUDIntVariable( G, 00004, "IP-адрес отправителя", "", "" ) );
		   G.Variables.Add( new TUIntVariable( G, 00006, "Серия №", "", "" ) );
		   G.Variables.Add( new TUIntVariable( G, 00007, "Пакет №", "", "" ) );
		   G.Variables.Add( new TDateTimeVariable( G, 00008, "Дата поступления по часам ФК", "", "" ) );
		   G.Variables.Add( new TUIntVariable( G, 00010, "Количество RTU", "", "" ) );
		   G.Variables.Add( new TStringVariable( G, 00011, "Идентификатор ECU", "", "" ) );

		   Groups.Add( G );

		   // группа "Данные о выполнении команд верхнего уровня АСУ"
		   G = new TCRZAGroup( this, "Данные о выполнении команд верхнего уровня АСУ", 1 );
		   // добавляем переменные
		   G.Variables.Add( new TUDIntVariable( G, 00050, "IP-адрес отправителя", "", "" ) );
		   G.Variables.Add( new TUIntVariable( G, 00052, "Серия №", "", "" ) );
		   G.Variables.Add( new TUIntVariable( G, 00053, "Пакет №", "", "" ) );
		   G.Variables.Add( new TDateTimeVariable( G, 00054, "Дата поступления по часам ФК", "", "" ) );
		   G.Variables.Add( new TIntVariable( G, 00056, "Причина отказа от выполнения", "", "" ) );
		   G.Variables.Add( new TIntVariable( G, 00057, "Состояние выполнения команды", "", "" ) );
		   G.Variables.Add( new TStringVariable( G, 00058, "Имя команды", "", "" ) );
		   G.Variables.Add( new TUIntVariable( G, 00060, "Первый параметр команды", "", "" ) );
		   G.Variables.Add( new TUIntVariable( G, 00061, "Второй параметр команды", "", "" ) );

		   Groups.Add( G );

		   //---------------------------------------------------------------------
		   // добавляем команды
		   Cmd = new TCRZACommand( this, "NOP", "Пустая команда" );
		   Commands.Add( Cmd );
		   Cmd = new TCRZACommand( this, "SNC", "Сигнал уведомления о соединении" );
		   Commands.Add( Cmd );
		   Cmd = new TCRZACommand( this, "SPC", "Установить период чтения накопителя" );
		   Commands.Add( Cmd );
		   Cmd = new TCRZACommand( this, "SPM", "Установить период чтения максметра" );
		   Commands.Add( Cmd );
			Cmd = new TCRZACommand( this, "GMT", "Установить часы ШАСУ" );
			Commands.Add( Cmd );
	   }
	   public override string ToString( )
	   {
		   return "ФК";
	   }
   }
	#endregion
   
	#region БМРЗ ВВ-14-31-12

   #endregion

	#region class TCRZAGroup
	/// <summary>
   /// public class TCRZAGroup
   /// класс для представления группы переменных
   /// </summary>
   public class TCRZAGroup : IEnumerable
	{
      public string   Name;                      // имя группы
      public int      Id;                        // целочисленный Id группы
      public TCRZADirectDevice  Device;   // устройство с которым связана группа
      public ArrayList Variables;                // переменные группы

      /// <summary>
      /// конструктор 
      /// </summary>
      /// <param name="gDevice"></param>
      /// <param name="gName"></param>
      /// <param name="gid"></param>
      public TCRZAGroup(TCRZADirectDevice gDevice, string gName, int gid)
      {
         Device   = gDevice;
         Name     = gName;
         Id       = gid;
         Variables = new ArrayList();
      }
      public IEnumerator GetEnumerator()
      {
         return Variables.GetEnumerator();
      }
       
      /// <summary>
      /// public SetQualityGroup(VarQuality vq )
      /// установить качество тегов данной группы
      /// </summary>
      public void SetQualityGroup(VarQuality vq )
      {
      foreach(TCRZAVariable var in Variables)
      var.SetQuality(vq);
      }
   }
   #endregion
   /// <summary>
	///	Признак качества переменной 
	/// </summary>
	public enum VarQuality
	{
		vqUndefined,      // Не определено (не производилось ни одного чтения,
                        // нет связи или неверная конфигурация
		vqGood,           // Хорошее качество
      vqArhiv,          // архивная переменная (из БД)
		vqRangeError,     // Выход за пределы диапазона
	};
	#region class TCRZAVariable
	/// <summary>
    /// class TCRZAVariable
    /// класс для представления переменной
    /// </summary>
    public class TCRZAVariable
	{
        // объявляем событие
      public virtual event ChVar OnChangeVar;

      public int RegInDev;       //адрес переменной в устройстве
		// подпись переменной
		public string Caption;
		// размерность переменной
		public string Dim;
		// имя переменной
		public string Name;
		// массив строк для ассоциирования с целочисл. значением
		public ArrayList ArrLiStringCB; 
		// качество переменной
		public VarQuality Quality;
		// признак первого чтения
		public bool firstRead;

      public virtual void SetQuality(VarQuality vq )
      {
         if( Quality != VarQuality.vqArhiv )
            Quality = vq;
         if( Quality == VarQuality.vqArhiv && vq == VarQuality.vqGood )
            Quality = vq;
      }
        
      public virtual void ExtractVarFrom(ArrayList MTD)
      {
      }

      /// <summary>
      /// перегруженная функция извлечения значения с нижнего уровня по событию
      /// </summary>
      /// <param name="varval">значение переменной</param>
      /// <param name="memb">массив байт представления переменной</param>
      /// <param name="lenR">длина в рег. MODBUS</param>
      /// <param name="lenB">длина в байтах</param>
      public virtual void ExtractVarFrom( string typeVar, object varval, byte[] memb, int lenR, int lenB )
      {
      }

      public virtual void PlaceVarTo( ArrayList MTD, VarQuality vQ )
      {
      }
   }
   #endregion
   #region class TFloatVariable
   /// <summary>
   /// class TFloatVariable : TCRZAVariable
   /// представление переменной типа Float - 32 бита
   /// </summary>
   public class TFloatVariable : TCRZAVariable
   {
      // объявляем событие
      override public event ChVar OnChangeVar;

      private TCRZAGroup Group;
      public int length_R = 2; //длина в 16 разрядных словах (регистрах MODBUS)
      public int length_B = 4; //длина в байтах
      // величина переменной
      public float Value;
      private float newValue;
      // признак первого чтения
      //bool firstRead;

      /// <summary>
      /// конструктор
      /// </summary>
      /// <param name="aGroup">- группа, к которой принадлежит переменная</param>
      /// <param name="aRegInDev">- адрес в устройстве, где лежит данная переменная</param>
      /// <param name="aName">- имя переменной</param>
      /// <param name="aCaption">- подпись для переменной</param>
      /// <param name="aDim">- размерность переменной</param>
      public TFloatVariable( TCRZAGroup aGroup, int aRegInDev, string aName, string aCaption, string aDim )
      {
         Group = aGroup;
         RegInDev = aRegInDev;
         Caption = aCaption;
         Dim = aDim;
         Name = aName;
         firstRead = false;
         Quality = VarQuality.vqUndefined;
         Value = 0;
      }

      /// <summary>
      /// public override string ToString( )
      /// </summary>
      /// <returns></returns>
      public override string ToString()
      {
         return "TFloatVariable";
      }

      /// <summary>
      /// private void fChVari() 
      /// генерация события по изменению тега на верхнем уровне
      /// </summary>
      private void fChVari()
      {
         if( OnChangeVar != null )
            OnChangeVar( );
      }

      /// <summary>
      /// public override void ExtractVarFrom(ArrayList MTD)
      /// извлечь переменную из источника данных, преобразовать и поместить в поле Value
      /// </summary>
      /// <param name="MTD"></param>
      public override void ExtractVarFrom( ArrayList MTD )
      {
         if( ( Quality == VarQuality.vqUndefined ) || ( ( this.Group.Device.crzaDeviceState != CRZADeviceState.CRZAdstOnline ) && Quality != VarQuality.vqArhiv ) )
         {
            Value = 0;
            // вызываем событие обновления link'ов
            fChVari( );
            return;
         }

         // пока просто извлечем raw-значение и сравним его со старым
         foreach( BMRZ aBMRZ in MTD )
         {
            if( this.Group.Device.NumDev == aBMRZ.NDev )
            {
               if( aBMRZ.varDev[ RegInDev ] is Real_FieldMT )
               {
                  Real_FieldMT temp = ( Real_FieldMT ) aBMRZ.varDev[ RegInDev ];
                  if( ( Value != temp.varMT_Value ) | !firstRead )
                  {
                     firstRead = true;

                     Value = ( float ) temp.varMT_Value;
                     // вызываем событие обновления link'ов
                     fChVari( );
                  }
               }
               break;
            }
         }
      }

      /// <summary>
      /// перегруженная функция извлечения значения с нижнего уровня по событию
      /// </summary>
      /// <param name="varval">значение переменной</param>
      /// <param name="memb">массив байт представления переменной</param>
      /// <param name="lenR">длина в рег. MODBUS</param>
      /// <param name="lenB">длина в байтах</param>
      public override void ExtractVarFrom( string typeVar, object varval, byte[] memb, int lenR, int lenB )
      {
         if( ( Quality == VarQuality.vqUndefined ) || ( ( this.Group.Device.crzaDeviceState != CRZADeviceState.CRZAdstOnline ) && Quality != VarQuality.vqArhiv ) )
            newValue = 0;
         else
            newValue = Convert.ToSingle( varval );

         if( ( Value != newValue ) || !firstRead )
         {
            firstRead = true;
            Value = newValue;

            // вызываем событие обновления link'ов
            fChVari( );
         }
      }

      /// <summary>
      /// public virtual void PlaceVarTo( ArrayList MTD, VarQuality vQ )
      /// поместить переменную в приемник данных
      /// </summary>
      /// <param name="MTD"></param>
      /// <param name="val"></param>
      public override void PlaceVarTo( ArrayList MTD, VarQuality vQ )
      {
         if( vQ == VarQuality.vqUndefined )
            Value = 0;

         // вызываем событие обновления link'ов
         fChVari( );
         // обнуляем данные и на нижнем уровне
         foreach( BMRZ aBMRZ in MTD )
         {
            if( this.Group.Device.NumDev == aBMRZ.NDev )
               if( aBMRZ.varDev[ RegInDev ] is Real_FieldMT )
               {
                  Real_FieldMT temp = ( Real_FieldMT ) aBMRZ.varDev[ RegInDev ];
                  temp.varMT_Value = Value;
               }
            break;
         }
      }
   }
	  #endregion
   #region class TIntVariable
   /// <summary>
   /// class TIntVariable : TCRZAVariable
   /// представление переменной типа Int - 16-разрядного двоичного числа со знаком
   /// </summary>
   public class TIntVariable : TCRZAVariable
   {
      // объявляем событие
      override public  event ChVar OnChangeVar;

      private TCRZAGroup Group;
      public int length_R = 1; //длина в 16 разрядных словах (регистрах MODBUS)
      public int length_B = 2; //длина в байтах
      // величина переменной
      public short Value;
      private short newValue;
      // признак первого чтения
      //bool firstRead;

      /// <summary>
      /// конструктор
      /// </summary>
      /// <param name="aGroup">- группа, к которой принадлежит переменная </param>
      /// <param name="aRegInDev">- адрес в устройстве, где лежит данная переменная</param>
      /// <param name="aName">- имя переменной</param>
      /// <param name="aCaption">- подпись для переменной</param>
      /// <param name="aDim">- размерность переменной</param>
      public TIntVariable(TCRZAGroup aGroup, int aRegInDev, string aName, string aCaption, string aDim)
      {
         Group = aGroup;
         RegInDev = aRegInDev;
         Caption = aCaption;
         Dim = aDim;
         Name = aName;
         firstRead = false;
         Quality = VarQuality.vqUndefined;
         Value = 0;
      }
      /// <summary>
      /// public override string ToString( )
      /// </summary>
      /// <returns></returns>
      public override string ToString( )
      {
         return "TIntVariable";
      }
      /// <summary>
      /// private void fChVari() 
      /// генерация события по изменению тега
      /// </summary>
      private void fChVari()
      {
         if (OnChangeVar != null)
         OnChangeVar();
      }

      /// <summary>
      /// public override void ExtractVarFrom(ArrayList MTD)
      /// извлечь переменную из источника данных, преобразовать и поместить в поле Value
      /// </summary>
      /// <param name="MTD"></param>
      public override void ExtractVarFrom(ArrayList MTD)
      {
         if( ( Quality == VarQuality.vqUndefined ) || ( ( this.Group.Device.crzaDeviceState != CRZADeviceState.CRZAdstOnline ) && Quality != VarQuality.vqArhiv ) )
         {
            Value = 0;
            // вызываем событие обновления link'ов
            fChVari();
            return;
         }

         // пока просто извлечем raw-значение и сравним его со старым
         foreach(BMRZ aBMRZ in MTD)
         {
            if (this.Group.Device.NumDev == aBMRZ.NDev)
               if (aBMRZ.varDev[RegInDev] is Int_FieldMT)
               {
                  Int_FieldMT temp = (Int_FieldMT)aBMRZ.varDev[RegInDev];
                  
                  if ((Value != temp.varMT_Value) | !firstRead)
                  {
                     firstRead = true;
                     Value = (short)temp.varMT_Value;
                     // вызываем событие обновления link'ов
                     fChVari();
                  }
               }
               break;
         }
      }

      /// <summary>
      /// перегруженная функция извлечения значения с нижнего уровня по событию
      /// </summary>
      /// <param name="varval">значение переменной</param>
      /// <param name="memb">массив байт представления переменной</param>
      /// <param name="lenR">длина в рег. MODBUS</param>
      /// <param name="lenB">длина в байтах</param>
      public override void ExtractVarFrom( string typeVar, object varval, byte[] memb, int lenR, int lenB )
      {
         if( ( Quality == VarQuality.vqUndefined ) || ( ( this.Group.Device.crzaDeviceState != CRZADeviceState.CRZAdstOnline ) && Quality != VarQuality.vqArhiv ) )
            newValue = 0;
         else
            newValue = Convert.ToInt16( varval );

         if( ( Value != newValue ) || !firstRead )
         {
            firstRead = true;

            Value = newValue;
            // вызываем событие обновления link'ов
            fChVari( );
         }
      }

      /// <summary>
		/// public virtual void PlaceVarTo( ArrayList MTD, VarQuality vQ )
      /// поместить переменную в приемник данных
      /// </summary>
      /// <param name="MTD"></param>
      /// <param name="val"></param>
		public override void PlaceVarTo( ArrayList MTD, VarQuality vQ )
      {
         if( vQ == VarQuality.vqUndefined )
            Value = 0;

         // вызываем событие обновления link'ов
         fChVari();
         
         // обнуляем данные и на нижнем уровне
         foreach( BMRZ aBMRZ in MTD )
            if( this.Group.Device.NumDev == aBMRZ.NDev )
            {
               if( aBMRZ.varDev[RegInDev] is Int_FieldMT )
               {
                  Int_FieldMT temp = ( Int_FieldMT ) aBMRZ.varDev[RegInDev];
                  temp.varMT_Value = Value;
               }
               break;
            }
      }
	}
	#endregion

	#region class TBitFieldVariable
	/// <summary>
   /// class TBitFieldVariable : TCRZAVariable
   /// представление битовой переменной из поля бит (длина поля, из которого извлекается бит не фиксирована)
   /// </summary>
   public class TBitFieldVariable : TCRZAVariable
   {
      // объявляем делегата
      override public event ChVar OnChangeVar;

      private TCRZAGroup Group;
      public int length_R = 0; //длина в 16 разрядных словах (регистрах MODBUS)
      public int length_B = 0; //длина в байтах
      public string bitMask = null; //
      // величина переменной
      public bool Value;
      private bool oldValue;
      // признак инверсного сигнала
      private bool isReverse = false;
      // признак первого чтения
      //bool firstRead;

      /// <summary>
      /// конструктор
      /// </summary>
      /// <param name="aGroup">- группа, к которой принадлежит переменная</param>
      /// <param name="aRegInDev">- адрес в устройстве, где лежит данная переменная</param>
      /// <param name="abitMask">- битовая маска, соответсвующая положению бит, определяющих значение битовой переменной (тега)-
      ///                             задается строкой, представляющей собой последовательность шестнадцатеричных цифр(без 0x)</param>
      /// <param name="aName">- имя переменной</param>
      /// <param name="aCaption">- подпись для переменной</param>
      public TBitFieldVariable(TCRZAGroup aGroup, int aRegInDev, string abitMask, string aName, string aCaption, string reverse)
      {
         Group = aGroup;
         RegInDev = aRegInDev;
         Caption = aCaption;
         Name = aName;
         bitMask = abitMask;
         Value = false;
         firstRead = false;
         Quality = VarQuality.vqUndefined;
         Value = false;
         
         if( reverse == "reverse" )
            isReverse = true;
         else
            isReverse = false;
      }

      /// <summary>
      /// public override string ToString( )
      /// </summary>
      /// <returns></returns>
      public override string ToString( )
      {
         return "TBitFieldVariable";
      }

      /// <summary>
      /// private void fChVari() 
      /// генерация события по изменению тега
      /// </summary>
      private void fChVari()
      {
         if (OnChangeVar != null)
            OnChangeVar();
      }

      /// <summary>
      /// public override void ExtractVarFrom(ArrayList MTD)
      /// извлечь переменную из источника данных, преобразовать и поместить в поле Value
      /// </summary>
      /// <param name="MTD"></param>
      public override void ExtractVarFrom(ArrayList MTD)
      {
         // сохраним для сравнения
         if( isReverse )
            oldValue = !Value;
         else
            oldValue = Value;    

         if( ( Quality == VarQuality.vqUndefined ) || ( ( this.Group.Device.crzaDeviceState != CRZADeviceState.CRZAdstOnline ) && Quality != VarQuality.vqArhiv ) )
         {
            Value = false;

            if( oldValue != Value || !firstRead )
            {
               firstRead = true;
               
               // вызываем событие обновления link'ов
               fChVari( );
               return;
            }
         }

         char[] sTmp = new char[2];
         // извлечем состояние бит в соответствии с маской и сравним его со старым
         
         foreach(BMRZ aBMRZ in MTD)
         {
            if (this.Group.Device.NumDev == aBMRZ.NDev)
            {
               if (aBMRZ.varDev[RegInDev] is Bit_FieldMT)
               {
                     Bit_FieldMT temp = (Bit_FieldMT)aBMRZ.varDev[RegInDev];
                     // сохраняем длины переменных
                     length_B = temp.varMT_Len;
                     length_R = temp.varMT_LenR;

                     byte[] mTmp = new byte[length_B];

                     // получаем битовое представление маски в виде массива байт
                     for (int i = 0; i < temp.varMT_Len; i++)
                     {
                        bitMask.CopyTo(i*2, sTmp, 0, 2);
                        string ssTmp = new string(sTmp);
                        mTmp[i] = byte.Parse(ssTmp, System.Globalization.NumberStyles.HexNumber);
                     }
                     // сравниваем массивы бит
                     System.Collections.BitArray mBits = new BitArray(mTmp);
                     System.Collections.BitArray vBits = new BitArray(temp.varMT_Value);
                     vBits.And(mBits);

                     for (int i = 0; i < vBits.Count; i++)
                     {
                        if (vBits.Get(i) == true)
                        {
                           Value = true ;
                           break;
                        }
                        else
                           Value = false;
                     }

                     if (oldValue != Value | !firstRead)
                     {
                        firstRead = true;
                        if( isReverse )
                           Value = !Value;

                        fChVari();
                        break;
                     }

                     if( isReverse )
                        Value = !Value;
                  }
                  else if (aBMRZ.varDev[RegInDev] is Byte_FieldMT)
                  {
                     Byte_FieldMT tempByte = (Byte_FieldMT)aBMRZ.varDev[RegInDev];
                     Bit_FieldMT ttmp = new Bit_FieldMT( tempByte.varMT_LenR, aBMRZ );
                     ttmp.varMT_Value = tempByte.varMT_Value;
                        
                     // сохраняем длины переменных
                     length_B = ttmp.varMT_Len;
                     length_R = ttmp.varMT_LenR;

                     byte[] mTmp = new byte[length_B];

                     // получаем битовое представление маски в виде массива байт
                     for (int i = 0; i < ttmp.varMT_Len; i++)
                     {
                        bitMask.CopyTo(i * 2, sTmp, 0, 2);
                        string ssTmp = new string(sTmp);
                        mTmp[i] = byte.Parse(ssTmp, System.Globalization.NumberStyles.HexNumber);
                     }
                     
                     // сравниваем массивы бит
                     System.Collections.BitArray mBits = new BitArray(mTmp);
                     System.Collections.BitArray vBits = new BitArray(ttmp.varMT_Value);
                     vBits.And(mBits);

                     for (int i = 0; i < vBits.Count; i++)
                     {
                        if (vBits.Get(i) == true)
                        {
                           Value = true;
                           break;
                        }
                        else
                           Value = false;
                     }

                     if (oldValue != Value | !firstRead)
                     {
                        firstRead = true;
                        if (isReverse)
                           Value = !Value;

                        fChVari();
                        break;
                     }

                     if (isReverse)
                        Value = !Value;
               }
               break;
            }
         }
      }

      /// <summary>
      /// перегруженная функция извлечения значения с нижнего уровня по событию
      /// </summary>
      /// <param name="varval">значение переменной</param>
      /// <param name="memb">массив байт представления переменной</param>
      /// <param name="lenR">длина в рег. MODBUS</param>
      /// <param name="lenB">длина в байтах</param>
      public override void ExtractVarFrom( string typeVar, object varval, byte[] memb, int lenR, int lenB )
      {
         // сохраним для сравнения
         if( isReverse )
            oldValue = !Value;
         else
            oldValue = Value;   
         
         if( ( Quality == VarQuality.vqUndefined ) || ( ( this.Group.Device.crzaDeviceState != CRZADeviceState.CRZAdstOnline ) && Quality != VarQuality.vqArhiv ) )
         {
            Value = false;

            if( oldValue || !firstRead )
            {
               firstRead = true;
               oldValue = false;
               // вызываем событие обновления link'ов
               fChVari( );
            }
               return;
         }

         char[] sTmp = new char[ 2 ];

         if( typeVar == "Bit_FieldMT" )
         {
            // копируем в локальную переменную valVar
            byte[] valVar = new byte[memb.Length];
            Buffer.BlockCopy(memb, 0, valVar, 0, memb.Length);

            // сохраняем длины переменных
            length_B = lenB;
            length_R = lenR;

            byte[] mTmp = new byte[ length_B ];

            // получаем битовое представление маски в виде массива байт
            for( int i = 0 ;i < lenB ;i++ )
            {
               bitMask.CopyTo( i * 2, sTmp, 0, 2 );
               string ssTmp = new string( sTmp );
               mTmp[ i ] = byte.Parse( ssTmp, System.Globalization.NumberStyles.HexNumber );
            }
            
            // сравниваем массивы бит
            System.Collections.BitArray mBits = new BitArray( mTmp );
            System.Collections.BitArray vBits = new BitArray( valVar );
            vBits.And( mBits );

            for( int i = 0 ;i < vBits.Count ;i++ )
            {
               if( vBits.Get( i ) == true )
               {
                  Value = true;
                  break;
               }
               else
                  Value = false;
            }

            if( oldValue != Value | !firstRead )
            {
               firstRead = true;

               if( isReverse )
                  Value = !Value;

               fChVari( );
            }
            else if( isReverse )
               Value = !Value;
         }
         else if( typeVar == "Byte_FieldMT" )
         {
            // копируем в локальную переменную valVar
            byte[] valVar = new byte[ memb.Length ];
            Buffer.BlockCopy( memb, 0, valVar, 0, memb.Length );

            // сохраняем длины переменных
            length_B = lenB;
            length_R = lenR;

            byte[] mTmp = new byte[ length_B ];

            // получаем битовое представление маски в виде массива байт
            for( int i = 0 ;i < valVar.Length ;i++ )
            {
               bitMask.CopyTo( i * 2, sTmp, 0, 2 );
               string ssTmp = new string( sTmp );
               mTmp[ i ] = byte.Parse( ssTmp, System.Globalization.NumberStyles.HexNumber );
            }
            
            // сравниваем массивы бит
            System.Collections.BitArray mBits = new BitArray( mTmp );
            System.Collections.BitArray vBits = new BitArray( valVar );
            vBits.And( mBits );

            for( int i = 0 ;i < vBits.Count ;i++ )
            {
               if( vBits.Get( i ) == true )
               {
                  Value = true;
                  break;
               }
               else
                  Value = false;
            }

            if( oldValue != Value | !firstRead )
            {
               firstRead = true;
               if( isReverse )
                  Value = !Value;

               fChVari( );
            }
            else if( isReverse )
               Value = !Value;
         }
      }
      /// <summary>
      /// public virtual void PlaceVarTo( ArrayList MTD, VarQuality vQ )
      /// поместить переменную в приемник данных
      /// </summary>
      /// <param name="MTD"></param>
      /// <param name="val"></param>
		public override void PlaceVarTo( ArrayList MTD, VarQuality vQ )
      {
         byte value = 0;

         if( vQ == VarQuality.vqUndefined )
            value = 0;

         // вызываем событие обновления link'ов
         fChVari();
         
         // обнуляем данные и на нижнем уровне
         foreach( BMRZ aBMRZ in MTD )
         {
            if( this.Group.Device.NumDev == aBMRZ.NDev )
            {
               if( aBMRZ.varDev[RegInDev] is Bit_FieldMT )
               {
                  Bit_FieldMT temp = ( Bit_FieldMT ) aBMRZ.varDev[RegInDev];
                          
                  for( int i = 0; i < temp.varMT_Value.Length; i++ )
                     temp.varMT_Value[i] = value;
               }
               break;
            }
         }
      }
	}
	#endregion
	
   #region class TUIntVariable
	/// <summary>
   /// class TUIntVariable : TCRZAVariable
   /// представление переменной типа UInt - 16-разрядного двоичного числа без знака
   /// </summary>
   public class TUIntVariable : TCRZAVariable
   {
      // объявляем событие
      override public event ChVar OnChangeVar;

       private TCRZAGroup Group;
       public int length_R = 1; //длина в 16 разрядных словах (регистрах MODBUS)
       public int length_B = 2; //длина в байтах
       // величина переменной
       public ushort Value;
      private ushort newValue;
       
      /// <summary>
      /// public TUIntVariable(TCRZAGroup aGroup, int aRegInDev, string aName, string aCaption, string aDim)
      /// конструктор
      /// </summary>
      /// <param name="aGroup">- группа, к которой принадлежит переменная</param>
      /// <param name="aRegInDev">- адрес в устройстве, где лежит данная переменная</param>
      /// <param name="aName">- имя переменной</param>
      /// <param name="aCaption">- подпись для переменной</param>
      /// <param name="aDim">- размерность переменной</param>
      public TUIntVariable(TCRZAGroup aGroup, int aRegInDev, string aName, string aCaption, string aDim)
      {
         Group = aGroup;
         RegInDev = aRegInDev;
         Caption = aCaption;
         Dim = aDim;
         Name = aName;
         firstRead = false;
			Quality = VarQuality.vqUndefined;
			Value = 0;
      }
		/// <summary>
		/// public override string ToString( )
		/// </summary>
		/// <returns></returns>
		public override string ToString( )
		{
			return "TUIntVariable";
		}
      /// <summary>
      /// private void fChVari()
      /// генерация события по изменению тега
      /// </summary>
       private void fChVari()
      {
         if (OnChangeVar != null)
            OnChangeVar();
      }
      
      /// <summary>
      /// public override void ExtractVarFrom(ArrayList MTD)
      /// извлечь переменную из источника данных, преобразовать и поместить в поле Value
      /// </summary>
      /// <param name="MTD"></param>
       public override void ExtractVarFrom( ArrayList MTD )
      {
         if( ( Quality == VarQuality.vqUndefined ) || ( ( this.Group.Device.crzaDeviceState != CRZADeviceState.CRZAdstOnline ) && Quality != VarQuality.vqArhiv ) )
         {
            Value = 0;
            // вызываем событие обновления link'ов
            fChVari();
            return;
         }

          // пока просто извлечем raw-значение и сравним его со старым
          foreach( BMRZ aBMRZ in MTD )
          {
              if( this.Group.Device.NumDev == aBMRZ.NDev )
              {
                  if( aBMRZ.varDev[RegInDev] is UInt_FieldMT )
                  {
                      UInt_FieldMT temp = ( UInt_FieldMT ) aBMRZ.varDev[RegInDev];
                      if( ( Value != temp.varMT_Value ) | !firstRead )
                      {
                          firstRead = true;

                          Value = ( ushort ) temp.varMT_Value;
                          // вызываем событие обновления link'ов
                          fChVari();
                      }
                  }
                  break;
              }
          }
      }

      /// <summary>
      /// перегруженная функция извлечения значения с нижнего уровня по событию
      /// </summary>
      /// <param name="varval">значение переменной</param>
      /// <param name="memb">массив байт представления переменной</param>
      /// <param name="lenR">длина в рег. MODBUS</param>
      /// <param name="lenB">длина в байтах</param>
      public override void ExtractVarFrom( string typeVar, object varval, byte[] memb, int lenR, int lenB )
      {
         if( ( Quality == VarQuality.vqUndefined ) || ( ( this.Group.Device.crzaDeviceState != CRZADeviceState.CRZAdstOnline ) && Quality != VarQuality.vqArhiv ) )
            newValue = 0;
         else
            newValue = Convert.ToUInt16( varval );

          if( ( Value != newValue ) || !firstRead )
          {
             firstRead = true;

             Value = newValue;

             // вызываем событие обновления link'ов
             fChVari( );
          }
      }

      /// <summary>
      /// public virtual void PlaceVarTo( ArrayList MTD, VarQuality vQ )
      /// поместить переменную в приемник данных
      /// </summary>
      /// <param name="MTD"></param>
      /// <param name="val"></param>
		public override void PlaceVarTo( ArrayList MTD, VarQuality vQ )
      {
         if( vQ == VarQuality.vqUndefined )
            Value = 0;
         
         // вызываем событие обновления link'ов
         fChVari();
         // обнуляем данные и на нижнем уровне
         foreach( BMRZ aBMRZ in MTD )
            if( this.Group.Device.NumDev == aBMRZ.NDev )
            {
               if( aBMRZ.varDev[RegInDev] is UInt_FieldMT )
               {
                  UInt_FieldMT temp = ( UInt_FieldMT ) aBMRZ.varDev[RegInDev];
                  temp.varMT_Value = Value;
               }
               break;
            }
      }
	}
	#endregion

	#region class TDIntVariable
	/// <summary>
   /// class TDIntVariable : TCRZAVariable
   /// представление переменной типа DUInt - 32-разрядного двоичного числа со знаком
   /// </summary>
   class TDIntVariable : TCRZAVariable
   {
      /// <summary>
      /// public override string ToString( )
      /// </summary>
      /// <returns></returns>
      public override string ToString( )
      {
         return "TDIntVariable";
      }
   }
   #endregion

   #region class TUDIntVariable
   /// <summary>
   /// class TUDIntVariable : TCRZAVariable
   /// представление переменной типа UDInt - 32-разрядного двоичного числа без знака
   /// </summary>
   public class TUDIntVariable : TCRZAVariable
   {
      // объявляем событие
      override public event ChVar OnChangeVar;

      private TCRZAGroup Group;
      public int length_R = 2; //длина в 16 разрядных словах (регистрах MODBUS)
      public int length_B = 4; //длина в байтах
      // величина переменной
      public uint Value;
      private uint newValue;

      /// <summary>
      /// конструктор
      /// </summary>
      /// <param name="aGroup">- группа, к которой принадлежит переменная </param>
      /// <param name="aRegInDev">- адрес в устройстве, где лежит данная переменная</param>
      /// <param name="aName">- имя переменной</param>
      /// <param name="aCaption">- подпись для переменной</param>
      /// <param name="aDim">- размерность переменной</param>
      public TUDIntVariable(TCRZAGroup aGroup, int aRegInDev, string aName, string aCaption, string aDim)
      {
         Group = aGroup;
         RegInDev = aRegInDev;
         Caption = aCaption;
         Dim = aDim;
         Name = aName;
         firstRead = false;
         Quality = VarQuality.vqUndefined;
         Value = 0;
      }

      /// <summary>
      /// public override string ToString( )
      /// </summary>
      /// <returns></returns>
      public override string ToString( )
      {
         return "TUDIntVariable";
      }

      /// <summary>
      /// private void fChVari() 
      /// генерация события по изменению тега
      /// </summary>
      private void fChVari()
      {
         if (OnChangeVar != null)   
            OnChangeVar();
      }

      /// <summary>
      /// public override void ExtractVarFrom(ArrayList MTD)
      /// извлечь переменную из источника данных, преобразовать и поместить в поле Value
      /// </summary>
      /// <param name="MTD"></param>
      public override void ExtractVarFrom( ArrayList MTD )
      {
         if( ( Quality == VarQuality.vqUndefined ) || ( ( this.Group.Device.crzaDeviceState != CRZADeviceState.CRZAdstOnline ) && Quality != VarQuality.vqArhiv ) )
         {
            Value = 0;
            // вызываем событие обновления link'ов
            fChVari();
            return;
         }

         // пока просто извлечем raw-значение и сравним его со старым
         foreach( BMRZ aBMRZ in MTD )
         {
            if( this.Group.Device.NumDev == aBMRZ.NDev )
            {
               if( aBMRZ.varDev[RegInDev] is UDInt_FieldMT )
               {
                  UDInt_FieldMT temp = ( UDInt_FieldMT ) aBMRZ.varDev[RegInDev];
                  
                  if( ( Value != temp.varMT_Value ) | !firstRead )
                  {
                     firstRead = true;

                     Value = ( uint ) temp.varMT_Value;
                     // вызываем событие обновления link'ов
                     fChVari();
                  }
               }
               break;
            }
          }
      }
      
      /// <summary>
      /// перегруженная функция извлечения значения с нижнего уровня по событию
      /// </summary>
      /// <param name="varval">значение переменной</param>
      /// <param name="memb">массив байт представления переменной</param>
      /// <param name="lenR">длина в рег. MODBUS</param>
      /// <param name="lenB">длина в байтах</param>
      public override void ExtractVarFrom( string typeVar, object varval, byte[] memb, int lenR, int lenB )
      {
         if( ( Quality == VarQuality.vqUndefined ) || ( ( this.Group.Device.crzaDeviceState != CRZADeviceState.CRZAdstOnline ) && Quality != VarQuality.vqArhiv ) )
            newValue =  0;
         else
            newValue = Convert.ToUInt32( varval );

         if( ( Value != newValue ) || !firstRead )
         {
            firstRead = true;

            Value = newValue;
            // вызываем событие обновления link'ов
            fChVari( );
         }
      }

      /// <summary>
      /// public virtual void PlaceVarTo( ArrayList MTD, VarQuality vQ )
      /// поместить переменную в приемник данных
      /// </summary>
      /// <param name="MTD"></param>
      /// <param name="val"></param>
		public override void PlaceVarTo( ArrayList MTD, VarQuality vQ )
      {
         if( vQ == VarQuality.vqUndefined )
            Value = 0;
         // вызываем событие обновления link'ов
         fChVari();
         // обнуляем данные и на нижнем уровне
         foreach( BMRZ aBMRZ in MTD )
         {
            if( this.Group.Device.NumDev == aBMRZ.NDev )
            {
               if( aBMRZ.varDev[RegInDev] is UDInt_FieldMT )
               {
                  UDInt_FieldMT temp = ( UDInt_FieldMT ) aBMRZ.varDev[RegInDev];
                  temp.varMT_Value = Value;
               }
               break;
            }
         }
      }
	}
	#endregion

	#region class TByteVariable
	/// <summary>
   /// class TByteVariable : TCRZAVariable
   /// представление переменной типа Byte - 8-разрядного двоичного числа
   /// </summary>
   class TByteVariable : TCRZAVariable
   {
      /// <summary>
      /// public override string ToString( )
      /// </summary>
      /// <returns></returns>
      public override string ToString( )
      {
         return "TByteVariable";
      }
   }
   #endregion

   #region class TByteField2StringVariable
   /// <summary>
   /// class TByteField2StringVariable : TCRZAVariable
   /// представление поля байт в виде символьного массива - строка шестнадцатеричных значений
   /// </summary>
	class TByteField2StringVariable : TCRZAVariable
   {
      // объявляем событие
      override public event ChVar OnChangeVar;

      private TCRZAGroup Group;
      public int length_R = 0; //длина в 16 разрядных словах (регистрах MODBUS)
      public int length_B = 0; //длина в байтах

      //// величина переменной
      //public string Value;
      //private byte[] memX;	// для хранения raw-значения переменной
      
      /// <summary>
      /// public override string ToString( )
      /// </summary>
      /// <returns></returns>
      public override string ToString( )
      {
         return "TByteField2StringVariable";
      }
   }
   #endregion
   
   #region class TByteField2UIntVariable
   /// <summary>
   /// class TByteField2UIntVariable : TCRZAVariable
   /// представление переменной типа массива байт, из которого можно извлекать значение отдельного поля бит как беззнакового целого
   /// </summary>
   public class TByteField2UIntVariable : TCRZAVariable
   {
      // объявляем событие
      override public event ChVar OnChangeVar;

      private TCRZAGroup Group;
      public int length_R = 0; //длина в 16 разрядных словах (регистрах MODBUS)
      public int length_B = 0; //длина в байтах
      public string bitMask = "";	// строковое значение маски для привязки
		byte[] aHexBitMask;
		BitArray hexBitMask;		// hex-битовая маска, по которой формируется итоговое значение тега
      // величина переменной
      public UInt32 Value;
      private UInt32 newValue;
      private byte[] memX;	// для хранения raw-значения переменной
      private BitArray memXBA;	// для преобразования 
      private BitVector32 Value_bv32;
      
      /// <summary>
      /// public TByteField2UIntVariable( TCRZAGroup aGroup, int aRegInDev, string aName, string aCaption, string aDim, int aLengthInByte, aBitMask )
      /// конструктор - длина битового поля не более 4 байт
      /// </summary>
      /// <param name="aGroup">- группа, к которой принадлежит переменная</param>
      /// <param name="aRegInDev">- адрес в устройстве, где лежит данная переменная</param>
      /// <param name="abitMask">- битовая маска в виде массива байтовых значений, показывает биты, которые формируют итоговое значение тега 
      ///									размер маски не более 32 бит</param>
      /// <param name="aName">- имя переменной</param>
      /// <param name="aCaption">- подпись для переменной</param>
      /// <param name="aDim">- размерность переменной</param>
      public TByteField2UIntVariable( TCRZAGroup aGroup, int aRegInDev, string abitMask, string aName, string aCaption)
      {
         Group = aGroup;
         RegInDev = aRegInDev;
         Caption = aCaption;
         Name = aName;
         firstRead = false;
			Quality = VarQuality.vqUndefined;

			Value = 0;
			// получим массив байт aHexBitMask из строки abitMask
			abitMask = abitMask.Trim();
			bitMask = abitMask.Trim();
         // сначала проверим
         aHexBitMask = new byte[abitMask.Length / 2];
			length_B = aHexBitMask.Length;
			length_R = length_B / 2;

			if( ( ( abitMask.Length % 2 ) == 1 ) || ( aHexBitMask.Length > 4 ) )
				throw new Exception( "Ошибка при инициализации переменной типа TByteField2UIntVariable" );

			// преобразуем
			StringBuilder sb = new StringBuilder();
         
         for( int j = 0 ; j < aHexBitMask.Length ; j++ )
            for( int i = abitMask.Length -1; i > 0 ; i-=2 )
            {
               sb.Length = 0;
               sb.Append( abitMask[i-1] );
               sb.Insert( 0, abitMask[i] );
               aHexBitMask[j] = byte.Parse(sb.ToString(), NumberStyles.HexNumber);
               j++;
            }

            // инициализируем массив байт с маской
            hexBitMask = new BitArray( aHexBitMask );
            memX = new byte[length_B];

			   Value_bv32 = new BitVector32( 0 );
      }

      /// <summary>
      /// public override string ToString( )
      /// </summary>
      /// <returns></returns>
      public override string ToString( )
      {
         return "TByteField2UIntVariable";
      }

      /// <summary>
      /// private void fChVari()
      /// генерация события по изменению тега
      /// </summary>
      private void fChVari( )
      {
         if( OnChangeVar != null )
            OnChangeVar();
      }

      /// <summary>
      /// public override void ExtractVarFrom(ArrayList MTD)
      /// извлечь переменную из источника данных, преобразовать и поместить в поле Value
      /// </summary>
      /// <param name="MTD"></param>
      public override void ExtractVarFrom( ArrayList MTD )
      {
         if( ( Quality == VarQuality.vqUndefined ) || ( ( this.Group.Device.crzaDeviceState != CRZADeviceState.CRZAdstOnline ) && Quality != VarQuality.vqArhiv ) )
         {
            Value = 0;
            // вызываем событие обновления link'ов
            fChVari();
            return;
         }

         // пока просто извлечем raw-значение и сравним его со старым
         Byte_FieldMT temp;
         Value_bv32 = new BitVector32( 0 );
         
         foreach( BMRZ aBMRZ in MTD )
         {
            if( this.Group.Device.NumDev == aBMRZ.NDev )
            {
               if( aBMRZ.varDev[RegInDev] is Byte_FieldMT )
               {
                  temp = ( Byte_FieldMT ) aBMRZ.varDev[RegInDev];

                  if( !firstRead )
                  {
                     for( int i = 0 ; i < length_B ; i++ )
                     {
                        if( memX[i] == temp.varMT_Value[i] )
                           continue;	// если равен
                        else
                           goto m1;
                     }
                     return;
                  }
m1:							// если не равен
                  firstRead = true;

                  // извлекаем новое значение и формируем uint
                  for( int i = 0 ; i < length_B ; i++ )
                     memX[i] = temp.varMT_Value[i];

                  memXBA = new BitArray( temp.varMT_Value );	// значение байтового массива во временную переменную
                  int j = 1;
                  for( int i = 0 ; i < memXBA.Count ; i++ )
                  {
                     if( hexBitMask[i] )	// обнаруживаем первый единичный бит маски
                     {
                        while (hexBitMask[i] )	// проходим маску и соответсвующие биты исходного массива, формируем результат
                        {
                           if (hexBitMask[i] && memXBA[i])
                              Value_bv32[j] = true;
                           else
                              Value_bv32[j] = false;

                           i++;
                           j*=2;
                           
                           if (i == memXBA.Count - 1)
                              break;
                        }
                        
                        Value = ( UInt32 ) Value_bv32.Data;
                        // вызываем событие обновления link'ов
                        fChVari();
                        break;
                     }
                  }
               }
               break;
            }
         }
      }

      /// <summary>
      /// перегруженная функция извлечения значения с нижнего уровня по событию
      /// </summary>
      /// <param name="varval">значение переменной</param>
      /// <param name="memb">массив байт представления переменной</param>
      /// <param name="lenR">длина в рег. MODBUS</param>
      /// <param name="lenB">длина в байтах</param>
      public override void ExtractVarFrom( string typeVar, object varval, byte[] memb, int lenR, int lenB )
      {
         if( ( Quality == VarQuality.vqUndefined ) || ( ( this.Group.Device.crzaDeviceState != CRZADeviceState.CRZAdstOnline ) && Quality != VarQuality.vqArhiv ) )
         {
            newValue = 0;

            if( ( Value != newValue ) || !firstRead )
            {
               Value = newValue;
               // вызываем событие обновления link'ов
               fChVari( );
            }
               return;
         } 

         Value_bv32 = new BitVector32( 0 );
         byte[] varVal = new byte[ lenB ];
         Buffer.BlockCopy( memb, 0, varVal, 0, lenB );
         
         // извлекаем новое значение и формируем uint
         for( int i = 0 ;i < length_B ;i++ )
            memX[ i ] = varVal[ i ];

         memXBA = new BitArray( varVal );	// значение байтового массива во временную переменную
         int j = 1;
         
         for( int i = 0 ;i < memXBA.Count ;i++ )
         {
            if( hexBitMask[ i ] )	// обнаруживаем первый единичный бит маски
            {
               while( hexBitMask[ i ] )	// проходим маску и соответсвующие биты исходного массива, формируем результат
               {
                  if( hexBitMask[ i ] && memXBA[ i ] )
                     Value_bv32[ j ] = true;
                  else
                     Value_bv32[ j ] = false;

                  i++;
                  j *= 2;
                  
                  if( i == memXBA.Count - 1 )
                     break;
               }
               
               newValue = ( UInt32 ) Value_bv32.Data;

               if( ( Value != newValue ) || !firstRead )
               {
                  Value = newValue;

                  // вызываем событие обновления link'ов
                  fChVari( );
               }
            }
         }
      }

      public override void PlaceVarTo( ArrayList MTD, VarQuality vQ )
      {
         if( vQ == VarQuality.vqUndefined )
            Value = 0;

         // вызываем событие обновления link'ов
         fChVari();
         // обнуляем данные и на нижнем уровне
         foreach( BMRZ aBMRZ in MTD )
         {
            if( this.Group.Device.NumDev == aBMRZ.NDev )
            {
               if( aBMRZ.varDev[RegInDev] is Byte_FieldMT )
               {
                  Byte_FieldMT temp = ( Byte_FieldMT ) aBMRZ.varDev[RegInDev];
                  for (int i = 0; i <  temp.varMT_Len; i++)
                  temp.varMT_Value[i] = 0;
               }
               break;
            }
         }
      }
   }
   #endregion

   #region class TDateTimeVariable
   /// <summary>
   /// class TDateTimeVariable : TCRZAVariable
   /// представление переменной типа DateTime - 6 или 8 байт или из UTC
   /// </summary>
   public class TDateTimeVariable : TCRZAVariable
   {
      // объявляем событие
      override public  event ChVar OnChangeVar;
   
      private TCRZAGroup Group;
      public int length_R = 4; //длина в 16 разрядных словах (регистрах MODBUS)
      public int length_B = 8; //длина в байтах
      // величина переменной
      public string Value;
      public DateTime Value_raw;
      private DateTime newValue;
      //// признак первого чтения
      //public bool firstRead;
       
      /// <summary>
      /// конструктор
      /// </summary>
      /// <param name="aGroup">- группа, к которой принадлежит переменная </param>
      /// <param name="aRegInDev">- адрес в устройстве, где лежит данная переменная</param>
      /// <param name="aName">- имя переменной</param>
      /// <param name="aCaption">- подпись для переменной</param>
      /// <param name="aDim">- размерность переменной</param>
      public TDateTimeVariable(TCRZAGroup aGroup, int aRegInDev, string aName, string aCaption, string aDim)
      {
         Group = aGroup;
         RegInDev = aRegInDev;
         Caption = aCaption;
         Dim = aDim;
         Name = aName;
         firstRead = false;
         Quality = VarQuality.vqUndefined;
         Value = DateTime.MinValue.ToString();
      }
      
      /// <summary>
      /// public override string ToString( )
      /// </summary>
      /// <returns></returns>
      public override string ToString( )
      {
         return "TDateTimeVariable";
      }
      
      /// <summary>
      /// private void fChVari() 
      /// генерация события по изменению тега
      /// </summary>
      private void fChVari()
      {
         if (OnChangeVar != null)
            OnChangeVar();
      }

      /// <summary>
      /// public override void ExtractVarFrom(ArrayList MTD)
      /// извлечь переменную из источника данных, преобразовать и поместить в поле Value
      /// </summary>
      /// <param name="MTD"></param>
      public override void ExtractVarFrom(ArrayList MTD)
      {
         if( ( Quality == VarQuality.vqUndefined ) || ( ( this.Group.Device.crzaDeviceState != CRZADeviceState.CRZAdstOnline ) && Quality != VarQuality.vqArhiv ) )
         {
            Value = DateTime.MinValue.ToString();
            // вызываем событие обновления link'ов
            fChVari();
            return;
         }

         // пока просто извлечем raw-значение и сравним его со старым
         foreach( BMRZ aBMRZ in MTD )
         {
            if( this.Group.Device.NumDev == aBMRZ.NDev )
            {
               if( aBMRZ.varDev[RegInDev] is DateTime4_FieldMT )
               {
                  DateTime4_FieldMT temp = ( DateTime4_FieldMT ) aBMRZ.varDev[RegInDev];
                  
                  if( ( Value_raw != temp.varMT_Value ) | !firstRead )// 
                  {
                     firstRead = true;

                     Value_raw = ( DateTime ) temp.varMT_Value;
                     Value = Value_raw.ToString() + ":" + Value_raw.Millisecond;
                     //вызываем событие обновления link'ов
                     fChVari();
                  }
               }
               else if( aBMRZ.varDev[RegInDev] is DateTime3_FieldMT )
               {
                  DateTime3_FieldMT temp = ( DateTime3_FieldMT ) aBMRZ.varDev[RegInDev];
                  
                  if( ( Value_raw != temp.varMT_Value ) | !firstRead )
                  {
                     firstRead = true;

                     Value_raw = ( DateTime ) temp.varMT_Value;
                     Value = Value_raw.ToString() + ":" + Value_raw.Millisecond;
                     //вызываем событие обновления link'ов
                     fChVari();
                  }
               }
               else if( aBMRZ.varDev[RegInDev] is DateTimeUTC_FieldMT )
               {
                  DateTimeUTC_FieldMT temp = ( DateTimeUTC_FieldMT ) aBMRZ.varDev[RegInDev];
                  if( ( Value_raw != temp.varMT_Value ) | !firstRead )
                  {
                     firstRead = true;

                     Value_raw = ( DateTime ) temp.varMT_Value;
                     Value = Value_raw.ToString() + ":" + Value_raw.Millisecond;
                     //вызываем событие обновления link'ов
                     fChVari();
                  }
               }
               break;
            }
         }
      }

      /// <summary>
      /// перегруженная функция извлечения значения с нижнего уровня по событию
      /// </summary>
      /// <param name="varval">значение переменной</param>
      /// <param name="memb">массив байт представления переменной</param>
      /// <param name="lenR">длина в рег. MODBUS</param>
      /// <param name="lenB">длина в байтах</param>
      public override void ExtractVarFrom( string typeVar, object varval, byte[] memb, int lenR, int lenB )
      {
         if( ( Quality == VarQuality.vqUndefined ) || ( ( this.Group.Device.crzaDeviceState != CRZADeviceState.CRZAdstOnline ) && Quality != VarQuality.vqArhiv ) )
         {
            newValue = DateTime.MinValue;

            if( Value_raw != newValue || !firstRead )
            {
               firstRead = true;

               Value = newValue.ToString( );
               Value_raw = newValue;
               // вызываем событие обновления link'ов
               fChVari( ); 
            }  
               return;
         }

         // пока просто извлечем raw-значение и сравним его со старым
         newValue = Convert.ToDateTime( varval );

         if( Value_raw != newValue || !firstRead )
         {
            firstRead = true;

            Value_raw = newValue;
            Value = Value_raw.ToString( ) + ":" + Value_raw.Millisecond.ToString();
            //вызываем событие обновления link'ов
            fChVari( );
          }          
       }

      /// <summary>
      /// public virtual void PlaceVarTo( ArrayList MTD, VarQuality vQ )
      /// поместить переменную в приемник данных
      /// </summary>
      /// <param name="MTD"></param>
      /// <param name="val"></param>
		public override void PlaceVarTo( ArrayList MTD, VarQuality vQ )
      {
         DateTime Value = DateTime.MinValue;
          
         if( vQ == VarQuality.vqUndefined )
            Value = DateTime.MinValue;
         
         // вызываем событие обновления link'ов
         fChVari();
         
         // обнуляем данные и на нижнем уровне
         foreach( BMRZ aBMRZ in MTD )
         {
            if( this.Group.Device.NumDev == aBMRZ.NDev )
            {
               if( aBMRZ.varDev[RegInDev] is DateTime4_FieldMT )
               {
                  DateTime4_FieldMT temp = ( DateTime4_FieldMT ) aBMRZ.varDev[RegInDev];                          
                  temp.varMT_Value = Value;
               }
               else if( aBMRZ.varDev[RegInDev] is DateTime3_FieldMT )
               {
                  DateTime3_FieldMT temp = ( DateTime3_FieldMT ) aBMRZ.varDev[RegInDev];
                  temp.varMT_Value = Value;
               }
               else if( aBMRZ.varDev[RegInDev] is DateTimeUTC_FieldMT )
               {
                  DateTimeUTC_FieldMT temp = ( DateTimeUTC_FieldMT ) aBMRZ.varDev[RegInDev];
                  temp.varMT_Value = Value;
               }
               break;
            }
         }
      }
	}
	#endregion

	#region class TStringVariable
	/// <summary>
   /// class TStringVariable : TCRZAVariable
   /// представление строки (с завершающим нулем) - длина не фиксирована
   /// </summary>
   public class TStringVariable : TCRZAVariable
   {
      // объявляем событие
      override public  event ChVar OnChangeVar;

      private TCRZAGroup Group;
      public int length_R = 1; //длина в 16 разрядных словах (регистрах MODBUS)
      public int length_B = 2; //длина в байтах
      // величина переменной
      public string Value;
      private string newValue;
 
      /// <summary>
      /// конструктор
      /// </summary>
      /// <param name="aGroup">- группа, к которой принадлежит переменная </param>
      /// <param name="aRegInDev">- адрес в устройстве, где лежит данная переменная</param>
      /// <param name="aName">- имя переменной</param>
      /// <param name="aCaption">- подпись для переменной</param>
      /// <param name="aDim">- размерность переменной</param>
      public TStringVariable( TCRZAGroup aGroup, int aRegInDev, string aName, string aCaption, string aDim )
      {
         Group = aGroup;
         RegInDev = aRegInDev;
         Caption = aCaption;
         Dim = aDim;
         Name = aName;
         firstRead = false;
         Quality = VarQuality.vqUndefined;
         Value = "";
      }

      /// <summary>
      /// public override string ToString( )
      /// </summary>
      /// <returns></returns>
      public override string ToString( )
      {
         return "TStringVariable";
      }
      
      /// <summary>
      /// private void fChVari() 
      /// генерация события по изменению тега
      /// </summary>
      private void fChVari()
      {
         if (OnChangeVar != null)
         OnChangeVar();
      }

      /// <summary>
      /// public override void ExtractVarFrom(ArrayList MTD)
      /// извлечь переменную из источника данных, преобразовать и поместить в поле Value
      /// </summary>
      /// <param name="MTD"></param>
      public override void ExtractVarFrom(ArrayList MTD)
      {
         if( ( Quality == VarQuality.vqUndefined ) || ( ( this.Group.Device.crzaDeviceState != CRZADeviceState.CRZAdstOnline ) && Quality != VarQuality.vqArhiv ) )
         {
            Value = "";
            // вызываем событие обновления link'ов
            fChVari();
            return;
         }

         // пока просто извлечем raw-значение и сравним его со старым
         foreach(BMRZ aBMRZ in MTD)
         {
            if (this.Group.Device.NumDev == aBMRZ.NDev)
            {
               if( aBMRZ.varDev[RegInDev] is Stringz_FieldMT )
               {
                  Stringz_FieldMT temp = ( Stringz_FieldMT ) aBMRZ.varDev[RegInDev];
                  if ((Value != temp.varMT_Value) | !firstRead)
                  {
                     firstRead = true;

                     Value = (string)temp.varMT_Value;
                     // вызываем событие обновления link'ов
                     fChVari();
                  }
               }
               break;
            }
         }
      }

      /// <summary>
      /// перегруженная функция извлечения значения с нижнего уровня по событию
      /// </summary>
      /// <param name="varval">значение переменной</param>
      /// <param name="memb">массив байт представления переменной</param>
      /// <param name="lenR">длина в рег. MODBUS</param>
      /// <param name="lenB">длина в байтах</param>
      public override void ExtractVarFrom( string typeVar, object varval, byte[] memb, int lenR, int lenB )
      {
         if( ( Quality == VarQuality.vqUndefined ) || ( ( this.Group.Device.crzaDeviceState != CRZADeviceState.CRZAdstOnline ) && Quality != VarQuality.vqArhiv ) )
            newValue = "";
         else
            newValue = Convert.ToString( varval );

         if( ( Value != newValue ) || !firstRead )
         {
            firstRead = true;

            Value = newValue;
            // вызываем событие обновления link'ов
            fChVari( );
         }
      }

      /// <summary>
      /// public virtual void PlaceVarTo( ArrayList MTD, VarQuality vQ )
      /// поместить переменную в приемник данных
      /// </summary>
      /// <param name="MTD"></param>
      /// <param name="val"></param>
		public override void PlaceVarTo( ArrayList MTD, VarQuality vQ )
      {
         if( vQ == VarQuality.vqUndefined )
            Value = "";
         
         // вызываем событие обновления link'ов
         fChVari();
         // обнуляем данные и на нижнем уровне
         foreach( BMRZ aBMRZ in MTD )
         {
            if( this.Group.Device.NumDev == aBMRZ.NDev )
            {
               if( aBMRZ.varDev[RegInDev] is Stringz_FieldMT )
               {
                  Stringz_FieldMT temp = ( Stringz_FieldMT ) aBMRZ.varDev[RegInDev];                          
                  temp.varMT_Value = Value;
               }
               break;
            }
         }
      }
	}
	#endregion

	#region class TBCDPackVariable
	/// <summary>
    /// class TBCDPackVariable : TCRZAVariable
    /// представление переменной в упакованном BCD-формате (массивом байт)- длина не фиксирована
    /// (для уставок - без преобразования в целочисленное значение)
    /// </summary>
   public class TBCDPackVariable : TCRZAVariable
   {
      // объявляем событие
      override public  event ChVar OnChangeVar;

      private TCRZAGroup Group;
      public int length_R = 1; //длина в 16 разрядных словах (регистрах MODBUS)
      public int length_B = 2; //длина в байтах
      string MASK = String.Empty;
      // величина переменной
      public string Value;
      private string newValue;
      private byte[] abValue;
      int  inset;
      int type;
      StringBuilder sb = new StringBuilder();
      
      /// <summary>
      /// конструктор
      /// </summary>
      /// <param name="aGroup">- группа, к которой принадлежит переменная </param>
      /// <param name="aRegInDev">- адрес в устройстве, где лежит данная переменная</param>
      /// <param name="aName">- имя переменной</param>
      /// <param name="aCaption">- подпись для переменной</param>
      /// <param name="aDim">- размерность переменной</param>
      /// <param name="aInset">- номер вкладки на панели уставок</param>
      /// <param name="aType">- тип (положение точки) - см. протокол описания MTRele</param>
      public TBCDPackVariable( TCRZAGroup aGroup, int aRegInDev, string aName, string aCaption, string aDim, int aInset, int aType )
      {
         Group = aGroup;
         RegInDev = aRegInDev;
         Caption = aCaption;
         Dim = aDim;
         Name = aName;
         firstRead = false;
         inset = aInset;
         type = aType;
         Quality = VarQuality.vqUndefined;
         
         for( int i = 0 ; i < length_B ; i++ )
            sb.Append( 0 );

         Value = sb.ToString();
      }

      /// <summary>
      /// конструктор 2
      /// </summary>
      /// <param name="aGroup">- группа, к которой принадлежит переменная </param>
      /// <param name="aRegInDev">- адрес в устройстве, где лежит данная переменная</param>
      /// <param name="aName">- имя переменной</param>
      /// <param name="aCaption">- подпись для переменной</param>
      /// <param name="aDim">- размерность переменной</param>
      /// <param name="aInset">- номер вкладки на панели уставок</param>
      /// <param name="aType">- тип (положение точки) - см. протокол описания MTRele</param>
      /// <param name="aMask">- маска для извлечения части регистра</param>
      public TBCDPackVariable( TCRZAGroup aGroup, int aRegInDev, string aName, string aCaption, string aDim, int aInset, int aType, string aMask )
         : this( aGroup, aRegInDev, aName, aCaption, aDim, aInset, aType)
      {
         MASK = aMask;
      }

      /// <summary>
      /// public override string ToString( )
      /// </summary>
      /// <returns></returns>
      public override string ToString( )
      {
         return "TBCDPackVariable";
      }

      /// <summary>
      /// private void fChVari() 
      /// генерация события по изменению тега
      /// </summary>
      private void fChVari()
      {
         if (OnChangeVar != null)
            OnChangeVar();
      }

      /// <summary>
      /// public override void ExtractVarFrom(ArrayList MTD)
      /// извлечь переменную из источника данных, преобразовать и поместить в поле Value
      /// </summary>
      /// <param name="MTD"></param>
      public override void ExtractVarFrom(ArrayList MTD)
      {
         StringBuilder sb = new StringBuilder();

         if( ( Quality == VarQuality.vqUndefined ) || ( ( this.Group.Device.crzaDeviceState != CRZADeviceState.CRZAdstOnline ) && Quality != VarQuality.vqArhiv ) )
         {
            for( int i = 0 ; i < length_B ; i++ )
               sb.Append( 0 );

            Value = sb.ToString();
            // вызываем событие обновления link'ов
            fChVari();
            return;
         }

         sb.Length = 0;
         char[] sTmp = new char[2];
         // извлечем raw-значение и сравним его со старым
         foreach(BMRZ aBMRZ in MTD)
         {
            if (this.Group.Device.NumDev == aBMRZ.NDev)
            {
               if( aBMRZ.varDev[RegInDev] is BCDPack_FieldMT )
               {
                  BCDPack_FieldMT temp = ( BCDPack_FieldMT ) aBMRZ.varDev[RegInDev];
                  if (!firstRead)
                     abValue = new byte[temp.varMT_Len];

                  if( abValue != temp.varMT_Value | !firstRead )
                  {
                     firstRead = true;
                     abValue = temp.varMT_Value;
                     // если есть маска то выделим байты по ней 
                     if( MASK != "" )
                     {
                        // преобразуем в массив байт
                        byte[] tmask = new byte[MASK.Length / 2];
										 
                        for( int i = 0 ; i < temp.varMT_Len ; i++ )
                        {
                           MASK.CopyTo( i * 2, sTmp, 0, 2 );
                           string ssTmp = new string( sTmp );
                           tmask[i] = byte.Parse( ssTmp, System.Globalization.NumberStyles.HexNumber );
                        }
                        
                        // выделим байты по маске
                        for( int i = 0 ; i < temp.varMT_Len ; i++ )
                           abValue[i] &= (byte)tmask[i];
                     }

                     // настоящее значение в виде строки
                     for( int i = 0; i < temp.varMT_Len; i++ )
                        sb.Append( abValue[i].ToString( "X2" ) );

                     Value = sb.ToString();
                     //вставить точку в соответсвии с типом
                     switch (type.ToString())
                     {
                        case "0":
                           // без точки
                           break;
                        case "2":
                           Value = Value.Insert( 1, "." );
                           break;
                        case "3":
                           Value = Value.Insert( 2, "." );
                           break;                                
                        case "4":
                           Value = Value.Insert( 3, "." );
                           break;
                        default:
                           MessageBox.Show("Тип уставок не поддерживается");
                           break;
                     }
                     // вызываем событие обновления link'ов
                     fChVari();
                  }
               }
               break;
            }
         }
      }

      /// <summary>
      /// перегруженная функция извлечения значения с нижнего уровня по событию
      /// </summary>
      /// <param name="varval">значение переменной</param>
      /// <param name="memb">массив байт представления переменной</param>
      /// <param name="lenR">длина в рег. MODBUS</param>
      /// <param name="lenB">длина в байтах</param>
      public override void ExtractVarFrom( string typeVar, object varval, byte[] memb, int lenR, int lenB )
      {
         StringBuilder sb = new StringBuilder( );

         if( ( Quality == VarQuality.vqUndefined ) || ( ( this.Group.Device.crzaDeviceState != CRZADeviceState.CRZAdstOnline ) && Quality != VarQuality.vqArhiv ) )
         {
            for( int i = 0 ;i < length_B ;i++ )
               sb.Append( 0 );

            newValue = sb.ToString( );

            if( ( Value != newValue ) || !firstRead )
            {
               firstRead = true;

               Value = newValue;
               // вызываем событие обновления link'ов
               fChVari( );
            }
            return;
         }

         sb.Length = 0;
         char[] sTmp = new char[ 2 ];
          
         byte[] varVal = new byte[ memb.Length ];
         Buffer.BlockCopy( memb, 0, varVal, 0, memb.Length );

         if( !firstRead )
            abValue = new byte[ lenB ];

         if( abValue != varVal | !firstRead )
         {
            firstRead = true;
            abValue = varVal;
            // если есть маска то выделим байты по ней 
            if( MASK != "" )
            {
               // преобразуем в массив байт
               byte[] tmask = new byte[ MASK.Length / 2 ];

               for( int i = 0 ;i < varVal.Length ;i++ )
               {
                  MASK.CopyTo( i * 2, sTmp, 0, 2 );
                  string ssTmp = new string( sTmp );
                  tmask[ i ] = byte.Parse( ssTmp, System.Globalization.NumberStyles.HexNumber );
               }
               // выделим байты по маске
               for( int i = 0 ;i < varVal.Length ;i++ )
                  abValue[ i ] &= ( byte ) tmask[ i ];
            }

            // настоящее значение в виде строки
            for( int i = 0 ;i < varVal.Length ;i++ )
               sb.Append( abValue[ i ].ToString( "X2" ) );

            newValue = sb.ToString( );
            
            //вставить точку в соответсвии с типом
            switch( type.ToString( ) )
            {
               case "0":
                  // без точки
                  break;
               case "2":
                  newValue = newValue.Insert( 1, "." );
                  break;
               case "3":
                  newValue = newValue.Insert( 2, "." );
                  break;
               case "4":
                  newValue = newValue.Insert( 3, "." );
                  break;
               default:
                  MessageBox.Show( "Тип уставок не поддерживается" );
                  break;
            }

            if( ( Value != newValue ) || !firstRead )
            {
               Value = newValue;
               // вызываем событие обновления link'ов
               fChVari( );
            }
         }
      }

      /// <summary>
      /// public virtual void PlaceVarTo( ArrayList MTD, VarQuality vQ )
      /// поместить переменную в приемник данных
      /// </summary>
      /// <param name="MTD"></param>
      /// <param name="val"></param>
      public override void PlaceVarTo( ArrayList MTD, VarQuality vQ )
      {
         StringBuilder sb = new StringBuilder();

         if( vQ == VarQuality.vqUndefined )
         {
            for( int i = 0 ; i < length_B ; i++ )
               sb.Append(0);

            Value = sb.ToString();
         }
				  
         // вызываем событие обновления link'ов
         fChVari();
         // обнуляем данные и на нижнем уровне
         foreach( BMRZ aBMRZ in MTD )
         {
            if( this.Group.Device.NumDev == aBMRZ.NDev )
            {
               if( aBMRZ.varDev[RegInDev] is BCDPack_FieldMT )
               {
                  BCDPack_FieldMT temp = ( BCDPack_FieldMT ) aBMRZ.varDev[RegInDev];

                  for( int i = 0 ; i < length_B ; i++ )				
                     temp.varMT_Value[i] = 0;				
               }
               break;
            }
         }
      }
   }
	#endregion

   #region class TBCDVariable
   /// <summary>
   /// class TBCDVariable : TCRZAVariable
   /// представление переменной в упакованном BCD-формате - длина не фиксирована
   /// </summary>
   public class TBCDVariable : TCRZAVariable
   {
      // объявляем событие
      override public event ChVar OnChangeVar;

      private TCRZAGroup Group;
      public int length_R = 1; //длина в 16 разрядных словах (регистрах MODBUS)
      public int length_B = 2; //длина в байтах
      // величина переменной
      public uint Value;
      public uint newValue;
   
      /// <summary>
      /// конструктор
      /// </summary>
      /// <param name="aGroup">- группа, к которой принадлежит переменная </param>
      /// <param name="aRegInDev">- адрес в устройстве, где лежит данная переменная</param>
      /// <param name="aName">- имя переменной</param>
      /// <param name="aCaption">- подпись для переменной</param>
      /// <param name="aDim">- размерность переменной</param>
      public TBCDVariable( TCRZAGroup aGroup, int aRegInDev, string aName, string aCaption, string aDim )
      {
         Group = aGroup;
         RegInDev = aRegInDev;
         Caption = aCaption;
         Dim = aDim;
         Name = aName;
         firstRead = false;
         Quality = VarQuality.vqUndefined;
         Value = 0;
      }

      /// <summary>
      /// public override string ToString( )
      /// </summary>
      /// <returns></returns>
      public override string ToString( )
      {
         return "TBCDVariable";
      }

      /// <summary>
      /// private void fChVari()
      /// генерация события по изменению тега
      /// </summary>
      private void fChVari( )
      {
         if( OnChangeVar != null )
            OnChangeVar();
      }

      /// <summary>
      /// public override void ExtractVarFrom(ArrayList MTD)
      /// извлечь переменную из источника данных, преобразовать и поместить в поле Value
      /// </summary>
      /// <param name="MTD"></param>
      public override void ExtractVarFrom( ArrayList MTD )
      {
         if( ( Quality == VarQuality.vqUndefined ) || ( ( this.Group.Device.crzaDeviceState != CRZADeviceState.CRZAdstOnline ) && Quality != VarQuality.vqArhiv ) )
         {
            Value = 0;
            // вызываем событие обновления link'ов
            fChVari();
            return;
         }

         // пока просто извлечем raw-значение и сравним его со старым
         foreach( BMRZ aBMRZ in MTD )
         {
            if( this.Group.Device.NumDev == aBMRZ.NDev )
            {
               if( aBMRZ.varDev[RegInDev] is BCD_FieldMT )
               {
                  BCD_FieldMT temp = ( BCD_FieldMT ) aBMRZ.varDev[RegInDev];
                  
                  if( ( Value != temp.varMT_Value ) | !firstRead )
                  {
                     firstRead = true;

                     Value = ( uint ) temp.varMT_Value;
                     // вызываем событие обновления link'ов
                     fChVari();
                  }
               }
               break;
            }
         }
      }

      /// <summary>
      /// перегруженная функция извлечения значения с нижнего уровня по событию
      /// </summary>
      /// <param name="varval">значение переменной</param>
      /// <param name="memb">массив байт представления переменной</param>
      /// <param name="lenR">длина в рег. MODBUS</param>
      /// <param name="lenB">длина в байтах</param>
      public override void ExtractVarFrom( string typeVar, object varval, byte[] memb, int lenR, int lenB )
      {
         if( ( Quality == VarQuality.vqUndefined ) || ( ( this.Group.Device.crzaDeviceState != CRZADeviceState.CRZAdstOnline ) && Quality != VarQuality.vqArhiv ) )
            newValue = 0;
         else 
            newValue = Convert.ToUInt32(varval);

         if( ( Value != newValue ) || !firstRead )
         {
            firstRead = true;

            Value = newValue;
            // вызываем событие обновления link'ов
            fChVari( );
         }     
      }

      /// <summary>
		/// public virtual void PlaceVarTo( ArrayList MTD, VarQuality vQ )
		/// поместить переменную в приемник данных
		/// </summary>
		/// <param name="MTD"></param>
		/// <param name="val"></param>
		public override void PlaceVarTo( ArrayList MTD, VarQuality vQ )
		{
         if( vQ == VarQuality.vqUndefined )
            Value = 0;
         
         // вызываем событие обновления link'ов
         fChVari();
         
         // обнуляем данные и на нижнем уровне
         foreach( BMRZ aBMRZ in MTD )
         {
            if( this.Group.Device.NumDev == aBMRZ.NDev )
            {
               if( aBMRZ.varDev[RegInDev] is BCD_FieldMT )
               {
                  BCD_FieldMT temp = ( BCD_FieldMT ) aBMRZ.varDev[RegInDev];
                  temp.varMT_Value = Value;
               }
               break;
            }
         }
		}
	}
	#endregion

   #region class TByteField2CBVariable - байт в ComboBox
   /// <summary>
   /// class TByteField2CBVariable : TCRZAVariable
   /// представление переменной типа массива байт, из которого можно извлекать значение отдельного байта как индекса для ComboBox
   /// </summary>
   public class TByteField2CBVariable : TCRZAVariable
   {
      // объявляем событие
      override public event ChVar OnChangeVar;

      private TCRZAGroup Group;
      public int length_R = 0; //длина в 16 разрядных словах (регистрах MODBUS)
      public int length_B = 0; //длина в байтах
      public string bitMask = "";	// строковое значение маски для привязки
      byte[] aHexBitMask;
      BitArray hexBitMask;		// hex-битовая маска, по которой формируется итоговое значение тега
      // величина переменной
       public ushort Value;
      private ushort newValue;
      private byte[] memX;	// для хранения raw-значения переменной
      private BitArray memXBA;	// для преобразования 
      private BitVector32 Value_bv32;
      
      /// <summary>
      /// public TByteField2UIntVariable( TCRZAGroup aGroup, int aRegInDev, string aName, string aCaption, string aDim, int aLengthInByte, aBitMask )
      /// конструктор - длина битового поля не более 4 байт
      /// </summary>
      /// <param name="aGroup">- группа, к которой принадлежит переменная</param>
      /// <param name="aRegInDev">- адрес в устройстве, где лежит данная переменная</param>
      /// <param name="abitMask">- битовая маска в виде массива байтовых значений, показывает биты, которые формируют итоговое значение тега 
      ///									размер маски не более 32 бит</param>
      /// <param name="aName">- имя переменной</param>
      /// <param name="aCaption">- подпись для переменной</param>
      /// <param name="aDim">- размерность переменной</param>
      public TByteField2CBVariable( TCRZAGroup aGroup, int aRegInDev, string abitMask, string aName, string aCaption, ArrayList arrLiString )
      {
         Group = aGroup;
         RegInDev = aRegInDev;
         Caption = aCaption;
         Name = aName;
         firstRead = false;
         Quality = VarQuality.vqUndefined;
         ArrLiStringCB = ( ArrayList ) arrLiString.Clone( );
         Value = 0;
         // получим массив байт aHexBitMask из строки abitMask
         abitMask = abitMask.Trim( );
         bitMask = abitMask.Trim( );
         // сначала проверим
         aHexBitMask = new byte[ abitMask.Length / 2 ];
         length_B = aHexBitMask.Length;
         length_R = length_B / 2;

         if( ( ( abitMask.Length % 2 ) == 1 ) || ( aHexBitMask.Length > 4 ) )
            throw new Exception( "Ошибка при инициализации переменной типа TByteField2CBVariable" );

         // преобразуем
         StringBuilder sb = new StringBuilder( );
         for( int j = 0 ;j < aHexBitMask.Length ;j++ )
            for( int i = abitMask.Length - 1 ;i > 0 ;i -= 2 )
            {
               sb.Length = 0;
               sb.Append( abitMask[ i - 1 ] );
               sb.Insert( 0, abitMask[ i ] );
               aHexBitMask[ j ] = byte.Parse( sb.ToString( ), NumberStyles.HexNumber );
               j++;
            }

         // инициализируем массив байт с маской
         hexBitMask = new BitArray( aHexBitMask );
         memX = new byte[ length_B ];

         Value_bv32 = new BitVector32( 0 );
      }

      /// <summary>
      /// public override string ToString( )
      /// </summary>
      /// <returns></returns>
      public override string ToString()
      {
         return "TByteField2CBVariable";
      }
      /// <summary>
      /// private void fChVari()
      /// генерация события по изменению тега
      /// </summary>
      private void fChVari()
      {
         if( OnChangeVar != null )
            OnChangeVar( );
      }
      /// <summary>
      /// public override void ExtractVarFrom(ArrayList MTD)
      /// извлечь переменную из источника данных, преобразовать и поместить в поле Value
      /// </summary>
      /// <param name="MTD"></param>
      public override void ExtractVarFrom( ArrayList MTD )
      {
         if( ( Quality == VarQuality.vqUndefined ) || ( ( this.Group.Device.crzaDeviceState != CRZADeviceState.CRZAdstOnline ) && Quality != VarQuality.vqArhiv ) )
         {
            Value = 0;
            // вызываем событие обновления link'ов
            fChVari( );
            return;
         }

         // пока просто извлечем raw-значение и сравним его со старым
         Byte_FieldMT temp;
         Value_bv32 = new BitVector32( 0 );

         foreach( BMRZ aBMRZ in MTD )
         {
            if( this.Group.Device.NumDev == aBMRZ.NDev )
            {
               if( aBMRZ.varDev[ RegInDev ] is Byte_FieldMT )
               {
                  temp = ( Byte_FieldMT ) aBMRZ.varDev[ RegInDev ];

                  if( !firstRead )
                  {
                     for( int i = 0 ;i < length_B ;i++ )
                     {
                        if( memX[ i ] == temp.varMT_Value[ i ] )
                           continue;	// если равен
                        else
                           goto m1;
                     }
                     return;
                  }
               m1:							// если не равен
                  firstRead = true;

                  // извлекаем новое значение и формируем uint
                  for( int i = 0 ;i < length_B ;i++ )
                     memX[ i ] = temp.varMT_Value[ i ];

                  memXBA = new BitArray( temp.varMT_Value );	// значение байтового массива во временную переменную
                  int j = 1;
                  for( int i = 0 ;i < memXBA.Count ;i++ )
                  {
                     if( hexBitMask[ i ] )	// обнаруживаем первый единичный бит маски
                     {
                        while( hexBitMask[ i ] )	// проходим маску и соответсвующие биты исходного массива, формируем результат
                        {
                           if( hexBitMask[ i ] && memXBA[ i ] )
                              Value_bv32[ j ] = true;
                           else
                              Value_bv32[ j ] = false;

                           i++;
                           j *= 2;
                           if( i == memXBA.Count - 1 )
                              break;
                        }

                        if( Value != ( ushort ) Value_bv32.Data )
                        {
                           Value = ( ushort ) Value_bv32.Data;
                           // вызываем событие обновления link'ов
                           fChVari( );
                        }
                        break;
                     }
                  }
               }
               break;
            }
         }
      }

      /// <summary>
      /// перегруженная функция извлечения значения с нижнего уровня по событию
      /// </summary>
      /// <param name="varval">значение переменной</param>
      /// <param name="memb">массив байт представления переменной</param>
      /// <param name="lenR">длина в рег. MODBUS</param>
      /// <param name="lenB">длина в байтах</param>
      public override void ExtractVarFrom( string typeVar, object varval, byte[] memb, int lenR, int lenB )
      {
         if( ( Quality == VarQuality.vqUndefined ) || ( ( this.Group.Device.crzaDeviceState != CRZADeviceState.CRZAdstOnline ) && Quality != VarQuality.vqArhiv ) )
         {
            newValue = 0;

            if( ( Value != newValue ) || !firstRead )
            {
               firstRead = true;

               Value = newValue;
               // вызываем событие обновления link'ов
               fChVari( );
            }
            return;
         }

         Value_bv32 = new BitVector32( 0 );
         byte[] varVal = new byte[ lenB ];
         Buffer.BlockCopy( memb, 0, varVal, 0, lenB );

         // извлекаем новое значение и формируем uint
         for( int i = 0 ;i < length_B ;i++ )
            memX[ i ] = varVal[ i ];

         memXBA = new BitArray( varVal );	// значение байтового массива во временную переменную
         int j = 1;
         for( int i = 0 ;i < memXBA.Count ;i++ )
         {
            if( hexBitMask[ i ] )	// обнаруживаем первый единичный бит маски
            {
               while( hexBitMask[ i ] )	// проходим маску и соответсвующие биты исходного массива, формируем результат
               {
                  if( hexBitMask[ i ] && memXBA[ i ] )
                     Value_bv32[ j ] = true;
                  else
                     Value_bv32[ j ] = false;

                  i++;
                  j *= 2;
                  
                  if( i == memXBA.Count - 1 )
                     break;
               }
                        
               newValue = ( ushort ) Value_bv32.Data;

               if( ( Value != newValue ) || !firstRead )
               {
                  firstRead = true;

                  Value = newValue;
                  // вызываем событие обновления link'ов
                  fChVari( );
               }
            }
         }
      }

      public override void PlaceVarTo( ArrayList MTD, VarQuality vQ )
      {
         if( vQ == VarQuality.vqUndefined )
            Value = 0;

         // вызываем событие обновления link'ов
         fChVari( );
         // обнуляем данные и на нижнем уровне
         foreach( BMRZ aBMRZ in MTD )
         {
            if( this.Group.Device.NumDev == aBMRZ.NDev )
            {
               if( aBMRZ.varDev[ RegInDev ] is Byte_FieldMT )
               {
                  Byte_FieldMT temp = ( Byte_FieldMT ) aBMRZ.varDev[ RegInDev ];
                  for( int i = 0 ;i < temp.varMT_Len ;i++ )
                     temp.varMT_Value[ i ] = 0;
               }
               break;
            }
         }
      }
   }
   #endregion

   #region class TUIntCBVariable - 16-разрядный UInt, ассоциированный со строкой для отображения в ComboBox
   /// <summary>
   /// class TUIntCBVariable : TCRZAVariable
   /// представление переменной типа UInt - 16-разрядного двоичного числа без знака,
   /// ассоциированного со строкой для отображения в ComboBox
   /// </summary>
   public class TUIntCBVariable : TCRZAVariable
   {
      // объявляем событие
      override public event ChVar OnChangeVar;

      private TCRZAGroup Group;

      public int length_R = 1; //длина в 16 разрядных словах (регистрах MODBUS)
      public int length_B = 2; //длина в байтах

      // величина переменной
      public ushort Value;
      private ushort newValue;
      
      /// <summary>
      /// конструктор
      /// </summary>
      /// <param name="aGroup">- группа, к которой принадлежит переменная</param>
      /// <param name="aRegInDev">- адрес в устройстве, где лежит данная переменная</param>
      /// <param name="aName">- имя переменной</param>
      /// <param name="aCaption">- подпись для переменной</param>
      /// <param name="arrLiString">- строки в combobox</param>
      public TUIntCBVariable( TCRZAGroup aGroup, int aRegInDev, string aName, string aCaption, ArrayList arrLiString )
      {
         Group = aGroup;
         RegInDev = aRegInDev;
         Caption = aCaption;
         Name = aName;
         firstRead = false;
         Quality = VarQuality.vqUndefined;
         ArrLiStringCB = ( ArrayList ) arrLiString.Clone( );
         Value = 0;
      }
      /// <summary>
      /// public override string ToString( )
      /// </summary>
      /// <returns></returns>
      public override string ToString()
      {
         return "TUIntCBVariable";
      }

      /// <summary>
      /// private void fChVari() 
      /// генерация события по изменению тега
      /// </summary>
      private void fChVari()
      {
         if( OnChangeVar != null )
            OnChangeVar( );
      }

      /// <summary>
      /// public override void ExtractVarFrom(ArrayList MTD)
      /// извлечь переменную из источника данных, преобразовать и поместить в поле Value
      /// </summary>
      /// <param name="MTD"></param>
      public override void ExtractVarFrom( ArrayList MTD )
      {
         if( ( Quality == VarQuality.vqUndefined ) || ( ( this.Group.Device.crzaDeviceState != CRZADeviceState.CRZAdstOnline ) && Quality != VarQuality.vqArhiv ) )
         {
            Value = 0;
            // вызываем событие обновления link'ов
            fChVari( );
            return;
         }

         // пока просто извлечем raw-значение и сравним его со старым
         foreach( BMRZ aBMRZ in MTD )
         {
            if( this.Group.Device.NumDev == aBMRZ.NDev )
            {
               if( aBMRZ.varDev[ RegInDev ] is UInt_FieldMT )
               {
                  UInt_FieldMT temp = ( UInt_FieldMT ) aBMRZ.varDev[ RegInDev ];
                  if( ( Value != temp.varMT_Value ) | !firstRead )
                  {
                     firstRead = true;
                     ArrayList a = this.ArrLiStringCB;
                     Value = ( ushort ) temp.varMT_Value;
                     // вызываем событие обновления link'ов
                     fChVari( );
                  }
               }
               break;
            }
         }
      }

      /// <summary>
      /// перегруженная функция извлечения значения с нижнего уровня по событию
      /// </summary>
      /// <param name="varval">значение переменной</param>
      /// <param name="memb">массив байт представления переменной</param>
      /// <param name="lenR">длина в рег. MODBUS</param>
      /// <param name="lenB">длина в байтах</param>
      public override void ExtractVarFrom( string typeVar, object varval, byte[] memb, int lenR, int lenB )
      {
         if( ( Quality == VarQuality.vqUndefined ) || ( ( this.Group.Device.crzaDeviceState != CRZADeviceState.CRZAdstOnline ) && Quality != VarQuality.vqArhiv ) )
            newValue = 0;
         else
            newValue = Convert.ToUInt16( varval );

         if( ( Value != newValue ) || !firstRead )
         {
            firstRead = true;

            ArrayList a = this.ArrLiStringCB;

            Value = newValue;
            // вызываем событие обновления link'ов
            fChVari( );
         }
     }

      /// <summary>
      /// public virtual void PlaceVarTo( ArrayList MTD, VarQuality vQ )
      /// поместить переменную в приемник данных
      /// </summary>
      /// <param name="MTD"></param>
      /// <param name="val"></param>
      public override void PlaceVarTo( ArrayList MTD, VarQuality vQ )
      {
         if( vQ == VarQuality.vqUndefined )
            Value = 0;

         // вызываем событие обновления link'ов
         fChVari( );
         // обнуляем данные и на нижнем уровне
         foreach( BMRZ aBMRZ in MTD )
         {
            if( this.Group.Device.NumDev == aBMRZ.NDev )
            {
               if( aBMRZ.varDev[ RegInDev ] is UInt_FieldMT )
               {
                  UInt_FieldMT temp = ( UInt_FieldMT ) aBMRZ.varDev[ RegInDev ];

                  temp.varMT_Value = Value;
               }
               break;
            }
         }
      }
   }
   #endregion

	#region class TCRZACommand
	/// <summary>
   /// class TCRZACommand
   /// представление команды
   /// </summary>
   public class TCRZACommand
   {
      public string Caption;   // подпись команды
      public string Name;      // имя команды
      public bool StatusActive = false; // состояние процесса ожидания: true - команда была послана устройству
      private bool StatusCommand = false;       // состояние команды - выполняется (true) или нет (false)
      private bool lastStatusCommand = false;   // предыдущее состояние команды
      private string bitNum;  // маска бита в регистре по адресу 60000, который соответсвует команде
      public string extraName;
      ToolStripProgressBar toolStripProgressBar1;
      StatusStrip statusStrip1;
      Form parentForm;
      private bool errorMesVis = false;   // для исключения многократной выдачи сообщения об ошибке

      private TCRZADirectDevice dev;
      //-----------------------------------------------------------
      // для подсчета времени выполнения команды
      [System.Runtime.InteropServices.DllImport( "kernel32.dll" )]
         extern static short QueryPerformanceCounter( ref long x );
      [System.Runtime.InteropServices.DllImport( "kernel32.dll" )]
         extern static short QueryPerformanceFrequency( ref long x );

      long ctr1 = 0, ctr2 = 0, freq = 0;/**/
      double interval2 = 0;
      System.Threading.Timer tm;
      //-----------------------------------------------------------

      /// <summary>
      /// конструктор
      /// </summary>
      /// <param name="gDevice"></param>
      /// <param name="aName"></param>
      /// <param name="aCaption"></param>
      public TCRZACommand( TCRZADirectDevice gDevice, string aName, string aCaption )
      { 
         Caption  = aCaption;
         Name = aName;
         extraName = String.Empty;
         dev = gDevice;
      }
      
      void TmTick( object state )
      {
         IsStatusCommand();
      }
      
      /// <summary>
      /// public bool Execute( )
      /// запуск команды на выполнение 
      /// </summary>
      /// <param name="netman"></param>
      /// <param name="cmd"></param>
      /// <param name="dopName">дополнительное имя для команд типа API</param>
      /// <param name="parameters"></param>
      /// <param name="aProgressBar"></param>
      /// <returns></returns> 
      public bool Execute(INetManager netman, string cmd, string dopName, byte[] parameters, ToolStripProgressBar aProgressBar, StatusStrip statusStrip, Form prnt  )
      {
         errorMesVis = false;

         byte[] scmd1;
         byte[] scmd2;
         bool success = false; // результат выполнения команды

         if( dopName != String.Empty )
            this.extraName = dopName;  // дополнительное имя для команд типа API

         toolStripProgressBar1 = aProgressBar;
         statusStrip1 = statusStrip;
         parentForm = prnt;
 
         int cmd1 = dev.NumDev;

         StringBuilder cmd2 = new StringBuilder();

         System.Diagnostics.Trace.TraceInformation( "Выдана команда << " + cmd + " >> устройство: {0} № {1}", dev.ToString(), dev.NumDev);

         switch( cmd )
         {
            case "NOP":    // пустая команда
               // формируем область данных пакета
               // номер устройства
               scmd1 = BitConverter.GetBytes( cmd1 );
               // команда
               cmd2.Append( "NOP\0" );
               scmd2 = Encoding.UTF8.GetBytes( cmd2.ToString() );
               
               if( netman.senddata( scmd1[0], scmd2, parameters ) )
               {
                  StatusActive = true; // команда послана, запускаем процесс ожидания исполнения
                  bitNum = "0000";	// нет соответствующего бита
                  success = true;
               }

               break;
            case "RBH":    // прочитать ресурс ТП
               // формируем область данных пакета
               // номер устройства
               scmd1 = BitConverter.GetBytes( cmd1 );
               // команда
               cmd2.Append( "RBH\0" );
               scmd2 = Encoding.UTF8.GetBytes( cmd2.ToString() );
               
               if( netman.senddata( scmd1[0], scmd2, parameters ) )
               {
                  StatusActive = true; // команда послана, запускаем процесс ожидания исполнения
                  bitNum = "1000";
                  success = true; 
               }

               break;
               case "IMB":    // включить сообщение о ресурсе ТП
                  // формируем область данных пакета
                  // номер устройства
                  scmd1 = BitConverter.GetBytes( cmd1 );
                  // команда
                  cmd2.Append( "IMB\0" );
                  scmd2 = Encoding.UTF8.GetBytes( cmd2.ToString() );

                  if( netman.senddata( scmd1[0], scmd2, parameters ) )
                  {
                     StatusActive = true; // команда послана, запускаем процесс ожидания исполнения
                     bitNum = "0001";
                     success = true; 
                  }
                  break;
                  case "OCB":    // выключатель разомкнуть
                     // формируем область данных пакета
                     // номер устройства
                     scmd1 = BitConverter.GetBytes( cmd1 );
                     // команда
                     cmd2.Append( "OCB\0" );
                     scmd2 = Encoding.UTF8.GetBytes( cmd2.ToString() );
                     
                     if( netman.senddata( scmd1[0], scmd2, parameters ) )
                     {
                         StatusActive = true; // команда послана, запускаем процесс ожидания исполнения
                         bitNum = "0001";
								 success = true; 
                     }
                     break;
                 case "CCB":    // выключатель замкнуть
                     // формируем область данных пакета
                     // номер устройства
                     scmd1 = BitConverter.GetBytes( cmd1 );
                     // команда
                     cmd2.Append( "CCB\0" );
                     scmd2 = Encoding.UTF8.GetBytes( cmd2.ToString() );

                     if( netman.senddata( scmd1[0], scmd2, parameters ) )
                     {
                         StatusActive = true; // команда послана, запускаем процесс ожидания исполнения
                         bitNum = "0002";
								 success = true; 
                     }
                     break;
                 case "CCD":    // очистить накопитель
                     // формируем область данных пакета
                     // номер устройства
                     scmd1 = BitConverter.GetBytes( cmd1 );
                     // команда
                     cmd2.Append( "CCD\0" );
                     scmd2 = Encoding.UTF8.GetBytes( cmd2.ToString() );
                     
                    if( netman.senddata( scmd1[0], scmd2, parameters ) )
                     {
                         StatusActive = true; // команда послана, запускаем процесс ожидания исполнения
                         bitNum = "0004";
								 success = true; 
                     }
                     break;
                 case "ECC":    // квитировать
                     // формируем область данных пакета
                     // номер устройства
                     scmd1 = BitConverter.GetBytes( cmd1 );
                     // команда
                     cmd2.Append( "ECC\0" );
                     scmd2 = Encoding.UTF8.GetBytes( cmd2.ToString() );
                     
                    if( netman.senddata( scmd1[0], scmd2, parameters ) )
                     {
                         StatusActive = true; // команда послана, запускаем процесс ожидания исполнения
                         bitNum = "0008";
								 success = true; 
                     }
                     break;
						case "IME":    // включить в исходящие пакеты записи аварий и журнала
							// формируем область данных пакета
							// номер устройства
							scmd1 = BitConverter.GetBytes( cmd1 );
							// команда
							cmd2.Append( "IME\0" );
							scmd2 = Encoding.UTF8.GetBytes( cmd2.ToString() );
							
                     if( netman.senddata( scmd1[0], scmd2, parameters ) )
							{
								StatusActive = true; // команда послана, запускаем процесс ожидания исполнения
								bitNum = "0000";
								success = true;
							}
							break;
                 case "CMD":    // сбросить максметр 
                     // формируем область данных пакета
                     // номер устройства
                     scmd1 = BitConverter.GetBytes( cmd1 );
                     // команда
                     cmd2.Append( "CMD\0" );
                     scmd2 = Encoding.UTF8.GetBytes( cmd2.ToString() );
                     
                    if( netman.senddata( scmd1[0], scmd2, parameters ) )
                     {
                         StatusActive = true; // команда послана, запускаем процесс ожидания исполнения
                         bitNum = "0010";
								 success = true; 
                     }
                     break;
                 case "RCD":    // прочитать накопитель
                     // формируем область данных пакета
                     // номер устройства
                     scmd1 = BitConverter.GetBytes( cmd1 );
                     // команда
                     cmd2.Append( "RCD\0" );
                     scmd2 = Encoding.UTF8.GetBytes( cmd2.ToString() );
                     
                    if( netman.senddata( scmd1[0], scmd2, parameters ) )
                     {
                         StatusActive = true; // команда послана, запускаем процесс ожидания исполнения
                         bitNum = "0020";
								 success = true; 
                     }
                     break;
                 case "RCP":    // прочитать уставки
                     // формируем область данных пакета
                     // номер устройства
                     scmd1 = BitConverter.GetBytes( cmd1 );
                     // команда
                     cmd2.Append( "RCP\0" );
                     scmd2 = Encoding.UTF8.GetBytes( cmd2.ToString() );
                     
                    if( netman.senddata( scmd1[0], scmd2, parameters ) )
                     {
                         StatusActive = true; // команда послана, запускаем процесс ожидания исполнения
                         bitNum = "0040";
								 success = true; 
                     }
                     break;
                 case "WCP":    // записать уставки
                     // формируем область данных пакета
                     // номер устройства
                     scmd1 = BitConverter.GetBytes( cmd1 );
                     // команда
                     cmd2.Append( "WCP\0" );
                     scmd2 = Encoding.UTF8.GetBytes( cmd2.ToString() );
                     if( netman.senddata( scmd1[0], scmd2, parameters ) )
                     {
                         StatusActive = true; // команда послана, запускаем процесс ожидания исполнения
                         bitNum = "0080";
								 success = true; 
                     }
                     break;
						case "CLS":    // замкнуть цепь выхода (УСО)
							// формируем область данных пакета
							// номер устройства
							scmd1 = BitConverter.GetBytes( cmd1 );
							// команда
							cmd2.Append( "CLS\0" );
							scmd2 = Encoding.UTF8.GetBytes( cmd2.ToString() );
							
                     if( netman.senddata( scmd1[0], scmd2, parameters ) )
							{
								StatusActive = true; // команда послана, запускаем процесс ожидания исполнения
								bitNum = "0002";
								success = true;
							}
							break;
						case "OPN":    // разомкнуть цепь выхода (УСО)
							// формируем область данных пакета
							// номер устройства
							scmd1 = BitConverter.GetBytes( cmd1 );
							// команда
							cmd2.Append( "OPN\0" );
							scmd2 = Encoding.UTF8.GetBytes( cmd2.ToString() );
							
                     if( netman.senddata( scmd1[0], scmd2, parameters ) )
							{
								StatusActive = true; // команда послана, запускаем процесс ожидания исполнения
								bitNum = "0001";
								success = true;
							}
							break;
						case "API":    // команда префикс
							// формируем область данных пакета
							// номер устройства
							scmd1 = BitConverter.GetBytes( cmd1 );
							// команда
							cmd2.Append( "API\0" );
							scmd2 = Encoding.UTF8.GetBytes( cmd2.ToString() );
							
                     if( netman.senddata( scmd1[0], scmd2, parameters ) )
							{
								StatusActive = true; // команда послана, запускаем процесс ожидания исполнения
								bitNum = "4000";
								success = true;
							}
							break;
						case "WBH":    // записать ресурс
							// формируем область данных пакета
							// номер устройства
							scmd1 = BitConverter.GetBytes( cmd1 );
							// команда
							cmd2.Append( "WBH\0" );
							scmd2 = Encoding.UTF8.GetBytes( cmd2.ToString() );
							
                     if( netman.senddata( scmd1[0], scmd2, parameters ) )
							{
								StatusActive = true; // команда послана, запускаем процесс ожидания исполнения
								bitNum = "2000";
								success = true; 
							}
							break;
                 case "RMD":    // прочитать максметр из памяти блока
                     // формируем область данных пакета
                     // номер устройства
                     scmd1 = BitConverter.GetBytes( cmd1 );
                     // команда
                     cmd2.Append( "RMD\0" );
                     scmd2 = Encoding.UTF8.GetBytes( cmd2.ToString() );
                     
                    if( netman.senddata( scmd1[0], scmd2, parameters ) )
                     {
                         StatusActive = true; // команда послана, запускаем процесс ожидания исполнения
                         bitNum = "0100";
								 success = true; 
                     }
                     break;
                 case "IMC":    // прочитать накопитель
                     // формируем область данных пакета
                     // номер устройства
                     scmd1 = BitConverter.GetBytes( cmd1 );
                     // команда
                     cmd2.Append( "IMC\0" );
                     scmd2 = Encoding.UTF8.GetBytes( cmd2.ToString() );
                     
                    if( netman.senddata( scmd1[0], scmd2, parameters ) )
                     {
                         // IMC пока нет в списке
                         //StatusActive = true; // команда послана, запускаем процесс ожидания исполнения
                         //bitNum = 0;
								success = true; 
                     }
                     break;
                 case "SPC":    // установить период чтения данных накопителя
                     // формируем область данных пакета
                     // номер устройства
					 //scmd1 = BitConverter.GetBytes( cmd1 );
					 scmd1 = BitConverter.GetBytes( 0 );	// команда ФК, здесь фиксирован, т.к. ФК один

                     // команда
                     cmd2.Append( "SPC\0" );
                     scmd2 = Encoding.UTF8.GetBytes( cmd2.ToString() );
                     
                    if( netman.senddata( scmd1[0], scmd2, parameters ) )
                     {
                         // SPC пока нет в списке
                         //StatusActive = true; // команда послана, запускаем процесс ожидания исполнения
                         //bitNum = 0;
								success = true; 
                     }
                     break;
						case "GMT":    // установить время
							// формируем область данных пакета
							// номер устройства
							//scmd1 = BitConverter.GetBytes( cmd1 );
							scmd1 = BitConverter.GetBytes( 0 );	// команда ФК, здесь фиксирован, т.к. ФК пока один

							// команда
							cmd2.Append( "GMT\0" );
							scmd2 = Encoding.UTF8.GetBytes( cmd2.ToString() );
							
                     if( netman.senddata( scmd1[0], scmd2, parameters ) )
							{
								// SPC пока нет в списке
								//StatusActive = true; // команда послана, запускаем процесс ожидания исполнения
								//bitNum = 0;
								success = true; 
							}
							break;
                 case "IMD":    // прочитать максметр из памяти ФК
                     // формируем область данных пакета
                     // номер устройства
                     scmd1 = BitConverter.GetBytes( cmd1 );
                     // команда
                     cmd2.Append( "IMD\0" );
                     scmd2 = Encoding.UTF8.GetBytes( cmd2.ToString() );
                    
                    if( netman.senddata( scmd1[0], scmd2, parameters ) )
                     {
                         // IMD пока нет в списке
                         //StatusActive = true; // команда послана, запускаем процесс ожидания исполнения
                         //bitNum = 0;
								success = true; 
                     }
                     break;
                 case "SPM":    // установить период чтения данных максметра
                     // формируем область данных пакета
                     // номер устройства
                     //scmd1 = BitConverter.GetBytes( cmd1 );
					 scmd1 = BitConverter.GetBytes( 0 );	// команда ФК, здесь фиксирован, т.к. ФК один

					// команда
                     cmd2.Append( "SPM\0" );
                     scmd2 = Encoding.UTF8.GetBytes( cmd2.ToString() );
                     
                    if( netman.senddata( scmd1[0], scmd2, parameters ) )
                     {
                         // CPM пока нет в списке
                         //StatusActive = true; // команда послана, запускаем процесс ожидания исполнения
                         //bitNum = 0;
								success = true; 
                     }
                     break;
                 case "IMP":    // прочитать уставки
                     // формируем область данных пакета
                     // номер устройства
                     scmd1 = BitConverter.GetBytes( cmd1 );
                     // команда
                     cmd2.Append( "IMP\0" );
                     scmd2 = Encoding.UTF8.GetBytes( cmd2.ToString() );
                     
                    if( netman.senddata( scmd1[0], scmd2, parameters ) )
                     {
                         // IMP пока нет в списке
                         //StatusActive = true; // команда послана, запускаем процесс ожидания исполнения
                         //bitNum = 0;
								success = true; 
                     }
                     break;
                 default:
                     System.Diagnostics.Trace.TraceInformation( "Попытка выполнения несуществующей команды" );
							success = false; 
                     break;
             }
				 if( success )
				 {
					 QueryPerformanceCounter( ref ctr1 );	// засекаем время
					 // запускаем таймер отслеживания выполнения команды
					 // таймер, запускается для отслеживания выполнения команды
					 tm = new System.Threading.Timer( new TimerCallback( TmTick ), null, 0, 500 );
					 //toolStripProgressBar1.Value = 25;
					 LinkSetTextTSB( 25 );
				 }

            return success;   // успешный запуск команды
         }
         /// <summary>
         /// public int IsStatusCommand()
         ///    состояние выполнения команды
         /// </summary>
         /// <returns> 0 - все спокойно (StatusActive = false, StatusCommand = false)
         ///           1 - команда послана (StatusActive = true), но устройство еще не приняло команду к исполнению (StatusCommand = false)
         ///           2 - команда послана (StatusActive = true), устройство приняло команду к исполнению (StatusCommand = true)
         ///           3 - команда послана (StatusActive = true), устройство выполнило команду (StatusCommand = false после состояния StatusCommand = true)</returns>
		 public int IsStatusCommand( )
          {
              // выясняем состояние бит в регистре битовых флагов RTU
              for( int i = 0; i < this.dev.Groups.Count; i++ )
              {
                  TCRZAGroup tcg = ( TCRZAGroup ) this.dev.Groups[i];
                  if( tcg.Name == "Результат выполнения команд БМРЗ" )
                     for( int j = 0 ;j < tcg.Variables.Count ;j++ )
                     {
                        TCRZAVariable tcv = ( TCRZAVariable ) tcg.Variables[ j ];
                        if( tcv.RegInDev == 60001 )
                        {
                           TBitFieldVariable tcvBF = ( TBitFieldVariable ) tcv;
                           if( tcvBF.bitMask == bitNum )
                           {
                              lastStatusCommand = StatusCommand;
                              if( tcvBF.Value )
                                 StatusCommand = tcvBF.Value;
                              else
                                 StatusCommand = false;

                              if( ( StatusActive == true && StatusCommand == false && lastStatusCommand == true ) )
                              {
                                 lastStatusCommand = false;
                                 QueryPerformanceCounter( ref ctr2 );
                                 QueryPerformanceFrequency( ref freq );
                                 interval2 = ( ctr2 - ctr1 ) * 1.0 / freq;
                                 tm.Dispose( );
                                 StatusActive = false;
                                 LinkSetTextTSB( 100 );	// toolStripProgressBar1
                                 if( extraName == String.Empty )
                                    MessageBox.Show( parentForm, "Команда << " + this.Caption + " >> выполнена\n Время выполнения: (" + interval2.ToString( "F3" ) + " сек)", "Выполнение команды", MessageBoxButtons.OK, MessageBoxIcon.Information );
                                 else
                                    MessageBox.Show( parentForm, "Команда << " + this.extraName + " >> выполнена\n Время выполнения: (" + interval2.ToString( "F3" ) + " сек)", "Выполнение команды", MessageBoxButtons.OK, MessageBoxIcon.Information );

                                 LinkSetTextTSB( 0 );	// toolStripProgressBar1
                                 return 3;
                              }
                              else if( StatusActive == true && StatusCommand == true )
                              {
                                 lastStatusCommand = StatusCommand;
                                 LinkSetTextTSB( 50 );	// toolStripProgressBar1
                                 return 2;
                              }
                              else if( StatusActive == true && StatusCommand == false )
                              {
                                 // выясняем условие выполнения команды по другим признакам - из устройства
                                 if( dev.LastCommand == ( this.Name + "\0" ) && ( dev.CodeCmdFailure == 0 ) )
                                 {
                                    lastStatusCommand = false;
                                    QueryPerformanceCounter( ref ctr2 );
                                    QueryPerformanceFrequency( ref freq );
                                    interval2 = ( ctr2 - ctr1 ) * 1.0 / freq;
                                    tm.Dispose( );
                                    StatusActive = false;
                                    LinkSetTextTSB( 100 );	// toolStripProgressBar1
                                    //parentForm..check
                                    if( this.extraName == String.Empty )
                                       MessageBox.Show( parentForm, "Команда << " + this.Caption + " >> выполнена\n Время выполнения: (" + interval2.ToString( "F3" ) + " сек)", "Выполнение команды", MessageBoxButtons.OK, MessageBoxIcon.Information );
                                    else
                                       MessageBox.Show( parentForm, "Команда << " + this.extraName + " >> выполнена\n Время выполнения: (" + interval2.ToString( "F3" ) + " сек)", "Выполнение команды", MessageBoxButtons.OK, MessageBoxIcon.Information );

                                    LinkSetTextTSB( 0 );	// toolStripProgressBar1
                                    return 3;	// устройство выполнило команду
                                 }
                                 else if( dev.CodeCmdFailure != 0 )
                                 {
                                    lastStatusCommand = false;
                                    QueryPerformanceCounter( ref ctr2 );
                                    QueryPerformanceFrequency( ref freq );
                                    interval2 = ( ctr2 - ctr1 ) * 1.0 / freq;
                                    LinkSetTextTSB( 100 );	// toolStripProgressBar1
                                    if( !errorMesVis )
                                    {
                                       errorMesVis = true;   // сообщение об ошибке выдано

                                       if( this.extraName == String.Empty )
                                          MessageBox.Show( parentForm, "Команда << " + this.Caption + " >> выполнена с ошибкой\n Время выполнения: (" + interval2.ToString( "F3" ) + " сек)\n Код ошибки: " + ( 10 + dev.CodeCmdFailure ).ToString( ), "Выполнение команды", MessageBoxButtons.OK, MessageBoxIcon.Error );
                                       else
                                          MessageBox.Show( parentForm, "Команда << " + this.extraName + " >> выполнена с ошибкой\n Время выполнения: (" + interval2.ToString( "F3" ) + " сек)\n Код ошибки: " + ( 10 + dev.CodeCmdFailure ).ToString( ), "Выполнение команды", MessageBoxButtons.OK, MessageBoxIcon.Error );
                                    }

                                    tm.Dispose( );
                                    LinkSetTextTSB( 0 );	// toolStripProgressBar1
                                    return 10 + dev.CodeCmdFailure;	// выполнено с ошибкой
                                 }
                                 LinkSetTextTSB( 25 );
                                 return 1;
                              }
                           }
                           else if( bitNum == null )   // для команд типа IMC и IMD
                           {
                              // выясняем условие выполнения команды по другим признакам - из устройства
                              if( ( ( dev.LastCommand == ( this.Name + "\0" ) ) || ( ( dev.LastCommandFC == ( this.Name + "\0" ) ) ) ) && ( dev.CodeCmdFailure == 0 ) )
                              {
                                 lastStatusCommand = false;
                                 QueryPerformanceCounter( ref ctr2 );
                                 QueryPerformanceFrequency( ref freq );
                                 interval2 = ( ctr2 - ctr1 ) * 1.0 / freq;
                                 tm.Dispose( );
                                 StatusActive = false;
                                 LinkSetTextTSB( 100 );	// toolStripProgressBar1
                                 //parentForm..check
                                 if( this.extraName == String.Empty )
                                    MessageBox.Show( parentForm, "Команда << " + this.Caption + " >> выполнена\n Время выполнения: (" + interval2.ToString( "F3" ) + " сек)", "Выполнение команды", MessageBoxButtons.OK, MessageBoxIcon.Information );
                                 else
                                    MessageBox.Show( parentForm, "Команда << " + this.extraName + " >> выполнена\n Время выполнения: (" + interval2.ToString( "F3" ) + " сек)", "Выполнение команды", MessageBoxButtons.OK, MessageBoxIcon.Information );

                                 LinkSetTextTSB( 0 );	// toolStripProgressBar1
                                 return 3;	// устройство выполнило команду
                              }
                           }
                        }
                     }
                     else
                        continue;
              }
              return 0;
          }
			 #region для потокобезопасного вызова процедуры (статусная строка)
			 /*==========================================================================*
			*   private void void LinkSetText(object Value)
			*      для потокобезопасного вызова процедуры
			*==========================================================================*/
			 delegate void SetTextCallback( int progress );

			 public void LinkSetTextTSB( int progress )
			 {
				 if( statusStrip1.InvokeRequired )
				 {
					 SetTextItemTSB( progress );
				 }
				 else
				 {
					 toolStripProgressBar1.Value = progress;
				 }
			 }

			 /*==========================================================================*
			 * private void SetText(object Value)
			 * //для потокобезопасного вызова процедуры
			 *==========================================================================*/
			 private void SetTextItemTSB( int progress )
			 {
				 if( statusStrip1.InvokeRequired )
				 {
					 SetTextCallback d = new SetTextCallback( SetTextItemTSB );
					 parentForm.Invoke( d, new object[] { progress } );
				 }
				 else
					 toolStripProgressBar1.Value = progress;
			 }
			 #endregion
		 }
		 #endregion
}