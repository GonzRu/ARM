using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using NormalModeLibrary.Sources;

namespace NormalModeLibrary.ViewModel
{
    public class ConfigurationViewModel : BaseCollectionViewModel
    {
        Configuration configuration;

        internal ConfigurationViewModel(Configuration configuration)
        {
            this.configuration = configuration;
            Core = configuration;

            foreach ( Panel panel in configuration.Collection )
                Collection.Add( new PanelViewModel( panel ) );
        }
        internal override ViewModelBase Copy()
        {
            Configuration config = (Configuration)configuration.Copy();
            return new ConfigurationViewModel( config );
        }

        public TimeMode ActiveTime { get { return configuration.ActiveTime; } }
        public Places Place { get { return configuration.Place; } }
        public Boolean IsActive { get { return configuration.IsActive; } }
    }
}
