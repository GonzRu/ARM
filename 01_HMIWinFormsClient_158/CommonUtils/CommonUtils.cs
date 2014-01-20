#define TRACE
/*#############################################################################
 *    Copyright (C) 2007 Mehanotronika Corporation.                            
 *    All rights reserved.                                                     
 *	~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
 *                                                                             
 *	��������: ����������� �����, ���������� ��������� �������� ������ 
 *					��� ������������� � �������� ��������.                            
 *	����                     : CommomUtils.cs                                         
 *	��� ��������� �����      : *.dll
 *	������ �� ��� ���������� : �#, Framework 2.0                                
 *	�����������              : ���� �.�.                                        
 *	���� ������ ����������   : 10.10.2007                                       
 *	���� (v1.0)              :                                                  
 *******************************************************************************
 * ���������:
 * 1. ����(�����): ...c���������...
 *#############################################################################*/

using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.IO;
using System.Xml.Linq;

using DebugStatisticLibrary;
using FileManager;
using HelperLibrary;
using LibraryElements;
using PTKStateLib;
using WindowsForms;
using Structure;
using Calculator;
using System.Data.SqlClient;
using System.Data;
using System.Diagnostics;
using System.Net;
using HMI_MT_Settings;
using InterfaceLibrary;

namespace CommonUtils
{

    /// <summary>
    /// public static class CommonUtils	/// 
    /// </summary>
    public static class CommonUtils
    {
        public enum UserActionType
        {
            b00_Control_Switch,
            b01_Control_ABP,
            b02_ACK_Signaling,
            b03_Administrate_Users,
            b04_Set_Time,
            b05_Set_ustav_config,
            b06_Reset_info,
            b07_RPN
        }

       /// <summary>
        /// ������� ����� � ������ �������
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string GetTimeInMTRACustomFormat(DateTime value)
        {
            return value.ToString("dd.MM.yyyy HH:mm:ss.fff");
        }
        /// <summary>
        /// ���������� �������� ���������� �� ����� PrgDevCFG.cdp
        /// </summary>
        /// <returns></returns>
        public static XElement GetXElementFrom_PrgDevCFG( int devguid )
        {
            XDocument xdoc = new XDocument();
            XElement xeDescDev = null;
            try
            {
                // ��������� �������� �� PrgDevCFG.cdp
                xeDescDev = HMI_MT_Settings.HMI_Settings.CONFIGURATION.GetDeviceXMLDescription( 0, "MOA_ECU", devguid );

            }
            catch ( Exception ex )
            {
                TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG( ex );
            }

            return xeDescDev; //GetXElementFrom_PrgDevCFG(numFC, numDev);

            #region ������ ���
            //xdoc = XDocument.Load(AppDomain.CurrentDomain.BaseDirectory + Path.DirectorySeparatorChar + "Project" + Path.DirectorySeparatorChar + "PrgDevCFG.cdp");
            //IEnumerable<XElement> iefcs = xdoc.Element("MT").Element("Configuration").Elements("FC");
            //foreach (XElement xefc in iefcs)
            //{
            //    if (xefc.Attribute("numFC").Value == numFC.ToString())
            //    {
            //        IEnumerable<XElement> devs = xefc.Element("FCDevices").Elements("Device");

            //        foreach (XElement xed in devs)
            //        {
            //            if (numDev == int.Parse(xed.Element("NumDev").Value))
            //                return xed;
            //        }
            //    }
            //}

            //iefcs = xdoc.Element("MT").Element("Configuration").Elements("PS");
            //foreach (XElement xefc in iefcs)
            //{
            //    if (xefc.Attribute("numFC").Value == numFC.ToString())
            //    {
            //        IEnumerable<XElement> devs = xefc.Element("PSDevices").Elements("Device");

            //        foreach (XElement xed in devs)
            //        {
            //            if (numDev == int.Parse(xed.Element("NumDev").Value))
            //                return xed;
            //        }
            //    }
            //}
            //return null; 
            #endregion
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="devguid"></param>
        /// <returns></returns>
        public static string GetDispCaptionForDevice( int devguid )
        {
            string rez = string.Empty;

            try
            {
                XElement xe4dev = GetXElementFrom_PrgDevCFG( devguid );

                if ( xe4dev == null )
                    throw new Exception( string.Format( "(221) : CommonUtils.cs : GetDispCaptionForDevice() : �������������� ���������� = {0}", devguid.ToString() ) );

                rez = xe4dev.Element( "DescDev" ).Element( "DescDev" ).Value;
            }
            catch ( Exception ex )
            {
                TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG( ex );
            }

            return rez;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="devguid"></param>
        /// <returns></returns>
        public static string GetDispNameForDevice( int devguid )
        {
            string rez = string.Empty;

            try
            {
                XElement xe4dev = GetXElementFrom_PrgDevCFG( devguid );

                if ( xe4dev == null )
                    throw new Exception( string.Format( "(221) : CommonUtils.cs : GetDispNameForDevice() : �������������� ���������� = {0}", devguid.ToString() ) );

                rez = xe4dev.Element( "DescDev" ).Element( "nameR" ).Value;
            }
            catch ( Exception ex )
            {
                TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG( ex );
            }

            return rez;
        }
        /// <summary>
        /// ������� ������ ������������ ���������� � ������������ 
        /// (��� ���� ����� �������� ������ ��������� ���� ��� ������� ����������� � ����)
        /// </summary>
        private static bool GetEnableStatusDev( int uniDevGuid )
        {
            XElement xe_dev;

            try
            {
                xe_dev = HMI_MT_Settings.HMI_Settings.CONFIGURATION.GetDeviceXMLDescription( 0, "MOA_ECU", uniDevGuid );

                if ( xe_dev == null )
                    return false;

                if ( xe_dev.Attribute( "enable" ).Value.ToLower() == "true" )
                    return true;
            }
            catch ( Exception ex )
            {
                TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG( ex );
            }

            return false;
        }

        /// <summary>
        /// ��������� ��������� ������� ������������ ���� ������
        /// </summary>
        /// <param name="cms">����������� ����</param>
        /// <param name="compressnumdev">DevGUID ����������</param>
        private static void CustomizeContextMenuItems( ContextMenuStrip cms, int compressnumdev )
        {
            try
            {
                if (cms == null) return;

                bool state;
                bool.TryParse(PTKState.Iinstance.GetValueAsString(compressnumdev.ToString(CultureInfo.InvariantCulture), "�����"), out state);

                foreach (ToolStripItem tsi in cms.Items)
                {
                    /* NormalModeLibrary - ��������������� ���� ��� ������ */
                    if (state && tsi.Tag.ToString() == "NML")
                    {
                        tsi.Enabled = tsi.Visible = true;
                        continue;
                    }
                    
                    var content = (CommandContent<String>)tsi.Tag;
                    
                    if ( !state || !PTKState.Iinstance.IsAdapterExist( compressnumdev.ToString( CultureInfo.InvariantCulture ),content.Command ) )
                        tsi.Enabled = tsi.Visible = false;
                    else
                    {
                        bool cmd;
                        bool.TryParse( PTKState.Iinstance.GetValueAsString( compressnumdev.ToString( CultureInfo.InvariantCulture ), content.Command ), out cmd );

                        switch ( content.Parameter )
                        {
                            case "normal":
                                tsi.Enabled = cmd;
                                tsi.Visible = cmd;
                                break;
                            default:
                                tsi.Enabled = !cmd;
                                tsi.Visible = !cmd;
                                break;
                        }
                    }
                }
            }
            catch ( Exception ex )
            {
                if ( cms != null )
                    foreach ( ToolStripItem tsi in cms.Items )
                        tsi.Enabled = false;

                TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG( ex );
            }
        }
        /// <summary>
        /// �������� ������������ ���� ��� �������� �����������
        /// </summary>
        /// <param name="region">��������� ����������� �������</param>
        /// <param name="node">Xml �������� ����������</param>
        /// <param name="form">����� ��� �������� ���� ���������� ���������� �������</param>
        public static void CreateContextMenu( BaseRegion region, XElement node, Form form )
        {
            var menu = node.Element( "DescDev" );
            if ( menu == null ) return;

            menu = menu.Element( "ContextMenu" );
            if ( menu == null ) return;

            region.MenuStrip = new ContextMenuStrip { Tag = form };
            region.MenuStrip.Opening += ( sender, args ) =>
                                                {
                                                    var isp = ( (ContextMenuStrip)sender ).SourceControl as IBasePanel;
                                                    if ( isp == null ) return;
                                                    var idp = isp.Core as IDynamicParameters;
                                                    if ( idp != null && idp.Parameters != null )
                                                        CustomizeContextMenuItems( (ContextMenuStrip)sender, (int)idp.Parameters.DeviceGuid );
                                                };

            var xItems = menu.Elements( "MenuItem" );
            foreach ( var item in xItems )
            {
                var content = ContentHelper.CreateContextMenuContent( item );
                if (content == null)
                {
                    TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG( new Exception( "������ � ������ ������ ��� ������� ������������ ����" ) );
                    continue;
                }

                var menuItem = new ToolStripMenuItem( content.Context ) { Tag = content };
                menuItem.Click += ( sender, args ) =>
                                      {
                                          if ( IsUserActionBan( UserActionType.b00_Control_Switch, HMI_Settings.UserRight ) ||
                                              ( HMI_Settings.isRegPass && !CanAction() ) )
                                              return;

                                          var dlg = MessageBox.Show( "��������� �������?", "�������������",
                                                                     MessageBoxButtons.YesNo, MessageBoxIcon.Question );
                                          if ( dlg != DialogResult.Yes ) return;

                                          var tsi = (ToolStripMenuItem)sender;
                                          if ( tsi == null ) return;
                                          var cms = (ContextMenuStrip)tsi.Owner;
                                          if ( cms == null ) return;
                                          var idp = region as IDynamicParameters;
                                          if ( idp == null || idp.Parameters == null ) return;

                                          // ���������� ������ � ������ �������� ������������ ����� ���������� � ������ ��
                                          var numdevfc = (int)idp.Parameters.DeviceGuid;

                                          // ��������� �������� �� ��������� ����������� ������� ��������� ����������
                                          TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG(
                                              TraceEventType.Critical, 618, string.Format(
                                                  "��������� ������� ��� ����������: {0}", numdevfc ) );

                                          try
                                          {
                                              WriteEventToLog( (int)content.Code, numdevfc.ToString( CultureInfo.InvariantCulture ), true );

                                              /*ICommand cmd = */
                                              HMI_Settings.CONFIGURATION.ExecuteCommand( 0, content.Code, content.Command, new byte[] { }, (Form)cms.Tag );
                                          }
                                          catch
                                          {
                                              TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG(
                                                  TraceEventType.Error, numdevfc,
                                                  "������ � ������ ������ ��� ������� ������������ ����" );
                                              MessageBox.Show( "���������� ������� ��������", "������ �������� �������",
                                                               MessageBoxButtons.OK, MessageBoxIcon.Error );
                                          }
                                      };
                region.MenuStrip.Items.Add( menuItem );
            }
        }
        public static FormulaEvalNds GetConnectionEvalNds( string typeBlock, uint dsGuid, uint devGuid, bool oldLink = false )
        {
            if ( devGuid == 0) return null;

            if ( oldLink )
                return new FormulaEvalNds( HMI_Settings.CONFIGURATION,
                                           string.Format( "0({0}.{1}.0.60013.0)", devGuid / 256, devGuid % 256 ), "0",
                                           "��������� ���������", "" );

            if ( typeBlock.Contains( "USO" ) && typeBlock.Contains( "MTR" ) || typeBlock.Contains( "ITDS" ) )
                return new FormulaEvalNds( HMI_Settings.CONFIGURATION,
                                           string.Format( "0({0}.{1}.8)", dsGuid, devGuid ),
                                           "��������� ���������", "" );

            if ( typeBlock.Contains( "BRCN_100" ) )
                return new FormulaEvalNds( HMI_Settings.CONFIGURATION,
                                           string.Format( "0({0}.{1}.130726656)", dsGuid, devGuid ),
                                           "��������� ���������", "" );

            return new FormulaEvalNds(HMI_Settings.CONFIGURATION,
                                       string.Format("0(0.1000.{0})", devGuid),
                                       "��������� ���������", "");
        }

        /// <summary>
        /// �������� ����������� ���������� �� �������� Panel
        /// ��� �������� ������� � ������� ���������
        /// </summary>
        public static bool CreateDevImg4Panel( out IBasePanel ibp, ref List<ITag> taglist_global, uint unidevguid, Panel pnl )
        {
            ibp = null;

            try
            {
                DebugStatistics.WindowStatistics.AddStatistic( "������ ����� � ���������� ��������." );
                var xed = HMI_Settings.CONFIGURATION.GetDeviceXMLDescription( 0, "MOA_ECU", (int)unidevguid );
                
                var sp = new SinglePanel( new DynamicElement( ), pnl.Width, pnl.Height )
                    { Parent = pnl, Dock = DockStyle.None, Visible = true };
                sp.Select( );
                pnl.Controls.Add( sp );
                ibp = sp;
                DebugStatistics.WindowStatistics.AddStatistic( "������ ����� � ���������� �������� ���������." );

                var simulation = new Simulation( );
                simulation.Parse( sp.Core );

                DebugStatistics.WindowStatistics.AddStatistic( "������ ������������� �������� �����." );
                var dev = HMI_Settings.CONFIGURATION.GetLink2Device( 0, unidevguid );

                var idp = sp.Core as IDynamicParameters;
                if ( idp != null && idp.Parameters != null )
                {
                    idp.Parameters.DsGuid = 0;
                    idp.Parameters.DeviceGuid = unidevguid;
                    idp.Parameters.Cell = 1;
                    idp.Parameters.ExternalDescription = true;
                    idp.Parameters.Type = xed.Attribute( "TypeName" ).Value;
                    sp.Core.ToolTipMessage = string.IsNullOrEmpty( idp.Parameters.ToolTipMessage )
                                                 ? dev.XESsectionDescribe.Element( "DescDev" ).Element( "DescDev" ).Value
                                                 : idp.Parameters.ToolTipMessage;

                    var calc = sp.Core as CalculationRegion;
                    if ( calc != null )
                    {
                        using ( var build = new BuildFormula( ) )
                        {
                            var path = Environment.CurrentDirectory + Path.DirectorySeparatorChar +
                                       "Project" + Path.DirectorySeparatorChar + idp.Parameters.Type +
                                       Path.DirectorySeparatorChar + BuildFormula.FormulaBlock;

                            if ( WorkFile.CheckExistFile( path ) )
                            {
                                build.LoadFile( path );
                                build.ParceDataFromFile( );
                                calc.CalculationContext = build.GetData( );
                            }
                            else
                                DebugStatistics.WindowStatistics.AddStatistic( "�� ������ ����: " + path );
                        }

                        idp.AdjustmentTags( );

                        // �������� �����, ��������� � �������� ����������� �������� � �������� ������������
                        // ���� Device enable="False � ����� PrgDevCFG.cdp, �� ����������� ���� � ���� �� �����
                        if ( xed.Attribute( "enable" ).Value.ToLower() == "true" && calc.CalculationContext != null && !calc.IsDemonstration )
                        {
                            var taglistLocal = new List<ITag>( );
                            FormulaEvalNds ev;
                            foreach ( LibraryElements.Sources.SignalMatchRecord link in calc.CalculationContext.Context.GetTags( ) )
                            {
                                ev = new FormulaEvalNds( HMI_Settings.CONFIGURATION,
                                                    string.Format( "0({0})", link.Result ), "", "" );

                                if ( ev.LinkVariableNewDs != null )
                                {
                                    ev.OnChangeValFormTI += calc.LinkSetText;
                                    taglistLocal.Add( ev.LinkVariableNewDs );
                                }
                            }

                            // �������� ��������� ��������� ����������
                            if ( GetEnableStatusDev( (int)idp.Parameters.DeviceGuid ) )
                            {
                                ev = GetConnectionEvalNds( idp.Parameters.Type,
                                                           idp.Parameters.DsGuid,
                                                           idp.Parameters.DeviceGuid );

                                if ( ev != null && ev.LinkVariableNewDs != null )
                                {
                                    ev.OnChangeValFormTI += calc.LinkSetTextStatusDev;
                                    taglistLocal.Add( ev.LinkVariableNewDs );
                                }
                            }

                            // ������������� �� ���������� �����
                            HMI_Settings.CONFIGURATION.CfgReqEntry.SubscribeTags( taglistLocal );
                            // ��������� �������� � ����������� ������
                            taglist_global.AddRange( taglistLocal.ToArray( ) );
                        }
                    }
                }
                DebugStatistics.WindowStatistics.AddStatistic( "������������� �������� ����� ���������." );
            }
            catch ( Exception ex )
            {
                TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG( ex );
            }

            return true;
        }
        /// <summary>
        /// ������� ��������� �������� ���� ����� ��� ������������ ������� � ���� ������
        /// </summary>
        public static int GetTypeBlockData4ThisDev( TypeBlockData4Req reqtype, uint uniDev )
        {
            int rez = 0;

            try
            {
                // �������� ������ �� ����������
                IDevice dev = HMI_Settings.CONFIGURATION.GetLink2Device( 0, uniDev );
                string rezstr = string.Empty;

                switch ( reqtype )
                {
                    case TypeBlockData4Req.TypeBlockData4Req_Ustavki:
                        rezstr = dev.SpecificDeviceValue.GetValueByName( "TypeBlock4Ust" );
                        //rez = tcdd.TypeBlock4Ust;
                        break;
                    case TypeBlockData4Req.TypeBlockData4Req_Srabat:
                        rezstr = dev.SpecificDeviceValue.GetValueByName( "TypeBlock4Srabat" );
                        //rez = tcdd.TypeBlock4Srabat;
                        break;
                    case TypeBlockData4Req.TypeBlockData4Req_Osc:
                        rezstr = dev.SpecificDeviceValue.GetValueByName( "TypeBlock4Osc" );
                        //rez = tcdd.TypeBlock4Osc;
                        break;
                    case TypeBlockData4Req.TypeBlockData4Req_Diagramm:
                        rezstr = dev.SpecificDeviceValue.GetValueByName( "TypeBlock4Diagram" );
                        //rez = tcdd.TypeBlock4Diagram;
                        break;
                    case TypeBlockData4Req.TypeBlockData_LogEventBlock:
                        rezstr = dev.SpecificDeviceValue.GetValueByName( "TypeBlock4LogEvent" );
                        //rez = tcdd.TypeBlock4LogEvent;
                        break;
                    default:
                        MessageBox.Show( "�������� ���������������� ��� �����, �������� ������� = " + reqtype, "CommonUtils.cs", MessageBoxButtons.OK, MessageBoxIcon.Warning );
                        break;
                }

                string re = HMI_Settings.CONFIGURATION.GetTypeBlockArchivData( rezstr );

                if ( !int.TryParse( re, out rez ) )
                    rez = 0;
            }
            catch ( Exception ex )
            {
                TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG( ex );
            }

            return rez;
        }
        /// <summary>
        /// �������� XML
        /// </summary>
        /// <param Name="FILE_NAME"></param>
        public static void LoadXml( string FILE_NAME )
        {
            if ( !File.Exists( FILE_NAME ) )
            {
                TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG( TraceEventType.Critical, 436, "(436) CommonUtils.cs : LoadXml() : ���� �� ���������� : " + FILE_NAME );
                return;
            }

            StreamReader sr = new StreamReader( FILE_NAME, Encoding.GetEncoding( "windows-1251" ) );
            String input;

            input = sr.ReadToEnd();
            sr.Close();
            //textBox1.Text = input;
        }
        /// <summary>
        /// public static void DrawAsZebra( object item )
        /// ��������� ��������� ListView � DataGridView
        /// </summary>
        /// <param Name="item"></param>
        public static void DrawAsZebra( object item )
        {
            ListView lvItem;
            DataGridView dgvItem;
            bool changeColor = false;

            if ( ( lvItem = item as ListView ) != null )
            {
                for ( int j = 0; j < lvItem.Items.Count; j++ )
                {
                    ListViewItem lvi = lvItem.Items[j];
                    for ( int i = 0; i < lvi.SubItems.Count; i++ )
                        lvi.SubItems[i].BackColor = changeColor ? SystemColors.Control : SystemColors.Window;
                    changeColor = !changeColor;
                }
            }
            else if ( ( dgvItem = item as DataGridView ) != null )
            {
            }
        }
        public static uint CalculateCRC32( Stream stream )
        {
            const int BUFFERSIZE = 1024;
            const uint POLYNOMIAL = 0xEDB88320;

            uint rezult = 0xFFFFFFFF;
            uint CRC32;
            byte[] buffer = new byte[BUFFERSIZE];
            uint[] Crc32Table = new uint[256];
            // ������������� �������
            unchecked
            {
                for ( int i = 0; i < 256; i++ )
                {
                    CRC32 = (uint)i;
                    for ( int j = 8; j > 0; j-- )
                    {
                        if ( ( CRC32 & 1 ) == 1 )
                            CRC32 = ( CRC32 >> 1 ) ^ POLYNOMIAL;
                        else
                            CRC32 >>= 1;
                    }

                    Crc32Table[i] = CRC32;
                }
                // ������ ������
                stream.Position = 0;
                int count = stream.Read( buffer, 0, BUFFERSIZE );
                // ���������� CRC
                while ( count > 0 )
                {
                    for ( int i = 0; i < count; i++ )
                        rezult = ( ( rezult ) >> 8 ) ^ Crc32Table[( buffer[i] ) ^ ( ( rezult ) & 0x000000ff )];
                    count = stream.Read( buffer, 0, BUFFERSIZE );
                }
            }
            return ~rezult;
        }
        /// <summary>
        /// ��������� ����������� ������ ����
        /// </summary>
        /// <param Name="str"></param>
        /// <param Name="arl"></param>
        /// <returns></returns>
        public static bool IsMIDenied( string str, ArrayList arl )
        {
            //��������� ������� �������� ������ ����
            MemoryStream mstream = new MemoryStream();
            BinaryWriter msbw = new BinaryWriter( mstream );
            msbw.Write( str );

            uint rez = CommonUtils.CalculateCRC32( mstream );

            msbw.Close();
            mstream.Close();

            return arl.Contains( rez );
        }
        /// <summary>
        /// public static bool IsUserActionBan( UserActionType uat, string strRights )
        /// �������� �� ������������ ���������� �������� �������������
        /// </summary>
        /// <param Name="uat"></param>
        /// <param Name="strRights"></param>
        /// <returns>true - �������� ���������</returns>
        public static bool IsUserActionBan( UserActionType uat, string strRights )
        {
            //����������� ������ � ������� ������
            if ( strRights.Length != 32 )
            {
                MessageBox.Show( "CommonUtils: ������������ ������ �������� �������!" );
                return true;	// ��������� ���������� ������������ ��������
            }
            UInt32 t = Convert.ToUInt32( strRights, 2 );
            byte[] at = BitConverter.GetBytes( t );
            BitArray ba = new BitArray( at );
            if ( ba.Get( (int)uat ) )
            {
                MessageBox.Show( "� ��� ��� ���� �� ���������� ����� ��������" );
                return true;
            }
            else
                return false;
        }
        public static StringBuilder ReversStr( string needStrRev )
        {
            if ( needStrRev == null )
                return null;

            StringBuilder sb = new StringBuilder();
            for ( int i = needStrRev.Length; i > 0; i-- )
                sb.Append( needStrRev[i - 1] );
            return sb;
        }
        /// <summary>
        ///     WriteEventToLog - ������������� ������� ������������ � ��������� ���� � ��
        /// </summary>
        /// <param Name="codEvent"> ��� ������� ��� ������������� (��� �� ������� UserAction � ��)</param>
        /// <param Name="sComment"> ���. ����������� � �������</param>
        /// <param Name="wrToBDLog"> ������� ������������� ������������� � ��</param>
        public static void WriteEventToLog( int codEvent, string sComment, bool wrToBDLog )
        {
            SqlCommand cmd;
            DataSet aDS;
            SqlDataAdapter aSDA;

            //System.Diagnostics.Trace.TraceInformation( DateTime.Now.ToString() + " : MainForm.cs ( ������ 2321): WriteEventToLog : "
            //   + " codEvent = " + codEvent.ToString()
            //   + " sComment = " + sComment
            //   + " wrToBDLog = " + wrToBDLog.ToString() );

            TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG(TraceEventType.Error, 946, string.Format("{0} : {1} : {2} : ������ � ������ �������� ������������ : codEvent = {3}, sComment = {4}, wrToBDLog = {5}.", DateTime.Now.ToString(), @"X:\Projects\01_HMIWinFormsClient\CommonUtils\CommonUtils.cs", "WriteEventToLog()", codEvent.ToString(), sComment, wrToBDLog.ToString()));
						
            int tmpi;

            if ( !wrToBDLog )	// ���� ������ � �� �� �����, �� �����
            {
                return;
            }

            // ���������� ���������� � ������� � �� (������� UserLog)
            // ��������� ����� ���������� � ���������� ������ �� ����� *.config
            //string cnStr = ConfigurationManager.ConnectionStrings["SqlProviderPTK"].ConnectionString;
            SqlConnection asqlconnect = new SqlConnection( HMI_MT_Settings.HMI_Settings.ProviderPtkSql );
            try
            {
                asqlconnect.Open();
            }
            catch ( Exception ex )
            {
                System.Diagnostics.Trace.TraceInformation( DateTime.Now.ToString() + " : MainForm.cs ( ������ 2366): WriteEventToLog : " + ex.Message );
                asqlconnect.Close();
                return;
            }

            string ipFromPrjCfg = String.Empty;
            string macPrjCfg = String.Empty;

            if ( codEvent < 11000000 )
            {
                // ������������ ������ ��� ������ �������� ���������
                cmd = new SqlCommand( "User~Log", asqlconnect );
                cmd.CommandType = CommandType.StoredProcedure;

                // ������� ���������
                /*@UserId int,
                 * @Action int,
                 * @Time datetime,
                 * @ip real,
                 * @ObjectId int,
                 * @ArmId int
                 */
                // 1. UserID
                SqlParameter pUserId = new SqlParameter();
                pUserId.ParameterName = "@UserId";
                pUserId.SqlDbType = SqlDbType.Int;
                pUserId.Value = HMI_MT_Settings.HMI_Settings.UserID;
                pUserId.Direction = ParameterDirection.Input;
                cmd.Parameters.Add( pUserId );
                // 2. id ��������
                SqlParameter pidEvent = new SqlParameter();
                pidEvent.ParameterName = "@Action";
                pidEvent.SqlDbType = SqlDbType.Int;
                pidEvent.Value = codEvent;
                pidEvent.Direction = ParameterDirection.Input;
                cmd.Parameters.Add( pidEvent );
                // 3. ����� ��������
                SqlParameter ptimeEvent = new SqlParameter();
                ptimeEvent.ParameterName = "@Time";
                ptimeEvent.SqlDbType = SqlDbType.DateTime;
                ptimeEvent.Value = HMI_MT_Settings.HMI_Settings.CurrentDateTime;
                ptimeEvent.Direction = ParameterDirection.Input;
                cmd.Parameters.Add( ptimeEvent );
                // 4. ip ����� ����������
                //---

                #region ���������� mac � ip-������ ���������� ����������� � ������������� �������� ����� � ��
                ipFromPrjCfg = HMI_MT_Settings.HMI_Settings.XDoc4PathToPrjFile.Element( "Project" ).Element( "IpForMACResolving" ).Attribute( "address" ).Value;
                macPrjCfg = HMI_MT_Settings.HMI_Settings.XDoc4PathToPrjFile.Element( "Project" ).Element( "IpForMACResolving" ).Attribute( "mac" ).Value;
                //macPrjCfg = "00-53-45-00-00-00";
                #endregion

                //---
                SqlParameter pIpEvent = new SqlParameter();
                pIpEvent.ParameterName = "@ip";
                pIpEvent.SqlDbType = SqlDbType.BigInt;
                pIpEvent.Value = Convert.ToInt64( BitConverter.ToInt32( IPAddress.Parse( ipFromPrjCfg ).GetAddressBytes(), 0 ) );// ipFromPrjCfg;// (float)localIP;
                pIpEvent.Direction = ParameterDirection.Input;
                cmd.Parameters.Add( pIpEvent );
                //5. @ObjectId
                tmpi = 0;

                if ( !int.TryParse( sComment, out tmpi ) )
                    tmpi = 0;

                SqlParameter pObjectId = new SqlParameter();
                pObjectId.ParameterName = "@ObjectId";
                pObjectId.SqlDbType = SqlDbType.Int;
                pObjectId.Value = tmpi;
                pObjectId.Direction = ParameterDirection.Input;
                cmd.Parameters.Add( pObjectId );

                SqlParameter pArmMAC = new SqlParameter();
                pArmMAC.ParameterName = "@MAC";
                pArmMAC.SqlDbType = SqlDbType.VarChar;
                pArmMAC.Value = macPrjCfg;//
                pArmMAC.Direction = ParameterDirection.Input;
                cmd.Parameters.Add( pArmMAC );

                // ���������� DataSet
                aDS = new DataSet( "ptk" );
                aSDA = new SqlDataAdapter();
                aSDA.SelectCommand = cmd;
            }
            else
            {
                // ������������ ������ ��� ������ �������� ���������
                cmd = new SqlCommand( "AddEventStandart", asqlconnect );
                cmd.CommandType = CommandType.StoredProcedure;

                // ������� ���������
                /*
                   @type as bit, -- ���� 0 - �� ������� ��� ������� �������, ���� 1 - �� ������� ��� ������� ������
                   @eventID as int, -- ������������� �������
                   @TimeFC as datetime, -- ����� �� - ������������ ������ ���� ��� ������� ��� ������� �������
                   @LocalTime as datetime, -- ��������� ����� ������
                   @ObjID as int -- ���� ��� ������� ��� ������� �����, �� ����� ������������ �����, ���� ���, �� ������������� �������
               */
                // 1. @type
                SqlParameter pType = new SqlParameter();
                pType.ParameterName = "@type";
                pType.SqlDbType = SqlDbType.Bit;
                pType.Value = 0;	// ������ � ������ �������
                pType.Direction = ParameterDirection.Input;
                cmd.Parameters.Add( pType );
                // 2. @eventID - id ��������
                SqlParameter pEventId = new SqlParameter();
                pEventId.ParameterName = "@eventID";
                pEventId.SqlDbType = SqlDbType.Int;
                pEventId.Value = codEvent;
                pEventId.Direction = ParameterDirection.Input;
                cmd.Parameters.Add( pEventId );
                // 3. @TimeFC - ����� ��
                SqlParameter pTimeFC = new SqlParameter();
                pTimeFC.ParameterName = "@TimeFC";
                pTimeFC.SqlDbType = SqlDbType.DateTime;
                pTimeFC.Value = DateTime.Now;
                pTimeFC.Direction = ParameterDirection.Input;
                cmd.Parameters.Add( pTimeFC );
                // 4. @LocalTime
                SqlParameter pLocalTime = new SqlParameter();
                pLocalTime.ParameterName = "@LocalTime";
                pLocalTime.SqlDbType = SqlDbType.DateTime;
                pLocalTime.Value = DateTime.Now;
                pLocalTime.Direction = ParameterDirection.Input;
                cmd.Parameters.Add( pLocalTime );
                //5. @ObjectId
                tmpi = 0;

                if ( !int.TryParse( sComment, out tmpi ) )
                    tmpi = 0;

                SqlParameter pObjID = new SqlParameter();
                pObjID.ParameterName = "@ObjID";
                pObjID.SqlDbType = SqlDbType.Int;
                pObjID.Value = tmpi;
                pObjID.Direction = ParameterDirection.Input;
                cmd.Parameters.Add( pObjID );

                // ���������� DataSet
                aDS = new DataSet( "ptk" );
                aSDA = new SqlDataAdapter();
                aSDA.SelectCommand = cmd;
            }

            try
            {
                aSDA.Fill( aDS );
            }
            catch ( Exception ex )
            {
                System.Diagnostics.Trace.TraceInformation( DateTime.Now.ToString() + " : MainForm.cs ( ������ 2537): WriteEventToLog : " + ex.Message );
                if ( codEvent == 1 )
                {
                    MessageBox.Show( "������� ����� � ����������� ip = " + ipFromPrjCfg + "; mac = " + macPrjCfg + " - ���������� � ������ ������������ �������.\n" + ex.Message, "������ �����", MessageBoxButtons.OK, MessageBoxIcon.Error );
                    System.Diagnostics.Trace.TraceInformation( DateTime.Now.ToString() + " : MainForm.cs ( ������ 2570): WriteEventToLog : ������� ����� � ����������� ���������� � ������ ������������ �������" );
                    Process.GetCurrentProcess().Kill();    // ����� �� ����������
                }
            }
            aDS.Clear();
            aDS.Dispose();
            aSDA.Dispose();
            asqlconnect.Close();
        }
        /// <summary>
        /// public bool CanAction()
        /// ��� ��������� ���������� ������ ������������ - ����� ����������� ������� ������������ �������� 
        /// �������� ������������� ������ �������� ������������. ������ ������� ������� ������� � ��������� ������ 
        ///  � ��������� ���������� ��������� �� ��������� �������� ����������� ���������� �� ���������� ������� ��������
        /// </summary>
        /// <param Name="UserName">��� ������������</param>
        /// <param Name="UserID">������������� ������������</param>
        /// <returns></returns>
        public static bool CanAction()
        {
            // ��������� ������
            dlgCanPassword dcp = new dlgCanPassword(HMI_MT_Settings.HMI_Settings.UserName, HMI_MT_Settings.HMI_Settings.UserID);
            DialogResult dr = dcp.ShowDialog();
            if (dr == DialogResult.Abort)
                return false;
            else
                return true;
        }
        /// <summary>
        /// ������������ �������� ������
        /// �������� - ����� �������.
        /// ���� ������ ����� ������� � �������� ���������
        /// ������� ������ �������
        /// </summary>
        /// <param name="listTags"></param>
        /// <returns></returns>
        public static byte[] GetUstConfMemXPacket(List<ITag> listTags)
        {
            MemoryStream mswr = new MemoryStream();
            BinaryWriter bw = new BinaryWriter(mswr);

			try
			{
                foreach( ITag tag in listTags )
                {
                    bw.Write(tag.TagGUID);
                    byte bTypeTagPriorityView = Convert.ToByte(tag.Device.TypeTagPriorityView);             
                    bw.Write(bTypeTagPriorityView); 
                    UInt16 lenMemX = Convert.ToUInt16(tag.ValueAsMemX.Length);
                    bw.Write(lenMemX);
                    bw.Write(tag.ValueAsMemX);
                    //Single con = 35.00F;
                    //byte[] conx = BitConverter.GetBytes(con);
                    //con = 30.00F;
                    //conx = BitConverter.GetBytes(con);
                    //conx = new byte[]{0x41,0xf0,0x01,0x05};
                    //con = BitConverter.ToSingle(conx,0);
                    //conx = new byte[] { 0x05, 0x01, 0xf0, 0x41 };
                    //con = BitConverter.ToSingle(conx, 0);
                }
			}
			catch(Exception ex)
			{
				TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG(ex );
			}
            
            mswr.Position = 0;

            return mswr.ToArray();
        }
    }
}
