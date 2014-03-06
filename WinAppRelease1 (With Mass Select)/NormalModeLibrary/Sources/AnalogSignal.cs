using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace NormalModeLibrary.Sources
{
    public class AnalogSignal : BaseSignal, IOutOfRangeHandler
    {
        private bool _isOutOfRange = false;

        public event EventHandler OutOfRangeEvent;
        double value = 0;
        
        internal AnalogSignal()
        {
            Range = new HysteresisRange();
        }
        public override void ParseXml( System.Xml.Linq.XElement xnode )
        {
            base.ParseXml( xnode );

            Range.RangeMinValue = double.Parse( xnode.Attribute( "rangeMinValue" ).Value );
            Range.RangeMaxValue = double.Parse( xnode.Attribute( "rangeMaxValue" ).Value );
            Range.RangeMinHysteresis = double.Parse( xnode.Attribute( "rangeMinHysteresis" ).Value );
            Range.RangeMaxHysteresis = double.Parse( xnode.Attribute( "rangeMaxHysteresis" ).Value );
        }
        public override XElement CreateXml()
        {
            XElement node = base.CreateXml();
            node.Add( new XAttribute( "rangeMinValue", Range.RangeMinValue ) );
            node.Add( new XAttribute( "rangeMaxValue", Range.RangeMaxValue ) );
            node.Add( new XAttribute( "rangeMinHysteresis", Range.RangeMinHysteresis ) );
            node.Add( new XAttribute( "rangeMaxHysteresis", Range.RangeMaxHysteresis ) );
            return node;
        }
        public override bool SetValue( object value, bool quality )
        {
            if ( !quality )
                return false;

            double tmp = 0;
            bool res = double.TryParse( value.ToString(), out tmp );
            if ( res )
            {
                if ( tmp != this.value )
                {
                    this.value = tmp;

                    bool isOutORrangeNow = Range.OutOfRange(this.value);
                    if (_isOutOfRange != isOutORrangeNow)
                    {
                        if (OutOfRangeEvent != null)
                            OutOfRangeEvent(this, new OutOfRangeEventArgs(isOutORrangeNow));

                        _isOutOfRange = isOutORrangeNow;
                    }
                }
                else res = false;
            }
            return res;
        }
        internal override string GetTreeNodeText()
        {
            return string.Format( "Аналоговый сигнал: {0}", Caption );
        }
        internal override BaseObject Copy()
        {
            AnalogSignal copy = new AnalogSignal();
            copy.Caption = Caption;
            copy.Commentary = Commentary;
            copy.Dim = Dim;
            copy.dsGuid = dsGuid;
            copy.Guid = Guid;
            copy.objectGuid = objectGuid;
            copy.Range.RangeMinValue = Range.RangeMinValue;
            copy.Range.RangeMaxValue = Range.RangeMaxValue;
            copy.Range.RangeMinHysteresis = Range.RangeMinHysteresis;
            copy.Range.RangeMaxHysteresis = Range.RangeMaxHysteresis;
            copy.type = type;
            copy.value = value;
            copy.FontSize = FontSize;
            return copy;
        }

        internal HysteresisRange Range { get; private set; }
        public override Object Value { get { return this.value; } }

    }
}
