using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using NormalModeLibrary.Sources;

namespace NormalModeLibrary.ViewModel
{
    public class UserViewModel : BaseCollectionViewModel
    {
        User user;

        internal UserViewModel( User user )
        {
            this.user = user;
            Core = user;

            foreach ( Configuration cfg in user.Collection )
                Collection.Add( new ConfigurationViewModel( cfg ) );
        }
        internal override ViewModelBase Copy()
        {
            User copyUser = (User)user.Copy();
            return new UserViewModel( copyUser );
        }

        public String Login { get { return user.Login; } }
        public TimeMode ActiveTime { get { return user.ActiveTime; } }
    }
}
