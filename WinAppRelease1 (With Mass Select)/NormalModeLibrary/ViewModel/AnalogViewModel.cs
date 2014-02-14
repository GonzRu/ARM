using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using NormalModeLibrary.Sources;

namespace NormalModeLibrary.ViewModel
{
    public class AnalogViewModel : BaseSignalViewModel, IOutOfRangeHandler
    {
        internal AnalogViewModel( BaseSignal signal ) : base( signal ) { }
        internal override ViewModelBase Copy()
        {
            BaseSignal signal = (BaseSignal)Core.Copy();
            AnalogViewModel model = new AnalogViewModel( signal );
            model.tag = tag;
            return model;
        }
        protected override void UpDateProperties()
        {
            base.UpDateProperties();
            OnPropertyChanged( "IsOutOfRange" );
        }
        public event EventHandler OutOfRangeEvent
        {
            add
            {
                ( (Sources.IOutOfRangeHandler)Core ).OutOfRangeEvent += value;
                IsOutOfRangeEvent = true;
            }
            remove
            {
                ( (Sources.IOutOfRangeHandler)Core ).OutOfRangeEvent -= value;
                IsOutOfRangeEvent = false;
            }
        }
        public bool IsOutOfRangeEvent { get; private set; }

        public bool IsOutOfRange
        {
            get { return ((AnalogSignal) Core).Range.OutOfRange((double) base.Value); }
        }
    }
}
