using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Text;

namespace HMI_MT_Settings
{
    public delegate void NewError( string strerror );
    public delegate void PingPongDelegate( bool state );

    public class CallbackHandler : IDSRouterCallback
    {
        public event NewError OnNewError;
        public event PingPongDelegate OnPingPongEvent;

        public void NewErrorEvent( string codeDataTimeEvent )
        {
            try
            {
                var oneEvent = OnNewError;
                //передаем выше, если есть куда
                if ( oneEvent != null )
                    oneEvent( codeDataTimeEvent );
            }
            catch ( Exception ex )
            {
                TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG( ex );
            }
        }
        public IAsyncResult BeginNewErrorEvent( string codeDataTimeEvent, AsyncCallback callback, object asyncState )
        {
            IAsyncResult rez = null;
            try
            {
                throw new NotImplementedException( );
            }
            catch ( Exception ex )
            {
                TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG( ex );
            }
            return rez;
        }
        public void EndNewErrorEvent( IAsyncResult result )
        {
            try
            {
                throw new NotImplementedException( );
            }
            catch ( Exception ex )
            {
                TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG( ex );
            }
        }
        
        public void Pong( )
        {
            try
            {
                var ppEvent = OnPingPongEvent;
                if ( ppEvent != null ) ppEvent( true );

                TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG( TraceEventType.Error, 84,
                                                                      string.Format( "{0} : {1} : {2} : Pong.",
                                                                                     DateTime.Now.ToString( CultureInfo.InvariantCulture ),
                                                                                     @"X:\Projects\00_DataServer\DataServerWPF\HMI_MT_Settings\CallbackHandler.cs",
                                                                                     "Pong()()" ) );
            }
            catch ( Exception ex )
            {
                TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG( TraceEventType.Error, 89,
                                                                      string.Format( "{0} : {1} : {2} : ОШИБКА: {3}",
                                                                                     DateTime.Now.ToString( CultureInfo.InvariantCulture ),
                                                                                     @"X:\Projects\00_DataServer\DataServerWPF\HMI_MT_Settings\CallbackHandler.cs",
                                                                                     "Pong()()", ex.Message ) );
            }
        }
        public IAsyncResult BeginPong( AsyncCallback callback, object asyncState )
        {
            IAsyncResult rez = null;
            try
            {
                throw new NotImplementedException( );
            }
            catch ( Exception ex )
            {
                TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG( ex );
            }
            return rez;
        }
        public void EndPong( IAsyncResult result )
        {
            try
            {
                throw new NotImplementedException( );
            }
            catch ( Exception ex )
            {
                TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG( ex );
            }
        }

        public void NotifyCMDExecuted( byte[] data )
        {
            try
            {
                var brrez = new BinaryReader( new MemoryStream( data ) );

                // тип пакета
                /*byte reqtype = */
                brrez.ReadByte( );
                // код ошибки
                var codeError = brrez.ReadByte( );

                /*
                * идентификатор корреляции:
                * 0 - не нужно следить за тем пришел 
                *		ответ на запрос или нет
                */
                /*id_correlation_out = */
                brrez.ReadUInt16( );
                // длина результирующего массива со значениями тегов
                /*lenfullrez = */
                brrez.ReadUInt16( );

                var uniDsGuid = brrez.ReadUInt16( ) /* = 0xffff*/;
                var locObjectGuid = brrez.ReadUInt32( ) /* = 0xffffffff*/;
                var lencmdname = brrez.ReadUInt16( );
                var cmdname = Encoding.UTF8.GetString( brrez.ReadBytes( lencmdname ) );
                var lenparams = brrez.ReadUInt16( );
                //byte[] rezcmdparams = new byte[] { };

                if ( lenparams > 0 )
                    /*rezcmdparams = */ brrez.ReadBytes( lenparams );

                var key = string.Format( "{0}.{1}.{2}", uniDsGuid, locObjectGuid, cmdname );

                HMI_Settings.CONFIGURATION.ActiveCommands.RemoveCmd( key, codeError );
            }
            catch ( Exception ex )
            {
                TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG( ex );
            }
        }
        public IAsyncResult BeginNotifyCMDExecuted( byte[] cmdarray, AsyncCallback callback, object asyncState )
        {
            IAsyncResult rez = null;
            try
            {
                throw new NotImplementedException( );
            }
            catch ( Exception ex )
            {
                TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG( ex );
            }
            return rez;
        }
        public void EndNotifyCMDExecuted( IAsyncResult result )
        {
            try
            {
                throw new NotImplementedException( );
            }
            catch ( Exception ex )
            {
                TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG( ex );
            }
        }
        
        public void NotifyChangedTags( System.Collections.Generic.Dictionary<string, DSRouter.DSTagValue> uu ) { }
        public IAsyncResult BeginNotifyChangedTags(
            System.Collections.Generic.Dictionary<string, DSRouter.DSTagValue> rr, AsyncCallback tt, object yy )
        {
            IAsyncResult rez = null;
            try
            {
                throw new NotImplementedException( );
            }
            catch ( Exception ex )
            {
                TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG( ex );
            }
            return rez;
        }
        public void EndNotifyChangedTags( IAsyncResult gg ) { }
    }
}