using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NormalModeLibrary.ViewModel
{
    public interface IOutOfRangeHandler : Sources.IOutOfRangeHandler
    {
        bool IsOutOfRangeEvent { get; }
        bool IsOutOfRange { get; }
    }
}
