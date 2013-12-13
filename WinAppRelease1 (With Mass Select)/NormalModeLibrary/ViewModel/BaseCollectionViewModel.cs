using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;

namespace NormalModeLibrary.ViewModel
{
    public abstract class BaseCollectionViewModel : ViewModelBase
    {
        protected BaseCollectionViewModel()
        {
            Collection = new ObservableCollection<ViewModelBase>();
        }
        public ObservableCollection<ViewModelBase> Collection { get; private set; }
    }
}
