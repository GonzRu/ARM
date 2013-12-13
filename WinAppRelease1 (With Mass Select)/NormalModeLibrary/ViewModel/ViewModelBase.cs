using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

using NormalModeLibrary.Sources;

namespace NormalModeLibrary.ViewModel
{
    /// <summary>
    /// Базовая модель представления
    /// </summary>
    public abstract class ViewModelBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged( string propertyName )
        {
            if ( PropertyChanged != null )
                PropertyChanged( this, new PropertyChangedEventArgs( propertyName ) );
        }
        internal abstract ViewModelBase Copy();
        
        public BaseObject Core { get; protected set; }
    }
}
