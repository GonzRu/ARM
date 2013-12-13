using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using NormalModeLibrary.Sources;

namespace NormalModeLibrary.ViewModel
{
    public abstract class BaseSignalViewModel : ViewModelBase
    {
        private BaseSignal signal;
        protected InterfaceLibrary.ITag tag;

        protected BaseSignalViewModel( BaseSignal signal )
        {
            this.signal = signal;
            Core = signal;
        }
        protected virtual void UpDateProperties()
        {
            OnPropertyChanged( "Value" );
        }
        internal void SetTag( InterfaceLibrary.ITag tag )
        {
            this.tag = tag;
        }
        internal bool Subscribe()
        {
            if ( tag == null )
                return IsSubscribe = false;

            if ( IsSubscribe )
                return false;

            tag.OnChangeVar += new InterfaceLibrary.ChVarNewDs( tag_OnChangeVar );
            HMI_MT_Settings.HMI_Settings.CONFIGURATION.CfgReqEntry.SubscribeTag( tag );
            return IsSubscribe = true;
        }
        internal bool UnSubscribe()
        {
            if ( tag == null )
                return IsSubscribe = false;

            if ( !IsSubscribe )
                return false;

            tag.OnChangeVar -= tag_OnChangeVar;
            HMI_MT_Settings.HMI_Settings.CONFIGURATION.CfgReqEntry.UnSubscribeTag( tag );
            IsSubscribe = false;
            return true;
        }
        private void tag_OnChangeVar( Tuple<string, byte[], DateTime, InterfaceLibrary.VarQualityNewDs> var )
        {
            bool qual = ( var.Item4 == InterfaceLibrary.VarQualityNewDs.vqGood ) ? true : false;

            if ( signal.SetValue( var.Item1, qual ) )
                UpDateProperties();
        }

        public String Caption { get { return signal.Caption; } }
        public String Commentary { get { return signal.Commentary; } }
        public String Dim { get { return signal.Dim; } }
        public UInt32 Guid { get { return signal.Guid; } }
        public Object Value { get { return signal.Value; } }
        internal Boolean IsSubscribe { get; private set; }
        internal Boolean IsChecked { get; set; }
        internal Boolean IsSupported { get { return ( BaseSignal.CheckSignalType( this.tag.Type ) ); } }

        internal static BaseSignalViewModel GetSignalViewModel( BaseSignal signal )
        {
            if ( signal is AnalogSignal )
                return new ViewModel.AnalogViewModel( signal );
            else if ( signal is DigitalSignal )
                return new ViewModel.DigitalViewModel( signal );
            else return null;
        }
    }
}
