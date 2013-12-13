/*############################################################################## *
 *    Copyright (C) 2007 Mehanotronika Corporation.                             *
 *    All rights reserved.                                                      *
 *	~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~*
 *                                                                              *
 *	��������: �������� ������ �������������� ��, ����������, ���������� ������  *
 *            ���������� ��� ��������� � ������������ �������� ������.          *
 *                                                                              *
 *	����                     : CRZADevices.cs                                   *
 *	��� ��������� �����      : CRZADevices.dll                                  *
 *	������ �� ��� ���������� : �#, Framework 2.0                                *
 *	�����������              : ���� �.�.                                        *
 *	���� ������ ����������   : xx.02.2007                                       *
 *	���� (v1.0)              :                                                  *
 *******************************************************************************
 * ���������:
 * 1. ����(�����): ...c���������...
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

// ������� �������� ����������
/*public enum TVarQuality
{
	vqUndefined,    // �� ���������� (�� ������������� �� ������ ������,
	// ��� ����� ��� �������� ������������
	vqGood,         // ������� ��������
	vqRangeError,   // ����� �� ������� ���������
};*/

namespace CRZADevices
{
   // ��������� ���������� �� ������� ������
   public enum CRZADeviceState
   {
      CRZAdstNone = 0,
      CRZAdstUndefine = 1, // ��� ����� � �����������
      //dstOffline,  // 
      //dstBlocked,  // 
      CRZAdstOnline = 2    // � ����������� �������������� ���������� �����
   }
   // ������� �� ��������� ����
   public delegate void ChVar();

   /// <summary>
   /// class Configurator
   /// �����, ����������� ���������� ������������
   /// </summary>
   public class Configurator : INetManagerSink
   {
      // ������������
      public static ArrayList MTD; 
      public ArrayList KB;

      // ����� ������ �� ��
      private Thread readFCThread;

		// ������� ������ �����
       //private bool isLostConnection = false;
		 private bool isLostConnection = false;
      public INetManager netman;

      // �������� ������� ���������� ������
       int timeInt = 0;

      //uint countActiveCmd = 0;  // ������� ������������� ������;

		 private DateTime m_crzaTimeFC;

		 BackgroundWorker bcw;	// �����, ����������� ������ �������� ������

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
      /// �����������
      /// </summary>
     
      public Configurator()
      {
         timeInt = 500; // ������ ���������� ������ (����)
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
      /// ������� ������� ������������ ������
      /// </summary>
      /// <returns></returns>
      public ArrayList GetConfigurator(string pathPrjCfgFile, string role)
      {
         // ������������ ������
         KB = new ArrayList( );

         // ���������� ������������ ���������
         string path = Environment.CurrentDirectory + Path.DirectorySeparatorChar + "CRZADevices.dll";
         if( !File.Exists( path ) )
            throw new Exception( "���� " + path + " �� ����������" );

         // �������� ������
         Assembly theAssembly = Assembly.LoadFile( path );
   		
         // ����������� ���������� � ������� ���������� �������������� �������
         
         // ��������� ���� ������������ ��������� �������
         string pathToF = Environment.CurrentDirectory + Path.DirectorySeparatorChar + "Project" + Path.DirectorySeparatorChar + "Project.cfg";
         
         if( !File.Exists( pathToF ) )
            throw new Exception( "���� " + pathToF + " �� ����������" );

         XmlTextReader reader = new XmlTextReader( pathToF );
         XmlDocument doc = new XmlDocument();
         doc.Load( reader );
         reader.Close();
         
         // ������� ���� �� �������
         XmlNode xn;
         XmlElement root = doc.DocumentElement;
         xn = root.SelectSingleNode( "/Project/DeviceConfig/PathDeviceConfig" );
         string PathToFDC = xn.InnerText;	// ���� ������������ ��������� �������
         string cdir = Environment.CurrentDirectory;
         
         if( !File.Exists( PathToFDC ) )
         {
            MessageBox.Show( "���� ������������ ��������� ������� �� ������ (������� ���)", "������", MessageBoxButtons.OK, MessageBoxIcon.Error );
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.DefaultExt = "cdp";
            openFileDialog1.Filter = "���� ������������ ��������� �������(*.cdp)|*.cdp";
            openFileDialog1.InitialDirectory = Application.StartupPath;	// Application.StartupPath;
            if( DialogResult.OK != openFileDialog1.ShowDialog() )
            {
               throw new Exception( "�� ����� ���� ������������ ��������� ������� " );
            }
            PathToFDC = openFileDialog1.FileName;
            //��������� ����
            if( DialogResult.Yes == MessageBox.Show( "��������� ���� ������������ ��� �������� �������������", "���������", MessageBoxButtons.YesNo, MessageBoxIcon.Question ) )
            {
               // ���������� � ����� ������� - ������� ���� �� � ����� ������� ���� � ����� � ������������� ���������
               XmlTextReader rreader = new XmlTextReader( pathToF );
               XmlDocument rdoc = new XmlDocument();
               rdoc.Load( rreader );
               rreader.Close();

               // ������� ���� �� �������
               XmlNode rxn;
               XmlElement rroot = rdoc.DocumentElement;
               rxn = rroot.SelectSingleNode( "/Project/DeviceConfig/PathDeviceConfig" );
               rxn.InnerText = PathToFDC;
               rdoc.Save( pathToF );
            }
         }
         Environment.CurrentDirectory = cdir;
         // ������ ����� �������� ������ ����� � ��������� �� � ��������� �������
         XPathNodeIterator xpni;
         XPathNodeIterator xpniCh;
         XPathNavigator cd;
         XPathNavigator cdd;

         xpni = GetNodeList( "/MT/Configuration/FC", PathToFDC );	// ������ ����� � ��
         Type blockType;
         Type[] argTypes;
         ConstructorInfo ctor;
         int numCurFC;
         int numCurDev;
         string strAdrFC = String.Empty;

         while( xpni.MoveNext() )
         {
            cd = xpni.Current;
            // ��������� �������� ��
            // ������� ��� 
            blockType = theAssembly.GetType( "CRZADevices.FC");
            argTypes = new Type[] { typeof( int ), typeof(string) };
            ctor = blockType.GetConstructor( argTypes );

            numCurFC = Convert.ToInt32( cd.GetAttribute( "numFC", "" ) );
            strAdrFC = cd.GetAttribute( "fcadr", "" );
            //�������� �����������
            FC fc = ( FC ) ctor.Invoke( new object[] { numCurFC, strAdrFC} );	// FC FC1 = new FC(0);  //������� ��
            //   					 Convert.ToInt32( cd.GetAttribute( "numdev", "" ) ) 
            // ������� ��� 
            blockType = theAssembly.GetType( "CRZADevices." + cd.GetAttribute( "nameEHighLevel", "" ) );
            argTypes = new Type[] { typeof( int ), typeof( int ), typeof( int ), typeof( string ) };
            ctor = blockType.GetConstructor( argTypes );
            //FCasDev dev0_FC = new FCasDev(0,0, 0, "�� 0");
            fc.Devices.Add( ctor.Invoke( new object[] { Convert.ToInt32( cd.GetAttribute( "numdev", "" )), 0, 0, cd.GetAttribute( "describe", "" )  } ) );	
   					
            // ������ �������� �� ����������� ������� �� � ������� ��
            xpniCh = cd.Select( "FCDevices/Device" );
            while( xpniCh.MoveNext() )
            {
               cdd = xpniCh.Current;
               if( cdd.GetAttribute( "enable", "" ) == "False" )
                  continue;	// ���������� ����������

               // ��������� ����� ���������� � ������ ��� ��
               numCurDev = Convert.ToInt32( cdd.SelectSingleNode( "NumDev" ).Value );
               numCurDev += numCurFC * 256;

               // ��������� �������� ����������
               // ������� ��� 
               blockType = theAssembly.GetType( "CRZADevices." + cdd.SelectSingleNode( "nameEHighLevel" ).Value );
               argTypes = new Type[] { typeof( int ), typeof( int ), typeof( int ), typeof( int ), typeof( string ) };
               ctor = blockType.GetConstructor( argTypes );
               //�������� �����������
               fc.Devices.Add( ctor.Invoke( new object[] { numCurFC,
                                                            numCurDev,//Convert.ToInt32( cdd.SelectSingleNode( "NumDev" ).Value )
                                                            Convert.ToInt32( cdd.SelectSingleNode( "NumLock" ).Value ),
                                                            Convert.ToInt32( cdd.SelectSingleNode( "NumSec" ).Value ),
                                                            cdd.SelectSingleNode( "DescDev" ).Value } ) );
            }
            
            // ��������� �� � ������������ ������
            KB.Add(fc);				
         }
         
         // ��������� ������� ���������� - ����������� ������������ ������ �� �������� ��������� (raw) ������
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

         // ������� ����� ������ �� �� � ������ raw-������ MTD
         readFCThread = new Thread(new ThreadStart(netman.getdata));
         readFCThread.Name = "ReaderFC";
         readFCThread.Start();

         // ��������� ��� ����������
         //// ��� ���������� � online
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
            
            // ��������� ��� ���������� ����������
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

         // ��������� �� ��� ���������� � ���������� ������������ ������� ��
         foreach( FC aFc in KB )
         {
            aFc.FCNet = (FC_net) netman.IP_FCs[aFc.IpAddrFC];  // ������������� � �������� �� ������� ������
            aFc.isFCConnection = true; // ��� ������ �������������, ��� ����� ����

            foreach( TCRZADirectDevice tdd in aFc )
            {
               if( tdd.NumDev == 0 ) // �� ���� ��� ��-����������
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
      /// ���������� ����� �������
      /// </summary>
      /// <param name="ip"></param>
      public void SetIPLogger( string ip )
      {
         netman.SetIP_Logger( ip );
      }

      /// <summary>
      /// �������������� ������ ������� � ������� �� ������� ������� - ��� ����� � ��������, ����� ���������� ���,
      /// � ������������ ���������� �� ���� ����
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
                  // �������� raw-������ �� ����������
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
      /// ������� ��������� ������ ��������� ������� � ��������� ������ ReceivePacketInThread()
      /// </summary>
      public void ReceivePacket()
      {
         while(! bcw.IsBusy )
            bcw.RunWorkerAsync();
      }
      private void ReceivePacketInThread(object sender, DoWorkEventArgs e)
      {
         //--------------------------------------------------------------------
         // ��������� ����� � �� - ���� ����� ��� �� �������� ������ �� ����� ��
         //int cn =  netman.GetCountLastPacketFromFC();
         //for( int ij = 0 ;ij < netman.IP_FCs.Count ;ij++ )
         foreach( FC aFC in KB )
         {
            if( !aFC.IsFCConnectHMI() )
            {
               if( !aFC.isLostConnection )   // ������������� ��������� �������
               {
                  aFC.isLostConnection = true;	//  ����� ��������

                  // �������� ����
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
               if( aFC.isLostConnection )	// ���� ����� ���� ��������
               {
                  aFC.isLostConnection = false;
                  // ������������� ���� ����� ������� ��������
                  foreach( TCRZADirectDevice tdd in aFC )
                  {
                     tdd.crzaDeviceState = CRZADeviceState.CRZAdstOnline;

                     foreach( TCRZAGroup tdg in tdd )
                        foreach( TCRZAVariable tgv in tdg )
                           tgv.SetQuality( VarQuality.vqGood );
                  }
                  netman.ClearNetPackQ( );
               }
               m_crzaTimeFC = netman.TimeFC;		// ������������� ����� �� ��
            }
         
         //--------------------------------------------------------------------
         // ��������� �������
         m_crzaIsLoggerAlive = netman.IsLoggerAlive;	// ����� �� ������
         m_crzaLoggerTimer = netman.LoggerTimer;		// ������ �������

         // ������ ��������� ������ � KB

         if( aFC.isLostConnection )
            continue;
            
         for( int i = 0 ; i < aFC.Devices.Count ; i++ )
         {
            TCRZADirectDevice aDev = ( TCRZADirectDevice ) aFC.Devices[i];

            if( aDev.crzaDeviceState == CRZADeviceState.CRZAdstOnline )
            {
               // �������� raw-������ �� ����������
               foreach( BMRZ aBMRZ in MTD )
               if( aDev.NumDev == aBMRZ.NDev )
               {
                  aDev.CRZAMemDev = aBMRZ.memDev;
                  break;
               }
               continue;

                  // ������ ������ �������� �� ���� �� �������
                  //foreach( TCRZAGroup aGroup in aDev )
                  //   foreach( TCRZAVariable aVariable in aGroup )
                  //      aVariable.ExtractVarFrom( MTD );
               }
            }			 
        }
      }

      /// <summary>
      /// public void ResetGroup( int idFC, int idDev, int idGroup )
      /// �������� ������ (���������� ������ �������� �����) idGroup ���������� idDev, �������������� �� idFC
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
                        if( tdg.Id == 0 )   // ������ � ���������� ���� - �� �������
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
                        if( aGroup.Id == idGroup && aGroup.Id != 0 )       // ������ � ���������� ���� - �� �������
                           for( int p = 0 ; p < aGroup.Variables.Count ; p++ )
                           {
                              TCRZAVariable aVariable = ( TCRZAVariable ) aGroup.Variables[p];
                              aVariable.PlaceVarTo( MTD, VarQuality.vqUndefined );
                           }
                     }
                  }
   					 
            }
            //--------------------------------------------------------------------
            // ���������� ������ � KB
            foreach( FC aFC in KB )
            for( int i = 0 ; i < aFC.Devices.Count ; i++ )
            {
               TCRZADirectDevice aDev = ( TCRZADirectDevice ) aFC.Devices[i];

               // �������� raw-������ �� ����������
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
      /// ��������� ������� �� �������� ������ � �������� ���������� �� ������ �������
      /// </summary>
      /// <param name="iFC"></param>
      /// <param name="iDev"></param>
      /// <param name="cmd"></param>
      /// <param name="parameters"></param>
      /// <param name="aProgressBar"></param>
      /// <returns></returns>
      public bool ExecuteCommand( int iFC, int iDev, string cmd, string extraName, byte[] parameters, ToolStripProgressBar aProgressBar, StatusStrip statusStrip, Form prnt )
      {
         // ���� ����������
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

         return false;    // ������� �� ���������
      }

      /// <summary>
      /// public void PacketToQueDev(byte[] pack, int adr , int iIDDev)
      ///       �����  �� ������ adr ���������� iIDDev
      /// </summary>
      /// <param name="pack"></param>
      /// <param name="adr"></param>
      /// <param name="iIDDev"></param>
      public void PacketToQueDev( byte[] pack, ushort adr, int iIDDev )
      {
         // ����������� 
         netman.PacketToQueDev( pack, adr , iIDDev);
      }

      /// <summary>
      /// int GetCountLastPacketFromFC( int interval )
      /// ������ ����� � �� - ����� ������� �� ��������
      /// </summary>
      /// <param name="interval"></param>
      /// <returns>����� ������� �� ��������</returns>
      public int GetCountLastPacketFromFC( )
      {
         // �����������
         return netman.GetCountLastPacketFromFC();
      }
   }

	/// <summary>
	/// class FC
	/// �����, ����������� �������������� ����������
	/// </summary>
	public class FC : IEnumerable
	{
		public int NumFC;          // ����� ��������������� �����������
      public string IpAddrFC;    // ip-����� ��
		public ArrayList Devices;  // ������ ���������
      public FC_net FCNet;       // ������ �� �������� �� ������� ������
      public bool isLostConnection;  // ������� ������ ����� ( ��� ������ � �������� )
      public bool isFCConnection;  // ������� ������ ����� ��� MainForm, ����� �� ����������� ������ ���������
        // ������� �������� �� ���������� ����� ������� ��� ��������� �� ��������� ���
        public bool isCollectDataForRemoteARM = false;


      /// <summary>
		/// �����������
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
		/// ���������� ���������
		/// </summary>
		/// <returns></returns>
		public IEnumerator GetEnumerator( )
		{
			return Devices.GetEnumerator();
		}
		/// <summary>
		/// public int GetCountLastPacketFromFC( int interval )
		/// ������ �������� ����� � ��
		/// </summary>
		/// <param name="interval"></param>
		/// �������� ������ (� �������������)
		/// <returns> ���������� ������� ���������� �� �������� �� �� - ���� 0, �� ����� � �� ���</returns>
		public bool IsFCConnectHMI( )
		{
			return FCNet.isFCConnectNET();
		}
	}

   /// <summary>
   /// class CRZADevice
   /// �����, ����������� ����������
   /// </summary>
   public class CRZADevice
	{
	}

	public enum DeviceState
	{
		dstNone = 0,
		dstUndefine = 1, // ��� ����� � �����������
		//dstOffline,  // 
		//dstBlocked,  // 
		dstOnline = 2,    // � ����������� �������������� ���������� �����
	}
	#region class TCRZADirectDevice
	/// <summary>
   /// class CRZADevice
   /// �����, ����������� ���������� ����
   /// </summary>
   public class TCRZADirectDevice : CRZADevice, IEnumerable
	{
      public DeviceState StateDev;	// ��������� ����������
      public bool isRemont = false;	// ���������� � ��������� ���������?
      public int NumFC;             // ����� ��
      public int NumDev;
      public int NumSlot;				// ����� ������
      public string RefDesign;			// �������� �������� ����������, ���������� ������������ ������ ��� ����
      public ArrayList Groups;			// ������ ����������
      public ArrayList Commands;		// ������� ����������
      public SortedList CRZAMemDev;  // ������ ����������
      public string LastCommand = String.Empty;	//��������� ����������� �������
      public string LastCommandFC = String.Empty;	//��������� ����������� ������� ��
      
      public int CodeCmdFailure = 0;	// ��� ������ � ���������� �������
      public CRZADeviceState crzaDeviceState;	// ��������� ���������� �� ������� ������
        // ������� �������� �� ���������� ����� ������� ��� ��������� �� ��������� ���
        public bool isCollectDataForRemoteARM = false;


      /// <summary>
      /// ����������� 
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
      /// ���������������� ����������
      /// </summary>
      public virtual void Configure()
		{
		}

      /// <summary>
      /// public IEnumerator GetEnumerator()
      /// ���������� ��� foreach
      /// </summary>
      /// <returns></returns>
      public IEnumerator GetEnumerator()
      {
         return Groups.GetEnumerator();
      }

      /// <summary>
      /// public bool ExecuteCommand()
      /// ���������� ������
      /// </summary>
      /// <param name="netman"></param>
      /// <param name="cmd"></param>
      /// <param name="parameters"></param>
      /// <param name="aProgressBar"></param>
      /// <returns></returns>
      public bool ExecuteCommand( INetManager netman, string cmd, string extraName, byte[] parameters, ToolStripProgressBar aProgressBar, StatusStrip statusStrip, Form prnt )
      {
         //���� ������� � ���������� � �������� ��� �� ����������
         foreach( TCRZACommand aCmd in Commands )
         {
            if( aCmd.Name == cmd )
               return aCmd.Execute( netman, aCmd.Name, extraName, parameters, aProgressBar, statusStrip, prnt );
         }

         return false;     // ��������� ���������� �������
      }

      /// <summary>
      /// ��� ������������ ��������� ����������� �������
      /// </summary>
      /// <param name="Value"></param>
      /// <param name="format"></param>
      public void LinkSetText( object Value, string format )
		{
         // ������� ��� �������
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
            else if( gr.Id == 1 ) // ��� ������ FC
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
      /// callback-������� ��� ������� ��������� ��������� ���������� �� ������ ������
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
	#region �� ��� ����������
	/// <summary>
   /// class FC
   /// �����, ����������� �������������� ����������
   /// </summary>
   public class FCasDev : TCRZADirectDevice, IEnumerable
   {
      public int NumFC;           // ����� ��������������� �����������
	   public ArrayList Devices;   // ������ ���������

	   /*public ArrayList Groups;      // ������ ����������
	   public ArrayList Commands;    // ������� ����������
	   public SortedList CRZAMemDev;    // ������ ����������*/

	   /// <summary>
	   /// �����������
	   /// </summary>
	   /// <param name="numFC"> - ����� ��</param>
	   /// <param name="numSlot"> - ����� ������ - ��� �� �� ����� ��������</param>          
	   public FCasDev( int numFC, int numSlot, int numSection, string refdes )
			: base( numFC, 0, numSlot, numSection, refdes )
	   {
		   this.Devices = new ArrayList();
		   NumFC = numFC;
	   }

	   /// <summary>
	   /// public IEnumerator GetEnumerator()
	   /// ���������� ���������
	   /// </summary>
	   /// <returns></returns>
	   public IEnumerator GetEnumerator( )
	   {
		   return Devices.GetEnumerator();
      }
	   /// <summary>
	   /// public int GetCountLastPacketFromFC( int interval )
	   /// ������ �������� ����� � ��
	   /// </summary>
	   /// <param name="interval"></param>
	   /// �������� ������ (� �������������)
	   /// <returns> ���������� ������� ���������� �� �������� �� �� - ���� 0, �� ����� � �� ���</returns>
	   public int GetCountLastPacketFromFC( int interval )
	   {
		   return 1;
	   }

	   /// <summary>
	   /// public virtual void Configure()
	   /// ���������������� ���������� FC
	   /// </summary>
	   public override void Configure( )
	   {
		   // ������� ���������� ������ ��� ����������:
		   Groups = new ArrayList();  // ������� ������ ����� ����������

		   TCRZAGroup G;

		   // ������� ������ ��� �������� ������:
		   Commands = new ArrayList();

		   TCRZACommand Cmd;

		   // ������ "�������� � �������� �������, ����� ��������� � �������������� ECU"
		   G = new TCRZAGroup( this, "�������� � �������� �������, ����� ��������� � �������������� ECU", 0 );
		   // ��������� ����������
			G.Variables.Add( new TBitFieldVariable( G, 00000, "0001", "��������� ������ ������ ���������", "", "" ) ); // 0 - ���������; 1 - ���������
			G.Variables.Add( new TBitFieldVariable( G, 00000, "0002", "��������� ������ ������ ���������", "", "" ) ); // 0 - ���������; 1 - ���������

		   // ��� ���� ��������, ��������������� ��������� ����� � ���������� ��� (������������ � ������������)
			G.Variables.Add( new TBitFieldVariable( G, 00000, "0100", "��� 08", "", "" ) );
			G.Variables.Add( new TBitFieldVariable( G, 00000, "0200", "��� 09", "", "" ) );
			G.Variables.Add( new TBitFieldVariable( G, 00000, "0400", "��� 10", "", "" ) );

		   G.Variables.Add( new TUIntVariable( G, 00001, "������� ������� ������������ �������", "", "" ) );
		   G.Variables.Add( new TUDIntVariable( G, 00002, "IP-����� ��������", "", "" ) );
		   G.Variables.Add( new TUDIntVariable( G, 00004, "IP-����� �����������", "", "" ) );
		   G.Variables.Add( new TUIntVariable( G, 00006, "����� �", "", "" ) );
		   G.Variables.Add( new TUIntVariable( G, 00007, "����� �", "", "" ) );
		   G.Variables.Add( new TDateTimeVariable( G, 00008, "���� ����������� �� ����� ��", "", "" ) );
		   G.Variables.Add( new TUIntVariable( G, 00010, "���������� RTU", "", "" ) );
		   G.Variables.Add( new TStringVariable( G, 00011, "������������� ECU", "", "" ) );

		   Groups.Add( G );

		   // ������ "������ � ���������� ������ �������� ������ ���"
		   G = new TCRZAGroup( this, "������ � ���������� ������ �������� ������ ���", 1 );
		   // ��������� ����������
		   G.Variables.Add( new TUDIntVariable( G, 00050, "IP-����� �����������", "", "" ) );
		   G.Variables.Add( new TUIntVariable( G, 00052, "����� �", "", "" ) );
		   G.Variables.Add( new TUIntVariable( G, 00053, "����� �", "", "" ) );
		   G.Variables.Add( new TDateTimeVariable( G, 00054, "���� ����������� �� ����� ��", "", "" ) );
		   G.Variables.Add( new TIntVariable( G, 00056, "������� ������ �� ����������", "", "" ) );
		   G.Variables.Add( new TIntVariable( G, 00057, "��������� ���������� �������", "", "" ) );
		   G.Variables.Add( new TStringVariable( G, 00058, "��� �������", "", "" ) );
		   G.Variables.Add( new TUIntVariable( G, 00060, "������ �������� �������", "", "" ) );
		   G.Variables.Add( new TUIntVariable( G, 00061, "������ �������� �������", "", "" ) );

		   Groups.Add( G );

		   //---------------------------------------------------------------------
		   // ��������� �������
		   Cmd = new TCRZACommand( this, "NOP", "������ �������" );
		   Commands.Add( Cmd );
		   Cmd = new TCRZACommand( this, "SNC", "������ ����������� � ����������" );
		   Commands.Add( Cmd );
		   Cmd = new TCRZACommand( this, "SPC", "���������� ������ ������ ����������" );
		   Commands.Add( Cmd );
		   Cmd = new TCRZACommand( this, "SPM", "���������� ������ ������ ���������" );
		   Commands.Add( Cmd );
			Cmd = new TCRZACommand( this, "GMT", "���������� ���� ����" );
			Commands.Add( Cmd );
	   }
	   public override string ToString( )
	   {
		   return "��";
	   }
   }
	#endregion
   
	#region ���� ��-14-31-12

   #endregion

	#region class TCRZAGroup
	/// <summary>
   /// public class TCRZAGroup
   /// ����� ��� ������������� ������ ����������
   /// </summary>
   public class TCRZAGroup : IEnumerable
	{
      public string   Name;                      // ��� ������
      public int      Id;                        // ������������� Id ������
      public TCRZADirectDevice  Device;   // ���������� � ������� ������� ������
      public ArrayList Variables;                // ���������� ������

      /// <summary>
      /// ����������� 
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
      /// ���������� �������� ����� ������ ������
      /// </summary>
      public void SetQualityGroup(VarQuality vq )
      {
      foreach(TCRZAVariable var in Variables)
      var.SetQuality(vq);
      }
   }
   #endregion
   /// <summary>
	///	������� �������� ���������� 
	/// </summary>
	public enum VarQuality
	{
		vqUndefined,      // �� ���������� (�� ������������� �� ������ ������,
                        // ��� ����� ��� �������� ������������
		vqGood,           // ������� ��������
      vqArhiv,          // �������� ���������� (�� ��)
		vqRangeError,     // ����� �� ������� ���������
	};
	#region class TCRZAVariable
	/// <summary>
    /// class TCRZAVariable
    /// ����� ��� ������������� ����������
    /// </summary>
    public class TCRZAVariable
	{
        // ��������� �������
      public virtual event ChVar OnChangeVar;

      public int RegInDev;       //����� ���������� � ����������
		// ������� ����������
		public string Caption;
		// ����������� ����������
		public string Dim;
		// ��� ����������
		public string Name;
		// ������ ����� ��� �������������� � ��������. ���������
		public ArrayList ArrLiStringCB; 
		// �������� ����������
		public VarQuality Quality;
		// ������� ������� ������
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
      /// ������������� ������� ���������� �������� � ������� ������ �� �������
      /// </summary>
      /// <param name="varval">�������� ����������</param>
      /// <param name="memb">������ ���� ������������� ����������</param>
      /// <param name="lenR">����� � ���. MODBUS</param>
      /// <param name="lenB">����� � ������</param>
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
   /// ������������� ���������� ���� Float - 32 ����
   /// </summary>
   public class TFloatVariable : TCRZAVariable
   {
      // ��������� �������
      override public event ChVar OnChangeVar;

      private TCRZAGroup Group;
      public int length_R = 2; //����� � 16 ��������� ������ (��������� MODBUS)
      public int length_B = 4; //����� � ������
      // �������� ����������
      public float Value;
      private float newValue;
      // ������� ������� ������
      //bool firstRead;

      /// <summary>
      /// �����������
      /// </summary>
      /// <param name="aGroup">- ������, � ������� ����������� ����������</param>
      /// <param name="aRegInDev">- ����� � ����������, ��� ����� ������ ����������</param>
      /// <param name="aName">- ��� ����������</param>
      /// <param name="aCaption">- ������� ��� ����������</param>
      /// <param name="aDim">- ����������� ����������</param>
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
      /// ��������� ������� �� ��������� ���� �� ������� ������
      /// </summary>
      private void fChVari()
      {
         if( OnChangeVar != null )
            OnChangeVar( );
      }

      /// <summary>
      /// public override void ExtractVarFrom(ArrayList MTD)
      /// ������� ���������� �� ��������� ������, ������������� � ��������� � ���� Value
      /// </summary>
      /// <param name="MTD"></param>
      public override void ExtractVarFrom( ArrayList MTD )
      {
         if( ( Quality == VarQuality.vqUndefined ) || ( ( this.Group.Device.crzaDeviceState != CRZADeviceState.CRZAdstOnline ) && Quality != VarQuality.vqArhiv ) )
         {
            Value = 0;
            // �������� ������� ���������� link'��
            fChVari( );
            return;
         }

         // ���� ������ �������� raw-�������� � ������� ��� �� ������
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
                     // �������� ������� ���������� link'��
                     fChVari( );
                  }
               }
               break;
            }
         }
      }

      /// <summary>
      /// ������������� ������� ���������� �������� � ������� ������ �� �������
      /// </summary>
      /// <param name="varval">�������� ����������</param>
      /// <param name="memb">������ ���� ������������� ����������</param>
      /// <param name="lenR">����� � ���. MODBUS</param>
      /// <param name="lenB">����� � ������</param>
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

            // �������� ������� ���������� link'��
            fChVari( );
         }
      }

      /// <summary>
      /// public virtual void PlaceVarTo( ArrayList MTD, VarQuality vQ )
      /// ��������� ���������� � �������� ������
      /// </summary>
      /// <param name="MTD"></param>
      /// <param name="val"></param>
      public override void PlaceVarTo( ArrayList MTD, VarQuality vQ )
      {
         if( vQ == VarQuality.vqUndefined )
            Value = 0;

         // �������� ������� ���������� link'��
         fChVari( );
         // �������� ������ � �� ������ ������
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
   /// ������������� ���������� ���� Int - 16-���������� ��������� ����� �� ������
   /// </summary>
   public class TIntVariable : TCRZAVariable
   {
      // ��������� �������
      override public  event ChVar OnChangeVar;

      private TCRZAGroup Group;
      public int length_R = 1; //����� � 16 ��������� ������ (��������� MODBUS)
      public int length_B = 2; //����� � ������
      // �������� ����������
      public short Value;
      private short newValue;
      // ������� ������� ������
      //bool firstRead;

      /// <summary>
      /// �����������
      /// </summary>
      /// <param name="aGroup">- ������, � ������� ����������� ���������� </param>
      /// <param name="aRegInDev">- ����� � ����������, ��� ����� ������ ����������</param>
      /// <param name="aName">- ��� ����������</param>
      /// <param name="aCaption">- ������� ��� ����������</param>
      /// <param name="aDim">- ����������� ����������</param>
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
      /// ��������� ������� �� ��������� ����
      /// </summary>
      private void fChVari()
      {
         if (OnChangeVar != null)
         OnChangeVar();
      }

      /// <summary>
      /// public override void ExtractVarFrom(ArrayList MTD)
      /// ������� ���������� �� ��������� ������, ������������� � ��������� � ���� Value
      /// </summary>
      /// <param name="MTD"></param>
      public override void ExtractVarFrom(ArrayList MTD)
      {
         if( ( Quality == VarQuality.vqUndefined ) || ( ( this.Group.Device.crzaDeviceState != CRZADeviceState.CRZAdstOnline ) && Quality != VarQuality.vqArhiv ) )
         {
            Value = 0;
            // �������� ������� ���������� link'��
            fChVari();
            return;
         }

         // ���� ������ �������� raw-�������� � ������� ��� �� ������
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
                     // �������� ������� ���������� link'��
                     fChVari();
                  }
               }
               break;
         }
      }

      /// <summary>
      /// ������������� ������� ���������� �������� � ������� ������ �� �������
      /// </summary>
      /// <param name="varval">�������� ����������</param>
      /// <param name="memb">������ ���� ������������� ����������</param>
      /// <param name="lenR">����� � ���. MODBUS</param>
      /// <param name="lenB">����� � ������</param>
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
            // �������� ������� ���������� link'��
            fChVari( );
         }
      }

      /// <summary>
		/// public virtual void PlaceVarTo( ArrayList MTD, VarQuality vQ )
      /// ��������� ���������� � �������� ������
      /// </summary>
      /// <param name="MTD"></param>
      /// <param name="val"></param>
		public override void PlaceVarTo( ArrayList MTD, VarQuality vQ )
      {
         if( vQ == VarQuality.vqUndefined )
            Value = 0;

         // �������� ������� ���������� link'��
         fChVari();
         
         // �������� ������ � �� ������ ������
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
   /// ������������� ������� ���������� �� ���� ��� (����� ����, �� �������� ����������� ��� �� �����������)
   /// </summary>
   public class TBitFieldVariable : TCRZAVariable
   {
      // ��������� ��������
      override public event ChVar OnChangeVar;

      private TCRZAGroup Group;
      public int length_R = 0; //����� � 16 ��������� ������ (��������� MODBUS)
      public int length_B = 0; //����� � ������
      public string bitMask = null; //
      // �������� ����������
      public bool Value;
      private bool oldValue;
      // ������� ���������� �������
      private bool isReverse = false;
      // ������� ������� ������
      //bool firstRead;

      /// <summary>
      /// �����������
      /// </summary>
      /// <param name="aGroup">- ������, � ������� ����������� ����������</param>
      /// <param name="aRegInDev">- ����� � ����������, ��� ����� ������ ����������</param>
      /// <param name="abitMask">- ������� �����, �������������� ��������� ���, ������������ �������� ������� ���������� (����)-
      ///                             �������� �������, �������������� ����� ������������������ ����������������� ����(��� 0x)</param>
      /// <param name="aName">- ��� ����������</param>
      /// <param name="aCaption">- ������� ��� ����������</param>
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
      /// ��������� ������� �� ��������� ����
      /// </summary>
      private void fChVari()
      {
         if (OnChangeVar != null)
            OnChangeVar();
      }

      /// <summary>
      /// public override void ExtractVarFrom(ArrayList MTD)
      /// ������� ���������� �� ��������� ������, ������������� � ��������� � ���� Value
      /// </summary>
      /// <param name="MTD"></param>
      public override void ExtractVarFrom(ArrayList MTD)
      {
         // �������� ��� ���������
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
               
               // �������� ������� ���������� link'��
               fChVari( );
               return;
            }
         }

         char[] sTmp = new char[2];
         // �������� ��������� ��� � ������������ � ������ � ������� ��� �� ������
         
         foreach(BMRZ aBMRZ in MTD)
         {
            if (this.Group.Device.NumDev == aBMRZ.NDev)
            {
               if (aBMRZ.varDev[RegInDev] is Bit_FieldMT)
               {
                     Bit_FieldMT temp = (Bit_FieldMT)aBMRZ.varDev[RegInDev];
                     // ��������� ����� ����������
                     length_B = temp.varMT_Len;
                     length_R = temp.varMT_LenR;

                     byte[] mTmp = new byte[length_B];

                     // �������� ������� ������������� ����� � ���� ������� ����
                     for (int i = 0; i < temp.varMT_Len; i++)
                     {
                        bitMask.CopyTo(i*2, sTmp, 0, 2);
                        string ssTmp = new string(sTmp);
                        mTmp[i] = byte.Parse(ssTmp, System.Globalization.NumberStyles.HexNumber);
                     }
                     // ���������� ������� ���
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
                        
                     // ��������� ����� ����������
                     length_B = ttmp.varMT_Len;
                     length_R = ttmp.varMT_LenR;

                     byte[] mTmp = new byte[length_B];

                     // �������� ������� ������������� ����� � ���� ������� ����
                     for (int i = 0; i < ttmp.varMT_Len; i++)
                     {
                        bitMask.CopyTo(i * 2, sTmp, 0, 2);
                        string ssTmp = new string(sTmp);
                        mTmp[i] = byte.Parse(ssTmp, System.Globalization.NumberStyles.HexNumber);
                     }
                     
                     // ���������� ������� ���
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
      /// ������������� ������� ���������� �������� � ������� ������ �� �������
      /// </summary>
      /// <param name="varval">�������� ����������</param>
      /// <param name="memb">������ ���� ������������� ����������</param>
      /// <param name="lenR">����� � ���. MODBUS</param>
      /// <param name="lenB">����� � ������</param>
      public override void ExtractVarFrom( string typeVar, object varval, byte[] memb, int lenR, int lenB )
      {
         // �������� ��� ���������
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
               // �������� ������� ���������� link'��
               fChVari( );
            }
               return;
         }

         char[] sTmp = new char[ 2 ];

         if( typeVar == "Bit_FieldMT" )
         {
            // �������� � ��������� ���������� valVar
            byte[] valVar = new byte[memb.Length];
            Buffer.BlockCopy(memb, 0, valVar, 0, memb.Length);

            // ��������� ����� ����������
            length_B = lenB;
            length_R = lenR;

            byte[] mTmp = new byte[ length_B ];

            // �������� ������� ������������� ����� � ���� ������� ����
            for( int i = 0 ;i < lenB ;i++ )
            {
               bitMask.CopyTo( i * 2, sTmp, 0, 2 );
               string ssTmp = new string( sTmp );
               mTmp[ i ] = byte.Parse( ssTmp, System.Globalization.NumberStyles.HexNumber );
            }
            
            // ���������� ������� ���
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
            // �������� � ��������� ���������� valVar
            byte[] valVar = new byte[ memb.Length ];
            Buffer.BlockCopy( memb, 0, valVar, 0, memb.Length );

            // ��������� ����� ����������
            length_B = lenB;
            length_R = lenR;

            byte[] mTmp = new byte[ length_B ];

            // �������� ������� ������������� ����� � ���� ������� ����
            for( int i = 0 ;i < valVar.Length ;i++ )
            {
               bitMask.CopyTo( i * 2, sTmp, 0, 2 );
               string ssTmp = new string( sTmp );
               mTmp[ i ] = byte.Parse( ssTmp, System.Globalization.NumberStyles.HexNumber );
            }
            
            // ���������� ������� ���
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
      /// ��������� ���������� � �������� ������
      /// </summary>
      /// <param name="MTD"></param>
      /// <param name="val"></param>
		public override void PlaceVarTo( ArrayList MTD, VarQuality vQ )
      {
         byte value = 0;

         if( vQ == VarQuality.vqUndefined )
            value = 0;

         // �������� ������� ���������� link'��
         fChVari();
         
         // �������� ������ � �� ������ ������
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
   /// ������������� ���������� ���� UInt - 16-���������� ��������� ����� ��� �����
   /// </summary>
   public class TUIntVariable : TCRZAVariable
   {
      // ��������� �������
      override public event ChVar OnChangeVar;

       private TCRZAGroup Group;
       public int length_R = 1; //����� � 16 ��������� ������ (��������� MODBUS)
       public int length_B = 2; //����� � ������
       // �������� ����������
       public ushort Value;
      private ushort newValue;
       
      /// <summary>
      /// public TUIntVariable(TCRZAGroup aGroup, int aRegInDev, string aName, string aCaption, string aDim)
      /// �����������
      /// </summary>
      /// <param name="aGroup">- ������, � ������� ����������� ����������</param>
      /// <param name="aRegInDev">- ����� � ����������, ��� ����� ������ ����������</param>
      /// <param name="aName">- ��� ����������</param>
      /// <param name="aCaption">- ������� ��� ����������</param>
      /// <param name="aDim">- ����������� ����������</param>
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
      /// ��������� ������� �� ��������� ����
      /// </summary>
       private void fChVari()
      {
         if (OnChangeVar != null)
            OnChangeVar();
      }
      
      /// <summary>
      /// public override void ExtractVarFrom(ArrayList MTD)
      /// ������� ���������� �� ��������� ������, ������������� � ��������� � ���� Value
      /// </summary>
      /// <param name="MTD"></param>
       public override void ExtractVarFrom( ArrayList MTD )
      {
         if( ( Quality == VarQuality.vqUndefined ) || ( ( this.Group.Device.crzaDeviceState != CRZADeviceState.CRZAdstOnline ) && Quality != VarQuality.vqArhiv ) )
         {
            Value = 0;
            // �������� ������� ���������� link'��
            fChVari();
            return;
         }

          // ���� ������ �������� raw-�������� � ������� ��� �� ������
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
                          // �������� ������� ���������� link'��
                          fChVari();
                      }
                  }
                  break;
              }
          }
      }

      /// <summary>
      /// ������������� ������� ���������� �������� � ������� ������ �� �������
      /// </summary>
      /// <param name="varval">�������� ����������</param>
      /// <param name="memb">������ ���� ������������� ����������</param>
      /// <param name="lenR">����� � ���. MODBUS</param>
      /// <param name="lenB">����� � ������</param>
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

             // �������� ������� ���������� link'��
             fChVari( );
          }
      }

      /// <summary>
      /// public virtual void PlaceVarTo( ArrayList MTD, VarQuality vQ )
      /// ��������� ���������� � �������� ������
      /// </summary>
      /// <param name="MTD"></param>
      /// <param name="val"></param>
		public override void PlaceVarTo( ArrayList MTD, VarQuality vQ )
      {
         if( vQ == VarQuality.vqUndefined )
            Value = 0;
         
         // �������� ������� ���������� link'��
         fChVari();
         // �������� ������ � �� ������ ������
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
   /// ������������� ���������� ���� DUInt - 32-���������� ��������� ����� �� ������
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
   /// ������������� ���������� ���� UDInt - 32-���������� ��������� ����� ��� �����
   /// </summary>
   public class TUDIntVariable : TCRZAVariable
   {
      // ��������� �������
      override public event ChVar OnChangeVar;

      private TCRZAGroup Group;
      public int length_R = 2; //����� � 16 ��������� ������ (��������� MODBUS)
      public int length_B = 4; //����� � ������
      // �������� ����������
      public uint Value;
      private uint newValue;

      /// <summary>
      /// �����������
      /// </summary>
      /// <param name="aGroup">- ������, � ������� ����������� ���������� </param>
      /// <param name="aRegInDev">- ����� � ����������, ��� ����� ������ ����������</param>
      /// <param name="aName">- ��� ����������</param>
      /// <param name="aCaption">- ������� ��� ����������</param>
      /// <param name="aDim">- ����������� ����������</param>
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
      /// ��������� ������� �� ��������� ����
      /// </summary>
      private void fChVari()
      {
         if (OnChangeVar != null)   
            OnChangeVar();
      }

      /// <summary>
      /// public override void ExtractVarFrom(ArrayList MTD)
      /// ������� ���������� �� ��������� ������, ������������� � ��������� � ���� Value
      /// </summary>
      /// <param name="MTD"></param>
      public override void ExtractVarFrom( ArrayList MTD )
      {
         if( ( Quality == VarQuality.vqUndefined ) || ( ( this.Group.Device.crzaDeviceState != CRZADeviceState.CRZAdstOnline ) && Quality != VarQuality.vqArhiv ) )
         {
            Value = 0;
            // �������� ������� ���������� link'��
            fChVari();
            return;
         }

         // ���� ������ �������� raw-�������� � ������� ��� �� ������
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
                     // �������� ������� ���������� link'��
                     fChVari();
                  }
               }
               break;
            }
          }
      }
      
      /// <summary>
      /// ������������� ������� ���������� �������� � ������� ������ �� �������
      /// </summary>
      /// <param name="varval">�������� ����������</param>
      /// <param name="memb">������ ���� ������������� ����������</param>
      /// <param name="lenR">����� � ���. MODBUS</param>
      /// <param name="lenB">����� � ������</param>
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
            // �������� ������� ���������� link'��
            fChVari( );
         }
      }

      /// <summary>
      /// public virtual void PlaceVarTo( ArrayList MTD, VarQuality vQ )
      /// ��������� ���������� � �������� ������
      /// </summary>
      /// <param name="MTD"></param>
      /// <param name="val"></param>
		public override void PlaceVarTo( ArrayList MTD, VarQuality vQ )
      {
         if( vQ == VarQuality.vqUndefined )
            Value = 0;
         // �������� ������� ���������� link'��
         fChVari();
         // �������� ������ � �� ������ ������
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
   /// ������������� ���������� ���� Byte - 8-���������� ��������� �����
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
   /// ������������� ���� ���� � ���� ����������� ������� - ������ ����������������� ��������
   /// </summary>
	class TByteField2StringVariable : TCRZAVariable
   {
      // ��������� �������
      override public event ChVar OnChangeVar;

      private TCRZAGroup Group;
      public int length_R = 0; //����� � 16 ��������� ������ (��������� MODBUS)
      public int length_B = 0; //����� � ������

      //// �������� ����������
      //public string Value;
      //private byte[] memX;	// ��� �������� raw-�������� ����������
      
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
   /// ������������� ���������� ���� ������� ����, �� �������� ����� ��������� �������� ���������� ���� ��� ��� ������������ ������
   /// </summary>
   public class TByteField2UIntVariable : TCRZAVariable
   {
      // ��������� �������
      override public event ChVar OnChangeVar;

      private TCRZAGroup Group;
      public int length_R = 0; //����� � 16 ��������� ������ (��������� MODBUS)
      public int length_B = 0; //����� � ������
      public string bitMask = "";	// ��������� �������� ����� ��� ��������
		byte[] aHexBitMask;
		BitArray hexBitMask;		// hex-������� �����, �� ������� ����������� �������� �������� ����
      // �������� ����������
      public UInt32 Value;
      private UInt32 newValue;
      private byte[] memX;	// ��� �������� raw-�������� ����������
      private BitArray memXBA;	// ��� �������������� 
      private BitVector32 Value_bv32;
      
      /// <summary>
      /// public TByteField2UIntVariable( TCRZAGroup aGroup, int aRegInDev, string aName, string aCaption, string aDim, int aLengthInByte, aBitMask )
      /// ����������� - ����� �������� ���� �� ����� 4 ����
      /// </summary>
      /// <param name="aGroup">- ������, � ������� ����������� ����������</param>
      /// <param name="aRegInDev">- ����� � ����������, ��� ����� ������ ����������</param>
      /// <param name="abitMask">- ������� ����� � ���� ������� �������� ��������, ���������� ����, ������� ��������� �������� �������� ���� 
      ///									������ ����� �� ����� 32 ���</param>
      /// <param name="aName">- ��� ����������</param>
      /// <param name="aCaption">- ������� ��� ����������</param>
      /// <param name="aDim">- ����������� ����������</param>
      public TByteField2UIntVariable( TCRZAGroup aGroup, int aRegInDev, string abitMask, string aName, string aCaption)
      {
         Group = aGroup;
         RegInDev = aRegInDev;
         Caption = aCaption;
         Name = aName;
         firstRead = false;
			Quality = VarQuality.vqUndefined;

			Value = 0;
			// ������� ������ ���� aHexBitMask �� ������ abitMask
			abitMask = abitMask.Trim();
			bitMask = abitMask.Trim();
         // ������� ��������
         aHexBitMask = new byte[abitMask.Length / 2];
			length_B = aHexBitMask.Length;
			length_R = length_B / 2;

			if( ( ( abitMask.Length % 2 ) == 1 ) || ( aHexBitMask.Length > 4 ) )
				throw new Exception( "������ ��� ������������� ���������� ���� TByteField2UIntVariable" );

			// �����������
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

            // �������������� ������ ���� � ������
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
      /// ��������� ������� �� ��������� ����
      /// </summary>
      private void fChVari( )
      {
         if( OnChangeVar != null )
            OnChangeVar();
      }

      /// <summary>
      /// public override void ExtractVarFrom(ArrayList MTD)
      /// ������� ���������� �� ��������� ������, ������������� � ��������� � ���� Value
      /// </summary>
      /// <param name="MTD"></param>
      public override void ExtractVarFrom( ArrayList MTD )
      {
         if( ( Quality == VarQuality.vqUndefined ) || ( ( this.Group.Device.crzaDeviceState != CRZADeviceState.CRZAdstOnline ) && Quality != VarQuality.vqArhiv ) )
         {
            Value = 0;
            // �������� ������� ���������� link'��
            fChVari();
            return;
         }

         // ���� ������ �������� raw-�������� � ������� ��� �� ������
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
                           continue;	// ���� �����
                        else
                           goto m1;
                     }
                     return;
                  }
m1:							// ���� �� �����
                  firstRead = true;

                  // ��������� ����� �������� � ��������� uint
                  for( int i = 0 ; i < length_B ; i++ )
                     memX[i] = temp.varMT_Value[i];

                  memXBA = new BitArray( temp.varMT_Value );	// �������� ��������� ������� �� ��������� ����������
                  int j = 1;
                  for( int i = 0 ; i < memXBA.Count ; i++ )
                  {
                     if( hexBitMask[i] )	// ������������ ������ ��������� ��� �����
                     {
                        while (hexBitMask[i] )	// �������� ����� � �������������� ���� ��������� �������, ��������� ���������
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
                        // �������� ������� ���������� link'��
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
      /// ������������� ������� ���������� �������� � ������� ������ �� �������
      /// </summary>
      /// <param name="varval">�������� ����������</param>
      /// <param name="memb">������ ���� ������������� ����������</param>
      /// <param name="lenR">����� � ���. MODBUS</param>
      /// <param name="lenB">����� � ������</param>
      public override void ExtractVarFrom( string typeVar, object varval, byte[] memb, int lenR, int lenB )
      {
         if( ( Quality == VarQuality.vqUndefined ) || ( ( this.Group.Device.crzaDeviceState != CRZADeviceState.CRZAdstOnline ) && Quality != VarQuality.vqArhiv ) )
         {
            newValue = 0;

            if( ( Value != newValue ) || !firstRead )
            {
               Value = newValue;
               // �������� ������� ���������� link'��
               fChVari( );
            }
               return;
         } 

         Value_bv32 = new BitVector32( 0 );
         byte[] varVal = new byte[ lenB ];
         Buffer.BlockCopy( memb, 0, varVal, 0, lenB );
         
         // ��������� ����� �������� � ��������� uint
         for( int i = 0 ;i < length_B ;i++ )
            memX[ i ] = varVal[ i ];

         memXBA = new BitArray( varVal );	// �������� ��������� ������� �� ��������� ����������
         int j = 1;
         
         for( int i = 0 ;i < memXBA.Count ;i++ )
         {
            if( hexBitMask[ i ] )	// ������������ ������ ��������� ��� �����
            {
               while( hexBitMask[ i ] )	// �������� ����� � �������������� ���� ��������� �������, ��������� ���������
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

                  // �������� ������� ���������� link'��
                  fChVari( );
               }
            }
         }
      }

      public override void PlaceVarTo( ArrayList MTD, VarQuality vQ )
      {
         if( vQ == VarQuality.vqUndefined )
            Value = 0;

         // �������� ������� ���������� link'��
         fChVari();
         // �������� ������ � �� ������ ������
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
   /// ������������� ���������� ���� DateTime - 6 ��� 8 ���� ��� �� UTC
   /// </summary>
   public class TDateTimeVariable : TCRZAVariable
   {
      // ��������� �������
      override public  event ChVar OnChangeVar;
   
      private TCRZAGroup Group;
      public int length_R = 4; //����� � 16 ��������� ������ (��������� MODBUS)
      public int length_B = 8; //����� � ������
      // �������� ����������
      public string Value;
      public DateTime Value_raw;
      private DateTime newValue;
      //// ������� ������� ������
      //public bool firstRead;
       
      /// <summary>
      /// �����������
      /// </summary>
      /// <param name="aGroup">- ������, � ������� ����������� ���������� </param>
      /// <param name="aRegInDev">- ����� � ����������, ��� ����� ������ ����������</param>
      /// <param name="aName">- ��� ����������</param>
      /// <param name="aCaption">- ������� ��� ����������</param>
      /// <param name="aDim">- ����������� ����������</param>
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
      /// ��������� ������� �� ��������� ����
      /// </summary>
      private void fChVari()
      {
         if (OnChangeVar != null)
            OnChangeVar();
      }

      /// <summary>
      /// public override void ExtractVarFrom(ArrayList MTD)
      /// ������� ���������� �� ��������� ������, ������������� � ��������� � ���� Value
      /// </summary>
      /// <param name="MTD"></param>
      public override void ExtractVarFrom(ArrayList MTD)
      {
         if( ( Quality == VarQuality.vqUndefined ) || ( ( this.Group.Device.crzaDeviceState != CRZADeviceState.CRZAdstOnline ) && Quality != VarQuality.vqArhiv ) )
         {
            Value = DateTime.MinValue.ToString();
            // �������� ������� ���������� link'��
            fChVari();
            return;
         }

         // ���� ������ �������� raw-�������� � ������� ��� �� ������
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
                     //�������� ������� ���������� link'��
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
                     //�������� ������� ���������� link'��
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
                     //�������� ������� ���������� link'��
                     fChVari();
                  }
               }
               break;
            }
         }
      }

      /// <summary>
      /// ������������� ������� ���������� �������� � ������� ������ �� �������
      /// </summary>
      /// <param name="varval">�������� ����������</param>
      /// <param name="memb">������ ���� ������������� ����������</param>
      /// <param name="lenR">����� � ���. MODBUS</param>
      /// <param name="lenB">����� � ������</param>
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
               // �������� ������� ���������� link'��
               fChVari( ); 
            }  
               return;
         }

         // ���� ������ �������� raw-�������� � ������� ��� �� ������
         newValue = Convert.ToDateTime( varval );

         if( Value_raw != newValue || !firstRead )
         {
            firstRead = true;

            Value_raw = newValue;
            Value = Value_raw.ToString( ) + ":" + Value_raw.Millisecond.ToString();
            //�������� ������� ���������� link'��
            fChVari( );
          }          
       }

      /// <summary>
      /// public virtual void PlaceVarTo( ArrayList MTD, VarQuality vQ )
      /// ��������� ���������� � �������� ������
      /// </summary>
      /// <param name="MTD"></param>
      /// <param name="val"></param>
		public override void PlaceVarTo( ArrayList MTD, VarQuality vQ )
      {
         DateTime Value = DateTime.MinValue;
          
         if( vQ == VarQuality.vqUndefined )
            Value = DateTime.MinValue;
         
         // �������� ������� ���������� link'��
         fChVari();
         
         // �������� ������ � �� ������ ������
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
   /// ������������� ������ (� ����������� �����) - ����� �� �����������
   /// </summary>
   public class TStringVariable : TCRZAVariable
   {
      // ��������� �������
      override public  event ChVar OnChangeVar;

      private TCRZAGroup Group;
      public int length_R = 1; //����� � 16 ��������� ������ (��������� MODBUS)
      public int length_B = 2; //����� � ������
      // �������� ����������
      public string Value;
      private string newValue;
 
      /// <summary>
      /// �����������
      /// </summary>
      /// <param name="aGroup">- ������, � ������� ����������� ���������� </param>
      /// <param name="aRegInDev">- ����� � ����������, ��� ����� ������ ����������</param>
      /// <param name="aName">- ��� ����������</param>
      /// <param name="aCaption">- ������� ��� ����������</param>
      /// <param name="aDim">- ����������� ����������</param>
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
      /// ��������� ������� �� ��������� ����
      /// </summary>
      private void fChVari()
      {
         if (OnChangeVar != null)
         OnChangeVar();
      }

      /// <summary>
      /// public override void ExtractVarFrom(ArrayList MTD)
      /// ������� ���������� �� ��������� ������, ������������� � ��������� � ���� Value
      /// </summary>
      /// <param name="MTD"></param>
      public override void ExtractVarFrom(ArrayList MTD)
      {
         if( ( Quality == VarQuality.vqUndefined ) || ( ( this.Group.Device.crzaDeviceState != CRZADeviceState.CRZAdstOnline ) && Quality != VarQuality.vqArhiv ) )
         {
            Value = "";
            // �������� ������� ���������� link'��
            fChVari();
            return;
         }

         // ���� ������ �������� raw-�������� � ������� ��� �� ������
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
                     // �������� ������� ���������� link'��
                     fChVari();
                  }
               }
               break;
            }
         }
      }

      /// <summary>
      /// ������������� ������� ���������� �������� � ������� ������ �� �������
      /// </summary>
      /// <param name="varval">�������� ����������</param>
      /// <param name="memb">������ ���� ������������� ����������</param>
      /// <param name="lenR">����� � ���. MODBUS</param>
      /// <param name="lenB">����� � ������</param>
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
            // �������� ������� ���������� link'��
            fChVari( );
         }
      }

      /// <summary>
      /// public virtual void PlaceVarTo( ArrayList MTD, VarQuality vQ )
      /// ��������� ���������� � �������� ������
      /// </summary>
      /// <param name="MTD"></param>
      /// <param name="val"></param>
		public override void PlaceVarTo( ArrayList MTD, VarQuality vQ )
      {
         if( vQ == VarQuality.vqUndefined )
            Value = "";
         
         // �������� ������� ���������� link'��
         fChVari();
         // �������� ������ � �� ������ ������
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
    /// ������������� ���������� � ����������� BCD-������� (�������� ����)- ����� �� �����������
    /// (��� ������� - ��� �������������� � ������������� ��������)
    /// </summary>
   public class TBCDPackVariable : TCRZAVariable
   {
      // ��������� �������
      override public  event ChVar OnChangeVar;

      private TCRZAGroup Group;
      public int length_R = 1; //����� � 16 ��������� ������ (��������� MODBUS)
      public int length_B = 2; //����� � ������
      string MASK = String.Empty;
      // �������� ����������
      public string Value;
      private string newValue;
      private byte[] abValue;
      int  inset;
      int type;
      StringBuilder sb = new StringBuilder();
      
      /// <summary>
      /// �����������
      /// </summary>
      /// <param name="aGroup">- ������, � ������� ����������� ���������� </param>
      /// <param name="aRegInDev">- ����� � ����������, ��� ����� ������ ����������</param>
      /// <param name="aName">- ��� ����������</param>
      /// <param name="aCaption">- ������� ��� ����������</param>
      /// <param name="aDim">- ����������� ����������</param>
      /// <param name="aInset">- ����� ������� �� ������ �������</param>
      /// <param name="aType">- ��� (��������� �����) - ��. �������� �������� MTRele</param>
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
      /// ����������� 2
      /// </summary>
      /// <param name="aGroup">- ������, � ������� ����������� ���������� </param>
      /// <param name="aRegInDev">- ����� � ����������, ��� ����� ������ ����������</param>
      /// <param name="aName">- ��� ����������</param>
      /// <param name="aCaption">- ������� ��� ����������</param>
      /// <param name="aDim">- ����������� ����������</param>
      /// <param name="aInset">- ����� ������� �� ������ �������</param>
      /// <param name="aType">- ��� (��������� �����) - ��. �������� �������� MTRele</param>
      /// <param name="aMask">- ����� ��� ���������� ����� ��������</param>
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
      /// ��������� ������� �� ��������� ����
      /// </summary>
      private void fChVari()
      {
         if (OnChangeVar != null)
            OnChangeVar();
      }

      /// <summary>
      /// public override void ExtractVarFrom(ArrayList MTD)
      /// ������� ���������� �� ��������� ������, ������������� � ��������� � ���� Value
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
            // �������� ������� ���������� link'��
            fChVari();
            return;
         }

         sb.Length = 0;
         char[] sTmp = new char[2];
         // �������� raw-�������� � ������� ��� �� ������
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
                     // ���� ���� ����� �� ������� ����� �� ��� 
                     if( MASK != "" )
                     {
                        // ����������� � ������ ����
                        byte[] tmask = new byte[MASK.Length / 2];
										 
                        for( int i = 0 ; i < temp.varMT_Len ; i++ )
                        {
                           MASK.CopyTo( i * 2, sTmp, 0, 2 );
                           string ssTmp = new string( sTmp );
                           tmask[i] = byte.Parse( ssTmp, System.Globalization.NumberStyles.HexNumber );
                        }
                        
                        // ������� ����� �� �����
                        for( int i = 0 ; i < temp.varMT_Len ; i++ )
                           abValue[i] &= (byte)tmask[i];
                     }

                     // ��������� �������� � ���� ������
                     for( int i = 0; i < temp.varMT_Len; i++ )
                        sb.Append( abValue[i].ToString( "X2" ) );

                     Value = sb.ToString();
                     //�������� ����� � ����������� � �����
                     switch (type.ToString())
                     {
                        case "0":
                           // ��� �����
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
                           MessageBox.Show("��� ������� �� ��������������");
                           break;
                     }
                     // �������� ������� ���������� link'��
                     fChVari();
                  }
               }
               break;
            }
         }
      }

      /// <summary>
      /// ������������� ������� ���������� �������� � ������� ������ �� �������
      /// </summary>
      /// <param name="varval">�������� ����������</param>
      /// <param name="memb">������ ���� ������������� ����������</param>
      /// <param name="lenR">����� � ���. MODBUS</param>
      /// <param name="lenB">����� � ������</param>
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
               // �������� ������� ���������� link'��
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
            // ���� ���� ����� �� ������� ����� �� ��� 
            if( MASK != "" )
            {
               // ����������� � ������ ����
               byte[] tmask = new byte[ MASK.Length / 2 ];

               for( int i = 0 ;i < varVal.Length ;i++ )
               {
                  MASK.CopyTo( i * 2, sTmp, 0, 2 );
                  string ssTmp = new string( sTmp );
                  tmask[ i ] = byte.Parse( ssTmp, System.Globalization.NumberStyles.HexNumber );
               }
               // ������� ����� �� �����
               for( int i = 0 ;i < varVal.Length ;i++ )
                  abValue[ i ] &= ( byte ) tmask[ i ];
            }

            // ��������� �������� � ���� ������
            for( int i = 0 ;i < varVal.Length ;i++ )
               sb.Append( abValue[ i ].ToString( "X2" ) );

            newValue = sb.ToString( );
            
            //�������� ����� � ����������� � �����
            switch( type.ToString( ) )
            {
               case "0":
                  // ��� �����
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
                  MessageBox.Show( "��� ������� �� ��������������" );
                  break;
            }

            if( ( Value != newValue ) || !firstRead )
            {
               Value = newValue;
               // �������� ������� ���������� link'��
               fChVari( );
            }
         }
      }

      /// <summary>
      /// public virtual void PlaceVarTo( ArrayList MTD, VarQuality vQ )
      /// ��������� ���������� � �������� ������
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
				  
         // �������� ������� ���������� link'��
         fChVari();
         // �������� ������ � �� ������ ������
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
   /// ������������� ���������� � ����������� BCD-������� - ����� �� �����������
   /// </summary>
   public class TBCDVariable : TCRZAVariable
   {
      // ��������� �������
      override public event ChVar OnChangeVar;

      private TCRZAGroup Group;
      public int length_R = 1; //����� � 16 ��������� ������ (��������� MODBUS)
      public int length_B = 2; //����� � ������
      // �������� ����������
      public uint Value;
      public uint newValue;
   
      /// <summary>
      /// �����������
      /// </summary>
      /// <param name="aGroup">- ������, � ������� ����������� ���������� </param>
      /// <param name="aRegInDev">- ����� � ����������, ��� ����� ������ ����������</param>
      /// <param name="aName">- ��� ����������</param>
      /// <param name="aCaption">- ������� ��� ����������</param>
      /// <param name="aDim">- ����������� ����������</param>
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
      /// ��������� ������� �� ��������� ����
      /// </summary>
      private void fChVari( )
      {
         if( OnChangeVar != null )
            OnChangeVar();
      }

      /// <summary>
      /// public override void ExtractVarFrom(ArrayList MTD)
      /// ������� ���������� �� ��������� ������, ������������� � ��������� � ���� Value
      /// </summary>
      /// <param name="MTD"></param>
      public override void ExtractVarFrom( ArrayList MTD )
      {
         if( ( Quality == VarQuality.vqUndefined ) || ( ( this.Group.Device.crzaDeviceState != CRZADeviceState.CRZAdstOnline ) && Quality != VarQuality.vqArhiv ) )
         {
            Value = 0;
            // �������� ������� ���������� link'��
            fChVari();
            return;
         }

         // ���� ������ �������� raw-�������� � ������� ��� �� ������
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
                     // �������� ������� ���������� link'��
                     fChVari();
                  }
               }
               break;
            }
         }
      }

      /// <summary>
      /// ������������� ������� ���������� �������� � ������� ������ �� �������
      /// </summary>
      /// <param name="varval">�������� ����������</param>
      /// <param name="memb">������ ���� ������������� ����������</param>
      /// <param name="lenR">����� � ���. MODBUS</param>
      /// <param name="lenB">����� � ������</param>
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
            // �������� ������� ���������� link'��
            fChVari( );
         }     
      }

      /// <summary>
		/// public virtual void PlaceVarTo( ArrayList MTD, VarQuality vQ )
		/// ��������� ���������� � �������� ������
		/// </summary>
		/// <param name="MTD"></param>
		/// <param name="val"></param>
		public override void PlaceVarTo( ArrayList MTD, VarQuality vQ )
		{
         if( vQ == VarQuality.vqUndefined )
            Value = 0;
         
         // �������� ������� ���������� link'��
         fChVari();
         
         // �������� ������ � �� ������ ������
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

   #region class TByteField2CBVariable - ���� � ComboBox
   /// <summary>
   /// class TByteField2CBVariable : TCRZAVariable
   /// ������������� ���������� ���� ������� ����, �� �������� ����� ��������� �������� ���������� ����� ��� ������� ��� ComboBox
   /// </summary>
   public class TByteField2CBVariable : TCRZAVariable
   {
      // ��������� �������
      override public event ChVar OnChangeVar;

      private TCRZAGroup Group;
      public int length_R = 0; //����� � 16 ��������� ������ (��������� MODBUS)
      public int length_B = 0; //����� � ������
      public string bitMask = "";	// ��������� �������� ����� ��� ��������
      byte[] aHexBitMask;
      BitArray hexBitMask;		// hex-������� �����, �� ������� ����������� �������� �������� ����
      // �������� ����������
       public ushort Value;
      private ushort newValue;
      private byte[] memX;	// ��� �������� raw-�������� ����������
      private BitArray memXBA;	// ��� �������������� 
      private BitVector32 Value_bv32;
      
      /// <summary>
      /// public TByteField2UIntVariable( TCRZAGroup aGroup, int aRegInDev, string aName, string aCaption, string aDim, int aLengthInByte, aBitMask )
      /// ����������� - ����� �������� ���� �� ����� 4 ����
      /// </summary>
      /// <param name="aGroup">- ������, � ������� ����������� ����������</param>
      /// <param name="aRegInDev">- ����� � ����������, ��� ����� ������ ����������</param>
      /// <param name="abitMask">- ������� ����� � ���� ������� �������� ��������, ���������� ����, ������� ��������� �������� �������� ���� 
      ///									������ ����� �� ����� 32 ���</param>
      /// <param name="aName">- ��� ����������</param>
      /// <param name="aCaption">- ������� ��� ����������</param>
      /// <param name="aDim">- ����������� ����������</param>
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
         // ������� ������ ���� aHexBitMask �� ������ abitMask
         abitMask = abitMask.Trim( );
         bitMask = abitMask.Trim( );
         // ������� ��������
         aHexBitMask = new byte[ abitMask.Length / 2 ];
         length_B = aHexBitMask.Length;
         length_R = length_B / 2;

         if( ( ( abitMask.Length % 2 ) == 1 ) || ( aHexBitMask.Length > 4 ) )
            throw new Exception( "������ ��� ������������� ���������� ���� TByteField2CBVariable" );

         // �����������
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

         // �������������� ������ ���� � ������
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
      /// ��������� ������� �� ��������� ����
      /// </summary>
      private void fChVari()
      {
         if( OnChangeVar != null )
            OnChangeVar( );
      }
      /// <summary>
      /// public override void ExtractVarFrom(ArrayList MTD)
      /// ������� ���������� �� ��������� ������, ������������� � ��������� � ���� Value
      /// </summary>
      /// <param name="MTD"></param>
      public override void ExtractVarFrom( ArrayList MTD )
      {
         if( ( Quality == VarQuality.vqUndefined ) || ( ( this.Group.Device.crzaDeviceState != CRZADeviceState.CRZAdstOnline ) && Quality != VarQuality.vqArhiv ) )
         {
            Value = 0;
            // �������� ������� ���������� link'��
            fChVari( );
            return;
         }

         // ���� ������ �������� raw-�������� � ������� ��� �� ������
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
                           continue;	// ���� �����
                        else
                           goto m1;
                     }
                     return;
                  }
               m1:							// ���� �� �����
                  firstRead = true;

                  // ��������� ����� �������� � ��������� uint
                  for( int i = 0 ;i < length_B ;i++ )
                     memX[ i ] = temp.varMT_Value[ i ];

                  memXBA = new BitArray( temp.varMT_Value );	// �������� ��������� ������� �� ��������� ����������
                  int j = 1;
                  for( int i = 0 ;i < memXBA.Count ;i++ )
                  {
                     if( hexBitMask[ i ] )	// ������������ ������ ��������� ��� �����
                     {
                        while( hexBitMask[ i ] )	// �������� ����� � �������������� ���� ��������� �������, ��������� ���������
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
                           // �������� ������� ���������� link'��
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
      /// ������������� ������� ���������� �������� � ������� ������ �� �������
      /// </summary>
      /// <param name="varval">�������� ����������</param>
      /// <param name="memb">������ ���� ������������� ����������</param>
      /// <param name="lenR">����� � ���. MODBUS</param>
      /// <param name="lenB">����� � ������</param>
      public override void ExtractVarFrom( string typeVar, object varval, byte[] memb, int lenR, int lenB )
      {
         if( ( Quality == VarQuality.vqUndefined ) || ( ( this.Group.Device.crzaDeviceState != CRZADeviceState.CRZAdstOnline ) && Quality != VarQuality.vqArhiv ) )
         {
            newValue = 0;

            if( ( Value != newValue ) || !firstRead )
            {
               firstRead = true;

               Value = newValue;
               // �������� ������� ���������� link'��
               fChVari( );
            }
            return;
         }

         Value_bv32 = new BitVector32( 0 );
         byte[] varVal = new byte[ lenB ];
         Buffer.BlockCopy( memb, 0, varVal, 0, lenB );

         // ��������� ����� �������� � ��������� uint
         for( int i = 0 ;i < length_B ;i++ )
            memX[ i ] = varVal[ i ];

         memXBA = new BitArray( varVal );	// �������� ��������� ������� �� ��������� ����������
         int j = 1;
         for( int i = 0 ;i < memXBA.Count ;i++ )
         {
            if( hexBitMask[ i ] )	// ������������ ������ ��������� ��� �����
            {
               while( hexBitMask[ i ] )	// �������� ����� � �������������� ���� ��������� �������, ��������� ���������
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
                  // �������� ������� ���������� link'��
                  fChVari( );
               }
            }
         }
      }

      public override void PlaceVarTo( ArrayList MTD, VarQuality vQ )
      {
         if( vQ == VarQuality.vqUndefined )
            Value = 0;

         // �������� ������� ���������� link'��
         fChVari( );
         // �������� ������ � �� ������ ������
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

   #region class TUIntCBVariable - 16-��������� UInt, ��������������� �� ������� ��� ����������� � ComboBox
   /// <summary>
   /// class TUIntCBVariable : TCRZAVariable
   /// ������������� ���������� ���� UInt - 16-���������� ��������� ����� ��� �����,
   /// ���������������� �� ������� ��� ����������� � ComboBox
   /// </summary>
   public class TUIntCBVariable : TCRZAVariable
   {
      // ��������� �������
      override public event ChVar OnChangeVar;

      private TCRZAGroup Group;

      public int length_R = 1; //����� � 16 ��������� ������ (��������� MODBUS)
      public int length_B = 2; //����� � ������

      // �������� ����������
      public ushort Value;
      private ushort newValue;
      
      /// <summary>
      /// �����������
      /// </summary>
      /// <param name="aGroup">- ������, � ������� ����������� ����������</param>
      /// <param name="aRegInDev">- ����� � ����������, ��� ����� ������ ����������</param>
      /// <param name="aName">- ��� ����������</param>
      /// <param name="aCaption">- ������� ��� ����������</param>
      /// <param name="arrLiString">- ������ � combobox</param>
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
      /// ��������� ������� �� ��������� ����
      /// </summary>
      private void fChVari()
      {
         if( OnChangeVar != null )
            OnChangeVar( );
      }

      /// <summary>
      /// public override void ExtractVarFrom(ArrayList MTD)
      /// ������� ���������� �� ��������� ������, ������������� � ��������� � ���� Value
      /// </summary>
      /// <param name="MTD"></param>
      public override void ExtractVarFrom( ArrayList MTD )
      {
         if( ( Quality == VarQuality.vqUndefined ) || ( ( this.Group.Device.crzaDeviceState != CRZADeviceState.CRZAdstOnline ) && Quality != VarQuality.vqArhiv ) )
         {
            Value = 0;
            // �������� ������� ���������� link'��
            fChVari( );
            return;
         }

         // ���� ������ �������� raw-�������� � ������� ��� �� ������
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
                     // �������� ������� ���������� link'��
                     fChVari( );
                  }
               }
               break;
            }
         }
      }

      /// <summary>
      /// ������������� ������� ���������� �������� � ������� ������ �� �������
      /// </summary>
      /// <param name="varval">�������� ����������</param>
      /// <param name="memb">������ ���� ������������� ����������</param>
      /// <param name="lenR">����� � ���. MODBUS</param>
      /// <param name="lenB">����� � ������</param>
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
            // �������� ������� ���������� link'��
            fChVari( );
         }
     }

      /// <summary>
      /// public virtual void PlaceVarTo( ArrayList MTD, VarQuality vQ )
      /// ��������� ���������� � �������� ������
      /// </summary>
      /// <param name="MTD"></param>
      /// <param name="val"></param>
      public override void PlaceVarTo( ArrayList MTD, VarQuality vQ )
      {
         if( vQ == VarQuality.vqUndefined )
            Value = 0;

         // �������� ������� ���������� link'��
         fChVari( );
         // �������� ������ � �� ������ ������
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
   /// ������������� �������
   /// </summary>
   public class TCRZACommand
   {
      public string Caption;   // ������� �������
      public string Name;      // ��� �������
      public bool StatusActive = false; // ��������� �������� ��������: true - ������� ���� ������� ����������
      private bool StatusCommand = false;       // ��������� ������� - ����������� (true) ��� ��� (false)
      private bool lastStatusCommand = false;   // ���������� ��������� �������
      private string bitNum;  // ����� ���� � �������� �� ������ 60000, ������� ������������ �������
      public string extraName;
      ToolStripProgressBar toolStripProgressBar1;
      StatusStrip statusStrip1;
      Form parentForm;
      private bool errorMesVis = false;   // ��� ���������� ������������ ������ ��������� �� ������

      private TCRZADirectDevice dev;
      //-----------------------------------------------------------
      // ��� �������� ������� ���������� �������
      [System.Runtime.InteropServices.DllImport( "kernel32.dll" )]
         extern static short QueryPerformanceCounter( ref long x );
      [System.Runtime.InteropServices.DllImport( "kernel32.dll" )]
         extern static short QueryPerformanceFrequency( ref long x );

      long ctr1 = 0, ctr2 = 0, freq = 0;/**/
      double interval2 = 0;
      System.Threading.Timer tm;
      //-----------------------------------------------------------

      /// <summary>
      /// �����������
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
      /// ������ ������� �� ���������� 
      /// </summary>
      /// <param name="netman"></param>
      /// <param name="cmd"></param>
      /// <param name="dopName">�������������� ��� ��� ������ ���� API</param>
      /// <param name="parameters"></param>
      /// <param name="aProgressBar"></param>
      /// <returns></returns> 
      public bool Execute(INetManager netman, string cmd, string dopName, byte[] parameters, ToolStripProgressBar aProgressBar, StatusStrip statusStrip, Form prnt  )
      {
         errorMesVis = false;

         byte[] scmd1;
         byte[] scmd2;
         bool success = false; // ��������� ���������� �������

         if( dopName != String.Empty )
            this.extraName = dopName;  // �������������� ��� ��� ������ ���� API

         toolStripProgressBar1 = aProgressBar;
         statusStrip1 = statusStrip;
         parentForm = prnt;
 
         int cmd1 = dev.NumDev;

         StringBuilder cmd2 = new StringBuilder();

         System.Diagnostics.Trace.TraceInformation( "������ ������� << " + cmd + " >> ����������: {0} � {1}", dev.ToString(), dev.NumDev);

         switch( cmd )
         {
            case "NOP":    // ������ �������
               // ��������� ������� ������ ������
               // ����� ����������
               scmd1 = BitConverter.GetBytes( cmd1 );
               // �������
               cmd2.Append( "NOP\0" );
               scmd2 = Encoding.UTF8.GetBytes( cmd2.ToString() );
               
               if( netman.senddata( scmd1[0], scmd2, parameters ) )
               {
                  StatusActive = true; // ������� �������, ��������� ������� �������� ����������
                  bitNum = "0000";	// ��� ���������������� ����
                  success = true;
               }

               break;
            case "RBH":    // ��������� ������ ��
               // ��������� ������� ������ ������
               // ����� ����������
               scmd1 = BitConverter.GetBytes( cmd1 );
               // �������
               cmd2.Append( "RBH\0" );
               scmd2 = Encoding.UTF8.GetBytes( cmd2.ToString() );
               
               if( netman.senddata( scmd1[0], scmd2, parameters ) )
               {
                  StatusActive = true; // ������� �������, ��������� ������� �������� ����������
                  bitNum = "1000";
                  success = true; 
               }

               break;
               case "IMB":    // �������� ��������� � ������� ��
                  // ��������� ������� ������ ������
                  // ����� ����������
                  scmd1 = BitConverter.GetBytes( cmd1 );
                  // �������
                  cmd2.Append( "IMB\0" );
                  scmd2 = Encoding.UTF8.GetBytes( cmd2.ToString() );

                  if( netman.senddata( scmd1[0], scmd2, parameters ) )
                  {
                     StatusActive = true; // ������� �������, ��������� ������� �������� ����������
                     bitNum = "0001";
                     success = true; 
                  }
                  break;
                  case "OCB":    // ����������� ����������
                     // ��������� ������� ������ ������
                     // ����� ����������
                     scmd1 = BitConverter.GetBytes( cmd1 );
                     // �������
                     cmd2.Append( "OCB\0" );
                     scmd2 = Encoding.UTF8.GetBytes( cmd2.ToString() );
                     
                     if( netman.senddata( scmd1[0], scmd2, parameters ) )
                     {
                         StatusActive = true; // ������� �������, ��������� ������� �������� ����������
                         bitNum = "0001";
								 success = true; 
                     }
                     break;
                 case "CCB":    // ����������� ��������
                     // ��������� ������� ������ ������
                     // ����� ����������
                     scmd1 = BitConverter.GetBytes( cmd1 );
                     // �������
                     cmd2.Append( "CCB\0" );
                     scmd2 = Encoding.UTF8.GetBytes( cmd2.ToString() );

                     if( netman.senddata( scmd1[0], scmd2, parameters ) )
                     {
                         StatusActive = true; // ������� �������, ��������� ������� �������� ����������
                         bitNum = "0002";
								 success = true; 
                     }
                     break;
                 case "CCD":    // �������� ����������
                     // ��������� ������� ������ ������
                     // ����� ����������
                     scmd1 = BitConverter.GetBytes( cmd1 );
                     // �������
                     cmd2.Append( "CCD\0" );
                     scmd2 = Encoding.UTF8.GetBytes( cmd2.ToString() );
                     
                    if( netman.senddata( scmd1[0], scmd2, parameters ) )
                     {
                         StatusActive = true; // ������� �������, ��������� ������� �������� ����������
                         bitNum = "0004";
								 success = true; 
                     }
                     break;
                 case "ECC":    // �����������
                     // ��������� ������� ������ ������
                     // ����� ����������
                     scmd1 = BitConverter.GetBytes( cmd1 );
                     // �������
                     cmd2.Append( "ECC\0" );
                     scmd2 = Encoding.UTF8.GetBytes( cmd2.ToString() );
                     
                    if( netman.senddata( scmd1[0], scmd2, parameters ) )
                     {
                         StatusActive = true; // ������� �������, ��������� ������� �������� ����������
                         bitNum = "0008";
								 success = true; 
                     }
                     break;
						case "IME":    // �������� � ��������� ������ ������ ������ � �������
							// ��������� ������� ������ ������
							// ����� ����������
							scmd1 = BitConverter.GetBytes( cmd1 );
							// �������
							cmd2.Append( "IME\0" );
							scmd2 = Encoding.UTF8.GetBytes( cmd2.ToString() );
							
                     if( netman.senddata( scmd1[0], scmd2, parameters ) )
							{
								StatusActive = true; // ������� �������, ��������� ������� �������� ����������
								bitNum = "0000";
								success = true;
							}
							break;
                 case "CMD":    // �������� �������� 
                     // ��������� ������� ������ ������
                     // ����� ����������
                     scmd1 = BitConverter.GetBytes( cmd1 );
                     // �������
                     cmd2.Append( "CMD\0" );
                     scmd2 = Encoding.UTF8.GetBytes( cmd2.ToString() );
                     
                    if( netman.senddata( scmd1[0], scmd2, parameters ) )
                     {
                         StatusActive = true; // ������� �������, ��������� ������� �������� ����������
                         bitNum = "0010";
								 success = true; 
                     }
                     break;
                 case "RCD":    // ��������� ����������
                     // ��������� ������� ������ ������
                     // ����� ����������
                     scmd1 = BitConverter.GetBytes( cmd1 );
                     // �������
                     cmd2.Append( "RCD\0" );
                     scmd2 = Encoding.UTF8.GetBytes( cmd2.ToString() );
                     
                    if( netman.senddata( scmd1[0], scmd2, parameters ) )
                     {
                         StatusActive = true; // ������� �������, ��������� ������� �������� ����������
                         bitNum = "0020";
								 success = true; 
                     }
                     break;
                 case "RCP":    // ��������� �������
                     // ��������� ������� ������ ������
                     // ����� ����������
                     scmd1 = BitConverter.GetBytes( cmd1 );
                     // �������
                     cmd2.Append( "RCP\0" );
                     scmd2 = Encoding.UTF8.GetBytes( cmd2.ToString() );
                     
                    if( netman.senddata( scmd1[0], scmd2, parameters ) )
                     {
                         StatusActive = true; // ������� �������, ��������� ������� �������� ����������
                         bitNum = "0040";
								 success = true; 
                     }
                     break;
                 case "WCP":    // �������� �������
                     // ��������� ������� ������ ������
                     // ����� ����������
                     scmd1 = BitConverter.GetBytes( cmd1 );
                     // �������
                     cmd2.Append( "WCP\0" );
                     scmd2 = Encoding.UTF8.GetBytes( cmd2.ToString() );
                     if( netman.senddata( scmd1[0], scmd2, parameters ) )
                     {
                         StatusActive = true; // ������� �������, ��������� ������� �������� ����������
                         bitNum = "0080";
								 success = true; 
                     }
                     break;
						case "CLS":    // �������� ���� ������ (���)
							// ��������� ������� ������ ������
							// ����� ����������
							scmd1 = BitConverter.GetBytes( cmd1 );
							// �������
							cmd2.Append( "CLS\0" );
							scmd2 = Encoding.UTF8.GetBytes( cmd2.ToString() );
							
                     if( netman.senddata( scmd1[0], scmd2, parameters ) )
							{
								StatusActive = true; // ������� �������, ��������� ������� �������� ����������
								bitNum = "0002";
								success = true;
							}
							break;
						case "OPN":    // ���������� ���� ������ (���)
							// ��������� ������� ������ ������
							// ����� ����������
							scmd1 = BitConverter.GetBytes( cmd1 );
							// �������
							cmd2.Append( "OPN\0" );
							scmd2 = Encoding.UTF8.GetBytes( cmd2.ToString() );
							
                     if( netman.senddata( scmd1[0], scmd2, parameters ) )
							{
								StatusActive = true; // ������� �������, ��������� ������� �������� ����������
								bitNum = "0001";
								success = true;
							}
							break;
						case "API":    // ������� �������
							// ��������� ������� ������ ������
							// ����� ����������
							scmd1 = BitConverter.GetBytes( cmd1 );
							// �������
							cmd2.Append( "API\0" );
							scmd2 = Encoding.UTF8.GetBytes( cmd2.ToString() );
							
                     if( netman.senddata( scmd1[0], scmd2, parameters ) )
							{
								StatusActive = true; // ������� �������, ��������� ������� �������� ����������
								bitNum = "4000";
								success = true;
							}
							break;
						case "WBH":    // �������� ������
							// ��������� ������� ������ ������
							// ����� ����������
							scmd1 = BitConverter.GetBytes( cmd1 );
							// �������
							cmd2.Append( "WBH\0" );
							scmd2 = Encoding.UTF8.GetBytes( cmd2.ToString() );
							
                     if( netman.senddata( scmd1[0], scmd2, parameters ) )
							{
								StatusActive = true; // ������� �������, ��������� ������� �������� ����������
								bitNum = "2000";
								success = true; 
							}
							break;
                 case "RMD":    // ��������� �������� �� ������ �����
                     // ��������� ������� ������ ������
                     // ����� ����������
                     scmd1 = BitConverter.GetBytes( cmd1 );
                     // �������
                     cmd2.Append( "RMD\0" );
                     scmd2 = Encoding.UTF8.GetBytes( cmd2.ToString() );
                     
                    if( netman.senddata( scmd1[0], scmd2, parameters ) )
                     {
                         StatusActive = true; // ������� �������, ��������� ������� �������� ����������
                         bitNum = "0100";
								 success = true; 
                     }
                     break;
                 case "IMC":    // ��������� ����������
                     // ��������� ������� ������ ������
                     // ����� ����������
                     scmd1 = BitConverter.GetBytes( cmd1 );
                     // �������
                     cmd2.Append( "IMC\0" );
                     scmd2 = Encoding.UTF8.GetBytes( cmd2.ToString() );
                     
                    if( netman.senddata( scmd1[0], scmd2, parameters ) )
                     {
                         // IMC ���� ��� � ������
                         //StatusActive = true; // ������� �������, ��������� ������� �������� ����������
                         //bitNum = 0;
								success = true; 
                     }
                     break;
                 case "SPC":    // ���������� ������ ������ ������ ����������
                     // ��������� ������� ������ ������
                     // ����� ����������
					 //scmd1 = BitConverter.GetBytes( cmd1 );
					 scmd1 = BitConverter.GetBytes( 0 );	// ������� ��, ����� ����������, �.�. �� ����

                     // �������
                     cmd2.Append( "SPC\0" );
                     scmd2 = Encoding.UTF8.GetBytes( cmd2.ToString() );
                     
                    if( netman.senddata( scmd1[0], scmd2, parameters ) )
                     {
                         // SPC ���� ��� � ������
                         //StatusActive = true; // ������� �������, ��������� ������� �������� ����������
                         //bitNum = 0;
								success = true; 
                     }
                     break;
						case "GMT":    // ���������� �����
							// ��������� ������� ������ ������
							// ����� ����������
							//scmd1 = BitConverter.GetBytes( cmd1 );
							scmd1 = BitConverter.GetBytes( 0 );	// ������� ��, ����� ����������, �.�. �� ���� ����

							// �������
							cmd2.Append( "GMT\0" );
							scmd2 = Encoding.UTF8.GetBytes( cmd2.ToString() );
							
                     if( netman.senddata( scmd1[0], scmd2, parameters ) )
							{
								// SPC ���� ��� � ������
								//StatusActive = true; // ������� �������, ��������� ������� �������� ����������
								//bitNum = 0;
								success = true; 
							}
							break;
                 case "IMD":    // ��������� �������� �� ������ ��
                     // ��������� ������� ������ ������
                     // ����� ����������
                     scmd1 = BitConverter.GetBytes( cmd1 );
                     // �������
                     cmd2.Append( "IMD\0" );
                     scmd2 = Encoding.UTF8.GetBytes( cmd2.ToString() );
                    
                    if( netman.senddata( scmd1[0], scmd2, parameters ) )
                     {
                         // IMD ���� ��� � ������
                         //StatusActive = true; // ������� �������, ��������� ������� �������� ����������
                         //bitNum = 0;
								success = true; 
                     }
                     break;
                 case "SPM":    // ���������� ������ ������ ������ ���������
                     // ��������� ������� ������ ������
                     // ����� ����������
                     //scmd1 = BitConverter.GetBytes( cmd1 );
					 scmd1 = BitConverter.GetBytes( 0 );	// ������� ��, ����� ����������, �.�. �� ����

					// �������
                     cmd2.Append( "SPM\0" );
                     scmd2 = Encoding.UTF8.GetBytes( cmd2.ToString() );
                     
                    if( netman.senddata( scmd1[0], scmd2, parameters ) )
                     {
                         // CPM ���� ��� � ������
                         //StatusActive = true; // ������� �������, ��������� ������� �������� ����������
                         //bitNum = 0;
								success = true; 
                     }
                     break;
                 case "IMP":    // ��������� �������
                     // ��������� ������� ������ ������
                     // ����� ����������
                     scmd1 = BitConverter.GetBytes( cmd1 );
                     // �������
                     cmd2.Append( "IMP\0" );
                     scmd2 = Encoding.UTF8.GetBytes( cmd2.ToString() );
                     
                    if( netman.senddata( scmd1[0], scmd2, parameters ) )
                     {
                         // IMP ���� ��� � ������
                         //StatusActive = true; // ������� �������, ��������� ������� �������� ����������
                         //bitNum = 0;
								success = true; 
                     }
                     break;
                 default:
                     System.Diagnostics.Trace.TraceInformation( "������� ���������� �������������� �������" );
							success = false; 
                     break;
             }
				 if( success )
				 {
					 QueryPerformanceCounter( ref ctr1 );	// �������� �����
					 // ��������� ������ ������������ ���������� �������
					 // ������, ����������� ��� ������������ ���������� �������
					 tm = new System.Threading.Timer( new TimerCallback( TmTick ), null, 0, 500 );
					 //toolStripProgressBar1.Value = 25;
					 LinkSetTextTSB( 25 );
				 }

            return success;   // �������� ������ �������
         }
         /// <summary>
         /// public int IsStatusCommand()
         ///    ��������� ���������� �������
         /// </summary>
         /// <returns> 0 - ��� �������� (StatusActive = false, StatusCommand = false)
         ///           1 - ������� ������� (StatusActive = true), �� ���������� ��� �� ������� ������� � ���������� (StatusCommand = false)
         ///           2 - ������� ������� (StatusActive = true), ���������� ������� ������� � ���������� (StatusCommand = true)
         ///           3 - ������� ������� (StatusActive = true), ���������� ��������� ������� (StatusCommand = false ����� ��������� StatusCommand = true)</returns>
		 public int IsStatusCommand( )
          {
              // �������� ��������� ��� � �������� ������� ������ RTU
              for( int i = 0; i < this.dev.Groups.Count; i++ )
              {
                  TCRZAGroup tcg = ( TCRZAGroup ) this.dev.Groups[i];
                  if( tcg.Name == "��������� ���������� ������ ����" )
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
                                    MessageBox.Show( parentForm, "������� << " + this.Caption + " >> ���������\n ����� ����������: (" + interval2.ToString( "F3" ) + " ���)", "���������� �������", MessageBoxButtons.OK, MessageBoxIcon.Information );
                                 else
                                    MessageBox.Show( parentForm, "������� << " + this.extraName + " >> ���������\n ����� ����������: (" + interval2.ToString( "F3" ) + " ���)", "���������� �������", MessageBoxButtons.OK, MessageBoxIcon.Information );

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
                                 // �������� ������� ���������� ������� �� ������ ��������� - �� ����������
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
                                       MessageBox.Show( parentForm, "������� << " + this.Caption + " >> ���������\n ����� ����������: (" + interval2.ToString( "F3" ) + " ���)", "���������� �������", MessageBoxButtons.OK, MessageBoxIcon.Information );
                                    else
                                       MessageBox.Show( parentForm, "������� << " + this.extraName + " >> ���������\n ����� ����������: (" + interval2.ToString( "F3" ) + " ���)", "���������� �������", MessageBoxButtons.OK, MessageBoxIcon.Information );

                                    LinkSetTextTSB( 0 );	// toolStripProgressBar1
                                    return 3;	// ���������� ��������� �������
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
                                       errorMesVis = true;   // ��������� �� ������ ������

                                       if( this.extraName == String.Empty )
                                          MessageBox.Show( parentForm, "������� << " + this.Caption + " >> ��������� � �������\n ����� ����������: (" + interval2.ToString( "F3" ) + " ���)\n ��� ������: " + ( 10 + dev.CodeCmdFailure ).ToString( ), "���������� �������", MessageBoxButtons.OK, MessageBoxIcon.Error );
                                       else
                                          MessageBox.Show( parentForm, "������� << " + this.extraName + " >> ��������� � �������\n ����� ����������: (" + interval2.ToString( "F3" ) + " ���)\n ��� ������: " + ( 10 + dev.CodeCmdFailure ).ToString( ), "���������� �������", MessageBoxButtons.OK, MessageBoxIcon.Error );
                                    }

                                    tm.Dispose( );
                                    LinkSetTextTSB( 0 );	// toolStripProgressBar1
                                    return 10 + dev.CodeCmdFailure;	// ��������� � �������
                                 }
                                 LinkSetTextTSB( 25 );
                                 return 1;
                              }
                           }
                           else if( bitNum == null )   // ��� ������ ���� IMC � IMD
                           {
                              // �������� ������� ���������� ������� �� ������ ��������� - �� ����������
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
                                    MessageBox.Show( parentForm, "������� << " + this.Caption + " >> ���������\n ����� ����������: (" + interval2.ToString( "F3" ) + " ���)", "���������� �������", MessageBoxButtons.OK, MessageBoxIcon.Information );
                                 else
                                    MessageBox.Show( parentForm, "������� << " + this.extraName + " >> ���������\n ����� ����������: (" + interval2.ToString( "F3" ) + " ���)", "���������� �������", MessageBoxButtons.OK, MessageBoxIcon.Information );

                                 LinkSetTextTSB( 0 );	// toolStripProgressBar1
                                 return 3;	// ���������� ��������� �������
                              }
                           }
                        }
                     }
                     else
                        continue;
              }
              return 0;
          }
			 #region ��� ����������������� ������ ��������� (��������� ������)
			 /*==========================================================================*
			*   private void void LinkSetText(object Value)
			*      ��� ����������������� ������ ���������
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
			 * //��� ����������������� ������ ���������
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