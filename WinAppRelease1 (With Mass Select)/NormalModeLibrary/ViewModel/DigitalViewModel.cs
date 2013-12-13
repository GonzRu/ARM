using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Globalization;

using NormalModeLibrary.Sources;

namespace NormalModeLibrary.ViewModel
{
    public class DigitalViewModel : BaseSignalViewModel
    {
        internal DigitalViewModel( BaseSignal signal ) : base( signal ) { }
        internal override ViewModelBase Copy()
        {
            BaseSignal signal = (BaseSignal)Core.Copy();
            DigitalViewModel model = new DigitalViewModel( signal );
            model.tag = tag;
            return model;
        }
    }
}
